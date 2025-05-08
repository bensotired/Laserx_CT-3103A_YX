using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_TestExecutorCombo_Builder : Form, ITesterAppUI
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
        public Form_TestExecutorCombo_Builder()
        {
            InitializeComponent();
        }
        private void Form_StepCombo_Load(object sender, EventArgs e)
        {
            this.RefreshExecutorFiles();
            this.InitializeTree(this.treeView_TestExecutorConfigItemDetails);
        }
        public void RefreshOnce()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((EventHandler)delegate
                {
                    RefreshComboFiles();
                    RefreshExecutorFiles();
                });
            }
            else
            {
                RefreshComboFiles();
                RefreshExecutorFiles();
            }
        }
        protected virtual void RefreshComboFiles()
        {
            try
            {
                var fileDict = _appInteration.GetLocalExecutorComboFiles();
                this._core.RefreshListView(lv_StepComboFiles, fileDict);
                //RefreshListView(this.lv_StepComboFiles, fileDict);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RefreshComboFiles错误:[{ex.Message}+{ex.StackTrace}]!");
            }
        }
        void RefreshExecutorFiles()
        {
            try
            {
                const int colIndex = 0;
                var fileDict = _appInteration.GetLocalExecutorFiles();
                this.dgv_localExecutors.Rows.Clear();
                foreach (var kvp in fileDict)
                {
                    var rIndex = this.dgv_localExecutors.Rows.Add();

                    this.dgv_localExecutors.Rows[rIndex].Cells[colIndex].Value = kvp.Key;
                    this.dgv_localExecutors.Rows[rIndex].Cells[colIndex].Tag = kvp.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RefreshExecutorFiles错误:[{ex.Message}+{ex.StackTrace}]!");
            }
        }
        void UpdateTreeView_ExecutorConfigItem(TreeView tv, ExecutorConfigItem exeConfig)
        {
            try
            {
                tv.Nodes.Clear();
                tv.Nodes.Add(this._appInteration.Convert_ExecutorConfigItemToTreeNode(exeConfig));
                tv.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新测试项树错误:[{ex.Message}+{ex.StackTrace}]!");
            }
        }
        void UpdateTreeView_TestExecutorCombo(TreeView tv, TestExecutorCombo comboConfig)
        {
            try
            {
                tv.Nodes.Clear();
                tv.Nodes.Add(this._appInteration.Convert_TestExecutorComboToTreeNode(comboConfig));
                tv.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新测试链树错误:[{ex.Message}+{ex.StackTrace}]!");
            }
        }
        void InitializeTree(TreeView treeView)
        {
            treeView.Nodes.Clear();

        }
        private void dgv_localExecutors_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var exeFileShortName = this.dgv_localExecutors.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                var tag = this.dgv_localExecutors.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                if (tag != null && File.Exists(tag.ToString()))
                {
                    //this.InitExecutorViewer();
                    this.InitializeTree(treeView_TestExecutorConfigItemDetails);
                    var eci = this._appInteration.LoadTestExecutorConfigItem(tag.ToString());
                    if (eci == null)
                    {
                        var msg = $"所选择的测试执行项[{exeFileShortName}]不能正确解析!";
                        MessageBox.Show(msg);
                        //this._core.Log_Global(msg);
                        this._appInteration.Log_Global(msg);
                    }
                    else
                    {
                        UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItemDetails, eci);
                        //UpdateExecutorViewer(eci);
                    }
                }
                else
                {
                    var msg = $"所选择的测试执行项[{exeFileShortName}]本地文件不存在!";
                    MessageBox.Show(msg);
                    //this._core.Log_Global(msg);
                    this._appInteration.Log_Global(msg);
                }
            }
        }


        //public void RefreshListView(ListView listView, Dictionary<string, string> files)
        //{//文件名(无扩展名), 完整目录
        //    while (!this.IsHandleCreated)
        //    {
        //        Application.DoEvents();
        //    }

        //    this.Invoke((EventHandler)delegate
        //    {
        //        try
        //        {
        //            string b4Plan = string.Empty;
        //            bool needReSelected = false;
        //            if (listView.SelectedItems.Count > 0)
        //            {
        //                b4Plan = listView.SelectedItems[0].Text;
        //                needReSelected = true;
        //            }

        //            int newIndex = 0;
        //            int i = -1;
        //            listView.Clear();
        //            listView.Columns.Add("文件名", listView.Width, HorizontalAlignment.Left);
        //            listView.BeginUpdate();
        //            foreach (var kvp in files)
        //            {
        //                i++;
        //                ListViewItem lvi = new ListViewItem();
        //                lvi.Text = kvp.Key;
        //                lvi.Tag = kvp.Value; //用Tag存路径
        //                listView.Items.Add(lvi);

        //                if (needReSelected)
        //                {
        //                    if (kvp.Key == b4Plan)
        //                    {
        //                        newIndex = i;
        //                    }
        //                }

        //            }
        //            listView.EndUpdate();

        //            if (needReSelected)
        //            {
        //                listView.Items[newIndex].Selected = true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    });
        //}

        //TreeNode Convert_ExecutorConfigItemToTreeNode(ExecutorConfigItem exeConfig)
        //{
        //    TreeNode rootNode = new TreeNode();

        //    rootNode.Name = exeConfig.TestExecutorName;
        //    rootNode.Text = exeConfig.TestExecutorName;

        //    var testModuleNode = new TreeNode();
        //    testModuleNode.Name = exeConfig.TestModuleClassName;
        //    testModuleNode.Text = exeConfig.TestModuleClassName;

        //    foreach (var calcKvp in exeConfig.CalculatorCollection)
        //    {
        //        var calcNode = new TreeNode();
        //        calcNode.Name = calcKvp.Key;
        //        calcNode.Text = calcKvp.Key + "  " + calcKvp.Value;
        //        testModuleNode.Nodes.Add(calcNode);
        //    }
        //    rootNode.Nodes.Add(testModuleNode);

        //    return rootNode;
        //}
        //TreeNode Convert_TestExecutorComboToTreeNode(TestExecutorCombo comboConfig)
        //{
        //    var rootNode = new TreeNode();
        //    rootNode.Name = comboConfig.Name;
        //    rootNode.Text = comboConfig.Name;
        //    var comboInfoNode = new TreeNode();
        //    comboInfoNode.Name = TestExecutorCombo.INFO_NODE_NAME;
        //    comboInfoNode.Text = TestExecutorCombo.INFO_NODE_NAME;

        //    var prodTypeNode = new TreeNode();
        //    prodTypeNode.Name = comboConfig.ProductType;
        //    prodTypeNode.Text = $"可支持产品类型[{comboConfig.ProductType}]";

        //    var applicableTestPluginNode = new TreeNode();
        //    applicableTestPluginNode.Name = comboConfig.ApplicableTestPlugin;
        //    applicableTestPluginNode.Text = $"可支持测试组件类型[{ comboConfig.ApplicableTestPlugin}]";

        //    comboInfoNode.Nodes.Add(prodTypeNode);
        //    comboInfoNode.Nodes.Add(applicableTestPluginNode);

        //    rootNode.Nodes.Add(comboInfoNode);

        //    var comboExecutorsRootNode = new TreeNode();
        //    comboExecutorsRootNode.Name = TestExecutorCombo.COMBO_EXECUTOR_ROOT_NODE_NAME;
        //    comboExecutorsRootNode.Text = TestExecutorCombo.COMBO_EXECUTOR_ROOT_NODE_NAME;

        //    foreach (var eci in comboConfig.ExecutorConfigCollection)
        //    {
        //        var eciNode = Convert_ExecutorConfigItemToTreeNode(eci);
        //        comboExecutorsRootNode.Nodes.Add(eciNode);
        //    }

        //    rootNode.Nodes.Add(comboExecutorsRootNode);
        //    return rootNode;
        //}

        private void btn_CreateNewStepCombo_Click(object sender, EventArgs e)
        {
            Form_NewExecutorCombo frm = new Form_NewExecutorCombo(this._core.GetTestPlugInKeys());
            if (frm.ShowDialog() == DialogResult.OK)
            {

                this.InitializeTree(this.treeView_TestExecutorCombo);
                this._editTestExecutorCombo = this._appInteration.CreateTestExecutorCombo(frm.NewName/*, frm.NewApplicableTestPluginName*/);
                this.UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
            }
            else
            {

            }
        }

        private void btn_AddNewStepToMainCombo_Click(object sender, EventArgs e)
        {
            if (this._editTestExecutorCombo == null)
            {
                var msg = $"不存在正在编辑的测试链表!";
                this._appInteration.Log_Global(msg);
                MessageBox.Show(msg); return;
            }
            const int destCellCount = 1;
            if (this.dgv_localExecutors.SelectedCells?.Count == destCellCount)
            {
                var exeFileShortName = this.dgv_localExecutors.SelectedCells[0].Value;
                var tag = this.dgv_localExecutors.SelectedCells[0].Tag;
                if (tag != null && File.Exists(tag.ToString()))
                {
                    //this.InitExecutorViewer();
                    this.InitializeTree(treeView_TestExecutorConfigItemDetails);
                    var eci = this._appInteration.LoadTestExecutorConfigItem(tag.ToString());
                    if (eci == null)
                    {
                        var msg = $"所选择的测试执行项[{exeFileShortName}]不能正确解析!";
                        MessageBox.Show(msg);
                        //this._core.Log_Global(msg);
                        this._appInteration.Log_Global(msg);
                    }
                    else
                    {
                        this._editTestExecutorCombo.Main_ExecutorConfigCollection.Add(eci);
                        UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
                    }
                }
                else
                {
                    var msg = $"所选择的测试执行项[{exeFileShortName}]本地文件不存在!";
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

        private void btn_MoveUpStepToCombo_Click(object sender, EventArgs e)
        {
            if (this._editTestExecutorCombo == null)
            {
                var msg = $"不存在正在编辑的测试链表!";
                this._appInteration.Log_Global(msg);
                MessageBox.Show(msg); return;
            }
            try
            {
                const int editableNodeLowerLevel = 2;
                const int editableNodeUpperLevel = 4;
                const int executorNodeLevel = 2;
                var selectNode = this.treeView_TestExecutorCombo.SelectedNode;
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

                        if (tempNode.Index > 0)
                        {
                            var sourceIndex = tempNode.Index;
                            var destIndex = tempNode.Index - 1;

                            var sourVal = this._editTestExecutorCombo.Main_ExecutorConfigCollection[sourceIndex];
                            var destVal = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destIndex];
                            this._editTestExecutorCombo.Main_ExecutorConfigCollection[sourceIndex] = destVal;
                            this._editTestExecutorCombo.Main_ExecutorConfigCollection[destIndex] = sourVal;
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
 
                            var sameKeyNodes = this.treeView_TestExecutorCombo.Nodes.Find(tempNode.Name, true);
                            if (sameKeyNodes?.Length > 0)
                            {
                                for (int i = 0; i < sameKeyNodes.Length; i++)
                                {
                                    if (sameKeyNodes[i].Level == tempNode.Level)
                                    {
                                        if (sameKeyNodes[i].Index == destIndex)
                                        {
                                            this.treeView_TestExecutorCombo.Select();
                                            this.treeView_TestExecutorCombo.SelectedNode = sameKeyNodes[i];
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("当前测试项已经处于测试链顶部!");
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

                        if (tempNode.Index > 0)
                        {
                            var sourceIndex = tempNode.Index;
                            var destIndex = tempNode.Index - 1;

                            var sourVal = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[sourceIndex];
                            var destVal = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destIndex];
                            this._editTestExecutorCombo.Pre_ExecutorConfigCollection[sourceIndex] = destVal;
                            this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destIndex] = sourVal;
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                            var sameKeyNodes = this.treeView_TestExecutorCombo.Nodes.Find(tempNode.Name, true);
                            if (sameKeyNodes?.Length > 0)
                            {
                                for (int i = 0; i < sameKeyNodes.Length; i++)
                                {
                                    if (sameKeyNodes[i].Level == tempNode.Level)
                                    {
                                        if (sameKeyNodes[i].Index == destIndex)
                                        {
                                            this.treeView_TestExecutorCombo.Select();
                                            this.treeView_TestExecutorCombo.SelectedNode = sameKeyNodes[i];
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("当前测试项已经处于测试链顶部!");
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

                        if (tempNode.Index > 0)
                        {
                            var sourceIndex = tempNode.Index;
                            var destIndex = tempNode.Index - 1;

                            var sourVal = this._editTestExecutorCombo.Post_ExecutorConfigCollection[sourceIndex];
                            var destVal = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destIndex];
                            this._editTestExecutorCombo.Post_ExecutorConfigCollection[sourceIndex] = destVal;
                            this._editTestExecutorCombo.Post_ExecutorConfigCollection[destIndex] = sourVal;
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                            var sameKeyNodes = this.treeView_TestExecutorCombo.Nodes.Find(tempNode.Name, true);
                            if (sameKeyNodes?.Length > 0)
                            {
                                for (int i = 0; i < sameKeyNodes.Length; i++)
                                {
                                    if (sameKeyNodes[i].Level == tempNode.Level)
                                    {
                                        if (sameKeyNodes[i].Index == destIndex)
                                        {
                                            this.treeView_TestExecutorCombo.Select();
                                            this.treeView_TestExecutorCombo.SelectedNode = sameKeyNodes[i];
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("当前测试项已经处于测试链顶部!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("未选择任何测试项节点或其子节点!");
                    }
                }
                else
                {
                    MessageBox.Show("未选择任何测试项节点或其子节点!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"移动测试节点异常:[{ex.Message} -{ex.StackTrace}]!");
            }
        }

        private void btn_MoveDownStepToCombo_Click(object sender, EventArgs e)
        {
            if (this._editTestExecutorCombo == null)
            {
                var msg = $"不存在正在编辑的测试链表!";
                this._appInteration.Log_Global(msg);
                MessageBox.Show(msg);
                return;
            }
            try
            {
                const int editableNodeLowerLevel = 2;
                const int editableNodeUpperLevel = 4;
                const int executorNodeLevel = 2;
                var selectNode = this.treeView_TestExecutorCombo.SelectedNode;
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

                        if (tempNode.Index < this._editTestExecutorCombo.Main_ExecutorConfigCollection?.Count - 1)
                        {
                            var sourceIndex = tempNode.Index;
                            var destIndex = tempNode.Index + 1;

                            var sourVal = this._editTestExecutorCombo.Main_ExecutorConfigCollection[sourceIndex];
                            var destVal = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destIndex];
                            this._editTestExecutorCombo.Main_ExecutorConfigCollection[sourceIndex] = destVal;
                            this._editTestExecutorCombo.Main_ExecutorConfigCollection[destIndex] = sourVal;
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                            var sameKeyNodes = this.treeView_TestExecutorCombo.Nodes.Find(tempNode.Name, true);
                            if (sameKeyNodes?.Length > 0)
                            {
                                for (int i = 0; i < sameKeyNodes.Length; i++)
                                {
                                    if (sameKeyNodes[i].Level == tempNode.Level)
                                    {
                                        if (sameKeyNodes[i].Index == destIndex)
                                        {
                                            this.treeView_TestExecutorCombo.Select();
                                            this.treeView_TestExecutorCombo.SelectedNode = sameKeyNodes[i];
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("当前测试项已经处于测试链底部!");
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

                        if (tempNode.Index < this._editTestExecutorCombo.Pre_ExecutorConfigCollection?.Count - 1)
                        {
                            var sourceIndex = tempNode.Index;
                            var destIndex = tempNode.Index + 1;

                            var sourVal = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[sourceIndex];
                            var destVal = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destIndex];
                            this._editTestExecutorCombo.Pre_ExecutorConfigCollection[sourceIndex] = destVal;
                            this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destIndex] = sourVal;
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                            var sameKeyNodes = this.treeView_TestExecutorCombo.Nodes.Find(tempNode.Name, true);
                            if (sameKeyNodes?.Length > 0)
                            {
                                for (int i = 0; i < sameKeyNodes.Length; i++)
                                {
                                    if (sameKeyNodes[i].Level == tempNode.Level)
                                    {
                                        if (sameKeyNodes[i].Index == destIndex)
                                        {
                                            this.treeView_TestExecutorCombo.Select();
                                            this.treeView_TestExecutorCombo.SelectedNode = sameKeyNodes[i];
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("当前测试项已经处于测试链底部!");
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

                        if (tempNode.Index < this._editTestExecutorCombo.Post_ExecutorConfigCollection?.Count - 1)
                        {
                            var sourceIndex = tempNode.Index;
                            var destIndex = tempNode.Index + 1;

                            var sourVal = this._editTestExecutorCombo.Post_ExecutorConfigCollection[sourceIndex];
                            var destVal = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destIndex];
                            this._editTestExecutorCombo.Post_ExecutorConfigCollection[sourceIndex] = destVal;
                            this._editTestExecutorCombo.Post_ExecutorConfigCollection[destIndex] = sourVal;
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                            var sameKeyNodes = this.treeView_TestExecutorCombo.Nodes.Find(tempNode.Name, true);
                            if (sameKeyNodes?.Length > 0)
                            {
                                for (int i = 0; i < sameKeyNodes.Length; i++)
                                {
                                    if (sameKeyNodes[i].Level == tempNode.Level)
                                    {
                                        if (sameKeyNodes[i].Index == destIndex)
                                        {
                                            this.treeView_TestExecutorCombo.Select();
                                            this.treeView_TestExecutorCombo.SelectedNode = sameKeyNodes[i];
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("当前测试项已经处于测试链底部!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("未选择任何测试项节点或其子节点!");
                    }
                }
                else
                {
                    MessageBox.Show("未选择任何测试项节点或其子节点!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"移动测试节点异常:[{ex.Message} -{ex.StackTrace}]!");
            }
        }

        private void btn_SaveStepCombo_Click(object sender, EventArgs e)
        {
            if (this._editTestExecutorCombo == null)
            {
                var msg = $"不存在正在编辑的测试链表!";
                this._appInteration.Log_Global(msg);
                MessageBox.Show(msg); return;
            }
            try
            {
                //if (this._editTestExecutorCombo == null)
                //{
                //    MessageBox.Show("没有正在编辑的测试链!");
                //    return;
                //}
                SaveFileDialog sfd = new SaveFileDialog();

                var prodPath = this._core.GetProductConfigFileDirectory();
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
                        //this._editExecutorConfigItem.Save(finalFileName);
                        MessageBox.Show("保存成功:\r\n\r\n" + finalFileName);

                        RefreshComboFiles();


                    }
                    else
                    {
                        var finalExeComboName = Path.GetFileNameWithoutExtension(finalFileName);
                        var renameDR = MessageBox.Show($"是否将测试链重命名为[{finalExeComboName}]?" +
                              $"\r\n提示:文件名与测试项名必须保持一致" +
                              $"\r\n若选是 则将测试链名重命名为[{finalExeComboName}]并存储." +
                              $"\r\n若选否 则放弃保存.", "保存提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);

                        if (renameDR == DialogResult.Yes)
                        {
                            this._editTestExecutorCombo.Name = finalExeComboName;
                            this._editTestExecutorCombo.Save(finalFileName);
                            MessageBox.Show("保存成功:\r\n\r\n" + finalFileName);
                            RefreshComboFiles();
                            this.UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败:[{ex.Message}+{ex.StackTrace}]!");
            }
        }

        private void lv_StepComboFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (this.lv_StepComboFiles.SelectedItems.Count == 1)
                {
                    var filePath = lv_StepComboFiles.SelectedItems[0].Tag.ToString();

                    if (File.Exists(filePath))
                    {
                        this._editTestExecutorCombo = this._appInteration.LoadTestExecutorCombo(filePath);
                        this.UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                    }
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
                var selectNode = this.treeView_TestExecutorCombo.SelectedNode;
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
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
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
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
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
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除测试节点异常:[{ex.Message} -{ex.StackTrace}]!");
            }
        }

        private void btn_AddNewStepToPreCombo_Click(object sender, EventArgs e)
        {
            if (this._editTestExecutorCombo == null)
            {
                var msg = $"不存在正在编辑的测试链表!";
                this._appInteration.Log_Global(msg);
                MessageBox.Show(msg); return;
            }
            const int destCellCount = 1;
            if (this.dgv_localExecutors.SelectedCells?.Count == destCellCount)
            {
                var exeFileShortName = this.dgv_localExecutors.SelectedCells[0].Value;
                var tag = this.dgv_localExecutors.SelectedCells[0].Tag;
                if (tag != null && File.Exists(tag.ToString()))
                {
                    //this.InitExecutorViewer();
                    this.InitializeTree(treeView_TestExecutorConfigItemDetails);
                    var eci = this._appInteration.LoadTestExecutorConfigItem(tag.ToString());
                    if (eci == null)
                    {
                        var msg = $"所选择的测试执行项[{exeFileShortName}]不能正确解析!";
                        MessageBox.Show(msg);
                        //this._core.Log_Global(msg);
                        this._appInteration.Log_Global(msg);
                    }
                    else
                    {
                        this._editTestExecutorCombo.Pre_ExecutorConfigCollection.Add(eci);
                        UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
                    }
                }
                else
                {
                    var msg = $"所选择的测试执行项[{exeFileShortName}]本地文件不存在!";
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

        private void btn_AddNewStepToPostCombo_Click(object sender, EventArgs e)
        {
            if (this._editTestExecutorCombo == null)
            {
                var msg = $"不存在正在编辑的测试链表!";
                this._appInteration.Log_Global(msg);
                MessageBox.Show(msg); return;
            }
            const int destCellCount = 1;
            if (this.dgv_localExecutors.SelectedCells?.Count == destCellCount)
            {
                var exeFileShortName = this.dgv_localExecutors.SelectedCells[0].Value;
                var tag = this.dgv_localExecutors.SelectedCells[0].Tag;
                if (tag != null && File.Exists(tag.ToString()))
                {
                    //this.InitExecutorViewer();
                    this.InitializeTree(treeView_TestExecutorConfigItemDetails);
                    var eci = this._appInteration.LoadTestExecutorConfigItem(tag.ToString());
                    if (eci == null)
                    {
                        var msg = $"所选择的测试执行项[{exeFileShortName}]不能正确解析!";
                        MessageBox.Show(msg);
                        //this._core.Log_Global(msg);
                        this._appInteration.Log_Global(msg);
                    }
                    else
                    {
                        this._editTestExecutorCombo.Post_ExecutorConfigCollection.Add(eci);
                        UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
                    }
                }
                else
                {
                    var msg = $"所选择的测试执行项[{exeFileShortName}]本地文件不存在!";
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

        private void btn_ClearStepCombo_Click(object sender, EventArgs e)
        {
            if (this._editTestExecutorCombo == null)
            {
                var msg = $"不存在正在编辑的测试链表!";
                this._appInteration.Log_Global(msg);
                MessageBox.Show(msg); return;
            }
        }
    }
}
