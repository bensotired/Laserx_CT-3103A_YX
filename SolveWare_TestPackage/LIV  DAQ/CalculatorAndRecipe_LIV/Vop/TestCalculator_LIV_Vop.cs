using System;
using System.Collections.Generic;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;

namespace SolveWare_TestPackage
{
    [Serializable]
    /// <summary>
    /// Vop算子
    /// 找出目标功率点下对应的驱动电流值
    /// </summary>
    public class TestCalculator_LIV_Vop : TestCalculatorBase
    {
        public TestCalculator_LIV_Vop() : base() { }

        public CalcRecipe_LIV_Vop CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Vop);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Vop)testRecipe;
        }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();

                if (CalcRecipe.IsForceRename == true)
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                }
                else
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Vop{CalcRecipe.CalcData_PostFix}");
                }
 
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string VoltageTag = "Voltage_V";
                const string PowerTag = "Power_mW";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(VoltageTag, PowerTag);

                if (dict[VoltageTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[VoltageTag]?.Count != dict[PowerTag]?.Count)
                {
                    throw new Exception($"Vop: xArray and yArray are of unequal size!");
                }

                double power_mW = this.CalcRecipe.Power_mW;
                double resultVoltage = GetVoltageForSpecifiedPower(dict[VoltageTag], dict[PowerTag], power_mW);
                dataSummary_1.Value = resultVoltage;

            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Vop_ParamError, ex);
            }
        }

        public double GetVoltageForSpecifiedPower(List<double> voltages, List<double> powers, double power_mW)
        {
            try
            {

                int indexS;
                int indexE;

                //找到比需求功率大的功率
                for (indexE = 0; indexE < voltages.Count; indexE++)
                {
                    if (powers[indexE] >= power_mW)
                    {
                        break;
                    }
                }
                if (indexE == voltages.Count) return 0;

                //找到比需求功率小的功率
                for (indexS = indexE; indexS > 0; indexS--)
                {
                    if (powers[indexS] < power_mW)
                    {
                        break;
                    }
                }
                if (indexS == 0) return 0;


                //将该区间的数据进行直线拟合
                double[] powerArr = new double[indexE - indexS + 1];  //功率组成的数组
                double[] voltageArr = new double[indexE - indexS + 1]; //电流点组成的数组

                //把数据装到数组中
                for (int i = indexS; i <= indexE; i++)
                {
                    voltageArr[i - indexS] = voltages[i];
                    powerArr[i - indexS] = powers[i];

                }

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:电流                       
                PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, voltageArr, 1);

                //计算出电流
                double CalcVoltage = polyData.Coeffs[1] * power_mW + polyData.Coeffs[0];
                return CalcVoltage;

            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentForSpecifiedPower:", ex);
            }

        }
    }
}