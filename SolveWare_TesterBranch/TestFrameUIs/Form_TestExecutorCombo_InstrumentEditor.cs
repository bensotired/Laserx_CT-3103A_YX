using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using SolveWare_TestComponents.Data;
using System;
using System.IO;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_TestExecutorCombo_InstrumentEditor : Form, ITesterAppUI
    {

        ITesterCoreInteration _core;
        TestFrameManager _appInteration;
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
        public Form_TestExecutorCombo_InstrumentEditor()
        {
            InitializeComponent();
        }
        private void Form_CRCombo_Load(object sender, EventArgs e)
        {
            RefreshOnce();
        }
        public void RefreshOnce()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((EventHandler)delegate
                {

                    this.RefreshPlatformInstrumentSelector();
                    this.RefreshExecutorProfileFilesListView();
                });
            }
            else
            {
                this.RefreshPlatformInstrumentSelector();
                this.RefreshExecutorProfileFilesListView();
            }
        }

        protected virtual void RefreshExecutorProfileFilesListView()
        {
            var fileDict = _appInteration.GetLocalExecutorComboFiles();
            this._core.RefreshListView(this.lv_StepComboProfileFiles, fileDict);
        }
        void RefreshPlatformInstrumentSelector()
        {
            try
            {
                var instrDict = this._core.GetStationInstrumentSimpleInfos();
                this.dgv_supportedInstruments.Rows.Clear();
                foreach (var kvp in instrDict)
                {
                    var rIndex = this.dgv_supportedInstruments.Rows.Add();
                    this.dgv_supportedInstruments.Rows[rIndex].SetValues(kvp.Key, kvp.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RefreshExecutorFiles错误:[{ex.Message}+{ex.StackTrace}]!");
            }
        }
        private void ClearSupportedSelector()
        {
            //this.dgv_supportedCalcRecipes.Rows.Clear();
            this.dgv_supportedInstruments.Rows.Clear();
        }
        //private void ClearSelectedContextOverview()
        //{
        //    this.dgv_selectedTestRecipeOverview.Rows.Clear();
        //    this.dgv_selectedCalcRecipeOverview.Rows.Clear();
        //}

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

        private void btn_AddNewTestRcpToCombo_Click(object sender, EventArgs e)
        {
            try
            {
                const int destTestModuleNodeLevel = 4;
                const int destCellCount = 1;
                if (this.dgv_supportedInstruments.SelectedCells?.Count == destCellCount)
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
                        MessageBox.Show("无效的测试项目索引!");
                        return;
                    }
                    if (nodePath.Contains(ExecutorConfigItem.REQ_INSTR_LIST_NODE_NAME) == false)
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("无效的需求仪器索引!");
                        return;
                    }
                    var tempArr = treeNode.Name.Split(new string[] { "$$" }, StringSplitOptions.None);
                    var moduleDefineInstrName = tempArr[0];
                    var moduleDefineInstrType = tempArr[1];
                    var userSelectInstrName = this.dgv_supportedInstruments.SelectedCells[0].Value.ToString();

                    var instrObj = this._core.GetStationHardwareObject(userSelectInstrName);
                    var instrTypeName = instrObj.GetType().Name;



                    if (instrTypeName != moduleDefineInstrType &&
                        instrObj.GetType().GetInterface(moduleDefineInstrType) == null)
                    {
                        MessageBox.Show($"测试模块需求仪器[{moduleDefineInstrName}]类型为[{moduleDefineInstrType}]\r\n" +
                            $"与用户选择仪器[{userSelectInstrName}]类型[{instrTypeName}]不匹配!");
                        return;
                    }
                    else
                    {
                        if (destTestExecutorItem.UserDefineInstrumentConfig.ContainsKey(moduleDefineInstrName))
                        {
                            destTestExecutorItem.UserDefineInstrumentConfig[moduleDefineInstrName] = userSelectInstrName;
                        }
                        else
                        {
                            destTestExecutorItem.UserDefineInstrumentConfig.Add(moduleDefineInstrName, userSelectInstrName);
                        }
                    }
                    this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorCombo);

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
                tv.Nodes.Add(this._appInteration.Convert_TestExecutorComboToTreeNode_WithInstrumentConfig(comboConfig));
                tv.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新测试链树错误:[{ex.Message}+{ex.StackTrace}]!");
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                const int destTestModuleNodeLevel = 2;


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
                var destTestExecutorIndex = treeNode.Index;

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
                if (MessageBox.Show($"是否要对测试项[{destTestExecutorItem.TestExecutorName}]进行单项调试?", "单项调试", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this._core.RunTestExecutorDebugger(destTestExecutorItem);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void cms_ExecutorComboTreeView_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            const int destTestModuleNodeLevel = 2;


            var treeNode = this.treeView_editExecutorCombo.SelectedNode;
            if (treeNode == null)
            {
                e.Cancel = true;
                return;
            }
            if (treeNode.Level != destTestModuleNodeLevel)
            {
                e.Cancel = true;
                return;
            }
            var nodePath = treeNode.FullPath;
            if (nodePath.Contains(TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME) ||
                 nodePath.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME) ||
                 nodePath.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
            {

            }
            else
            {
                e.Cancel = true;
                return;
            }
        }
    }
}