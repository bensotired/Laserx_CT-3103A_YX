using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Threading;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_BurnInCommon;

namespace SolveWare_TestPackage
{
    public class TestModule_OES_FineTuning : TestModule_OESBase
    {

        public TestModule_OES_FineTuning() : base() { }


        string MirrorMapWlFileName { get; set; }
        string CoarseTuningMidlineFileName { get; set; }
        string CoarseTuningDeviationsFileName { get; set; }

        protected override void ReadAdditionalStreamData(IDeviceStreamDataBase dutStreamData)
        {
            // Fine-tuning needs additional file paths from the DUT stream
            this.MirrorMapWlFileName = dutStreamData.MirrorMapWlPath;
            this.CoarseTuningDeviationsFileName = dutStreamData.CoarseTuningPath;
            this.CoarseTuningMidlineFileName = dutStreamData.CoarseTuningMidlinePath;
        }

        protected override bool RunAutoTestCore()
        {
            bool autoTestResult = false;

            if (frmMain != null)
            {
                frmMain.Show();

                SetChipInformation();
                SetFineTuningInputFileNames();
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

        /// <summary>
        /// Fine-tuning saves into a Fine_tuning subfolder.
        /// </summary>
        private void SetSavePath()
        {
            General.modGlobals.PATH_TO_TEST_ANALYSIS =
                System.Windows.Forms.Application.StartupPath +
                $"\\Data\\{this.SerialNumber}\\Fine_tuning\\";
        }

        /// <summary>
        /// Sets the fine-tuning input filenames used by the DLL.
        /// </summary>
        private void SetFineTuningInputFileNames()
        {
            if (frmMain != null)
            {
                frmMain.MirrorMapWlFileName = this.MirrorMapWlFileName;
                frmMain.CoarseTuningMidlineFileName = this.CoarseTuningMidlineFileName;
                frmMain.CoarseTuningDeviationsFileName = this.CoarseTuningDeviationsFileName;
            }
        }
    }
}
