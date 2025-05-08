using AxMG17NanoTrakLib;
using SolveWare_BurnInAppInterface;
using System.Windows.Forms;
using System;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;

namespace SolveWare_BurnInInstruments
{
    public partial class Thorlabs_NanoTrakUI : Form,IAccessPermissionLevel
    {
        public Thorlabs_NanoTrakUI()
        {
            InitializeComponent();
        }
        public AccessPermissionLevel APL
        {
            get { return AccessPermissionLevel.None; }
        }
        public AxMG17NanoTrak NanoTrakControl
        {
            get
            { return this.axMG17NanoTrak1; }
        }      

        private void Thorlabs_NanoTrakUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }       
    }
}
