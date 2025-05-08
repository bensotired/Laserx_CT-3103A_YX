 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using SolveWare_BurnInInstruments;


namespace LX_BurnInSolution.Utilities
{
    /// <summary>
    /// Provides methods to calculate parameters for optical spectrum.
    /// </summary>
    public static class OpticalSpectrumMath
    {

        /// <summary>
        /// Calculates SMSR etc using a simple way.
        /// <para>Center Wavelength: the point with the max power.</para>
        /// <para>Side mode search way: the max power which the wavelength should be between (center WL - |wavelengthOffsetFromCenter2|) and (center WL - |wavelengthOffsetFromCenter1|)
        /// <para></para> or between (center WL + |wavelengthOffsetFromCenter1|) and (center WL + |wavelengthOffsetFromCenter2|).</para>
        /// </summary>
        /// <param name="trace">The optical spectrum trace.</param>
        /// <param name="wavelengthOffsetFromCenter1">|wavelengthOffsetFromCenter1| should be smaller than |wavelengthOffsetFromCenter2|</param>
        /// <param name="wavelengthOffsetFromCenter2">|wavelengthOffsetFromCenter1| should be smaller than |wavelengthOffsetFromCenter2|</param>
        /// <returns></returns>
        //public static SimpleSmsrCalculationData CalculateSimpleSmsr(PowerWavelengthTrace trace,
        //        double wavelengthOffsetFromCenter1, double wavelengthOffsetFromCenter2)
        //{
        //    if (trace.Count < 10)
        //    {
        //        throw new ArgumentException(string.Format("The length of trace ({0}) is too small.", trace.Count), "trace");
        //    }
        //    SimpleSmsrCalculationData data = new SimpleSmsrCalculationData();
        //    wavelengthOffsetFromCenter1 = Math.Abs(wavelengthOffsetFromCenter1);
        //    wavelengthOffsetFromCenter2 = Math.Abs(wavelengthOffsetFromCenter2);

        //    //1. find center wl
        //    double peakPower = double.MinValue;
        //    int peakIndex = 0;
        //    for (int i = 2; i < trace.Count - 2; i++)
        //    {
        //        PowerWavelength item = trace[i];
        //        if (item.Power_dBm > peakPower)
        //        {
        //            peakPower = item.Power_dBm;
        //            peakIndex = i;
        //        }
        //    }

        //    data.MaxPower = peakPower;
        //    data.CenterWavelength = trace[peakIndex].Wavelength_nm;

        //    int rightPeakIndex = 0;
        //    int index = peakIndex;
        //    double rightPeakPower = double.MinValue;
        //    //2. right SMSR
        //    while (true)
        //    {
        //        PowerWavelength item = trace[index];
        //        if (item.Wavelength_nm > data.CenterWavelength + wavelengthOffsetFromCenter2)// 3                 center   1  3
        //        {
        //            break;
        //        }

        //        if (item.Wavelength_nm >= data.CenterWavelength + wavelengthOffsetFromCenter1)//1    右边的波峰
        //        {
        //            if (rightPeakPower < item.Power_dBm)
        //            {
        //                rightPeakPower = item.Power_dBm;
        //                rightPeakIndex = index;
        //            }
        //        }

        //        index++;

        //        if (index > trace.Count - 1)
        //        {
        //            break;
        //        }

        //    }

        //    if (rightPeakPower == double.MinValue)
        //    {
        //        throw new Exception("Unable to find Right peak of the trace.");
        //    }
        //    data.RightSidePeakWavelength = trace[rightPeakIndex].Wavelength_nm;
        //    data.RightSideSmsr = data.MaxPower - trace[rightPeakIndex].Power_dBm;

        //    //3. left SMSR
        //    index = peakIndex;
        //    int leftPeakIndex = 0;
        //    double leftPeakPower = double.MinValue;
        //    while (true)
        //    {
        //        PowerWavelength item = trace[index];
        //        if (item.Wavelength_nm < data.CenterWavelength - wavelengthOffsetFromCenter2)
        //        {
        //            break;
        //        }

        //        if (item.Wavelength_nm <= data.CenterWavelength - wavelengthOffsetFromCenter1)
        //        {
        //            if (leftPeakPower < item.Power_dBm)
        //            {
        //                leftPeakPower = item.Power_dBm;
        //                leftPeakIndex = index;
        //            }
        //        }

        //        index--;

        //        if (index < 0)
        //        {
        //            break;
        //        }

        //    }

        //    if (leftPeakPower == double.MinValue)
        //    {
        //        throw new Exception("Unable to find Left peak of the trace.");
        //    }
        //    data.LeftSidePeakWavelength = trace[leftPeakIndex].Wavelength_nm;
        //    data.LeftSideSmsr = data.MaxPower - trace[leftPeakIndex].Power_dBm;

        //    data.Smsr = Math.Min(data.LeftSideSmsr, data.RightSideSmsr);

        //    return data;
        //}

        //public static double CalculateCenterWavelength(PowerWavelengthTrace trace, double powerDown_db,
        //double wavelengthOffsetFromCenter1, double wavelengthOffsetFromCenter2)
        //{
        //    if (trace.Count < 10)
        //    {
        //        throw new ArgumentException(string.Format("The length of trace ({0}) is too small.", trace.Count), "trace");
        //    }
        //    if (powerDown_db <= 0)
        //    {
        //        throw new ArgumentException("The powerDown_db must be bigger than 0.", "powerDown_db");
        //    }
        //    wavelengthOffsetFromCenter1 = Math.Abs(wavelengthOffsetFromCenter1);
        //    wavelengthOffsetFromCenter2 = Math.Abs(wavelengthOffsetFromCenter2);
        //    double[] Wavelengthes_nm = trace.GetWavelengthes();
        //    double[] Powers_dBm = trace.GetPowers();
        //    int indexS;
        //    int indexE;
        //    double[] powerArr;
        //    double[] wavelengthArr;

        //    for (indexS = 0; indexS < Wavelengthes_nm.Length; indexS++)
        //    {
        //        if (Wavelengthes_nm[indexS] >= wavelengthOffsetFromCenter1)
        //        {
        //            break;
        //        }
        //    }
        //    for (indexE = Wavelengthes_nm.Length - 1; indexE > 0; indexE--)
        //    {
        //        if (Wavelengthes_nm[indexE] <= wavelengthOffsetFromCenter2)
        //        {
        //            break;
        //        }
        //    }
        //    int Length = indexE - indexS + 1;
        //    wavelengthArr = new double[Length];
        //    powerArr = new double[Length];

        //    Array.Copy(Wavelengthes_nm, indexS, wavelengthArr, 0, Length);
        //    Array.Copy(Powers_dBm, indexS, powerArr, 0, Length);

        //    int peakIndex = 0;
        //    int minIndex;
        //    ArrayMath.GetMaxAndMinIndex(powerArr, out peakIndex, out minIndex);
        //    double MaxPower_dBm = powerArr[peakIndex];
        //    double[] leftPowerArr = new double[peakIndex + 1];
        //    double[] leftWavelengthArr = new double[peakIndex + 1];
        //    double[] rightPowerArr = new double[Length - peakIndex];
        //    double[] rightWavelengthArr = new double[Length - peakIndex];

        //    Array.Copy(powerArr, 0, leftPowerArr, 0, peakIndex + 1);
        //    Array.Copy(wavelengthArr, 0, leftWavelengthArr, 0, peakIndex + 1);
        //    Array.Copy(powerArr, peakIndex, rightPowerArr, 0, Length - peakIndex);
        //    Array.Copy(wavelengthArr, peakIndex, rightWavelengthArr, 0, Length - peakIndex);
        //    Array.Reverse(rightPowerArr);
        //    Array.Reverse(rightWavelengthArr);

        //    double LeftWavelength = Interpolator.DoPiecewiseLinearInterpolation(MaxPower_dBm - powerDown_db, leftPowerArr, leftWavelengthArr);
        //    double RightWavelength = Interpolator.DoPiecewiseLinearInterpolation(MaxPower_dBm - powerDown_db, rightPowerArr, rightWavelengthArr);

        //    return (LeftWavelength + RightWavelength) / 2;
        //}
    //}

    ///// <summary>
    ///// Contains calculation results for SMSR.
    ///// </summary>
    //public class SimpleSmsrCalculationData
    //{
    //    /// <summary>
    //    /// Initializes an instance.
    //    /// </summary>
    //    public SimpleSmsrCalculationData()
    //    {

    //    }

    //    /// <summary>
    //    /// Gets or sets Center wavelength.
    //    /// </summary>
    //    public double CenterWavelength { get; set; }

    //    /// <summary>
    //    /// Gets or sets the max power of the whole trace.
    //    /// </summary>
    //    public double MaxPower { get; set; }

    //    /// <summary>
    //    /// The wavelength range is between (center WL - |wavelengthOffsetFromCenter2|) and (center WL - |wavelengthOffsetFromCenter1|)
    //    /// </summary>
    //    public double LeftSidePeakWavelength { get; set; }

    //    /// <summary>
    //    /// The wavelength range is between (center WL - |wavelengthOffsetFromCenter2|) and (center WL - |wavelengthOffsetFromCenter1|)
    //    /// </summary>
    //    public double LeftSideSmsr { get; set; }

    //    /// <summary>
    //    /// The wavelength range is between (center WL + |wavelengthOffsetFromCenter1|) and (center WL + |wavelengthOffsetFromCenter2|)
    //    /// </summary>
    //    public double RightSidePeakWavelength { get; set; }
    //    /// <summary>
    //    /// The wavelength range is between (center WL + |wavelengthOffsetFromCenter1|) and (center WL + |wavelengthOffsetFromCenter2|)
    //    /// </summary>
    //    public double RightSideSmsr { get; set; }
    //    /// <summary>
    //    /// Gets or sets SMSR.
    //    /// </summary>
    //    public double Smsr { get; set; }
    }
}
