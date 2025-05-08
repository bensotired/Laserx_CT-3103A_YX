using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AxMG17NanoTrakLib;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInInstruments;

namespace SolveWare_BurnInInstruments
{

    public class Thorlabs_NanoTrakChassis : InstrumentChassisBase, IInstrumentChassis
    {
        ITesterCoreInteration _core;
        public Thorlabs_NanoTrakUI UI { get; private set; }
        public AxMG17NanoTrak NanoTrakControl { get; private set; }
        public Thorlabs_NanoTrakChassis(string name, string resource, bool isOnline)
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
                    this.UI = new Thorlabs_NanoTrakUI();
                    this.UI.Text = this.Name;
                    this.NanoTrakControl = this.UI.NanoTrakControl;
                    this.NanoTrakControl.HWSerialNum = int.Parse(this.Resource);
                    if (this.IsOnline == true)
                    {
                        Thread.Sleep(2000);
                        //this.NanoTrakControl.StartCtrl();
                        this.NanoTrakControl.StartPowerCapture();
                        Thread.Sleep(2000);
                        this.NanoTrakControl.DoEvents();
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
                this._core.GUIRunUIInvokeActionSYNC(() =>
                {
                    this.UI = new Thorlabs_NanoTrakUI();
                    //Thread.Sleep(1000);
                    this.UI.Text = this.Name;
                    this.NanoTrakControl = this.UI.NanoTrakControl;
                    this.NanoTrakControl.HWSerialNum = int.Parse(this.Resource);
                    if(this.IsOnline == true)
                    {
                        Thread.Sleep(3000);
                        //Thread.Sleep(timeout);
                        this.NanoTrakControl.StartCtrl();
                        this.NanoTrakControl.Latch();
                        //Thread.Sleep(3000);
                        //Thread.Sleep(timeout);
                        //this.NanoTrakControl.DoEvents();
                    }
                });
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
                //if (this.IsOnline == true)
                {
                    //this.NanoTrakControl.StopCtrl();//.StopCtrl();
                    //Thread.Sleep(2000);
                    try
                    {
                        this.NanoTrakControl.Hide();
                      
                    }
                    catch(Exception nanoEx)
                    {

                    }
                    this.UI?.Hide();
                    //this.NanoTrakControl.Dispose();
                    this.UI?.Close();
                    //this.UI?.Dispose();
                    //this.NanoTrakControl.Dispose();//.StopCtrl();
                    //Thread.Sleep(2000);
                    //this.NanoTrakControl.DoEvents();
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Initialize error. Chassis resource = [{1}].[{2}]-[{3}]", this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg);

            }
        }
    }
}