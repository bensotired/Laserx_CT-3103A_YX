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
    /// <summary>
    /// Common base for OES-related test modules.
    /// Handles recipe type, raw data, stream-data mapping, form lifecycle,
    /// resource access (switches), and the common Run() orchestration.
    /// </summary>
    public abstract class TestModule_OESBase : TestModuleBase
    {
        protected TestModule_OESBase() : base() { }

        #region Resource access

        protected OpticalSwitch OSwitch
            => (OpticalSwitch)this.ModuleResource["OSwitch"];

        protected IOBase SwitchPD
            => (IOBase)this.ModuleResource["PD_3"];

        #endregion

        #region State shared by OES modules

        protected TestRecipe_OES TestRecipe { get; private set; }

        protected string MaskName { get; private set; }
        protected string WaferID { get; private set; }
        protected string SerialNumber { get; private set; }
        protected string OeskID { get; private set; }

        /// <summary>
        /// Shared instance of the OES DLL main form.
        /// </summary>
        protected LaserXFineTuningDllTest.frmMain frmMain;

        #endregion

        #region TestModuleBase overrides

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
            // Common fields
            this.MaskName = dutStreamData.MaskName;
            this.WaferID = dutStreamData.WaferName;
            this.SerialNumber = dutStreamData.SerialNumber; // chip name
            this.OeskID = dutStreamData.OeskID;

            // Allow children to read extra fields (mirror map, etc.)
            ReadAdditionalStreamData(dutStreamData);
        }

        /// <summary>
        /// Hook for derived classes to grab additional data from the DUT stream
        /// (e.g. mirror-map and coarse-tuning file paths).
        /// </summary>
        protected virtual void ReadAdditionalStreamData(IDeviceStreamDataBase dutStreamData)
        {
            // default: nothing extra
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
                throw new Exception($"Init OES MainForm Exception:{ex.Message}");
            }
        }

        public override void RunPostAction(CancellationToken token)
        {
            try
            {
                // Intentionally left as no-op for now.
                // If you later decide to close/dispose, do it here
                // so all OES modules share the same behavior.
                //if (frmMain != null)
                //{
                //    frmMain.Close();
                //    frmMain.Dispose();
                //}
            }
            catch (Exception ex)
            {
                throw new Exception($"Dispose OES MainForm Exception:{ex.Message}");
            }
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_OES>(testRecipe);
        }

        /// <summary>
        /// Common run pipeline:
        /// - Route optical switch / PD
        /// - Disconnect all instruments
        /// - Execute specific OES test (implemented by child)
        /// - Reconnect all instruments
        /// </summary>
        public override void Run(CancellationToken token)
        {
            try
            {
                // 1. Route to SMU tap PD
                Circuit_Controller.TapPD_ConnectTo(SwitchPD, TapPD_Circuit.SMU);
                OptialPath_Controller.SwitchTo(OSwitch, OptialPath.TapPD);

                this.Log_Global("关闭镭神测试平台所有仪器库连接...\r\nClose all instrument library connections of the LaserX test platform...");
                // 2. Disconnect from instruments (currently NI SMU)
                this._core.TryDisConnectAllInstruments();

                this.Log_Global("打开OES测试窗体...\r\nOpen the OES dll main form...");

                // 3. Execute the specific OES test defined by the child class
                RunOESAutoTest();
            }
            catch (Exception ex)
            {
                this._core.Log_Global($"[{ex.Message}]-[{ex.StackTrace}]");
            }
            finally
            {
                this.Log_Global("打开镭神测试平台所有仪器库连接...\r\nReconnect all instrument library connections of the LaserX test platform...");
                this._core.TryConnectAllInstruments();
            }
        }

        #endregion

        #region Common helpers

        private bool RunOESAutoTest()
        {
            bool testSuccess = false;

            if (frmMain != null)
            {
                testSuccess = RunAutoTestCore();
                this.Log_Global($"OES DLL 测试结果:{testSuccess}\r\n the OES TEST Result:{testSuccess}");
            }
            else
            {
                this.Log_Global("OES测试窗体未初始化...\r\n The OES test form is not initialized...");
            }

            return testSuccess;
        }

        /// <summary>
        /// Derived classes implement their specific test logic:
        /// - show form
        /// - set chip info
        /// - set save path / input files
        /// - call the appropriate Execute* method on frmMain
        /// - hide form
        /// </summary>
        protected abstract bool RunAutoTestCore();

        /// <summary>
        /// Sets the chip information on the OES form from the current DUT info.
        /// Shared by all OES modules.
        /// </summary>
        protected void SetChipInformation()
        {
            if (frmMain != null)
            {
                frmMain.MaskID = this.MaskName;
                frmMain.WaferID = this.WaferID;
                frmMain.ChipID = this.SerialNumber; // used in file names
                frmMain.OeskID = this.OeskID;
            }
        }

        #endregion
    }
}
