using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_RecipeInfo : Form
    {
        public Form_RecipeInfo()
        {
            InitializeComponent();
        }

        public string FileName { get; private set; }


        public List<string> FileNameList { get; set; }


        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_confirmNewFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_fileName.Text))
            {
                MessageBox.Show("文件名称不能为空!");
                return;
            }
            if (FileNameList!=null)
            {
                if (FileNameList.Contains(txt_fileName.Text.Trim()))
                {
                    MessageBox.Show("当前目录下已存在重名文件");
                    return;
                }
            }

            this.FileName = this.txt_fileName.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

    }
}