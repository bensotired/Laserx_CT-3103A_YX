using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_SEmax : CalcRecipe
    {
        public CalcRecipe_LIV_SEmax()
        {
            Ith2_StartP_mW = 0;
            Ith2_StopP_mW = 1;
        }

        [DisplayName("ith开始的功率点")]
        [PropEditable(true)]
        public double Ith2_StartP_mW { get; set; }

        [DisplayName("ith结束的功率点")]
        [PropEditable(true)]
        public double Ith2_StopP_mW { get; set; }

        [DisplayName("SE起始电流点(比ith对应电流的增量，例如ith=1，值设定2，起始电流为3）")]
        [PropEditable(true)]
        public double SE_Find_AboveIthCurrent { get; set; }

        [DisplayName("SE最大电流点")]
        [PropEditable(true)]
        public double SE_Find_MaxCurrent { get; set; }
    }
}
