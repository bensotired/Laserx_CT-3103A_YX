using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_CreateNewProductConfig : Form
    {
        public Form_CreateNewProductConfig()
        {
            InitializeComponent();
        }

        public string NewProductName { get; private set; }
 
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_confirmNewFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_newName.Text))
            {
                MessageBox.Show("产品名称不能为空!");
                return;
            }
     

            this.NewProductName = this.txt_newName.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

    }
}