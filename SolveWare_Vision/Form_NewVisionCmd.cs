using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SolveWare_Vision
{
    public partial class Form_NewVisionCmd : Form
    {
        public Form_NewVisionCmd( string visionCmdPurpose)
        {
            InitializeComponent();
            this.Text =$"新增{visionCmdPurpose}视觉模板命令" ;
        }
 
        public string NewVisionCmd { get; private set; }

        public List<string> CurrnentVisionCmdList { get; set; }


        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_confirmNewSpec_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_NewVisionCmd.Text))
            {
                MessageBox.Show("视觉模板命令不能为空!");
                return;
            }

            if (CurrnentVisionCmdList!=null)
            {
                if (CurrnentVisionCmdList.Contains(txt_NewVisionCmd.Text.Trim()))
                {
                    MessageBox.Show("已存在相同视觉模板命令!");
                    return;
                }
            }

          
            this.NewVisionCmd = this.txt_NewVisionCmd.Text;
         
            this.DialogResult = DialogResult.OK;
        }
    }
}