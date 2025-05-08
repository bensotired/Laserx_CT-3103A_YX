using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LX_BurnInSolution.Utilities
{
    public static class OpticalMath
    {
       




        /// <summary>
        /// Velocity of Light in m/second.
        /// </summary>
        public const double VelocityOfLight_mPerSec = 299792458;

        /// <summary>
        /// Converts wavelength in nm to frequency in GHz.
        /// </summary>
        /// <param name="wavelength_nm"></param>
        /// <returns>Returns frequency in GHz.</returns>
        public static double ConvertWavelengthToFrequency_GHz(double wavelength_nm)
        {
            return VelocityOfLight_mPerSec / wavelength_nm;
        }

        /// <summary>
        /// Converts frequency in GHz to wavelength in nm.
        /// </summary>
        /// <param name="frequency_GHz"></param>
        /// <returns>Returns wavelegth in nm.</returns>
        public static double ConvertFrequencyToWavelength_nm(double frequency_GHz)
        {
            return VelocityOfLight_mPerSec / frequency_GHz;
        }

        /// <summary>
        /// Converts power from dBm to W.
        /// </summary>
        /// <param name="dBm"></param>
        /// <returns></returns>
        public static double ConvertdBmToW(double dBm)
        {
            return Math.Pow(10, (dBm / 10)) / 1000.0;
        }

        /// <summary>
        /// Converts power from W to dBm.
        /// </summary>
        /// <param name="W"></param>
        /// <returns></returns>
        public static double ConvertWtodBm(double W)
        {
            return 10 * Math.Log10(W * 1000.0);
        }

        /// <summary>
        /// Converts power from dBm to mW.
        /// </summary>
        /// <param name="dBm"></param>
        /// <returns></returns>
        public static double ConvertdBmTomW(double dBm)
        {
            return Math.Pow(10, (dBm / 10));
        }

        /// <summary>
        /// Converts power from mw to dBm.
        /// </summary>
        /// <param name="mW"></param>
        /// <returns></returns>
        public static double ConvertmWtodBm(double mW)
        {
            return 10 * Math.Log10(mW);
        }


        /// <summary>
        /// 二阶导一次平滑移动平均在范围内求ITH
        /// </summary>
        /// <param name="currents">x数组  电流</param>
        /// <param name="powers">y数组  光功率</param>
        /// <param name="currentLowerLimit">计算ith的电流区间开始值</param>
        /// <param name="currentUpperLimit">计算ith的电流区间终止值</param>
        /// <param name="halfWindowPoints">移动平均半窗点数如 11点平均值- 那半窗点数应该为5</param>
        /// <returns></returns>
        public static double CalculateIth_2ndDerivedWithRange(double[] currents,double[]  powers,double currentLowerLimit, double currentUpperLimit,  int halfWindowPoints)
        {
            double ith = 0.0;
            if (currents?.Length != powers?.Length ||
                currentLowerLimit > currentUpperLimit ||
                halfWindowPoints < 1)
            {
                return ith;
            }
            try
            {
                const int nthDerived = 2;
                const int smthCount = 1;
                var dp_di_2ndDER = ArrayMath.CalculateSmoothedNthDerivate
                    (
                        currents,
                        powers,
                        nthDerived,
                        smthCount,
                        halfWindowPoints
                    );
                double dpdi2_max = double.MinValue;
                int dpdi2_max_index = 0;
                for (int i = 0; i < currents.Length; i++)
                {
                    if (i < halfWindowPoints * 2) continue;

                    if (JuniorMath.IsValueInLimitRange(currents[i], currentLowerLimit, currentUpperLimit))
                    {
                        if (dp_di_2ndDER[i] > dpdi2_max)
                        {
                            dpdi2_max = dp_di_2ndDER[i];
                            dpdi2_max_index = i;
                        }
                    }
                }
                ith = currents[dpdi2_max_index];
            }
            catch 
            {

            }
            return ith;
        }
    }


    //    public static double CalculateLinearity_dB(double[] x, double[] y)
    //    {
    //        if ((x == null) || (y == null))
    //            return 0;
    //        if (x.Length != y.Length)
    //            return 0;
    //        int length = x.Length;
    //        double slope = (y[length - 1] - y[0]) / (x[length - 1] - x[0]);
    //        double offset = (y[0] * x[length - 1] - y[length - 1] * x[0]) / (x[length - 1] - x[0]);
    //        double[] CalculatedY = new double[length];
    //        double[] AbsDelta = new double[length];
    //        double[] DeltaDivY_db = new double[length];
    //        for (int i = 0; i < length; i++)
    //        {
    //            CalculatedY[i] = slope * x[i] + offset;
    //            AbsDelta[i] = Math.Abs(CalculatedY[i] - y[i]);
    //            DeltaDivY_db[i] = 10 * Math.Log10(AbsDelta[i] / y[i]);
    //        }
    //        return DeltaDivY_db.Max();
    //    }

    //    public static double CalculateIthInteger(double[] Currents_mA, double[] Readings,
    //            int smoothingCount = 3, int movingAvergeHalfWindowSize = 5)
    //    {
    //        int indexS;
    //        int indexE;
    //        int maxIndex;
    //        int minIndex;
    //        double IthInteger_mA = 0;
    //        double maxReading;

    //        try
    //        {
    //            // Step 1
    //            ArrayMath.GetMaxAndMinIndex(Readings, out maxIndex, out minIndex);
    //            maxReading = Readings[maxIndex];
    //            if (Readings[maxIndex] >= Readings[minIndex])
    //            {
    //                // Step 2
    //                indexS = 0;
    //                for (indexE = 0; indexE < Readings.Length - 1; indexE++)
    //                {
    //                    if (Readings[indexE] >= maxReading)
    //                    {
    //                        break;
    //                    }
    //                }
    //                double[] readingArr = new double[indexE - indexS + 1];
    //                double[] currentArr = new double[indexE - indexS + 1];
    //                Array.Copy(Currents_mA, indexS, currentArr, 0, indexE - indexS + 1);
    //                Array.Copy(Readings, indexS, readingArr, 0, indexE - indexS + 1);

    //                //step 3
    //                double[] D2LD2I = ArrayMath.CalculateSmoothedNthDerivate(currentArr, readingArr, 2, smoothingCount, movingAvergeHalfWindowSize);
    //                double[] currentArrayD2LD2I = new double[currentArr.Length];
    //                Array.Copy(currentArr, 0, currentArrayD2LD2I, 0, currentArr.Length);

    //                // Step 4
    //                ArrayMath.GetMaxAndMinIndex(D2LD2I, out maxIndex, out minIndex);
    //                IthInteger_mA = currentArrayD2LD2I[maxIndex];
    //            }
    //        }
    //        catch
    //        {

    //        }
    //        return IthInteger_mA;
    //    }

    //    public static double CalculateIthDerived(double IthInteger_mA, double[] Currents_mA, double[] Readings,
    //            int minShift_mA = 1, int maxShift_mA = 10)
    //    {
    //        int indexS;
    //        int indexE;
    //        int info;
    //        double[] a;
    //        alglib.barycentricinterpolant p;
    //        alglib.polynomialfitreport rep;
    //        double IthDerived_mA = 0;
    //        try
    //        {
    //            for (indexS = 0; indexS < Currents_mA.Length; indexS++)
    //            {
    //                if (Currents_mA[indexS] >= IthInteger_mA + minShift_mA)
    //                {
    //                    break;
    //                }
    //            }
    //            for (indexE = Currents_mA.Length - 1; indexE > 0; indexE--)
    //            {
    //                if (Currents_mA[indexE] <= IthInteger_mA + maxShift_mA)
    //                {
    //                    break;
    //                }
    //            }
    //            double[] currentArr = new double[indexE - indexS + 1];
    //            double[] readingArr = new double[indexE - indexS + 1];

    //            Array.Copy(Currents_mA, indexS, currentArr, 0, indexE - indexS + 1);
    //            Array.Copy(Readings, indexS, readingArr, 0, indexE - indexS + 1);
    //            alglib.polynomialfit(currentArr, readingArr, 2, out info, out p, out rep);
    //            alglib.polynomialbar2pow(p, out a);
    //            IthDerived_mA = -a[0] / a[1];
    //        }
    //        catch
    //        {

    //        }
    //        return IthDerived_mA;
    //    }

    //    public static double CalculateIthIntegerByFirstDerivate(double[] Currents_mA, double[] Readings)
    //    {
    //        int indexS;
    //        int indexE;
    //        int maxIndex;
    //        int minIndex;
    //        double IthInteger_mA = 0;
    //        double maxReading;

    //        try
    //        {
    //            ArrayMath.GetMaxAndMinIndex(Readings, out maxIndex, out minIndex);
    //            maxReading = Readings[maxIndex];
    //            if (Readings[maxIndex] >= Readings[minIndex])
    //            {
    //                indexS = 0;
    //                for (indexE = 0; indexE < Readings.Length - 1; indexE++)
    //                {
    //                    if (Readings[indexE] >= maxReading)
    //                    {
    //                        break;
    //                    }
    //                }
    //                double[] powerArr = new double[indexE - indexS + 1];
    //                double[] currentArr = new double[indexE - indexS + 1];
    //                Array.Copy(Currents_mA, indexS, currentArr, 0, indexE - indexS + 1);
    //                Array.Copy(Readings, indexS, powerArr, 0, indexE - indexS + 1);

    //                double[] DLDI = ArrayMath.CalculateFirstDerivate(currentArr, powerArr);
    //                Array.Resize(ref DLDI, currentArr.Length - 1);
    //                double maxDL = DLDI.Max();
    //                int index;
    //                for (index = 0; index < DLDI.Length - 1; index++)
    //                {
    //                    if (DLDI[index] >= maxDL / 2)
    //                    {
    //                        break;
    //                    }
    //                }

    //                IthInteger_mA = currentArr[index];
    //            }
    //        }
    //        catch
    //        {

    //        }
    //        return IthInteger_mA;
    //    }

    //    public static double CalculateIthDerivedByPlot(double IthInteger_mA, double[] CurrentSets_mA, double[] Currents_mA, double[] Readings,
    //            int minShift_mA = 1, int maxShift_mA = 6)
    //    {
    //        int indexS;
    //        int indexE;
    //        int info;
    //        double[] a;
    //        alglib.barycentricinterpolant p;
    //        alglib.polynomialfitreport rep;
    //        double IthDerived_mA = 0;
    //        try
    //        {
    //            // plot 1
    //            for (indexS = 0; indexS < Currents_mA.Length; indexS++)
    //            {
    //                if (Currents_mA[indexS] >= IthInteger_mA + minShift_mA)
    //                {
    //                    break;
    //                }
    //            }
    //            for (indexE = Currents_mA.Length - 1; indexE > 0; indexE--)
    //            {
    //                if (Currents_mA[indexE] <= IthInteger_mA + maxShift_mA)
    //                {
    //                    break;
    //                }
    //            }
    //            double[] currentArr = new double[indexE - indexS + 1];
    //            double[] powerArr = new double[indexE - indexS + 1];

    //            Array.Copy(Currents_mA, indexS, currentArr, 0, indexE - indexS + 1);
    //            Array.Copy(Readings, indexS, powerArr, 0, indexE - indexS + 1);
    //            alglib.polynomialfit(currentArr, powerArr, 2, out info, out p, out rep);
    //            alglib.polynomialbar2pow(p, out a);
    //            double a1 = a[1];
    //            double b1 = a[0];

    //            // plot 2 
    //            for (indexS = 0; indexS < Currents_mA.Length; indexS++)
    //            {
    //                if (Currents_mA[indexS] >= IthInteger_mA - 8)
    //                {
    //                    break;
    //                }
    //            }
    //            for (indexE = Currents_mA.Length - 1; indexE > 0; indexE--)
    //            {
    //                if (Currents_mA[indexE] <= IthInteger_mA - 3)
    //                {
    //                    break;
    //                }
    //            }
    //            currentArr = new double[indexE - indexS + 1];
    //            powerArr = new double[indexE - indexS + 1];

    //            Array.Copy(Currents_mA, indexS, currentArr, 0, indexE - indexS + 1);
    //            Array.Copy(Readings, indexS, powerArr, 0, indexE - indexS + 1);
    //            alglib.polynomialfit(currentArr, powerArr, 2, out info, out p, out rep);
    //            alglib.polynomialbar2pow(p, out a);
    //            double a2 = a[1];
    //            double b2 = a[0];

    //            IthDerived_mA = (b2 - b1) / (a1 - a2);
    //            if (IthDerived_mA > CurrentSets_mA.Max())
    //            {
    //                IthDerived_mA = CurrentSets_mA.Max();
    //            }
    //        }
    //        catch
    //        {

    //        }
    //        return IthDerived_mA;
    //    }

    //    public static double CalculateRolloverCurrent(double[] CurrentSets_mA, double[] Currents_mA, double[] Powers_mW, double[] EACurrents_mA, out double Imod_RV_mA,
    //            double minCurrent_mA = 40, double maxCurrent_mA = 89)
    //    {
    //        int indexS = 0;
    //        int indexE = 0;
    //        double RolloverCurrent_mA = 0;
    //        Imod_RV_mA = 0;

    //        try
    //        {
    //            for (indexS = 0; indexS < CurrentSets_mA.Length; indexS++)
    //            {
    //                if (CurrentSets_mA[indexS] >= minCurrent_mA)
    //                {
    //                    break;
    //                }
    //            }
    //            for (indexE = indexS + 1; indexE < CurrentSets_mA.Length; indexE++)
    //            {
    //                if (CurrentSets_mA[indexE] > maxCurrent_mA)
    //                {
    //                    break;
    //                }
    //            }
    //            if (indexE >= CurrentSets_mA.Length)
    //            {
    //                indexE = CurrentSets_mA.Length - 1;
    //            }
    //            double[] powerArr = new double[indexE - indexS + 1];
    //            double[] currentArr = new double[indexE - indexS + 1];
    //            Array.Copy(CurrentSets_mA, indexS, currentArr, 0, indexE - indexS + 1);
    //            Array.Copy(Powers_mW, indexS, powerArr, 0, indexE - indexS + 1);
    //            double[] DLDIRollover = ArrayMath.CalculateFirstDerivate(currentArr, powerArr);

    //            RolloverCurrent_mA = CurrentSets_mA.Max();
    //            for (int i = 0; i < DLDIRollover.Length - 2; i++)
    //            {
    //                if (Math.Sign(DLDIRollover[i]) != Math.Sign(DLDIRollover[i + 1]))
    //                {
    //                    RolloverCurrent_mA = currentArr[i];
    //                    break;
    //                }
    //            }
    //            Imod_RV_mA = Interpolator.DoPiecewiseLinearInterpolation(RolloverCurrent_mA, Currents_mA, EACurrents_mA);
    //        }
    //        catch
    //        {
    //        }

    //        return RolloverCurrent_mA;
    //    }

    //    public static double CalculateKinkCurrent(double[] CurrentSets_mA, double[] Currents_mA, double[] EACurrents_mA, double IthInteger_mA, out double Imod_Kink_CH,
    //            int KinkCurrentOffset_mA = 4, double KinkCurrentCalculationParameter = 0.4)
    //    {
    //        int indexS = 0;
    //        int indexE = 0;
    //        int info;
    //        double[] a;
    //        alglib.barycentricinterpolant p;
    //        alglib.polynomialfitreport rep;
    //        double KinkCurrent_mA = 0;
    //        Imod_Kink_CH = 0;

    //        try
    //        {
    //            double pointStart = IthInteger_mA + KinkCurrentOffset_mA;
    //            for (indexS = 0; indexS < CurrentSets_mA.Length; indexS++)
    //            {
    //                if (CurrentSets_mA[indexS] >= pointStart)
    //                {
    //                    break;
    //                }
    //            }
    //            for (indexE = indexS + 1; indexE < CurrentSets_mA.Length; indexE++)
    //            {
    //                if (CurrentSets_mA[indexE] <= CurrentSets_mA[indexE - 1])
    //                {
    //                    break;
    //                }
    //            }
    //            if (indexE >= CurrentSets_mA.Length)
    //            {
    //                indexE = CurrentSets_mA.Length - 1;
    //            }
    //            double[] eaCurrentArr = new double[indexE - indexS + 1];
    //            double[] currentArr = new double[indexE - indexS + 1];
    //            Array.Copy(CurrentSets_mA, indexS, currentArr, 0, indexE - indexS + 1);
    //            Array.Copy(EACurrents_mA, indexS, eaCurrentArr, 0, indexE - indexS + 1);
    //            double[] currentsRawForFit = new double[indexE - indexS];
    //            double[] DLDIForFit = new double[indexE - indexS];
    //            Array.Copy(CurrentSets_mA, indexS + 1, currentsRawForFit, 0, indexE - indexS);
    //            Array.Copy(ArrayMath.CalculateFirstDerivate(currentArr, eaCurrentArr), 0, DLDIForFit, 0, indexE - indexS);

    //            alglib.polynomialfit(currentsRawForFit, DLDIForFit, 3, out info, out p, out rep);
    //            alglib.polynomialbar2pow(p, out a);

    //            KinkCurrent_mA = CurrentSets_mA.Max();
    //            for (int i = 0; i < DLDIForFit.Length - 1; i++)
    //            {
    //                double fitDLDI = a[2] * currentsRawForFit[i] * currentsRawForFit[i] + a[1] * currentsRawForFit[i] + a[0];
    //                //double fitDLDI = slope * current[i] + intercept;
    //                if (Math.Abs((DLDIForFit[i] - fitDLDI) / fitDLDI) > KinkCurrentCalculationParameter)
    //                {
    //                    KinkCurrent_mA = currentsRawForFit[i];
    //                    Imod_Kink_CH = Interpolator.DoPiecewiseLinearInterpolation(KinkCurrent_mA, Currents_mA, EACurrents_mA);
    //                    break;
    //                }
    //            }
    //        }
    //        catch
    //        {

    //        }
    //        return KinkCurrent_mA;
    //    }
    //}
}
