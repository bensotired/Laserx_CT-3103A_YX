using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class CalcRecipe_Spect_SMSR : CalcRecipe
    {
        //ZH 直接问的，不用这个
        //public CalcRecipe_Spect_SMSR()
        //{
        //    SearchMin_nm = 2;
        //    SearchMax_nm = 5;
        //}

        //[DisplayName("次峰距离主峰多远开始搜索 取正值 值小")]
        //[PropEditable(true)]
        //public int SearchMin_nm { get; set; }

        //[DisplayName("次峰距离主峰多远结束搜索 取正值 值大")]
        //[PropEditable(true)]
        //public int SearchMax_nm { get; set; }
    }
}
