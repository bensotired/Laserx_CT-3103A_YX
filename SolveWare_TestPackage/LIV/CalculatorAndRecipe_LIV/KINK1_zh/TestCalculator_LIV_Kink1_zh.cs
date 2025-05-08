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
    /// 找出SE最大值(纵慧版本)
    /// </summary>
    public class TestCalculator_LIV_Kink1_Percent_zh : TestCalculatorBase
    {
        public TestCalculator_LIV_Kink1_Percent_zh() : base() { } 

        public CalcRecipe_LIV_Kink1_zh CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Kink1_zh);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Kink1_zh)(testRecipe);
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Kink1_Percent_zh{CalcRecipe.CalcData_PostFix}");
                }

                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string PowerTag = "Power_mW";
                const string VoltageTag = "Voltage_V";
                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag, VoltageTag);

                if (dict[CurrentTag]?.Count <= 0 ||
                    dict[PowerTag]?.Count <= 0 ||
                    dict[VoltageTag]?.Count <= 0 ||
                    dict[CurrentTag]?.Count != dict[PowerTag]?.Count ||
                    dict[CurrentTag]?.Count != dict[VoltageTag]?.Count)
                {
                    throw new Exception($"Kink1: xArray and yArray are of unequal size!");
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
                var ith2 = polyData.Coeffs[0];


                double[] SElist_All = ArrayMath.CalculateFirstDerivate(dict[CurrentTag].ToArray(), dict[PowerTag].ToArray());

                //计算出区间的SE 寻找最大的SE
                double SE_Find_StartCurrent = ith2 + this.CalcRecipe.SEMax_Start_AboveIthCurrent;
                double SE_Find_EndCurrent   = ith2 + this.CalcRecipe.SEMax_End_AboveIthCurrent;
                List<double> SElist_SelectRange = new List<double>();
                List<double> Currentlist_SelectRange = new List<double>(); 
                for (int i = 0; i < dict[CurrentTag].Count; i++)
                {
                    if (SE_Find_StartCurrent <= dict[CurrentTag][i] && dict[CurrentTag][i] <= SE_Find_EndCurrent)
                    {
                        SElist_SelectRange.Add(SElist_All[i]);
                        Currentlist_SelectRange.Add(dict[CurrentTag][i]);
                    }
                }
                double SEMax  = SElist_SelectRange.Max();
               //double SE0 = Interpolator.DoPiecewiseLinearInterpolation(CalcRecipe. SE0_Power, dict[PowerTag].ToArray(), SElist_All);
                
                double A2_mA= (double)this.GetCurrentForSpecifiedPower(dict[CurrentTag], dict[PowerTag], CalcRecipe.A2_Power_mW);   //计算出功率对应的电流 

                double A1_SE = (double)this.GetCurrentForSpecifiedPower( SElist_All.ToList(),dict[CurrentTag], ith2 + CalcRecipe.A1_AboveIthCurrent);   //计算出电流对应的SE




                double A2_SE = (double)this.GetCurrentForSpecifiedPower(SElist_All.ToList(),dict[CurrentTag],  A2_mA);   //计算出电流对应的SE 
                //将这2个点数据进行直线拟合
                double[] LineSEArr = new double[] { A1_SE, A2_SE };

                double[] LinecurrentArr = new double[] { ith2 + CalcRecipe.A1_AboveIthCurrent, A2_mA };
                if (ith2 + CalcRecipe.A1_AboveIthCurrent< A2_mA)
                {
                      LineSEArr = new double[] { A1_SE, A2_SE };
                       LinecurrentArr = new double[] { ith2 + CalcRecipe.A1_AboveIthCurrent, A2_mA };
                }
                else
                {
                    LineSEArr = new double[] {  A2_SE , A1_SE, };
                    LinecurrentArr = new double[] {  A2_mA , ith2 + CalcRecipe.A1_AboveIthCurrent};
                }


                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:电流                       
                double[] SEline1 = PolyFitMath.PolynomialFit(LineSEArr, LinecurrentArr, 1).FittedYArray;
                var line1Coeffs = PolyFitMath.PolynomialFit(LinecurrentArr, LineSEArr, 1);

                var k_line1 = line1Coeffs.Coeffs[1];
                var b_line1 = line1Coeffs.Coeffs[0];


             var line2Fitted =   PolyFitMath.PolynomialFit(Currentlist_SelectRange.ToArray(), SElist_SelectRange.ToArray(), 2);

                double del_y = CalcRecipe.SE_Line_YDiff;//0.001;这个放在recipe里面

      
                List<double> interPointCurrents = new List<double>();
                List<double> interPointSe = new List<double>();

                for (int i = 0; i < Currentlist_SelectRange.Count; i++)
                {
                    var currentBase = Currentlist_SelectRange[i];
                    var y_line2 = line2Fitted.Coeffs[2] * currentBase * currentBase +
                                    line2Fitted.Coeffs[1] * currentBase +
                                    line2Fitted.Coeffs[0];

                    var y_line1 = line1Coeffs.Coeffs[1] * currentBase + line1Coeffs.Coeffs[0];

                    if (Math.Abs(y_line2 - y_line1) < del_y)
                    {
                        interPointCurrents.Add(currentBase);
                        interPointSe.Add(y_line2);
                    }
                }
                double se0 = double.NaN;
                if(interPointSe.Count >  0)
                {
                    se0 = interPointSe.First();
                }
                if(se0 == double.NaN)
                {
                    dataSummary_1.Value = 0;
                }
                else
                {
                    dataSummary_1.Value = Math.Round(100 * SEMax / se0);//使用的是百分比
                }

                return;
  
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Kink1_Percent_zh_ParamError, ex);
            }
        }
        public double GetSE0(List<double> Line1_X, List<double> Line1_Y, List<double> Line2_X, List<double> Line2_Y)
        {
            // line2 = I - Slope曲线   就是点1的线 就是你这里的  current_SelectRange vs  SElist_ SelectRange
            //line1 就是 LinecurrentArr vs SEline1
            //按line 1遍历x找出 line1的 y与 line2该x下的y  先筛选一遍
            //会得到 line 1 y与line 2 y 最靠近的一组
            //然后 给他一个插值补偿就可以得到   se0 就是 line1与line 2相等的 y
          
            double DifferenceValue = double.PositiveInfinity;
            int  index = 0; 
            for (int i = 0; i < Line1_Y.Count; i++)
            {
                for (int j = 0; j < Line2_Y.Count; j++)
                {
                    if (Math.Abs(Line1_Y[i] - Line2_Y[j]) < DifferenceValue)
                    {
                        DifferenceValue = Math.Abs(Line1_Y[i] - Line2_Y[j]);
                        index = j; 
                    }                   
                }
            }
            //将该区间的数据进行直线拟合
            double[] Diff_xArr = new double[3];  
            double[] Diff_yArr = new double[3]; 

            //把数据装到数组中         
            for (int i = 0; i <3; i++)
            {               
                Diff_xArr[i] = Line2_X[index-1+i];
                Diff_yArr[i] = Line2_Y[index - 1 + i];
            }
            //拟合成函数, 设定为1阶拟合                               
            PolyFitData polyData = PolyFitMath.PolynomialFit(Diff_xArr, Diff_yArr, 1);

            //计算出SE      
            double SE0 = polyData.Coeffs[1] * Line2_X[index] + polyData.Coeffs[0];
            return SE0;
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
        public double GetSlopeEfficiencyForSpecifiedCurrent(List<double> slopeEffs, List<double> currents, double current_mA)
        {
            try
            {

                int indexStart;
                int indexEnd;

                //找到比需求功率大的功率
                for (indexEnd = 0; indexEnd < slopeEffs.Count; indexEnd++)
                {
                    if (currents[indexEnd] >= current_mA)
                    {
                        break;
                    }
                }
                if (indexEnd == slopeEffs.Count) return 0;

                //找到比需求功率小的功率
                for (indexStart = indexEnd; indexStart > 0; indexStart--)
                {
                    if (currents[indexStart] < current_mA)
                    {
                        break;
                    }
                }
                if (indexStart == 0) return 0;


                //将该区间的数据进行直线拟合
                double[] currentArr = new double[indexEnd - indexStart + 1];  //功率组成的数组
                double[] slopeEffArr = new double[indexEnd - indexStart + 1]; //电流点组成的数组

                //把数据装到数组中
                for (int i = indexStart; i <= indexEnd; i++)
                {
                    slopeEffArr[i - indexStart] = slopeEffs[i];
                    currentArr[i - indexStart] = currents[i];

                }

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:SE                      
                PolyFitData polyData = PolyFitMath.PolynomialFit(currentArr, slopeEffArr, 1);

                //计算出SE      
                double CalcSE = polyData.Coeffs[1] * current_mA + polyData.Coeffs[0];
                return CalcSE;

            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentForSpecifiedPower:", ex);
            }

        }
    }
}