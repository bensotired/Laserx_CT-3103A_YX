using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Ith2算子 拟合  根据1100
    /// 找出目标电流点下对应的器件输出功率
    /// </summary>
    public class TestCalculator_LIV_Tap_PD_Ith2_MultiRawData : TestCalculatorBase
    {
        public TestCalculator_LIV_Tap_PD_Ith2_MultiRawData() : base() { }

        public CalcRecipe_LIV_Tap_PD_Ith2_MultiRawData CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Tap_PD_Ith2_MultiRawData);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Tap_PD_Ith2_MultiRawData)(testRecipe);
        }

        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                if (rawData is IRawDataMenuCollection)
                {
                    var multiRawData = rawData as IRawDataMenuCollection;
                    var rawdata = multiRawData.GetDataMenuCollection();
                    foreach (var item in rawdata)
                    {
                       
                        const string CurrentTag = "Current_mA";
                        const string PowerTag = "Power_mW";
                        const string SectionTag = "Section";
                        var localRawData = item as IRawDataCollectionBase;
                        var dict = localRawData.GetDataDictByPropNames<object>(CurrentTag, PowerTag, SectionTag);
                        var sectionName = dict[SectionTag].First().ToString();
                        if (sectionName != "Gain")
                        {
                            continue;
                        }
                        SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                        dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}Tap_PD_Ith2{CalcRecipe.CalcData_PostFix}";
                        //if (CalcRecipe.IsForceRename == true)
                        //{
                        //    dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}{sectionName}_{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}";
                        //}
                        //else
                        //{
                        //    dataSummary_1.Name = string.Format(item.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{sectionName}_Ith2{CalcRecipe.CalcData_PostFix}");
                        //}
                        dataSummary_1.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_1);


                        if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag]?.Count)
                        {
                            throw new Exception($"Ith2: xArray and yArray are of unequal size!");
                        }

                        List<double> ldCurrs_org = dict[CurrentTag].ConvertAll<double>(new Converter<object, double>((obj) => { return Convert.ToDouble(obj); }));
                        List<double> ldPows_org = dict[PowerTag].ConvertAll<double>(new Converter<object, double>((obj) => { return Convert.ToDouble(obj); }));

                        double current1_mA = (double)this.GetCurrentForSpecifiedPower(ldCurrs_org, ldPows_org, CalcRecipe.Ith2_StartP_mW);   //计算出功率对应的电流
                        double current2_mA = (double)this.GetCurrentForSpecifiedPower(ldCurrs_org, ldPows_org, CalcRecipe.Ith2_StopP_mW);   //计算出功率对应的电流

                        //将这2个点数据进行直线拟合
                        double[] powerArr = new double[] { CalcRecipe.Ith2_StartP_mW, CalcRecipe.Ith2_StopP_mW };

                        double[] currentArr = new double[] { current1_mA, current2_mA };

                        //拟合成函数, 设定为1阶拟合 
                        //X:功率
                        //Y:电流                       
                        PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, currentArr, 1);

                        //计算出电流
                        dataSummary_1.Value = polyData.Coeffs[0];

                    }
                }
                else
                {
                    SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                    if (CalcRecipe.IsForceRename == true)
                    {
                        dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                    }
                    else
                    {
                        dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Ith2{CalcRecipe.CalcData_PostFix}");
                    }

                    dataSummary_1.Value = double.NaN;
                    summaryDataWithoutSpec.Add(dataSummary_1);
                    const string CurrentTag = "Current_mA";
                    const string PowerTag = "Power_mW";

                    var localRawData = rawData as IRawDataCollectionBase;
                    var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag);

                    if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag]?.Count)
                    {
                        throw new Exception($"Ith2: xArray and yArray are of unequal size!");
                    }

                    double current1_mA = (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag], CalcRecipe.Ith2_StartP_mW);   //计算出功率对应的电流
                    double current2_mA = (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag], CalcRecipe.Ith2_StopP_mW);   //计算出功率对应的电流

                    //将这2个点数据进行直线拟合
                    double[] powerArr = new double[] { CalcRecipe.Ith2_StartP_mW, CalcRecipe.Ith2_StopP_mW };

                    double[] currentArr = new double[] { current1_mA, current2_mA };

                    //拟合成函数, 设定为1阶拟合 
                    //X:功率
                    //Y:电流                       
                    PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, currentArr, 1);

                    //计算出电流
                    dataSummary_1.Value = polyData.Coeffs[0];

                }










            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Ith2_ParamError, ex);
            }
        }
        public double? GetCurrentForSpecifiedPower(List<double> CurrentList, List<double> PowerList, double Power_mW)
        {
            try
            {

                int indexS;
                int indexE;

                //找到比需求功率大的功率
                for (indexE = 0; indexE < PowerList.Count; indexE++)
                {
                    if (PowerList[indexE] >= Power_mW)
                    {
                        break;
                    }
                }
                if (indexE == PowerList.Count) return 0;

                //找到比需求功率小的功率
                for (indexS = indexE; indexS > 0; indexS--)
                {
                    if (PowerList[indexS] < Power_mW)
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
                    currentArr[i - indexS] = CurrentList[i];
                    powerArr[i - indexS] = PowerList[i];

                }

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:电流                       
                PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, currentArr, 1);

                //计算出电流
                double CalcCurrent = polyData.Coeffs[1] * Power_mW + polyData.Coeffs[0];

                return CalcCurrent;

            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentForSpecifiedPower:", ex);
            }

        }
    }
}