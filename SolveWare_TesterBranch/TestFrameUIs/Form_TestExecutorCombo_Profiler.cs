using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_TestExecutorCombo_Profiler : Form, ITesterAppUI
    {
        internal enum SelectorPageOwner
        {
            TestRecipe,
            CalcRecipe
        }

        ITesterCoreInteration _core;
        TestFrameManager _appInteration;
        Dictionary<SelectorPageOwner, int[]> SelectorPageRowHeightDict = new Dictionary<SelectorPageOwner, int[]>();
        TestExecutorCombo _editTestExecutorCombo { get; set; }
        public void ConnectToAppInteration(ITesterAppPluginInteration app)
        {
            _appInteration = (TestFrameManager)app;
        }

        public void ConnectToCore(ITesterCoreInteration core)
        {
            _core = core as ITesterCoreInteration;
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core.SendOutFormCoreToGUIEvent += ReceiveMessageFromCore;
        }

        private void ReceiveMessageFromCore(IMessage message)
        {
            //预留消息处理
        }

        public void DisconnectFromCore(ITesterCoreInteration core)
        {
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core = null;
        }
        public Form_TestExecutorCombo_Profiler()
        {
            InitializeComponent();

            SelectorPageRowHeightDict.Add(SelectorPageOwner.TestRecipe, new int[] { 100, 0 });
            SelectorPageRowHeightDict.Add(SelectorPageOwner.CalcRecipe, new int[] { 0, 100 });
        }

        private void Form_CRCombo_Load(object sender, EventArgs e)
        {
            this.SwitchUIOwnerElementTo(SelectorPageOwner.TestRecipe);
            RefreshOnce();
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
        public void RefreshOnce()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((EventHandler)delegate
                {
                    //this.RefreshCRecipeFileSelector();

                    this.RefreshExecutorProfileFilesListView();
                });
            }
            else
            {
                //this.RefreshCRecipeFileSelector();
                this.RefreshExecutorProfileFilesListView();
            }
        }
        protected virtual void RefreshExecutorProfileFilesListView()
        {
            var fileDict = _appInteration.GetLocalExecutorComboFiles();
            this._core.RefreshListView(this.lv_StepComboProfileFiles, fileDict);
        }

        private void ClearSupportedSelector()
        {
            this.dgv_supportedCalcRecipes.Rows.Clear();
            this.dgv_supportedTestRecipes.Rows.Clear();
        }
        private void ClearSelectedContextOverview()
        {
            this.dgv_selectedTestRecipeOverview.Rows.Clear();
            this.dgv_selectedCalcRecipeOverview.Rows.Clear();
        }

        /// <summary>
        /// 预览测试链
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void lv_StepComboProfileFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.lv_StepComboProfileFiles.SelectedItems.Count == 1)
                {
                    var filePath = lv_StepComboProfileFiles.SelectedItems[0].Tag.ToString();

                    if (File.Exists(filePath))
                    {
                        this._editTestExecutorCombo = this._appInteration.LoadTestExecutorCombo(filePath);
                        this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorCombo);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载错误:{ex.Message}-{ex.StackTrace}!");
            }
        }
        /// <summary>
        /// 预览算子配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CR_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    var fileShortName = this.dgv_supportedCalcRecipes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                    var last_calc_recipeFile = this._core.LoadCalcRecipeInstance(/*this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CALC_RECIPE_PATH),*/ fileShortName);
                    //var last_calc_recipeFile = this._core.LoadInstanceFromFile_ByXmlRoot(this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CALC_RECIPE_PATH), fileShortName);
                    _appInteration.Updatedgv(dgv_selectedCalcRecipeOverview, last_calc_recipeFile, 0, 1, 2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解析算子条件失败[{ex.Message}-{ex.StackTrace}]!");
            }

        }

        private void dgv_supportedTestRecipes_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    var fileShortName = this.dgv_supportedTestRecipes.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    var last_test_recipeFile = this._core.LoadTestRecipeInstance(/*this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CALC_RECIPE_PATH),*/ fileShortName);
                    //var last_test_recipeFile = this._core.LoadInstanceFromFile_ByXmlRoot(this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_RECIPE_PATH), fileShortName);
                    _appInteration.Updatedgv(dgv_selectedTestRecipeOverview, last_test_recipeFile, 0, 1, 2);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("解析失败!");
            }
        }
        /// <summary>
        /// 为算子更新配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddNewCRToCombo_Click(object sender, EventArgs e)
        {
            try
            {
                const int destCalculatorNodeLevel = 4;
                const int destCellCount = 1;
                if (this.dgv_supportedCalcRecipes.SelectedCells?.Count == destCellCount)
                {
                    var treeNode = this.treeView_editExecutorCombo.SelectedNode;
                    if (treeNode == null)
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("选中任何算子!");
                        return;
                    }
                    if (treeNode.Level != destCalculatorNodeLevel)
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("选中任何算子!");
                        return;
                    }
                    if (treeNode.Parent == null)
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("选中任何算子!");
                        return;
                    }
                    if (treeNode.Parent.Parent == null)
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("选中任何算子!");
                        return;
                    }
                    var nodePath = treeNode.FullPath;
                    //拿到测试执行项索引
                    var destTestExecutorIndex = treeNode.Parent.Parent.Index;

                    ExecutorConfigItem destTestExecutorItem = null;
                    if (nodePath.Contains(TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {
                        //拿到测试执行项
                        destTestExecutorItem = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else if (nodePath.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {     //拿到测试执行项
                        destTestExecutorItem = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else if (nodePath.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {     //拿到测试执行项
                        destTestExecutorItem = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("无效的任何算子索引!");
                        return;
                    }


                    ////拿到测试执行项
                    //  destTestExecutorItem = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                    //拿到选中的算子索引
                    var destCalculatorIndex = treeNode.Index;
                    //拿到选中的算子vs算子条件 键值对
                    var destCalculatorKvp = destTestExecutorItem.CalculatorCollection.Book[destCalculatorIndex];

                    var destCalcRecipeFileShortName = this.dgv_supportedCalcRecipes.SelectedCells[0].Value.ToString();
                    var tag = this.dgv_supportedCalcRecipes.SelectedCells[0].Tag;

                    if (tag != null && File.Exists(tag.ToString()))
                    {
                        //this.InitExecutorViewer();
                        var eci = this._core.LoadCalcRecipeInstance(/*this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CALC_RECIPE_PATH),*/ destCalcRecipeFileShortName);
                        //var eci = this._core.LoadInstanceFromFile_ByXmlRoot(this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CALC_RECIPE_PATH), destCalcRecipeFileShortName);
                        if (eci == null)
                        {
                            var msg = $"所选择的算子条件[{destCalcRecipeFileShortName}]不存在!";
                            MessageBox.Show(msg);
                            //this._core.Log_Global(msg);
                            this._appInteration.Log_Global(msg);
                        }
                        else
                        {
                            destCalculatorKvp.Value = destCalcRecipeFileShortName;

                            this.UpdateTreeView_CR_ExecutorCombo(treeView_editExecutorCombo, this._editTestExecutorCombo);
                        }
                    }
                    else
                    {
                        var msg = $"所选择的算子条件[{destCalcRecipeFileShortName}]本地文件不存在!";
                        MessageBox.Show(msg);
                        //this._core.Log_Global(msg);
                        this._appInteration.Log_Global(msg);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新算子条件异常:{ex.Message}-{ex.StackTrace}!");
            }

        }
        private void btn_AddNewTestRcpToCombo_Click(object sender, EventArgs e)
        {
            try
            {
                const int destTestModuleNodeLevel = 3;
                const int destCellCount = 1;
                if (this.dgv_supportedTestRecipes.SelectedCells?.Count == destCellCount)
                {
                    var treeNode = this.treeView_editExecutorCombo.SelectedNode;
                    if (treeNode == null)
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("选中任何测试项目!");
                        return;
                    }
                    if (treeNode.Level != destTestModuleNodeLevel)
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("选中任何测试项目!");
                        return;
                    }

                    var nodePath = treeNode.FullPath;
                    //拿到测试执行项索引
                    var destTestExecutorIndex = treeNode.Parent.Index;

                    ExecutorConfigItem destTestExecutorItem = null;
                    if (nodePath.Contains(TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {
                        //拿到测试执行项
                        destTestExecutorItem = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else if (nodePath.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {     //拿到测试执行项
                        destTestExecutorItem = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else if (nodePath.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {     //拿到测试执行项
                        destTestExecutorItem = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("无效的测试项目索引!");
                        return;
                    }

                    var destTestRecipeFileShortName = this.dgv_supportedTestRecipes.SelectedCells[0].Value.ToString();
                    var tag = this.dgv_supportedTestRecipes.SelectedCells[0].Tag;

                    if (tag != null && File.Exists(tag.ToString()))
                    {
                        //this.InitExecutorViewer();
                        var eci = this._core.LoadTestRecipeInstance(/*this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_RECIPE_PATH),*/ destTestRecipeFileShortName);
                        //var eci = this._core.LoadInstanceFromFile_ByXmlRoot(this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_RECIPE_PATH), destTestRecipeFileShortName);
                        if (eci == null)
                        {
                            var msg = $"所选择的测试条件[{destTestRecipeFileShortName}]不存在!";
                            MessageBox.Show(msg);
                            //this._core.Log_Global(msg);
                            this._appInteration.Log_Global(msg);
                        }
                        else
                        {
                            destTestExecutorItem.TestRecipeFileName = destTestRecipeFileShortName;

                            this.UpdateTreeView_CR_ExecutorCombo(treeView_editExecutorCombo, this._editTestExecutorCombo);
                        }
                    }
                    else
                    {
                        var msg = $"所选择的测试条件[{destTestRecipeFileShortName}]本地文件不存在!";
                        MessageBox.Show(msg);
                        //this._core.Log_Global(msg);
                        this._appInteration.Log_Global(msg);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新测试条件异常:{ex.Message}-{ex.StackTrace}!");
            }
        }
        /// <summary>
        /// 添加算子配置后保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveCRCombo_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._editTestExecutorCombo == null)
                {
                    MessageBox.Show("没有正在编辑的测试链!");
                    return;
                }

                foreach (var item in this._editTestExecutorCombo.Main_ExecutorConfigCollection)
                {
                    foreach (var it in item.CalculatorCollection.Book)
                    {
                        if (it.Value == "")
                        {
                            MessageBox.Show("未全部配置所有算子！");
                            return;
                        }

                    }
                }

                SaveFileDialog sfd = new SaveFileDialog();
                var prodPath = _core.GetProductConfigFileDirectory();
                var filePath = _core.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_EXECUTIVE_COMBO_PATH);
                sfd.InitialDirectory = Path.GetFullPath($@"{prodPath}\{filePath}");
                var defaultFileName = string.Concat(this._editTestExecutorCombo.Name, FileExtension.XML);
                sfd.FileName = defaultFileName;
                var dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {


                    string finalFileName = sfd.FileName;
                    finalFileName = Path.ChangeExtension(finalFileName, "xml");

                    if (finalFileName.Equals(defaultFileName))
                    {
                        this._editTestExecutorCombo.Save(finalFileName);
                        MessageBox.Show("保存成功:\r\n\r\n" + finalFileName);


                        RefreshExecutorProfileFilesListView();

                    }
                    else
                    {
                        var finalExeComboName = Path.GetFileNameWithoutExtension(finalFileName);
                        var renameDR = MessageBox.Show($"是否重命名为[{finalExeComboName}]?" +
                              $"\r\n提示:文件名与测试项名必须保持一致" +
                              $"\r\n若选是 则将重命名为[{finalExeComboName}]并存储." +
                              $"\r\n若选否 则放弃保存.", "保存提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);

                        if (renameDR == DialogResult.Yes)
                        {
                            this._editTestExecutorCombo.Name = finalExeComboName;
                            this._editTestExecutorCombo.Save(finalFileName);
                            MessageBox.Show("保存成功:\r\n\r\n" + finalFileName);
                            RefreshExecutorProfileFilesListView();
                            this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorCombo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败:[{ex.Message}+{ex.StackTrace}]!");
            }
        }
        /// <summary>
        /// 添加算子配置后，更新树
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="comboConfig"></param>
        void UpdateTreeView_CR_ExecutorCombo(TreeView tv, TestExecutorCombo comboConfig)
        {
            try
            {
                tv.Nodes.Clear();
                //tv.Nodes.Add(this._appInteration.Convert_TestExecutorComboToTreeNode(comboConfig));
                tv.Nodes.Add(this._appInteration.Convert_TestExecutorComboToTreeNode(comboConfig));
                tv.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新测试链树错误:[{ex.Message}+{ex.StackTrace}]!");
            }
        }
        /// <summary>
        /// 展示匹配算子类型的算子配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_CExecutorCombo_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                this.ClearSupportedSelector();
                this.ClearSelectedContextOverview();
                const int destCalculatorNodeLevel = 4;
                const int destTestModuleNodeLevel = 3;
                var treeNode = this.treeView_editExecutorCombo.SelectedNode;
                if (treeNode == null)
                {
                    //选择操作很正常 不是一直会选中算子节点  因此不提示
                    return;
                }

                switch (treeNode.Level)
                {
                    case destCalculatorNodeLevel:
                        {

                            #region 选中算子节点  呈现可选算子条件细节
                            var nodePath = treeNode.FullPath;

                            var destTestExecutorIndex = treeNode.Parent.Parent.Index;

                            ExecutorConfigItem destTestExecutorItem = null;
                            if (nodePath.Contains(TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME))
                            {
                                //拿到测试执行项
                                destTestExecutorItem = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                            }
                            else if (nodePath.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME))
                            {     //拿到测试执行项
                                destTestExecutorItem = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                            }
                            else if (nodePath.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
                            {     //拿到测试执行项
                                destTestExecutorItem = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                            }
                            else
                            {
                                //配置算子操作 必须要选中算子节点 
                                MessageBox.Show("无效的任何算子索引!");
                                return;
                            }

                            //拿到选中的算子索引
                            var destCalculatorIndex = treeNode.Index;
                            //拿到选中的算子vs算子条件 键值对
                            var destCalculatorKvp = destTestExecutorItem.CalculatorCollection.Book[destCalculatorIndex];
                            //拿到选中的算子实例
                            var destCalculatorInstance = this._core.CreateCalculator(destCalculatorKvp.Key) as ITestCalculator;
                            //拿到选中的算子条件类型
                            var destCalcRecipeType = destCalculatorInstance.GetCalcRecipeType();

                            const int colIndex = 0;
                            var fileDict = _appInteration.GetLocalCalcRecipeFileDict();

                            foreach (var kvp in fileDict)
                            {
                                var local_calcRecipeInstance = this._core.LoadCalcRecipeInstance(kvp.Key);
                                 // local_calcRecipeInstance = this._core.LoadInstanceFromFile_ByXmlRoot(this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CALC_RECIPE_PATH), kvp.Key);

                                if (local_calcRecipeInstance.GetType() == destCalcRecipeType)
                                {
                                    var rIndex = this.dgv_supportedCalcRecipes.Rows.Add();

                                    this.dgv_supportedCalcRecipes.Rows[rIndex].Cells[colIndex].Value = kvp.Key;
                                    this.dgv_supportedCalcRecipes.Rows[rIndex].Cells[colIndex].Tag = kvp.Value;
                                }
                            }
                            this.SwitchUIOwnerElementTo(SelectorPageOwner.CalcRecipe);
                            #endregion
                        }
                        break;
                    case destTestModuleNodeLevel:
                        {

                            #region 选中测试module节点  呈现可选测试条件细节

                            var nodePath = treeNode.FullPath;

                            var destTestExecutorIndex = treeNode.Parent.Index;

                            ExecutorConfigItem destTestExecutorItem = null;
                            if (nodePath.Contains(TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME))
                            {
                                //拿到测试执行项
                                destTestExecutorItem = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                            }
                            else if (nodePath.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME))
                            {     //拿到测试执行项
                                destTestExecutorItem = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                            }
                            else if (nodePath.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
                            {     //拿到测试执行项
                                destTestExecutorItem = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                            }
                            else
                            {
                                //配置算子操作 必须要选中算子节点 
                                MessageBox.Show("无效的测试模块索引!");
                                return;
                            }
                            //拿到选中的测试模块类名
                            if (string.IsNullOrEmpty(destTestExecutorItem.TestModuleClassName))
                            {
                                MessageBox.Show("无效的测试模块类型!");
                                return;
                            }


                            //拿到选中的算子实例
                            var destTestModuleInstance = this._core.CreateTestModule(destTestExecutorItem.TestModuleClassName) as ITestModule;
                            var destTestRecipeType = destTestModuleInstance.GetTestRecipeType();

                            const int colIndex = 0;
                            var fileDict = _appInteration.GetLocalTestRecipeFileDict();

                            foreach (var kvp in fileDict)
                            {
                                var local_testRecipeInstance = this._core.LoadTestRecipeInstance(/*this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_RECIPE_PATH),*/ kvp.Key);
                               // var local_testRecipeInstance = this._core.LoadInstanceFromFile_ByXmlRoot(this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_RECIPE_PATH), kvp.Key);
                                if (local_testRecipeInstance.GetType() == destTestRecipeType)
                                {
                                    var rIndex = this.dgv_supportedTestRecipes.Rows.Add();

                                    this.dgv_supportedTestRecipes.Rows[rIndex].Cells[colIndex].Value = kvp.Key;
                                    this.dgv_supportedTestRecipes.Rows[rIndex].Cells[colIndex].Tag = kvp.Value;
                                }
                            }
                            this.SwitchUIOwnerElementTo(SelectorPageOwner.TestRecipe);
                            #endregion
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载错误:{ex.Message}-{ex.StackTrace}!");
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                const int editableNodeLowerLevel = 2;
                const int editableNodeUpperLevel = 4;
                const int executorNodeLevel = 2;
                var selectNode = this.treeView_editExecutorCombo.SelectedNode;
                if (selectNode == null)
                {
                    MessageBox.Show("未选择任何测试项节点或其子节点!");
                    return;
                }
                if (selectNode.Level >= editableNodeLowerLevel &&
                    selectNode.Level <= editableNodeUpperLevel)
                {

                    var path = selectNode.FullPath;
                    if (path.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {
                        var tempNode = selectNode;
                        while (true)
                        {
                            if (tempNode.Level == executorNodeLevel)
                            {
                                break;
                            }
                            else
                            {
                                tempNode = tempNode.Parent;
                            }
                        }
                        if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorCombo.Main_ExecutorConfigCollection.Count - 1)
                        {
                            var dr = MessageBox.Show($"确定删除[主要]测试项节点[{tempNode.FullPath}]?", "删除[主要]测试项节点", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                            this._editTestExecutorCombo.Main_ExecutorConfigCollection.RemoveAt(tempNode.Index);
                            this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorCombo);
                        }
                    }
                    else if (path.Contains(TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {
                        var tempNode = selectNode;
                        while (true)
                        {
                            if (tempNode.Level == executorNodeLevel)
                            {
                                break;
                            }
                            else
                            {
                                tempNode = tempNode.Parent;
                            }
                        }
                        if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorCombo.Pre_ExecutorConfigCollection.Count - 1)
                        {
                            var dr = MessageBox.Show($"确定删除[前置]测试项节点[{tempNode.FullPath}]?", "删除[前置]测试项节点", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                            this._editTestExecutorCombo.Pre_ExecutorConfigCollection.RemoveAt(tempNode.Index);
                            this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorCombo);
                        }
                    }
                    else if (path.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {
                        var tempNode = selectNode;
                        while (true)
                        {
                            if (tempNode.Level == executorNodeLevel)
                            {
                                break;
                            }
                            else
                            {
                                tempNode = tempNode.Parent;
                            }
                        }
                        if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorCombo.Post_ExecutorConfigCollection.Count - 1)
                        {
                            var dr = MessageBox.Show($"确定删除[后置]测试项节点[{tempNode.FullPath}]?", "删除[后置]测试项节点", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                            this._editTestExecutorCombo.Post_ExecutorConfigCollection.RemoveAt(tempNode.Index);
                            this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorCombo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除测试节点异常:[{ex.Message} -{ex.StackTrace}]!");
            }
        }


        private void SwitchUIOwnerElementTo(SelectorPageOwner spOwner)
        {
            for (int i = 0; i < tlp_TestRecipeAndCalcRecipe.RowCount; i++)
            {
                tlp_TestRecipeAndCalcRecipe.RowStyles[i].Height = this.SelectorPageRowHeightDict[spOwner][i];
            }
            switch (spOwner)
            {
                case SelectorPageOwner.TestRecipe:
                    {
                        btn_AddNewTestRcpToCombo.Visible = true;
                        btn_AddNewCalcRcpToCombo.Visible = false;
                    }
                    break;
                case SelectorPageOwner.CalcRecipe:
                    {
                        btn_AddNewTestRcpToCombo.Visible = false;
                        btn_AddNewCalcRcpToCombo.Visible = true;
                    }
                    break;
            }
        }
    }
}