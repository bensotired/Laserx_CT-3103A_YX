using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SolveWare_IO
{
    public partial class Form_IOManager : Form, ITesterAppUI, IAccessPermissionLevel
    {
        Color YES_COLOR = Color.LimeGreen;
        Color NO_COLOR = Color.DarkGreen;
        //long _lastEditCURDItem_ID = 0;
        ITesterCoreInteration _core;
        ITesterAppPluginInteration _appInteration;
        IOManager _myIOManager;
        const int NAME_COL_INDEX = 0;
        const int CARD_COL_INDEX = 1;
        const int Slave_COL_INDEX = 2;
        const int BITS_COL_INDEX = 3;
        const int LOGIC_COL_INDEX = 4;
        const int ACTIVE_COL_INDEX = 5;
        const int ISEXT_OP_IO_COL_DISPLAY_INDEX = 6;
        const int ISEXT_IP_IO_COL_DISPLAY_INDEX = 5;
        const int ISEXT_OP_IO_COL_SETTING_INDEX = 5;
        const int ISEXT_IP_IO_COL_SETTING_INDEX = 5;
        //Form_IO_DisPlay _IODisPlayForm = new Form_IO_DisPlay();
        object lockRefresh = new object();
        System.Windows.Forms.Timer timer1 = new Timer();

        public Form_IOManager()
        {
            InitializeComponent();
        }
        private void Form_MotionManager_Load(object sender, EventArgs e)
        {
            UpdateEditorContext_Input();
            UpdateEditorContext_Output();
            System.Threading.Thread.Sleep(100);

            //RefreshDgvDisplay();
            this.RefreshOnce();

            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer_Tick);
            this.timer1.Enabled = true;

            this.dgv_OutputIODisplay.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgv_OutputIODisplay_CurrentCellDirtyStateChanged);
        }

        public AccessPermissionLevel APL
        {
            get { return AccessPermissionLevel.None; }
        }



        public void ConnectToAppInteration(ITesterAppPluginInteration app)
        {
            _appInteration = app;
            _myIOManager = (IOManager)app;
        }
        public void ConnectToCore(ITesterCoreInteration core)
        {
            _core = core as ITesterCoreInteration;
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core.SendOutFormCoreToGUIEvent += ReceiveMessageFromCore;
        }
        public void DisconnectFromCore(ITesterCoreInteration core)
        {
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
        }

        private void ReceiveMessageFromCore(IMessage message)
        {
            //      throw new NotImplementedException();
        }
        private void 浮动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _appInteration.PopUI();
        }
        private void 还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            _appInteration.DockUI();
        }
        private void Form_MotionManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            _appInteration.DockUI();
        }




        private void btn_addInputIOSetting_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                Form_New_IP_IOStarter nas = new Form_New_IP_IOStarter();
                var ret = nas.ShowDialog();
                if (ret == DialogResult.OK)
                {
                    IOSetting singleIO = new IOSetting();
                    //可以保证ID绝对不一样
                    //singleIO.ID = IdentityGenerator.IG.GetIdentity();
                    singleIO.Name = nas.New_INP_IO_Name;
                    singleIO.CardNo = nas.New_INP_IO_CardNo;
                    singleIO.SlaveNo = nas.New_INP_IO_Slave;
                    singleIO.Bit = nas.New_INP_IO_BitNo;
                    singleIO.ActiveLogic = nas.New_INP_IO_Logic;
                    singleIO.IOType = IOType.INPUT;
                    singleIO.IsExtendIO = nas.New_INP_IO_IsExtenalIO;
                    //在保存的过程中，需要检查现在的setting是否包含有这IO的属性
                    if (_myIOManager.Config.IOGeneralSetting.isExistSameIO(singleIO))
                    {
                        MessageBox.Show($"插入的输入IO点已存在!!!");
                        return;
                    }
                    //将IO点的配置放到config的setting list中
                    _myIOManager.Config.IOGeneralSetting.AddSingleItem(singleIO, ref errMsg);

                    //保存到config
                    try
                    {
                        SaveInputIOSetting();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"保存失败!!!\nException:{ex}");
                        return;
                    }
                    UpdateEditorContext_Input();
                    _myIOManager.ReinstallController();

                }
                else
                {
                    //取消了增加操作
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"新增输入IO参数失败:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        void SaveInputIOSetting()
        {
            DataGridView dataGridView = dgv_InputIOEditor;
            int IOSettingsCount = _myIOManager.Config.IOGeneralSetting.GetSettingsDuetoIOType(IOType.INPUT).Count();
            int dataGridViewRowsCount = dataGridView.Rows.Count;
            bool Same = true;
            if (dataGridViewRowsCount == IOSettingsCount)
            {
                for (int dgvRowIndex = 0; dgvRowIndex < dataGridViewRowsCount; dgvRowIndex++)
                {
                    string name = dataGridView.Rows[dgvRowIndex].Cells[0].Value.ToString();
                    short slaveNo = Convert.ToInt16(dataGridView.Rows[dgvRowIndex].Cells[1].Value.ToString());
                    short bitNO = Convert.ToInt16(dataGridView.Rows[dgvRowIndex].Cells[2].Value.ToString());
                    short logic = Convert.ToInt16(dataGridView.Rows[dgvRowIndex].Cells[3].Value.ToString());
                    bool isExtIO = Convert.ToBoolean(dataGridView.Rows[dgvRowIndex].Cells[4].Value);
                    if (!_myIOManager.Config.IOGeneralSetting.isExistSameIO(name, 1, slaveNo, bitNO, logic, IOType.INPUT, isExtIO))
                    {
                        Same = false;
                        break;
                    }
                }
            }
            else
            {
                Same = false;
            }
            if (Same)
            {
                MessageBox.Show($"已经有相同的IO配置!");
                return;
            }

            try
            {
                _myIOManager.SaveConfig();
                MessageBox.Show($"IO参数保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"IO参数保存失败:[{ex}]!");
            }
        }
        private void UpdateEditorContext_Input()
        {
            try
            {
                dgv_InputIOEditor.Rows.Clear();
                List<IOSetting> iOSettings = _myIOManager.Config.IOGeneralSetting.GetSettingsDuetoIOType(IOType.INPUT);
                for (int i = 0; i < iOSettings.Count; i++)
                {
                    int rowIndex = dgv_InputIOEditor.Rows.Add();
                    dgv_InputIOEditor.Rows[rowIndex].Cells[NAME_COL_INDEX].Value = iOSettings[i].Name;
                    dgv_InputIOEditor.Rows[rowIndex].Cells[CARD_COL_INDEX].Value = iOSettings[i].CardNo;
                    dgv_InputIOEditor.Rows[rowIndex].Cells[Slave_COL_INDEX].Value = iOSettings[i].SlaveNo;
                    dgv_InputIOEditor.Rows[rowIndex].Cells[BITS_COL_INDEX].Value = iOSettings[i].Bit;
                    dgv_InputIOEditor.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Value = iOSettings[i].ActiveLogic;
                    //((DataGridViewCheckBoxCell)dgv_InputIOEditor.Rows[rowIndex].Cells[ISEXT_OP_IO_COL_SETTING_INDEX]).Value = iOSettings[i].IsExtendIO;
                    ((DataGridViewCheckBoxCell)dgv_InputIOEditor.Rows[rowIndex].Cells[ISEXT_IP_IO_COL_SETTING_INDEX]).Value = iOSettings[i].IsExtendIO;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示失败!!!\nException:{ex}");
                return;
            }
        }
        private void btn_addOutputIOSetting_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                Form_New_OP_IOStarter nas = new Form_New_OP_IOStarter();
                var ret = nas.ShowDialog();
                if (ret == DialogResult.OK)
                {
                    IOSetting singleIO = new IOSetting();
                    singleIO.Name = nas.New_OP_IO_Name;
                    //singleIO.CardNo = 1;
                    singleIO.CardNo = nas.New_OP_IO_CardNo;
                    singleIO.SlaveNo = nas.New_OP_IO_Slave;
                    singleIO.Bit = nas.New_OP_IO_No;
                    singleIO.ActiveLogic = nas.New_OP_IO_Logic;
                    singleIO.IOType = IOType.OUTPUT;
                    singleIO.IsExtendIO = nas.New_OP_IO_IsExtenalIO;
                    if (_myIOManager.Config.IOGeneralSetting.isExistSameIO(singleIO))
                    {
                        MessageBox.Show($"插入的输出IO点已存在!!!");
                        return;
                    }
                    _myIOManager.Config.IOGeneralSetting.AddSingleItem(singleIO, ref errMsg);
                    try
                    {
                        SaveOutputIOSetting();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"保存失败!!!\nException:{ex}");
                        return;
                    }
                    UpdateEditorContext_Output();
                    _myIOManager.ReinstallController();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新增输入IO参数失败:[{ex.Message}-{ex.StackTrace}]!");
            }
        }
        private void UpdateEditorContext_Output()
        {
            try
            {
                dgv_OutputIOEditor.Rows.Clear();
                List<IOSetting> iOSettings = _myIOManager.Config.IOGeneralSetting.GetSettingsDuetoIOType(IOType.OUTPUT);
                for (int i = 0; i < iOSettings.Count; i++)
                {
                    int rowIndex = dgv_OutputIOEditor.Rows.Add();
                    dgv_OutputIOEditor.Rows[rowIndex].Cells[NAME_COL_INDEX].Value = iOSettings[i].Name;
                    dgv_OutputIOEditor.Rows[rowIndex].Cells[CARD_COL_INDEX].Value = iOSettings[i].CardNo;
                    dgv_OutputIOEditor.Rows[rowIndex].Cells[Slave_COL_INDEX].Value = iOSettings[i].SlaveNo;
                    dgv_OutputIOEditor.Rows[rowIndex].Cells[BITS_COL_INDEX].Value = iOSettings[i].Bit;
                    dgv_OutputIOEditor.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Value = iOSettings[i].ActiveLogic;
                    ((DataGridViewCheckBoxCell)dgv_OutputIOEditor.Rows[rowIndex].Cells[ISEXT_OP_IO_COL_SETTING_INDEX]).Value = iOSettings[i].IsExtendIO;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示失败!!!\nException:{ex}");
                return;
            }
        }



        void SaveOutputIOSetting()
        {
            DataGridView dataGridView = dgv_OutputIOEditor;
            int IOSettingsCount = _myIOManager.Config.IOGeneralSetting.GetSettingsDuetoIOType(IOType.OUTPUT).Count();
            int dataGridViewRowsCount = dataGridView.Rows.Count;
            bool Same = true;
            if (dataGridViewRowsCount == IOSettingsCount)
            {
                for (int dgvRowIndex = 0; dgvRowIndex < dataGridViewRowsCount; dgvRowIndex++)
                {
                    string name = dataGridView.Rows[dgvRowIndex].Cells[0].Value.ToString();
                    short slaveNo = Convert.ToInt16(dataGridView.Rows[dgvRowIndex].Cells[1].Value.ToString());
                    short bitNO = Convert.ToInt16(dataGridView.Rows[dgvRowIndex].Cells[2].Value.ToString());
                    short logic = Convert.ToInt16(dataGridView.Rows[dgvRowIndex].Cells[3].Value.ToString());
                    bool isExtIO = Convert.ToBoolean(dataGridView.Rows[dgvRowIndex].Cells[4].Value);
                    if (!_myIOManager.Config.IOGeneralSetting.isExistSameIO(name, 1, slaveNo, bitNO, logic, IOType.OUTPUT, isExtIO))
                    {
                        Same = false;
                        break;
                    }
                }
            }
            else
            {
                Same = false;
            }

            if (Same)
            {
                MessageBox.Show($"已经有相同的输出IO配置!");
                return;
            }

            try
            {
                _myIOManager.SaveConfig();
                MessageBox.Show($"输出IO参数保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"输出IO参数保存失败:[{ex}]!");
            }
        }

        private void RefreshDgvDisplay()
        {

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            lock (lockRefresh)
            {
                this.RefreshOnce();
            }
            timer1.Enabled = true;
        }

        private void dgv_OutputIODisplay_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            try
            {
                lock (lockRefresh)
                {

                    if (dgv_OutputIODisplay.IsCurrentCellDirty)
                    {
                        dgv_OutputIODisplay.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
                    else
                    {
                        if (dgv_OutputIODisplay.Rows.Count > 0)
                        {
                            int columnIndex = dgv_OutputIODisplay.CurrentCell.ColumnIndex;
                            if (columnIndex == ACTIVE_COL_INDEX)
                            {

                                string name = dgv_OutputIODisplay.CurrentRow.Cells[NAME_COL_INDEX].Value.ToString();
                                string _selectValue = ((DataGridViewCheckBoxCell)dgv_OutputIODisplay.CurrentRow.Cells[columnIndex]).EditedFormattedValue.ToString();
                                if (_selectValue == "True")
                                {
                                    _myIOManager.GetIO(name).TurnOn(true);
                                }
                                else
                                {
                                    _myIOManager.GetIO(name).TurnOn(false);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                this.timer1.Enabled = true;
            }
        }



        private void dgv_OutputIODisplay_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                var dgv = sender as DataGridView;
                if (e.Exception is System.ArgumentException)
                {
                    if (dgv[e.ColumnIndex, e.RowIndex] is DataGridViewComboBoxCell)
                    {
                        var value = dgv[e.ColumnIndex, e.RowIndex].Value;
                        var valueType = (dgv[e.ColumnIndex, e.RowIndex].Tag as PropertyInfo).PropertyType;

                        if (valueType.IsEnum)
                        {
                            dgv[e.ColumnIndex, e.RowIndex].Value = Converter.ConvertObjectTo(value, valueType).ToString();
                        }
                        else
                        {
                            dgv[e.ColumnIndex, e.RowIndex].Value = Converter.ConvertObjectTo(value, valueType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void RefreshOnce()
        {
            try
            {
                dgv_InputIODisplay.Rows.Clear();
                dgv_OutputIODisplay.Rows.Clear();
                System.Threading.Thread.Sleep(50);
                List<IOBase> IOBaseList = _myIOManager.GetIOBaseCollection();

                //得到单个IO的
                for (int i = 0; i < IOBaseList.Count; i++)
                {
                    switch (IOBaseList[i].Interation.IOType)
                    {
                        case IOType.INPUT:
                            {
                                int rowIndex = dgv_InputIODisplay.Rows.Add();
                                dgv_InputIODisplay.Rows[rowIndex].Cells[NAME_COL_INDEX].Value = IOBaseList[i].IOSetting.Name;
                                dgv_InputIODisplay.Rows[rowIndex].Cells[CARD_COL_INDEX].Value = IOBaseList[i].IOSetting.CardNo;
                                dgv_InputIODisplay.Rows[rowIndex].Cells[Slave_COL_INDEX].Value = IOBaseList[i].IOSetting.SlaveNo;
                                dgv_InputIODisplay.Rows[rowIndex].Cells[BITS_COL_INDEX].Value = IOBaseList[i].IOSetting.Bit;
                                dgv_InputIODisplay.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Value = IOBaseList[i].Interation.IsActive;
                                dgv_InputIODisplay.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Style.BackColor = IOBaseList[i].Interation.IsActive ? YES_COLOR : NO_COLOR;//0为关闭
                                ((DataGridViewCheckBoxCell)dgv_InputIODisplay.Rows[rowIndex].Cells[ISEXT_IP_IO_COL_DISPLAY_INDEX]).Value = IOBaseList[i].IOSetting.IsExtendIO;
                            }
                            break;
                        case IOType.OUTPUT:
                            {
                                int rowIndex = dgv_OutputIODisplay.Rows.Add();
                                dgv_OutputIODisplay.Rows[rowIndex].Cells[NAME_COL_INDEX].Value = IOBaseList[i].IOSetting.Name;
                                dgv_OutputIODisplay.Rows[rowIndex].Cells[CARD_COL_INDEX].Value = IOBaseList[i].IOSetting.CardNo;
                                dgv_OutputIODisplay.Rows[rowIndex].Cells[Slave_COL_INDEX].Value = IOBaseList[i].IOSetting.SlaveNo;
                                dgv_OutputIODisplay.Rows[rowIndex].Cells[BITS_COL_INDEX].Value = IOBaseList[i].IOSetting.Bit;
                                dgv_OutputIODisplay.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Value = IOBaseList[i].Interation.IsActive;
                                dgv_OutputIODisplay.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Style.BackColor = IOBaseList[i].Interation.IsActive ? YES_COLOR : NO_COLOR;//0为关闭
                                ((DataGridViewCheckBoxCell)dgv_OutputIODisplay.Rows[rowIndex].Cells[ACTIVE_COL_INDEX]).Value = IOBaseList[i].Interation.IsActive;
                                ((DataGridViewCheckBoxCell)dgv_OutputIODisplay.Rows[rowIndex].Cells[ISEXT_OP_IO_COL_DISPLAY_INDEX]).Value = IOBaseList[i].IOSetting.IsExtendIO;

                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示失败!!!\nException:{ex}");
                return;
            }
        }



        private void tsmi_delete_opIo_Click(object sender, EventArgs e)
        {
            try
            {
                const int destRowCount = 1;
                if (this.dgv_OutputIOEditor.SelectedCells?.Count == destRowCount)
                {
                    int rowIndex = this.dgv_OutputIOEditor.SelectedCells[0].RowIndex;

                    var Name = dgv_OutputIOEditor.Rows[rowIndex].Cells[NAME_COL_INDEX].Value.ToString();
                    var CardNo = Convert.ToInt16(dgv_OutputIOEditor.Rows[rowIndex].Cells[CARD_COL_INDEX].Value);
                    var SlaveNo = Convert.ToInt16(dgv_OutputIOEditor.Rows[rowIndex].Cells[Slave_COL_INDEX].Value);
                    var Bit = Convert.ToInt16(dgv_OutputIOEditor.Rows[rowIndex].Cells[BITS_COL_INDEX].Value);
                    var ActiveLogic = Convert.ToInt16(dgv_OutputIOEditor.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Value);
                    var IsExtIO = Convert.ToBoolean(dgv_OutputIOEditor.Rows[rowIndex].Cells[ISEXT_OP_IO_COL_SETTING_INDEX].Value);

                    if (MessageBox.Show($"确定删除输出IO点[{Name}]?", "删除输出IO", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (this._myIOManager.DeleteIoSettingItem(Name, CardNo, SlaveNo, Bit, ActiveLogic, IOType.OUTPUT, IsExtIO) == true)
                        {
                            this.SaveInputIOSetting();
                            this.UpdateEditorContext_Output();
                            this._myIOManager.ReinstallController();
                            MessageBox.Show($"输出IO点[{Name}]已删除!");
                        }
                        else
                        {
                            MessageBox.Show($"输出IO点[{Name}]删除失败!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除输出IO点错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void tsmi_delete_ipIo_Click(object sender, EventArgs e)
        {
            try
            {
                const int destRowCount = 1;


                if (this.dgv_InputIOEditor.SelectedCells?.Count == destRowCount)
                {
                    int rowIndex = this.dgv_InputIOEditor.SelectedCells[0].RowIndex;

                    var Name = dgv_InputIOEditor.Rows[rowIndex].Cells[NAME_COL_INDEX].Value.ToString();
                    var CardNo = Convert.ToInt16(dgv_InputIOEditor.Rows[rowIndex].Cells[CARD_COL_INDEX].Value);
                    var SlaveNo = Convert.ToInt16(dgv_InputIOEditor.Rows[rowIndex].Cells[Slave_COL_INDEX].Value);
                    var Bit = Convert.ToInt16(dgv_InputIOEditor.Rows[rowIndex].Cells[BITS_COL_INDEX].Value);
                    var ActiveLogic = Convert.ToInt16(dgv_InputIOEditor.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Value);
                    var IsExtIO = Convert.ToBoolean(dgv_InputIOEditor.Rows[rowIndex].Cells[ISEXT_IP_IO_COL_SETTING_INDEX].Value);

                    if (MessageBox.Show($"确定删除输入IO点[{Name}]?", "删除输入IO", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (this._myIOManager.DeleteIoSettingItem(Name, CardNo, SlaveNo, Bit, ActiveLogic, IOType.INPUT, IsExtIO) == true)
                        {
                            this.SaveInputIOSetting();
                            this.UpdateEditorContext_Input();
                            this._myIOManager.ReinstallController();
                            MessageBox.Show($"输入IO点[{Name}]已删除!");
                        }
                        else
                        {
                            MessageBox.Show($"输入IO点[{Name}]删除失败!");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除输入IO点错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }
    }
}