using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Threading;
using System.Threading.Tasks;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_BurnInCommon;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestModule_OES")]

    #region  轴、位置、IO、仪器


    [StaticResource(ResourceItemType.IO, "PD_3", "切换PD")]
    [ConfigurableInstrument("OpticalSwitch", "OSwitch", "用于切换光路(1*4切换器)")]
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

        string MaskName { get; set; }
        string WaferID { set; get; }
        string SerialNumber { get; set; }
        string OeskID { set; get; }

        string MirrorMapWlFileName { get; set; }
        string CoarseTuningMidlineFileName { get; set; }
        string CoarseTuningDeviationsFileName { get; set; }

        private OpticalSwitch OSwitch { get { return (OpticalSwitch)this.ModuleResource["OSwitch"]; } }
        private IOBase SwitchPD { get { return (IOBase)this.ModuleResource["PD_3"]; } }
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

        public override void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData)
        {
            this.MaskName = dutStreamData.MaskName;
            this.WaferID = dutStreamData.WaferName;
            this.SerialNumber = dutStreamData.SerialNumber; //This is what needs to be the name of the chip.
            this.OeskID = dutStreamData.OeskID;

            this.MirrorMapWlFileName = dutStreamData.MirrorMapWlPath;
            this.CoarseTuningDeviationsFileName = dutStreamData.CoarseTuningPath;
            this.CoarseTuningMidlineFileName = dutStreamData.CoarseTuningMidlinePath;
        }
        public override void RunRreAction(CancellationToken token)
        {
            try
            {
                if (frmMain == null)
                {
                    frmMain = new LaserXFineTuningDllTest.frmMain();
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Init OES MainForm Exception:{ ex.Message }");
            }
        }
        public override void RunPostAction(CancellationToken token)
        {
            try
            {
                //if (frmMain != null)
                //{
                //    frmMain.Close();
                //    frmMain.Dispose();
                //}
            }
            catch (Exception ex)
            {
                throw new Exception($"Dispose OES MainForm Exception:{ ex.Message }");
            }
        }
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_OES>(testRecipe);
        }


   
        //This should run after coarse tuning!!!
        //After finishing, integrate into auto test after coarse tuning
        public override void Run(CancellationToken token)
        {
            try
            {
                //1. Re-align the fiber, using stable laser settings (maybe from QWLT?). Is there a function that does this?

               
                Circuit_Controller.TapPD_ConnectTo(SwitchPD, TapPD_Circuit.SMU);
                OptialPath_Controller.SwitchTo(OSwitch, OptialPath.TapPD);
      

                this.Log_Global($"关闭镭神测试平台所有仪器库连接...\r\nClose all instrument library connections of the LaserX test platform...");
                //2. Disconnect from the Ni SMU
                this._core.TryDisConnectAllInstruments(); //right now, this only needs to disconnect from the NI SMU

                this.Log_Global($"打开OES测试窗体...\r\nOpen the OES dll main form...");

                //This function runs on a separate thread, so we need to wait for RunOESAutoTest() to complete
                RunOESAutoTest();

            }
            catch (Exception ex)
            {
                this._core.Log_Global($"[{ex.Message}]-[{ex.StackTrace}]");
            }
            finally
            {
                this.Log_Global($"打开镭神测试平台所有仪器库连接...\r\nReconnect all instrument library connections of the LaserX test platform...");
                this._core.TryConnectAllInstruments(); //Re-connect to all instruments disconncted from
            }
        }

        #region Form control events 

        private bool RunOESAutoTest()
        {
            bool testSuccess = false;
            if (frmMain != null)
            {
                testSuccess = RunAutoTest();
                this.Log_Global($"OES DLL 测试结果:{testSuccess}\r\n the OES TEST Result:{testSuccess}");
            }
            return testSuccess;
        }

        #endregion

        #region "Auto test execution"
        private bool RunAutoTest()
        {
            bool autoTestResult = false;

            if (frmMain != null)
            {
                frmMain.Show();
                SetChipInformation();
                SetFineTuningInputFileNames();
                autoTestResult = frmMain.ExecuteFineTuningTest();
                frmMain.Hide(); //IMPORTANT, use hide() instead of close since calling Close() will dispose the form.
            }
            else
            {
                this.Log_Global($"OES测试窗体未初始化...\r\n The OES test form is not initialized...");
            }
            return autoTestResult;
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
                frmMain.MaskID = this.MaskName; //SET FROM CURRENT COC INFO!!
                frmMain.WaferID = this.WaferID; //SET FROM CURRENT COC INFO!!
                frmMain.ChipID = this.SerialNumber; //This is the thing that needs the chip name, whatever we set this to is automatically included in the file name for fine tuning.
                frmMain.OeskID = this.OeskID; //SET FROM CURRENT COC INFO!!

                //Need to make sure this is set from the current CoC.
            }
        }
        #endregion


        #region Mirror map and coarse tuning fileName input        
        /// <summary>
        /// Sets the filename which the fine tuning routine uses as its input
        /// The file names for the mirror map wavelength, the coarse tuning midlines,
        /// and coarse tuning deviations are set in this method
        /// </summary>
        private void SetFineTuningInputFileNames()
        {
            if (frmMain != null)
            {
                frmMain.MirrorMapWlFileName = this.MirrorMapWlFileName;//Set this fileName to the mirror tuning file for this current CoC
                frmMain.CoarseTuningMidlineFileName = this.CoarseTuningMidlineFileName; //Set this to the corresponding midline file for this current CoC
                frmMain.CoarseTuningDeviationsFileName = this.CoarseTuningDeviationsFileName; //Set this to the corresponding coarse tuning deviations file for this current CoC

                //These should be automatically set. The user should not have to manually set them.
                //They should be chosen based on the mask, wafer, and Chip ID of the CoC being tested.

                //When selecting the files, if there are multiple files matching the name criteria, then
                //select the most recently saved file. This goes for all 3 files here. 

                //MirrorMapWlFilename needs to be the complete directory path of the file saved from the
                //current device.
            }



        }
        #endregion


    }
}