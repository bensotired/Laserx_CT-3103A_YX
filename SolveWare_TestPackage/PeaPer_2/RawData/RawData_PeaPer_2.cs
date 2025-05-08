using SolveWare_TestComponents.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_PeaPer_2 : RawDataCollectionBase<RawDatumItem_PeaPer_2>
    {
        public RawData_PeaPer_2()
        {
            DrivingCurrent_A = 0;

        }
        [RawDataBrowsableElement]
        public double DrivingCurrent_A { get; set; }


        //需要时在添加
        //[RawDataBrowsableElement]
        //public double PDMaxPosition_Deg { get; set; }//积分球位置



        [RawDataBrowsableElement]
        public double DegPosition_PDMax { get; set; }//偏振片位置


        [RawDataBrowsableElement]
        public double PDMax { get; set; }



        [RawDataBrowsableElement]
        public double DegPosition_PDMin { get; set; }//偏振片位置


        [RawDataBrowsableElement]
        public double PDMin { get; set; }



        [RawDataBrowsableElement]
        public double Per_DB { get; set; }


    }
}
