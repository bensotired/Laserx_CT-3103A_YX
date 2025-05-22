using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestModule_OES")]

    #region  轴、位置、IO、仪器

    //[ConfigurableInstrument("PXISourceMeter_4143", "PD", "PD")] 
    #endregion
    public class TestModule_OES : TestModuleBase
    {

        public TestModule_OES() : base() { }

        #region 以Get获取资源
        //PXISourceMeter_4143 PD { get { return (PXISourceMeter_4143)this.ModuleResource["PD"]; } } 

        #endregion

        TestRecipe_OES TestRecipe { get; set; }
        //RawData_Curr RawData { get; set; }
        //RawDataMenu_Curr RawDataMenu { get; set; }


        #region instance variables
        LaserXFineTuningDllTest.frmMain frmMain; //The OES dll main form
        #endregion

        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_OES);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            return new RawDataBaseLite();
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_OES>(testRecipe);
            try
            {
                if (frmMain == null)
                {
                    frmMain = new LaserXFineTuningDllTest.frmMain();
                    frmMain.MirrDiagGainCurrent = 130;
                    frmMain.MirrDiagLaserPhaseCurrent = 4;
                    frmMain.MirrDiagPhase1Current = 1;
                    frmMain.MirrDiagPhase2Current = 0;
                    frmMain.MirrDiagSoa1Current = 50;
                    frmMain.MirrDiagSoa2Current = 40;
                    frmMain.MirrDiagMZM1Voltage = -2.5M; //These are decimal data types, so using the 'M' handles the type casting
                    frmMain.MirrDiagMZM2Voltage = -2.5M;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Init OES MainForm Exception:{ ex.Message }");
            }

        }

        public override void Run(CancellationToken token)
        {
            try
            {
                this.Log_Global($"关闭镭神测试平台所有仪器库连接...\r\nClose all instrument library connections of the LaserX test platform...");
                this._core.TryDisConnectAllInstruments();

                this.Log_Global($"打开OES测试窗体...\r\nOpen the OES dll main form...");
                RunOESAutoTest();

            }
            catch (Exception ex)
            {
                this._core.Log_Global($"[{ex.Message}]-[{ex.StackTrace}]");
            }
            finally
            {
                this.Log_Global($"打开镭神测试平台所有仪器库连接...\r\nReconnect all instrument library connections of the LaserX test platform...");
                this._core.TryConnectAllInstruments();
            }
        }

        #region Form control events 

        private async void RunOESAutoTest()
        {
            if (frmMain != null)
            {
                bool testSuccess = await RunAutoTest();
                this.Log_Global($"OES DLL 测试结果:{testSuccess}\r\n the OES TEST Result:{testSuccess}");
            }
        }

        #endregion

        #region "Auto test execution"
        private async Task<bool> RunAutoTest()
        {
            bool autoTestResult = false;

            if (frmMain != null)
            {
                frmMain.Show();
                SetChipInformation();
                TransferQuickWavelngthSettings();
                autoTestResult = await frmMain.ExecuteAutoTest();
                frmMain.Hide(); //IMPORTANT, use hide() instead of close since calling Close() will dispose the form.
            }
            else
            {
                this.Log_Global($"OES测试窗体未初始化...\r\n The OES test form is not initialized...");
            }
            return autoTestResult;
        }
        #endregion

        #region "Laser settings transfer functions"
        /// <summary>
        /// Sends the laser settings that were calculated from the quick wavelength form to the OES gui.
        /// The settings are used if the mirror diagonal coarse tuning test runs
        /// In this example, I am using arbitrary settings, but they should come from the QWLT test in the 
        /// laserX GUI
        /// </summary>
        private void TransferQuickWavelngthSettings()
        {
            if (frmMain != null)
            {
                frmMain.MirrDiagGainCurrent = 130;
                frmMain.MirrDiagLaserPhaseCurrent = 4;
                frmMain.MirrDiagPhase1Current = 1;
                frmMain.MirrDiagPhase2Current = 0;
                frmMain.MirrDiagSoa1Current = 50;
                frmMain.MirrDiagSoa2Current = 40;
                frmMain.MirrDiagMZM1Voltage = -2.5M; //These are decimal data types, so using the 'M' handles the type casting
                frmMain.MirrDiagMZM2Voltage = -2.5M;
            }
        }
        #endregion

        #region Chip information setting

        /// <summary>
        /// Demonstrates how to set the chip information. These are random values I picked
        /// You would use the information of the current CoC to set this info
        /// </summary>
        private void SetChipInformation()
        {
            if (frmMain != null)
            {
                frmMain.MaskID = "DO987";
                frmMain.WaferID = "TM678";
                frmMain.ChipID = "T0123";
                frmMain.OeskID = "Dll_demo";
            }
        }
        #endregion


    }
}