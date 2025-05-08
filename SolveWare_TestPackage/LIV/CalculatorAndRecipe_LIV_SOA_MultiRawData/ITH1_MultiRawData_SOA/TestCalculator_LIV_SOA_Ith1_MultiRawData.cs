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
    public class TestCalculator_LIV_SOA_Ith1_MultiRawData : TestCalculatorBase
    {
        public TestCalculator_LIV_SOA_Ith1_MultiRawData() : base() { }

        public CalcRecipe_LIV_SOA_Ith1_MultiRawData CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_SOA_Ith1_MultiRawData);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_SOA_Ith1_MultiRawData)(testRecipe);
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

                        const string CurrentTag = "G_Current_mA";
                        const string PowerTag_S1 = "S1_Current_mA";
                        const string PowerTag_S2 = "S2_Current_mA";

                        var localRawData = item as IRawDataCollectionBase;
                        var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag_S1, PowerTag_S2);

                        if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag_S1]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag_S1]?.Count)
                        {
                            throw new Exception($"Ith1: xArray and yArray are of unequal size!");
                        }
                        #region SOA1
                        SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                        dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}SOA1_Ith1{CalcRecipe.CalcData_PostFix}";
                        //if (CalcRecipe.IsForceRename == true)
                        //{
                        //    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                        //}
                        //else
                        //{
                        //    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Ith1{CalcRecipe.CalcData_PostFix}");
                        //}

                        dataSummary_1.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_1);

                        for (int i = 0; i < dict[PowerTag_S1].Count; i++)
                        {
                            dict[PowerTag_S1][i] = dict[PowerTag_S1][i] * (-1);
                        }


                        double slopeLowerLimit_mWpermA = 0.0001;
                        //20200226 对Ith 增加一个上极限, 默认为20mA
                        double ithUpperLimit_mA = CalcRecipe.IthLimitMax_mA;

                        double[] powers_mW = new double[dict[PowerTag_S1].Count];
                        double[] laserCurrents_mA = new double[dict[CurrentTag].Count];
                        for (int i = 0; i < dict[CurrentTag].Count; i++)
                        {
                            powers_mW[i] = dict[PowerTag_S1][i];
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
                        #endregion

                        #region SOA2
                        {
                            SummaryDatumItemBase dataSummary_2 = new SummaryDatumItemBase();
                            dataSummary_2.Name = $"{CalcRecipe.CalcData_PreFix}SOA2_Ith1{CalcRecipe.CalcData_PostFix}";
                            //if (CalcRecipe.IsForceRename == true)
                            //{
                            //    dataSummary_2.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                            //}
                            //else
                            //{
                            //    dataSummary_2.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Ith1{CalcRecipe.CalcData_PostFix}");
                            //}

                            dataSummary_2.Value = double.NaN;
                            summaryDataWithoutSpec.Add(dataSummary_2);

                            for (int i = 0; i < dict[PowerTag_S2].Count; i++)
                            {
                                dict[PowerTag_S1][i] = dict[PowerTag_S2][i] * (-1);
                            }


                            double slopeLowerLimit_mWpermA2 = 0.0001;
                            //20200226 对Ith 增加一个上极限, 默认为20mA
                            double ithUpperLimit_mA2 = CalcRecipe.IthLimitMax_mA;

                            double[] powers_mW2 = new double[dict[PowerTag_S1].Count];
                            double[] laserCurrents_mA2 = new double[dict[CurrentTag].Count];
                            for (int i = 0; i < dict[CurrentTag].Count; i++)
                            {
                                powers_mW2[i] = dict[PowerTag_S1][i];
                                laserCurrents_mA2[i] = dict[CurrentTag][i];
                            }

                            double[] firstDerivativeValues2 = ArrayMath.CalculateFirstDerivate(laserCurrents_mA2, powers_mW2);
                            double maxSlope_mWpermA2 = double.MinValue;
                            int indexAtMaxSlope_mWpermA2 = 0;
                            for (int i = 0; i < laserCurrents_mA2.Length; i++)
                            {
                                if (laserCurrents_mA2[i] > ithUpperLimit_mA2)
                                {
                                    break;
                                }
                                if (firstDerivativeValues2[i] > maxSlope_mWpermA2)
                                {
                                    maxSlope_mWpermA2 = firstDerivativeValues2[i];
                                    indexAtMaxSlope_mWpermA2 = i;
                                }
                            }

                            if (maxSlope_mWpermA2 < slopeLowerLimit_mWpermA2)
                            {
                                dataSummary_2.Value = 0;
                                return;
                            }

                            if (laserCurrents_mA2.Length < 3)
                            {
                                dataSummary_2.Value = 0;
                                return;
                            }

                            double step_mA2 = laserCurrents_mA2[1] - laserCurrents_mA2[0];
                            int startOffset2 = (int)(Math.Round(CalcRecipe.Ith1_Cal_Left_Offset_mA / step_mA2, 0));
                            int stopOffset2 = (int)(Math.Round(CalcRecipe.Ith1_Cal_Right_Offset_mA / step_mA2, 0));

                            double halfMaxSlope_mWpermA2 = maxSlope_mWpermA2 / 2;
                            double ith_Int2 = 0;
                            int indexAtIth_Int2 = 0;
                            for (int i = 0; i < laserCurrents_mA2.Length - 1; i++)
                            {
                                if (firstDerivativeValues2[i] <= halfMaxSlope_mWpermA2 && firstDerivativeValues2[i + 1] > halfMaxSlope_mWpermA2)
                                {
                                    if (halfMaxSlope_mWpermA2 - firstDerivativeValues2[i] > firstDerivativeValues2[i + 1] - halfMaxSlope_mWpermA2)
                                    {
                                        ith_Int2 = laserCurrents_mA2[i + 1];
                                        indexAtIth_Int2 = i + 1;
                                    }
                                    else
                                    {
                                        ith_Int2 = laserCurrents_mA2[i];
                                        indexAtIth_Int2 = i;
                                    }

                                    break;
                                }
                            }

                            if (indexAtIth_Int2 + stopOffset2 > laserCurrents_mA2.Length - 1 || indexAtIth_Int2 - startOffset2 < 0)
                            {
                                dataSummary_2.Value = 0;
                                return;
                            }

                            List<double> currents2 = new List<double>();
                            List<double> powers2 = new List<double>();
                            for (int i = indexAtIth_Int2 + 1; i <= indexAtIth_Int2 + stopOffset2; i++)
                            {
                                currents2.Add(laserCurrents_mA2[i]);
                                powers2.Add(powers_mW2[i]);
                            }

                            PolyFitData aboveIthFitData2 = PolyFitMath.PolynomialFit(currents2.ToArray(), powers2.ToArray(), 1);

                            currents2.Clear();
                            powers2.Clear();
                            for (int i = indexAtIth_Int2 - startOffset2; i <= indexAtIth_Int2 - 1; i++)
                            {
                                currents2.Add(laserCurrents_mA2[i]);
                                powers2.Add(powers_mW2[i]);
                            }

                            PolyFitData bolowIthFitData2 = PolyFitMath.PolynomialFit(currents2.ToArray(), powers2.ToArray(), 1);
                            dataSummary_2.Value = Math.Round((bolowIthFitData2.Coeffs[0] - aboveIthFitData2.Coeffs[0]) / (aboveIthFitData2.Coeffs[1] - bolowIthFitData2.Coeffs[1]), 3);
                        }
                        #endregion
                    }
                }
                else
                {
                    
                    const string CurrentTag = "G_Current_mA";
                    const string PowerTag_S1 = "S1_Current_mA";
                    const string PowerTag_S2 = "S2_Current_mA";

                    var localRawData = rawData as IRawDataCollectionBase;
                    var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag_S1, PowerTag_S2);

                    if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag_S1]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag_S1]?.Count)
                    {
                        throw new Exception($"Ith1: xArray and yArray are of unequal size!");
                    }
                    #region SOA1
                    SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                    dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}SOA1_Ith1{CalcRecipe.CalcData_PostFix}";
                    //if (CalcRecipe.IsForceRename == true)
                    //{
                    //    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                    //}
                    //else
                    //{
                    //    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Ith1{CalcRecipe.CalcData_PostFix}");
                    //}

                    dataSummary_1.Value = double.NaN;
                    summaryDataWithoutSpec.Add(dataSummary_1);

                    for (int i = 0; i < dict[PowerTag_S1].Count; i++)
                    {
                        dict[PowerTag_S1][i] = dict[PowerTag_S1][i] * (-1);
                    }


                    double slopeLowerLimit_mWpermA = 0.0001;
                    //20200226 对Ith 增加一个上极限, 默认为20mA
                    double ithUpperLimit_mA = CalcRecipe.IthLimitMax_mA;

                    double[] powers_mW = new double[dict[PowerTag_S1].Count];
                    double[] laserCurrents_mA = new double[dict[CurrentTag].Count];
                    for (int i = 0; i < dict[CurrentTag].Count; i++)
                    {
                        powers_mW[i] = dict[PowerTag_S1][i];
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
                    #endregion

                    #region SOA2
                    {
                        SummaryDatumItemBase dataSummary_2 = new SummaryDatumItemBase();
                        dataSummary_2.Name = $"{CalcRecipe.CalcData_PreFix}SOA2_Ith1{CalcRecipe.CalcData_PostFix}";
                        //if (CalcRecipe.IsForceRename == true)
                        //{
                        //    dataSummary_2.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                        //}
                        //else
                        //{
                        //    dataSummary_2.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Ith1{CalcRecipe.CalcData_PostFix}");
                        //}

                        dataSummary_2.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_2);

                        for (int i = 0; i < dict[PowerTag_S2].Count; i++)
                        {
                            dict[PowerTag_S1][i] = dict[PowerTag_S2][i] * (-1);
                        }


                        double slopeLowerLimit_mWpermA2 = 0.0001;
                        //20200226 对Ith 增加一个上极限, 默认为20mA
                        double ithUpperLimit_mA2 = CalcRecipe.IthLimitMax_mA;

                        double[] powers_mW2 = new double[dict[PowerTag_S1].Count];
                        double[] laserCurrents_mA2 = new double[dict[CurrentTag].Count];
                        for (int i = 0; i < dict[CurrentTag].Count; i++)
                        {
                            powers_mW2[i] = dict[PowerTag_S1][i];
                            laserCurrents_mA2[i] = dict[CurrentTag][i];
                        }

                        double[] firstDerivativeValues2 = ArrayMath.CalculateFirstDerivate(laserCurrents_mA2, powers_mW2);
                        double maxSlope_mWpermA2 = double.MinValue;
                        int indexAtMaxSlope_mWpermA2 = 0;
                        for (int i = 0; i < laserCurrents_mA2.Length; i++)
                        {
                            if (laserCurrents_mA2[i] > ithUpperLimit_mA2)
                            {
                                break;
                            }
                            if (firstDerivativeValues2[i] > maxSlope_mWpermA2)
                            {
                                maxSlope_mWpermA2 = firstDerivativeValues2[i];
                                indexAtMaxSlope_mWpermA2 = i;
                            }
                        }

                        if (maxSlope_mWpermA2 < slopeLowerLimit_mWpermA2)
                        {
                            dataSummary_2.Value = 0;
                            return;
                        }

                        if (laserCurrents_mA2.Length < 3)
                        {
                            dataSummary_2.Value = 0;
                            return;
                        }

                        double step_mA2 = laserCurrents_mA2[1] - laserCurrents_mA2[0];
                        int startOffset2 = (int)(Math.Round(CalcRecipe.Ith1_Cal_Left_Offset_mA / step_mA2, 0));
                        int stopOffset2 = (int)(Math.Round(CalcRecipe.Ith1_Cal_Right_Offset_mA / step_mA2, 0));

                        double halfMaxSlope_mWpermA2 = maxSlope_mWpermA2 / 2;
                        double ith_Int2 = 0;
                        int indexAtIth_Int2 = 0;
                        for (int i = 0; i < laserCurrents_mA2.Length - 1; i++)
                        {
                            if (firstDerivativeValues2[i] <= halfMaxSlope_mWpermA2 && firstDerivativeValues2[i + 1] > halfMaxSlope_mWpermA2)
                            {
                                if (halfMaxSlope_mWpermA2 - firstDerivativeValues2[i] > firstDerivativeValues2[i + 1] - halfMaxSlope_mWpermA2)
                                {
                                    ith_Int2 = laserCurrents_mA2[i + 1];
                                    indexAtIth_Int2 = i + 1;
                                }
                                else
                                {
                                    ith_Int2 = laserCurrents_mA2[i];
                                    indexAtIth_Int2 = i;
                                }

                                break;
                            }
                        }

                        if (indexAtIth_Int2 + stopOffset2 > laserCurrents_mA2.Length - 1 || indexAtIth_Int2 - startOffset2 < 0)
                        {
                            dataSummary_2.Value = 0;
                            return;
                        }

                        List<double> currents2 = new List<double>();
                        List<double> powers2 = new List<double>();
                        for (int i = indexAtIth_Int2 + 1; i <= indexAtIth_Int2 + stopOffset2; i++)
                        {
                            currents2.Add(laserCurrents_mA2[i]);
                            powers2.Add(powers_mW2[i]);
                        }

                        PolyFitData aboveIthFitData2 = PolyFitMath.PolynomialFit(currents2.ToArray(), powers2.ToArray(), 1);

                        currents2.Clear();
                        powers2.Clear();
                        for (int i = indexAtIth_Int2 - startOffset2; i <= indexAtIth_Int2 - 1; i++)
                        {
                            currents2.Add(laserCurrents_mA2[i]);
                            powers2.Add(powers_mW2[i]);
                        }

                        PolyFitData bolowIthFitData2 = PolyFitMath.PolynomialFit(currents2.ToArray(), powers2.ToArray(), 1);
                        dataSummary_2.Value = Math.Round((bolowIthFitData2.Coeffs[0] - aboveIthFitData2.Coeffs[0]) / (aboveIthFitData2.Coeffs[1] - bolowIthFitData2.Coeffs[1]), 3);
                    }
                    #endregion
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