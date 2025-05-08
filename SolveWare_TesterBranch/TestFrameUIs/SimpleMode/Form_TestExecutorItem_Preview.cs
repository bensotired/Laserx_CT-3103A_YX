using SolveWare_BurnInAppInterface;
using SolveWare_TestComponents.Data;
using System;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_TestExecutorItem_Preview : Form, ITesterAppUI
    {
        TestFrameManager _appInteration;
        ExecutorConfigItem _executorConfigItem;
 
        public Form_TestExecutorItem_Preview(ExecutorConfigItem  executorConfigItem)
        {
            InitializeComponent();
            _executorConfigItem = executorConfigItem;
        }
        private void Form_TestExecutorItem_Preview_Load(object sender, EventArgs e)
        {
            this.Text = $"测试模块内容预览[{_executorConfigItem.TestExecutorName}]";
            UpdateTreeView_ExecutorConfigItem(this.treeView_TestExecutorConfigItemDetails, _executorConfigItem);
        }

        public void ConnectToAppInteration(ITesterAppPluginInteration app)
        {
            _appInteration = (TestFrameManager)app;
        }

        public void ConnectToCore(ITesterCoreInteration core)
        {
            
        }

        public void DisconnectFromCore(ITesterCoreInteration core)
        {
         
        }

        public void RefreshOnce()
        {
            
        }

        void InitializeTree(TreeView treeView)
        {
            treeView.Nodes.Clear();

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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }
    }
}