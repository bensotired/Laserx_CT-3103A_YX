using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class BurnInStageBase : IBurnInStageBase
    {
        public BurnInStageBase()
        {

        }
      
        [PropEditable(false)]
        public int ID { get; set; }
       
        [PropEditable(true)]
        public virtual double SoakingTime_Mins { get; set; }
        [PropEditable(true)]
    
        public virtual double Duration_Hours { get; set; }
        [PropEditable(true)]
      
        public virtual double SamplingInterval_Mins { get; set; }
        public virtual object CreateInstance()
        {
            return new BurnInStageBase();
        }
    }
}