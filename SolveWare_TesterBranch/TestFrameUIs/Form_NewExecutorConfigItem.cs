using LX_BurnInSolution.Utilities;
using System;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_NewExecutorConfigItem : Form
    {
        public Form_NewExecutorConfigItem()
        {
            InitializeComponent();
        }
        public string NewTestExecutorConfigName { get; private set; }
        private void btn_confirmNewTestExecutorConfigItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tb_newTestExecutorConfigItemName.Text))
            {
                MessageBox.Show("测试项名不能为空!");
                return;
            }
            this.NewTestExecutorConfigName = this.tb_newTestExecutorConfigItemName.Text;

            this.DialogResult = DialogResult.OK;
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void Form_NewExecutorConfigItem_Load(object sender, EventArgs e)
        {
            this.tb_newTestExecutorConfigItemName.Text = $"NEW_TestExecutorConfig_{BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)} ";
        }
    }
}