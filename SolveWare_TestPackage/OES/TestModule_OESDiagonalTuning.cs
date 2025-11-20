using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Threading;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_BurnInCommon;
using General.Storage;

namespace SolveWare_TestPackage
{
    // You may want to change the calculator name to something unique if needed.
    [SupportedCalculator("TestModule_OES")]
    [StaticResource(ResourceItemType.IO, "PD_3", "切换PD")]
    [ConfigurableInstrument("OpticalSwitch", "OSwitch", "用于切换光路(1*4切换器)")]
    public class TestModule_OESDiagonalTuning : TestModule_OESBase
    {
        #region instance vars
        QWLT2_TestData QwltSettings { get; set; }
        #endregion
        public TestModule_OESDiagonalTuning() : base() { }

        protected override bool RunAutoTestCore()
        {
            bool autoTestResult = false;

            if (frmMain != null)
            {
                frmMain.Show();

                SetChipInformation();
                SetSavePath();
                PopulateConstantSettings();
                autoTestResult = frmMain.ExecuteMirrorDiagonalCoarseTuning();
                frmMain.Hide(); // IMPORTANT: use Hide() to avoid disposing the form.
            }
            else
            {
                this.Log_Global("OES测试窗体未初始化...\r\n The OES test form is not initialized...");
            }

            return autoTestResult;
        }

        private void PopulateConstantSettings()
        {
            if (frmMain != null) {
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
    }
}
