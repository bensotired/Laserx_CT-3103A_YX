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
    /// <summary>
    /// iop算子
    /// 找出目标功率点下对应的驱动电流值
    /// </summary>
    public class TestCalculator_PeaPer : TestCalculatorBase
    {
        public TestCalculator_PeaPer() : base() { } 
        public CalcRecipe_PeaPer CalcRecipe { get; private set; }
        public override Type GetCalcRecipeType() { return typeof(CalcRecipe_PeaPer); }
        public override void Localization(ICalcRecipe testRecipe) { CalcRecipe = (CalcRecipe_PeaPer)testRecipe; }
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Iop{CalcRecipe.CalcData_PostFix}");
                }


                dataSummary_1.Value = 0.0;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Angle_deg";
                const string PowerTag = "Power_mW";
                const string Angle = "Angle_deg"; 


                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag,Angle);

                if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 || dict[Angle]?.Count <= 0 | 
                    (dict[Angle]?.Count != dict[CurrentTag]?.Count|| dict[Angle]?.Count != dict[PowerTag]?.Count))
                {
                    throw new Exception($"Iop: xArray and yArray are of unequal size!");
                }
                //double power_mW = this.CalcRecipe.Power_mW; //可以从Recipe里面选的，不选了，直接用第一个方法
                //bool UsePDCurrentCalculatePER = this.CalcRecipe.UsePDCurrentCalculatePER;
                bool Curve_Fit = true;
                bool UsePDCurrentCalculatePER = true;
               double resultCurrent = CalculatotAllData_ReturnSingle(Curve_Fit, UsePDCurrentCalculatePER,dict[CurrentTag], dict[PowerTag],dict[Angle]);
                dataSummary_1.Value = resultCurrent; 
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Iop_ParamError, ex);
            }
        }
        public double CalculatotAllData_ReturnSingle(bool Curve_Fit,bool UsePDCurrentCalculatePER,List<double> currents, List<double> powers, List<double> angles)  
        {
            #region 未理后       
            try
            {
                PeaPerTestDataItem data = new PeaPerTestDataItem();
                double peakPower = double.MinValue;
                double nullPower = double.MaxValue;
                double peakCurrent = double.MinValue;
                double nullCurrent = double.MaxValue;
                double peakAngle = 0;
                double nullAngle = 0;

                if (Curve_Fit)
                {
                    var len = angles.Count / 2;
                    #region peak  
                    var peakSideAngles = ArrayMath.ExtractSubArray(angles.ToArray(), 0, len - 1);
                    var peakSidePowers = ArrayMath.ExtractSubArray(powers.ToArray(), 0, len - 1);
                    var peakSideCurrents = ArrayMath.ExtractSubArray(currents.ToArray(), 0, len - 1);

                    var polyFit_PeakPows = PolyFitMath.PolynomialFit(peakSideAngles, peakSidePowers, 2);
                    var polyFit_PeakCurrents = PolyFitMath.PolynomialFit(peakSideAngles, peakSideCurrents, 2);

                    var dPeakPows = ArrayMath.CalculateSmoothedNthDerivate(peakSideAngles, polyFit_PeakPows.FittedYArray, 1, 4, 2);

                    bool foundPeak = false;
                    int peakLeftIndex = 0;
                    int peakRightIndex = 0;
                    int peakIndex = 0;

                    for (int i = 0; i < dPeakPows.Length - 1; i++)
                    {
                        if (foundPeak == false)
                        {
                            if (dPeakPows[i] > 0 && dPeakPows[i + 1] < 0)
                            {
                                foundPeak = true;
                                peakLeftIndex = i;
                                peakRightIndex = i + 1;
                            }
                        }
                    }
                    if (Math.Abs(dPeakPows[peakLeftIndex]) < Math.Abs(dPeakPows[peakRightIndex]))
                    {
                        peakIndex = peakLeftIndex;
                    }
                    else
                    {
                        peakIndex = peakRightIndex;
                    }
                    var tempPeakAngle = peakSideAngles[peakIndex];
                    var tempPeakPower = polyFit_PeakPows.FittedYArray[peakIndex];
                    var tempPeakCurrent = polyFit_PeakCurrents.FittedYArray[peakIndex];
                    #endregion
                    #region valley
                    var valleySideAngles = ArrayMath.ExtractSubArray(angles.ToArray(), len, angles.Count - 1);
                    var valleySidePowers = ArrayMath.ExtractSubArray(powers.ToArray(), len, angles.Count - 1);
                    var valleySideCurrents = ArrayMath.ExtractSubArray(currents.ToArray(), len, angles.Count - 1);

                    var polyFit_valleyPows = PolyFitMath.PolynomialFit(valleySideAngles, valleySidePowers, 2);
                    var polyFit_valleyCurrents = PolyFitMath.PolynomialFit(valleySideAngles, valleySideCurrents, 2);

                    bool foundValley = false;
                    int valleyLeftIndex = 0;
                    int valleyRightIndex = 0;
                    int valleyIndex = 0;

                    var dValleyPows = ArrayMath.CalculateSmoothedNthDerivate(valleySideAngles, polyFit_valleyPows.FittedYArray, 1, 4, 2);

                    for (int i = 0; i < dValleyPows.Length - 1; i++)
                    {
                        if (foundValley == false)
                        {
                            if (dValleyPows[i] < 0 && dValleyPows[i + 1] > 0)
                            {
                                foundValley = true;
                                valleyLeftIndex = i;
                                valleyRightIndex = i + 1;
                            }
                        }
                    }

                    if (Math.Abs(dValleyPows[valleyLeftIndex]) < Math.Abs(dValleyPows[valleyRightIndex]))
                    {
                        valleyIndex = valleyLeftIndex;
                    }
                    else
                    {
                        valleyIndex = valleyRightIndex;
                    }
                    var tempValleyAngle = valleySideAngles[valleyIndex];
                    var tempValleyPower = polyFit_valleyPows.FittedYArray[valleyIndex];
                    var tempValleyCurrent = polyFit_valleyCurrents.FittedYArray[valleyIndex];
                    #endregion
                    //峰谷都有才继续 不然用旧方法
                    if (foundPeak && foundValley)
                    {
                        data.NullA1_Deg = tempValleyAngle;
                        data.Power_NullA1_mW = tempValleyPower;
                        data.PEA1_Deg = tempPeakAngle;
                        data.Power_PEA1_mW = tempPeakPower;

                        if (UsePDCurrentCalculatePER)
                        {
                            data.PER1_dB = 10 * Math.Log10(tempPeakCurrent / tempValleyCurrent);
                        }
                        else
                        {
                            data.PER1_dB = 10 * Math.Log10(tempPeakPower / tempValleyPower);
                        }
                        data.PEA = tempPeakAngle;
                    }
                    else
                    {
                        for (int i = 0; i < angles.Count; i++)
                        {
                            if (powers[i] > peakPower)
                            {
                                peakPower = powers[i];
                                peakAngle = angles[i];
                                peakCurrent = currents[i];
                            }
                            if (powers[i] < nullPower)
                            {
                                nullPower = powers[i];
                                nullAngle = angles[i];
                                nullCurrent = currents[i];
                            }
                        }
                        data.NullA1_Deg = nullAngle;
                        data.Power_NullA1_mW = nullPower;
                        data.PEA1_Deg = peakAngle;
                        data.Power_PEA1_mW = peakPower;
                        if (UsePDCurrentCalculatePER)
                        {
                            data.PER1_dB = 10 * Math.Log10(peakCurrent / nullCurrent);
                        }
                        else
                        {
                            data.PER1_dB = 10 * Math.Log10(peakPower / nullPower);
                        }

                        data.PEA = peakAngle;
                    }
                }
                else
                {
                    for (int i = 0; i < angles.Count; i++)
                    {
                        if (powers[i] > peakPower)
                        {
                            peakPower = powers[i];
                            peakAngle = angles[i];
                            peakCurrent = currents[i];
                        }
                        if (powers[i] < nullPower)
                        {
                            nullPower = powers[i];
                            nullAngle = angles[i];
                            nullCurrent = currents[i];
                        }
                    }
                    data.NullA1_Deg = nullAngle;
                    data.Power_NullA1_mW = nullPower;
                    data.PEA1_Deg = peakAngle;
                    data.Power_PEA1_mW = peakPower;
                    if (UsePDCurrentCalculatePER)
                    {
                        data.PER1_dB = 10 * Math.Log10(peakCurrent / nullCurrent);
                    }
                    else
                    {
                        data.PER1_dB = 10 * Math.Log10(peakPower / nullPower);
                    }
                    data.PEA = peakAngle;
                }
                return data.PEA;  //想传啥就传啥  
            }
            catch (Exception ex)
            {
                return 0;
                throw new Exception("CalculatotAllData_ReturnSingle:", ex);
            }
            #endregion

            #region 从3102里面移植过来的，未整理       
            //try
            //{

            //    //foreach (var rawData in RawDataList)
            //    //{
            //    //    string dataName = rawData.Key;
            //        PeaPerTestDataItem data = new PeaPerTestDataItem();
            //        double peakPower = double.MinValue;
            //        double nullPower = double.MaxValue;
            //        double peakCurrent = double.MinValue;
            //        double nullCurrent = double.MaxValue;
            //        double peakAngle = 0;
            //        double nullAngle = 0;
            //        //if (Curve_Fit)
            //        //20200904变成离散的  不能用了
            //        //if (true)
            //        //20200904 12：39变成离散的 分段算 又可以用了
            //        if (Curve_Fit)
            //        {
            //            //List<double> angles = new List<double>();
            //            //List<double> powers = new List<double>();
            //            //List<double> currents = new List<double>();
            //            //foreach (var datapoint in rawData.Value.Points)
            //            //{
            //            //    angles.Add(datapoint.Angle_deg);
            //            //    powers.Add(datapoint.Power_mW);
            //            //    currents.Add(datapoint.Current_mA);
            //            //}

            //            var len = angles.Count / 2;
            //            #region peak  
            //            var peakSideAngles = ArrayMath.ExtractSubArray(angles.ToArray(), 0, len - 1);
            //            var peakSidePowers = ArrayMath.ExtractSubArray(powers.ToArray(), 0, len - 1);
            //            var peakSideCurrents = ArrayMath.ExtractSubArray(currents.ToArray(), 0, len - 1);

            //            var polyFit_PeakPows = PolyFitMath.PolynomialFit(peakSideAngles, peakSidePowers, 2);
            //            var polyFit_PeakCurrents = PolyFitMath.PolynomialFit(peakSideAngles, peakSideCurrents, 2);

            //            var dPeakPows = ArrayMath.CalculateSmoothedNthDerivate(peakSideAngles, polyFit_PeakPows.FittedYArray, 1, 4, 2);

            //            //int maxi_2nd_peak = 0;
            //            //int mini_2nd_peak = 0;
            //            //ArrayMath.GetMaxMinIndexOf2ndDerivative(peakSideAngles, polyFit_PeakPows.FittedYArray, out maxi_2nd_peak, out mini_2nd_peak);
            //            //var tempPeakAngle2ndDerivative = peakSideAngles[mini_2nd_peak];

            //            bool foundPeak = false;
            //            int peakLeftIndex = 0;
            //            int peakRightIndex = 0;
            //            int peakIndex = 0;

            //            for (int i = 0; i < dPeakPows.Length - 1; i++)
            //            {
            //                if (foundPeak == false)
            //                {
            //                    if (dPeakPows[i] > 0 && dPeakPows[i + 1] < 0)
            //                    {
            //                        foundPeak = true;
            //                        peakLeftIndex = i;
            //                        peakRightIndex = i + 1;
            //                    }
            //                }
            //            }
            //            if (Math.Abs(dPeakPows[peakLeftIndex]) < Math.Abs(dPeakPows[peakRightIndex]))
            //            {
            //                peakIndex = peakLeftIndex;
            //            }
            //            else
            //            {
            //                peakIndex = peakRightIndex;
            //            }
            //            var tempPeakAngle = peakSideAngles[peakIndex];
            //            var tempPeakPower = polyFit_PeakPows.FittedYArray[peakIndex];
            //            var tempPeakCurrent = polyFit_PeakCurrents.FittedYArray[peakIndex];
            //            #endregion
            //            #region valley
            //            var valleySideAngles = ArrayMath.ExtractSubArray(angles.ToArray(), len, angles.Count - 1);
            //            var valleySidePowers = ArrayMath.ExtractSubArray(powers.ToArray(), len, angles.Count - 1);
            //            var valleySideCurrents = ArrayMath.ExtractSubArray(currents.ToArray(), len, angles.Count - 1);

            //            var polyFit_valleyPows = PolyFitMath.PolynomialFit(valleySideAngles, valleySidePowers, 2);
            //            var polyFit_valleyCurrents = PolyFitMath.PolynomialFit(valleySideAngles, valleySideCurrents, 2);

            //            bool foundValley = false;
            //            int valleyLeftIndex = 0;
            //            int valleyRightIndex = 0;
            //            int valleyIndex = 0;

            //            var dValleyPows = ArrayMath.CalculateSmoothedNthDerivate(valleySideAngles, polyFit_valleyPows.FittedYArray, 1, 4, 2);

            //            //int maxi_2nd_valley = 0;
            //            //int mini_2nd_valley = 0;

            //            //ArrayMath.GetMaxMinIndexOf2ndDerivative(valleySideAngles, polyFit_valleyPows.FittedYArray, out maxi_2nd_valley, out mini_2nd_valley);
            //            //var tempValleyAngle2ndDerivative = valleySideAngles[mini_2nd_valley];

            //            for (int i = 0; i < dValleyPows.Length - 1; i++)
            //            {
            //                if (foundValley == false)
            //                {
            //                    if (dValleyPows[i] < 0 && dValleyPows[i + 1] > 0)
            //                    {
            //                        foundValley = true;
            //                        valleyLeftIndex = i;
            //                        valleyRightIndex = i + 1;
            //                    }
            //                }
            //            }

            //            if (Math.Abs(dValleyPows[valleyLeftIndex]) < Math.Abs(dValleyPows[valleyRightIndex]))
            //            {
            //                valleyIndex = valleyLeftIndex;
            //            }
            //            else
            //            {
            //                valleyIndex = valleyRightIndex;
            //            }
            //            var tempValleyAngle = valleySideAngles[valleyIndex];
            //            var tempValleyPower = polyFit_valleyPows.FittedYArray[valleyIndex];
            //            var tempValleyCurrent = polyFit_valleyCurrents.FittedYArray[valleyIndex];
            //            #endregion
            //            //峰谷都有才继续 不然用旧方法
            //            if (foundPeak && foundValley)
            //            {
            //                //I1_PEA_mA = data.I1_PEA_mA = rawData.Value.IPea;
            //                //Temp_PER_C = data.Temp_PER_C = rawData.Value.Temp;
            //                data.NullA1_Deg = tempValleyAngle;
            //                data.Power_NullA1_mW = tempValleyPower;
            //                data.PEA1_Deg = tempPeakAngle;
            //                data.Power_PEA1_mW = tempPeakPower;

            //                if (UsePDCurrentCalculatePER)
            //                {
            //                   data.PER1_dB = 10 * Math.Log10(tempPeakCurrent / tempValleyCurrent);
            //                }
            //                else
            //                {
            //                    data.PER1_dB = 10 * Math.Log10(tempPeakPower / tempValleyPower);
            //                }
            //                //data.NullA = angles[valleyIndex];
            //                //data.NullP = powers[valleyIndex];
            //                //data.PeakA = angles[peakIndex];
            //                //data.PeakP = powers[peakIndex];

            //                //if (UsePDCurrentCalculatePER)
            //                //{
            //                //    data.PER = 10 * Math.Log10(currents[peakIndex] / currents[valleyIndex]);
            //                //}
            //                //else
            //                //{
            //                //    data.PER = 10 * Math.Log10(powers[peakIndex] / powers[valleyIndex]);
            //                //}

            //                data.PEA = tempPeakAngle;
            //                //var datakey = dataName.Replace("raw", "cal");
            //                //DataList.Add(datakey, data);
            //                //PeaPerTestDataItem.Add(data);
            //            }
            //            else
            //            {
            //            //foreach (var datapoint in rawData.Value.Points)
            //            //{
            //            //    if (datapoint.Power_mW > peakPower)
            //            //    {
            //            //        peakPower = datapoint.Power_mW;
            //            //        peakAngle = datapoint.Angle_deg;
            //            //        peakCurrent = datapoint.Current_mA;
            //            //    }
            //            //    if (datapoint.Power_mW < nullPower)
            //            //    {
            //            //        nullPower = datapoint.Power_mW;
            //            //        nullAngle = datapoint.Angle_deg;
            //            //        nullCurrent = datapoint.Current_mA;
            //            //    }
            //            //}currents, List<double> powers, List<double> angles)  
            //            for (int i = 0; i < angles.Count; i++)
            //            {
            //                if (powers[i] > peakPower)
            //                {
            //                    peakPower = powers[i];
            //                    peakAngle = angles[i];
            //                    peakCurrent = currents[i];
            //                }
            //                if (powers[i] < nullPower)
            //                {
            //                    nullPower = powers[i];
            //                    nullAngle = angles[i];
            //                    nullCurrent = currents[i];
            //                }
            //            }
            //            //I1_PEA_mA = data.I1_PEA_mA = rawData.Value.IPea;
            //            //Temp_PER_C = data.Temp_PER_C = rawData.Value.Temp;
            //            data.NullA1_Deg = nullAngle;
            //                data.Power_NullA1_mW = nullPower;
            //                data.PEA1_Deg = peakAngle;
            //                data.Power_PEA1_mW = peakPower;
            //                if (UsePDCurrentCalculatePER)
            //                {
            //                    data.PER1_dB = 10 * Math.Log10(peakCurrent / nullCurrent);
            //                }
            //                else
            //                {
            //                    data.PER1_dB = 10 * Math.Log10(peakPower / nullPower);
            //                }

            //                data.PEA = peakAngle;
            //                //var datakey = dataName.Replace("raw", "cal");
            //                //DataList.Add(datakey, data);
            //                //PeaPerTestDataItem.Add(data);
            //            }
            //        }
            //        else
            //        {
            //            //foreach (var datapoint in rawData.Value.Points)
            //            //{
            //            //    if (datapoint.Power_mW > peakPower)
            //            //    {
            //            //        peakPower = datapoint.Power_mW;
            //            //        peakAngle = datapoint.Angle_deg;
            //            //        peakCurrent = datapoint.Current_mA;
            //            //    }
            //            //    if (datapoint.Power_mW < nullPower)
            //            //    {
            //            //        nullPower = datapoint.Power_mW;
            //            //        nullAngle = datapoint.Angle_deg;
            //            //        nullCurrent = datapoint.Current_mA;
            //            //    }
            //            //}
            //        for (int i = 0; i < angles.Count; i++)
            //        {
            //            if (powers[i] > peakPower)
            //            {
            //                peakPower = powers[i];
            //                peakAngle = angles[i];
            //                peakCurrent = currents[i];
            //            }
            //            if (powers[i] < nullPower)
            //            {
            //                nullPower = powers[i];
            //                nullAngle = angles[i];
            //                nullCurrent = currents[i];
            //            }
            //        }
            //        //I1_PEA_mA = data.I1_PEA_mA = rawData.Value.IPea;
            //        //Temp_PER_C = data.Temp_PER_C = rawData.Value.Temp;
            //        data.NullA1_Deg = nullAngle;
            //            data.Power_NullA1_mW = nullPower;
            //            data.PEA1_Deg = peakAngle;
            //            data.Power_PEA1_mW = peakPower;
            //            if (UsePDCurrentCalculatePER)
            //            {
            //                data.PER1_dB = 10 * Math.Log10(peakCurrent / nullCurrent);
            //            }
            //            else
            //            {
            //                data.PER1_dB = 10 * Math.Log10(peakPower / nullPower);
            //            }

            //             data.PEA = peakAngle;
            //            //var datakey = dataName.Replace("raw", "cal");
            //            //DataList.Add(datakey, data);
            //            //PeaPerTestDataItem.Add(data);

            //        }
            //    //}
            //    return data.PEA;
            //}
            //catch (Exception ex)
            //{
            //    return 0;
            //    throw new Exception("CalculatotAllData_ReturnSingle:", ex);
            //    ////ExceptionManager.HandleException(ex);
            //    //throw (ex);
            //}
            #endregion

        }
        public class PeaPerTestDataItem
        {
            public double Temp_PER_C { get; set; }
            public double I1_PEA_mA { get; set; }
            public double PEA1_Deg { get; set; }
            public double Power_PEA1_mW { get; set; }
            public double NullA1_Deg { get; set; }
            public double Power_NullA1_mW { get; set; }
            public double PER1_dB { get; set; }
            public double PEA { get; set; }
        }
    }
}