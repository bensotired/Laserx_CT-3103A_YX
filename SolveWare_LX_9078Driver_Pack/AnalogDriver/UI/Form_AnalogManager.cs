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

namespace SolveWare_Analog
{
    public partial class Form_AnalogManager : Form, ITesterAppUI, IAccessPermissionLevel
    {
        private Color YES_COLOR = Color.LimeGreen;
        private Color NO_COLOR = Color.DarkGreen;

        //long _lastEditCURDItem_ID = 0;
        private ITesterCoreInteration _core;

        private ITesterAppPluginInteration _appInteration;
        private AnalogManager _myAnalogManager;
        private const int NAME_COL_INDEX = 0;
        private const int CARD_COL_INDEX = 1;
        private const int Slave_COL_INDEX = 2;
        private const int BITS_COL_INDEX = 3;
        private const int LOGIC_COL_INDEX = 4;
        private const int ACTIVE_COL_INDEX = 5;
        private const int ISEXT_OP_IO_COL_DISPLAY_INDEX = 6;
        private const int ISEXT_IP_IO_COL_DISPLAY_INDEX = 5;
        private const int ISEXT_OP_IO_COL_SETTING_INDEX = 5;
        private const int ISEXT_IP_IO_COL_SETTING_INDEX = 5;

        //Form_IO_DisPlay _IODisPlayForm = new Form_IO_DisPlay();
        private object lockRefresh = new object();

        private System.Windows.Forms.Timer timer1 = new Timer();

        public Form_AnalogManager()
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

            this.dgv_OutputAnalogDisplay.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgv_OutputAnalogDisplay_CurrentCellDirtyStateChanged);
        }

        public AccessPermissionLevel APL
        {
            get { return AccessPermissionLevel.None; }
        }

        public void ConnectToAppInteration(ITesterAppPluginInteration app)
        {
            _appInteration = app;
            _myAnalogManager = (AnalogManager)app;
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
                Form_New_IP_AnalogStarter nas = new Form_New_IP_AnalogStarter();
                var ret = nas.ShowDialog();
                if (ret == DialogResult.OK)
                {
                    AnalogSetting singleAnalog = new AnalogSetting();
                    //可以保证ID绝对不一样
                    //singleIO.ID = IdentityGenerator.IG.GetIdentity();
                    singleAnalog.Name = nas.New_INP_Analog_Name;
                    singleAnalog.CardNo = nas.New_INP_Analog_CardNo;
                    singleAnalog.SlaveNo = nas.New_INP_Analog_Slave;
                    singleAnalog.Bit = nas.New_INP_Analog_BitNo;
                    singleAnalog.AnalogType = AnalogType.ADC;
                    singleAnalog.IsExtendAnalog = nas.New_INP_Analog_IsExtenalAnalog;
                    //在保存的过程中，需要检查现在的setting是否包含有这IO的属性
                    if (_myAnalogManager.Config.AnalogGeneralSetting.isExistSameAnalog(singleAnalog))
                    {
                        MessageBox.Show($"插入的输入Analog点已存在!!!");
                        return;
                    }
                    //将IO点的配置放到config的setting list中
                    _myAnalogManager.Config.AnalogGeneralSetting.AddSingleItem(singleAnalog, ref errMsg);

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
                    _myAnalogManager.ReinstallController();
                }
                else
                {
                    //取消了增加操作
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新增输入Analog参数失败:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void SaveInputIOSetting()
        {
            DataGridView dataGridView = dgv_InputAnalogEditor;
            int IOSettingsCount = _myAnalogManager.Config.AnalogGeneralSetting.GetSettingsDuetoAnalogType(AnalogType.ADC).Count();
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
                    if (!_myAnalogManager.Config.AnalogGeneralSetting.isExistSameAnalog(name, 1, slaveNo, bitNO, logic, AnalogType.ADC, isExtIO))
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
                MessageBox.Show($"已经有相同的Analog配置!");
                return;
            }

            try
            {
                _myAnalogManager.SaveConfig();
                MessageBox.Show($"Analog参数保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Analog参数保存失败:[{ex}]!");
            }
        }

        private void UpdateEditorContext_Input()
        {
            try
            {
                dgv_InputAnalogEditor.Rows.Clear();
                List<AnalogSetting> iOSettings = _myAnalogManager.Config.AnalogGeneralSetting.GetSettingsDuetoAnalogType(AnalogType.ADC);
                for (int i = 0; i < iOSettings.Count; i++)
                {
                    int rowIndex = dgv_InputAnalogEditor.Rows.Add();
                    dgv_InputAnalogEditor.Rows[rowIndex].Cells[NAME_COL_INDEX].Value = iOSettings[i].Name;
                    dgv_InputAnalogEditor.Rows[rowIndex].Cells[CARD_COL_INDEX].Value = iOSettings[i].CardNo;
                    dgv_InputAnalogEditor.Rows[rowIndex].Cells[Slave_COL_INDEX].Value = iOSettings[i].SlaveNo;
                    dgv_InputAnalogEditor.Rows[rowIndex].Cells[BITS_COL_INDEX].Value = iOSettings[i].Bit;
                    dgv_InputAnalogEditor.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Value = iOSettings[i].ActiveLogic;
                    //((DataGridViewCheckBoxCell)dgv_InputIOEditor.Rows[rowIndex].Cells[ISEXT_OP_IO_COL_SETTING_INDEX]).Value = iOSettings[i].IsExtendIO;
                    ((DataGridViewCheckBoxCell)dgv_InputAnalogEditor.Rows[rowIndex].Cells[ISEXT_IP_IO_COL_SETTING_INDEX]).Value = iOSettings[i].IsExtendAnalog;
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
                Form_New_OP_AnalogStarter nas = new Form_New_OP_AnalogStarter();
                var ret = nas.ShowDialog();
                if (ret == DialogResult.OK)
                {
                    AnalogSetting singleIO = new AnalogSetting();
                    singleIO.Name = nas.New_OP_Analog_Name;
                    //singleIO.CardNo = 1;
                    singleIO.CardNo = nas.New_OP_Analog_CardNo;
                    singleIO.SlaveNo = nas.New_OP_Analog_Slave;
                    singleIO.Bit = nas.New_OP_Analog_No;
                    singleIO.ActiveLogic = nas.New_OP_Analog_Logic;
                    singleIO.AnalogType = AnalogType.DAC;
                    singleIO.IsExtendAnalog = nas.New_OP_Analog_IsExtenalAnalog;
                    if (_myAnalogManager.Config.AnalogGeneralSetting.isExistSameAnalog(singleIO))
                    {
                        MessageBox.Show($"插入的输出Analog点已存在!!!");
                        return;
                    }
                    _myAnalogManager.Config.AnalogGeneralSetting.AddSingleItem(singleIO, ref errMsg);
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
                    _myAnalogManager.ReinstallController();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新增输入Analog参数失败:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void UpdateEditorContext_Output()
        {
            try
            {
                dgv_OutputAnalogEditor.Rows.Clear();
                List<AnalogSetting> iOSettings = _myAnalogManager.Config.AnalogGeneralSetting.GetSettingsDuetoAnalogType(AnalogType.DAC);
                for (int i = 0; i < iOSettings.Count; i++)
                {
                    int rowIndex = dgv_OutputAnalogEditor.Rows.Add();
                    dgv_OutputAnalogEditor.Rows[rowIndex].Cells[NAME_COL_INDEX].Value = iOSettings[i].Name;
                    dgv_OutputAnalogEditor.Rows[rowIndex].Cells[CARD_COL_INDEX].Value = iOSettings[i].CardNo;
                    dgv_OutputAnalogEditor.Rows[rowIndex].Cells[Slave_COL_INDEX].Value = iOSettings[i].SlaveNo;
                    dgv_OutputAnalogEditor.Rows[rowIndex].Cells[BITS_COL_INDEX].Value = iOSettings[i].Bit;
                    dgv_OutputAnalogEditor.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Value = iOSettings[i].ActiveLogic;
                    ((DataGridViewCheckBoxCell)dgv_OutputAnalogEditor.Rows[rowIndex].Cells[ISEXT_OP_IO_COL_SETTING_INDEX]).Value = iOSettings[i].IsExtendAnalog;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"显示失败!!!\nException:{ex}");
                return;
            }
        }

        private void SaveOutputIOSetting()
        {
            DataGridView dataGridView = dgv_OutputAnalogEditor;
            int IOSettingsCount = _myAnalogManager.Config.AnalogGeneralSetting.GetSettingsDuetoAnalogType(AnalogType.DAC).Count();
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
                    if (!_myAnalogManager.Config.AnalogGeneralSetting.isExistSameAnalog(name, 1, slaveNo, bitNO, logic, AnalogType.DAC, isExtIO))
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
                MessageBox.Show($"已经有相同的输出Analog配置!");
                return;
            }

            try
            {
                _myAnalogManager.SaveConfig();
                MessageBox.Show($"输出Analog参数保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"输出Analog参数保存失败:[{ex}]!");
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

        private void dgv_OutputAnalogDisplay_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            try
            {
                lock (lockRefresh)
                {
                    if (dgv_OutputAnalogDisplay.IsCurrentCellDirty)
                    {
                        dgv_OutputAnalogDisplay.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
                    else
                    {
                        if (dgv_OutputAnalogDisplay.Rows.Count > 0)
                        {
                            int columnIndex = dgv_OutputAnalogDisplay.CurrentCell.ColumnIndex;
                            if (columnIndex == ACTIVE_COL_INDEX)
                            {
                                string name = dgv_OutputAnalogDisplay.CurrentRow.Cells[NAME_COL_INDEX].Value.ToString();
                                string _selectValue = ((DataGridViewCheckBoxCell)dgv_OutputAnalogDisplay.CurrentRow.Cells[columnIndex]).EditedFormattedValue.ToString();

                                double dvalue = 0;
                                if (double.TryParse(_selectValue, out dvalue))
                                {
                                    _myAnalogManager.GetIO(name).OutputValue(dvalue);
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

        private void dgv_OutputAnalogDisplay_DataError(object sender, DataGridViewDataErrorEventArgs e)
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
                dgv_InputAnalogDisplay.Rows.Clear();
                dgv_OutputAnalogDisplay.Rows.Clear();
                System.Threading.Thread.Sleep(50);
                List<AnalogBase> IOBaseList = _myAnalogManager.GetIOBaseCollection();

                //得到单个IO的
                for (int i = 0; i < IOBaseList.Count; i++)
                {
                    switch (IOBaseList[i].Interation.AnalogType)
                    {
                        case AnalogType.ADC:
                            {
                                int rowIndex = dgv_InputAnalogDisplay.Rows.Add();
                                dgv_InputAnalogDisplay.Rows[rowIndex].Cells[NAME_COL_INDEX].Value = IOBaseList[i].AnalogSetting.Name;
                                dgv_InputAnalogDisplay.Rows[rowIndex].Cells[CARD_COL_INDEX].Value = IOBaseList[i].AnalogSetting.CardNo;
                                dgv_InputAnalogDisplay.Rows[rowIndex].Cells[Slave_COL_INDEX].Value = IOBaseList[i].AnalogSetting.SlaveNo;
                                dgv_InputAnalogDisplay.Rows[rowIndex].Cells[BITS_COL_INDEX].Value = IOBaseList[i].AnalogSetting.Bit;
                                dgv_InputAnalogDisplay.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Value = IOBaseList[i].Interation.Value;
                                //dgv_InputIODisplay.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Style.BackColor = IOBaseList[i].Interation.Value ? YES_COLOR : NO_COLOR;//0为关闭
                                ((DataGridViewCheckBoxCell)dgv_InputAnalogDisplay.Rows[rowIndex].Cells[ISEXT_IP_IO_COL_DISPLAY_INDEX]).Value = IOBaseList[i].AnalogSetting.IsExtendAnalog;
                            }
                            break;

                        case AnalogType.DAC:
                            {
                                int rowIndex = dgv_OutputAnalogDisplay.Rows.Add();
                                dgv_OutputAnalogDisplay.Rows[rowIndex].Cells[NAME_COL_INDEX].Value = IOBaseList[i].AnalogSetting.Name;
                                dgv_OutputAnalogDisplay.Rows[rowIndex].Cells[CARD_COL_INDEX].Value = IOBaseList[i].AnalogSetting.CardNo;
                                dgv_OutputAnalogDisplay.Rows[rowIndex].Cells[Slave_COL_INDEX].Value = IOBaseList[i].AnalogSetting.SlaveNo;
                                dgv_OutputAnalogDisplay.Rows[rowIndex].Cells[BITS_COL_INDEX].Value = IOBaseList[i].AnalogSetting.Bit;
                                dgv_OutputAnalogDisplay.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Value = IOBaseList[i].Interation.Value;
                                //dgv_OutputAnalogDisplay.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Style.BackColor = IOBaseList[i].Interation.Value ? YES_COLOR : NO_COLOR;//0为关闭
                                dgv_OutputAnalogDisplay.Rows[rowIndex].Cells[ACTIVE_COL_INDEX].Value = IOBaseList[i].Interation.Value;
                                ((DataGridViewCheckBoxCell)dgv_OutputAnalogDisplay.Rows[rowIndex].Cells[ISEXT_OP_IO_COL_DISPLAY_INDEX]).Value = IOBaseList[i].AnalogSetting.IsExtendAnalog;
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
                if (this.dgv_OutputAnalogEditor.SelectedCells?.Count == destRowCount)
                {
                    int rowIndex = this.dgv_OutputAnalogEditor.SelectedCells[0].RowIndex;

                    var Name = dgv_OutputAnalogEditor.Rows[rowIndex].Cells[NAME_COL_INDEX].Value.ToString();
                    var CardNo = Convert.ToInt16(dgv_OutputAnalogEditor.Rows[rowIndex].Cells[CARD_COL_INDEX].Value);
                    var SlaveNo = Convert.ToInt16(dgv_OutputAnalogEditor.Rows[rowIndex].Cells[Slave_COL_INDEX].Value);
                    var Bit = Convert.ToInt16(dgv_OutputAnalogEditor.Rows[rowIndex].Cells[BITS_COL_INDEX].Value);
                    var ActiveLogic = Convert.ToInt16(dgv_OutputAnalogEditor.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Value);
                    var IsExtIO = Convert.ToBoolean(dgv_OutputAnalogEditor.Rows[rowIndex].Cells[ISEXT_OP_IO_COL_SETTING_INDEX].Value);

                    if (MessageBox.Show($"确定删除输出Analog点[{Name}]?", "删除输出Analog", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (this._myAnalogManager.DeleteIoSettingItem(Name, CardNo, SlaveNo, Bit, ActiveLogic, AnalogType.DAC, IsExtIO) == true)
                        {
                            this.SaveInputIOSetting();
                            this.UpdateEditorContext_Output();
                            this._myAnalogManager.ReinstallController();
                            MessageBox.Show($"输出Analog点[{Name}]已删除!");
                        }
                        else
                        {
                            MessageBox.Show($"输出Analog点[{Name}]删除失败!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除输出Analog点错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void tsmi_delete_ipIo_Click(object sender, EventArgs e)
        {
            try
            {
                const int destRowCount = 1;

                if (this.dgv_InputAnalogEditor.SelectedCells?.Count == destRowCount)
                {
                    int rowIndex = this.dgv_InputAnalogEditor.SelectedCells[0].RowIndex;

                    var Name = dgv_InputAnalogEditor.Rows[rowIndex].Cells[NAME_COL_INDEX].Value.ToString();
                    var CardNo = Convert.ToInt16(dgv_InputAnalogEditor.Rows[rowIndex].Cells[CARD_COL_INDEX].Value);
                    var SlaveNo = Convert.ToInt16(dgv_InputAnalogEditor.Rows[rowIndex].Cells[Slave_COL_INDEX].Value);
                    var Bit = Convert.ToInt16(dgv_InputAnalogEditor.Rows[rowIndex].Cells[BITS_COL_INDEX].Value);
                    var ActiveLogic = Convert.ToInt16(dgv_InputAnalogEditor.Rows[rowIndex].Cells[LOGIC_COL_INDEX].Value);
                    var IsExtIO = Convert.ToBoolean(dgv_InputAnalogEditor.Rows[rowIndex].Cells[ISEXT_IP_IO_COL_SETTING_INDEX].Value);

                    if (MessageBox.Show($"确定删除输入Analog点[{Name}]?", "删除输入Analog", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (this._myAnalogManager.DeleteIoSettingItem(Name, CardNo, SlaveNo, Bit, ActiveLogic, AnalogType.ADC, IsExtIO) == true)
                        {
                            this.SaveInputIOSetting();
                            this.UpdateEditorContext_Input();
                            this._myAnalogManager.ReinstallController();
                            MessageBox.Show($"输入Analog点[{Name}]已删除!");
                        }
                        else
                        {
                            MessageBox.Show($"输入Analog点[{Name}]删除失败!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除输入Analog点错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void bt_SetSenseCurrentRange_Click(object sender, EventArgs e)
        {
            try
            {
                int Card = 0;
                int Bit = 0;
                double Current = 0.1;
                if (int.TryParse(this.textBox_SetCurr_Card.Text, out Card) == false)
                {
                    MessageBox.Show("卡号格式错误!");
                    return;
                }
                if (int.TryParse(this.textBox_SetCurr_Bit.Text, out Bit) == false)
                {
                    MessageBox.Show("端口号格式错误!");
                    return;
                }
                if (double.TryParse(this.textBox_SetCurrent_mA.Text, out Current) == false)
                {
                    MessageBox.Show("电流值格式错误!");
                    return;
                }
                SolveWare_BurnInInstruments.LaserX_9078_Utilities.P9078_SetSenseCurrentRange_mA(Card, Bit, Current);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SetSenseCurrentRange 错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void bt_GetCurrentRange_Click(object sender, EventArgs e)
        {
            try
            {
                int Card = 0;
                int Bit = 0;
                if (int.TryParse(this.textBox_SetCurr_Card.Text, out Card) == false)
                {
                    MessageBox.Show("卡号格式错误!");
                    return;
                }
                if (int.TryParse(this.textBox_SetCurr_Bit.Text, out Bit) == false)
                {
                    MessageBox.Show("端口号格式错误!");
                    return;
                }
                this.lab_CurrentRange.Text= SolveWare_BurnInInstruments.LaserX_9078_Utilities.P9078_GetSenseCurrentRange_mA(Card, Bit).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"GetSenseCurrentRange 错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }
    }
}