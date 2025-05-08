using SolveWare_TestComponents.Specification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_TestSpecification
{
    public partial class Form_TestSpecInfo : Form
    {
        public Form_TestSpecInfo(TestSpecification tempSpec)
        {
            InitializeComponent();
            this.txt_testSpecName.Text = tempSpec.Name;
            this.txt_testSpecVer.Text = tempSpec.Version;
        }

        public string TestSpecName { get; private set; }

        public string TestSpecVersion { get; private set; }

        public List<string> SpecNameList { get; set; }


        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_confirmNewSpec_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_testSpecName.Text))
            {
                MessageBox.Show("测试规格名称不能为空!");
                return;
            }

            if (SpecNameList!=null)
            {
                if (SpecNameList.Contains(txt_testSpecName.Text.Trim()))
                {
                    MessageBox.Show("已存在相同测试规格名称");
                    return;
                }
            }

            if (string.IsNullOrEmpty(this.txt_testSpecVer.Text))
            {
                MessageBox.Show("测试规格版本号不能为空!");
                return;
            }
            this.TestSpecName = this.txt_testSpecName.Text;
            this.TestSpecVersion = this.txt_testSpecVer.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}