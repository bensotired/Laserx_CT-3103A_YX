using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Threading;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_BurnInCommon;
using System.Windows.Forms;

namespace SolveWare_TestPackage

{
    // You may want to change the calculator name to something unique if needed.
    [SupportedCalculator("TestModule_OES")]
    [StaticResource(ResourceItemType.IO, "PD_3", "切换PD")]
    [ConfigurableInstrument("FWM8612", "FWM8612", "波长计")]

    public class TestModule_OES_FineTuning : TestModule_OESBase
    {

        public TestModule_OES_FineTuning() : base() { }

        FWM8612 wlm { get { return (FWM8612)this.ModuleResource["FWM8612"]; } }

        protected override void ReadAdditionalStreamData(IDeviceStreamDataBase dutStreamData)
        {
            
        }

        protected override bool RunAutoTestCore()
        {
            bool autoTestResult = false;

            if (frmMain != null)
            {
                frmMain.Show();
                Application.DoEvents();
                Thread.Sleep(1);
                SetChipInformation();
                SetSavePath();

                autoTestResult = frmMain.ExecuteFineTuningTest();

                // IMPORTANT: Hide instead of Close() so the form is not disposed.
                frmMain.Hide();
            }
            else
            {
                this.Log_Global("OES测试窗体未初始化...\r\n The OES test form is not initialized...");
            }

            return autoTestResult;
        }

        public override void Run(CancellationToken token)
        {
            wlm.SetAutomaticExposure(Auto.On);
            base.Run(token);
        }

        /// <summary>
        /// Fine-tuning saves into a Fine_tuning subfolder.
        /// </summary>
        private void SetSavePath()
        {
            General.modGlobals.PATH_TO_TEST_ANALYSIS =
                System.Windows.Forms.Application.StartupPath +
                $"\\Data\\{this.SerialNumber}";
        }

    }
}
