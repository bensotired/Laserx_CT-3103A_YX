using System;
using System.Diagnostics;
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
        public void Set_QWLT2_SettingData
        (
            double GainCurrent,
            double Soa1Current,
            double Soa2Current,
            double LaserPhaseCurrent,
            double Phase1Current,
            double Phase2Current,
            double Mzm1VBias,
            double Mzm2VBias
        )
        {
            coarseTuning.myQWLT2_SettingData.GainCurrent = GainCurrent;
            coarseTuning.myQWLT2_SettingData.Soa1Current = Soa1Current;
            coarseTuning.myQWLT2_SettingData.Soa2Current = Soa2Current;
            coarseTuning.myQWLT2_SettingData.LaserPhaseCurrent = LaserPhaseCurrent;
            coarseTuning.myQWLT2_SettingData.Phase1Current = Phase1Current;
            coarseTuning.myQWLT2_SettingData.Phase2Current = Phase2Current;
            coarseTuning.myQWLT2_SettingData.Mzm1VBias = Mzm1VBias;
            coarseTuning.myQWLT2_SettingData.Mzm2VBias = Mzm2VBias;
        }
        // the function to save csv
        public (string,string) SaveCVS(string SerialNumber, string MaskName, string WaferName, string ChipName, string OeskID, double temp, DateTime time)
        {
            try
            {
             return   coarseTuning.SaveToCsv(SerialNumber, MaskName, WaferName, ChipName, OeskID, temp, time);
            }
            catch (Exception ex)
            {
                throw new Exception($"CoarseTuning SaveCVS Eorr [{ex.Message}]");
            }
            return (string.Empty, string.Empty);
        }
  
    }
}
