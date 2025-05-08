using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_LIV_SEf : CalcRecipe
    {
        
        public CalcRecipe_LIV_SEf()
        {
            Current_mA = 1;
        }
        
        [DisplayName("以驱动电流为标定计算对应点斜率,此处为作为标定的驱动电流值")]
        [PropEditable(true)]
        public double Current_mA{ get; set; }

      
    }
}
