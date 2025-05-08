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
    /// Iop 对应PCE
    /// </summary>
    public class TestCalculator_LIV_PCE_Ref_IOP : TestCalculatorBase
    {
        public TestCalculator_LIV_PCE_Ref_IOP() : base() { } 

        public CalcRecipe_LIV_PCE_Ref_IOP CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_PCE_Ref_IOP);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_PCE_Ref_IOP)(testRecipe);
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}PCE_Ref_IOP{CalcRecipe.CalcData_PostFix}");
                }
              
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string PCE = "PCE";

                List<double> CurrentArry = new List<double>();
                List<double> PCEArray = new List<double>(); 

                var localRawData = rawData as IRawDataCollectionBase;
                //var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PCE);

                //if (dict[CurrentTag]?.Count <= 0 || dict[PCE]?.Count <= 0  | dict[CurrentTag]?.Count != dict[PCE]?.Count )
                //{
                //    throw new Exception($"SE_mWpermA: xArray and yArray are of unequal size!");
                //}

                CurrentArry = localRawData.GetDataListByPropName(CurrentTag);
                PCEArray = localRawData.GetDataListByPropName(PCE);
                if (CurrentArry.Count <= 0 || PCEArray.Count <= 0 | CurrentArry.Count != PCEArray.Count)
                {
                    throw new Exception($"SE_mWpermA: xArray and yArray are of unequal size!");
                }

                double refIopValue = 15.0;
                if (summaryDataWithoutSpec.Exists(item => item.Name == this.CalcRecipe.Ref_IOP))
                {
                    var sItem = summaryDataWithoutSpec.Find(item => item.Name == this.CalcRecipe.Ref_IOP);
                    refIopValue = Convert.ToDouble(sItem.Value);
                }

                dataSummary_1.Value = GetSlopeEfficiencyForSpecifiedPower( PCEArray, CurrentArry, refIopValue);


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
                //    if (dict[CurrentTag][i] > refIopValue)
                //    {
                //        indexE = i;
                //        break;
                //    }
                //}
                //if (index_OK != 0 && Search_ok)
                //{
                //    //说明找到对应的点了
                //}
                //else
                //{
                //    if (Math.Abs(dict[CurrentTag][indexS] - refIopValue) < Math.Abs(dict[CurrentTag][indexE] - refIopValue))
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

                //dataSummary_1.Value = dict[CurrentTag][index_OK];               

            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_PCE_Ref_IOP_ParamError, ex);
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
                double[] powerArr = new double[indexEnd - indexStart + 1];  //功率组成的数组
                double[] slopeEffArr = new double[indexEnd - indexStart + 1]; //电流点组成的数组

                //把数据装到数组中
                for (int i = indexStart; i <= indexEnd; i++)
                {
                    //if ( indexEnd + 2 > slopeEffs.Count)
                    //{
                    //    break;
                    //}
                    slopeEffArr[i - indexStart] = slopeEffs[i];
                    powerArr[i - indexStart] = powers[i];

                }

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:SE                      
                PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, slopeEffArr, 1);

                //计算出SE      
                double CalcSE = polyData.Coeffs[1] * power_mW + polyData.Coeffs[0];
                return CalcSE;

            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentForSpecifiedPower:", ex);
            }

        }

    }
}