using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Threading;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_BurnInCommon;
using General.Storage;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{

    // You may want to change the calculator name to something unique if needed.
    [SupportedCalculator("TestModule_OES")]

    #region Instruments 
    [StaticResource(ResourceItemType.IO, "PD_3", "切换PD")]
    [ConfigurableInstrument("OpticalSwitch", "OSwitch", "用于切换光路(1*4切换器)")]
    [ConfigurableInstrument("PXISourceMeter_4143", "SOA1", "SOA1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "SOA2", "SOA2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "LP", "LP")]
    [ConfigurableInstrument("PXISourceMeter_4143", "PH1", "PH1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "PH2", "PH2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MIRROR1", "MIRROR1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "MIRROR2", "MIRROR2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "BIAS1", "BIAS1")]
    [ConfigurableInstrument("PXISourceMeter_4143", "BIAS2", "BIAS2")]
    [ConfigurableInstrument("PXISourceMeter_4143", "GAIN", "GAIN")]
    [ConfigurableInstrument("FWM8612", "FWM8612", "波长计")]
    #endregion

    public class TestModule_OESDiagonalTuning : TestModule_OESBase
    {
        #region instance vars
        QWLT2_TestData QwltSettings { get; set; }
        int optExposureTime;

        //instrument drivers
        PXISourceMeter_4143 SOA1 { get { return (PXISourceMeter_4143)this.ModuleResource["SOA1"]; } }
        PXISourceMeter_4143 SOA2 { get { return (PXISourceMeter_4143)this.ModuleResource["SOA2"]; } }
        PXISourceMeter_4143 LP { get { return (PXISourceMeter_4143)this.ModuleResource["LP"]; } }
        PXISourceMeter_4143 PH1 { get { return (PXISourceMeter_4143)this.ModuleResource["PH1"]; } }
        PXISourceMeter_4143 PH2 { get { return (PXISourceMeter_4143)this.ModuleResource["PH2"]; } }
        PXISourceMeter_4143 MIRROR1 { get { return (PXISourceMeter_4143)this.ModuleResource["MIRROR1"]; } }
        PXISourceMeter_4143 MIRROR2 { get { return (PXISourceMeter_4143)this.ModuleResource["MIRROR2"]; } }
        PXISourceMeter_4143 BIAS1 { get { return (PXISourceMeter_4143)this.ModuleResource["BIAS1"]; } }
        PXISourceMeter_4143 BIAS2 { get { return (PXISourceMeter_4143)this.ModuleResource["BIAS2"]; } }
        PXISourceMeter_4143 GAIN { get { return (PXISourceMeter_4143)this.ModuleResource["GAIN"]; } }
        FWM8612 FWM8612 { get { return (FWM8612)this.ModuleResource["FWM8612"]; } }
        #endregion

        #region constructor
        public TestModule_OESDiagonalTuning() : base() { }
        #endregion



        #region Wavelength meter exposure calibration
        private void TurnOnLaser() {
            GAIN.AssignmentMode_Current(QwltSettings.GAIN, 2.5);
            LP.AssignmentMode_Current(QwltSettings.LP, 2.5);
            MIRROR1.AssignmentMode_Current(QwltSettings.MIRROR1, 2.5);
            MIRROR2.AssignmentMode_Current(QwltSettings.MIRROR2, 2.5);

            SOA1.AssignmentMode_Current(QwltSettings.SOA1, 2.5);
            SOA2.AssignmentMode_Current(QwltSettings.SOA2, 2.5);

            PH1.AssignmentMode_Current(QwltSettings.PH1, 2.5);
            PH2.AssignmentMode_Current(QwltSettings.PH2, 2.5);

            BIAS1.AssignmentMode_Voltage(QwltSettings.BIAS1, 50);
            BIAS2.AssignmentMode_Voltage(QwltSettings.BIAS2, 50);
        }

        private void TurnOffLaser()
        {
            GAIN.Reset();
            LP.Reset();
            MIRROR1.Reset();
            MIRROR2.Reset();    
            SOA1.Reset();   
            SOA2.Reset();   
            PH1.Reset();
            PH2.Reset();
            BIAS1.Reset();
            BIAS2.Reset();
        }

        private bool ConnectToWlm()
        {
            return FWM8612.IsOnline;
        }

        private void OptimizeWlmExposureTime()
        {
            if (ConnectToWlm())
            {
                TurnOnLaser();
                optExposureTime = FWM8612.ConfigureOptimalExposureTime();
                TurnOffLaser();
                this.Log_Global($"Optimal WLM exposure time: {optExposureTime} usec");
            }
        }
        #endregion



        #region Overrides
        protected override bool RunAutoTestCore()
        {
            bool autoTestResult = false;

            if (frmMain != null)
            {
                frmMain.Show();
                Application.DoEvents();
                Thread.Sleep(1000);
                SetChipInformation();
                SetSavePath();
                PopulateConstantSettings();
                //OptimizeWlmExposureTime();
                autoTestResult = frmMain.ExecuteMirrorDiagonalCoarseTuning();
                frmMain.Hide(); // IMPORTANT: use Hide() to avoid disposing the form.
            }
            else
            {
                this.Log_Global("OES测试窗体未初始化...\r\n The OES test form is not initialized...");
            }

            return autoTestResult;
        }

        public override void Run(CancellationToken token)
        {
            OptimizeWlmExposureTime();
            if (optExposureTime > 500)
            {
                return;
            }
            base.Run(token);
        }

        private void PopulateConstantSettings()
        {
            if (frmMain != null)
            {
                frmMain.GainCurrent = QwltSettings.GAIN;
                frmMain.LaserPhaseCurrent = QwltSettings.LP;
                frmMain.Phase1Current = QwltSettings.PH1;
                frmMain.Phase2Current = QwltSettings.PH2;
                frmMain.Soa1Current = QwltSettings.SOA1;
                frmMain.Soa2Current = QwltSettings.SOA2;
                frmMain.Mzm1Voltage = QwltSettings.BIAS1;
                frmMain.Mzm2Voltage = QwltSettings.BIAS2;
            }

        }

        /// <summary>
        /// Diagonal-tuning uses a simpler folder (no Fine_tuning subfolder).
        /// </summary>
        private void SetSavePath()
        {
            General.modGlobals.PATH_TO_TEST_ANALYSIS =
                System.Windows.Forms.Application.StartupPath +
                $"\\Data\\{this.SerialNumber}\\";
        }

        protected override void ReadAdditionalStreamData(IDeviceStreamDataBase dutStreamData)
        {
            base.ReadAdditionalStreamData(dutStreamData);
            QwltSettings = QWLT2_TestData.GetQwlt2_Data(dutStreamData);
        }
        #endregion


    }
}
