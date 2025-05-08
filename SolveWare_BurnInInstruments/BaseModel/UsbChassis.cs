using NationalInstruments.VisaNS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public class UsbChassis : InstrumentChassisBase, IInstrumentChassis
    {

        public UsbChassis(string name, string resource, bool isOnline)
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

        public override void Initialize()
        {
            this.Initialize(5000);
        }
        public override void Initialize(int timeout)
        {
        }
    }
}