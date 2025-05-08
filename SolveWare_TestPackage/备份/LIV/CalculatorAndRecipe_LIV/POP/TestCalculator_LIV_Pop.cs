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
    /// Pop算子
    /// 找出目标电流点下对应的器件输出功率
    /// </summary>
    public class TestCalculator_LIV_Pop : TestCalculatorBase
    {
        public TestCalculator_LIV_Pop() : base() { }

        public CalcRecipe_LIV_Pop CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Pop);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            //CalcRecipe = this.ConvertObjectTo<CalcRecipe_LIV_Pop>(testRecipe);
            CalcRecipe = (CalcRecipe_LIV_Pop)(testRecipe);
        }

        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                //根据1100A 
                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();


                if (CalcRecipe.IsForceRename == true)
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                }
                else
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Pop{CalcRecipe.CalcData_PostFix}");
                }

              
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string PowerTag = "Power_mW";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag);

                if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag]?.Count)
                {
                    throw new Exception($"Pop: xArray and yArray are of unequal size!");
                }
                
                int indexS;
                int indexE;

                //找到比需求功率大的功率
                for (indexE = 0; indexE < dict[PowerTag]?.Count; indexE++)
                {
                    if (dict[PowerTag][indexE] >= CalcRecipe.Power_mW)
                    {
                        break;
                    }
                }
                if (indexE == dict[PowerTag]?.Count)
                {
                    dataSummary_1.Value = 0;
                    return;
                }
                  

                //找到比需求功率小的功率
                for (indexS = indexE; indexS > 0; indexS--)
                {
                    if (dict[PowerTag][indexS] < CalcRecipe.Power_mW)
                    {
                        break;
                    }
                }
                if (indexS == 0)
                {
                    dataSummary_1.Value = 0;
                    return;
                }
                
                //将该区间的数据进行直线拟合
                double[] powerArr = new double[indexE - indexS + 1];  //功率组成的数组
                double[] currentArr = new double[indexE - indexS + 1]; //电流点组成的数组

                //把数据装到数组中
                for (int i = indexS; i <= indexE; i++)
                {
                    currentArr[i - indexS] = dict[CurrentTag][i];
                    powerArr[i - indexS] = dict[PowerTag][i];
                }

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:电流                       
                PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, currentArr, 1);

                //计算出电流
                double CalcCurrent = polyData.Coeffs[1] * CalcRecipe.Power_mW + polyData.Coeffs[0];
                dataSummary_1.Value = CalcCurrent;
          

            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Pop_ParamError, ex);
            }
        }
    }
}