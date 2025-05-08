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
    public partial class Form_TestExecutor_Builder : Form, ITesterAppUI
    {
        ITesterCoreInteration _core;
        TestFrameManager _appInteration;
        ExecutorConfigItem _editExecutorConfigItem { get; set; }

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
        public Form_TestExecutor_Builder()
        {
            InitializeComponent();
        }

        private void Form_TestModule_Load(object sender, EventArgs e)
        {
            RefreshOnce();

            this.InitializeTree(this.treeView_TestExecutorConfigItem);


            this._editExecutorConfigItem = this._appInteration.DemoTestExecutorConfigItem();
            this.UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItem, this._editExecutorConfigItem);
        }
        public void RefreshOnce()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((EventHandler)delegate
                {
                    this.RefreshSupportedTestModuleClass();
                    this.InitSupportedCalculatorClassViewer();
                    this.RefreshExecutorFilesListView();
                });
            }
            else
            {
                this.RefreshSupportedTestModuleClass();
                this.InitSupportedCalculatorClassViewer();
                this.RefreshExecutorFilesListView();

            }
        }
        protected virtual void RefreshSupportedTestModuleClass()
        {
            try
            {
                const int colIndex = 0;
                var fileDict = _appInteration.GetSupportedTestModuleClass();
                this.dgv_ImportedTestModuleClass.Rows.Clear();

                foreach (var kvp in fileDict)
                {
                    var rIndex = this.dgv_ImportedTestModuleClass.Rows.Add();
                    ///import test module types
                    this.dgv_ImportedTestModuleClass.Rows[rIndex].Cells[colIndex].Value = kvp.Key;
                    ///supported calculator types
                    this.dgv_ImportedTestModuleClass.Rows[rIndex].Cells[colIndex].Tag = kvp.Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RefreshSupportedTestModuleClass错误:[{ex.Message}+{ex.StackTrace}]!");
            }

        }

        protected virtual void RefreshExecutorFilesListView()
        {
            var fileDict = _appInteration.GetLocalExecutorFiles();
            this._core.RefreshListView(this.listView_TestExecutorConfig, fileDict);
        }

        void InitSupportedCalculatorClassViewer()
        {
            //dgv_supportedCalculatorClass1.Rows.Clear();
            dgv_supportedCalculatorClass.Rows.Clear();

        }
        void UpdateSupportedCalculatorClassViewer(List<string> calcCls)
        {

            if (calcCls?.Count <= 0)
            {
                return;
            }
            const int valColIndex = 0;
            foreach (var calc in calcCls)
            {
                var rIndex = dgv_supportedCalculatorClass.Rows.Add();
                dgv_supportedCalculatorClass.Rows[rIndex].Cells[valColIndex].Value = calc;
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


        private void listView_TestExecutorConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.listView_TestExecutorConfig.SelectedItems.Count == 1)
                {
                    var filePath = listView_TestExecutorConfig.SelectedItems[0].Tag.ToString();
                    if (File.Exists(filePath))
                    {
                        this._editExecutorConfigItem = this._appInteration.LoadTestExecutorConfigItem(filePath);
                        this.UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItem, this._editExecutorConfigItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载测试项错误:{ex.Message}-{ex.StackTrace}!");
            }
        }



        void InitializeTree(TreeView treeView)
        {
            treeView.Nodes.Clear();

        }
        void ExpandAllTree(TreeView treeView)
        {
            treeView.ExpandAll();

        }

        private void dgv_ImportedTestModuleClass_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var tstMdl = this.dgv_ImportedTestModuleClass.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                var tag = this.dgv_ImportedTestModuleClass.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                this.InitSupportedCalculatorClassViewer();
                var supCalcCls = tag as List<string>;
                if (supCalcCls?.Count > 0)
                {
                    this.UpdateSupportedCalculatorClassViewer(supCalcCls);
                }
                else
                {
                    var msg = $"所选择的测试模块项[{tstMdl}]没有支持的算法模块!";
                    MessageBox.Show(msg);
                    this._appInteration.Log_Global(msg);
                }
            }
        }

        private void tv_CreateTestExecutorConfigTree_Click(object sender, EventArgs e)
        {
            Form_NewExecutorConfigItem frm = new Form_NewExecutorConfigItem();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //InitializeTestExecutorConfigTree();
                this.InitializeTree(this.treeView_TestExecutorConfigItem);
                this._editExecutorConfigItem = this._appInteration.CreateTestExecutorConfigItem(frm.NewTestExecutorConfigName);
                this.UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItem, this._editExecutorConfigItem);
            }
            else
            {

            }
        }

        private void btn_UpdateTestModule_To_TestExecutorConfigTree_Click(object sender, EventArgs e)
        {
            try
            {
                const int onlyCellCount = 1;
                if (this.dgv_ImportedTestModuleClass.SelectedCells.Count != onlyCellCount)
                {
                    MessageBox.Show("选择测试模块错误,更新测试模块节点失败!");
                    return;
                }
                var destTestModuleClassName = this.dgv_ImportedTestModuleClass.SelectedCells[0].Value.ToString();
                if (string.IsNullOrEmpty(destTestModuleClassName))
                {
                    MessageBox.Show("选择测试模块错误,更新测试模块节点失败!");
                    return;
                }
                if (this._editExecutorConfigItem == null)
                {
                    MessageBox.Show("没有正在编辑的测试项,更新测试模块节点失败!");
                    return;
                }
                if (string.IsNullOrEmpty(this._editExecutorConfigItem.TestModuleClassName))
                {
                    this._appInteration.UpdateTestModuleClassOfExecutorConfigItem(this._editExecutorConfigItem, destTestModuleClassName);

                    this.UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItem, this._editExecutorConfigItem);
                }
                else
                {
                    if (this._editExecutorConfigItem.TestModuleClassName != destTestModuleClassName)
                    {
                        var dr = MessageBox.Show
                             (
                             $"是否将原有测试模块类型[{this._editExecutorConfigItem.TestModuleClassName}]更新为[{destTestModuleClassName}]?" +
                             $"\r\n(注意:若确定替换则原有的附带算子将重置)",
                             "更新测试模块类型",
                             MessageBoxButtons.YesNoCancel,
                             MessageBoxIcon.Question)
                             ;
                        if (dr == DialogResult.Yes)
                        {

                        }
                        else
                        {
                            return;
                        }
                    }
                    this._appInteration.UpdateTestModuleClassOfExecutorConfigItem(this._editExecutorConfigItem, destTestModuleClassName);

                    this.UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItem, this._editExecutorConfigItem);

                }
                //this.ExpandAllTestExecutorConfigTree();
                this.ExpandAllTree(this.treeView_TestExecutorConfigItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void btn_UpdateCalculator_To_TestExecutorConfigTree_Click(object sender, EventArgs e)
        {
            try
            {
                const int onlyCellCount = 1;
                if (this.dgv_supportedCalculatorClass.SelectedCells.Count != onlyCellCount)
                {
                    MessageBox.Show("选择算子错误,更新算子节点失败!");
                    return;
                }
                var destCalculatorName = this.dgv_supportedCalculatorClass.SelectedCells[0].Value.ToString();
                if (string.IsNullOrEmpty(destCalculatorName))
                {
                    MessageBox.Show("选择算子错误,更新算子节点失败!");
                    return;
                }
                if (this._editExecutorConfigItem == null)
                {
                    MessageBox.Show("没有正在编辑的测试项,更新算子节点失败!");
                    return;
                }
                if (string.IsNullOrEmpty(this._editExecutorConfigItem.TestModuleClassName))
                {
                    MessageBox.Show("请先为测试项添加测试模块,更新算子节点失败!");
                    return;
                }
                var ok = this._appInteration.TryAddCalculatorToExecutorConfigItem(this._editExecutorConfigItem, destCalculatorName);
                if (ok)
                {
                    this.UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItem, this._editExecutorConfigItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void btn_SaveTestExecutorConfigTree_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();


            sfd.InitialDirectory = Path.GetFullPath(_core.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_EXECUTOR_FILES_PATH));
            var defaultFileName = string.Concat(this._editExecutorConfigItem.TestExecutorName, FileExtension.XML);
            sfd.FileName = defaultFileName;
            var dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string finalFileName = sfd.FileName;
                finalFileName = Path.ChangeExtension(finalFileName, "xml");

                if (finalFileName.Equals(defaultFileName))
                {
                    this._editExecutorConfigItem.Save(finalFileName);
                    MessageBox.Show("保存成功:\r\n\r\n" + finalFileName);

                    RefreshExecutorFilesListView();
                }
                else
                {
                    var finalTestExecutorItemName = Path.GetFileNameWithoutExtension(finalFileName);
                    var renameDR = MessageBox.Show($"是否将测试项重命名为[{finalTestExecutorItemName}]?" +
                          $"\r\n提示:文件名与测试项名必须保持一致" +
                          $"\r\n若选是 则将测试项名重命名为[{finalTestExecutorItemName}]并存储." +
                          $"\r\n若选否 则放弃保存.", "保存提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);

                    if (renameDR == DialogResult.Yes)
                    {
                        this._editExecutorConfigItem.TestExecutorName = finalTestExecutorItemName;
                        this._editExecutorConfigItem.Save(finalFileName);
                        MessageBox.Show("保存成功:\r\n\r\n" + finalFileName);
                        RefreshExecutorFilesListView();
                        this.UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItem, this._editExecutorConfigItem);
                    }
                }


            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.treeView_TestExecutorConfigItem.SelectedNode == null)
                {
                    MessageBox.Show($"未选中任何节点!");return;
                }
                const int lvl_calcNode = 2;
                if (this.treeView_TestExecutorConfigItem.SelectedNode?.Level == lvl_calcNode)
                {
                    var dr = MessageBox.Show($"确定删除算子节点[{this.treeView_TestExecutorConfigItem.SelectedNode.FullPath}]?", "删除算子节点", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                    var nodeIndex = this.treeView_TestExecutorConfigItem.SelectedNode.Index;
                    if (this._editExecutorConfigItem.CalculatorCollection.IsIndexInRange(nodeIndex))
                    {
                        this._editExecutorConfigItem.CalculatorCollection.RemoveAtIndex(nodeIndex);
                        this.UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItem, this._editExecutorConfigItem);
                        this.ExpandAllTree(treeView_TestExecutorConfigItem);
                    }
                    else
                    {
                        MessageBox.Show("算子级节点索引超出范围!");
                    }

                }

                const int lvl_moduleNode = 1;
                if (this.treeView_TestExecutorConfigItem.SelectedNode?.Level == lvl_moduleNode)
                {
                    var dr = MessageBox.Show($"确定删除测试模块节点[{this.treeView_TestExecutorConfigItem.SelectedNode.FullPath}]?", "删除测试模块节点", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                    if (dr != DialogResult.Yes)
                    {
                        return;
                    }
                    this._editExecutorConfigItem.Clear();
                    this.UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItem, this._editExecutorConfigItem);
                    this.ExpandAllTree(treeView_TestExecutorConfigItem);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除节点错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }
    }
}