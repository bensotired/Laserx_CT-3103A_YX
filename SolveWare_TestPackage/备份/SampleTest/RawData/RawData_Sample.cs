using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_Sample : RawDataCollectionBase<RawDatumItem_Sample>
    {
        public RawData_Sample()
        {
            Temperature_degC = 25.0;

        }
        [RawDataBrowsableElement]
        public double Temperature_degC { get; set; }
    }
    [Serializable]
    public class RawData_SampleLite : RawDataBaseLite
    {
        public RawData_SampleLite()
        {
            Temperature_degC = 25.0;
        }
        [RawDataBrowsableElement]
        public double Temperature_degC { get; set; }
    }
}