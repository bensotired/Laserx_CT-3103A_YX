using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_IO
{
    public partial class Form_New_OP_IOStarter : Form
    {
        public Form_New_OP_IOStarter()
        {
            InitializeComponent();
        }
        public string New_OP_IO_Name { get; private set; }
        public short New_OP_IO_CardNo { get; private set; }
        public short New_OP_IO_Slave { get; private set; }
        public short New_OP_IO_No { get; private set; }
        public short New_OP_IO_Logic { get; private set; }
        public bool New_OP_IO_IsExtenalIO { get; private set; }
        private void Form_NewAxisStarter_Load(object sender, EventArgs e)
        {
            this.tb_newAxisName.Text = $"NEW_OP_IO_{DateTime.Now:YYYYMMDD_HHmmss}";

        }

        private void btn_confirmNewAxis_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tb_newAxisName.Text))
            {
                MessageBox.Show("IO点名字不能为空!");
                return;
            }
            this.New_OP_IO_Name = this.tb_newAxisName.Text;
            this.New_OP_IO_CardNo = Convert.ToInt16(this.nud_CardNo.Value);
            this.New_OP_IO_Slave= Convert.ToInt16(this.nud_ioSlave.Value);
            this.New_OP_IO_No = Convert.ToInt16(this.nud_ioNo.Value);
            this.New_OP_IO_Logic = rbtnLowLevel.Checked ? (short)0 : (short)1;
            this.New_OP_IO_IsExtenalIO = cb_externIOCard.Checked;
            this.DialogResult = DialogResult.OK;

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}