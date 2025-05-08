using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_CoarseTuning
{
    public partial class Form_Main : Form
    {
        CoarseTuning coarseTuning = new CoarseTuning();
        public Form_Main()
        {
            InitializeComponent();
        }

        private void bt_run_Click(object sender, EventArgs e)
        {
            try
            {
                var resutl = coarseTuning.ReadMirrorMapWavelengthFileAndSetupItuHelper();
                if (!resutl)
                {
                    return;
                }
                coarseTuning.ClearChart(ref this.Chart_Groups_main, ref this.Chart_Midlines_main, ref this.Chart_LabeledPoints_main);

                coarseTuning.GroupWavelegnthValues();
                coarseTuning.PopulateMidlines();
                coarseTuning.GetAllItuChannels();

                coarseTuning.PlotWavelengthGroups(ref this.Chart_Groups_main);
                coarseTuning.PlotMidlines(ref this.Chart_Midlines_main);
                coarseTuning.PlotLabeledItuChannels(ref this.Chart_LabeledPoints_main);
            }
            catch (Exception ex)
            {
                if (ex is OutOfMemoryException)
                {
                    var currentProcess = Process.GetCurrentProcess();
                    var msg = $"Out of Memory Exception! \r\n" +
                              $"Private Memory Size: {currentProcess.PrivateMemorySize64} \r\n" +
                              $"Virtual Memory Size: {currentProcess.VirtualMemorySize64} \r\n" +
                              $"Working Set: {currentProcess.WorkingSet64} \r\n";
                    throw new Exception(msg);
                }
            }
        }

        private void bt_savecsv_Click(object sender, EventArgs e)
        {
            try
            {
                coarseTuning.SaveToCsv();
                MessageBox.Show("保存成功！\r\ncoarseTuning.GetPath()");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败！- [{ex.Message}]-[{ex.StackTrace}]");
            }
        }
    }
}
