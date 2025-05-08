using SolveWare_BurnInAppInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
   
    public partial class Form_changeModelName : Form
    {
        public string NewName { get; set; } = string.Empty;
        public string OldName { get; set; } = string.Empty;
        public bool CopyName_OK { get; set; } = false;
        public Form_changeModelName(string name) 
        {
            InitializeComponent();
            txt_ModelName.Text = name;
            OldName = name;            
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (txt_ModelName.Text.Trim()!=string.Empty)
            {
                NewName = txt_ModelName.Text.Trim();
            }
            else
            {
                NewName = OldName;
            }
            CopyName_OK = true;
            this.Close();
        }    

        private void Form_changeModelName_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (txt_ModelName.Text.Trim() != string.Empty&& CopyName_OK)
            {
                CopyName_OK = true;
                NewName = txt_ModelName.Text.Trim();
            }
            else
            {
                CopyName_OK = false;
               // NewName = OldName;
            }
           

        }

        private void btn_NoCopy_Click(object sender, EventArgs e)
        {
            CopyName_OK = false;
            this.Close();
        }
    }
}
