using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDatumItem_PeaPer_2 : RawDatumItemBase
    {
        public RawDatumItem_PeaPer_2()
        {

        }

        [RawDataChartAxisElement(CEAxisXY.X)]
        [RawDataCollectionItemElement("Drgree")]
        public float Drgree { get; set; }


        [RawDataChartAxisElement(CEAxisXY.Y)]
        [RawDataCollectionItemElement("Power_mW")]
        public float Power_mW { get; set; }



        //[RawDataChartAxisElement(CEAxisXY.Y)]
        //[RawDataCollectionItemElement("PDMax")]
        //public double PDMax { get; set; }


        //[RawDataChartAxisElement(CEAxisXY.Y)]
        //[RawDataCollectionItemElement("PDMin")]
        //public double PDMin { get; set; }


        //[RawDataCollectionItemElement("Drgree_RoughSweep")]
        //public double Drgree_RoughSweep { get; set; }


        //[RawDataCollectionItemElement("PDValue_RoughSweep")]
        //public double PDValue_RoughSweep { get; set; }

    }
}
