using SolveWare_TestComponents.Attributes;
using System;
using System.Collections.Generic;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_PeaPer : RawDataCollectionBase<RawDataumItem_PeaPer>
    {
        public RawData_PeaPer() 
        {
            //this.Points = new List<RawDataumItem_PeaPer>();
            IsAlignmentDone = false;
            Per_Temperature_degC = 25;
        }
        [RawDataBrowsableElement]
        public bool IsAlignmentDone { get; set; }
        //[RawDataBrowsableElement]
        //public List<RawDataumItem_PeaPer> Points { get; set; }
        [RawDataBrowsableElement]
        public double Per_DrivingCurrent_mA { get; set; }        
        [RawDataBrowsableElement]
        public double Per_Temperature_degC { get; set; }
        [RawDataBrowsableElement]
        public double PER_Factor_K { get; set; }
        [RawDataBrowsableElement]
        public double PER_Factor_B { get; set; }

    }
}