using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInInstruments;

namespace SolveWare_BurnInInstruments
{

    public partial class Photon_NanoScanChassis : InstrumentChassisBase, IInstrumentChassis
    {
        ITesterCoreInteration _core;
        //public static bool _DevicesOnline { get; set; }
        public Photon_NanoScanChassis(string name, string resource, bool isOnline)
            : base(name, resource, isOnline)
        {
            try
            {
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Constructor error, Chassis resource = [{1}].[{2}]-[{3}]", this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg, ex);
            }
        }
        public override void SetupLogger(ILogHandle logHandler, IExceptionHandle exceptionHandler)
        {
            _core = logHandler as ITesterCoreInteration;
            base.SetupLogger(logHandler, exceptionHandler);
        }
        public override void Initialize()
        {
            try
            {
                this._core.GUIRunUIInvokeAction(() =>
                {
                    if (this.IsOnline == true)
                    {
                        ////Display GUI.
                        //NsAsSetShowWindow(true);

                        //Retrieve number of connected scan heads.
                        Int16 NumberOfDevices = -1;
                        Int32 Scode = NsInteropGetNumDevices(ref NumberOfDevices);
                    }
                });

            }
            catch (Exception ex)
            {
                throw new Exception($"Initialize NanoTrakChassis exception:[{ex.Message}{ex.StackTrace}]", ex);
            }
           
        }
        public override void Initialize(int timeout)
        {
            try
            {
                //this._core.GUIRunUIInvokeAction(() =>
                //{
                //    if (this.IsOnline == true && 1 == InitNsInterop())
                //    {
                //        ////Display GUI.
                //        //NsAsSetShowWindow(true);

                //        //Retrieve number of connected scan heads.
                //        Int16 NumberOfDevices = -1;
                //        Int32 Scode = NsInteropGetNumDevices(ref NumberOfDevices);
                //    }
                //});
                //this._core.GUIRunUIInvokeAction(() =>
                //{
                //    if (1 == InitNsInterop())
                //    {
                //        //Display GUI.
                //        NsInteropSetShowWindow(true);

                //        //Retrieve number of connected scan heads.
                //        Int16 NumberOfDevices = -1;
                //        Int32 Scode = NsInteropGetNumDevices(ref NumberOfDevices);

                //        //Start DAQ so that we have meaningful profile data before reading the profile.
                //        NsInteropSetDataAcquisition(true);
                //    }
                //});
            }
            catch (Exception ex)
            {
                throw new Exception($"Initialize NanoTrakChassis exception:[{ex.Message}{ex.StackTrace}]", ex);
            }
   
        }
        public override void ClearConnection()
        {
            try
            {
                if (Photon_NanoScan.DevicesOnline)
                {
                    ShutdownNsInterop();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($" NanoTrakChassis  ClearConnection exception:[{ex.Message}{ex.StackTrace}]", ex);
            }
        }
    }
}