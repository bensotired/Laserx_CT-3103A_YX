using SolveWare_BurnInCommon;
using System.ComponentModel;
using System.Xml.Serialization;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class CalcRecipe : CURDItem, ICalcRecipe
    {
        [DisplayName("计算结果参数前缀格式如:[PreFix_]若用在计算结果参数为[X]算子上,则计算结果参数名为[PreFix_X]")]
        [PropEditable(false)]
        public string CalcData_PreFix { get; set; } = string.Empty;
        [DisplayName("计算结果参数后缀式如:[_PostFix]若用在计算结果参数为[X]算子上,则计算结果参数名为[X_PostFix]")]
        [PropEditable(false)]
        public string CalcData_PostFix { get; set; } = string.Empty;

        [DisplayName("计算结果参数强命名,若该参数不为空值，若用在计算结果参数为[X]算子上,则替代[X]作为结果参数输出")]
        [PropEditable(false)]
        public string CalcData_Rename { get; set; } = string.Empty;
        [XmlIgnore]
        public bool IsForceRename
        {
            get
            {
                if (string.IsNullOrEmpty(CalcData_Rename) == true)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

    }
}