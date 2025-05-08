using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_Spect_FWHM : CalcRecipe
    {
        public CalcRecipe_Spect_FWHM()
        {
            level = 0.5;
        }

        [DisplayName("最大功率的百分比  参数范围0-1")]
        [PropEditable(true)]
        public double level { get; set; }
    }
}
