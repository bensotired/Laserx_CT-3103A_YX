using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_Lipo1 : CalcRecipe
    {
        public CalcRecipe_LIV_Lipo1()
        {
            PowerStart = 1;
            PowerEnd = 2;
        }

        [DisplayName("第一个功率点")]
        [PropEditable(true)]
        public double PowerStart { get; set; }

        [DisplayName("第二个功率点")]
        [PropEditable(true)]
        public double PowerEnd { get; set; }
    }
}
