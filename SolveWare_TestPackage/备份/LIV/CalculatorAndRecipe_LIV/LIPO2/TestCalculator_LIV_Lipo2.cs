using System;
using System.Collections.Generic;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System.Linq;

namespace SolveWare_TestPackage
{
    [Serializable]
    /// <summary>
    /// 
    /// 结束电流
    /// </summary>
    public class TestCalculator_LIV_Lipo2 : TestCalculatorBase
    {
        public TestCalculator_LIV_Lipo2() : base() { }

        public CalcRecipe_LIV_Lipo2 CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Lipo2);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Lipo2)testRecipe;
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Lipo2{CalcRecipe.CalcData_PostFix}");
                }


                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string PowerTag = "Power_mW";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag);

                if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag]?.Count)
                {
                    throw new Exception($"Lipo2: xArray and yArray are of unequal size!");
                }
                double _PowerStart1 = this.CalcRecipe.PowerStart1_mW;
                double _PowerEnd1 = this.CalcRecipe.PowerEnd1_mW;
                double _PowerStart2 = this.CalcRecipe.PowerStart2_mW;
                double _PowerEnd2 = this.CalcRecipe.PowerEnd2_mW;
                
                double _CurrentStart1 = GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag], _PowerStart1);
                double _CurrentEnd1   = GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag], _PowerEnd1);
                double _CurrentStart2 = GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag], _PowerStart2);
                double _CurrentEnd2   = GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag], _PowerEnd2);

                //SE2/SE1  结果需要百分比
                dataSummary_1.Value =Math.Round(
                                        (100*
                                        (_PowerEnd2- _PowerStart2)/(_CurrentEnd2- _CurrentStart2))/
                                        ((_PowerEnd1 - _PowerStart1) / (_CurrentEnd1 - _CurrentStart1))
                                       ,3);

            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Lipo2_ParamError, ex);
            }
        }

        public double GetCurrentForSpecifiedPower(List<double> currents, List<double> powers, double power_mW)
        {
            try
            {

                int indexS;
                int indexE;

                //找到比需求功率大的功率
                for (indexE = 0; indexE < currents.Count; indexE++)
                {
                    if (powers[indexE] >= power_mW)
                    {
                        break;
                    }
                }
                if (indexE == currents.Count) return 0;

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
                double[] currentArr = new double[indexE - indexS + 1]; //电流点组成的数组

                //把数据装到数组中
                for (int i = indexS; i <= indexE; i++)
                {
                    currentArr[i - indexS] = currents[i];
                    powerArr[i - indexS] = powers[i];

                }

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:电流                       
                PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, currentArr, 1);

                //计算出电流
                double CalcCurrent = polyData.Coeffs[1] * power_mW + polyData.Coeffs[0];
                return CalcCurrent;

            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentForSpecifiedPower:", ex);
            }

        }


    }
}