using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System.ComponentModel;

namespace SolveWare_TestPackage
{
    public class CalcRecipe_ACR : CalcRecipe
    {
     public CalcRecipe_ACR()
        {
            Resistance_UpperLimit = 2.0;
            Resistance_LowerLimit = 1.0;
            SamplingTemperature_C = 25;

            IsPass = false;
            ResistanceCompensation_R = 0.06;
            Temperature_C = 25;
            StandardResistance_R = 0;
        }
        [DisplayName("电阻上限值")]
        [PropEditable(true)]
        public double Resistance_UpperLimit { get; set; }
        
        [DisplayName("电阻下限值")]
        [PropEditable(true)]
        public double Resistance_LowerLimit { get; set; }

        [DisplayName("测试温度℃")]
        [PropEditable(true)]
        public double SamplingTemperature_C { get; set; }

        [DisplayName("是否计算理想电阻")]
        [PropEditable(true)]
        public bool IsPass { get; set; }

        [DisplayName("电阻补偿值/℃")]
        [PropEditable(true)]
        public double ResistanceCompensation_R { get; set; }

        [DisplayName("标准温度℃")]
        [PropEditable(true)]
        public double Temperature_C { get; set; }

        [DisplayName("标准温度下电阻值_R")]
        [PropEditable(true)]
        public double StandardResistance_R { get; set; }
    }
}
