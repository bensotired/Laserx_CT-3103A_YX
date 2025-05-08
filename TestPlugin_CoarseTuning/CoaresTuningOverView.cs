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
    public partial class CoaresTuningOverView : Form
    {
        CoarseTuning coarseTuning = new CoarseTuning();
        private string Path { get; set; }
        public CoaresTuningOverView(string path)
        {
            try
            {
                InitializeComponent();
                Path = path;

                coarseTuning.ClearChart(ref this.Chart_Groups, ref this.Chart_Midlines, ref this.Chart_LabeledPoints);
                coarseTuning.ReadMirrorMapWavelengthFileAndSetupItuHelper(Path);
                coarseTuning.GroupWavelegnthValues();
                coarseTuning.PopulateMidlines();
                coarseTuning.GetAllItuChannels();
                coarseTuning.PlotWavelengthGroups(ref this.Chart_Groups);
                coarseTuning.PlotMidlines(ref this.Chart_Midlines);
                coarseTuning.PlotLabeledItuChannels(ref this.Chart_LabeledPoints);
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
                else
                {
                    throw new Exception($"CoarseTuning Chart Eorr [{ex.Message}]");
                }
            }
        }

        private void CoaresTuningOverView_Load(object sender, EventArgs e)
        {
            //coarseTuning.ReadMirrorMapWavelengthFileAndSetupItuHelper(Path);
            //coarseTuning.GroupWavelegnthValues();
            //coarseTuning.PopulateMidlines();
            //coarseTuning.GetAllItuChannels();
            //coarseTuning.PlotWavelengthGroups(ref this.Chart_Groups);
            //coarseTuning.PlotMidlines(ref this.Chart_Midlines);
            //coarseTuning.PlotLabeledItuChannels(ref this.Chart_LabeledPoints);
        }
        public string SaveCVS(string SerialNumber, string MaskName, string WaferName, string ChipName, string OeskID, double temp, DateTime time)
        {
            try
            {
                coarseTuning.SaveToCsv(SerialNumber, MaskName, WaferName, ChipName, OeskID, temp, time);
            }
            catch (Exception ex)
            {
                throw new Exception($"CoarseTuning SaveCVS Eorr [{ex.Message}]");
            }
            return coarseTuning.GetPath();
        }
    }
}
