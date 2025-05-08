using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_NF_Gain : CalcRecipe 
    {
        public CalcRecipe_NF_Gain() 
        {
            Specified_Wavelength_nm = 1510;
            USE_Specified_Wavelength_nm = false;
        }
        [DisplayName("是否使用指定下的波长点（true=Specified_Wavelength_nm，false=TraceA_CenterWavelength_nm）")]
        [PropEditable(true)]
        public bool USE_Specified_Wavelength_nm { get; set; }

        [DisplayName("计算波长下的NF  输入值:波长")]
        [PropEditable(true)]
        public double Specified_Wavelength_nm { get; set; }
    }
}
