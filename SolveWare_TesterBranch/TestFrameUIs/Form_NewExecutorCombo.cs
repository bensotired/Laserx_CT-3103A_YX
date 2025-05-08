using LX_BurnInSolution.Utilities;
using System;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_NewExecutorCombo : Form
    {
        public Form_NewExecutorCombo()
        {
            InitializeComponent();
        }
        public Form_NewExecutorCombo(string[] testPluginKeys) : this()
        {
            //if (testPluginKeys?.Length > 0)
            //{
            //    this.cb_testPluginTypes.Items.AddRange(testPluginKeys);
            //    this.cb_testPluginTypes.SelectedIndex = 0;
            //}
        }
        public string NewName { get; private set; }
        //public string NewApplicableTestPluginName { get; private set; }
        private void btn_confirmNewTestExecutorConfigItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tb_newTestExecutorComboName.Text))
            {
                MessageBox.Show("测试链名不能为空!");
                return;
            }
            //if (this.cb_testPluginTypes.SelectedItem == null)
            //{
            //    MessageBox.Show("测试链适用组件类型不能为空!");
            //    return;
            //}
            //if (string.IsNullOrEmpty(this.cb_testPluginTypes.SelectedItem.ToString()))
            //{
            //    MessageBox.Show("测试链适用组件类型不能为空!");
            //    return;
            //}

            this.NewName = this.tb_newTestExecutorComboName.Text;
            //this.NewApplicableTestPluginName = this.cb_testPluginTypes.SelectedItem.ToString();

            this.DialogResult = DialogResult.OK;
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void Form_NewExecutorConfigItem_Load(object sender, EventArgs e)
        {
            this.tb_newTestExecutorComboName.Text = $"NEW_TestExecutorCombo_{BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)} ";
        }
    }
}