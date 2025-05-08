using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_BurnInInstruments.TLX1.TestModule
{
    public partial class Form_SetTLX1Wavelength : Form
    {
        public double TLX1Power { get; set; }
        public Form_SetTLX1Wavelength()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            double poswe = 0;
            //bool _ok = double.TryParse(txt_wavelength.Text, out wavelength);
            if (double.TryParse(txt_power.Text, out poswe))
            {
                TLX1Power = poswe;
            }
            this.Close();
        }
    }
}
