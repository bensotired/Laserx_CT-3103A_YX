using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawDataMenu_AlternativeQWLT : RawDataMenuCollection<RawData_AlternativeQWLT>
    {
        public RawDataMenu_AlternativeQWLT()
        {
        }
 
        [RawDataBrowsableElement]
        public double MIRROR1_val { get; set; }
        [RawDataBrowsableElement]
        public double MIRROR2_val { get; set; }
        [RawDataBrowsableElement]
        public double LP_val { get; set; }
        [RawDataBrowsableElement]
        public double PH1_Max_val { get; set; }
        [RawDataBrowsableElement]
        public double PH2_Max_val { get; set; }

        [RawDataBrowsableElement]
        public double Wavelength { get; set; }
    }
}