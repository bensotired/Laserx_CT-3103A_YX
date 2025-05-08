using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TestPlugin_Demo
{
    public partial class Frm_ResetPlatform : Form_MainPage_TestPlugin<TestPluginWorker_CT3103>
    {
        public bool _stop = false;
        public Frm_ResetPlatform()
        {
            InitializeComponent();
        }
        public delegate bool HomeStation();
        public event HomeStation Homestation;
        public delegate void CancelHomeStation();
        public event CancelHomeStation CancelHome;
        public TestPluginResourceProvider_CT3103 LocalResource { get; set; }
        private void Frm_ResetPlatform_Load(object sender, EventArgs e)
        {
            this.timer1.Interval = 10;
            this.timer1.Enabled = true;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    bool a = false;
                    a = Homestation();
                    //Thread.Sleep(10000);
                    if (a)
                    {
                        this._plugin.Interation.RunStatus = SolveWare_TestPlugin.PluginRunStatus.Idle;
                        MessageBox.Show("整机台复位结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        this._stop = true;
                    }
                    else
                    {
                        LocalResource.IOs[IONameEnum_CT3103.TWR_YEL].TurnOn(false);
                        LocalResource.IOs[IONameEnum_CT3103.TWR_RED].TurnOn(true);
                        LocalResource.IOs[IONameEnum_CT3103.TWR_GRN].TurnOn(false);
                        LocalResource.IOs[IONameEnum_CT3103.BEEP].TurnOn(true);
                        MessageBox.Show("整机台复位失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        
                        LocalResource.IOs[IONameEnum_CT3103.BEEP].TurnOn(false);
                        this._stop = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"整机台复位错误:[{ex.Message}-{ex.StackTrace}]!");
                    this._stop = true;
                }
                
            });

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Left = label2.Left - 3;
            if (label2.Right < 0)
            {
                label2.Left = this.Width;
            }
            if (_stop)
            {
                this.Close();
            }
        }

        private void bt_CancelHome_Click(object sender, EventArgs e)
        {
            CancelHome();
            this.Close();
        }
    }
}
