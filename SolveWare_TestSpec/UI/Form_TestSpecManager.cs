using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Windows.Forms;
using SolveWare_TestComponents.Specification;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;

namespace SolveWare_TestSpecification
{
    public partial class Form_TestSpecManager : Form, ITesterAppUI, IAccessPermissionLevel
    {

        public Form_TestSpecManager()
        {
            InitializeComponent();
        }

        ITesterCoreInteration _core;
        ITesterAppPluginInteration _appInteration;

        //_myMotionManager.Config=TestSpecManagerConfig.Instance=TestSpecManagerConfig(实例化)
        //将所有信息最终存储到TestSpecManagerConfig(实例化)中
        TestSpecManager _mySpecManager;

        //从文件中读取、或者已保存到文件中的测试规格集合
        TestSpecCollection tempSpecList;

        //需要进行操作的测试规格
        TestSpecification tempSpec;

        public void RefreshOnce()
        {

        }
        public AccessPermissionLevel APL
        {
            get { return AccessPermissionLevel.None; }
        }

        #region 平台交互UI及信息

        public void ConnectToAppInteration(ITesterAppPluginInteration app)
        {
            _appInteration = app;
            _mySpecManager = (TestSpecManager)app;
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
            tempSpecList = new TestSpecCollection(_mySpecManager.GetTestSpecCollection());

        }

        #region 界面更新

        private void UpdateSpecsSelector()
        {
            var orgSelectItem = cb_SpecToEdit.SelectedItem;
            //重新加载所有的轴的名称，轴的名称来源_myMotionManager.Config这个变量
            cb_SpecToEdit.Items.Clear();

            //var specNames = _mySpecManager.Config.TestSpecCollection.GetDataListByPropName<string>("SpecTag");
            var specNames = _mySpecManager.GetSpecificationTags();
            foreach (var sName in specNames)
            {
                cb_SpecToEdit.Items.Add(sName);
            }
        }

        private void cb_SpecToEdit_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                string specTag = string.Empty;
                specTag = cb_SpecToEdit.SelectedItem.ToString();
                if (tempSpec != null)
                {//dgv有内容
                    //specTag = cb_SpecToEdit.SelectedItem.ToString();
                    if (IsExistInXML(tempSpec))
                    { //文件中存在
                        var rs = IsEqualDgvValuesAndSpcValues(tempSpec, this.pdgv_specEditor);
                        if (rs == false)
                        {//数据变动
                            var dr = MessageBox.Show("当前界面数据发生变动，是否保存？", "提示", MessageBoxButtons.OKCancel);
                            if (dr == DialogResult.OK)
                            {//保存
                                try
                                {
                                    var pdgvSpec = GetDataFromPdgv(tempSpec);
                                    tempSpec = pdgvSpec;
                                    SaveAndUpdate(tempSpec);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {//文件中不存在
                        var dr1 = MessageBox.Show("当前界面展示为新建临时测试规格，是否保存新建项", "提示", MessageBoxButtons.OKCancel);
                        if (dr1 == DialogResult.OK)
                        {//保存
                            try
                            {
                                var pdgvSpec = GetDataFromPdgv(tempSpec);
                                tempSpec = pdgvSpec;
                                SaveAndUpdate(tempSpec);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
                                return;
                            }
                        }
                        else
                        {//从缓存中删除
                            try
                            {
                                string msg = string.Empty;
                                var result = _mySpecManager.DeleteTestSpecFromCollection(tempSpec.ID, ref msg);
                                if (!result)
                                {
                                    throw new Exception(msg);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
                                return;
                            }
                        }
                    }
                    this.pdgv_specEditor.Clear();

                }
                //var spec = _mySpecManager.Config.TestSpecCollection.GetSpecByTag(specTag);
                var spec = _mySpecManager.GetTestSpecByTag(specTag);
                if (spec == null)
                {
                    MessageBox.Show($"未找到对应的规格 SpecTag = {specTag}!");
                    return;
                }
                this.pdgv_specEditor.ImportSourceData<TestSpecificationItem>(spec, true);
                this.cb_SpecToEdit.Text = spec.SpecTag;
                tempSpec = new TestSpecification();
                tempSpec = spec;

            }
            catch (Exception ex)
            {
                this.cb_SpecToEdit.Text = tempSpec?.SpecTag;
                MessageBox.Show(ex.Message, "提示");
            }
        }
        #endregion


        #region 添加空白测试规格

        private void btn_addSpec_Click(object sender, EventArgs e)
        {
            if (tempSpec != null)
            {//dgv中有数据
                if (IsExistInXML(tempSpec))
                { //文件中存在
                    var rs = IsEqualDgvValuesAndSpcValues(tempSpec, this.pdgv_specEditor);
                    if (rs == false)
                    {//数据变动
                        var dr = MessageBox.Show("当前界面数据发生变动，是否保存？", "提示", MessageBoxButtons.OKCancel);
                        if (dr == DialogResult.OK)
                        {//保存
                            try
                            {
                                var pdgvSpec = GetDataFromPdgv(tempSpec);
                                tempSpec = pdgvSpec;
                                SaveAndUpdate(tempSpec);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
                                return;
                            }
                        }
                    }
                }
                else
                {//文件中不存在
                    var dr1 = MessageBox.Show("当前界面展示为新建临时测试规格，是否保存新建项", "提示", MessageBoxButtons.OKCancel);
                    if (dr1 == DialogResult.OK)
                    {//保存
                        try
                        {
                            var pdgvSpec = GetDataFromPdgv(tempSpec);
                            tempSpec = pdgvSpec;
                            SaveAndUpdate(tempSpec);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
                            return;
                        }

                    }
                    else
                    {//从缓存中删除
                        try
                        {
                            string msg = string.Empty;
                            var result = _mySpecManager.DeleteTestSpecFromCollection(tempSpec.ID, ref msg);
                            if (!result)
                            {
                                throw new Exception(msg);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
                            return;
                        }
                    }
                }
                this.cb_SpecToEdit.SelectedIndex = -1;
                this.cb_SpecToEdit.Text = string.Empty;
                this.pdgv_specEditor.Clear();
            }

            try
            {//dgv中无数据
                AddNewSpec();
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
            }
        }


        private void AddNewSpec()
        {
            TestSpecification newSpec = new TestSpecification();
            DateTime dateTime = DateTime.Now;
            newSpec.Name = "SampleSpec_" + dateTime.ToString("MMddHHmmssss");
            newSpec.Version = dateTime.ToString("yyyyMMdd");

            for (int specIndex = 0; specIndex < 3; specIndex++)
            {
                newSpec.AddSingleItem(new TestSpecificationItem
                {
                    Max = "99",
                    Min = "0",
                    //IsCirtcal = true,
                    Name = $"SampleSpecItem_{specIndex}",
                    FailureCode = $"FC_{specIndex:0000}",
                    Unit = "NA",
                    DataType = SpecDataType.Double,
                });
            }

            List<string> specName = new List<string>();
            foreach (var item in tempSpecList.ItemCollection)
            {
                specName.Add(item.Name);
            }
            Form_TestSpecInfo frm = new Form_TestSpecInfo(newSpec);
            frm.SpecNameList = specName;
            var dr1 = frm.ShowDialog();
            if (dr1 == DialogResult.OK)
            {
                newSpec.Name = frm.TestSpecName;
                newSpec.Version = frm.TestSpecVersion;
            }
            else
            {
                return;
            }
            try
            {
                _mySpecManager.AddTestSpecToCollection(newSpec);
                UpdateSpecsSelector();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            this.cb_SpecToEdit.Text = newSpec.SpecTag;
            this.pdgv_specEditor.ImportSourceData<TestSpecificationItem>(newSpec, true);
            tempSpec = new TestSpecification();
            tempSpec = newSpec;
        }
        #endregion


        #region 删除当前测试规格

        private void btn_deleteSpec_Click(object sender, EventArgs e)
        {
            if (tempSpec == null)
            {
                MessageBox.Show("请选择需要删除的测试规格");
                return;
            }
            var dr = MessageBox.Show("确认删除当前测试规格吗", "提示", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            var isExist = IsExistInXML(tempSpec);
            try
            {
                DelAndUpdate(tempSpec, isExist);
                MessageBox.Show("删除成功！", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
            }

        }
        #endregion


        #region 复制当前测试规格并新增

        private void btn_copySpec_Click(object sender, EventArgs e)
        {
            if (tempSpec == null)
            {
                MessageBox.Show("请选择需要复制的测试规格");
                return;
            }

            if (IsExistInXML(tempSpec))
            { //文件中存在
                var rs = IsEqualDgvValuesAndSpcValues(tempSpec, this.pdgv_specEditor);
                if (rs == false)
                {//数据变动
                    var dr = MessageBox.Show("当前界面数据发生变动，是否保存？", "提示", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)
                    {//保存
                        try
                        {
                            var pdgvSpec = GetDataFromPdgv(tempSpec);
                            tempSpec = pdgvSpec;
                            SaveAndUpdate(tempSpec);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
                            return;
                        }
                    }
                }

            }
            else
            {//文件中不存在
                var dr = MessageBox.Show("当前界面展示为新建临时测试规格，是否保存新建项", "提示", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {//保存
                    try
                    {
                        var pdgvSpec = GetDataFromPdgv(tempSpec);
                        tempSpec = pdgvSpec;
                        SaveAndUpdate(tempSpec);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
                        return;
                    }
                }
                else
                {//从缓存中删除
                    try
                    {
                        string msg = string.Empty;
                        var result = _mySpecManager.DeleteTestSpecFromCollection(tempSpec.ID, ref msg);
                        if (!result)
                        {
                            throw new Exception(msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
                        return;
                    }
                }
            }



            //新建
            TestSpecification newSpec = new TestSpecification();
            DateTime dateTime = DateTime.Now;
            newSpec.Name = "SampleSpec_" + dateTime.ToString("MMddHHmmssss");
            newSpec.Version = dateTime.ToString("yyyyMMdd");
            try
            {
                var pdgvSpec = GetDataFromPdgv(tempSpec);
                newSpec.ItemCollection = pdgvSpec.ItemCollection;

            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
                return;

            }

            List<string> specName = new List<string>();
            foreach (var item in tempSpecList.ItemCollection)
            {
                specName.Add(item.Name);
            }
            Form_TestSpecInfo frm = new Form_TestSpecInfo(newSpec);
            frm.SpecNameList = specName;
            var dr1 = frm.ShowDialog();
            if (dr1 == DialogResult.OK)
            {
                newSpec.Name = frm.TestSpecName;
                newSpec.Version = frm.TestSpecVersion;

                //0418
                //this.pdgv_specEditor.Clear();
                //this.cb_SpecToEdit.Text = newSpec.SpecTag;
                //this.pdgv_specEditor.ImportSourceData<TestSpecificationItem>(newSpec, true);
                //tempSpec = new TestSpecification();
                //tempSpec = newSpec;
                //_mySpecManager.AddTestSpecToCollection(newSpec);
                //0418
            }

            try
            {
                //0418
                _mySpecManager.AddTestSpecToCollection(newSpec);
                //0418

                UpdateSpecsSelector();
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常原因：" + ex.Message, "提示");
            }

            //0418
            this.cb_SpecToEdit.Text = newSpec.SpecTag;
            this.pdgv_specEditor.ImportSourceData<TestSpecificationItem>(newSpec, true);
            tempSpec = new TestSpecification();
            tempSpec = newSpec;

            //0418

        }

        #endregion


        #region 添加空白测试规格项

        private void btn_addSpecItem_Click(object sender, EventArgs e)
        {
            if (tempSpec == null)
            {
                MessageBox.Show("请选择需要添加空白测试规格项的测试规格");
                return;
            }
            try
            {
                this.pdgv_specEditor.Rows.Add();
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加空白测试规格项失败，异常原因：" + ex.Message, "提示");
            }
        }

        #endregion


        #region 删除当前测试规格项

        private void btn_deleteSpecItem_Click(object sender, EventArgs e)
        {
            if (tempSpec == null)
            {
                MessageBox.Show("请选择一项测试规格", "提示");
                return;
            }
            if (this.pdgv_specEditor.RowCount == 0)
            {
                MessageBox.Show("当前测试规格内容为空，无法删除", "提示");
                return;
            }
            int index = this.pdgv_specEditor.CurrentRow.Index;
            if (index == -1)
            {
                MessageBox.Show("请选择需要删除的行", "提示");
                return;
            }
            this.pdgv_specEditor.Rows.RemoveAt(index);
        }

        #endregion


        #region 保存当前测试规格

        private void btn_saveSpec_Click(object sender, EventArgs e)
        {

            if (tempSpec == null)
            {
                MessageBox.Show("请选择需要保存的测试规格");
                return;
            }
            //0418
            tempSpec.ItemCollection.Clear();
            //0418
            try
            {
                var pdgvSpec = GetDataFromPdgv(tempSpec);
                tempSpec = pdgvSpec;
                SaveAndUpdate(tempSpec);
                MessageBox.Show("保存成功！", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存当前测试规格失败，异常原因：" + ex.Message, "提示");
                return;
            }

        }

        #endregion


        #region pdgv获取数据
        private TestSpecification GetDataFromPdgv(TestSpecification newSpec)
        {
            try
            {
                if (this.pdgv_specEditor.RowCount < 1)
                {
                    throw new Exception("当前测试规格无内容");
                }
                newSpec.ItemCollection = new List<TestSpecificationItem>();
                List<int> errorlist = new List<int>();
                bool isOK = true;
                string errMsg = string.Empty;
                //获取数据
                for (int rowIndex = 0; rowIndex < this.pdgv_specEditor.RowCount; rowIndex++)
                {
                    var row = this.pdgv_specEditor.Rows[rowIndex];
                    TestSpecificationItem item = new TestSpecificationItem();
                    //遍历当前dgv的列
                    foreach (DataGridViewColumn dgvCol in this.pdgv_specEditor.Columns)
                    {
                        //之前存下来的属性Tag可以反向转化
                        var colProp = dgvCol.Tag as PropertyInfo;
                        //列号
                        var colIndex = dgvCol.Index;
                        //设置对象的属性值(对象+值)
                        colProp.SetValue(item, Converter.ConvertObjectTo(row.Cells[dgvCol.Index].Value, colProp.PropertyType));
                    }
                    if (string.IsNullOrEmpty(item.Name) || item.Name?.Trim().Length == 0)
                    {//名称为空
                        isOK = false;
                        errorlist.Add(rowIndex + 1);
                    }
                    else
                    {//不为空
                     //添加同时判断重名
                        newSpec.AddSingleItem(item);
                    }
                }
                //判断整个规格数据有效性
                if (!isOK)
                {
                    foreach (var item in errorlist)
                    {
                        errMsg += $"第{item}行、";
                    }
                    errMsg += "测试规格项无效！";
                    errMsg = errMsg.Insert(0, $"测试规格{newSpec.SpecTag}的");
                    throw new Exception(errMsg);
                }
                if (!newSpec.Check(out errMsg))
                {
                    throw new Exception(errMsg);
                }
                return newSpec;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion



        //判断是否改动

        //采集表中数据    行、列，值Dictionary<int, Dictionary<string, object>>
        //列与值，构成   TestSpecificationItem
        //行，是TestSpecificationItem的单体

        //将当前操作TestSpecification，中的List<TestSpecificationItem>()
        //变为行+单体对象，，即Dictionary<int, TestSpecificationItem>

        //分别比对行数，以及TestSpecificationItem

        //比对完后在进行检查数据有效性


        public Dictionary<int, TestSpecificationItem> GetKeyValuePairs(TestSpecification spec)
        {
            Dictionary<int, TestSpecificationItem> listSourseDict = new Dictionary<int, TestSpecificationItem>();
            if (spec.ItemCollection.Count < 1)
            {
                return listSourseDict;
            }

            for (int index = 0; index < spec.ItemCollection.Count; index++)
            {
                listSourseDict.Add(index, spec.ItemCollection[index]);
            }
            return listSourseDict;
        }

        public Dictionary<int, Dictionary<string, object>> GetDataFromDGV(DataGridView dgv)
        {
            Dictionary<int, Dictionary<string, object>> listDict = new Dictionary<int, Dictionary<string, object>>();


            if (dgv.Rows.Count <= 0)
            {
                return listDict;
            }
            try
            {
                for (int rIndex = 0; rIndex < dgv.RowCount; rIndex++)
                {
                    Dictionary<string, object> rowDict = new Dictionary<string, object>();
                    foreach (DataGridViewColumn dgvCol in dgv.Columns)
                    {
                        //获取属性
                        var pInfo = dgvCol.Tag as PropertyInfo;
                        var key = pInfo.Name;
                        var cIndex = dgvCol.Index;
                        var pValue = dgv.Rows[rIndex].Cells[cIndex].Value;
                        try
                        {
                            var value = Converter.ConvertObjectTo(pValue, pInfo.PropertyType);
                            rowDict.Add(key, value);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"当前第[{rIndex + 1}]行、第[{cIndex + 1}]列的{key}项输入的数据类型有误！");
                        }

                    }
                    listDict.Add(rIndex, rowDict);
                }
                return listDict;

            }
            catch (Exception ex)

            {
                throw ex;
            }
        }

        public bool CompareDgvValuesAndRecipeValues(object sourceObject, Dictionary<string, object> dict)
        {
            try
            {
                return ReflectionTool.ComparePropertyValues(sourceObject, dict);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 判断DGV展示数据前后，是否变化
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="dgv"></param>
        /// <returns>返回true表示无变化，返回false表示变化</returns>
        public bool IsEqualDgvValuesAndSpcValues(TestSpecification spec, DataGridView dgv)
        {
            try
            {
                //dgv展示前数据
                var listSourseDict = GetKeyValuePairs(spec);
                //dgv展示后数据
                var listDict = GetDataFromDGV(dgv);

                var rs = listSourseDict.Keys.Count.Equals(listDict.Keys.Count);

                if (!rs)
                {//数目不对，不相等
                    return false;
                }

                if (listSourseDict == null && listDict == null)
                {//null即相等
                    return true;
                }

                if (listSourseDict.Keys.Count == 0 && listDict.Keys.Count == 0)
                {//数目为0，相等
                    return true;
                }

                var count = listSourseDict.Keys.Count;

                for (int index = 0; index < count; index++)
                {
                    var sourceObject = listSourseDict[index];
                    var dict = listDict[index];

                    //返回true，表示不相等
                    rs = CompareDgvValuesAndRecipeValues(sourceObject, dict);
                    if (rs)
                    {//rs返回true,表示不相等
                        return false;
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 判断文件中是否存在

        private bool IsExistInXML(TestSpecification spec)
        {
            var specTag = spec.SpecTag;
            var rs = tempSpecList.GetSpecByTag(specTag);
            if (rs != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        #endregion

        #region 保存并更新文件

        private void SaveAndUpdate(TestSpecification spec)
        {
            string msg = string.Empty;
            try
            {
                if (spec.ItemCollection == null)
                {
                    throw new Exception("测试规格内容为空！！！");
                }


                var result = _mySpecManager.UpdateTestSpecInCollection(spec, ref msg);


                _mySpecManager.SaveConfig();
                //更新
                tempSpecList = new TestSpecCollection(_mySpecManager.GetTestSpecCollection());
                UpdateSpecsSelector();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //private void SaveAndUpdate_v1(TestSpecification spec)
        //{
        //    string msg = string.Empty;
        //    try
        //    {
        //        if (spec.ItemCollection == null)
        //        {
        //            throw new Exception("测试规格内容为空！！！");
        //        }
        //        var result = _mySpecManager.UpdateTestSpecInCollection(spec, ref msg);

        //        _mySpecManager.SaveConfig();
        //        //更新
        //        tempSpecList = new TestSpecCollection(_mySpecManager.GetTestSpecCollection());
        //        UpdateSpecsSelector();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        #endregion

        #region 删除并更新文件

        private void DelAndUpdate(TestSpecification spec, bool isExist)
        {
            try
            {
                string msg = string.Empty;
                //文件中无
                var result = _mySpecManager.DeleteTestSpecFromCollection(spec.ID, ref msg);
                if (!result)
                {
                    throw new Exception(msg);
                }
                if (isExist)
                {//文件中有
                    _mySpecManager.SaveConfig();
                    tempSpecList = new TestSpecCollection(_mySpecManager.GetTestSpecCollection());
                }
                //更新文件
                tempSpec = null;
                UpdateSpecsSelector();
                cb_SpecToEdit.Text = string.Empty;
                this.pdgv_specEditor.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion

    }
}
