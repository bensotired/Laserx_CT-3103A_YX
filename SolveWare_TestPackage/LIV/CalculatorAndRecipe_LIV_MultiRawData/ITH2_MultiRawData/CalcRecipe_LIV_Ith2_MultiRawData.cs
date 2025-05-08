using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Ith2_MultiRawData : CalcRecipe
    {
        public CalcRecipe_LIV_Ith2_MultiRawData()
        {
            Ith2_StartP_mW = 0;
            Ith2_StopP_mW = 1;
        }

        [DisplayName("开始的功率点")]
        [PropEditable(true)]
        public double Ith2_StartP_mW { get; set; }

        [DisplayName("结束的功率点")]
        [PropEditable(true)]
        public double Ith2_StopP_mW { get; set; }
    }
}
