using SolveWare_BurnInCommon;
using System;
using System.ComponentModel;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class TestRecipeBase : CURDItem, ITestRecipe
    {
        [DisplayName("输出结果前缀格式如:[PreFix_]若用在[SummaryX]汇总结果参数上,则汇总结果参数名为[PreFix_SummaryX]")]

        [PropEditable(true)]
        public string SummaryData_PreFix { get; set; } = string.Empty;
        [DisplayName("输出结果后缀式如:[_PostFix]若用在[SummaryX]汇总结果参数上,则汇总结果参数名为[SummaryX_PostFix]")]
        [PropEditable(true)]
        public string SummaryData_PostFix { get; set; } = string.Empty;
    }
}
