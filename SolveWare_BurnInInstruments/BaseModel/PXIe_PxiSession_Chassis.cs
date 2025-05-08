using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.OptoelectronicComponentTest;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using System;

namespace SolveWare_BurnInInstruments
{


    public class PXIe_PxiSession_Chassis : InstrumentChassisBase, IInstrumentChassis
    {
        public PXIe_PxiSession_Chassis(string name, string resource, bool isOnline)
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
            try
            {
            }
            catch(Exception ex)
            {

            }
           
        }
    }
}