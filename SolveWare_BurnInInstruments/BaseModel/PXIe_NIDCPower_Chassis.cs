using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.OptoelectronicComponentTest;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using System;

namespace SolveWare_BurnInInstruments
{


    public class PXIe_NIDCPower_Chassis : InstrumentChassisBase, IInstrumentChassis
    {
     
        //= new ResourceChannelSettings(slot_resourceName, channel_number_of_slot); //PXI1Slot2@0
        //Chassis = SmuUtility.CreateSession(pxi_resource);
        public PXIe_NIDCPower_Chassis(string name, string resource, bool isOnline)
            : base(name, resource, isOnline)
        {
            try
            {
                //var temp = resource.Split('@');
                //slot_resourceName = temp[0];
                //channel_number_of_slot = temp[1];


                //slot_resourceName = resource;


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
                //pxi_resource = new ResourceChannelSettings(slot_resourceName, channel_number_of_slot); //PXI1Slot2@0
                //_myVisa = SmuUtility.CreateSession(pxi_resource);
            }
            catch(Exception ex)
            {

            }
           
        }
    }
}