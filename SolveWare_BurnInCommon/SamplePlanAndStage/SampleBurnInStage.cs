using SolveWare_BurnInCommon;
 
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace SolveWare_BurnInCommon
{
    public class SampleBurnInStage :BurnInStageBase, IBurnInStageBase //: BurnInStageBase<SampleContinuityCheckItem>, IBurnInStageBase<SampleContinuityCheckItem>
    {
        public SampleBurnInStage()
        {
            //this.ContinuityCheckList = new List<SampleContinuityCheckItem>();
        }
   
        public double BurnInCurrent_A { get; set; }
        public double BurnInVoltageUpperLimit_V { get; set; }
        public double BurnInPowerLowerLimit  { get; set; }


        public void Debug()
        {
            //this.ContinuityCheckList.Add(new SampleContinuityCheckItem());
            //var ccItem = this.ContinuityCheckList.First();
            ////这是自定义参数
            //var p1 = ccItem.CustomizeParams_1;
            //var p2 = ccItem.CustomizeParams_2;
            //var p3 = ccItem.CustomizeParams_3;
        }
    }
}