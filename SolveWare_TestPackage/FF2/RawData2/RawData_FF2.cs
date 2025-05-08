using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;

namespace SolveWare_TestPackage
{
    public class RawData_FF2 : RawDataCollectionBase<RawDatumItem_FF2>
    {
        public RawData_FF2()
        {
            this.Short_PD_Max = double.NaN;
            this.Short_PD_Max_Theta = double.NaN;
            this.Long_PD_Max = double.NaN;
            this.Long_PD_Max_Theta = double.NaN;


        }
        [RawDataBrowsableElement]
        public double Short_PD_Max { get; set; }
        [RawDataBrowsableElement]
        public double Short_PD_Max_Theta { get; set; }
        [RawDataBrowsableElement]
        public double Long_PD_Max { get; set; }
        [RawDataBrowsableElement]
        public double Long_PD_Max_Theta { get; set; }
    }
}
