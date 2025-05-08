using SolveWare_BurnInAppInterface;
using SolveWare_TestComponents.Data;
using System;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_InsertCalculatorIntoExecutorCombo : Form, ITesterAppUI
    {
        TestFrameManager _appInteration;
 
        string _editingTestModuleTypeName = string.Empty;
        public string CalculatorTypeName { get; private set; }
        public string CalculatorParamName { get; private set; }
        public Form_InsertCalculatorIntoExecutorCombo(string editingTestModuleTypeName)
        {
            InitializeComponent();
            if(string.IsNullOrEmpty(editingTestModuleTypeName) == true)
            {
                return;
            }
            _editingTestModuleTypeName = editingTestModuleTypeName.Split(' ')[0];
            this.lbl_info.Text = $"当前测试模块[{editingTestModuleTypeName}]";
        }
        private void Form_TestExecutorItem_Preview_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_editingTestModuleTypeName) == true)
            {
                return;
            }
            //this.Text = $"测试模块内容预览[{_editingTestModuleTypeName}]";
            try
            {
                const int valColIndex = 0;
                var fileDict = _appInteration.GetSupportedTestModuleClass();

                if (fileDict.ContainsKey(_editingTestModuleTypeName) == true)
                {
                    dgv_supportedCalculatorClass.Rows.Clear();
                    foreach (var calc in fileDict[_editingTestModuleTypeName])
                    {
                        var rIndex = dgv_supportedCalculatorClass.Rows.Add();
                        dgv_supportedCalculatorClass.Rows[rIndex].Cells[valColIndex].Value = calc;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RefreshSupportedTestModuleClass错误:[{ex.Message}+{ex.StackTrace}]!");
            }

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
 
        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty( this.tb_calculatorParamName.Text) ==true||
                string.IsNullOrEmpty(this.tb_calculatorTypeName.Text) == true)
            {
                MessageBox.Show($"算子类型或算子参数名为空!");
                return;
            }
            this.CalculatorTypeName = this.tb_calculatorTypeName.Text;
            this.CalculatorParamName = this.tb_calculatorParamName.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dgv_supportedCalculatorClass_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
         
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void dgv_supportedCalculatorClass_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    this.tb_calculatorTypeName.Text = this.dgv_supportedCalculatorClass.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                }
            }
            catch
            {
            }
        }
    }
}