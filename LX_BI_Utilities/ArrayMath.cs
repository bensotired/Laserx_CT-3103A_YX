using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LX_BurnInSolution.Utilities
{
    /// <summary>
    /// Math for array calculation.
    /// </summary>
    public static class ArrayMath
    {
        /// <summary>
        /// Gets indexes of max and min value in the specific array.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="maxIndex"></param>
        /// <param name="minIndex"></param>
        public static void GetMaxAndMinIndex(double[] values, out int maxIndex, out int minIndex)
        {
            GetMaxAndMinIndexBeforeSpecificIndex(values, values.Length - 1, out maxIndex, out minIndex);
        }

        /// <summary>
        /// Gets indexes of max and min value in the sub array(between the specific start and stop indexes) of the specific array.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="startIndex"></param>
        /// <param name="stopIndex"></param>
        /// <param name="maxIndex"></param>
        /// <param name="minIndex"></param>
        public static void GetMaxAndMinIndex(double[] values, int startIndex, int stopIndex, out int maxIndex, out int minIndex)
        {
            if (stopIndex >= values.Length || stopIndex < 0)
            {
                throw new ArgumentOutOfRangeException("index", stopIndex, "Index is out of the values range.");
            }

            if (startIndex >= values.Length || startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("index", startIndex, "Index is out of the values range.");
            }

            maxIndex = 0;
            minIndex = 0;
            double maxValue = double.MinValue;
            double minValue = double.MaxValue;
            for (int i = startIndex; i <= stopIndex; i++)
            {
                double value = values[i];
                if (value > maxValue)
                {
                    maxValue = value;
                    maxIndex = i;
                }
                if (value < minValue)
                {
                    minValue = value;
                    minIndex = i;
                }
            }
        }

        /// <summary>
        /// Gets indexes of max and min value in the sub array(between 0 and the specific stop index) of the specific array.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="stopIndex"></param>
        /// <param name="maxIndex"></param>
        /// <param name="minIndex"></param>
        public static void GetMaxAndMinIndexBeforeSpecificIndex(double[] values, int stopIndex, out int maxIndex, out int minIndex)
        {
            if (stopIndex >= values.Length || stopIndex < 0)
            {
                throw new ArgumentOutOfRangeException("index", stopIndex, "Index is out of the values range.");
            }
            maxIndex = 0;
            minIndex = 0;
            double maxValue = double.MinValue;
            double minValue = double.MaxValue;
            for (int i = 0; i <= stopIndex; i++)
            {
                double value = values[i];
                if (value > maxValue)
                {
                    maxValue = value;
                    maxIndex = i;
                }
                if (value < minValue)
                {
                    minValue = value;
                    minIndex = i;
                }
            }
        }

        /// <summary>
        /// Gets indexes of max and min value in the sub array(between the specific start index and the last index) of the specific array.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="startIndex"></param>
        /// <param name="maxIndex"></param>
        /// <param name="minIndex"></param>
        public static void GetMaxAndMinIndexAfterSpecificIndex(double[] values, int startIndex, out int maxIndex, out int minIndex)
        {
            if (startIndex >= values.Length || startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("index", startIndex, "Index is out of the values range.");
            }
            maxIndex = 0;
            minIndex = 0;
            double maxValue = double.MinValue;
            double minValue = double.MaxValue;
            for (int i = startIndex; i < values.Length; i++)
            {
                double value = values[i];
                if (value > maxValue)
                {
                    maxValue = value;
                    maxIndex = i;
                }
                if (value < minValue)
                {
                    minValue = value;
                    minIndex = i;
                }
            }
        }
        

        /// <summary>
        /// Calculates values according to start/stop and step value.
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="stopValue"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static double[] CalculateArray(double startValue, double stopValue, double step)
        {
            return CalculateArray(startValue, stopValue, step, 5);
        }

        /// <summary>
        /// Calculates values according to start/stop and step value.
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="stopValue"></param>
        /// <param name="step"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static double[] CalculateArray(double startValue, double stopValue, double step, int digits)
        {
            List<double> list = new List<double>();
            int count = 0;
            double value = 0;
            while (true)
            {
                value = (startValue + count * step).ScientificNotationRound(digits);
                if (value * step >= stopValue * step)
                {
                    list.Add(stopValue);
                    break;
                }

                list.Add(value);

                count++;
            }
            return list.ToArray();
        }
        /// <summary>
        /// Calculates values according to start/stop and count value.
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="stopValue"></param>
        /// <param name="count"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static double[] CalculateArray(double startValue, double stopValue, int count, int digits)
        {
            double step = (stopValue - startValue) / (count - 1);
            double[] values = new double[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = startValue + i * step;
                if (digits > 0)
                {
                    values[i] = values[i].ScientificNotationRound(digits);
                }
            }

            return values;
        }
        /// <summary>
        /// Calculates values according to start/stop and count value.
        /// <para>The square root differences between two neighbor points are same.</para>
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="stopValue"></param>
        /// <param name="count"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static double[] CalculateArrayInSQRT(double startValue, double stopValue, int count, int digits)
        {
            if (startValue < 0 || stopValue < 0)
            {
                throw new ArgumentOutOfRangeException("startValue and stopValue should not be smaller than 0.");
            }

            double sqrtStart = Math.Sqrt(startValue);
            double sqrtStop = Math.Sqrt(stopValue);

            double step = (sqrtStop - sqrtStart) / (count - 1);
            double[] values = new double[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = Math.Pow(sqrtStart + i * step, 2);
                if (digits > 0)
                {
                    values[i] = values[i].ScientificNotationRound(digits);
                }
            }

            return values;
        }
        /// <summary>
        /// Calculates values according to start/stop and count value.
        /// <para>The square differences between two neighbor points are same.</para>
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="stopValue"></param>
        /// <param name="count"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static double[] CalculateArrayInSquare(double startValue, double stopValue, int count, int digits)
        {

            double squareStart = startValue * startValue;
            double squareStop = stopValue * stopValue;

            double step = (Math.Sign(stopValue) * squareStop - Math.Sign(startValue) * squareStart) / (count - 1);
            double[] values = new double[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = squareStart + i * step;
                values[i] = Math.Sign(values[i]) * Math.Sqrt(Math.Abs(values[i]));
                if (digits > 0)
                {
                    values[i] = values[i].ScientificNotationRound(digits);
                }
            }

            return values;
        }

        public static double[] CreateArrayUsingFixedValue(double value, int count)
        {
            double[] values = new double[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = value;
            }
            return values;
        }

        /// <summary>
        /// Calculates the second derivatives for X-Y array and gets the index of the max second derivative.
        /// </summary>
        /// <param name="xArray"></param>
        /// <param name="yArray"></param>
        /// <returns></returns>
        public static int GetMaxIndexOf2ndDerivative(double[] xArray, double[] yArray)
        {
            int maxIndex, minIndex;

            double[] firstDerivateArray = CalculateFirstDerivate(xArray, yArray);
            double[] secondDerivateArray = CalculateFirstDerivate(xArray, firstDerivateArray);
            GetMaxAndMinIndex(secondDerivateArray, out maxIndex, out minIndex);


            return maxIndex;
        }

        /// <summary>
        /// Caculates the second derivatives for X-Y array and gets the index of the max/min second derivative.
        /// </summary>
        /// <param name="xArray"></param>
        /// <param name="yArray"></param>
        /// <param name="maxIndex">The index of the max second derivative.</param>
        /// <param name="minIndex">The index of the min second derivative.</param>
        /// <returns></returns>
        public static void GetMaxMinIndexOf2ndDerivative(double[] xArray, double[] yArray, out int maxIndex, out int minIndex)
        {
            double[] firstDerivateArray = CalculateFirstDerivate(xArray, yArray);
            double[] secondDerivateArray = CalculateFirstDerivate(xArray, firstDerivateArray);
            GetMaxAndMinIndex(secondDerivateArray, out maxIndex, out minIndex);
        }

        /// <summary>
        /// Calculates the first derivates(dy/dx) of X-Y array.
        /// </summary>
        /// <param name="xArray"></param>
        /// <param name="yArray"></param>
        /// <returns>the first derivates(dy/dx).</returns>
        public static double[] CalculateFirstDerivate(double[] xArray, double[] yArray)
        {
            if (xArray.Length != yArray.Length)
            {
                throw new ArgumentException("X and Y array should be the same size.");
            }

            List<double> firstDerivateList = new List<double>();
            if (xArray.Length >= 2)
            {
                firstDerivateList.Add((yArray[1] - yArray[0]) / (xArray[1] - xArray[0]));

                for (int i = 1; i < xArray.Length - 1; i++)
                {
                    double firstDerivate = (yArray[i + 1] - yArray[i - 1]) / (xArray[i + 1] - xArray[i - 1]);
                    firstDerivateList.Add(firstDerivate);
                }

                firstDerivateList.Add((yArray[yArray.Length - 1] - yArray[yArray.Length - 2]) / (xArray[xArray.Length - 1] - xArray[xArray.Length - 2]));

            }
            else
            {
                firstDerivateList.Add(0);
            }
            return firstDerivateList.ToArray();
        }
        /// <summary>
        /// Calculates the first derivates(dy/dx) of X-Y array with n and n+1 points.
        /// </summary>
        /// <param name="xArray"></param>
        /// <param name="yArray"></param>
        /// <returns>the first derivates(dy/dx).</returns>
        public static double[] CalculateFirstDerivateWithNandNPlus1(double[] xArray, double[] yArray)
        {
            if (xArray.Length != yArray.Length)
            {
                throw new ArgumentException("X and Y array should be the same size.");
            }
            double[] derivates = new double[xArray.Length];

            for (int i = 0; i < derivates.Length - 1; i++)
            {
                derivates[i] = (yArray[i + 1] - yArray[i]) / (xArray[i + 1] - xArray[i]);
            }

            derivates[derivates.Length - 1] = double.NaN;

            return derivates;
        }
        /// <summary>
        /// Calculates the first derivates(dy/dx) of X-Y array with same points.
        /// </summary>
        /// <param name="xArray"></param>
        /// <param name="yArray"></param>
        /// <returns>the first derivates(dy/dx).</returns>
        public static double[] CalculateFirstDerivateWithSameSize(double[] xArray, double[] yArray)
        {
            if (xArray.Length != yArray.Length)
            {
                throw new ArgumentException("X and Y array should be the same size.");
            }
            double[] derivates = new double[xArray.Length];
            double[] derivateOutputs = new double[xArray.Length];

            for (int i = 0; i < derivates.Length - 1; i++)
            {
                derivates[i] = (yArray[i + 1] - yArray[i]) / (xArray[i + 1] - xArray[i]);
            }
            derivateOutputs[0] = derivates[0];
            for (int i = 1; i < derivates.Length - 1; i++)
            {
                derivateOutputs[i] = (derivates[i - 1] + derivates[i]) / 2;
            }
            derivateOutputs[derivates.Length - 1] = derivates[derivates.Length - 2];

            return derivateOutputs;
        }
        /// <summary>
        /// Gets peak indexes from an array.
        /// </summary>
        /// <param name="values">The values for the array.</param>
        /// <param name="halfWindow">A peak should be larger than any value in the range between peakIndex - halfWindow and peakIndex + halfWindow.</param>
        /// <param name="minDifference">The difference between a peak and a min value( in the window around the peak) should be larger than minDifference.</param>
        /// <param name="needPeakHigherThanAverage">If it's true, the peak should be higher than average value.</param>
        /// <param name="excludingEdge">If it's true, 'peak' near edges will be excluded.</param>
        /// <param name="minDifferenceInPercent">If minDifferenceInPercent, minDifference will be re-calculated using the formula: minDifference= (global Max-global Min) * minDifference / 100.0</param>
        /// <returns>The peak indexes.</returns>
        public static int[] GetPeakIndexes(double[] values, int halfWindow, double minDifference, bool needPeakHigherThanAverage, bool excludingEdge, bool minDifferenceInPercent)
        {
            List<int> peakIndexes = new List<int>();
            double average = values.Average();
            if (minDifferenceInPercent)
            {
                double globalMax = values.Max();
                double globalMin = values.Min();
                minDifference = minDifference * (globalMax - globalMin) / 100.0;
            }

            for (int peakIndex = 0; peakIndex < values.Length; peakIndex++)
            {
                double min = double.MaxValue;
                double leftMin = double.MaxValue;
                double rightMin = double.MaxValue;
                bool found = true;

                if (needPeakHigherThanAverage && values[peakIndex] < average)
                {
                    continue;
                }

                for (int aroundPointIndexOffset = 0; aroundPointIndexOffset < 2 * halfWindow + 1; aroundPointIndexOffset++)
                {
                    int aroundPointIndex = aroundPointIndexOffset - halfWindow + peakIndex;
                    if (aroundPointIndex < 0 || aroundPointIndex >= values.Length)
                    {
                        if (excludingEdge)
                        {
                            found = false;
                            break;
                        }
                        continue;
                    }
                    if (values[aroundPointIndex] > values[peakIndex])
                    {
                        found = false;
                        break;
                    }
                    if (values[aroundPointIndex] < min)
                    {
                        min = values[aroundPointIndex];
                    }
                    if (aroundPointIndex < peakIndex && values[aroundPointIndex] < leftMin)
                    {
                        leftMin = values[aroundPointIndex];
                    }
                    if (aroundPointIndex > peakIndex && values[aroundPointIndex] < rightMin)
                    {
                        rightMin = values[aroundPointIndex];
                    }
                }

                if (found)
                {
                    double leftMinDifference = minDifference;
                    if (peakIndex - halfWindow < 0)
                    {
                        leftMinDifference = minDifference * peakIndex / halfWindow;
                    }
                    double rightMinDifference = minDifference;
                    if (peakIndex + halfWindow > values.Length - 1)
                    {
                        rightMinDifference = minDifference * (values.Length - 1 - peakIndex) / halfWindow;
                    }

                    if (Math.Abs(min - values[peakIndex]) >= minDifference && Math.Abs(leftMin - values[peakIndex]) > leftMinDifference
                                && Math.Abs(rightMin - values[peakIndex]) >rightMinDifference)
                    {
                        if (peakIndexes.Count == 0 || peakIndex - 1 != peakIndexes[peakIndexes.Count - 1])//remove one peak which is near another.
                        {
                            peakIndexes.Add(peakIndex);
                        }
                    }
                }
            }
            return peakIndexes.ToArray();
        }

        /// <summary>
        /// Gets valley indexes from an array.
        /// </summary>
        /// <param name="values">The values for the array.</param>
        /// <param name="halfWindow">A valley should be lower than any value in the range between valleyIndex - halfWindow and valleyIndex + halfWindow.</param>
        /// <param name="minDifference">The difference between a valley and a max value( in the window around the vally) should be larger than minDifference.</param>
        /// <param name="needPeakLowerThanAverage">If it's true, the valley should be lower than the average value.</param>
        /// <param name="excludingEdge">If it's true, 'valley' near edges will be excluded.</param>
        /// <param name="minDifferenceInPercent">If minDifferenceInPercent, minDifference will be re-calculated using the formula: minDifference= (global Max-global Min) * minDifference / 100.0</param>
        /// <returns>The valley indexes.</returns>
        public static int[] GetValleyIndexes(double[] values, int halfWindow, double minDifference, bool needPeakLowerThanAverage, bool excludingEdge, bool minDifferenceInPercent)
        {
            List<int> valleyIndexes = new List<int>();
            double average = values.Average();
            if (minDifferenceInPercent)
            {
                double globalMax = values.Max();
                double globalMin = values.Min();
                minDifference = minDifference * (globalMax - globalMin) / 100.0;
            }

            for (int valleyIndex = 0; valleyIndex < values.Length; valleyIndex++)
            {
                double max = double.MinValue;
                double leftMax = double.MinValue;
                double rightMax = double.MinValue;
                bool found = true;

                if (needPeakLowerThanAverage && values[valleyIndex] > average)
                {
                    continue;
                }

                for (int aroundPointIndexOffset = 0; aroundPointIndexOffset < 2 * halfWindow + 1; aroundPointIndexOffset++)
                {
                    int aroundPointIndex = aroundPointIndexOffset - halfWindow + valleyIndex;
                    if (aroundPointIndex < 0 || aroundPointIndex >= values.Length)
                    {
                        if (excludingEdge)
                        {
                            found = false;
                            break;
                        }
                        continue;
                    }
                    if (values[aroundPointIndex] < values[valleyIndex])
                    {
                        found = false;
                        break;
                    }
                    if (values[aroundPointIndex] > max)
                    {
                        max = values[aroundPointIndex];
                    }
                    if (aroundPointIndex < valleyIndex && values[aroundPointIndex] > leftMax)
                    {
                        leftMax = values[aroundPointIndex];
                    }
                    if (aroundPointIndex > valleyIndex && values[aroundPointIndex] > rightMax)
                    {
                        rightMax = values[aroundPointIndex];
                    }
                }

                if (found)
                {
                    double leftMinDifference = minDifference;
                    if (valleyIndex - halfWindow < 0)
                    {
                        leftMinDifference = minDifference * valleyIndex / halfWindow;
                    }
                    double rightMinDifference = minDifference;
                    if (valleyIndex + halfWindow > values.Length - 1)
                    {
                        rightMinDifference = minDifference * (values.Length - 1 - valleyIndex) / halfWindow;
                    }
                    if (Math.Abs(max - values[valleyIndex]) >= minDifference && Math.Abs(leftMax - values[valleyIndex]) > leftMinDifference
                                && Math.Abs(rightMax - values[valleyIndex]) > rightMinDifference)
                    {
                        if (valleyIndexes.Count == 0 || valleyIndex - 1 != valleyIndexes[valleyIndexes.Count - 1])//remove one valley which is near another.
                        {
                            valleyIndexes.Add(valleyIndex);
                        }
                    }
                }
            }
            return valleyIndexes.ToArray();
        }

        /// <summary>
        /// Gets peak indexes from an array.
        /// </summary>
        /// <param name="values">The values for the array.</param>
        /// <param name="halfWindow">A peak should be larger than any value in the range between peakIndex - halfWindow and peakIndex + halfWindow.</param>
        /// <param name="minDifference">The difference between a peak and a min value( in the window around the peak) should be larger than minDifference.</param>
        /// <param name="needPeakHigherThanAverage">If it's true, the peak should be higher than average value.</param>
        /// <returns>The peak indexes.</returns>
        public static int[] GetPeakIndexes(double[] values, int halfWindow, double minDifference, bool needPeakHigherThanAverage)
        {
            return GetPeakIndexes(values, halfWindow, minDifference, needPeakHigherThanAverage, false, false);
        }

        /// <summary>
        /// Calculates moving average array.
        /// <para>Usually this function is used to remove noise of an array.</para>
        /// </summary>
        /// <param name="values">The values to be calculated.</param>
        /// <param name="halfWindowSize">The half window size for averaging.</param>
        /// <returns>Returns moving averages array.</returns>
        public static double[] CalculateMovingAverage(double[] values, int halfWindowSize)
        {
            if (halfWindowSize < 1 || halfWindowSize >= values.Length / 2)
            {
                return values; //保持不变
                //throw new ArgumentOutOfRangeException("halfWindowSize", halfWindowSize, string.Format("halfWindowSize(={0}) should is too small or too big.", halfWindowSize));
            }
            double[] averages = new double[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                int count = 0;
                double total = 0.0;
                for (int j = -halfWindowSize; j <= halfWindowSize; j++)
                {
                    int index = i + j;
                    if (index < 0 || index >= values.Length)
                    {
                        continue;
                    }

                    count++;
                    total += values[index];
                }

                averages[i] = total / (double)count;
            }

            return averages;
        }

        /// <summary>
        /// Calculates moving average array.
        /// <para>Usually this function is used to remove noise of an array.</para>
        /// </summary>
        /// <param name="values">The values to be calculated.</param>
        /// <param name="halfWindowSize">The half window size for averaging.</param>
        /// <param name="loopCount">how many time to average values.</param>
        /// <returns>Returns moving averages array.</returns>
        public static double[] CalculateMovingAverage(double[] values, int halfWindowSize, int loopCount)
        {
            double[] outputArray = new double[values.Length];
            for (int i = 0; i < loopCount; i++)
            {
                outputArray = CalculateMovingAverage(values, halfWindowSize);
            }

            return outputArray;
        }

        public static double[] CalculateSmoothedNthDerivate(double[] xArray, double[] yArray, int numberOfNth, int smoothingCount, int movingAvergeHalfWindowSize)
        {
            double[] outputYArray = yArray;

            for (int i = 0; i < numberOfNth; i++)
            {
                for (int j = 0; j < smoothingCount; j++)
                {
                    outputYArray = CalculateMovingAverage(outputYArray, movingAvergeHalfWindowSize);
                }

                outputYArray = CalculateFirstDerivate(xArray, outputYArray);
            }
            return outputYArray;
        }


        /// <summary>
        /// Converts byte array to string.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string ByteArray2String(byte[] buffer)
        {
            string stringBuffer;
            int i = 0;
            stringBuffer = "";
            while ((buffer[i] > 0) && (i < buffer.Length))
            {
                stringBuffer = stringBuffer + Convert.ToChar(buffer[i]);
                i++;
            }
            return stringBuffer;
        }
        /// <summary>
        /// Converts byte array to string.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ByteArray2String(byte[] buffer, uint length)
        {
            string stringBuffer;
            int i = 0;
            stringBuffer = "";
            for (i = 0; i < length; i++)
            {
                stringBuffer = stringBuffer + Convert.ToChar(buffer[i]);
            }
            return stringBuffer;
        }

        /// <summary>
        /// Add a value to each element of the array.
        /// </summary>
        /// <param name="arrayIn">An array of doubles</param>
        /// <param name="valueToAdd">A value to be added</param>
        /// <returns>A new array in which 'valueToAdd' has been added to each element</returns>
        public static double[] AddToEachArrayElement(double[] arrayIn, double valueToAdd)
        {
            if (arrayIn == null)
            {
                throw new ArgumentException("AddToEachArrayElement : arrayIn is null");
            }
            double[] resultArray = new double[arrayIn.Length];

            for (int i = 0; i < arrayIn.Length; i++)
            {
                resultArray[i] = arrayIn[i] + valueToAdd;
            }
            return resultArray;
        }

        /// <summary>
        /// Subtract a value from each element of the array.
        /// </summary>
        /// <param name="arrayIn">An array of doubles</param>
        /// <param name="valueToSubtract">A value to be subtracted</param>
        /// <returns>A new array in which 'valueToSubtract' has been subtracted from each element</returns>
        public static double[] SubtractFromEachArrayElement(double[] arrayIn, double valueToSubtract)
        {
            if (arrayIn == null)
            {
                throw new ArgumentException("SubtractFromEachArrayElement : arrayIn is null");
            }
            return AddToEachArrayElement(arrayIn, -valueToSubtract);
        }

        /// <summary>
        /// Multiply each element of the array by a value.
        /// </summary>
        /// <param name="arrayIn">An array of doubles</param>
        /// <param name="valueToMultiplyBy">A value to be multiply by</param>
        /// <returns>A new array in which each element is the product of the original element and 'valueToMultiplyBy'</returns>
        public static double[] MultiplyEachArrayElement(double[] arrayIn, double valueToMultiplyBy)
        {
            if (arrayIn == null)
            {
                throw new ArgumentException("MultiplyEachArrayElement : arrayIn is null");
            }

            double[] resultArray = new double[arrayIn.Length];

            for (int i = 0; i < arrayIn.Length; i++)
            {
                resultArray[i] = arrayIn[i] * valueToMultiplyBy;
            }
            return resultArray;
        }

        /// <summary>
        /// Divide each element of the array by a value.
        /// </summary>
        /// <param name="arrayIn">An array of doubles</param>
        /// <param name="valueToDivideBy">A non-zero value to divide by</param>
        /// <returns>A new array in which each element is the dividend of the original element and the divisor 'valueToDivideBy'</returns>
        public static double[] DivideEachArrayElement(double[] arrayIn, double valueToDivideBy)
        {
            // PRECONDITIONS
            if (arrayIn == null)
            {
                throw new ArgumentException("DivideEachArrayElement : arrayIn is null");
            }
            if (valueToDivideBy == 0)
            {
                throw new ArgumentException("Alg_ArrayFunctions.divideEachArrayElement : Cannot divide by 0");
            }

            double[] resultArray = new double[arrayIn.Length];

            for (int i = 0; i < arrayIn.Length; i++)
            {
                resultArray[i] = arrayIn[i] / valueToDivideBy;
            }
            return resultArray;
        }

        /// <summary>
        /// Reverses the sequence of elements of an array without altering the original data
        /// </summary>
        /// <param name="arrayIn">Input Array</param>
        /// <returns>A copy of the original array with all elements in reverse order</returns>
        public static double[] ReverseArray(double[] arrayIn)
        {
            if (arrayIn == null)
            {
                throw new ArgumentException("ReverseArray : arrayIn is null");
            }
            double[] returnArray = (double[])arrayIn.Clone();
            Array.Reverse(returnArray);
            return returnArray;
        }

        /// <summary>
        /// Sums each element of a pair of arrays of matching size
        /// </summary>
        /// <param name="array1">First array</param>
        /// <param name="array2">Second array</param>
        /// <returns>A new array where each element of the sum of the corresponding elements of the two input arrays</returns>
        public static double[] AddArrays(double[] array1, double[] array2)
        {
            // PRECONDITIONS
            if (array1 == null || array2 == null)
            {
                throw new ArgumentException("ArrayMath.addArrays : neither array may be null");
            }
            if (array1.Length != array2.Length)
            {
                throw new ArgumentException("ArrayMath.addArrays : The arrays must be of the same size. Array 1 has " + array1.Length + " elements and Array 2 has " + array2.Length + " elements");
            }

            double[] resultArray = new double[array1.Length];

            for (int i = 0; i < array1.Length; i++)
            {
                resultArray[i] = array1[i] + array2[i];
            }
            return resultArray;
        }

        /// <summary>
        /// Subtracts each element of the second array from the matching element of the first array
        /// </summary>
        /// <param name="array1">First array</param>
        /// <param name="array2">Array of values to be subtracted</param>
        /// <returns>A new array where each element of the difference between the corresponding elements of the two input arrays</returns>
        public static double[] SubtractArrays(double[] array1, double[] array2)
        {
            // PRECONDITIONS
            if (array1 == null || array2 == null)
            {
                throw new ArgumentException("ArrayMath.JoinArrays : Neither array may be null");
            }
            if (array1.Length != array2.Length)
            {
                throw new ArgumentException("ArrayMath.subtractArrays : The arrays must be of the same size. Array 1 has " + array1.Length + " elements and Array 2 has " + array2.Length + " elements");
            }

            double[] resultArray = new double[array1.Length];

            for (int i = 0; i < array1.Length; i++)
            {
                resultArray[i] = array1[i] - array2[i];
            }
            return resultArray;
        }
        /// <summary>
        /// Divides each element of the second array from the matching element of the first array.
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static double[] DivideArrays(double[] array1, double[] array2)
        {
            if (array1 == null || array2 == null)
            {
                throw new ArgumentException("Neither array may be null.");
            }

            if (array1.Length != array2.Length)
            {
                throw new ArgumentException("The arrays must be of the same size. Array 1 has " + array1.Length + " elements and Array 2 has " + array2.Length + " elements");
            }

            double[] resultArray = new double[array1.Length];

            for (int i = 0; i < array1.Length; i++)
            {
                resultArray[i] = array1[i] / array2[i];
            }
            return resultArray;
        }
        /// <summary>
        /// Multiplies each element of the second array from the matching element of the first array.
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static double[] MultiplyArrays(double[] array1, double[] array2)
        {
            if (array1 == null || array2 == null)
            {
                throw new ArgumentException("Neither array may be null.");
            }

            if (array1.Length != array2.Length)
            {
                throw new ArgumentException("The arrays must be of the same size. Array 1 has " + array1.Length + " elements and Array 2 has " + array2.Length + " elements");
            }

            double[] resultArray = new double[array1.Length];

            for (int i = 0; i < array1.Length; i++)
            {
                resultArray[i] = array1[i] * array2[i];
            }
            return resultArray;
        }

        /// <summary>
        /// Appends two arrays.
        /// </summary>
        /// <param name="array1">Start array</param>
        /// <param name="array2">Array to append</param>
        /// <returns>An array of consisting of all the elements of array2 appended to array1.</returns>
        public static double[] JoinArrays(double[] array1, double[] array2)
        {
            if (array1 == null || array2 == null)
            {
                throw new ArgumentException("ArrayMath.JoinArrays : Neither array may be null");
            }
            double[] bigArray = new double[array1.Length + array2.Length];

            array1.CopyTo(bigArray, 0);
            array2.CopyTo(bigArray, array1.Length);

            return bigArray;
        }

        /// <summary>
        /// Appends two arrays.
        /// </summary>
        /// <param name="array1">Start array</param>
        /// <param name="array2">Array to append</param>
        /// <returns>An array of consisting of all the elements of array2 appended to array1.</returns>
        public static int[] JoinArrays(int[] array1, int[] array2)
        {
            if (array1 == null || array2 == null)
            {
                throw new ArgumentException("JoinArrays: Neither array may be null");
            }

            int[] bigArray = new int[array1.Length + array2.Length];

            array1.CopyTo(bigArray, 0);
            array2.CopyTo(bigArray, array1.Length);

            return bigArray;
        }

        /// <summary>
        /// Extracts the subset of data between two specified indeces.
        /// </summary>
        /// <param name="inputArray">Array to extract data from</param>
        /// <param name="fromIndex">Starting index</param>
        /// <param name="toIndex">Stop index</param>
        /// <returns>A copy of the array containing data between fromIndex and toIndex</returns>
        public static double[] ExtractSubArray(double[] inputArray, int fromIndex, int toIndex)
        {
            // PRECONDITIONS
            if (fromIndex < 0 || fromIndex >= inputArray.Length)
            {
                throw new ArgumentException("ArrayMath.extractSubArray : fromIndex is out of bounds. Array has " + inputArray.Length + " elements, fromIndex is " + fromIndex);
            }
            if (toIndex < 0 || toIndex >= inputArray.Length)
            {
                throw new ArgumentException("ArrayMath.extractSubArray : toIndex is out of bounds. Array has " + inputArray.Length + " elements, toIndex is " + toIndex);
            }
            if (fromIndex > toIndex)
            {
                throw new ArgumentException("ArrayMath.extractSubArray : fromIndex (" + fromIndex + ") is greater then toIndex (" + toIndex + ")");
            }
            if (inputArray.Length == 0)
            {
                throw new ArgumentException("ArrayMath.extractSubArray : inputArray is empty");
            }

            double[] subsetArray = new double[toIndex - fromIndex + 1];

            for (int i = fromIndex; i <= toIndex; i++)
            {
                subsetArray[i - fromIndex] = inputArray[i];
            }
            return subsetArray;
        }

        /// <summary>
        /// Extracts the subset of data between two specified indeces.
        /// </summary>
        /// <param name="inputArray">Array to extract data from</param>
        /// <param name="fromIndex">Starting index</param>
        /// <param name="toIndex">Stop index</param>
        /// <returns>A copy of the array containing data between fromIndex and toIndex</returns>
        public static byte[] ExtractSubArray(byte[] inputArray, int fromIndex, int toIndex)
        {
            // PRECONDITIONS
            if (fromIndex < 0 || fromIndex >= inputArray.Length)
            {
                throw new ArgumentException("ArrayMath.extractSubArray : fromIndex is out of bounds. Array has " + inputArray.Length + " elements, fromIndex is " + fromIndex);
            }
            if (toIndex < 0 || toIndex >= inputArray.Length)
            {
                throw new ArgumentException("ArrayMath.extractSubArray : toIndex is out of bounds. Array has " + inputArray.Length + " elements, toIndex is " + toIndex);
            }
            if (fromIndex > toIndex)
            {
                throw new ArgumentException("ArrayMath.extractSubArray : fromIndex (" + fromIndex + ") is greater then toIndex (" + toIndex + ")");
            }
            if (inputArray.Length == 0)
            {
                throw new ArgumentException("ArrayMath.extractSubArray : inputArray is empty");
            }

            byte[] subsetArray = new byte[toIndex - fromIndex + 1];

            for (int i = fromIndex; i <= toIndex; i++)
            {
                subsetArray[i - fromIndex] = inputArray[i];
            }
            return subsetArray;
        }


        /// <summary>
        /// Finds the index of the element that has a value closest to valueToFind
        /// </summary>
        /// <param name="inputArray">Set of data to search</param>
        /// <param name="valueToFind">Value to be found</param>
        /// <returns>Index of the element having a value closest to 'valueToFind'</returns>
        public static int FindIndexOfNearestElement(double[] inputArray, double valueToFind)
        {
            // PRECONDITIONS
            if (inputArray == null)
            {
                throw new ArgumentException("ArrayMath.findIndexOfNearestElement : inputArray is null");
            }
            if (inputArray.Length == 0)
            {
                throw new ArgumentException("ArrayMath.findIndexOfNearestElement : inputArray is empty");
            }

            double smallestDifference = double.MaxValue;
            int nearestElement = 0;

            for (int i = 0; i < inputArray.Length; i++)
            {
                double difference = Math.Abs(inputArray[i] - valueToFind);
                if (difference < smallestDifference)
                {
                    smallestDifference = difference;
                    nearestElement = i;
                }
            }

            return nearestElement;
        }

        /// <summary>
        /// Removes any data points outside the range specified by minimumValue and maximumValue.
        /// </summary>
        /// <param name="minimumValue">Minimum value</param>
        /// <param name="maximumValue">Maximum value</param>
        /// <param name="rawArray">Array of data</param>
        /// <param name="mode">Specify whether to use data within or outside the min and max values</param>
        /// <returns>The subset of data matching the above criteria</returns>
        public static double[] GetSubArrayByValueRange(double minimumValue, double maximumValue, double[] rawArray, BoundaryCondition mode)
        {
            double[] subsetArray;

            switch (mode)
            {
                case BoundaryCondition.OutsideMinMaxBoundary:
                    subsetArray = Array.FindAll(rawArray, delegate(double value) { return value < minimumValue || value > maximumValue; });
                    break;

                case BoundaryCondition.InsideMinMaxBoundary:
                default:
                    subsetArray = Array.FindAll(rawArray, delegate(double value) { return value >= minimumValue && value <= maximumValue; });
                    break;
            }

            return subsetArray;
        }
        /// <summary>
        /// Gets sub array between start index and stop index.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="startIndex"></param>
        /// <param name="stopIndex"></param>
        /// <returns></returns>
        public static double[] GetSubArray(double[] values, int startIndex, int stopIndex)
        {
            List<double> list = new List<double>();
            if (startIndex < 0 || stopIndex >= values.Length)
            {
                throw new ArgumentException(string.Format("StartIndex or StopIndex is out of range. startIndex={0}; stopIndex={1}.", startIndex, stopIndex));
            }

            for (int i = startIndex; i <= stopIndex; i++)
            {
                list.Add(values[i]);
            }

            return list.ToArray();
        }  
        /// <summary>
        /// Calculate first derivate using fixed step size
        /// </summary>
        /// <param name="xArray"></param>
        /// <param name="yArray"></param>
        /// <param name="stepSize"></param>
        /// <returns></returns>
        public static double[] CalculateFirstDerivateWithFixedStepSize(double[] xArray, double[] yArray, int stepSize)
        {
            if (xArray.Length != yArray.Length)
            {
                throw new ArgumentException("X and Y array should be the same size.");
            }
            double[] derivates = new double[xArray.Length];
            double[] derivateOutputs = new double[xArray.Length];

            for (int i = 0; i < derivates.Length - stepSize; i++)
            {
                derivates[i] = (yArray[i + stepSize] - yArray[i]) / (xArray[i + stepSize] - xArray[i]);
            }
            for (int i = derivates.Length - stepSize; i < derivates.Length; i++)
            {
                derivates[i] = derivates[derivates.Length - stepSize - 1];
            }

            return derivates;
        }

        /// <summary>
        /// Defines how to search for values
        /// </summary>
        public enum BoundaryCondition
        {
            /// <summary>
            /// Less than or equal to the boundary conditions
            /// </summary>
            InsideMinMaxBoundary,
            /// <summary>
            /// Greater than the boundary conditions
            /// </summary>
            OutsideMinMaxBoundary
        }
    }
}
