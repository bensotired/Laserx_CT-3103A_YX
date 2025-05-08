using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Specification;
using SolveWare_TestComponents.UIComponents;
using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    public partial class Form_TestProfileEditor_CT3103 : Form_TestEnterance_TestPlugin<TestPluginWorker_CT3103>
    {
        Form _binCollectionListUI;
        const string EMPTY_COMBO_NODE = "未指定任何测试项目链";
        const string EMPTY_PROFILE_STATUS = "当前编辑的测试方案:[- - ]";

        public Form_TestProfileEditor_CT3103()
        {
            InitializeComponent();
            //this.tab_ComboToProfile.Parent = null;
            this.tab_SpecToProfile.Parent = null;

            this.tab_CalibrationToProfile.Parent = null;
            this.tab_BinToProfile.Parent = null;
        }
        private void Form_TestProfileEditor_CT3103_Load(object sender, EventArgs e)
        {
            this._editTestProfile = new TestPluginImportProfile_CT3103();
            this._editTestProfile.CreateDefaultCalibrationData();

            _binCollectionListUI = this._plugin.GetBinSortEditorUI();
            pnl_binSort.Controls.Clear();
            pnl_binSort.Controls.Add(_binCollectionListUI);
            _binCollectionListUI.Show();

            this._plugin.LocalResource.Local_BinSortList_ResourceProvider.SetBinCollectionToApplication = null;
            this._plugin.LocalResource.Local_BinSortList_ResourceProvider.SetBinCollectionToApplication = UpdateTestProfileBinCollectionName;

        }
        public void UpdateTestProfileBinCollectionName(string binCollectionName)
        {
            try
            {
                _editTestProfile.BinCollectionName = binCollectionName;
                this.UpdateTestProfileToUI(this._editTestProfile);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新测试方案[{this._editTestProfile.Name}]Bin设置失败!");
            }

        }

        Dictionary<string, string> _LocalExecutorComboFileDict = new Dictionary<string, string>();
        TestPluginImportProfile_CT3103 _editTestProfile { get; set; }
        public override void RefreshOnce()
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    RefreshComboFiles();
                    RefreshSpecSelector();
                    RefreshTestProfileFiles();
                    this.ClearTreeView(this.treeView_TestExecutor_1);
                }));
            }
            catch (Exception ex)
            {

            }
        }
        private void ClearTestProfileStatus()
        {
            tssl_currentEditingTestProfile.Text = EMPTY_PROFILE_STATUS;
        }
        private void RefreshSpecSelector()
        {
            cb_SpecToUse.Items.Clear();
            var specTags = this._plugin.LocalResource.Local_Spec_ResourceProvider.GetSpecificationTags();
            cb_SpecToUse.Items.AddRange(specTags.ToArray());
            //if (specTags.Count > 0)
            //{
            //    cb_SpecToUse.SelectedIndex = 0;
            //}
        }
        private void RefreshComboFiles()
        {
            cb_localTestExecutorComboFiles.Items.Clear();
            _LocalExecutorComboFileDict = this._core.GetLocalExecutorComboFiles();
            cb_localTestExecutorComboFiles.Items.AddRange(_LocalExecutorComboFileDict.Keys.ToArray());
            if (_LocalExecutorComboFileDict.Count > 0)
            {
                cb_localTestExecutorComboFiles.SelectedIndex = 0;
            }
        }
        protected virtual void RefreshTestProfileFiles()
        {
            try
            {
                var fileDict = this._core.GetLocalTestProfileFiles();
                this._core.RefreshListView(this.lv_TestProfileFiles, fileDict);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RefreshComboFiles错误:[{ex.Message}+{ex.StackTrace}]!");
            }
        }
        private void UpdateTreeView(TreeView tv, object comboInstance)
        {
            var comboNode = this._core.Convert_TestExecutorComboToTreeNode(comboInstance);
            tv.Nodes.Clear();
            tv.Nodes.Add(comboNode);
            tv.ExpandAll();
        }
        private void ClearTreeView(TreeView tv)
        {
            tv.Nodes.Clear();
            tv.Nodes.Add(EMPTY_COMBO_NODE);

        }
        private void Form_TestProfileEditor_CT3103_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                RefreshOnce();

            }
        }

        private void btn_UpdateComboToExecutor_1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.cb_localTestExecutorComboFiles.SelectedItem == null)
                {
                    MessageBox.Show("未选择任何测试项目链!");
                    return;
                }
                var comboFileShortName = this.cb_localTestExecutorComboFiles.SelectedItem.ToString();
                if (string.IsNullOrEmpty(comboFileShortName))
                {
                    MessageBox.Show("未选择任何测试项目链!");
                    return;
                }
                if (this._LocalExecutorComboFileDict.ContainsKey(comboFileShortName) == false)
                {
                    MessageBox.Show($"未找到测试项目链[{comboFileShortName}]对应文件!");
                    return;
                }
                var filePath = this._LocalExecutorComboFileDict[comboFileShortName];

                var comboParamObj = this._core.LoadTestExecutorComboWithParams(filePath);
                if (comboParamObj == null || (comboParamObj is TestExecutorComboWithParams) == false)
                {
                    MessageBox.Show($"未能正确解析测试项目链[{comboFileShortName}]对应文件[{filePath}]!");
                    return;
                }
                this._editTestProfile.AddTestExecutorComboWithParams(MT.测试站1.ToString(), (comboParamObj as TestExecutorComboWithParams));
                UpdateTestProfileToUI(this._editTestProfile);

                //var comboObj = this._core.LoadTestExecutorComboWithParams(filePath);//this._core.LoadTestExecutorCombo(filePath);
                //if (comboObj == null || (comboObj is TestExecutorCombo) == false)
                //{
                //    MessageBox.Show($"未能正确解析测试项目链[{comboFileShortName}]对应文件[{filePath}]!");
                //    return;
                //}
                //this._editTestProfile.AddComboToExecutorDict(this._core, MT.测试站1.ToString(), (TestExecutorCombo)comboObj);
                //UpdateTestProfileToUI(this._editTestProfile);



            }
            catch (Exception ex)
            {

            }
        }

        private void btn_ClearExecutor_1_Click(object sender, EventArgs e)
        {
            this._editTestProfile.TestExecutorComboDict[MT.测试站1.ToString()] = null;
            ClearTreeView(this.treeView_TestExecutor_1);

        }

        private void cb_SpecToUse_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                var specTag = cb_SpecToUse.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(specTag))
                {
                    return;
                }
                var specObject = this._plugin.LocalResource.Local_Spec_ResourceProvider.GetSpecResource(specTag);

                if (specObject == null)
                {
                    MessageBox.Show($"未找到对应的规格 SpecTag = {specTag}!");
                    return;
                }
                this.pdgv_specEditor.ImportSourceData((TestSpecification)specObject, true);
                this._editTestProfile.Spec = (TestSpecification)specObject;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "提示");
            }
        }

        private void btn_saveTestProfile_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._editTestProfile == null)
                {
                    MessageBox.Show("没有正在编辑的测试方案!");
                    return;
                }
                //var SpecStr = tb_CurrentProfileSpecTag.Text?.ToString();
                //if (string.IsNullOrEmpty(SpecStr))
                //{
                //    MessageBox.Show("请为测试方案配置测试规格!");
                //    return;
                //}
                //if (string.IsNullOrEmpty(this._editTestProfile.BinCollectionName ) == true)
                //{
                //    MessageBox.Show("请为测试方案配置Bin参数!");
                //    return;
                //}
                //if (this.CheckCalibrationDataUI(this.dgv_CalibrationData_PosLoaderToProfile) == false )
                //{
                //    MessageBox.Show("无效的校准系数!");
                //    return; 
                //}



                SaveFileDialog sfd = new SaveFileDialog();

                var prodDir = this._core.GetProductConfigFileDirectory();

                var fileDir = _core.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_PROFILE_PATH);

                sfd.InitialDirectory = Path.GetFullPath($@"{prodDir}\{fileDir}");
                var defaultFileName = string.Concat(this._editTestProfile.Name, FileExtension.XML);
                sfd.FileName = defaultFileName;
                var dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {

                    string finalFileName = sfd.FileName;
                    finalFileName = Path.ChangeExtension(finalFileName, "xml");
                    var finalFileShortName = Path.GetFileName(finalFileName);
                    if (finalFileShortName.Equals(defaultFileName))
                    {
                        foreach (var kvp in _TestAndCalcParamDict)
                        {
                            kvp.Value.Update();
                        }
                        this.SaveCalibrationDataFromUI(this.dgv_CalibrationData_PosLoaderToProfile,   this._editTestProfile.UserDefinedCalibrationData_Loader);
                        this._editTestProfile.Save(finalFileName);
                        MessageBox.Show("保存成功:\r\n\r\n" + finalFileName);
                        //刷新
                        //this.RefreshTestProfileFiles();
                        this.RefreshTestProfileFiles();
                        //tssl_currentEditingTestProfile.Text = $"当前编辑的测试方案:[{this._editTestProfile.Name}]";
                    }
                    else
                    {
                        var finalExeComboName = Path.GetFileNameWithoutExtension(finalFileShortName);
                        var renameDR = MessageBox.Show($"是否将测试方案重命名为[{finalExeComboName}]?" +
                              $"\r\n提示:文件名与测试项名必须保持一致" +
                              $"\r\n若选是 则将测试方案重命名为[{finalExeComboName}]并存储." +
                              $"\r\n若选否 则放弃保存.", "保存提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);

                        if (renameDR == DialogResult.Yes)
                        {
                            foreach (var kvp in _TestAndCalcParamDict)
                            {
                                kvp.Value.Update();
                            }
                            this.SaveCalibrationDataFromUI(this.dgv_CalibrationData_PosLoaderToProfile, this._editTestProfile.UserDefinedCalibrationData_Loader);
                            this._editTestProfile.Name = finalExeComboName;
                            this._editTestProfile.Save(finalFileName);
                            MessageBox.Show("保存成功:\r\n\r\n" + finalFileName);
                            //RefreshComboFiles();
                            //this.UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
                            this.RefreshTestProfileFiles();
                            //tssl_currentEditingTestProfile.Text = $"当前编辑的测试方案:[{this._editTestProfile.Name}]";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败:[{ex.Message}+{ex.StackTrace}]!");
            }
        }

        private void lv_TestProfileFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.lv_TestProfileFiles.SelectedItems.Count == 1)
                {
                    var filePath = lv_TestProfileFiles.SelectedItems[0].Tag.ToString();

                    if (File.Exists(filePath))
                    {
                        this._editTestProfile = (TestPluginImportProfile_CT3103)this._core.LoadTestProfileWithExtraTypes<TestPluginImportProfile_CT3103>(filePath);
                        this.UpdateTestProfileToUI(this._editTestProfile);
                    }
                    else
                    {
                        MessageBox.Show($"所选测试方案不存在!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载错误:{ex.Message}-{ex.StackTrace}!");
            }
        }

        private void btn_createNewTestProfile_Click(object sender, EventArgs e)
        {
            try
            {
                this._editTestProfile = new TestPluginImportProfile_CT3103();
                UpdateTestProfileToUI(this._editTestProfile);
                //tssl_currentEditingTestProfile.Text = $"当前编辑的测试方案:[{this._editTestProfile.Name}]";

                tssl_currentEditingTestProfile.Text = $"当前编辑的测试方案:[{_editTestProfile.Name}] ";//Bin设置[{_editTestProfile.BinCollectionName}]";

            }
            catch
            {

            }
        }
        private void UpdateTestProfileToUI(TestPluginImportProfile_CT3103 profile)
        {
            try
            {
                //throw new NotImplementedException("需要做UI了！！");

                tssl_currentEditingTestProfile.Text = $"当前编辑的测试方案:[{profile.Name}] ";//Bin设置[{_editTestProfile.BinCollectionName}]";
                if (profile.TestExecutorComboDict[MT.测试站1.ToString()] != null)
                {
                    UpdateTreeView(this.treeView_TestExecutor_1, profile.TestExecutorComboDict[MT.测试站1.ToString()]);
                }
                else
                {
                    ClearTreeView(this.treeView_TestExecutor_1);
                }
                if (profile.Spec != null)
                {
                    this.tb_CurrentProfileSpecTag.Text = profile.Spec.SpecTag;
                    this.pdgv_specEditor.ImportSourceData(profile.Spec, true);
                }
                else
                {
                    this.tb_CurrentProfileSpecTag.Text = "未配置任何测试规格";
                    this.pdgv_specEditor.Clear();
                }
                if (profile.TestParamsConfigComboDict[MT.测试站1.ToString()] != null)
                {
                    UpdateTestParamEditorLayerView(profile.TestParamsConfigComboDict[MT.测试站1.ToString()]);
                }
                else
                {
                    ClearTestParamEditorLayerView();
                }
                if (profile.UserDefinedCalibrationData_Loader?.Count <= 0)
                {
                    profile.CreateDefaultCalibrationData();
                }
                this.UpdateCalibrationDataUI(this.dgv_CalibrationData_PosLoaderToProfile, this._editTestProfile.UserDefinedCalibrationData_Loader);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新界面测试方案信息错误:[{ex.Message}]-[{ex.StackTrace}]!");
            }
        }
        Dictionary<string, Form_TestAndCalcParamEditor> _TestAndCalcParamDict = new Dictionary<string, Form_TestAndCalcParamEditor>();
        private void ClearTestParamEditorLayerView()
        {
            try
            {
                if (_TestAndCalcParamDict.Count > 0)
                {
                    foreach (var oldFrm in _TestAndCalcParamDict.Values)
                    {
                        oldFrm.Close();
                        oldFrm.Dispose();
                    }
                }
                _TestAndCalcParamDict.Clear();

                tab_TestParamEditorLayer.TabPages.Clear();
            }
            catch (Exception ex)
            {
            }
        }
        private void UpdateTestParamEditorLayerView(ExecutorConfigItem_TestParamsConfigCombo paramsCombo)
        {
            try
            {

                if (_TestAndCalcParamDict.Count > 0)
                {
                    foreach (var oldFrm in _TestAndCalcParamDict.Values)
                    {
                        oldFrm.Close();
                        oldFrm.Dispose();
                    }
                }
                _TestAndCalcParamDict.Clear();

                tab_TestParamEditorLayer.TabPages.Clear();

                //var paramsCombo = this._editTestProfile.TestParamsConfigComboDict[MT.PD测试模组1.ToString()];

                foreach (var testParamConfig in paramsCombo.Pre_TestParamsCollection)
                {
                    TabPage tp = new TabPage(testParamConfig.Name);

                    Form_TestAndCalcParamEditor frm = new Form_TestAndCalcParamEditor();
                    this._core.ModifyDockableUI(frm, true);
                    frm.Import(testParamConfig);
                    frm.Show();

                    tp.Controls.Add(frm);

                    _TestAndCalcParamDict.Add(testParamConfig.Name, frm);

                    tab_TestParamEditorLayer.TabPages.Add(tp);
                }
                foreach (var testParamConfig in paramsCombo.Main_TestParamsCollection)
                {
                    TabPage tp = new TabPage(testParamConfig.Name);

                    Form_TestAndCalcParamEditor frm = new Form_TestAndCalcParamEditor();
                    this._core.ModifyDockableUI(frm, true);
                    frm.Import(testParamConfig);
                    frm.Show();

                    tp.Controls.Add(frm);

                    _TestAndCalcParamDict.Add(testParamConfig.Name, frm);

                    tab_TestParamEditorLayer.TabPages.Add(tp);
                }
                foreach (var testParamConfig in paramsCombo.Post_TestParamsCollection)
                {
                    TabPage tp = new TabPage(testParamConfig.Name);

                    Form_TestAndCalcParamEditor frm = new Form_TestAndCalcParamEditor();
                    this._core.ModifyDockableUI(frm, true);
                    frm.Import(testParamConfig);
                    frm.Show();

                    tp.Controls.Add(frm);

                    _TestAndCalcParamDict.Add(testParamConfig.Name, frm);

                    tab_TestParamEditorLayer.TabPages.Add(tp);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void UpdateCalibrationDataUI( DataGridView dgv ,    List<CalibrationItem>  sourceData)
        {
            //this.dgv_CalibrationData_PosLoaderToProfile.Rows.Clear();
            //foreach (var item in sourceData)
            //{
            //    this.dgv_CalibrationData_PosLoaderToProfile.Rows.Add(item.Name, item.K, item.B);
            //}
            dgv.Rows.Clear();
            foreach (var item in sourceData)
            {
                dgv.Rows.Add(item.Name, item.K, item.B);
            }
        }
        public void ClearCalibrationDataUI()
        {
            this.dgv_CalibrationData_PosLoaderToProfile.Rows.Clear();
        }
        public bool CheckCalibrationDataUI(DataGridView dgv )
        {
            bool isok = true;
            const int name_ColIndex = 0;
            const int kVal_ColIndex = 1;
            const int bVal_ColIndex = 2;
            try
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    var cDataName = row.Cells[name_ColIndex].Value.ToString();
                    var kVal = row.Cells[kVal_ColIndex].Value?.ToString();
                    var bVal = row.Cells[bVal_ColIndex].Value?.ToString();
                    double k_dblVal = 0.0;
                    double b_dblVal = 0.0;
                    if (double.TryParse(kVal, out k_dblVal))
                    {

                    }
                    else
                    {
                        isok = false;
                    }
                    if (double.TryParse(bVal, out b_dblVal))
                    {

                    }
                    else
                    {
                        isok = false;
                    }
                }
            }
            catch
            {
                isok = false;
            }

            return isok;
        }

        public void SaveCalibrationDataFromUI( DataGridView dgv ,  List<CalibrationItem> sourceData)
        {
            const int name_ColIndex = 0;
            const int kVal_ColIndex = 1;
            const int bVal_ColIndex = 2;
            try
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    var cDataName = row.Cells[name_ColIndex].Value.ToString();
                    var kVal = row.Cells[kVal_ColIndex].Value?.ToString();
                    var bVal = row.Cells[bVal_ColIndex].Value?.ToString();
                    if (sourceData.Exists(item => item.Name == cDataName))
                    {
                        var index = sourceData.FindIndex(item => item.Name == cDataName);
                        sourceData[index].K = Convert.ToDouble(kVal);
                        sourceData[index].B = Convert.ToDouble(bVal);
                    }
                    else
                    {
                        sourceData.Add(new CalibrationItem
                        {
                            Name = cDataName,
                            K = Convert.ToDouble(kVal),
                            B = Convert.ToDouble(bVal),
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"保存校准数据失败:[{ex.Message}][{ex.StackTrace}]!");
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lv_TestProfileFiles.SelectedItems.Count == 1)
                {
                    var dr = MessageBox.Show(
                                         $"确定删除{ lv_TestProfileFiles.SelectedItems[0].Text}?",
                                         "删除操作",
                                         MessageBoxButtons.YesNoCancel,
                                         MessageBoxIcon.Question
                                     );
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                    var filePath = lv_TestProfileFiles.SelectedItems[0].Tag.ToString();
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                var fileDict = this._core.GetLocalTestProfileFiles();
                this._core.RefreshListView(this.lv_TestProfileFiles, fileDict);
            }
            catch
            {

            }
        }
    }
}