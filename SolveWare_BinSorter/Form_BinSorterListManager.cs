using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;

namespace SolveWare_BinSorter
{
    public partial class Form_BinSorterListManager : Form, ITesterAppUI, IAccessPermissionLevel
    {

        public Form_BinSorterListManager()
        {
            InitializeComponent();
        }

        ITesterCoreInteration _core;
        ITesterAppPluginInteration _appInteration;

        //_myMotionManager.Config=TestSpecManagerConfig.Instance=TestSpecManagerConfig(实例化)
        //将所有信息最终存储到TestSpecManagerConfig(实例化)中
        BinSortListManager _myBinSortListManager;

        //从文件中读取、或者已保存到文件中的测试规格集合
        //  BinSettingCollection tempBinCollection;

        //需要进行操作的测试规格
        //   BinSetting tempBin;

        public void RefreshOnce()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((EventHandler)delegate
                    {


                    });
                }
                else
                {
                    //tempBin = null;
                    //this.pdgv_specEditor.Clear();
                    //tempBinCollection = new BinSettingCollection(_myBinSortManager.CurrentBinSettingCollection);
                    UpdateSpecsSelector();

                }
            }
            catch (Exception ex)
            {
            }
        }
        public AccessPermissionLevel APL
        {
            get { return AccessPermissionLevel.None; }
        }

        #region 平台交互UI及信息

        public void ConnectToAppInteration(ITesterAppPluginInteration app)
        {
            _appInteration = app;
            _myBinSortListManager = (BinSortListManager)app;
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
            _core = null;
        }

        private void ReceiveMessageFromCore(IMessage message)
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region 菜单栏操作

        private void 浮动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _appInteration.PopUI();
        }

        private void 还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _appInteration.DockUI();
        }

        #endregion


        //关闭前操作
        private void Form_TestSpecManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            _appInteration.DockUI();
        }

        //初始化操作
        private void Form_TestSpecManager_Load(object sender, EventArgs e)
        {
            UpdateSpecsSelector();
            this.InitialBinParamsFromSystem();
            this.InsetTagToDgv();
            this.RefreshComboBox();
            textBoxes = new List<TextBox>() { textBox_Gear1, textBox_Gear2, textBox_Gear3, textBox_Gear4, textBox_Gear5, textBox_Gear6, textBox_Gear7, textBox_Gear8, textBox_Gear9, textBox_Gear10, textBox_Gear11 };
        }

        #region 界面更新

        private void UpdateSpecsSelector()
        {
            cb_BinSettingCollectionToEdit.Items.Clear();
            var specNames = _myBinSortListManager.GetBinSettingCollectionNames();
            foreach (var sName in specNames)
            {
                cb_BinSettingCollectionToEdit.Items.Add(sName);
            }
            cb_BinSettingCollectionToEdit.SelectedIndex = -1;
            cb_BinSettingCollectionToEdit.Text = string.Empty;
        }

        private void cb_SpecToEdit_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string binSetCollectionName = string.Empty;
                binSetCollectionName = cb_BinSettingCollectionToEdit.SelectedItem.ToString();
                var binSetCollection = _myBinSortListManager.GetBinSettingCollection(binSetCollectionName);
                if (binSetCollection == null)
                {
                    MessageBox.Show($"未找到对应的规格 binSetting = {binSetCollectionName}!");
                    UpdateSpecsSelector();
                    return;
                }
                //将binSetCollection的名字全部都拿出来  先布置界面和comboBox
                Dictionary<string, int> comboBoxname_RowIndex = new Dictionary<string, int>();

                this.dgv_binCollectionEditor.Rows.Clear();
                for (int i = 0; i < binSetCollection.ItemCollection.Count; i++)//BinSetting
                {
                    for (int j = 0; j < binSetCollection.ItemCollection[i].ItemCollection.Count; j++)//BinJudgeItem
                    {
                        string binJudgeItemName = binSetCollection.ItemCollection[i].ItemCollection[j].Name;
                        if (!comboBoxname_RowIndex.Keys.Contains(binJudgeItemName))
                        {
                            int rowIndex = this.dgv_binCollectionEditor.Rows.Add();//有多少就增加多少行  这是直接触发Add事件
                            System.Threading.Thread.Sleep(1);
                            (this.dgv_binCollectionEditor.Rows[rowIndex].Tag as ComboBox).Text = binJudgeItemName;
                            comboBoxname_RowIndex.Add(binJudgeItemName, rowIndex);
                        }
                    }
                }

                //填充dgv
                const int BinColumnCount = 11;//总共是4个bin
                for (int i = 0; i < BinColumnCount; i++)
                {
                    textBoxes[i].Text = binSetCollection.ItemCollection[i].Name;
                    for (int j = 0; j < binSetCollection.ItemCollection[i].ItemCollection.Count; j++)//BinJudgeItem
                    {
                        BinJudgeItem binJudgeItem = binSetCollection.ItemCollection[i].ItemCollection[j];
                        ((DataGridViewCheckBoxCell)dgv_binCollectionEditor.Rows[comboBoxname_RowIndex[binJudgeItem.Name]].Cells[checkboxIndex[i]]).Value = binJudgeItem.Enable;
                        dgv_binCollectionEditor.Rows[comboBoxname_RowIndex[binJudgeItem.Name]].Cells[checkboxIndex[i] + 1].Value = binJudgeItem.LL;
                        dgv_binCollectionEditor.Rows[comboBoxname_RowIndex[binJudgeItem.Name]].Cells[checkboxIndex[i] + 2].Value = binJudgeItem.UL;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示");
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                int[] chkColIndex = { 0, 3, 6, 9, 12, 15, 18, 21, 24, 27, 30 };
                int ret = 0;
                if (int.TryParse(textBox_AddRowCount.Text, out ret))
                {
                    if (ret < 1)
                    {
                        ret = 1;
                    }
                    else if (ret > 20)
                    {
                        ret = 20;
                    }
                    textBox_AddRowCount.Text = ret.ToString();
                    for (int i = 0; i < ret; i++)
                    {
                        var newRowIndex = this.dgv_binCollectionEditor.Rows.Add();
                        System.Threading.Thread.Sleep(1);
                        foreach (var colIndex in chkColIndex)
                        {
                            this.dgv_binCollectionEditor.Rows[newRowIndex].Cells[colIndex].Value = true;
                            System.Threading.Thread.Sleep(1);
                        }
                    }
                    //var a = tagList;
                    //var b = tagList.Select(t => (t as ComboBox).Name).ToList();//可以看看每个comboBox是不是不一样
                }
                else
                {
                    MessageBox.Show("请输入正确的数值");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #endregion


        //#region 添加空白测试规格

        //private void btn_addSpec_Click(object sender, EventArgs e)
        //{
        //    try
        //    {


        //        string binSetName = string.Empty;
        //        if (cb_BinSettingCollectionToEdit.SelectedItem == null ||
        //        string.IsNullOrEmpty(cb_BinSettingCollectionToEdit.SelectedItem.ToString()))
        //        {

        //        }
        //        else
        //        {
        //            binSetName = cb_BinSettingCollectionToEdit.SelectedItem.ToString();
        //            var binsetting = _myBinSortListManager.GetBinSettingCollection(binSetName);
        //            if (binsetting != null)
        //            {
        //                var dr = MessageBox.Show("是否保存当前项", "提示", MessageBoxButtons.OKCancel);
        //                if (dr == DialogResult.OK)
        //                {//保存

        //                    BinSetting newBinsetting = new BinSetting(); 
        //                    newBinsetting.Name = "pdgvBin";
        //                    var dgvBin = GetDataFromPdgv(newBinsetting);
        //                    newBinsetting = dgvBin;
        //                    string msg = string.Empty;

        //                    //替换保存
        //                    var result = _myBinSortListManager.UpdateBinSettingCollection_Of_MasterList(binSetName, newBinsetting, ref msg);
        //                    _myBinSortListManager.Save_Collection();

        //                }
        //                else
        //                {
        //                    //不操作
        //                }
        //            }
        //        }
        //        //新建

        //        BinSetting newBin = new BinSetting();
        //        DateTime dateTime = DateTime.Now;
        //        newBin.Name = "SampleBin_" + dateTime.ToString("MMddHHmmssss");


        //        List<string> specName = new List<string>();
        //        var orgBinNames = _myBinSortListManager.GetBinSettingTags();

        //        specName.AddRange(orgBinNames);

        //        Form_NewBinInfo frm = new Form_NewBinInfo(newBin);
        //        frm.SpecNameList = specName;
        //        var dr1 = frm.ShowDialog();
        //        if (dr1 == DialogResult.OK)
        //        {
        //            newBin.Name = frm.TestSpecName;
        //        }
        //        else
        //        {
        //            return;
        //        }

        //        newBin.ItemCollection = BinSetting.CreateDefaultCollection();
        //        _myBinSortListManager.AddBinSettingToCollection(newBin);
        //        UpdateSpecsSelector();
        //        cb_BinSettingCollectionToEdit.SelectedItem = newBin.Name;
        //        this.pdgv_specEditor.ImportSourceData<BinJudgeItem>(newBin, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"添加空白Bin失败:{ex.Message}{ex.StackTrace}!");
        //    }
        //}


        //#endregion


        //#region 删除当前测试规格

        //private void btn_deleteSpec_Click(object sender, EventArgs e)
        //{
        //    if (cb_BinSettingCollectionToEdit.SelectedItem == null ||
        //        string.IsNullOrEmpty(cb_BinSettingCollectionToEdit.SelectedItem.ToString()))
        //    {
        //        MessageBox.Show("请选择需要删除的测试规格");
        //        return;
        //    }

        //    var dr = MessageBox.Show("确认删除当前测试规格吗", "提示", MessageBoxButtons.OKCancel);
        //    if (dr == DialogResult.Cancel)
        //    {
        //        return;
        //    }

        //    var binName = cb_BinSettingCollectionToEdit.SelectedItem.ToString();
        //    try
        //    {
        //        DelAndUpdate(binName);
        //        MessageBox.Show("删除成功！", "提示");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
        //    }
        //}
        //#endregion


        //#region 复制当前测试规格并新增

        //private void btn_copySpec_Click(object sender, EventArgs e)
        //{


        //    if (cb_BinSettingCollectionToEdit.SelectedItem == null ||
        //   string.IsNullOrEmpty(cb_BinSettingCollectionToEdit.SelectedItem.ToString()))
        //    {
        //        MessageBox.Show("请选择需要复制的测试规格");
        //        return;
        //    }
        //    var binName = cb_BinSettingCollectionToEdit.SelectedItem.ToString();
        //    BinSetting sourceBin = null;
        //    //新建
        //    BinSetting newBin = null;
        //    try
        //    {
        //        //0418XINZENG
        //        sourceBin = _myBinSortListManager.GetBinByTag(binName);
        //        newBin = new BinSetting(sourceBin);
        //        DateTime dateTime = DateTime.Now;
        //        newBin.Name = "SampleBin_" + dateTime.ToString("MMddHHmmssss");

        //        this.pdgv_specEditor.Clear();
        //        this.cb_BinSettingCollectionToEdit.Text = string.Empty;

        //        this.cb_BinSettingCollectionToEdit.SelectedIndex = -1;// = string.Empty;
        //        //this.cb_SpecToEdit.Text = string.Empty;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
        //        return;

        //    }
        //    var orgBinNames = _myBinSortListManager.GetBinSettingTags();
        //    List<string> specName = new List<string>();

        //        specName.AddRange(orgBinNames);

        //    Form_NewBinInfo frm = new Form_NewBinInfo(newBin);
        //    frm.SpecNameList = specName;
        //    var dr1 = frm.ShowDialog();
        //    if (dr1 == DialogResult.OK)
        //    {
        //        newBin.Name = frm.TestSpecName;
        //    }
        //    else
        //    {
        //        return;
        //    }

        //    try
        //    {
        //        //0418
        //        _myBinSortListManager.AddBinSettingToCollection(newBin);
        //        //0418
        //        UpdateSpecsSelector();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
        //    }

        //    //0418
        //    this.cb_BinSettingCollectionToEdit.Text = newBin.BinTag;
        //    this.pdgv_specEditor.ImportSourceData<BinJudgeItem>(newBin, true);

        //    //0418

        //}

        //#endregion


        //#region 添加空白测试规格项


        //#endregion


        //#region 删除当前测试规格项


        //#endregion


        //#region 保存当前测试规格

        //private void btn_saveSpec_Click(object sender, EventArgs e)
        //{
        //    try
        //    {


        //        string binSetName = string.Empty;
        //        if (cb_BinSettingCollectionToEdit.SelectedItem == null ||
        //        string.IsNullOrEmpty(cb_BinSettingCollectionToEdit.SelectedItem.ToString()))
        //        {
        //            this._myBinSortListManager.Save_Collection();
        //        }
        //        else
        //        {
        //            binSetName = cb_BinSettingCollectionToEdit.SelectedItem.ToString();
        //            var binsetting = _myBinSortListManager.GetBinByTag(binSetName);
        //            if (binsetting != null)
        //            {

        //                BinSetting newBinsetting = new BinSetting();
        //                newBinsetting.Name = "pdgvBin";
        //                var dgvBin = GetDataFromPdgv(newBinsetting);
        //                newBinsetting = dgvBin;
        //                string msg = string.Empty;

        //                //替换保存
        //                var result = _myBinSortListManager.UpdateTestSpecInCollection(binSetName, newBinsetting, ref msg);
        //                _myBinSortListManager.Save_Collection();

        //            }
        //        }
        //        MessageBox.Show("保存成功！", "提示");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("保存当前测试规格失败，异常原因：" + ex.Message, "提示");
        //        return;
        //    }
        //}

        //#endregion


        //#region pdgv获取数据
        //private BinSetting GetDataFromPdgv(BinSetting newBin)
        //{
        //    try
        //    {
        //        if (this.pdgv_specEditor.RowCount < 1)
        //        {
        //            throw new Exception("当前测试规格无内容");
        //        }
        //        newBin.ItemCollection = new List<BinJudgeItem>();
        //        List<int> errorlist = new List<int>();
        //        bool isOK = true;
        //        string errMsg = string.Empty;
        //        //获取数据
        //        for (int rowIndex = 0; rowIndex < this.pdgv_specEditor.RowCount; rowIndex++)
        //        {
        //            var row = this.pdgv_specEditor.Rows[rowIndex];
        //            BinJudgeItem item = new BinJudgeItem();
        //            //遍历当前dgv的列
        //            foreach (DataGridViewColumn dgvCol in this.pdgv_specEditor.Columns)
        //            {
        //                //之前存下来的属性Tag可以反向转化
        //                var colProp = dgvCol.Tag as PropertyInfo;
        //                //列号
        //                var colIndex = dgvCol.Index;
        //                //设置对象的属性值(对象+值)
        //                colProp.SetValue(item, Converter.ConvertObjectTo(row.Cells[dgvCol.Index].Value, colProp.PropertyType));
        //            }
        //            if (string.IsNullOrEmpty(item.Name) || item.Name?.Trim().Length == 0)
        //            {//名称为空
        //                isOK = false;
        //                errorlist.Add(rowIndex + 1);
        //            }
        //            else
        //            {//不为空
        //             //添加同时判断重名
        //                newBin.AddSingleItem(item);
        //            }
        //        }
        //        //判断整个规格数据有效性
        //        if (!isOK)
        //        {
        //            foreach (var item in errorlist)
        //            {
        //                errMsg += $"第{item}行、";
        //            }
        //            errMsg += "测试规格项无效！";
        //            errMsg = errMsg.Insert(0, $"测试规格{newBin.BinTag}的");
        //            throw new Exception(errMsg);
        //        }
        //        if (!newBin.Check(out errMsg))
        //        {
        //            throw new Exception(errMsg);
        //        }
        //        return newBin;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        //#endregion



        ////判断是否改动

        ////采集表中数据    行、列，值Dictionary<int, Dictionary<string, object>>
        ////列与值，构成   TestSpecificationItem
        ////行，是TestSpecificationItem的单体

        ////将当前操作TestSpecification，中的List<TestSpecificationItem>()
        ////变为行+单体对象，，即Dictionary<int, TestSpecificationItem>

        ////分别比对行数，以及TestSpecificationItem

        ////比对完后在进行检查数据有效性


        //public Dictionary<int, BinJudgeItem> GetKeyValuePairs(BinSetting bin)
        //{
        //    Dictionary<int, BinJudgeItem> listSourseDict = new Dictionary<int, BinJudgeItem>();
        //    if (bin.ItemCollection.Count < 1)
        //    {
        //        return listSourseDict;
        //    }

        //    for (int index = 0; index < bin.ItemCollection.Count; index++)
        //    {
        //        listSourseDict.Add(index, bin.ItemCollection[index]);
        //    }
        //    return listSourseDict;
        //}

        //public Dictionary<int, Dictionary<string, object>> GetDataFromDGV(DataGridView dgv)
        //{
        //    Dictionary<int, Dictionary<string, object>> listDict = new Dictionary<int, Dictionary<string, object>>();


        //    if (dgv.Rows.Count <= 0)
        //    {
        //        return listDict;
        //    }
        //    try
        //    {
        //        for (int rIndex = 0; rIndex < dgv.RowCount; rIndex++)
        //        {
        //            Dictionary<string, object> rowDict = new Dictionary<string, object>();
        //            foreach (DataGridViewColumn dgvCol in dgv.Columns)
        //            {
        //                //获取属性
        //                var pInfo = dgvCol.Tag as PropertyInfo;
        //                var key = pInfo.Name;
        //                var cIndex = dgvCol.Index;
        //                var pValue = dgv.Rows[rIndex].Cells[cIndex].Value;
        //                try
        //                {
        //                    var value = Converter.ConvertObjectTo(pValue, pInfo.PropertyType);
        //                    rowDict.Add(key, value);
        //                }
        //                catch (Exception ex)
        //                {
        //                    throw new Exception($"当前第[{rIndex + 1}]行、第[{cIndex + 1}]列的{key}项输入的数据类型有误！");
        //                }

        //            }
        //            listDict.Add(rIndex, rowDict);
        //        }
        //        return listDict;

        //    }
        //    catch (Exception ex)

        //    {
        //        throw ex;
        //    }
        //}

        //public bool CompareDgvValuesAndRecipeValues(object sourceObject, Dictionary<string, object> dict)
        //{
        //    try
        //    {
        //        return ReflectionTool.ComparePropertyValues(sourceObject, dict);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}


        ///// <summary>
        ///// 判断DGV展示数据前后，是否变化
        ///// </summary>
        ///// <param name="spec"></param>
        ///// <param name="dgv"></param>
        ///// <returns>返回true表示无变化，返回false表示变化</returns>
        //public bool IsEqualDgvValuesAndSpcValues(BinSetting bin, DataGridView dgv)
        //{
        //    try
        //    {
        //        //dgv展示前数据
        //        var listSourseDict = GetKeyValuePairs(bin);
        //        //dgv展示后数据
        //        var listDict = GetDataFromDGV(dgv);

        //        var rs = listSourseDict.Keys.Count.Equals(listDict.Keys.Count);

        //        if (!rs)
        //        {//数目不对，不相等
        //            return false;
        //        }

        //        if (listSourseDict == null && listDict == null)
        //        {//null即相等
        //            return true;
        //        }

        //        if (listSourseDict.Keys.Count == 0 && listDict.Keys.Count == 0)
        //        {//数目为0，相等
        //            return true;
        //        }

        //        var count = listSourseDict.Keys.Count;

        //        for (int index = 0; index < count; index++)
        //        {
        //            var sourceObject = listSourseDict[index];
        //            var dict = listDict[index];

        //            //返回true，表示不相等
        //            rs = CompareDgvValuesAndRecipeValues(sourceObject, dict);
        //            if (rs)
        //            {//rs返回true,表示不相等
        //                return false;
        //            }
        //        }
        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //#region 判断文件中是否存在


        //#endregion

        //#region 保存并更新文件

        //private void SaveAndUpdate(string binName,    BinSetting spec)
        //{
        //    string msg = string.Empty;
        //    try
        //    {
        //        if (spec.ItemCollection == null)
        //        {
        //            throw new Exception("测试规格内容为空！！！");
        //        }


        //        var result = _myBinSortListManager.UpdateTestSpecInCollection(binName,spec, ref msg);


        //        _myBinSortListManager.Save_Collection();
        //        //更新
        //        //tempBinCollection = new BinSettingCollection(_myBinSortManager.CurrentBinSettingCollection);
        //        UpdateSpecsSelector();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //#endregion

        //#region 删除并更新文件

        //private void DelAndUpdate(BinSetting spec, bool isExist)
        //{
        //    try
        //    {
        //        string msg = string.Empty;
        //        //文件中无
        //        var result = _myBinSortListManager.DeleteTestSpecFromCollection(spec, ref msg);
        //        if (!result)
        //        {
        //            throw new Exception(msg);
        //        }
        //        if (isExist)
        //        {//文件中有
        //            _myBinSortListManager.Save_Collection();
        //            //tempBinCollection = new BinSettingCollection(_myBinSortManager.CurrentBinSettingCollection);
        //        }
        //        //更新文件
        //        //tempBin = null;
        //        UpdateSpecsSelector();
        //        //cb_BinToEdit.Text = string.Empty;
        //        this.pdgv_specEditor.Clear();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        //private void DelAndUpdate(string binName/*, bool isExist*/)
        //{
        //    try
        //    {
        //        string msg = string.Empty;
        //        //文件中无
        //        var result = _myBinSortListManager.DeleteTestSpecFromCollection(binName, ref msg);
        //        if (!result)
        //        {
        //            throw new Exception(msg);
        //        }

        //        _myBinSortListManager.Save_Collection();


        //        UpdateSpecsSelector();
        //        //cb_BinToEdit.Text = string.Empty;
        //        this.pdgv_specEditor.Clear();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}


        //#endregion

        //private void tsmi_DeleteBinJudgeItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //if (tempBin == null)
        //        //{
        //        //    MessageBox.Show("请选择一项测试规格", "提示");
        //        //    return;
        //        //}
        //        if (this.pdgv_specEditor.RowCount == 0)
        //        {
        //            MessageBox.Show("当前测试规格内容为空，无法删除", "提示");
        //            return;
        //        }
        //        int index = this.pdgv_specEditor.CurrentRow.Index;
        //        if (index == -1)
        //        {
        //            MessageBox.Show("请选择需要删除的行", "提示");
        //            return;
        //        }

        //        if (MessageBox.Show("确定要删除当前BIN设置所选的单个判断项?", "删除当前BIN设置所选的单个判断项", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
        //        {
        //            this.pdgv_specEditor.Rows.RemoveAt(index);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("删除所选测试规格项失败，异常原因：" + ex.Message, "提示");
        //    }
        //}

        //private void tsmi_AddNewBinJudgeItem_Click(object sender, EventArgs e)
        //{
        //    //if (tempBin == null)
        //    //{
        //    //    MessageBox.Show("请选择需要添加空白判断项的BIN设置！");
        //    //    return;
        //    //}
        //    try
        //    {
        //        this.pdgv_specEditor.Rows.Add();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("添加空白判断项失败，异常原因：" + ex.Message, "提示");
        //    }
        //}

        //zkz初步写
        const int rowHeight = 34;
        List<object> tagList = new List<object>();
        List<string> binParams = new List<string>();
        Dictionary<string, string> binParam_Unit = new Dictionary<string, string>();
        List<int> checkboxIndex = new List<int>() { 0, 3, 6, 9, 12, 15, 18, 21, 24, 27, 30 };
        List<TextBox> textBoxes = null;
        private void RefreshComboBox()//将list的combobox排列
        {
            panelComboBox.Controls.Clear();
            int dgvRowHeight = dgv_binCollectionEditor.RowTemplate.Height;//获取当前的行高
            int lineHeight = 0;//线的粗2像素  2X2=4   
            for (int i = 0; i < tagList.Count; i++)
            {
                int panelComboBoxWidth = panelComboBox.Width;
                ComboBox comboBox = tagList[i] as ComboBox;
                comboBox.Parent = panelComboBox;
                comboBox.Location = new System.Drawing.Point(3, 5 + (dgvRowHeight + lineHeight) * i);
                comboBox.Width = panelComboBoxWidth - 30;
                comboBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            }
        }

        private ComboBox CreateComboBox()//创建一个ComboBox
        {
            ComboBox comboBox = new ComboBox();
            string timeNow = DateTime.Now.ToString("HHmmssffff");
            comboBox.Name = "ComboBox" + timeNow;
            comboBox.Items.AddRange(binParams.ToArray());
            comboBox.TextUpdate += comboBox_TextUpdate;
            return comboBox;
        }


        private void dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            this.InsetTagToDgv();
            this.RefreshComboBox();
        }

        private bool InsetTagToDgv()
        {
            try
            {
                tagList.Clear();
                System.Threading.Thread.Sleep(1);
                for (int i = 0; i < dgv_binCollectionEditor.Rows.Count; i++)
                {
                    if (dgv_binCollectionEditor.Rows[i].Tag == null)
                    {
                        dgv_binCollectionEditor.Rows[i].ContextMenuStrip = this.menuStripLaserX;
                        ComboBox combo = CreateComboBox();
                        dgv_binCollectionEditor.Rows[i].Tag = combo as object;
                        tagList.Add(combo as object);
                    }
                    else
                    {
                        tagList.Add(dgv_binCollectionEditor.Rows[i].Tag);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void comboBox_TextUpdate(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            cb.Items.Clear();
            List<string> temp = new List<string>(); //临时存放备查数据
            foreach (var item in binParams)
            {
                if (item.Contains(cb.Text))
                {
                    temp.Add(item);
                }
            }
            if (temp.Count < 1)
            {
                cb.Items.Add("");
                cb.SelectionStart = cb.Text.Length;
            }
            else
            {
                try
                {
                    cb.Items.AddRange(temp.ToArray());  //combobox添加已经查到的关键词
                    cb.SelectionStart = cb.Text.Length;     //设置光标位置，否则光标位置始终保持在第一列，造成输入关键词的倒序排列
                    Cursor = Cursors.Default; //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
                    cb.DroppedDown = true;       //自动弹出下拉框
                }
                catch
                {
                    cb.SelectedIndex = -1;
                }
            }
        }


        private void InitialBinParamsFromSystem()
        {

            //List<BinJudgeItem> items = BinSetting.CreateDefaultCollection();
            foreach (var item in _myBinSortListManager.BinJudgeItemPool)
            {
                binParams.Add(item.Name);
                binParam_Unit.Add(item.Name, item.Unit);
            }
        }

        private void 删除当前行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentRowIndex = dgv_binCollectionEditor.CurrentRow.Index;
            tagList.Remove(dgv_binCollectionEditor.CurrentRow.Tag);
            dgv_binCollectionEditor.Rows.RemoveAt(currentRowIndex);
            RefreshComboBox();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show($"是否保存", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
                //检测左侧的comboBox是否都选择了分Bin参数
                List<string> panelComboBoxNames = new List<string>();
                for (int i = 0; i < panelComboBox.Controls.Count; i++)
                {
                    if ((panelComboBox.Controls[i] as ComboBox) != null)
                    {
                        if (string.IsNullOrEmpty(((ComboBox)panelComboBox.Controls[i]).Text))
                        {
                            MessageBox.Show("保存失败:\n有分档参数为空，请检查！！！");
                            return;
                        }
                        else
                        {
                            if (!binParam_Unit.Keys.Contains(((ComboBox)panelComboBox.Controls[i]).Text))
                            {
                                MessageBox.Show("保存失败:\n有分档参数不在选择项，请检查！！！");
                                return;
                            }
                            panelComboBoxNames.Add(((ComboBox)panelComboBox.Controls[i]).Text);
                        }
                    }
                }
                List<string> distinctNames = panelComboBoxNames.Distinct().ToList();
                if (distinctNames.Count != panelComboBoxNames.Count)
                {
                    MessageBox.Show("保存失败:\n有重复的分档参数，请检查！！！");
                    return;
                }
                if (dgv_binCollectionEditor.Rows.Count < 1)
                {
                    MessageBox.Show("保存失败:\n表格中没有数值，无法保存");
                    return;
                }

                const int BinColumnCount = 11;//总共是4个bin
                const double minValue = -9999;
                const double maxValue = 9999;
                double defaultValueMin = -9999;
                double defaultValueMax = 9999;
                BinSettingCollection binSettingCollection = new BinSettingCollection();
                for (int i = 0; i < BinColumnCount; i++)//i列
                {
                    BinSetting binSetting = new BinSetting();//列
                    binSetting.Name = textBoxes[i].Text;
                    for (int j = 0; j < dgv_binCollectionEditor.Rows.Count; j++)//循环行
                    {

                        defaultValueMin = -9999;
                        defaultValueMax = 9999;
                        BinJudgeItem binJudgeItem = new BinJudgeItem();
                        binJudgeItem.Name = (dgv_binCollectionEditor.Rows[j].Tag as ComboBox).Text;
                        binJudgeItem.Visible = true;
                        object dgvcheckboxValue = ((DataGridViewCheckBoxCell)dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i]]).Value;
                        if (dgvcheckboxValue == null)
                        {
                            binJudgeItem.Enable = false;
                        }
                        else
                        {
                            binJudgeItem.Enable = Convert.ToBoolean(dgvcheckboxValue);
                        }

                        //如果没有填写值  我们默认初始值-9999 9999
                        if (dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i] + 1].Value == null)//为不动
                        {
                            binJudgeItem.LL = minValue;
                        }
                        else if (dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i] + 1].Value.ToString() == string.Empty)
                        {
                            binJudgeItem.LL = minValue;
                        }
                        else if (!double.TryParse(dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i] + 1].Value.ToString(), out defaultValueMin))
                        {
                            MessageBox.Show($"保存失败:\n表格  [ 行:{j + 1}  列:{checkboxIndex[i] + 2} ]  中有无法识别的文字，无法保存");
                            return;
                        }
                        else
                        {
                            binJudgeItem.LL = defaultValueMin;
                        }


                        if (dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i] + 2].Value == null)
                        {
                            binJudgeItem.UL = maxValue;
                        }
                        else if (dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i] + 2].Value.ToString() == string.Empty)
                        {
                            binJudgeItem.UL = maxValue;
                        }
                        else if (!double.TryParse(dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i] + 2].Value.ToString(), out defaultValueMax))
                        {
                            MessageBox.Show($"保存失败:\n表格  [ 行:{j + 1}  列:{checkboxIndex[i] + 3} ]  中有无法识别的文字，无法保存");
                            return;
                        }
                        else
                        {
                            binJudgeItem.UL = defaultValueMax;
                        }
                        binJudgeItem.Unit = binParam_Unit[binJudgeItem.Name];
                        binSetting.AddSingleItem(binJudgeItem);

                    }
                    binSettingCollection.AddSingleItem(binSetting);
                }
                Form_NewBinInfo form_NewBinInfo = new Form_NewBinInfo(binSettingCollection.ItemCollection[0]);
                form_NewBinInfo.ShowDialog();
                if (form_NewBinInfo.DialogResult != DialogResult.OK)
                {
                    MessageBox.Show("用户取消保存");
                    return;
                }
                if (string.IsNullOrEmpty(form_NewBinInfo.TestSpecName))
                {
                    MessageBox.Show("保存名称不能为空");
                    return;
                }
                if (cb_BinSettingCollectionToEdit.Items.Contains(form_NewBinInfo.TestSpecName))
                {
                    MessageBox.Show("存在相同的名称，请更改bin规格名字");
                    return;
                }

                binSettingCollection.Name = form_NewBinInfo.TestSpecName;
                _myBinSortListManager.AddBinSettingCollection_Of_MasterList(binSettingCollection);
                _myBinSortListManager.Save_Collection();
                MessageBox.Show("保存成功");
                List<string> names = _myBinSortListManager.GetBinSettingCollectionNames();
                if (names.Count > 0)
                {
                    cb_BinSettingCollectionToEdit.Items.Clear();
                    cb_BinSettingCollectionToEdit.Items.AddRange(names.ToArray());
                    cb_BinSettingCollectionToEdit.Text = binSettingCollection.Name;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败:\n{ex.Message}");
            }
        }

        private void btnSaveNowBin_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cb_BinSettingCollectionToEdit.Text))
                {
                    MessageBox.Show($"当前产品规格名为空，无法保当前规格，请输入规格名再进行保存");
                    return;
                }
                if (MessageBox.Show($"是否保存", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
                //检测左侧的comboBox是否都选择了分Bin参数
                List<string> panelComboBoxNames = new List<string>();
                for (int i = 0; i < panelComboBox.Controls.Count; i++)
                {
                    if ((panelComboBox.Controls[i] as ComboBox) != null)
                    {
                        if (string.IsNullOrEmpty(((ComboBox)panelComboBox.Controls[i]).Text))
                        {
                            MessageBox.Show("保存失败:\n有分档参数为空，请检查！！！");
                            return;
                        }
                        else
                        {
                            if (!binParam_Unit.Keys.Contains(((ComboBox)panelComboBox.Controls[i]).Text))
                            {
                                MessageBox.Show("保存失败:\n有分档参数不在选择项，请检查！！！");
                                return;
                            }
                            panelComboBoxNames.Add(((ComboBox)panelComboBox.Controls[i]).Text);
                        }
                    }
                }
                List<string> distinctNames = panelComboBoxNames.Distinct().ToList();
                if (distinctNames.Count != panelComboBoxNames.Count)
                {
                    MessageBox.Show("保存失败:\n有重复的分档参数，请检查！！！");
                    return;
                }
                if (dgv_binCollectionEditor.Rows.Count < 1)
                {
                    MessageBox.Show("保存失败:\n表格中没有数值，无法保存");
                    return;
                }

                const int BinColumnCount = 11;//总共是4个bin
                const double minValue = -9999;
                const double maxValue = 9999;
                double defaultValueMin = -9999;
                double defaultValueMax = 9999;
                BinSettingCollection binSettingCollection = new BinSettingCollection();
                for (int i = 0; i < BinColumnCount; i++)//i列
                {
                    BinSetting binSetting = new BinSetting();//列
                    binSetting.Name = textBoxes[i].Text;
                    for (int j = 0; j < dgv_binCollectionEditor.Rows.Count; j++)//循环行
                    {

                        defaultValueMin = -9999;
                        defaultValueMax = 9999;
                        BinJudgeItem binJudgeItem = new BinJudgeItem();
                        binJudgeItem.Name = (dgv_binCollectionEditor.Rows[j].Tag as ComboBox).Text;
                        binJudgeItem.Visible = true;
                        object dgvcheckboxValue = ((DataGridViewCheckBoxCell)dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i]]).Value;
                        if (dgvcheckboxValue == null)
                        {
                            binJudgeItem.Enable = false;
                        }
                        else
                        {
                            binJudgeItem.Enable = (bool)dgvcheckboxValue;
                        }

                        //如果没有填写值  我们默认初始值-9999 9999
                        if (dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i] + 1].Value == null)//为不动
                        {
                            binJudgeItem.LL = minValue;
                        }
                        else if (dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i] + 1].Value.ToString() == string.Empty)
                        {
                            binJudgeItem.LL = minValue;
                        }
                        else if (!double.TryParse(dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i] + 1].Value.ToString(), out defaultValueMin))
                        {
                            MessageBox.Show($"保存失败:\n表格  [ 行:{j + 1}  列:{checkboxIndex[i] + 2} ]  中有无法识别的文字，无法保存");
                            return;
                        }
                        else
                        {
                            binJudgeItem.LL = defaultValueMin;
                        }


                        if (dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i] + 2].Value == null)
                        {
                            binJudgeItem.UL = maxValue;
                        }
                        else if (dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i] + 2].Value.ToString() == string.Empty)
                        {
                            binJudgeItem.UL = maxValue;
                        }
                        else if (!double.TryParse(dgv_binCollectionEditor.Rows[j].Cells[checkboxIndex[i] + 2].Value.ToString(), out defaultValueMax))
                        {
                            MessageBox.Show($"保存失败:\n表格  [ 行:{j + 1}  列:{checkboxIndex[i] + 3} ]  中有无法识别的文字，无法保存");
                            return;
                        }
                        else
                        {
                            binJudgeItem.UL = defaultValueMax;
                        }
                        binJudgeItem.Unit = binParam_Unit[binJudgeItem.Name];
                        binSetting.AddSingleItem(binJudgeItem);

                    }
                    binSettingCollection.AddSingleItem(binSetting);
                }
                binSettingCollection.Name = cb_BinSettingCollectionToEdit.Text;
                string errorMsg = string.Empty;
                _myBinSortListManager.DeleteBinSettingCollection_Of_MasterList(binSettingCollection.Name, ref errorMsg);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    MessageBox.Show(errorMsg);
                    return;
                }
                _myBinSortListManager.AddBinSettingCollection_Of_MasterList(binSettingCollection);
                _myBinSortListManager.Save_Collection();
                MessageBox.Show("保存成功");
                List<string> names = _myBinSortListManager.GetBinSettingCollectionNames();
                if (names.Count > 0)
                {
                    cb_BinSettingCollectionToEdit.Items.Clear();
                    cb_BinSettingCollectionToEdit.Items.AddRange(names.ToArray());
                    cb_BinSettingCollectionToEdit.Text = binSettingCollection.Name;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败:\n{ex.Message}");
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show($"是否删除Bin规格{cb_BinSettingCollectionToEdit.Text}", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                }
                if (cb_BinSettingCollectionToEdit.Items.Count < 1)
                {
                    MessageBox.Show("没有对应的Bin规格，无法删除");
                    return;
                }
                if (string.IsNullOrEmpty(cb_BinSettingCollectionToEdit.Text))
                {
                    MessageBox.Show("名称为空，无法删除");
                    return;
                }
                string binSetCollectionName = cb_BinSettingCollectionToEdit.SelectedItem.ToString();
                var binSetCollection = _myBinSortListManager.GetBinSettingCollection(binSetCollectionName);
                if (binSetCollection == null)
                {
                    MessageBox.Show($"未找到对应的规格 binSetting = {binSetCollectionName}!");
                    UpdateSpecsSelector();
                    return;
                }
                string errorMsg = string.Empty;
                _myBinSortListManager.DeleteBinSettingCollection_Of_MasterList(binSetCollection, ref errorMsg);
                _myBinSortListManager.Save_Collection();
                if (string.IsNullOrEmpty(errorMsg))
                {
                    MessageBox.Show("删除成功");
                }
                else
                {
                    MessageBox.Show($"删除出错\n ErrorMsg:{errorMsg}");
                }

                //全清空   不要了
                UpdateSpecsSelector();
                this.dgv_binCollectionEditor.Rows.Clear();
                this.panelComboBox.Controls.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgv_binCollectionEditor_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                string ClipboardText = Clipboard.GetText();
                if (string.IsNullOrEmpty(ClipboardText))
                {
                    return;
                }
                InsertValueInDgv(dgv_binCollectionEditor, ClipboardText);
            }
            if (e.KeyCode == Keys.Delete)
            {
                deleteValueInDgv(dgv_binCollectionEditor);
            }
        }
        private void InsertValueInDgv(DataGridView dataGridView, string txt)
        {
            //解析数据
            string newtxt = txt.Replace("\r\n", "|");
            string[] rowstringCollection = newtxt.Split('|');
            List<List<string>> allStringList = new List<List<string>>();

            for (int i = 0; i < rowstringCollection.Length; i++)
            {
                string[] eachRow = rowstringCollection[i].Split('\t');
                List<string> rowStringList = new List<string>();
                for (int j = 0; j < eachRow.Length; j++)
                {
                    rowStringList.Add(eachRow[j]);
                }
                allStringList.Add(rowStringList);
            }


            //获取当前的单元格
            int currentColumnIndex = dataGridView.CurrentCell.ColumnIndex;
            int currentRowIndex = dataGridView.CurrentCell.RowIndex;
            int pasteIndex = (currentColumnIndex + 1) % 3;//1 选择    2 0 都是文本


            if (allStringList[0].Count == 1)//复制单列
            {
                //获取复制的是什么类型
                if (allStringList[0][0].ToLower() == "true" || allStringList[0][0].ToLower() == "false")
                {
                    if (pasteIndex == 0 || pasteIndex == 2)
                    {
                        MessageBox.Show("单元格不同无法复制");
                        return;
                    }
                }
                else
                {
                    if (pasteIndex == 1)
                    {
                        MessageBox.Show("单元格不同无法复制");
                        return;
                    }
                }
            }
            else if (allStringList[0].Count == 2)
            {
                int copyStartIndex = 0;
                if (allStringList[0][0].ToLower() == "true" || allStringList[0][0].ToLower() == "false")
                {
                    copyStartIndex = 1;
                }
                else if (allStringList[0][1].ToLower() == "true" || allStringList[0][1].ToLower() == "false")
                {
                    copyStartIndex = 0;
                }
                else
                {
                    copyStartIndex = 2;//第二列
                }

                if (pasteIndex != copyStartIndex)
                {
                    MessageBox.Show("单元格不同无法复制");
                    return;
                }
            }
            else if (allStringList[0].Count >= 3)
            {
                int copyStartIndex = 0;
                if (allStringList[0][0].ToLower() == "true" || allStringList[0][0].ToLower() == "false")
                {
                    copyStartIndex = 1;
                }
                else if (allStringList[0][1].ToLower() == "true" || allStringList[0][1].ToLower() == "false")
                {
                    copyStartIndex = 3;
                }
                else if (allStringList[0][2].ToLower() == "true" || allStringList[0][2].ToLower() == "false")
                {
                    copyStartIndex = 2;//第二列
                }

                if (pasteIndex != copyStartIndex)
                {
                    MessageBox.Show("单元格不同无法复制");
                    return;
                }
            }

            //找出复制的限制   //1+3
            int columnMax = (allStringList[0].Count + currentColumnIndex) < dataGridView.ColumnCount ?
                (allStringList[0].Count + currentColumnIndex) : dataGridView.ColumnCount;
            int rowMax = (allStringList.Count + currentRowIndex) < dataGridView.RowCount ?
                (allStringList.Count + currentRowIndex) : dataGridView.RowCount;

            for (int i = currentRowIndex; i < rowMax; i++)
            {
                for (int j = currentColumnIndex; j < columnMax; j++)
                {
                    dataGridView.Rows[i].Cells[j].Value = allStringList[i - currentRowIndex][j - currentColumnIndex];
                }
            }
        }

        private void deleteValueInDgv(DataGridView dataGridView)
        {
            DataGridViewSelectedCellCollection cells = dataGridView.SelectedCells;
            foreach (var item in cells)
            {
                if (item as DataGridViewCheckBoxCell == null)
                {
                    (item as DataGridViewTextBoxCell).Value = "";
                }
                else
                {
                    (item as DataGridViewCheckBoxCell).Value = false;
                }
            }
        }

        private void btn_SetCurrentBinCollectionToTestProfile_Click(object sender, EventArgs e)
        {
            if (cb_BinSettingCollectionToEdit.SelectedItem != null)
            {
                this._myBinSortListManager.SetBinCollectionToApplication?.Invoke(cb_BinSettingCollectionToEdit.SelectedItem.ToString());
                MessageBox.Show($"更新{cb_BinSettingCollectionToEdit.SelectedItem.ToString()}到测试项完成");
            }
        }
    }
}
