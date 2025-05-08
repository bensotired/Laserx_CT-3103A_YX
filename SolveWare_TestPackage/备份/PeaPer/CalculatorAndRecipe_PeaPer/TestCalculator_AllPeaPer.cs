using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestPackage.PeaPer
{  public class PeaPerTestDataItem
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
    public static class PeaPerCalcuator 
    {     
        public static PeaPerTestDataItem CalculatotAllData_ReturnSingle(bool Curve_Fit, bool UsePDCurrentCalculatePER, List<double> currents, List<double> powers, List<double> angles)
        {     
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
                return data;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception("CalculatotAllData_ReturnSingle:", ex);
            }

        }
    }
}
