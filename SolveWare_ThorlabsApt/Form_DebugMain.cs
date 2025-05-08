using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MG17NanoTrakLib;

namespace SolveWare_BurnInInstruments
{
    public partial class Form_DebugMain : Form
    {
        public Form_DebugMain()
        {
            InitializeComponent();
        }

        private void Form_DebugMain_Load(object sender, EventArgs e)
        {
            try
            {
                //this._MG17SystemClass = new MG17SystemLib.MG17SystemClass();
                //this._MG17SystemClass.StartCtrl();
                this.axMG17NanoTrak1.StartCtrl();
            }
            catch (Exception ex)
            {

            }
        }

        private void axMG17NanoTrak1_Enter(object sender, EventArgs e)
        {

        }
    }
}
