using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;

namespace SolveWare_TestPackage
{
    // You may want to change the calculator name to something unique if needed.
    [SupportedCalculator("TestModule_OES")]
    [StaticResource(ResourceItemType.IO, "PD_3", "切换PD")]
    [ConfigurableInstrument("OpticalSwitch", "OSwitch", "用于切换光路(1*4切换器)")]
    public class TestModule_OESDiagonalTuning : TestModule_OESBase
    {
       
        public TestModule_OESDiagonalTuning() : base() { }

        protected override bool RunAutoTestCore()
        {
            bool autoTestResult = false;

            if (frmMain != null)
            {
                frmMain.Show();

                SetChipInformation();
                SetSavePath();

                autoTestResult = frmMain.ExecuteMirrorDiagonalCoarseTuning();

                frmMain.Hide(); // IMPORTANT: use Hide() to avoid disposing the form.
            }
            else
            {
                this.Log_Global("OES测试窗体未初始化...\r\n The OES test form is not initialized...");
            }

            return autoTestResult;
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
    }
}
