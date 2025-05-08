using NationalInstruments.Restricted;
using System.Linq;
using System.Security.Principal;

namespace NationalInstruments.OptoelectronicComponentTest
{
    /// <summary>
    /// Includes the APIs to analyze the LIV curve
    /// </summary>
    public class LivAnalysis
    {
        /// <summary>
        /// Use this API to analyze the LIV curve and get related optical characteristics. 
        /// </summary>
        /// <param name="driveCurrents">Specifies the current value of the laser diode drive at each step during a sweeping. </param>
        /// <param name="measuredPowers">Specifies the measured optical power at each step during a sweeping. </param>
        /// <param name="smoothWindowLength">Specifies the length of a window where a smooth data point is calculated by averaging data points within this window. The window is moving along the data.</param>
        /// <returns></returns>
        public static LivAnalysisResults AnalyzeLiv(double[] driveCurrents, double[] measuredPowers, int smoothWindowLength)
        {
            LivAnalysisResults livAnalysisResult = new LivAnalysisResults();
            double[] smoothDriveCurrents = SmoothArray(driveCurrents, smoothWindowLength);
            double[] smoothOpticalPowers = SmoothArray(measuredPowers, smoothWindowLength);

            // With derivative of powers, currents and powers are defined to be the midpoint of each two line segments in previous array.
            double[] derivativeArray = GetFirstDerivative(smoothOpticalPowers);
            double[] midDriveCurrents = SmoothArray(smoothDriveCurrents, 2);
            double[] midOpticalPowers = SmoothArray(smoothOpticalPowers, 2);

            double maxDerivative = derivativeArray.Max();
            int maxIndex = derivativeArray.IndexOf(maxDerivative);
            double thresholdValue = maxDerivative / 2;
            double thresholdIndex = ThresholdArray(derivativeArray, thresholdValue);
            livAnalysisResult.ThresholdCurrent = InterpolateArray(midDriveCurrents, thresholdIndex);
            
            double thresholdPower = InterpolateArray(midOpticalPowers, thresholdIndex);

            //The datas near thresholdIndex are obviously small, so set startIndex between thresholdIndex and maxIndex
            int startIndex = (maxIndex + (int)thresholdIndex) / 2;
            double[] slopeEfficencies = new double[midOpticalPowers.Length - startIndex];
            for (int i = startIndex; i < midOpticalPowers.Length; i++)
            {
                slopeEfficencies[i - startIndex] = (midOpticalPowers[i] - thresholdPower) / (midDriveCurrents[i] - livAnalysisResult.ThresholdCurrent);
            }
            livAnalysisResult.SlopeEfficency = slopeEfficencies.Average();
            return livAnalysisResult;
        }

        private static double[] SmoothArray(double[] data, int windowLength)
        {
            int length = data.Length;
            if (windowLength > length)
                windowLength = length;
            int smoothCount = length - windowLength + 1;
            double[] smoothData = new double[smoothCount];
            for (int i = 0; i < smoothCount; i++)
            {
                double sumValue = 0;
                for (int j = 0; j < windowLength; j++)
                {
                    sumValue += data[i + j];
                }
                smoothData[i] = sumValue / windowLength;
            }
            return smoothData;
        }

        private static double[] GetFirstDerivative(double[] dataArray)
        {
            int length = dataArray.Length;
            double[] derivativeDataArray = new double[length - 1];
            for (int i = 0; i < length - 1; i++)
            {

                derivativeDataArray[i] = dataArray[i + 1] - dataArray[i];
            }
            return derivativeDataArray;
        }

        private static double ThresholdArray(double[] dataArray, double threshholdVaule)
        {
            int length = dataArray.Length;
            for (int i = 0; i < length; i++)
            {
                if (threshholdVaule == dataArray[i])
                {
                    return i;
                }
                else if (dataArray[i] > threshholdVaule)
                {
                    if (i == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        double index = i - (dataArray[i] - threshholdVaule) / (dataArray[i] - dataArray[i - 1]);
                        return index;
                    }
                }

            }
            return -1;
        }

        private static double InterpolateArray(double[] data, double fractionalIndex)
        {
            int length = data.Length;
            if (fractionalIndex >= length - 1)
                return data[length - 1];
            int integerIndex = (int)fractionalIndex;
            double value = data[integerIndex] + (data[integerIndex + 1] - data[integerIndex]) * (fractionalIndex - integerIndex);
            return value;
        }
    }
}
