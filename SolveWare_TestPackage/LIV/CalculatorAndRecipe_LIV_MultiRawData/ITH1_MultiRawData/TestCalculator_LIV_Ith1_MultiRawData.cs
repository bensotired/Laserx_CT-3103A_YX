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
    /// Ith1算子 拟合  根据1100
    /// 找出目标电流点下对应的器件输出功率
    /// </summary>
    public class TestCalculator_LIV_Ith1_MultiRawData : TestCalculatorBase
    {
        public TestCalculator_LIV_Ith1_MultiRawData() : base() { }

        public CalcRecipe_LIV_Ith1_MultiRawData CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Ith1_MultiRawData);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Ith1_MultiRawData)(testRecipe);
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
                        dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}LIV_Ith1{CalcRecipe.CalcData_PostFix}";
                        //if (CalcRecipe.IsForceRename == true)
                        //{
                        //    dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}{sectionName}_{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}";
                        //}
                        //else
                        //{
                        //    dataSummary_1.Name = string.Format(item.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{sectionName}_Ith1{CalcRecipe.CalcData_PostFix}");
                        //}

                        dataSummary_1.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_1);
                        if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag]?.Count)
                        {
                            throw new Exception($"Ith1: xArray and yArray are of unequal size!");
                        }


                        List<double> ldCurrs_org = dict[CurrentTag].ConvertAll<double>(new Converter<object, double>((obj) => { return Convert.ToDouble(obj); }));
                        List<double> ldPows_org = dict[PowerTag].ConvertAll<double>(new Converter<object, double>((obj) => { return Convert.ToDouble(obj); }));





                        double slopeLowerLimit_mWpermA = 0.0001;
                        //20200226 对Ith 增加一个上极限, 默认为20mA
                        double ithUpperLimit_mA = CalcRecipe.IthLimitMax_mA;

                        double[] powers_mW = new double[dict[PowerTag].Count];
                        double[] laserCurrents_mA = new double[dict[CurrentTag].Count];
                        for (int i = 0; i < dict[CurrentTag].Count; i++)
                        {
                            laserCurrents_mA[i] = ldCurrs_org[i];
                            powers_mW[i] = ldPows_org[i];
                        }

                        double[] firstDerivativeValues = ArrayMath.CalculateFirstDerivate(laserCurrents_mA, powers_mW);
                        double maxSlope_mWpermA = double.MinValue;
                        int indexAtMaxSlope_mWpermA = 0;
                        for (int i = 0; i < laserCurrents_mA.Length; i++)
                        {
                            if (laserCurrents_mA[i] > ithUpperLimit_mA)
                            {
                                break;
                            }
                            if (firstDerivativeValues[i] > maxSlope_mWpermA)
                            {
                                maxSlope_mWpermA = firstDerivativeValues[i];
                                indexAtMaxSlope_mWpermA = i;
                            }
                        }

                        if (maxSlope_mWpermA < slopeLowerLimit_mWpermA)
                        {
                            dataSummary_1.Value = 0;
                            return;
                        }

                        if (laserCurrents_mA.Length < 3)
                        {
                            dataSummary_1.Value = 0;
                            return;
                        }

                        double step_mA = laserCurrents_mA[1] - laserCurrents_mA[0];
                        int startOffset = (int)(Math.Round(CalcRecipe.Ith1_Cal_Left_Offset_mA / step_mA, 0));
                        int stopOffset = (int)(Math.Round(CalcRecipe.Ith1_Cal_Right_Offset_mA / step_mA, 0));

                        double halfMaxSlope_mWpermA = maxSlope_mWpermA / 2;
                        double ith_Int = 0;
                        int indexAtIth_Int = 0;
                        for (int i = 0; i < laserCurrents_mA.Length - 1; i++)
                        {
                            if (firstDerivativeValues[i] <= halfMaxSlope_mWpermA && firstDerivativeValues[i + 1] > halfMaxSlope_mWpermA)
                            {
                                if (halfMaxSlope_mWpermA - firstDerivativeValues[i] > firstDerivativeValues[i + 1] - halfMaxSlope_mWpermA)
                                {
                                    ith_Int = laserCurrents_mA[i + 1];
                                    indexAtIth_Int = i + 1;
                                }
                                else
                                {
                                    ith_Int = laserCurrents_mA[i];
                                    indexAtIth_Int = i;
                                }

                                break;
                            }
                        }

                        if (indexAtIth_Int + stopOffset > laserCurrents_mA.Length - 1 || indexAtIth_Int - startOffset < 0)
                        {
                            dataSummary_1.Value = 0;
                            return;
                        }

                        List<double> currents = new List<double>();
                        List<double> powers = new List<double>();
                        for (int i = indexAtIth_Int + 1; i <= indexAtIth_Int + stopOffset; i++)
                        {
                            currents.Add(laserCurrents_mA[i]);
                            powers.Add(powers_mW[i]);
                        }

                        PolyFitData aboveIthFitData = PolyFitMath.PolynomialFit(currents.ToArray(), powers.ToArray(), 1);

                        currents.Clear();
                        powers.Clear();
                        for (int i = indexAtIth_Int - startOffset; i <= indexAtIth_Int - 1; i++)
                        {
                            currents.Add(laserCurrents_mA[i]);
                            powers.Add(powers_mW[i]);
                        }

                        PolyFitData bolowIthFitData = PolyFitMath.PolynomialFit(currents.ToArray(), powers.ToArray(), 1);
                        dataSummary_1.Value = Math.Round((bolowIthFitData.Coeffs[0] - aboveIthFitData.Coeffs[0]) / (aboveIthFitData.Coeffs[1] - bolowIthFitData.Coeffs[1]), 3);
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
                        dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Ith1{CalcRecipe.CalcData_PostFix}");
                    }

                    dataSummary_1.Value = double.NaN;
                    summaryDataWithoutSpec.Add(dataSummary_1);
                    const string CurrentTag = "Current_mA";
                    const string PowerTag = "Power_mW";

                    var localRawData = rawData as IRawDataCollectionBase;
                    var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag);

                    if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag]?.Count)
                    {
                        throw new Exception($"Ith1: xArray and yArray are of unequal size!");
                    }

                    double slopeLowerLimit_mWpermA = 0.0001;
                    //20200226 对Ith 增加一个上极限, 默认为20mA
                    double ithUpperLimit_mA = CalcRecipe.IthLimitMax_mA;

                    double[] powers_mW = new double[dict[PowerTag].Count];
                    double[] laserCurrents_mA = new double[dict[CurrentTag].Count];
                    for (int i = 0; i < dict[CurrentTag].Count; i++)
                    {
                        powers_mW[i] = dict[PowerTag][i];
                        laserCurrents_mA[i] = dict[CurrentTag][i];
                    }

                    double[] firstDerivativeValues = ArrayMath.CalculateFirstDerivate(laserCurrents_mA, powers_mW);
                    double maxSlope_mWpermA = double.MinValue;
                    int indexAtMaxSlope_mWpermA = 0;
                    for (int i = 0; i < laserCurrents_mA.Length; i++)
                    {
                        if (laserCurrents_mA[i] > ithUpperLimit_mA)
                        {
                            break;
                        }
                        if (firstDerivativeValues[i] > maxSlope_mWpermA)
                        {
                            maxSlope_mWpermA = firstDerivativeValues[i];
                            indexAtMaxSlope_mWpermA = i;
                        }
                    }

                    if (maxSlope_mWpermA < slopeLowerLimit_mWpermA)
                    {
                        dataSummary_1.Value = 0;
                        return;
                    }

                    if (laserCurrents_mA.Length < 3)
                    {
                        dataSummary_1.Value = 0;
                        return;
                    }

                    double step_mA = laserCurrents_mA[1] - laserCurrents_mA[0];
                    int startOffset = (int)(Math.Round(CalcRecipe.Ith1_Cal_Left_Offset_mA / step_mA, 0));
                    int stopOffset = (int)(Math.Round(CalcRecipe.Ith1_Cal_Right_Offset_mA / step_mA, 0));

                    double halfMaxSlope_mWpermA = maxSlope_mWpermA / 2;
                    double ith_Int = 0;
                    int indexAtIth_Int = 0;
                    for (int i = 0; i < laserCurrents_mA.Length - 1; i++)
                    {
                        if (firstDerivativeValues[i] <= halfMaxSlope_mWpermA && firstDerivativeValues[i + 1] > halfMaxSlope_mWpermA)
                        {
                            if (halfMaxSlope_mWpermA - firstDerivativeValues[i] > firstDerivativeValues[i + 1] - halfMaxSlope_mWpermA)
                            {
                                ith_Int = laserCurrents_mA[i + 1];
                                indexAtIth_Int = i + 1;
                            }
                            else
                            {
                                ith_Int = laserCurrents_mA[i];
                                indexAtIth_Int = i;
                            }

                            break;
                        }
                    }

                    if (indexAtIth_Int + stopOffset > laserCurrents_mA.Length - 1 || indexAtIth_Int - startOffset < 0)
                    {
                        dataSummary_1.Value = 0;
                        return;
                    }

                    List<double> currents = new List<double>();
                    List<double> powers = new List<double>();
                    for (int i = indexAtIth_Int + 1; i <= indexAtIth_Int + stopOffset; i++)
                    {
                        currents.Add(laserCurrents_mA[i]);
                        powers.Add(powers_mW[i]);
                    }

                    PolyFitData aboveIthFitData = PolyFitMath.PolynomialFit(currents.ToArray(), powers.ToArray(), 1);

                    currents.Clear();
                    powers.Clear();
                    for (int i = indexAtIth_Int - startOffset; i <= indexAtIth_Int - 1; i++)
                    {
                        currents.Add(laserCurrents_mA[i]);
                        powers.Add(powers_mW[i]);
                    }

                    PolyFitData bolowIthFitData = PolyFitMath.PolynomialFit(currents.ToArray(), powers.ToArray(), 1);
                    dataSummary_1.Value = Math.Round((bolowIthFitData.Coeffs[0] - aboveIthFitData.Coeffs[0]) / (aboveIthFitData.Coeffs[1] - bolowIthFitData.Coeffs[1]), 3);
                }

               
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Ith1_ParamError, ex);
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
                    if (PowerList[indexE]>= Power_mW)
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