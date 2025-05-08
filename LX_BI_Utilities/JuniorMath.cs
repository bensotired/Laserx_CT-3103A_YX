using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LX_BurnInSolution.Utilities
{
    public static class JuniorMath
    {
        public static float GetStringValuesArrayAverage(string[] sourceArray)
        {
            if (sourceArray?.Count() <= 0) { return 0.0f; }
            double totalVal = 0.0;
            foreach (var val in sourceArray)
            {
                totalVal += Convert.ToDouble(val);
            }
            var avgVal = totalVal / sourceArray.Length;
            if (IsEdgeValue(avgVal))
            {
                return 0.0f;
            }
            else
            {
                return Convert.ToSingle(avgVal);
            }
        }

        public static bool IsEdgeValue(double val)
        {
            if (double.IsInfinity(val) ||
                        double.IsNaN(val) ||
                        val == double.MaxValue ||
                        val == double.MinValue)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// binary calculation, bitIndex starts form 0 ！！！！！！
        /// eg. decVal 4 -> bin 0100,  bitIndex = 2, it returns true,
        /// eg. devVal 3 -> bin 0011,  bitIndex = 2, it returns false, bitIndex =1 it returns true, bitIndex =0 it returns true, 
        /// </summary>
        /// <param name="decVal"></param>
        /// <param name="bitIndex"></param> 
        /// <returns></returns>
        public static bool IsBitEqualsOne(int decVal, int bitIndex)
        {
            return (decVal & (1 << (bitIndex))) != 0;
        }
        public static bool IsBitEqualsOne(uint decVal, int bitIndex)
        {
            return (decVal & (1 << (bitIndex))) != 0;
        }
        public static bool IsValueInLimitRange(double val, double lowerLimitVal, double upperLimitVal)
        {
            if (val <= lowerLimitVal || val >= upperLimitVal)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool IsValueInLimitRange(int val, int lowerLimitVal, int upperLimitVal)
        {
            if (val <= lowerLimitVal || val >= upperLimitVal)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool IsValueInRateRange(double actVal, double tarVal, double rate)
        {
            if (Math.Abs(actVal - tarVal) > (tarVal * rate))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool IsValueInRateRange(double actVal, double tarVal, double rate, double noiseOffset)
        {
            if (Math.Abs(actVal - tarVal) > ((tarVal * rate) + noiseOffset))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}