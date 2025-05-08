using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents;

namespace SolveWare_TestPackage
{
    [Serializable]
    /// <summary>
    /// Kink_Percentage算子 CT3102
    /// 找出目标电流点下对应的器件输出功率
    /// </summary>
    public class TestCalculator_LIV_Kink_Percentage : TestCalculatorBase
    {
        public TestCalculator_LIV_Kink_Percentage() : base() { }

        public CalcRecipe_LIV_Kink_Percentage CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Kink_Percentage);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Kink_Percentage)(testRecipe);
        }

        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                //var abc =summaryDataWithoutSpec.Find(x => x.Name == "Ith3").Value;//算子里面找ith dym
                if (CalcRecipe.IsForceRename == true)
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                }
                else
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Kink_Percentage{CalcRecipe.CalcData_PostFix}");
                }

             
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string PowerTag = "Power_mW";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag);

                if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag]?.Count)
                {
                    throw new Exception($"Kink_Percentage: xArray and yArray are of unequal size!");
                }



                // Kink Percentage
                int Ithindex = 0;
                int indexS = 0; //百分比方式的开始电流
                int indexE = 0; //百分比方式的结束电流
                double pointStart = (double)CalcRecipe._KinkStartI_mA;   //开始电流
                double pointStop = (double)CalcRecipe._KinkStopI_mA;   //结束电流

                //结束点必须大于开始点
                if (pointStop < pointStart)
                {
                    dataSummary_1.Value = 0;
                    return;
                }

                //找到比开始电流点大的电流点
                for (indexS = 0; indexS < dict[CurrentTag].Count; indexS++)
                {
                    if (dict[CurrentTag][indexS]>= pointStart)
                    {
                        break;
                    }
                }

                //找到比结束电流小一点的电流点
                for (indexE = indexS + 1; indexE < dict[CurrentTag].Count; indexE++)
                {
                    if (dict[CurrentTag][indexE] >= pointStop)
                    {
                        indexE--; //倒回去一个
                        break;
                    }
                }

                if (indexE >= dict[CurrentTag].Count)
                {
                    indexE = dict[CurrentTag].Count - 1;
                }

                double[] powerArr = new double[indexE - indexS + 1];  //功率组成的数组
                double[] currentArr = new double[indexE - indexS + 1]; //电流点组成的数组

                //把数据装到数组中
                for (int i = indexS; i <= indexE; i++)
                {
                    currentArr[i - indexS] = dict[CurrentTag][i];
                    powerArr[i - indexS] = dict[PowerTag][i];
                }

                int stepSize = (int)Math.Ceiling(1 / (currentArr[1] - currentArr[0])); //用1mA步进以消除功率采样分辨率低的问题
                if (stepSize == 0) stepSize = 1;
                double[] SE = ArrayMath.CalculateFirstDerivateWithFixedStepSize(currentArr, powerArr, stepSize);

                PolyFitData polyData = PolyFitMath.PolynomialFit(currentArr, SE, CalcRecipe.KinkFitOrder);

                int MaxKink_Index = 0;

                double SE_Diff_Max = double.MinValue;
                double KinkPercentage = double.MinValue;
                double KinkPower = double.MinValue;
                double KinkCurrent = double.MinValue;

                double KinkCurrent_mA = currentArr.Max();//this.LIV.GetLaserCurrents_mA().Max();
                for (int i = 0; i < SE.Length - 1; i++)
                {
                    //计算最大误差百分比 ( 实际斜率与
                    double fittedSE = polyData.FittedYArray[i];
                    if (Math.Abs(SE[i] - fittedSE) > SE_Diff_Max)
                    {
                        //找到最大偏差的位置
                        SE_Diff_Max = Math.Abs(SE[i] - fittedSE);
                        MaxKink_Index = i;
                        KinkPercentage = (SE[i] - fittedSE) / fittedSE * 100; //百分比
                        KinkPower = powerArr[i]; //kink时候的功率
                        KinkCurrent = currentArr[i]; //kink时候的电流
                    }
                }
                //最后赋值
                dataSummary_1.Value = KinkPercentage;
            

            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Kink_Percentage_ParamError, ex);
            }
        }
       
    }
}