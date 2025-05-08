using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_Motion
{
    public partial class Form_NewAxisStarter : Form
    {
        public Form_NewAxisStarter()
        {
            InitializeComponent();
        }
        public string NewAxisName { get; private set; }
        public short NewAxisCardNo { get; private set; }
        public short NewAxisNo { get; private set; }
        public bool CopyCurrentAxisConfig { get; private set; }
        private void Form_NewAxisStarter_Load(object sender, EventArgs e)
        {
            this.tb_newAxisName.Text = $"NEW_AXIS_{DateTime.Now:YYYYMMDD_HHmmss}";

        }

        private void btn_confirmNewAxis_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tb_newAxisName.Text))
            {
                MessageBox.Show("轴名字不能为空!");
                return;
            }
            this.NewAxisName = this.tb_newAxisName.Text;
            this.NewAxisCardNo = Convert.ToInt16(this.nud_CardNo.Value);
            this.NewAxisNo = Convert.ToInt16(this.nud_AxisNo.Value);
            this.CopyCurrentAxisConfig = this.cb_copyCurrentAxisConfig.Checked;
            this.DialogResult = DialogResult.OK;

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}