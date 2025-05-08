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
    /// 前后五点计算，输出I=Iop时的SE
    /// </summary>
    public class TestCalculator_LIV_SE_Ref_IOP_mWpermA : TestCalculatorBase
    {
        public TestCalculator_LIV_SE_Ref_IOP_mWpermA() : base() { }

        public CalcRecipe_LIV_SE_Ref_IOP_mWpermA CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_SE_Ref_IOP_mWpermA);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_SE_Ref_IOP_mWpermA)(testRecipe);
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}SE_Ref_IOP_mWpermA{CalcRecipe.CalcData_PostFix}");
                }
              
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string PowerTag = "Power_mW";

                List<double> CurrentArry = new List<double>();
                List<double> PowerArray = new List<double>(); 

                //var localRawData = rawData as IRawDataCollectionBase;
                //var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag);

                //if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0  | dict[CurrentTag]?.Count != dict[PowerTag]?.Count )
                //{
                //    throw new Exception($"SE_mWpermA: xArray and yArray are of unequal size!");
                //}
                var localRawData = rawData as IRawDataCollectionBase;
                CurrentArry= localRawData.GetDataListByPropName(CurrentTag);
                PowerArray = localRawData.GetDataListByPropName(PowerTag); 
                if (CurrentArry.Count <= 0 || PowerArray.Count <= 0 | CurrentArry.Count != PowerArray.Count)
                {
                    throw new Exception($"SE_mWpermA: xArray and yArray are of unequal size!");
                }

                //
                double refIopValue = 15.0;
                if (summaryDataWithoutSpec.Exists(item => item.Name == this.CalcRecipe.Ref_IOP))
                {
                    var sItem = summaryDataWithoutSpec.Find(item => item.Name == this.CalcRecipe.Ref_IOP);
                    refIopValue = Convert.ToDouble(sItem.Value);
                }

                //下面开始基于iop的se计算   你来补完    所有需要参考iop的算子都按这个做    需要注意iop算子需要在前面已经执行

                dataSummary_1.Value = GetSlopeEfficiencyForSpecifiedPower(PowerArray, CurrentArry, refIopValue);

                //double[] powerArr = new double[dict[CurrentTag].Count];
                //double[] currentArr = new double[dict[CurrentTag].Count];
                //for (int i = 0; i < dict[CurrentTag].Count; i++)
                //{
                //    powerArr[i] = dict[PowerTag][i];
                //    currentArr[i] = dict[CurrentTag][i];
                //}

                //int indexS = 0;
                //int indexE = 0;
                //int index_OK = 0;
                //bool Search_ok = false;
                //for (int i = 0; i < dict[CurrentTag].Count; i++)
                //{
                //    if (dict[CurrentTag][i] < refIopValue)
                //    {
                //        indexS = i;
                //    }
                //    if (refIopValue == dict[CurrentTag][i])
                //    {
                //        indexE = i;
                //        Search_ok = true;
                //        break;
                //    }
                //    if (dict[CurrentTag][i]> refIopValue)
                //    {
                //        indexE = i;
                //        break;
                //    }
                //}
                //if (index_OK!=0&& Search_ok)
                //{
                //    //说明找到对应的点了
                //}
                //else 
                //{
                //    if (Math.Abs(dict[CurrentTag][indexS] -refIopValue)< Math.Abs(dict[CurrentTag][indexE] - refIopValue))
                //    {
                //        index_OK = indexS;
                //        Search_ok = true;
                //    }
                //    else
                //    {
                //        index_OK = indexE;
                //        Search_ok = true;
                //    }
                //}

                //double[] _powerArr = new double[5];
                //double[] _currentArr = new double[5];
                //for (int i = index_OK - 2; i < index_OK + 2; i++)
                //{
                //    _powerArr[i] = dict[PowerTag][i];
                //    _currentArr[i] = dict[CurrentTag][i];
                //}

                //double SlopeEfficiencyStartI_mA = dict[CurrentTag][index_OK - 2];
                //double SlopeEfficiencyStop_mA = dict[CurrentTag][index_OK + 2];

                //double powerAtStart_mW = Interpolator.DoPiecewiseLinearInterpolation(SlopeEfficiencyStartI_mA, currentArr, powerArr);
                //double powerAtStop_mW = Interpolator.DoPiecewiseLinearInterpolation(SlopeEfficiencyStop_mA, currentArr, powerArr);

                //dataSummary_1.Value = (powerAtStop_mW - powerAtStart_mW) / (SlopeEfficiencyStop_mA - SlopeEfficiencyStartI_mA);

                //double[] powerArr = new double[dict[CurrentTag].Count];
                //double[] currentArr = new double[dict[CurrentTag].Count];
                //for (int i = 0; i < dict[CurrentTag].Count; i++)
                //{
                //    powerArr[i] = dict[PowerTag][i];
                //    currentArr[i] = dict[CurrentTag][i];
                //}

                //double SlopeEfficiencyStartI_mA = this.CalcRecipe.StartCurrent_mA;
                //double SlopeEfficiencyStop_mA = this.CalcRecipe.StopCurrent_mA;

                //double powerAtStart_mW = Interpolator.DoPiecewiseLinearInterpolation(SlopeEfficiencyStartI_mA, currentArr, powerArr);
                //double powerAtStop_mW = Interpolator.DoPiecewiseLinearInterpolation(SlopeEfficiencyStop_mA, currentArr, powerArr);

                //dataSummary_1.Value = (powerAtStop_mW - powerAtStart_mW) / (SlopeEfficiencyStop_mA - SlopeEfficiencyStartI_mA);

            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_SE_Ref_IOP_mWpermA_ParamError, ex);
            }
        }
        public double GetSlopeEfficiencyForSpecifiedPower(List<double> slopeEffs, List<double> powers, double power_mW)
        {
            try
            {

                int indexStart;
                int indexEnd;

                //找到比需求功率大的功率
                for (indexEnd = 0; indexEnd < slopeEffs.Count; indexEnd++)
                {
                    if (powers[indexEnd] >= power_mW)
                    {
                        break;
                    }
                }
                if (indexEnd == slopeEffs.Count) return 0;

                //找到比需求功率小的功率
                for (indexStart = indexEnd; indexStart > 0; indexStart--)
                {
                    if (powers[indexStart] < power_mW)
                    {
                        break;
                    }
                }
                if (indexStart == 0) return 0;


                //将该区间的数据进行直线拟合
                double[] powerArr = new double[indexEnd - indexStart + 4];  //功率组成的数组
                double[] slopeEffArr = new double[indexEnd - indexStart + 4]; //电流点组成的数组

                //把数据装到数组中
                for (int i = indexStart-1; i <= indexEnd+2; i++)
                {
                    //if (i - indexStart + 1< slopeEffs.Count|| i - indexStart + 1 > slopeEffs.Count)
                    //{
                    //    break;
                    //}
                    if (indexEnd + 2 > slopeEffs.Count)
                    {
                        //break;
                        return 999;
                    }
                    slopeEffArr[i - indexStart+1] = slopeEffs[i];
                    powerArr[i - indexStart+1] = powers[i];

                }

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:SE                      
                PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, slopeEffArr, 1);
               // PolyFitData polyData = PolyFitMath.PolynomialFit( slopeEffArr, powerArr, 1);

                //计算出SE      
                //double CalcSE = polyData.Coeffs[1] * power_mW + polyData.Coeffs[0];
                double CalcSE = polyData.Coeffs[1];//* power_mW + polyData.Coeffs[0];
                return CalcSE;

            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentForSpecifiedPower:", ex);
            }

        }

    }
}