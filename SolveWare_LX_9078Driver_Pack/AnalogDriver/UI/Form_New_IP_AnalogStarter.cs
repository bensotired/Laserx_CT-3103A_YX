using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_Analog
{
    public partial class Form_New_IP_AnalogStarter : Form
    {
        public Form_New_IP_AnalogStarter()
        {
            InitializeComponent();
        }

        public string New_INP_Analog_Name { get; private set; }
        public short New_INP_Analog_CardNo { get; private set; }
        public short New_INP_Analog_Slave { get; private set; }
        public short New_INP_Analog_BitNo { get; private set; }
        public bool New_INP_Analog_IsExtenalAnalog { get; private set; }

        private void Form_NewAxisStarter_Load(object sender, EventArgs e)
        {
            this.tb_newAxisName.Text = $"NEW_INP_Analog_{DateTime.Now:YYYYMMDD_HHmmss}";
        }

        private void btn_confirmNewAxis_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tb_newAxisName.Text))
            {
                MessageBox.Show("Analog点名字不能为空!");
                return;
            }
            this.New_INP_Analog_Name = this.tb_newAxisName.Text;
            this.New_INP_Analog_CardNo = Convert.ToInt16(this.nud_CardNo.Value);
            this.New_INP_Analog_Slave = Convert.ToInt16(this.nud_ioSlave.Value);
            this.New_INP_Analog_BitNo = Convert.ToInt16(this.nud_ioNo.Value);
            this.New_INP_Analog_IsExtenalAnalog = cb_externIOCard.Checked;

            this.DialogResult = DialogResult.OK;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}