using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_FarField : RawDataCollectionBase<RawDatumItem_FarField>
    {
        public RawData_FarField()
        {
        }

        [RawDataBrowsableElement]
        public double Current_mA { get; set; }

        [RawDataBrowsableElement]
        public string Axis { get; set; }

        [RawDataBrowsableElement]
        public double DarkCurrent { get; set; }
    }
}