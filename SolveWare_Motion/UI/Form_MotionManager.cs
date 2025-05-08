using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_Motion
{
    public partial class Form_MotionManager : Form, ITesterAppUI, IAccessPermissionLevel
    {
        long _lastEditCURDItem_ID = 0;
        ITesterCoreInteration _core;
        ITesterAppPluginInteration _appInteration;
        MotionManager _myMotionManager;
        Form_SingleAxisControl _singleAxisCtrlForm;
        Form_AxesRealtimeInfo _axesInfoForm;
        const int INFO_COL_INDEX = 0;
        const int KEY_COL_INDEX = 1;
        const int VAL_COL_INDEX = 2;

        List<MotorAxisBase> AxisCollection = new List<MotorAxisBase>();//所有轴      表1显示
        List<MotorAxisBase> AxisSelected = new List<MotorAxisBase>();//被选中的轴  表2显示
        List<string> PositionNameList = new List<string>();//所有已经保存的点的名字  表3显示
        //MotionAction _motorAction = new MotionAction();
        MotionActionV2 _motorActionNewV2 = new MotionActionV2();
        CancellationTokenSource _tokenSource = new CancellationTokenSource();
        AxesPosition lastSelectedAxesPosition = null;
        public Form_MotionManager()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            _singleAxisCtrlForm = new Form_SingleAxisControl();
        }

        private void Form_MotionManager_Load(object sender, EventArgs e)
        {
            DockSingleAxisControlForm();

            //表格编序号
            //1 3 4
            //2

            Update_AxisCollectionTable_Table_1();
            Clear_AxisSelected_Table_2();
            Update_PositionCollectionTable_Table_3();

            UpdateAxesSelector(); //移动到后面试试

        }

        private void DockSingleAxisControlForm()
        {
            _singleAxisCtrlForm.Hide();
            _singleAxisCtrlForm.TopLevel = false;
            _singleAxisCtrlForm.FormBorderStyle = FormBorderStyle.None;
            _singleAxisCtrlForm.Dock = DockStyle.Fill;
            _singleAxisCtrlForm.Show();

            this.gb_singleAxisPlace.Controls.Clear();
            this.gb_singleAxisPlace.Controls.Add(_singleAxisCtrlForm);
            _singleAxisCtrlForm.Show();
        }

        public void AssignAxisToControlForm(MotorAxisBase axis)
        {
            _singleAxisCtrlForm.AssignAxis(axis);
        }

        public void UninstallAxisToControlForm()
        {
            _singleAxisCtrlForm.UninstallAxis();
            gb_singleAxisPlace.Text = $"单轴试运行[未加载]";
        }

        public AccessPermissionLevel APL
        {
            get { return AccessPermissionLevel.None; }
        }

        private void UpdateAxesSelector(string forceAxisName = "")
        {
            var orgSelectItem = cb_AxisToEdit.SelectedItem;
            //重新加载所有的轴的名称，轴的名称来源_myMotionManager.Config这个变量
            cb_AxisToEdit.Items.Clear();
            //获取轴的名字
            var axisNames = _myMotionManager.Config.MotorGeneralSetting.GetDataListByPropName<string>("Name");
            cb_AxisToEdit.Items.AddRange(axisNames.ToArray());
            if (axisNames.Count <= 0)
            {
                ClearListDGV_V2(this.dgv_MotorModes);
                ClearListDGV_V2(this.dgv_MotorSpeed);
                ClearListDGV_V2(this.dgv_MotorTable);
                return;
            }

            //如果有轴就选择轴 没有就INdex=0
            else
            {
                if (string.IsNullOrEmpty(forceAxisName))//这个是相当于已经添加单轴了但是没有保存吗？
                {
                    if (orgSelectItem != null)
                    {
                        if (axisNames.Contains(orgSelectItem.ToString()))
                        {
                            cb_AxisToEdit.SelectedItem = orgSelectItem;
                        }
                        else
                        {
                            cb_AxisToEdit.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        cb_AxisToEdit.SelectedIndex = 0;
                    }
                }
                else
                {
                    if (axisNames.Contains(forceAxisName))
                    {
                        cb_AxisToEdit.SelectedItem = forceAxisName;
                    }
                    else
                    {
                        cb_AxisToEdit.SelectedIndex = 0;
                    }
                }
            }
        }

        public void ConnectToAppInteration(ITesterAppPluginInteration app)
        {
            _appInteration = app;
            _myMotionManager = (MotionManager)app;
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

        private void Dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
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

        private void ClearListDGV_V2(DataGridView dgv)
        {
            dgv.Rows.Clear();
        }

        private void cb_AxisToEdit_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var cb = sender as ComboBox;
            var axisName = cb.SelectedItem.ToString();
            if (_lastEditCURDItem_ID == 0)
            {
                //第一次下拉  什么都没有
                UpdateEditorContext(axisName);
            }
            else
            {

                if (_myMotionManager.Config.MotorGeneralSetting.ExistSingleItem(_lastEditCURDItem_ID) == true)
                {
                    //询问是否保存之前编辑的轴参数
                    SaveAxisSetting(_lastEditCURDItem_ID);
                }
            }

            var axis = _myMotionManager.GetAxis(axisName);
            if (axis != null)
            {
                AssignAxisToControlForm(axis);
                UpdateEditorContext(axisName);
                gb_singleAxisPlace.Text = $"单轴试运行[{axis.Interation.AxisTag}]";
            }
            else
            {
                ClearListDGV_V2(this.dgv_MotorSpeed);
                ClearListDGV_V2(this.dgv_MotorTable);
                ClearListDGV_V2(this.dgv_MotorModes);
                UninstallAxisToControlForm();
                gb_singleAxisPlace.Text = $"单轴试运行[未加载]";
            }
        }
        private void UpdateEditorContext(string axisName)
        {
            //获取单轴的配置  主要是如果有单轴存在的配置 就用最新的配置来存？

            try
            {
                var item = _myMotionManager.Config.MotorGeneralSetting.GetSingleItem(axisName);
                UIGeneric.FillListDGV_InfoKeyValue(this.dgv_MotorTable, item.MotorTable, INFO_COL_INDEX, KEY_COL_INDEX, VAL_COL_INDEX);
                UIGeneric.FillListDGV_InfoKeyValue(this.dgv_MotorSpeed, item.MotorSpeed, INFO_COL_INDEX, KEY_COL_INDEX, VAL_COL_INDEX);
                UIGeneric.FillListDGV_InfoKeyValue(this.dgv_MotorModes, item.MotorModes, INFO_COL_INDEX, KEY_COL_INDEX, VAL_COL_INDEX);

                _lastEditCURDItem_ID = item.ID;
            }
            catch (Exception ex)
            {
                _lastEditCURDItem_ID = 0;
            }
        }

        /// <summary>
        /// 保存单轴按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_saveSingleAxisSetting_Click(object sender, EventArgs e)
        {
            if (this._lastEditCURDItem_ID == 0)
            {
                var ret = MessageBox.Show("没有正在编辑的轴参数需要保存!");
                return;
            }
            else
            {
                SaveAxisSetting(this._lastEditCURDItem_ID);
                Clear_AxisSelected_Table_2();
                Update_AxisCollectionTable_Table_1();
            }
        }

        /// <summary>
        /// 保存单轴设定
        /// </summary>
        /// <param name="axisSettingItemID"></param>
        void SaveAxisSetting(long axisSettingItemID)
        {
            if (_myMotionManager.Config.MotorGeneralSetting.ExistSingleItem(axisSettingItemID) == false)
            {
                //不存在
                //询问是否新增
            }
            else
            {
                var _editSourceItem = _myMotionManager.Config.MotorGeneralSetting.GetSingleItem(axisSettingItemID);

                if (_editSourceItem == null || axisSettingItemID == 0)
                {
                    var ret = MessageBox.Show("不存在正在编辑的轴参数!");
                    return;
                }

                //将页面的信息全部都放在dictionary中
                var motorTable_EditDict = UIGeneric.Grab_DGV_KeyValueDict(this.dgv_MotorTable, KEY_COL_INDEX, VAL_COL_INDEX);
                var motorSpeed_EditDict = UIGeneric.Grab_DGV_KeyValueDict(this.dgv_MotorSpeed, KEY_COL_INDEX, VAL_COL_INDEX);
                var motorModes_EditDict = UIGeneric.Grab_DGV_KeyValueDict(this.dgv_MotorModes, KEY_COL_INDEX, VAL_COL_INDEX);

                ///---------------------------------------------
                ///表格与dictionary的区别  返回的是不等式的变量   只有在不相等的情况下才可以进行编辑
                var needToAlertTable = ReflectionTool.ComparePropertyValues(_editSourceItem.MotorTable, motorTable_EditDict);

                var needToAlertSpeed = ReflectionTool.ComparePropertyValues(_editSourceItem.MotorSpeed, motorSpeed_EditDict);

                var needToAlertModes = ReflectionTool.ComparePropertyValues(_editSourceItem.MotorModes, motorModes_EditDict);

                if (needToAlertTable || needToAlertSpeed || needToAlertModes)
                {
                    var ret = MessageBox.Show($"是否更新当前编辑轴 Name[{_editSourceItem.Name}] ID[{_editSourceItem.ID}]参数?", "保存参数", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (ret == DialogResult.Yes)
                    {
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                //将属性设置到对应的类中
                ReflectionTool.SetPropertyValues(_editSourceItem.MotorTable, motorTable_EditDict);
                ReflectionTool.SetPropertyValues(_editSourceItem.MotorSpeed, motorSpeed_EditDict);
                ReflectionTool.SetPropertyValues(_editSourceItem.MotorModes, motorModes_EditDict);

                string errMsg = string.Empty;

                if (_myMotionManager.Config.MotorGeneralSetting.UpdateSingleItem(_editSourceItem, ref errMsg))
                {
                    _myMotionManager.SaveConfig();
                    MessageBox.Show($"轴参数 Name[{_editSourceItem.Name}] ID[{_editSourceItem.ID}]保存成功!");
                }
                else
                {
                    MessageBox.Show($"轴参数 Name[{_editSourceItem.Name}] ID[{_editSourceItem.ID}]保存失败:[{errMsg}]!");
                }
            }
        }

        /// <summary>
        /// 保存所有轴的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_saveAllAxesSetting_Click(object sender, EventArgs e)
        {
            try
            {
                _myMotionManager.SaveConfig();
                UpdateAxesSelector();
                if (cb_AxisToEdit.SelectedItem != null)
                {
                    UpdateEditorContext(cb_AxisToEdit.SelectedItem.ToString());
                    Clear_AxisSelected_Table_2();
                    Update_AxisCollectionTable_Table_1();
                }
                MessageBox.Show($"保存所有轴参数成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存所有轴参数失败:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        /// <summary>
        /// 增加单轴设置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_addSingleAxisSetting_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                Form_NewAxisStarter nas = new Form_NewAxisStarter();
                var ret = nas.ShowDialog();
                if (ret == DialogResult.OK)
                {
                    //单个轴的设置
                    var mSetting = new MotorSetting();

                    //如果选择复制参数
                    if (nas.CopyCurrentAxisConfig)
                    {
                        //在选择下拉框的时候就有了
                        if (this._lastEditCURDItem_ID != 0)
                        {
                            //根据轴ID获取MotorAxisBase  this._AxesCollection.FindLast
                            //明白接口的方法为什么可以直接跳到接口的方法的实现？？？？？？
                            //MotionController_GUGAOEtherCAT : MotionControllerBase, IMotionController
                            //MotionControllerBase : InstrumentBase, IMotionController
                            //MotionControllerBase是虚拟类
                            //MotionControllerBase不是调用接口的方法，其实是调用MotionControllerBase里面的虚拟方法 我还是太笨了

                            if (_myMotionManager.Config.MotorGeneralSetting.ExistSingleItem(_lastEditCURDItem_ID) == true)
                            {
                                //询问是否保存之前编辑的轴参数
                                SaveAxisSetting(_lastEditCURDItem_ID);
                            }
                            var currentAxis = _myMotionManager.GetAxis(this._lastEditCURDItem_ID);
                            if (currentAxis != null)
                            {
                                //将设置复制
                                //var cloneSetting = currentAxis.MotorGeneralSetting.Clone();
                                var cloneSetting = CloneHelper.Clone<MotorSetting>(currentAxis.MotorGeneralSetting);
                                mSetting = cloneSetting;
                            }
                            else
                            {
                                MessageBox.Show($"当前没有可以拷贝的轴配置!将创建新的轴配置!");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"当前没有可以拷贝的轴配置!将创建新的轴配置!");
                        }
                    }
                    mSetting.Name = nas.NewAxisName;
                    mSetting.ID = IdentityGenerator.IG.GetIdentity();
                    mSetting.MotorTable.Name = nas.NewAxisName;
                    mSetting.MotorTable.AxisNo = nas.NewAxisNo;
                    mSetting.MotorTable.CardNo = nas.NewAxisCardNo;

                    //将单个放置在马达列表中  List<MotorSetting>
                    //需要将IO也序列化到配置文件中
                    //
                    bool isOk = _myMotionManager.AddNewAxis(mSetting);
                    //bool isOk = _myMotionManager.Config.MotorGeneralSetting.AddSingleItem(mSetting, ref errMsg);
                    if (isOk)
                    {
                        UpdateAxesSelector(nas.NewAxisName);
                        UpdateEditorContext(nas.NewAxisName);
                    }
                }
                else
                {
                    //取消了增加操作
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新增轴参数失败:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        /// <summary>
        /// 删除单轴按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_deleteSingleAxisSetting_Click(object sender, EventArgs e)
        {
            //通过下拉列表中得到的ID
            if (this._lastEditCURDItem_ID == 0)
            {
                var ret = MessageBox.Show("没有选中的轴参数可删除!");
                return;
            }
            else
            {
                if (_myMotionManager.Config.MotorGeneralSetting.ExistSingleItem(this._lastEditCURDItem_ID))
                {
                    var axisSetting = _myMotionManager.Config.MotorGeneralSetting.GetSingleItem(this._lastEditCURDItem_ID);

                    var ret = MessageBox.Show($"确定要删除轴 Name=[{axisSetting.Name}] id=[{axisSetting.ID}] 参数?");

                    string errMsg = string.Empty;

                    if (ret == DialogResult.OK)
                    {
                        //配置的数组中删除单个元素
                        if (_myMotionManager.Config.MotorGeneralSetting.DeleteSingleItem(axisSetting, ref errMsg))
                        {
                            _myMotionManager.SaveConfig();
                            MessageBox.Show($"轴参数 Name=[{axisSetting.Name}] id=[{axisSetting.ID}] 删除成功!");

                            ClearListDGV_V2(this.dgv_MotorModes);
                            ClearListDGV_V2(this.dgv_MotorSpeed);
                            ClearListDGV_V2(this.dgv_MotorTable);
                            UpdateAxesSelector();

                            if (cb_AxisToEdit.SelectedItem != null)
                            {
                                UpdateEditorContext(cb_AxisToEdit.SelectedItem.ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show($"轴参数 Name=[{axisSetting.Name}] id=[{axisSetting.ID}] 删除失败:[{errMsg}]!");
                        }
                    }
                    else
                    {
                        //轴参数 存在  但用户不删除了
                    }
                }
                else
                {
                    MessageBox.Show($"没有轴参数 id=[{this._lastEditCURDItem_ID}]可删除!");
                }
            }
        }

        /// <summary>
        /// 重新应用所有的轴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReinstallAxes_Click(object sender, EventArgs e)
        {
            try
            {
                this._core.Log_Global("应用所有轴配置参数到硬件...");
                UninstallAxisToControlForm();
                this._core.Log_Global("正在断开控制总线...");
                _myMotionManager.Close();
                this._core.Log_Global("正在应用最新配置...");
                _myMotionManager.ReinstallController();
                this._core.Log_Global("应用所有轴配置参数到硬件完成!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"应用所有轴配置参数到硬件完成错误[{ex.Message}{ex.StackTrace}] ");
            }
        }

        //****************************************************************************************************************
        //****************************************************************************************************************
        //****************************************************************************************************************
        //****************************************************************************************************************
        //****************************************************************************************************************
        /// <summary>
        /// 更新表格1的所有的轴
        /// </summary>
        private void Update_AxisCollectionTable_Table_1()
        {
            dgv_AxisCollectionTable.Rows.Clear();

            AxisCollection = _myMotionManager.GetAxesCollection();
            for (int i = 0; i < AxisCollection.Count; i++)
            {
                int rowIndex = dgv_AxisCollectionTable.Rows.Add();
                dgv_AxisCollectionTable.Rows[rowIndex].Cells[INFO_COL_INDEX].Value = AxisCollection[i].MotorGeneralSetting.Name.ToString();
                dgv_AxisCollectionTable.Rows[rowIndex].Cells[KEY_COL_INDEX].Value = AxisCollection[i].MotorGeneralSetting.MotorTable.AxisNo.ToString();
                ((DataGridViewCheckBoxCell)dgv_AxisCollectionTable.Rows[rowIndex].Cells[VAL_COL_INDEX]).Value = false;
            }
        }

        /// <summary>
        /// DataGridViewCheckBoxCell点击事件
        /// 该方法可以得到点击后的结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_AxisCollectionTable_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_AxisCollectionTable.IsCurrentCellDirty)
            {
                dgv_AxisCollectionTable.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            else
            {
                try
                {
                    string name = dgv_AxisCollectionTable.CurrentRow.Cells[INFO_COL_INDEX].Value.ToString();
                    string axis = dgv_AxisCollectionTable.CurrentRow.Cells[KEY_COL_INDEX].Value.ToString();
                    string position = $"axisPosition{axis}";
                    string _selectValue = ((DataGridViewCheckBoxCell)dgv_AxisCollectionTable.CurrentRow.Cells[VAL_COL_INDEX]).Value.ToString();
                    if (_selectValue == "True")
                    {
                        AxisSelected.Add(AxisCollection.Find(item => item.MotorGeneralSetting.Name == name && item.MotorGeneralSetting.MotorTable.AxisNo.ToString() == axis));
                    }
                    else
                    {
                        if (AxisSelected.Count < 1)
                        {
                            MessageBox.Show("没有被选中的轴，错误");
                        }
                        AxisSelected.RemoveAll(item => item.MotorGeneralSetting.Name == name && item.MotorGeneralSetting.MotorTable.AxisNo.ToString() == axis);
                    }
                    dgv_AxisSelectedTable.Rows.Clear();
                    for (int i = 0; i < AxisSelected.Count; i++)
                    {
                        int rowIndex = dgv_AxisSelectedTable.Rows.Add();
                        dgv_AxisSelectedTable.Rows[rowIndex].Cells[INFO_COL_INDEX].Value = AxisSelected[i].MotorGeneralSetting.Name.ToString();
                        dgv_AxisSelectedTable.Rows[rowIndex].Cells[KEY_COL_INDEX].Value = AxisSelected[i].MotorGeneralSetting.MotorTable.AxisNo.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception:{ex}");
                }
            }
        }

        /// <summary>
        /// 表格4显示 √
        /// </summary>
        /// <param name="apPos">position</param>
        private void dgv_AxisDisplayTable_Display_Table_4(AxesPosition apPos)
        {
            try
            {
                dgv_AxisDisplayTable.Rows.Clear();
                foreach (var ap in apPos)
                {
                    int rowIndex = dgv_AxisDisplayTable.Rows.Add();
                    dgv_AxisDisplayTable.Rows[rowIndex].Cells[INFO_COL_INDEX].Value = ap.Name;//轴说明
                    dgv_AxisDisplayTable.Rows[rowIndex].Cells[KEY_COL_INDEX].Value = ap.AxisNo.ToString();
                    dgv_AxisDisplayTable.Rows[rowIndex].Cells[VAL_COL_INDEX].Value = ap.Position;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception:{ex}");
            }
        }

        //从数据表返回来更新坐标点
        private void dgv_AxisDisplayTable_ReUpdate_Table_4(ref AxesPosition apPos)
        {
            try
            {
                Dictionary<string, double> dicpos = new Dictionary<string, double>();

                for(int rowIndex = 0; rowIndex < dgv_AxisDisplayTable.Rows.Count; rowIndex++)
                {
                    string apname = dgv_AxisDisplayTable.Rows[rowIndex].Cells[INFO_COL_INDEX].Value.ToString();
                    double pos = double.Parse(dgv_AxisDisplayTable.Rows[rowIndex].Cells[VAL_COL_INDEX].Value.ToString());
                    dicpos.Add(apname, pos);
                }

                foreach(var item in dicpos)
                {
                    apPos.GetSingleItem(item.Key).Position = item.Value;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception:{ex}");
            }
        }

        /// <summary>
        /// 点击单点操作按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreatePosition_Click(object sender, EventArgs e)
        {
            if (AxisSelected.Count < 1)
            {
                MessageBox.Show("坐标点没有对应的轴");
                return;
            }

            Form_PositionNameSetting form_PositionNameSetting = new Form_PositionNameSetting();
            form_PositionNameSetting.ConnectToCore(this._core);
            form_PositionNameSetting.Insert_SelectAxes(AxisSelected);//选中的轴
            //form_PositionNameSetting.Insert_AxesPositionCollection(_myMotionManager.PositionConfig.AxesPositionCollection);//AxesPositionCollection传入
            form_PositionNameSetting.Insert_AxesPositionCollection(_myMotionManager.GetPositionCollection());
            form_PositionNameSetting.ShowDialog();
            if (form_PositionNameSetting.DialogResult == DialogResult.Cancel)
            {
                MessageBox.Show("取消保存");
            }
            else
            {
                _myMotionManager.SavePositionConfig();
                Update_PositionCollectionTable_Table_3();
                MessageBox.Show("保存成功");
            }
            form_PositionNameSetting.DisconnectFromCore(this._core);
        }

        /// <summary>
        /// 清除表格2
        /// </summary>
        private void Clear_AxisSelected_Table_2()
        {
            AxisSelected.Clear();
            dgv_AxisSelectedTable.Rows.Clear();
        }

        /// <summary>
        /// 更新表格3  包含更新表格4
        /// </summary>
        private void Update_PositionCollectionTable_Table_3()
        {
            try
            {
                dgv_PositionCollectionTable.Rows.Clear();
                PositionNameList.Clear();
                PositionNameList = _myMotionManager.GetPositionNames();
                if (PositionNameList.Count > 0)
                {
                    for (int i = 0; i < PositionNameList.Count; i++)
                    {
                        int rowIndex = dgv_PositionCollectionTable.Rows.Add();
                        dgv_PositionCollectionTable.Rows[rowIndex].Cells[INFO_COL_INDEX].Value = PositionNameList[i];
                    }
                    dgv_PositionCollectionTable.Rows[0].Selected = true;
                    lblsinglePositionName.Text = dgv_PositionCollectionTable.CurrentCell == null ?
                                                    dgv_PositionCollectionTable.Rows[0].Cells[0].Value.ToString().Trim() :
                                                    dgv_PositionCollectionTable.CurrentCell.Value.ToString().Trim();
                    AxesPosition axesPosition = _myMotionManager.GetAxesPosition(lblsinglePositionName.Text);
                    dgv_AxisDisplayTable_Display_Table_4(axesPosition);
                }
                else
                {
                    dgv_AxisDisplayTable.Rows.Clear();
                    lblsinglePositionName.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception:{ex}");
            }
        }

        /// <summary>
        /// 单项点击  获取表格3的position句柄  表格4显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_PositionCollectionTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && dgv_PositionCollectionTable.Rows.Count > 0)
            {
                //之前的
                string selectPositionName = string.Empty;
                if (dgv_PositionCollectionTable.CurrentCell == null)
                {
                    selectPositionName = dgv_PositionCollectionTable.Rows[0].Cells[0].Value.ToString();//更新dgv_PositionCollectionTable  不是点击事件的时候
                }
                else
                {
                    selectPositionName = dgv_PositionCollectionTable.CurrentCell.Value.ToString().Trim();//是点击事件的时候
                }
                lblsinglePositionName.Text = selectPositionName;
                //var ap = _myMotionManager.PositionConfig.AxesPositionCollection.GetSingleItem(selectPositionName);
                var ap = _myMotionManager.GetAxesPosition(selectPositionName);
                if (ap != null)
                {
                    lastSelectedAxesPosition = ap;
                    dgv_AxisDisplayTable_Display_Table_4(ap);
                }
            }
        }

        /// <summary>
        /// 删除功能满足要求 √
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_myMotionManager.GetPositionNames().Count < 1)
                {
                    MessageBox.Show("没有点位置  无法进行操作");
                    return;
                }
                string message = $"是否删除点位置:{lblsinglePositionName.Text}";
                DialogResult dialogResult = MessageBox.Show(
                      message,
                      "提示",
                      MessageBoxButtons.OKCancel,
                      MessageBoxIcon.Information,
                      MessageBoxDefaultButton.Button1,
                      MessageBoxOptions.DefaultDesktopOnly);
                if (dialogResult == DialogResult.Cancel)
                {
                    return;
                }

                string selectPositionName = dgv_PositionCollectionTable.CurrentCell.Value.ToString().Trim();
                _myMotionManager.GetPositionCollection().RemoveItem(selectPositionName);
                _myMotionManager.SavePositionConfig();
                Update_PositionCollectionTable_Table_3();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception:{ex.Message}");
            }
        }

        private void btnRunSelectPosition_Click(object sender, EventArgs e)
        {
            if (lastSelectedAxesPosition != null)
            {
                if (_motorActionNewV2?.GetSTATUS() != STATUS.FREE)
                {
                    MessageBox.Show("轴正在运行");
                    return;
                }
                _motorActionNewV2?.Reset();

                Task.Factory.StartNew(() =>
                {
                    _motorActionNewV2?.MultipleAxisMotion(AxisCollection, lastSelectedAxesPosition);
                });
            }
        }

        private void btnRunStop_Click(object sender, EventArgs e)
        {
            _motorActionNewV2.Cancel();
            btnPause.Text = "暂停";
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (_motorActionNewV2?.GetSTATUS() == STATUS.FREE)
            {
                MessageBox.Show("机器没有运行");
                return;
            }
            if (btnPause.Text == "暂停")
            {
                btnPause.Text = "恢复";
                _motorActionNewV2.Pause();
            }
            else
            {
                btnPause.Text = "暂停";
                _motorActionNewV2.Resume();
            }
        }

        private void btnMulityAxisRun_Click(object sender, EventArgs e)
        {
            if (lastSelectedAxesPosition != null)
            {
                if (_motorActionNewV2?.GetSTATUS() != STATUS.FREE)
                {
                    MessageBox.Show("轴正在运行");
                    return;
                }
                _motorActionNewV2?.Reset();

                Task.Factory.StartNew(() =>
                {
                    _motorActionNewV2?.ParallelAxesMotion(AxisCollection, lastSelectedAxesPosition);
                });
            }
        }

        public void RunSingleThreadHome()
        {
            const short cardNo = 1;
            MotorAxisBase axis_STPY2_9 = _myMotionManager.GetAxis(cardNo, 9);//右侧PD模块前后
            if (axis_STPY2_9 == null)
            {
            }
            _motorActionNewV2.SingleAxisHome(axis_STPY2_9);

            MotorAxisBase axis_STPZ2_10 = _myMotionManager.GetAxis(cardNo, 10); //右侧PD模块上下
            if (axis_STPZ2_10 == null)
            {
            }
            _motorActionNewV2.SingleAxisHome(axis_STPZ2_10);

            MotorAxisBase axis_STPX2_20 = _myMotionManager.GetAxis(cardNo, 20); //右测试头切换X
            if (axis_STPX2_20 == null)
            {
            }
            _motorActionNewV2.SingleAxisHome(axis_STPX2_20);

            MotorAxisBase axis_STPY1_7 = _myMotionManager.GetAxis(cardNo, 7);//左侧PD模块前后
            if (axis_STPY1_7 == null)
            {
            }
            _motorActionNewV2.SingleAxisHome(axis_STPY1_7);

            MotorAxisBase axis_STPZ1_8 = _myMotionManager.GetAxis(cardNo, 8);//左侧PD模块上下
            if (axis_STPZ1_8 == null)
            {
            }
            _motorActionNewV2.SingleAxisHome(axis_STPZ1_8);

            MotorAxisBase axis_STPX1_23 = _myMotionManager.GetAxis(cardNo, 23);//右测试头切换X
            if (axis_STPX1_23 == null)
            {
            }
            _motorActionNewV2.SingleAxisHome(axis_STPX1_23);

            MotorAxisBase axis_SNZ2_12 = _myMotionManager.GetAxis(cardNo, 12);//右侧探针
            if (axis_SNZ2_12 == null)
            {
            }
            _motorActionNewV2.SingleAxisHome(axis_SNZ2_12);
            MotorAxisBase axis_SNZ1_11 = _myMotionManager.GetAxis(cardNo, 11);//左侧探针
            if (axis_SNZ1_11 == null)
            {
            }
            _motorActionNewV2.SingleAxisHome(axis_SNZ1_11);
            string a = "123";
        }

        public void RefreshOnce()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((EventHandler)delegate
                    {
                        Update_AxisCollectionTable_Table_1();
                        Clear_AxisSelected_Table_2();
                        Update_PositionCollectionTable_Table_3();
                    });
                }
                else
                {
                    Update_AxisCollectionTable_Table_1();
                    Clear_AxisSelected_Table_2();
                    Update_PositionCollectionTable_Table_3();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btn_callAxesRealtimeInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_axesInfoForm == null || _axesInfoForm.IsDisposed == true)
                {
                    _axesInfoForm = new Form_AxesRealtimeInfo();
                    _axesInfoForm.AssignMotionController(_myMotionManager.DefaultController);
                }
                this._core.PopUI_DefaultSize(_axesInfoForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"轴实时信息窗口弹出失败[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void btnEditPosition_Click(object sender, EventArgs e)
        {
            if (lastSelectedAxesPosition == null)
            {
                MessageBox.Show("没有选择任何需要编辑的坐标!");
                return;
            }

            var axes = this._myMotionManager.GetAxesCollection_ByAxesPosition(lastSelectedAxesPosition);        //选择的位置

            Form_PositionNameSetting form_PositionNameSetting = new Form_PositionNameSetting();
            form_PositionNameSetting.ConnectToCore(this._core);
            form_PositionNameSetting.Insert_SelectAxes(axes);//选中的轴
            form_PositionNameSetting.Insert_AxesPositionCollection(_myMotionManager.GetPositionCollection());//AxesPositionCollection传入
            form_PositionNameSetting.Enable_AxesPositionNameEditor(false);
            form_PositionNameSetting.SetDefault_AxesPositionNameEditor(lastSelectedAxesPosition.Name);  //选择的位置
            form_PositionNameSetting.ShowDialog();
            if (form_PositionNameSetting.DialogResult == DialogResult.Cancel)
            {
                MessageBox.Show("取消保存");
            }
            else
            {
                _myMotionManager.SavePositionConfig();
                Update_PositionCollectionTable_Table_3();
                MessageBox.Show("保存成功");
            }
            form_PositionNameSetting.DisconnectFromCore(this._core);
        }

        private void dgv_MotorTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgv_MotorTable.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        }

        private void dgv_MotorTable_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void cb_AxisToEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var cb = sender as ComboBox;
                var axisName = cb.SelectedItem.ToString();
                if (_lastEditCURDItem_ID == 0)
                {
                    //第一次下拉  什么都没有
                    UpdateEditorContext(axisName);
                }
                else
                {
                    if (_myMotionManager.Config.MotorGeneralSetting.ExistSingleItem(_lastEditCURDItem_ID) == true)
                    {
                        //询问是否保存之前编辑的轴参数
                        SaveAxisSetting(_lastEditCURDItem_ID);
                    }
                }

                var axis = _myMotionManager.GetAxis(axisName);
                if (axis != null)
                {
                    AssignAxisToControlForm(axis);
                    UpdateEditorContext(axisName);
                    //_singleAxisCtrlForm.ShowInfo(axis.Interation.AxisTag);
                    //gb_singleAxisPlace.Text = $"单轴试运行[{axis.Interation.AxisTag}]";
                }
                else
                {
                    ClearListDGV_V2(this.dgv_MotorSpeed);
                    ClearListDGV_V2(this.dgv_MotorTable);
                    ClearListDGV_V2(this.dgv_MotorModes);
                    UninstallAxisToControlForm();
                    //_singleAxisCtrlForm.ShowInfo(axis.Interation.AxisTag);
                    //gb_singleAxisPlace.Text = $"单轴试运行[未加载]";
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void btnDeletePosition_Click(object sender, EventArgs e)
        {
            try
            {
                if (_myMotionManager.GetPositionNames().Count < 1)
                {
                    MessageBox.Show("没有点位置  无法进行操作");
                    return;
                }

                if (dgv_PositionCollectionTable.SelectedCells.Count == 1)
                {
                    string selectPositionName = dgv_PositionCollectionTable.CurrentCell.Value.ToString().Trim();
                    string message = $"是否删除点位置:{selectPositionName}";
                    //                string message = $"是否删除点位置:{lblsinglePositionName.Text}";
                    DialogResult dialogResult = MessageBox.Show(
                          message,
                          "提示",
                          MessageBoxButtons.OKCancel,
                          MessageBoxIcon.Information,
                          MessageBoxDefaultButton.Button1);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    //string selectPositionName = dgv_PositionCollectionTable.CurrentCell.Value.ToString().Trim();
                    _myMotionManager.GetPositionCollection().RemoveItem(selectPositionName);
                    _myMotionManager.SavePositionConfig();
                    Update_PositionCollectionTable_Table_3();
                }
                else
                {
                    //20231219 支持批量删除
                    string message = $"是否批量删除点位置:共{dgv_PositionCollectionTable.SelectedCells.Count}个点位";
                    //                string message = $"是否删除点位置:{lblsinglePositionName.Text}";
                    DialogResult dialogResult = MessageBox.Show(
                          message,
                          "提示",
                          MessageBoxButtons.OKCancel,
                          MessageBoxIcon.Information,
                          MessageBoxDefaultButton.Button1);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                    foreach (var item in dgv_PositionCollectionTable.SelectedCells)
                    {
                        string selectPositionName = (item as DataGridViewTextBoxCell).Value.ToString();

                        _myMotionManager.GetPositionCollection().RemoveItem(selectPositionName);
                        _myMotionManager.SavePositionConfig();
                    }

                    Update_PositionCollectionTable_Table_3();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception:{ex.Message}");
            }
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //复制到剪贴板
            try
            {
                if (_myMotionManager.GetPositionNames().Count < 1)
                {
                    MessageBox.Show("没有点位置  无法进行操作");
                    return;
                }
                string selectPositionName = dgv_PositionCollectionTable.CurrentCell.Value.ToString().Trim();

                Clipboard.SetDataObject(selectPositionName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception:{ex.Message}");
            }
        }

        private void btn_SaveEditedPos_Click(object sender, EventArgs e)
        {
            if (lastSelectedAxesPosition == null)
            {
                MessageBox.Show("没有选择任何需要编辑的坐标!");
                return;
            }

            if(MessageBox.Show($"确认要保存编辑后的 Name[{lastSelectedAxesPosition.Name}] 坐标?","询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.No)
            {
                return;
            }


            //从界面取回
            dgv_AxisDisplayTable_ReUpdate_Table_4(ref lastSelectedAxesPosition);


            var axes = this._myMotionManager.GetAxesCollection_ByAxesPosition(lastSelectedAxesPosition);


            string errMsg = "";
            if (_myMotionManager.GetPositionCollection().UpdateSingleItem(lastSelectedAxesPosition, ref errMsg))
            {
                this._core.Log_Global($"成功修改{lastSelectedAxesPosition.Name}坐标");

                _myMotionManager.SavePositionConfig();
                MessageBox.Show($"坐标 Name[{lastSelectedAxesPosition.Name}]保存成功!");
                Update_PositionCollectionTable_Table_3();
            }
            else
            {
                MessageBox.Show($"坐标 Name[{lastSelectedAxesPosition.Name}]保存失败:[{errMsg}]!");
            }
             

        }

        private void btn_MoveUpStepToPos_Click(object sender, EventArgs e)
        {


            //dgv_AxisDisplayTable.SelectedCells


            //if (lastSelectedAxesPosition == null)
            //{
            //    MessageBox.Show("没有选择任何需要编辑的坐标!");
            //    return;
            //}

            ////从界面取回
            //dgv_AxisDisplayTable_ReUpdate_Table_4(ref lastSelectedAxesPosition);


            //var axes = this._myMotionManager.GetAxesCollection_ByAxesPosition(lastSelectedAxesPosition);


            //string errMsg = "";
            //if (_myMotionManager.GetPositionCollection().UpdateSingleItem(lastSelectedAxesPosition, ref errMsg))
            //{
            //    this._core.Log_Global($"成功修改{lastSelectedAxesPosition.Name}坐标");

            //    _myMotionManager.SavePositionConfig();
            //    MessageBox.Show($"坐标 Name[{lastSelectedAxesPosition.Name}]保存成功!");
            //    Update_PositionCollectionTable_Table_3();
            //}
            //else
            //{
            //    MessageBox.Show($"坐标 Name[{lastSelectedAxesPosition.Name}]保存失败:[{errMsg}]!");
            //}

        }

        //用当前坐标位置更新坐标
        private void btn_EditedPosByCurrPos_Click(object sender, EventArgs e)
        {
            if (lastSelectedAxesPosition == null)
            {
                MessageBox.Show("没有选择任何需要编辑的坐标!");
                return;
            }

            if (MessageBox.Show($"确认要使用当前坐标更新 Name[{lastSelectedAxesPosition.Name}] 坐标?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            //获取电机坐标
            var axes = this._myMotionManager.GetAxesCollection_ByAxesPosition(lastSelectedAxesPosition);

            //更新坐标
            foreach (var item in axes)
            {
                lastSelectedAxesPosition.GetSingleItem(item.Name).Position = item.Get_CurUnitPos();
            }

            //保存坐标
            string errMsg = "";
            if (_myMotionManager.GetPositionCollection().UpdateSingleItem(lastSelectedAxesPosition, ref errMsg))
            {
                this._core.Log_Global($"成功修改{lastSelectedAxesPosition.Name}坐标");

                _myMotionManager.SavePositionConfig();
                MessageBox.Show($"坐标 Name[{lastSelectedAxesPosition.Name}]保存成功!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                Update_PositionCollectionTable_Table_3();
            }
            else
            {
                MessageBox.Show($"坐标 Name[{lastSelectedAxesPosition.Name}]保存失败:[{errMsg}]!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

    }
}