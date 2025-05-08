using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_Spect_PowerMax : CalcRecipe
    {
        public CalcRecipe_Spect_PowerMax() 
        {
            NanoTrakAverageCurrent_mA = 1;
        }
        [DisplayName("耦合的平均电流")]
        [PropEditable(true)]
        public double NanoTrakAverageCurrent_mA { get; set; }
    }
}
