using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Kink_OutRange : CalcRecipe
    {
        public CalcRecipe_LIV_Kink_OutRange()
        {
            _KinkStartI_mA = 0;
            _KinkStopI_mA = 100;
            KinkFitOrder = 2;
            KinkOutRangeValue = 10;
        }

        [DisplayName("KINK开始的电流值")]
        [PropEditable(true)]
        public double _KinkStartI_mA { get; set; }

        [DisplayName("KINK结束的电流值")]
        [PropEditable(true)]
        public double _KinkStopI_mA { get; set; }

        [DisplayName("拟合曲线的阶数")]
        [PropEditable(true)]
        public int KinkFitOrder { get; set; }

        [DisplayName("Kink超出范围值(值为正数，不是百分比)")]
        [PropEditable(true)]
        public double KinkOutRangeValue { get; set; }
    }
}
