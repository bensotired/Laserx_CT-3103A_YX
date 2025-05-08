using SolveWare_TestComponents.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_PeaPer : RawDataCollectionBase<RawDatumItem_PeaPer>
    {
        public RawData_PeaPer()
        {
            DrivingCurrent_mA = 0;

        }
        [RawDataBrowsableElement]
        public double DrivingCurrent_mA { get; set; }

        [RawDataBrowsableElement]
        public double DegPosition_PDMax { get; set; }//偏振片位置


        [RawDataBrowsableElement]
        public double PDMax { get; set; }



        [RawDataBrowsableElement]
        public double DegPosition_PDMin { get; set; }//偏振片位置


        [RawDataBrowsableElement]
        public double PDMin { get; set; }



        //[RawDataBrowsableElement]
        //public double Per_DB { get; set; }


    }
}
