using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using SolveWare_TestComponents.Data;
using System;
using System.IO;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_TestExecutorCombo_InstrumentEditor_SimpleMode : Form, ITesterAppUI
    {

        ITesterCoreInteration _core;
        TestFrameManager _appInteration;
        //TestExecutorCombo _editTestExecutorCombo { get; set; }
        TestExecutorComboWithParams _editTestExecutorComboWithParams { get; set; }
        //TestExecutorCombo _editTestExecutorCombo { get; set; }
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
        public Form_TestExecutorCombo_InstrumentEditor_SimpleMode()
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
                    this.RefreshPlatformPositionSelector();
                    this.RefreshPlatformAxesSelector();
                    this.RefreshPlatformInstrumentSelector();
                    this.RefreshExecutorProfileFilesListView();
                });
            }
            else
            {
                this.RefreshPlatformPositionSelector();
                this.RefreshPlatformAxesSelector();
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
                MessageBox.Show($"RefreshPlatformInstrumentSelector错误:[{ex.Message}+{ex.StackTrace}]!");
            }
        }
        void RefreshPlatformAxesSelector()
        {
            try
            {
                var temp = this._appInteration.GetAxesNameCollection();
                this.dgv_supportedAxes.Rows.Clear();
                foreach (var item in temp)
                {
                    var rIndex = this.dgv_supportedAxes.Rows.Add();
                    this.dgv_supportedAxes.Rows[rIndex].SetValues(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RefreshPlatformAxesSelector错误:[{ex.Message}+{ex.StackTrace}]!");
            }
        }
        void RefreshPlatformPositionSelector()
        {
            try
            {
                var temp = this._appInteration.GetPositionNameCollection();
                this.dgv_supportedAxesPosition.Rows.Clear();
                foreach (var item in temp)
                {
                    var rIndex = this.dgv_supportedAxesPosition.Rows.Add();
                    this.dgv_supportedAxesPosition.Rows[rIndex].SetValues(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RefreshPlatformAxesSelector错误:[{ex.Message}+{ex.StackTrace}]!");
            }
        }
        private void ClearSupportedSelector()
        {
            //this.dgv_supportedCalcRecipes.Rows.Clear();
            this.dgv_supportedInstruments.Rows.Clear();
            this.dgv_supportedAxes.Rows.Clear();
            this.dgv_supportedAxesPosition.Rows.Clear();
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
                        //this._editTestExecutorCombo = this._appInteration.LoadTestExecutorCombo(filePath);
                        //this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorCombo);
                        this._editTestExecutorComboWithParams = this._appInteration.LoadTestExecutorComboWithParams(filePath);
                        this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载错误:{ex.Message}-{ex.StackTrace}!");
            }
        }


        //优化：一键更新仪器配置
        private void OneClickUpdateInstrument()
        {
            try
            {
                const int destTestModuleNodeLevel = 3;//需要更新的节点-仪器列表

                //拿到需要更新的测试模块节点
                var treeNode = this.treeView_editExecutorCombo.SelectedNode;
                if (treeNode == null)
                {
                    //配置算子操作 必须要选中算子节点 
                    MessageBox.Show("未选中任何测试项目!");
                    return;
                }
                if (treeNode.Level != destTestModuleNodeLevel)
                {
                    MessageBox.Show("请选择：[测试项]所需<仪器>列表，再重试！");
                    return;
                }

                var nodePath = treeNode.FullPath;
                //拿到测试执行项索引
                var destTestExecutorIndex = treeNode.Parent.Index;

                foreach (TreeNode node in this.treeView_editExecutorCombo.SelectedNode.Nodes)
                {
                    ExecutorConfigItem destTestExecutorItem = null;
                    if (nodePath.Contains(TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {
                        //拿到测试执行项
                        destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else if (nodePath.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {     //拿到测试执行项
                        destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else if (nodePath.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {     //拿到测试执行项
                        destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else
                    {
                        MessageBox.Show("请选择：测试项信息节点，再重试！");
                        return;
                    }

                    var tempArr = node.Name.Split(new string[] { "$$" }, StringSplitOptions.None);
                    var moduleDefineInstrName = tempArr[0];
                    var moduleDefineInstrType = tempArr[1];

                    if (destTestExecutorItem.UserDefineInstrumentConfig.ContainsKey(moduleDefineInstrName))
                    {
                        destTestExecutorItem.UserDefineInstrumentConfig[moduleDefineInstrName] = moduleDefineInstrName;
                    }
                    else
                    {
                        destTestExecutorItem.UserDefineInstrumentConfig.Add(moduleDefineInstrName, moduleDefineInstrName);
                    }
                }
                this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorComboWithParams.Combo);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }

        }


        private void btn_AddNewTestRcpToCombo_Click(object sender, EventArgs e)
        {
            OneClickUpdateInstrument();
            return;
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
                        //destTestExecutorItem = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                        destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else if (nodePath.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {     //拿到测试执行项
                        //destTestExecutorItem = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                        destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else if (nodePath.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {     //拿到测试执行项
                        //destTestExecutorItem = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                        destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Post_ExecutorConfigCollection[destTestExecutorIndex];
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
                    this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                    //this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorCombo);

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
                //if (this._editTestExecutorCombo == null)
                //{
                //    MessageBox.Show("没有正在编辑的测试链!");
                //    return;
                //}
                if (this._editTestExecutorComboWithParams?.Combo == null)
                {
                    MessageBox.Show("没有正在编辑的测试链!");
                    return;
                }


                //foreach (var item in this._editTestExecutorCombo.Main_ExecutorConfigCollection)
                //{
                //    foreach (var it in item.CalculatorCollection.Book)
                //    {
                //        if (it.Value == "")
                //        {
                //            MessageBox.Show("未全部配置所有算子！");
                //            return;
                //        }
                //    }
                //}
                SaveFileDialog sfd = new SaveFileDialog();
                var prodPath = _core.GetProductConfigFileDirectory();
                var filePath = _core.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_EXECUTIVE_COMBO_PATH);
                sfd.InitialDirectory = Path.GetFullPath($@"{prodPath}\{filePath}");
                //var defaultFileName = string.Concat(this._editTestExecutorCombo.Name, FileExtension.XML);
                var defaultFileName = string.Concat(this._editTestExecutorComboWithParams.Name, FileExtension.XML);
                sfd.FileName = defaultFileName;
                var dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    string finalFileName = sfd.FileName;
                    finalFileName = Path.ChangeExtension(finalFileName, "xml");

                    if (finalFileName.Equals(defaultFileName))
                    {
                        this._editTestExecutorComboWithParams.Save(finalFileName);
                        //this._editTestExecutorCombo.Save(finalFileName);
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
                            //this._editTestExecutorCombo.Name = finalExeComboName;
                            //this._editTestExecutorCombo.Save(finalFileName);
                            this._editTestExecutorComboWithParams.Name = finalExeComboName;
                            this._editTestExecutorComboWithParams.Save(finalFileName);


                            MessageBox.Show("保存成功:\r\n\r\n" + finalFileName);
                            RefreshExecutorProfileFilesListView();
                            //this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorCombo);
                            this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorComboWithParams.Combo);
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
                ExecutorConfigItem_TestParamsConfig destTestParamItem = null;

                if (nodePath.Contains(TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME))
                {
                    //拿到测试执行项
                    //destTestExecutorItem = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                    destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                    destTestParamItem = this._editTestExecutorComboWithParams.ComboParams.Pre_TestParamsCollection[destTestExecutorIndex];
                }
                else if (nodePath.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME))
                {     //拿到测试执行项
                    //destTestExecutorItem = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                    destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                    destTestParamItem = this._editTestExecutorComboWithParams.ComboParams.Main_TestParamsCollection[destTestExecutorIndex];
                }
                else if (nodePath.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
                {     //拿到测试执行项
                    //destTestExecutorItem = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                    destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                    destTestParamItem = this._editTestExecutorComboWithParams.ComboParams.Post_TestParamsCollection[destTestExecutorIndex];
                }
                if (MessageBox.Show($"是否要对测试项[{destTestExecutorItem.TestExecutorName}]进行单项调试?", "单项调试", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //this._core.RunTestExecutorDebugger(destTestExecutorItem);
                    this._core.RunTestExecutorDebugger(destTestExecutorItem, destTestParamItem);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void toolStripMenuItem1_Copy_Click(object sender, EventArgs e) 
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
                ExecutorConfigItem_TestParamsConfig destTestParamItem = null;

                //ExecutorConfigItem destTestExecutorItem_c = null;
                //ExecutorConfigItem_TestParamsConfig destTestParamItem_c = null; 

                if (nodePath.Contains(TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME))
                {
                    //拿到测试执行项
                    //destTestExecutorItem = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                    destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                    destTestParamItem = this._editTestExecutorComboWithParams.ComboParams.Pre_TestParamsCollection[destTestExecutorIndex];
                    //复制
                    //destTestExecutorItem.TestExecutorName=
                }
                else if (nodePath.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME))
                {     //拿到测试执行项
                    //destTestExecutorItem = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                    destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                    destTestParamItem = this._editTestExecutorComboWithParams.ComboParams.Main_TestParamsCollection[destTestExecutorIndex];
                   
                    //复制                 
                    var destTestExecutorItem_c = CloneHelper.Clone(destTestExecutorItem);
                    var destTestParamItem_c = CloneHelper.Clone(destTestParamItem);

                    if (destTestExecutorItem_c==null|| destTestParamItem_c==null)
                    {
                        this._core.Log_Global($"有模块没有序列化，不能复制");
                        return;
                    }

                    string OldName = destTestExecutorItem_c.TestExecutorName + "_Clone_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    Form_changeModelName frm = new Form_changeModelName(OldName);
                    frm.ShowDialog();
                    if (frm.CopyName_OK)
                    {
                        foreach (var item in this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection)
                        {
                            if (item.TestExecutorName == frm.NewName)
                            {
                                if (MessageBox.Show($"已经存在模块名[{item.TestExecutorName}]，复制失败", "复制失败", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                                {
                                    return;
                                }
                            }                           
                        }
                        foreach (var item in this._editTestExecutorComboWithParams.ComboParams.Main_TestParamsCollection)
                        {
                            if (item.Name == frm.NewName)
                            {
                                if (MessageBox.Show($"已经存在模块名[{item.Name}]，复制失败", "复制失败", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                                {
                                    return;
                                }
                            }
                        }
                        this._core.Log_Global($"重新确认后的模块名字为[{frm.NewName}]");
                        destTestExecutorItem_c.TestExecutorName = frm.NewName;
                        destTestParamItem_c.Name = frm.NewName;
                      
                    }
                    else
                    {
                        this._core.Log_Global($"用户选择不复制模块");
                        return;
                    }

                    int ChageNo = destTestExecutorIndex + 1;

                    if (this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection.Count < ChageNo ||
                        this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection.Count == ChageNo)
                    {
                        this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection.Add(destTestExecutorItem_c);
                        this._editTestExecutorComboWithParams.ComboParams.Main_TestParamsCollection.Add(destTestParamItem_c);
                    }
                    else
                    {
                        this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection.Insert(ChageNo, destTestExecutorItem_c);
                        this._editTestExecutorComboWithParams.ComboParams.Main_TestParamsCollection.Insert(ChageNo, destTestParamItem_c);
                    }

                    #region 有风险
                    //if (MessageBox.Show($"是否要对测试项[{destTestExecutorItem.TestExecutorName}]进行复制?", "复制模块", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.No)
                    //{
                    //    return;
                    //}
                    //复制
                    // //destTestExecutorItem_c =destTestExecutorItem;
                    // //destTestParamItem_c = destTestParamItem;

                    // var prodPath = _core.GetProductConfigFileDirectory();
                    // var filePath = _core.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_EXECUTIVE_COMBO_PATH);
                    // string FullPath = Path.GetFullPath($@"{prodPath}\{filePath}\Temp");
                    // string finalFileName = Path.Combine(FullPath, destTestExecutorItem.TestExecutorName);//sfd.FileName;
                    // finalFileName = Path.ChangeExtension(finalFileName, "xml");

                    // if (Directory.GetFiles(FullPath).Length > 30)
                    // {
                    //     DirectoryInfo dir = new DirectoryInfo(FullPath);
                    //     FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                    //     foreach (FileSystemInfo i in fileinfo)
                    //     {
                    //         if (i is DirectoryInfo)            //判断是否文件夹
                    //         {
                    //             DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                    //             subdir.Delete(true);          //删除子目录和文件
                    //         }
                    //         else
                    //         {
                    //             File.Delete(i.FullName);      //删除指定文件
                    //         }
                    //     }
                    // }

                    // if (File.Exists(finalFileName) == false)
                    // {
                    //     if (Directory.Exists(Path.GetDirectoryName(finalFileName)) == false)
                    //     {
                    //         Directory.CreateDirectory(Path.GetDirectoryName(finalFileName));
                    //     }

                    // }

                    // XmlHelper.SerializeFile(finalFileName, destTestExecutorItem);

                    // destTestExecutorItem_c = (XmlHelper.DeserializeFile(finalFileName, typeof(ExecutorConfigItem)) as ExecutorConfigItem);

                    // //var prodPath = _core.GetProductConfigFileDirectory();
                    // //var filePath = _core.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_EXECUTIVE_COMBO_PATH);
                    // //string FullPath = Path.GetFullPath($@"{prodPath}\{filePath}\Temp");
                    // string finalFileName_par = Path.Combine(FullPath, destTestParamItem.Name+"par");//sfd.FileName; 
                    // finalFileName_par = Path.ChangeExtension(finalFileName_par, "xml");

                    // if (File.Exists(finalFileName_par) == false)
                    // {
                    //     if (Directory.Exists(Path.GetDirectoryName(finalFileName_par)) == false)
                    //     {
                    //         Directory.CreateDirectory(Path.GetDirectoryName(finalFileName_par)); 
                    //     }

                    // }

                    // var tempTypes = destTestParamItem.GetIncludingTypes();
                    // destTestParamItem.IncludingTypes = new System.Collections.Generic.List<string> ();
                    // foreach (var t in tempTypes)
                    // {
                    //     destTestParamItem.IncludingTypes.Add(t.FullName);
                    // }

                    // XmlHelper.SerializeFile(finalFileName_par, destTestParamItem, tempTypes);

                    //// XmlHelper.SerializeFile(finalFileName_par, destTestParamItem);  

                    // destTestParamItem_c = XmlHelper.DeserializeFile(finalFileName_par, typeof(ExecutorConfigItem_TestParamsConfig), tempTypes) as ExecutorConfigItem_TestParamsConfig;

                    // int count = 0;
                    // string  _Name = string.Empty;
                    // if (destTestExecutorItem.TestExecutorName.Split('_').Length > 1)
                    // {
                    //     _Name = destTestExecutorItem.TestExecutorName.Split('_')[0];
                    // }
                    // else
                    // {
                    //     _Name = destTestExecutorItem.TestExecutorName;
                    // }
                    // int CollectionCount = 0; //list里面的计数
                    // int ItemCount = 0;  //对应Item的计数
                    // foreach (var item in this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection)
                    // {
                    //     if(item.TestExecutorName.Contains(_Name))
                    //     {                           
                    //         if (item.TestExecutorName.Split('_').Length>1)
                    //         {
                    //             int _NoOK=0;
                    //             if (!int.TryParse(item.TestExecutorName.Split('_')[1], out _NoOK)) continue;
                    //             //int _count = Convert.ToInt16(destTestExecutorItem.TestExecutorName.Split('_')[1] != null);
                    //              int _count = Convert.ToInt16(item.TestExecutorName.Split('_')[1]);
                    //             if (_count > count)
                    //             {
                    //                 count = _count;
                    //                 ItemCount = CollectionCount;
                    //             }
                    //         }                           
                    //     }
                    //     CollectionCount++;
                    // }
                    // if (destTestExecutorItem_c.TestExecutorName.Contains("_")|| count>0)
                    // {
                    //     destTestExecutorItem_c.TestExecutorName =
                    //          destTestExecutorItem_c.TestExecutorName.Split('_')[0] + "_" + (count + 1).ToString();
                    // }
                    // else
                    // {
                    //     destTestExecutorItem_c.TestExecutorName =
                    //          destTestExecutorItem_c.TestExecutorName.Split('_')[0] + "_" + (1).ToString();
                    // }
                    // if (destTestParamItem_c.Name.Contains("_") || count > 0)
                    // {
                    //     destTestParamItem_c.Name =
                    //          destTestParamItem_c.Name.Split('_')[0] + "_" + (count + 1).ToString();
                    // }
                    // else
                    // {
                    //     destTestParamItem_c.Name = 
                    //          destTestParamItem_c.Name.Split('_')[0] + "_" + (1).ToString();

                    // }

                    // int ChageNo = destTestExecutorIndex + 1;
                    // if (ItemCount!=0)
                    // {
                    //      ChageNo = ItemCount + 2; //+count;
                    // }                    

                    // if (this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection.Count<ChageNo||
                    //     this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection.Count == ChageNo)
                    // {
                    //     this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection.Add( destTestExecutorItem_c);
                    //     this._editTestExecutorComboWithParams.ComboParams.Main_TestParamsCollection.Add( destTestParamItem_c);
                    // }
                    // else
                    // {
                    //     this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection.Insert(ChageNo, destTestExecutorItem_c);
                    //     this._editTestExecutorComboWithParams.ComboParams.Main_TestParamsCollection.Insert(ChageNo, destTestParamItem_c);
                    // }
                    #endregion

                }
                else if (nodePath.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
                {     //拿到测试执行项
                    //destTestExecutorItem = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                    destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                    destTestParamItem = this._editTestExecutorComboWithParams.ComboParams.Post_TestParamsCollection[destTestExecutorIndex];
                }
                this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorComboWithParams.Combo);//刷新

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

        private void btn_editComboParam_Click(object sender, EventArgs e)
        {
            if (this._editTestExecutorComboWithParams?.Combo == null)
            {
                MessageBox.Show("没有正在编辑的测试链!");
                return;
            }
            try
            {
                this._core.RunTestExecutorCombo_ParamsEditor(_editTestExecutorComboWithParams);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开测试链表参数编辑页面错误:[{ex.Message}-{ex.StackTrace}]！");
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lv_StepComboProfileFiles.SelectedItems.Count == 1)
                {
                    var dr = MessageBox.Show(
                                         $"确定删除{ lv_StepComboProfileFiles.SelectedItems[0].Text}?",
                                         "删除操作",
                                         MessageBoxButtons.YesNoCancel,
                                         MessageBoxIcon.Question
                                     );
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                    var filePath = lv_StepComboProfileFiles.SelectedItems[0].Tag.ToString();
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    var fileDict = _appInteration.GetLocalExecutorComboFiles();
                    this._core.RefreshListView(this.lv_StepComboProfileFiles, fileDict);
                }

            }
            catch
            {

            }

        }

        private void btn_AddAxisToCombo_Click(object sender, EventArgs e)
        {
            try
            {
                const int destTestModuleNodeLevel = 4;
                const int destCellCount = 1;
                if (this.dgv_supportedAxes.SelectedCells?.Count == destCellCount)
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
                        //destTestExecutorItem = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                        destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else if (nodePath.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {     //拿到测试执行项
                        //destTestExecutorItem = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                        destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else if (nodePath.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {     //拿到测试执行项
                        //destTestExecutorItem = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                        destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                    }

                    else
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("无效的测试项目索引!");
                        return;
                    }
                    if (nodePath.Contains(ExecutorConfigItem.REQ_AXIS_LIST_NODE_NAME) == false)
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("无效的需求仪器索引!");
                        return;
                    }
                    var tempArr = treeNode.Name.Split(new string[] { "$$" }, StringSplitOptions.None);
                    var moduleDefineAxisName = tempArr[0];

                    var userSelectAxisName = this.dgv_supportedAxes.SelectedCells[0].Value.ToString();


                    if (destTestExecutorItem.UserDefineAxisConfig.ContainsKey(moduleDefineAxisName))
                    {
                        destTestExecutorItem.UserDefineAxisConfig[moduleDefineAxisName] = userSelectAxisName;
                    }
                    else
                    {
                        destTestExecutorItem.UserDefineAxisConfig.Add(moduleDefineAxisName, userSelectAxisName);
                    }

                    this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                    //this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorCombo);

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

        private void tp_resourceSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.dgv_supportedInstruments.ClearSelection();
                this.dgv_supportedAxes.ClearSelection();
                this.dgv_supportedAxesPosition.ClearSelection();

            }
            catch
            {

            }
        }

        private void btn_AddPositionToCombo_Click(object sender, EventArgs e)
        {
            try
            {
                const int destTestModuleNodeLevel = 4;
                const int destCellCount = 1;
                if (this.dgv_supportedAxesPosition.SelectedCells?.Count == destCellCount)
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
                        //destTestExecutorItem = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                        destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Pre_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else if (nodePath.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {     //拿到测试执行项
                        //destTestExecutorItem = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                        destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection[destTestExecutorIndex];
                    }
                    else if (nodePath.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
                    {     //拿到测试执行项
                        //destTestExecutorItem = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                        destTestExecutorItem = this._editTestExecutorComboWithParams.Combo.Post_ExecutorConfigCollection[destTestExecutorIndex];
                    }

                    else
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("无效的测试项目索引!");
                        return;
                    }
                    if (nodePath.Contains(ExecutorConfigItem.REQ_POS_LIST_NODE_NAME) == false)
                    {
                        //配置算子操作 必须要选中算子节点 
                        MessageBox.Show("无效的需求仪器索引!");
                        return;
                    }
                    var tempArr = treeNode.Name.Split(new string[] { "$$" }, StringSplitOptions.None);
                    var moduleDefinePositionName = tempArr[0];

                    var userSelectPositionName = this.dgv_supportedAxesPosition.SelectedCells[0].Value.ToString();


                    if (destTestExecutorItem.UserDefinePositionConfig.ContainsKey(moduleDefinePositionName))
                    {
                        destTestExecutorItem.UserDefinePositionConfig[moduleDefinePositionName] = userSelectPositionName;
                    }
                    else
                    {
                        destTestExecutorItem.UserDefinePositionConfig.Add(moduleDefinePositionName, userSelectPositionName);
                    }

                    this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                    //this.UpdateTreeView_CR_ExecutorCombo(this.treeView_editExecutorCombo, this._editTestExecutorCombo);

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
    }
}