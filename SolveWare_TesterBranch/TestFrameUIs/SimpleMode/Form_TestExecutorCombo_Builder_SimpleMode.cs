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
    public partial class Form_TestExecutorCombo_Builder_SimpleMode : Form, ITesterAppUI
    {
        ITesterCoreInteration _core;
        TestFrameManager _appInteration;

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
        public Form_TestExecutorCombo_Builder_SimpleMode()
        {
            InitializeComponent();
        }
        private void Form_StepCombo_Load(object sender, EventArgs e)
        {
            this.RefreshExecutorFiles();
            //this.InitializeTree(this.treeView_TestExecutorConfigItemDetails);
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
            //if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            //{
            //    var exeFileShortName = this.dgv_localExecutors.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            //    var tag = this.dgv_localExecutors.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
            //    if (tag != null && File.Exists(tag.ToString()))
            //    {
            //        //this.InitExecutorViewer();
            //        // this.InitializeTree(treeView_TestExecutorConfigItemDetails);
            //        var eci = this._appInteration.LoadTestExecutorConfigItem(tag.ToString());
            //        if (eci == null)
            //        {
            //            var msg = $"所选择的测试执行项[{exeFileShortName}]不能正确解析!";
            //            MessageBox.Show(msg);
            //            //this._core.Log_Global(msg);
            //            this._appInteration.Log_Global(msg);
            //        }
            //        else
            //        {
            //            // UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItemDetails, eci);
            //            //UpdateExecutorViewer(eci);
            //        }
            //    }
            //    else
            //    {
            //        var msg = $"所选择的测试执行项[{exeFileShortName}]本地文件不存在!";
            //        MessageBox.Show(msg);
            //        //this._core.Log_Global(msg);
            //        this._appInteration.Log_Global(msg);
            //    }
            //}
        }



        private void btn_CreateNewStepCombo_Click(object sender, EventArgs e)
        {
            Form_NewExecutorCombo frm = new Form_NewExecutorCombo(this._core.GetTestPlugInKeys());
            if (frm.ShowDialog() == DialogResult.OK)
            {

                this.InitializeTree(this.treeView_TestExecutorCombo);
                this._editTestExecutorComboWithParams = this._appInteration.CreateTestExecutorComboWithParams(frm.NewName);
                this.UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);

                //this._editTestExecutorCombo = this._appInteration.CreateTestExecutorCombo(frm.NewName );
                //this.UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
            }
            else
            {

            }
        }

        private void btn_AddNewStepToMainCombo_Click(object sender, EventArgs e)
        {
            //if (this._editTestExecutorCombo == null)
            //{
            //    var msg = $"不存在正在编辑的测试链表!";
            //    this._appInteration.Log_Global(msg);
            //    MessageBox.Show(msg); return;
            //}
            if (this._editTestExecutorComboWithParams?.Combo == null)
            {
                var msg = $"不存在正在编辑的[带参数]测试链表!";
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
                    //this.InitializeTree(treeView_TestExecutorConfigItemDetails);
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

                        //this._editTestExecutorCombo.Main_ExecutorConfigCollection.Add(eci);
                        //UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                        this._editTestExecutorComboWithParams.AddExecutorConfigItemTo_Main_List(this._core, eci);
                        UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
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
            //if (this._editTestExecutorCombo == null)
            //{
            //    var msg = $"不存在正在编辑的测试链表!";
            //    this._appInteration.Log_Global(msg);
            //    MessageBox.Show(msg); return;
            //}
            if (this._editTestExecutorComboWithParams?.Combo == null)
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

                            //var sourVal = this._editTestExecutorCombo.Main_ExecutorConfigCollection[sourceIndex];
                            //var destVal = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destIndex];
                            //this._editTestExecutorCombo.Main_ExecutorConfigCollection[sourceIndex] = destVal;
                            //this._editTestExecutorCombo.Main_ExecutorConfigCollection[destIndex] = sourVal;
                            //UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
                            this._editTestExecutorComboWithParams.Switch_ExecutorConfigItem_Of_Main_List(sourceIndex, destIndex);

                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);


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

                            //var sourVal = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[sourceIndex];
                            //var destVal = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destIndex];
                            //this._editTestExecutorCombo.Pre_ExecutorConfigCollection[sourceIndex] = destVal;
                            //this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destIndex] = sourVal;
                            //UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                            this._editTestExecutorComboWithParams.Switch_ExecutorConfigItem_Of_Pre_List(sourceIndex, destIndex);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);


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

                            //var sourVal = this._editTestExecutorCombo.Post_ExecutorConfigCollection[sourceIndex];
                            //var destVal = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destIndex];
                            //this._editTestExecutorCombo.Post_ExecutorConfigCollection[sourceIndex] = destVal;
                            //this._editTestExecutorCombo.Post_ExecutorConfigCollection[destIndex] = sourVal;
                            //UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                            this._editTestExecutorComboWithParams.Switch_ExecutorConfigItem_Of_Post_List(sourceIndex, destIndex);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);

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
            //if (this._editTestExecutorCombo == null)
            //{
            //    var msg = $"不存在正在编辑的测试链表!";
            //    this._appInteration.Log_Global(msg);
            //    MessageBox.Show(msg);
            //    return;
            //}
            if (this._editTestExecutorComboWithParams?.Combo == null)
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
                        //if (tempNode.Index < this._editTestExecutorCombo.Main_ExecutorConfigCollection?.Count - 1)
                        if (tempNode.Index < this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection?.Count - 1)
                        {
                            var sourceIndex = tempNode.Index;
                            var destIndex = tempNode.Index + 1;

                            //var sourVal = this._editTestExecutorCombo.Main_ExecutorConfigCollection[sourceIndex];
                            //var destVal = this._editTestExecutorCombo.Main_ExecutorConfigCollection[destIndex];
                            //this._editTestExecutorCombo.Main_ExecutorConfigCollection[sourceIndex] = destVal;
                            //this._editTestExecutorCombo.Main_ExecutorConfigCollection[destIndex] = sourVal;
                            //UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                            this._editTestExecutorComboWithParams.Switch_ExecutorConfigItem_Of_Main_List(sourceIndex, destIndex);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);


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
                        if (tempNode.Index < this._editTestExecutorComboWithParams.Combo.Pre_ExecutorConfigCollection?.Count - 1)
                        //if (tempNode.Index < this._editTestExecutorCombo.Pre_ExecutorConfigCollection?.Count - 1)
                        {
                            var sourceIndex = tempNode.Index;
                            var destIndex = tempNode.Index + 1;

                            //var sourVal = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[sourceIndex];
                            //var destVal = this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destIndex];
                            //this._editTestExecutorCombo.Pre_ExecutorConfigCollection[sourceIndex] = destVal;
                            //this._editTestExecutorCombo.Pre_ExecutorConfigCollection[destIndex] = sourVal;
                            //UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                            this._editTestExecutorComboWithParams.Switch_ExecutorConfigItem_Of_Pre_List(sourceIndex, destIndex);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);


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
                        if (tempNode.Index < this._editTestExecutorComboWithParams.Combo.Post_ExecutorConfigCollection?.Count - 1)
                        //if (tempNode.Index < this._editTestExecutorCombo.Post_ExecutorConfigCollection?.Count - 1)
                        {
                            var sourceIndex = tempNode.Index;
                            var destIndex = tempNode.Index + 1;

                            //var sourVal = this._editTestExecutorCombo.Post_ExecutorConfigCollection[sourceIndex];
                            //var destVal = this._editTestExecutorCombo.Post_ExecutorConfigCollection[destIndex];
                            //this._editTestExecutorCombo.Post_ExecutorConfigCollection[sourceIndex] = destVal;
                            //this._editTestExecutorCombo.Post_ExecutorConfigCollection[destIndex] = sourVal;
                            //UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                            this._editTestExecutorComboWithParams.Switch_ExecutorConfigItem_Of_Post_List(sourceIndex, destIndex);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);


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
            //if (this._editTestExecutorCombo == null)
            //{
            //    var msg = $"不存在正在编辑的测试链表!";
            //    this._appInteration.Log_Global(msg);
            //    MessageBox.Show(msg); return;
            //}
            if (this._editTestExecutorComboWithParams?.Combo == null)
            {
                var msg = $"不存在正在编辑的测试链表!";
                this._appInteration.Log_Global(msg);
                MessageBox.Show(msg); return;
            }
            if (false)
            {
                var anyDupMsg = this._editTestExecutorComboWithParams.CheckDuplicateCalcParamName();
                if (string.IsNullOrEmpty(anyDupMsg) == false)
                {
                    var msg = $@"存在同名的计算结果参数:{Environment.NewLine}{anyDupMsg}!!{Environment.NewLine}测试链表保存失败!";
                    this._appInteration.Log_Global(msg);
                    MessageBox.Show(msg); return;
                }
            }
       
            var anyEmptyCalcName = this._editTestExecutorComboWithParams.CheckEmptyCalcParamName();
            if (anyEmptyCalcName == true)
            {
                var msg = $@"存在[未命名]计算结果参数!!{Environment.NewLine}测试链表保存失败!";
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
                            //this._editTestExecutorCombo.Name = finalExeComboName;
                            //this._editTestExecutorCombo.Save(finalFileName);

                            this._editTestExecutorComboWithParams.Name = finalExeComboName;
                            this._editTestExecutorComboWithParams.Save(finalFileName);


                            MessageBox.Show("保存成功:\r\n\r\n" + finalFileName);
                            RefreshComboFiles();
                            //this.UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
                            this.UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
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
                        this._editTestExecutorComboWithParams = this._appInteration.LoadTestExecutorComboWithParams(filePath);
                        //this._editTestExecutorCombo = this._appInteration.LoadTestExecutorCombo(filePath);
                        //this.UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
                        this.UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载错误:{ex.Message}-{ex.StackTrace}!");
            }
        }

   

        private void btn_AddNewStepToPreCombo_Click(object sender, EventArgs e)
        {
            //if (this._editTestExecutorCombo == null)
            //{
            //    var msg = $"不存在正在编辑的测试链表!";
            //    this._appInteration.Log_Global(msg);
            //    MessageBox.Show(msg); return;
            //}
            if (this._editTestExecutorComboWithParams?.Combo == null)
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
                    //this.InitializeTree(treeView_TestExecutorConfigItemDetails);
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
                        //this._editTestExecutorCombo.Pre_ExecutorConfigCollection.Add(eci);
                        //UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                        this._editTestExecutorComboWithParams.AddExecutorConfigItemTo_Pre_List(this._core, eci);
                        UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
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
            //if (this._editTestExecutorCombo == null)
            //{
            //    var msg = $"不存在正在编辑的测试链表!";
            //    this._appInteration.Log_Global(msg);
            //    MessageBox.Show(msg); return;
            //}
            if (this._editTestExecutorComboWithParams?.Combo == null)
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
                    //this.InitializeTree(treeView_TestExecutorConfigItemDetails);
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
                        //this._editTestExecutorCombo.Post_ExecutorConfigCollection.Add(eci);
                        //UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                        this._editTestExecutorComboWithParams.AddExecutorConfigItemTo_Post_List(this._core, eci);
                        UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
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
            //if (this._editTestExecutorCombo == null)
            //{
            //    var msg = $"不存在正在编辑的测试链表!";
            //    this._appInteration.Log_Global(msg);
            //    MessageBox.Show(msg); return;
            //}
            if (this._editTestExecutorComboWithParams?.Combo == null)
            {
                var msg = $"不存在正在编辑的测试链表!";
                this._appInteration.Log_Global(msg);
                MessageBox.Show(msg); return;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_localExecutors.SelectedCells.Count != 1)
                {
                    return;
                }
                DataGridViewCell cell = dgv_localExecutors.SelectedCells[0];
                if (cell.RowIndex >= 0 && cell.ColumnIndex >= 0)
                {
                    var exeFileShortName = this.dgv_localExecutors.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value;
                    var tag = this.dgv_localExecutors.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Tag;
                    if (tag != null && File.Exists(tag.ToString()))
                    {
                        //this.InitExecutorViewer();
                        // this.InitializeTree(treeView_TestExecutorConfigItemDetails);
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
                            Form_TestExecutorItem_Preview frm = new Form_TestExecutorItem_Preview(eci);
                            frm.ConnectToAppInteration(this._appInteration);
                            frm.Show();
                            // UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItemDetails, eci);
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
            catch
            {

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
                        if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection.Count - 1)
                        //if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorCombo.Main_ExecutorConfigCollection.Count - 1)
                        {
                            var dr = MessageBox.Show($"确定删除[主要]测试项节点[{tempNode.FullPath}]?", "删除[主要]测试项节点", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }

                            //this._editTestExecutorCombo.Main_ExecutorConfigCollection.RemoveAt(tempNode.Index);
                            //UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                            this._editTestExecutorComboWithParams.RemoveAt_ExecutorConfigItem_Of_Main_List(tempNode.Index);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
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
                        if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorComboWithParams.Combo.Pre_ExecutorConfigCollection.Count - 1)
                        //if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorCombo.Pre_ExecutorConfigCollection.Count - 1)
                        {
                            var dr = MessageBox.Show($"确定删除[前置]测试项节点[{tempNode.FullPath}]?", "删除[前置]测试项节点", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                            //this._editTestExecutorCombo.Pre_ExecutorConfigCollection.RemoveAt(tempNode.Index);
                            //UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);

                            this._editTestExecutorComboWithParams.RemoveAt_ExecutorConfigItem_Of_Pre_List(tempNode.Index);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);

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
                        if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorComboWithParams.Combo.Post_ExecutorConfigCollection.Count - 1)
                        //if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorCombo.Post_ExecutorConfigCollection.Count - 1)
                        {
                            var dr = MessageBox.Show($"确定删除[后置]测试项节点[{tempNode.FullPath}]?", "删除[后置]测试项节点", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                            if (dr != DialogResult.Yes)
                            {
                                return;
                            }
                            this._editTestExecutorComboWithParams.RemoveAt_ExecutorConfigItem_Of_Post_List(tempNode.Index);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                            //this._editTestExecutorCombo.Post_ExecutorConfigCollection.RemoveAt(tempNode.Index);
                            //UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorCombo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除测试节点异常:[{ex.Message} -{ex.StackTrace}]!");
            }
        }
        private void tsm_deleteCalculatorFromTestModule_Click(object sender, EventArgs e)
        {
            try
            {
                const int executorNodeLevel = 2;// 3;
                const int calculatorNodeLevel = 4;
                var selectNode = this.treeView_TestExecutorCombo.SelectedNode;
                if (selectNode == null ||
                    selectNode.Level != calculatorNodeLevel)
                {
                    MessageBox.Show("未选择任何算子节点!");
                    return;
                }

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
                    if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection.Count - 1)
                    {
                        var dr = MessageBox.Show($"确定要从[{tempNode.Text}]删除算子[{selectNode.Text}]?", "删除算子", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (dr == DialogResult.Yes)
                        {
                            this._editTestExecutorComboWithParams.RemoveCalculatorAt_ExecutorConfigItem_Of_Main_List(tempNode.Index, selectNode.Index);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                        }
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
                    if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorComboWithParams.Combo.Pre_ExecutorConfigCollection.Count - 1)
                    {
                        var dr = MessageBox.Show($"确定要从[{tempNode.Text}]删除算子[{selectNode.Text}]?", "删除算子", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (dr == DialogResult.Yes)
                        {
                            this._editTestExecutorComboWithParams.RemoveCalculatorAt_ExecutorConfigItem_Of_Pre_List(tempNode.Index, selectNode.Index);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                        }
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
                    if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorComboWithParams.Combo.Post_ExecutorConfigCollection.Count - 1)
                    {
                        var dr = MessageBox.Show($"确定要从[{tempNode.Text}]删除算子[{selectNode.Text}]?", "删除算子", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (dr == DialogResult.Yes)
                        {
                            this._editTestExecutorComboWithParams.RemoveCalculatorAt_ExecutorConfigItem_Of_Post_List(tempNode.Index, selectNode.Index);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除测试节点异常:[{ex.Message} -{ex.StackTrace}]!");
            }
        }

        private void tsm_InsertCalculatorIntoTestModule_Click(object sender, EventArgs e)
        {
            InsertCalculatorIntoTestModule();
            return;

            try
            {
                const int executorNodeLevel = 3;
                const int calculatorNodeLevel = 4;
                var selectNode = this.treeView_TestExecutorCombo.SelectedNode;
                if (selectNode == null ||
                    selectNode.Level != calculatorNodeLevel)
                {
                    MessageBox.Show("未选择任何算子节点!");
                    return;
                }

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
                    if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection.Count - 1)
                    {

                        Form_InsertCalculatorIntoExecutorCombo frm = new Form_InsertCalculatorIntoExecutorCombo(tempNode.Text);
                        frm.ConnectToAppInteration(this._appInteration);
                        var dr = frm.ShowDialog();
                        if (dr == DialogResult.OK)
                        {

                            var rcp = this._core.CreateExeItem_CalcRecipeInstance(frm.CalculatorTypeName);
                            this._editTestExecutorComboWithParams.InsertCalculatorInto_ExecutorConfigItem_Of_Main_List(tempNode.Index, selectNode.Index, frm.CalculatorTypeName, frm.CalculatorParamName, (CalcRecipe)rcp);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                        }

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
                    if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorComboWithParams.Combo.Pre_ExecutorConfigCollection.Count - 1)

                    {
                        Form_InsertCalculatorIntoExecutorCombo frm = new Form_InsertCalculatorIntoExecutorCombo(tempNode.Text);
                        frm.ConnectToAppInteration(this._appInteration);
                        var dr = frm.ShowDialog();
                        if (dr == DialogResult.OK)
                        {

                            var rcp = this._core.CreateExeItem_CalcRecipeInstance(frm.CalculatorTypeName);
                            this._editTestExecutorComboWithParams.InsertCalculatorInto_ExecutorConfigItem_Of_Pre_List(tempNode.Index, selectNode.Index, frm.CalculatorTypeName, frm.CalculatorParamName, (CalcRecipe)rcp);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                        }

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
                    if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorComboWithParams.Combo.Post_ExecutorConfigCollection.Count - 1)

                    {
                        Form_InsertCalculatorIntoExecutorCombo frm = new Form_InsertCalculatorIntoExecutorCombo(tempNode.Text);
                        frm.ConnectToAppInteration(this._appInteration);
                        var dr = frm.ShowDialog();
                        if (dr == DialogResult.OK)
                        {

                            var rcp = this._core.CreateExeItem_CalcRecipeInstance(frm.CalculatorTypeName);
                            this._editTestExecutorComboWithParams.InsertCalculatorInto_ExecutorConfigItem_Of_Post_List(tempNode.Index, selectNode.Index, frm.CalculatorTypeName, frm.CalculatorParamName, (CalcRecipe)rcp);
                            UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除测试节点异常:[{ex.Message} -{ex.StackTrace}]!");
            }
        }

        //优化：插入算子功能
        private void InsertCalculatorIntoTestModule()
        {
            try
            {
                const int executorNodeLevel = 3;
                //const int calculatorNodeLevel = 4;
                var selectNode = this.treeView_TestExecutorCombo.SelectedNode;
                if (selectNode == null ||
                     //selectNode.Level != calculatorNodeLevel||
                     selectNode.Level < executorNodeLevel)
                {
                    MessageBox.Show("未选择任何算子节点!");
                    return;
                }

                var tempNode = selectNode;
                while (true)
                {
                    if (tempNode.Level == 3)
                    {
                        break;
                    }
                    else
                    {
                        tempNode = tempNode.Parent;
                    }
                }

                var path = selectNode.FullPath;
                if (path.Contains(TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME))
                {
                    UpdateTreeView_Main(selectNode, tempNode);
                }
                else if (path.Contains(TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME))
                {
                    UpdateTreeView_Pre(selectNode, tempNode);
                }
                else if (path.Contains(TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME))
                {
                    UpdateTreeView_Post(selectNode, tempNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除测试节点异常:[{ex.Message} -{ex.StackTrace}]!");
            }
        }
        private void UpdateTreeView_Post(TreeNode selectNode, TreeNode tempNode)
        {
            if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorComboWithParams.Combo.Post_ExecutorConfigCollection.Count - 1)
            {
                Form_InsertCalculatorIntoExecutorCombo frm = new Form_InsertCalculatorIntoExecutorCombo(tempNode.Text);
                frm.ConnectToAppInteration(this._appInteration);
                var dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    var rcp = this._core.CreateExeItem_CalcRecipeInstance(frm.CalculatorTypeName);
                    if (selectNode.Level == 3)
                    {
                        this._editTestExecutorComboWithParams.InsertCalculatorInto_ExecutorConfigItem_Of_Post_List_V2(tempNode.Parent.Index, selectNode.Index, frm.CalculatorTypeName, frm.CalculatorParamName, (CalcRecipe)rcp);
                    }
                    else
                    {
                        this._editTestExecutorComboWithParams.InsertCalculatorInto_ExecutorConfigItem_Of_Post_List(tempNode.Parent.Index, selectNode.Index, frm.CalculatorTypeName, frm.CalculatorParamName, (CalcRecipe)rcp);
                    }
                    UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                }
            }
        }
        private void UpdateTreeView_Pre(TreeNode selectNode, TreeNode tempNode)
        {
            if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorComboWithParams.Combo.Pre_ExecutorConfigCollection.Count - 1)
            {
                Form_InsertCalculatorIntoExecutorCombo frm = new Form_InsertCalculatorIntoExecutorCombo(tempNode.Text);
                frm.ConnectToAppInteration(this._appInteration);
                var dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {

                    var rcp = this._core.CreateExeItem_CalcRecipeInstance(frm.CalculatorTypeName);
                    if (selectNode.Level == 3)
                    {
                        this._editTestExecutorComboWithParams.InsertCalculatorInto_ExecutorConfigItem_Of_Pre_List_V2(tempNode.Parent.Index, selectNode.Index, frm.CalculatorTypeName, frm.CalculatorParamName, (CalcRecipe)rcp);
                    }
                    else
                    {
                        this._editTestExecutorComboWithParams.InsertCalculatorInto_ExecutorConfigItem_Of_Pre_List(tempNode.Parent.Index, selectNode.Index, frm.CalculatorTypeName, frm.CalculatorParamName, (CalcRecipe)rcp);
                    }
                    UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                }

            }
        }
        private void UpdateTreeView_Main(TreeNode selectNode, TreeNode tempNode)
        {
            if (tempNode.Index >= 0 && tempNode.Index <= this._editTestExecutorComboWithParams.Combo.Main_ExecutorConfigCollection.Count - 1)
            {
                Form_InsertCalculatorIntoExecutorCombo frm = new Form_InsertCalculatorIntoExecutorCombo(tempNode.Text);
                frm.ConnectToAppInteration(this._appInteration);
                var dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {

                    var rcp = this._core.CreateExeItem_CalcRecipeInstance(frm.CalculatorTypeName);
                    if (selectNode.Level == 3)
                    {
                        this._editTestExecutorComboWithParams.InsertCalculatorInto_ExecutorConfigItem_Of_Main_List_V2(tempNode.Parent.Index, selectNode.Index, frm.CalculatorTypeName, frm.CalculatorParamName, (CalcRecipe)rcp);
                    }
                    else
                    {
                        this._editTestExecutorComboWithParams.InsertCalculatorInto_ExecutorConfigItem_Of_Main_List(tempNode.Parent.Index, selectNode.Index, frm.CalculatorTypeName, frm.CalculatorParamName, (CalcRecipe)rcp);
                    }
                    UpdateTreeView_TestExecutorCombo(this.treeView_TestExecutorCombo, this._editTestExecutorComboWithParams.Combo);
                }

            }
        }


        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (this.lv_StepComboFiles.SelectedItems.Count == 1)
                {
                    var dr = MessageBox.Show(
                                         $"确定删除{ lv_StepComboFiles.SelectedItems[0].Text}?",
                                         "删除操作",
                                         MessageBoxButtons.YesNoCancel,
                                         MessageBoxIcon.Question
                                     );
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                    var filePath = lv_StepComboFiles.SelectedItems[0].Tag.ToString();
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    var fileDict = _appInteration.GetLocalExecutorComboFiles();
                    this._core.RefreshListView(this.lv_StepComboFiles, fileDict);
                }

            }
            catch
            {

            }
        }
 
    }
}