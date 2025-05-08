using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_SOA_Ith3_MultiRawData : CalcRecipe
    {
        public CalcRecipe_LIV_SOA_Ith3_MultiRawData()
        {
            ThresholdCurrentLowerLimit_mA = 5;
            ThresholdCurrentUpperLimit_mA = 20;
        }
        [DisplayName("Ith3的下限")]
        [PropEditable(true)]
        public double ThresholdCurrentLowerLimit_mA { get; set; }
        [DisplayName("Ith3的上限")]
        [PropEditable(true)]
        public double ThresholdCurrentUpperLimit_mA { get; set; }

    }
}