using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_SE_DSE : CalcRecipe
    {
        public CalcRecipe_LIV_SE_DSE()
        {
            SE1_Current = 20;
            SE2_Current = 40;
        }

        [DisplayName("SE1电流点(mA)")]
        [PropEditable(true)]
        public double SE1_Current { get; set; }

        [DisplayName("SE2电流点(mA)")]
        [PropEditable(true)]
        public double SE2_Current { get; set; }

    }
}
