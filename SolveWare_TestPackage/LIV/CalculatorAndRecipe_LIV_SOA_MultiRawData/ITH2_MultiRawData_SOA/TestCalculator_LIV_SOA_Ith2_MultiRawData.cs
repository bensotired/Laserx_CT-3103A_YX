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
    public class TestCalculator_LIV_SOA_Ith2_MultiRawData : TestCalculatorBase
    {
        public TestCalculator_LIV_SOA_Ith2_MultiRawData() : base() { }

        public CalcRecipe_LIV_SOA_Ith2_MultiRawData CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_SOA_Ith2_MultiRawData);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_SOA_Ith2_MultiRawData)(testRecipe);
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
                        dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}SOA1_Ith2{CalcRecipe.CalcData_PostFix}";
                        //if (CalcRecipe.IsForceRename == true)
                        //{
                        //    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                        //}
                        //else
                        //{
                        //    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Ith2{CalcRecipe.CalcData_PostFix}");
                        //}

                        dataSummary_1.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_1);

                        for (int i = 0; i < dict[PowerTag_S1].Count; i++)
                        {
                            dict[PowerTag_S1][i] = dict[PowerTag_S1][i] * (-1);
                        }

                        double current1_mA = (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag_S1], CalcRecipe.Ith2_StartP_mW);   //计算出功率对应的电流
                        double current2_mA = (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag_S1], CalcRecipe.Ith2_StopP_mW);   //计算出功率对应的电流

                        //将这2个点数据进行直线拟合
                        double[] powerArr = new double[] { CalcRecipe.Ith2_StartP_mW, CalcRecipe.Ith2_StopP_mW };

                        double[] currentArr = new double[] { current1_mA, current2_mA };

                        //拟合成函数, 设定为1阶拟合 
                        //X:功率
                        //Y:电流                       
                        PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, currentArr, 1);

                        //计算出电流
                        dataSummary_1.Value = polyData.Coeffs[0];
                        #endregion

                        #region SOA2
                        SummaryDatumItemBase dataSummary_2 = new SummaryDatumItemBase();
                        dataSummary_2.Name = $"{CalcRecipe.CalcData_PreFix}SOA2_Ith2{CalcRecipe.CalcData_PostFix}";
                        //if (CalcRecipe.IsForceRename == true)
                        //{
                        //    dataSummary_2.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                        //}
                        //else
                        //{
                        //    dataSummary_2.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Ith2{CalcRecipe.CalcData_PostFix}");
                        //}

                        dataSummary_2.Value = double.NaN;
                        summaryDataWithoutSpec.Add(dataSummary_2);

                        for (int i = 0; i < dict[PowerTag_S2].Count; i++)
                        {
                            dict[PowerTag_S2][i] = dict[PowerTag_S2][i] * (-1);
                        }

                        double current1_mA2 = (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag_S2], CalcRecipe.Ith2_StartP_mW);   //计算出功率对应的电流
                        double current2_mA2 = (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag_S2], CalcRecipe.Ith2_StopP_mW);   //计算出功率对应的电流

                        //将这2个点数据进行直线拟合
                        double[] powerArr2 = new double[] { CalcRecipe.Ith2_StartP_mW, CalcRecipe.Ith2_StopP_mW };

                        double[] currentArr2 = new double[] { current1_mA2, current2_mA2 };

                        //拟合成函数, 设定为1阶拟合 
                        //X:功率
                        //Y:电流                       
                        PolyFitData polyData2 = PolyFitMath.PolynomialFit(powerArr2, currentArr2, 1);

                        //计算出电流
                        dataSummary_2.Value = polyData2.Coeffs[0];
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
                    dataSummary_1.Name = $"{CalcRecipe.CalcData_PreFix}SOA1_Ith2{CalcRecipe.CalcData_PostFix}";
                    //if (CalcRecipe.IsForceRename == true)
                    //{
                    //    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                    //}
                    //else
                    //{
                    //    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Ith2{CalcRecipe.CalcData_PostFix}");
                    //}

                    dataSummary_1.Value = double.NaN;
                    summaryDataWithoutSpec.Add(dataSummary_1);

                    for (int i = 0; i < dict[PowerTag_S1].Count; i++)
                    {
                        dict[PowerTag_S1][i] = dict[PowerTag_S1][i] * (-1);
                    }

                    double current1_mA = (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag_S1], CalcRecipe.Ith2_StartP_mW);   //计算出功率对应的电流
                    double current2_mA = (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag_S1], CalcRecipe.Ith2_StopP_mW);   //计算出功率对应的电流

                    //将这2个点数据进行直线拟合
                    double[] powerArr = new double[] { CalcRecipe.Ith2_StartP_mW, CalcRecipe.Ith2_StopP_mW };

                    double[] currentArr = new double[] { current1_mA, current2_mA };

                    //拟合成函数, 设定为1阶拟合 
                    //X:功率
                    //Y:电流                       
                    PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, currentArr, 1);

                    //计算出电流
                    dataSummary_1.Value = polyData.Coeffs[0];
                    #endregion

                    #region SOA2
                    SummaryDatumItemBase dataSummary_2 = new SummaryDatumItemBase();
                    dataSummary_2.Name = $"{CalcRecipe.CalcData_PreFix}SOA2_Ith2{CalcRecipe.CalcData_PostFix}";
                    //if (CalcRecipe.IsForceRename == true)
                    //{
                    //    dataSummary_2.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                    //}
                    //else
                    //{
                    //    dataSummary_2.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Ith2{CalcRecipe.CalcData_PostFix}");
                    //}

                    dataSummary_2.Value = double.NaN;
                    summaryDataWithoutSpec.Add(dataSummary_2);

                    for (int i = 0; i < dict[PowerTag_S2].Count; i++)
                    {
                        dict[PowerTag_S2][i] = dict[PowerTag_S2][i] * (-1);
                    }

                    double current1_mA2 = (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag_S2], CalcRecipe.Ith2_StartP_mW);   //计算出功率对应的电流
                    double current2_mA2 = (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag_S2], CalcRecipe.Ith2_StopP_mW);   //计算出功率对应的电流

                    //将这2个点数据进行直线拟合
                    double[] powerArr2 = new double[] { CalcRecipe.Ith2_StartP_mW, CalcRecipe.Ith2_StopP_mW };

                    double[] currentArr2 = new double[] { current1_mA2, current2_mA2 };

                    //拟合成函数, 设定为1阶拟合 
                    //X:功率
                    //Y:电流                       
                    PolyFitData polyData2 = PolyFitMath.PolynomialFit(powerArr2, currentArr2, 1);

                    //计算出电流
                    dataSummary_2.Value = polyData2.Coeffs[0];
                    #endregion
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