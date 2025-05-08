using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_MenuChartDemo : RawDataCollectionBase<RawDatumItem_MenuChartDemo>
    {
        public RawData_MenuChartDemo()
        {

        }

        [RawDataBrowsableElement()]
        [RawDataDrawMulitipeLinesElement]

        public double LDDrivingCurrent_mA { get; set; }

    }
}