using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_AlternativeQWLT : RawDataCollectionBase<RawDatumItem_AlternativeQWLT>
    {
     

        public RawData_AlternativeQWLT()
        {
            
        }
        public string SerachMode { get; set; }
        public string resut { get; set; }
        public string Section { get;   set; }
        public string Driver_mA { get;   set; }
        public double PH_Max_1 { get;   set; }
        public double PH_Max_2 { get;   set; }
    }
}