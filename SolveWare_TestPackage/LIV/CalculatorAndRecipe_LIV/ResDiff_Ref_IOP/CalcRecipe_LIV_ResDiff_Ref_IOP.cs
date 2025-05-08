using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_ResDiff_Ref_IOP : CalcRecipe
    {
        public CalcRecipe_LIV_ResDiff_Ref_IOP()
        {
            //StartCurrent_mA = 0;
            //StopCurrent_mA = 10;
        }
        [DisplayName("计算SE电流点(参考的IOP值)")]
        [PropEditable(true)]
        public string Ref_IOP { get; set; }


        //[DisplayName("SE开始电流点")]
        //[PropEditable(true)]
        //public double StartCurrent_mA { get; set; }

        //[DisplayName("SE结束电流点")]
        //[PropEditable(true)]
        //public double StopCurrent_mA { get; set; }
    }
}
