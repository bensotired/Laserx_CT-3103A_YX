using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestPackage
{  
    public class FarFieldTestDataItem
    {       
        public double Temp_FF_C { get; set; }
        public double IFF1_mA { get; set; }
        public double AnglularWidth_95Percent_Deg { get; set; }
        public double AnglularWidth_e2_Deg { get; set; }
        public double AW1_FWHM_Deg { get; set; }
        public double AC1_Deg { get; set; }
        public double leftPos { get; set; }
        public double rightPos { get; set; }
        public double width { get; set; }
        public double center { get; set; }
        public double ripple { get; set; }       
    }
    public class FarFieldWidth
    {
        public double H_width_13p5 { get; set; }
        public double V_width_13p5 { get; set; }
        public double H_width_50 { get; set; }
        public double V_width_50 { get; set; }
        public double H_width_5 { get; set; }
        public double V_width_5 { get; set; }
    }
    public static class AllNanoScanAnalyse
    {  
       
        public static FarFieldTestDataItem CalculatotAllData(bool Curve_Fit, List<double> angle, List<double> power, bool UseOldMethod=false)  //1063
        {
            try
            {
                FarFieldTestDataItem data = new FarFieldTestDataItem();
                
                    int maxIndex, minIndex;
                    List<double> newP = new List<double>();
                    if (Curve_Fit == true)
                    {
                        var coeff = PolyFitMath.PolynomialFit(angle.ToArray(), power.ToArray(), 18);
                        ArrayMath.GetMaxAndMinIndex(coeff.FittedYArray, out maxIndex, out minIndex);
                        double maxPower = coeff.FittedYArray[maxIndex];
                        double minPower = coeff.FittedYArray[minIndex];
                        double dev = maxPower;
                        foreach (double p in coeff.FittedYArray)
                        {
                            newP.Add((p - minPower) / (maxPower - minPower));
                        }                      
                    }
                    else
                    {
                        if (UseOldMethod)
                        {
                            ArrayMath.GetMaxAndMinIndex(power.ToArray(), out maxIndex, out minIndex);
                            double maxPower = power[maxIndex];
                            double minPower = power[minIndex];
                            double dev = maxPower;
                            foreach (double p in power)
                            {
                                newP.Add((p - minPower) / (maxPower - minPower));
                            }
                        }
                        else
                        {
                            //new method 
                            ArrayMath.GetMaxAndMinIndex(power.ToArray(), out maxIndex, out minIndex);
                            double maxPower = power[maxIndex];
                            double minPower = power[minIndex];

                            foreach (double p in power)
                            {
                                newP.Add((p));
                            }
                        }
                    }
                    
                    if (UseOldMethod)
                    {
                            data.AW1_FWHM_Deg = CalculateAngleWidth(angle, newP, 0.5);//max 20   0.5    20*0.5 =10 
                            data.AnglularWidth_e2_Deg = CalculateAngleWidth(angle, newP, 0.135);
                            data.AnglularWidth_95Percent_Deg =CalculateAngleWidth(angle, newP, 0.05);

                            ArrayMath.GetMaxAndMinIndex(newP.ToArray(), out maxIndex, out minIndex);
                            data.AC1_Deg = angle[maxIndex];                       
                    }
                    else
                    {
                            data.AW1_FWHM_Deg = CalculateAngleWidthPLUSPLUS(angle, newP, 0.5 * (newP[maxIndex]));//max 20   0.5    20*0.5 =10 
                            data.AnglularWidth_e2_Deg =CalculateAngleWidthPLUSPLUS(angle, newP, 0.135 * (newP[maxIndex]));
                            data.AnglularWidth_95Percent_Deg = CalculateAngleWidthPLUSPLUS(angle, newP, 0.05 * (newP[maxIndex]));
                            double Center, leftPos, rightPos, width, ripple;
                            Get_Data(angle, power, out leftPos, out rightPos, out width, out Center, out ripple);
                            data.center = Center;
                            data.leftPos = leftPos;
                            data.rightPos = rightPos;
                            data.width = width;
                            data.ripple = ripple;
                            ArrayMath.GetMaxAndMinIndex(newP.ToArray(), out maxIndex, out minIndex);
                            data.AC1_Deg = angle[maxIndex];                        
                    }
                    return data;
                }         
            catch (Exception ex)
            {
                return null;
                throw new Exception("CalculatotAllData_ReturnSingle:", ex);
            }
        }
        public static FarFieldWidth CalculatoWidth(double DistanceDiff, List<double> angle, List<double> power_X_1st, List<double> power_Y_1st, List<double> power_X_2nd, List<double> power_Y_2nd)
        {
            try
            {
                FarFieldWidth data = new FarFieldWidth();
                double H_width = 0;
                double V_width = 0;               

                double BW_X_1st_0_5, BW_X_1st_0_135, BW_X_1st_0_05;
                Calculate_NanoScan(angle, power_X_1st, out BW_X_1st_0_5, out BW_X_1st_0_135, out BW_X_1st_0_05);

                double BW_Y_1st_0_5, BW_Y_1st_0_135, BW_Y_1st_0_05;
                Calculate_NanoScan(angle, power_Y_1st, out BW_Y_1st_0_5, out BW_Y_1st_0_135, out BW_Y_1st_0_05);


                double BW_X_2nd_0_5, BW_X_2nd_0_135, BW_X_2nd_0_05;
                Calculate_NanoScan(angle, power_X_2nd, out BW_X_2nd_0_5, out BW_X_2nd_0_135, out BW_X_2nd_0_05);

                double BW_Y_2nd_0_5, BW_Y_2nd_0_135, BW_Y_2nd_0_05;
                Calculate_NanoScan(angle, power_Y_2nd, out BW_Y_2nd_0_5, out BW_Y_2nd_0_135, out BW_Y_2nd_0_05);

                H_width = (BW_X_2nd_0_5 / 2 - BW_X_1st_0_5 / 2) / DistanceDiff;
                V_width = (BW_Y_2nd_0_5 / 2 - BW_Y_1st_0_5 / 2) / DistanceDiff;
                data.H_width_50 = Math.Atan(H_width) * 2;
                data.H_width_50 = Math.Atan(V_width) * 2;

                H_width = (BW_X_2nd_0_135 / 2 - BW_X_1st_0_135 / 2) / DistanceDiff;
                V_width = (BW_Y_2nd_0_135 / 2 - BW_Y_1st_0_135 / 2) / DistanceDiff;
                data.H_width_13p5 = Math.Atan(H_width) * 2;
                data.V_width_13p5 = Math.Atan(V_width) * 2;

                H_width = (BW_X_2nd_0_05 / 2 - BW_X_1st_0_05 / 2) / DistanceDiff;
                V_width = (BW_Y_2nd_0_05 / 2 - BW_Y_1st_0_05 / 2) / DistanceDiff;
                data.H_width_5 = Math.Atan(H_width) * 2;
                data.V_width_5 = Math.Atan(V_width) * 2;

                return data;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception("CalculatotAllData_ReturnSingle:", ex);
            }
        }
        private static void Calculate_NanoScan(List<double> x_arr, List<double> y_arr, out double width_BW0_5, out double width_BW0_135, out double width_BW0_05)
        {
            double leftPos, rightPos, center, ripple;
            double[] normPower = new double[y_arr.Count];
            for (int i = 0; i < y_arr.Count; i++)
            {
                normPower[i] = (y_arr[i] - y_arr.Min()) / (y_arr.Max() - y_arr.Min());
            }

            //高斯拟合
            double[] fitPower = GaussianFit(x_arr.ToArray(), normPower.ToArray(), 0.5);

            int maxIndex = 0;
            int minIndex = 0;
            GetMaxAndMinIndex(normPower, out maxIndex, out minIndex);
            //插值找到功率0.5对应的左右角度
            leftPos = DoPiecewiseLinearInterpolation(0.5, ExtractSubArray(normPower, 0, maxIndex), ExtractSubArray(x_arr.ToArray(), 0, maxIndex));
            rightPos = DoPiecewiseLinearInterpolation(0.5, ExtractSubArray(normPower, maxIndex, fitPower.Length - 1), ExtractSubArray(x_arr.ToArray(), maxIndex, x_arr.Count - 1));
            //左右功率=0.5时的角度差，也就是发散角
            width_BW0_5 = rightPos - leftPos;
            //左右功率=0.5时的中点角度，
            center = (rightPos + leftPos) / 2;
            //原始点偏离拟合点的最大最小差值
            ripple = CalculateRipple(normPower.ToArray(), fitPower);

            //插值找到功率0.135对应的左右角度
            leftPos = DoPiecewiseLinearInterpolation(0.135, ExtractSubArray(normPower, 0, maxIndex), ExtractSubArray(x_arr.ToArray(), 0, maxIndex));
            rightPos = DoPiecewiseLinearInterpolation(0.135, ExtractSubArray(normPower, maxIndex, fitPower.Length - 1), ExtractSubArray(x_arr.ToArray(), maxIndex, x_arr.Count - 1));
            //左右功率=0.5时的角度差，也就是发散角
            width_BW0_135 = rightPos - leftPos;

            //插值找到功率0.05对应的左右角度
            leftPos = DoPiecewiseLinearInterpolation(0.05, ExtractSubArray(normPower, 0, maxIndex), ExtractSubArray(x_arr.ToArray(), 0, maxIndex));
            rightPos = DoPiecewiseLinearInterpolation(0.05, ExtractSubArray(normPower, maxIndex, fitPower.Length - 1), ExtractSubArray(x_arr.ToArray(), maxIndex, x_arr.Count - 1));
            //左右功率=0.5时的角度差，也就是发散角
            width_BW0_05 = rightPos - leftPos;
        }
        private static double CalculateAngleWidth(List<double> angle, List<double> power, double level)
        {
            if (angle.Count != power.Count) return 0;
            double x1 = 0;
            double x2 = 0;
            for (int i = 0; i < angle.Count - 1; i++)
            {
                if (power[i] < level && power[i + 1] > level)
                {
                    x1 = (level - power[i]) / ((power[i + 1] - power[i]) / (angle[i + 1] - angle[i])) + angle[i];
                }
                if (power[i] > level && power[i + 1] < level)
                {
                    x2 = angle[i + 1] - (level - power[i + 1]) / ((power[i] - power[i + 1]) / (angle[i + 1] - angle[i]));
                }
            }
            var ret = x2 - x1;
            return ret;
        }
        private static double CalculateAngleWidthPLUSPLUS(List<double> angle, List<double> power, double level)
        {
            if (angle.Count != power.Count) return 0;
            int maxIndex;
            int minIndex;
            ArrayMath.GetMaxAndMinIndex(power.ToArray(), out maxIndex, out minIndex);
            List<double> angleUp = new List<double>();
            List<double> angleDown = new List<double>();
            List<double> powerUp = new List<double>();
            List<double> powerDown = new List<double>();
            for (int i = 0; i < maxIndex; i++)
            {
                angleUp.Add(angle[i]);
                powerUp.Add(power[i]);
            }
            for (int i = maxIndex; i < angle.Count; i++)
            {
                angleDown.Add(angle[i]);
                powerDown.Add(power[i]);
            }
            double x1 = Interpolator.DoPiecewiseLinearInterpolation(level, powerUp.ToArray(), angleUp.ToArray());
            double x2 = Interpolator.DoPiecewiseLinearInterpolation(level, powerDown.ToArray(), angleDown.ToArray());

            var ret = Math.Abs(x2 - x1);
            return ret;
        }
        public static void Get_Data(List<double> x_arr, List<double> y_arr, out double leftPos, out double rightPos, out double width, out double center, out double ripple)
        {

            double[] normPower = new double[y_arr.Count];
            for (int i = 0; i < y_arr.Count; i++)
            {
                normPower[i] = (y_arr[i] - y_arr.Min()) / (y_arr.Max() - y_arr.Min());
            }

            //高斯拟合
            double[] fitPower = GaussianFit(x_arr.ToArray(), normPower.ToArray(), 0.5);



            int maxIndex = 0;
            int minIndex = 0;
            GetMaxAndMinIndex(normPower, out maxIndex, out minIndex);
            //插值找到功率0.5对应的左右角度
            leftPos = DoPiecewiseLinearInterpolation(0.5, ExtractSubArray(normPower, 0, maxIndex), ExtractSubArray(x_arr.ToArray(), 0, maxIndex));
            rightPos = DoPiecewiseLinearInterpolation(0.5, ExtractSubArray(normPower, maxIndex, fitPower.Length - 1), ExtractSubArray(x_arr.ToArray(), maxIndex, x_arr.Count - 1));
            //左右功率=0.5时的角度差，也就是发散角
            width = rightPos - leftPos;
            //左右功率=0.5时的中点角度，
            center = (rightPos + leftPos) / 2;
            //原始点偏离拟合点的最大最小差值
            ripple = CalculateRipple(normPower.ToArray(), fitPower);
        }
        public static double DoPiecewiseLinearInterpolation(double newX, double[] xData, double[] yData)
        {
            // Handle the special cases of mismatched or single points.
            if (xData.Length != yData.Length || xData.Length == 0)
            {
                throw new ArgumentException("xData and yData array length don't match or don't have any points");
            }
            if (xData.Length == 1)
            {
                // There is only 1 point so use the line y = a, where
                // the slope is 0 and the y value for all x values is
                // the y value of the point.
                return yData[0];
            }

            // Our real work.  Since we don't require the points to be in 
            // a particular order, we go through great pains to find which
            // pair of points to use in the interpolation.  Even though
            // the calculation only requires 2 points, we find the 4
            // closest points.  We'd like the 2 closest points to straddle 
            // the X target.  However, we can't rely on this because of
            // cases where the data is irregularly spaced or when we are 
            // extrapolating past the end of the dataset.  Thus finding 
            // the 2 closest points on each side of the target is required.  
            // The counts are used to determine which case our data fits.  At 
            // the end of the loop, the following conditions will be true:
            //   leftX2 <= leftX1 < targetX <= rightX1 <= rightX2
            // (Note:  The initial values for the Ys aren't really needed  
            // but I couldn't convince the compiler that an unassigned value
            // wouldn't be used.)
            int leftCount = 0;
            double leftX1 = double.NegativeInfinity;
            double leftY1 = 0;
            double leftX2 = double.NegativeInfinity;
            double leftY2 = 0;
            int rightCount = 0;
            double rightX1 = double.PositiveInfinity;
            double rightY1 = 0;
            double rightX2 = double.PositiveInfinity;
            double rightY2 = 0;
            for (int i = 0; i < xData.Length; i++)
            {
                // Lazy typer assignments
                double x = xData[i];
                double y = yData[i];

                // See if the point in the list is on the left or right of
                // the target point we are interpolating to.
                if (x < newX)
                {
                    // Keep the biggest two on the left
                    leftCount++;
                    if (x > leftX1)
                    {
                        leftX2 = leftX1;
                        leftY2 = leftY1;
                        leftX1 = x;
                        leftY1 = y;
                    }
                    else if (x > leftX2)
                    {
                        leftX2 = x;
                        leftY2 = y;
                    }
                }
                else
                {
                    // Keep the smallest two on the right
                    rightCount++;
                    if (x < rightX1)
                    {
                        rightX2 = rightX1;
                        rightY2 = rightY1;
                        rightX1 = x;
                        rightY1 = y;
                    }
                    else if (x < rightX2)
                    {
                        rightX2 = x;
                        rightY2 = y;
                    }
                }
            }

            // See which pair of points we should use to interpolate
            // with.  The choices are L2+L1, L1+R1, or R1+R2.  
            double x1, x2, y1, y2;
            if (rightCount == 0)
            {
                // use the two left points
                x1 = leftX1;
                y1 = leftY1;
                x2 = leftX2;
                y2 = leftY2;
            }
            else if (leftCount == 0)
            {
                // use the two right points
                x1 = rightX1;
                y1 = rightY1;
                x2 = rightX2;
                y2 = rightY2;
            }
            else
            {
                // use a left and a right point
                x1 = leftX1;
                y1 = leftY1;
                x2 = rightX1;
                y2 = rightY1;
            }

            // Here we are, the actual interpolation!
            double newY;
            if (x1 == x2)
            {
                newY = (y1 + y2) / 2.0;
            }
            else
            {
                newY = y1 + (newX - x1) * (y1 - y2) / (x1 - x2);
            }
            return newY;
        }
        public static double[] GaussianFit(double[] xArray, double[] yArray, double thres = 0.5)
        {
            List<double> newArray = new List<double>();
            int pNum = 0;
            foreach (var item in yArray)
                if (item > thres)
                    pNum += 1;

            double[] xFit = new double[pNum];
            double[] yFit = new double[pNum];

            int j = 0;
            for (int i = 0; i < yArray.Length; i++)
                if (yArray[i] > thres)
                {
                    xFit[j] = xArray[i];
                    yFit[j++] = Math.Log(yArray[i]);
                }

            double[] fit = MathNet.Numerics.Fit.Polynomial(xFit, yFit, 2);
            //
            //for (int i = 0; i < yFit.Length; i++)
            //    newArray.Add(Math.Exp(fit[0] + fit[1] * xFit[i] + fit[2] * Math.Pow(xFit[i], 2)));

            for (int i = 0; i < yArray.Length; i++)
            {
                newArray.Add(Math.Exp(fit[0] + fit[1] * xArray[i] + fit[2] * Math.Pow(xArray[i], 2)));
            }

            return newArray.ToArray();
        }
        public static void GetMaxAndMinIndex(double[] values, out int maxIndex, out int minIndex)
        {
            GetMaxAndMinIndexBeforeSpecificIndex(values, values.Length - 1, out maxIndex, out minIndex);
        }
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
        private static double CalculateRipple(double[] rawPower, double[] fitPower)
        {
            double ripple = 1;
            double maxVariance = 0;
            double minVariance = 1;
            for (int i = 0; i < rawPower.Length; i++)
            {
                if ((rawPower[i] / rawPower.Max() >= 0.5))
                {
                    double variance = rawPower[i] - fitPower[i];

                    if (variance > maxVariance)
                    {
                        maxVariance = variance;
                    }
                    if (variance < minVariance)
                    {
                        minVariance = variance;
                    }
                }
            }
            ripple = Math.Abs(maxVariance) + Math.Abs(minVariance);

            return ripple;
        }
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
        #region 1063
        //public void Calculate()  //1063
        //{
        //    try
        //    {

        //        foreach (var rawData in RawDataList)
        //        {
        //            string dataName = rawData.Key;

        //            List<double> angle = new List<double>();
        //            List<double> power = new List<double>();
        //            FarFieldTestDataItem data = null;
        //            var keyArr = rawData.Key.Split('_');//FF_I{0}_H_raw
        //            string datakey = string.Format("{0}_{1}_cal", keyArr[0], keyArr[1]);
        //            foreach (var dataPoint in rawData.Value.Points)
        //            {
        //                angle.Add(dataPoint.Theta);
        //                power.Add(dataPoint.PD_Reading);
        //            }

        //            int maxIndex, minIndex;
        //            List<double> newP = new List<double>();
        //            if (Curve_Fit == true)
        //            {
        //                //this is a joke
        //                var coeff = PolyFitMath.PolynomialFit(angle.ToArray(), power.ToArray(), 18);
        //                ArrayMath.GetMaxAndMinIndex(coeff.FittedYArray, out maxIndex, out minIndex);
        //                double maxPower = coeff.FittedYArray[maxIndex];
        //                double minPower = coeff.FittedYArray[minIndex];
        //                double dev = maxPower;
        //                foreach (double p in coeff.FittedYArray)
        //                {
        //                    newP.Add((p - minPower) / (maxPower - minPower));
        //                }
        //                //StreamWriter sw = new StreamWriter("temp.csv");
        //                //for (int i = 0; i < angle.Count; i++)
        //                //{
        //                //    sw.WriteLine("{0},{1},{2}", angle[i], power[i], newP[i]);
        //                //}
        //                //sw.Close();
        //            }
        //            else
        //            {
        //                if (UseOldMethod)
        //                {
        //                    ArrayMath.GetMaxAndMinIndex(power.ToArray(), out maxIndex, out minIndex);
        //                    double maxPower = power[maxIndex];
        //                    double minPower = power[minIndex];
        //                    double dev = maxPower;
        //                    foreach (double p in power)
        //                    {
        //                        newP.Add((p - minPower) / (maxPower - minPower));
        //                    }
        //                }
        //                else
        //                {
        //                    //new method 
        //                    ArrayMath.GetMaxAndMinIndex(power.ToArray(), out maxIndex, out minIndex);
        //                    double maxPower = power[maxIndex];
        //                    double minPower = power[minIndex];

        //                    foreach (double p in power)
        //                    {
        //                        newP.Add((p));
        //                    }
        //                }
        //            }

        //            if (DataList.ContainsKey(datakey))
        //            {
        //                data = DataList[datakey];
        //            }
        //            else
        //            {
        //                data = new FarFieldTestDataItem();
        //                data.Temp_FF_C = rawData.Value.Temp;
        //                data.IFF1_mA = rawData.Value.Current;
        //                DataList.Add(datakey, data);
        //                FFTestDataItem.Add(data);
        //            }

        //            if (UseOldMethod)
        //            {
        //                if (rawData.Key.Split('_')[2] == "H")//FF_I{0}_H_raw
        //                {
        //                    data.AW1_H_FWHM_Deg = this.CalculateAngleWidth(angle, newP, 0.5);//max 20   0.5    20*0.5 =10 
        //                    data.AnglularWidth_H_e2_Deg = this.CalculateAngleWidth(angle, newP, 0.135);
        //                    data.AnglularWidth_H_95Percent_Deg = this.CalculateAngleWidth(angle, newP, 0.05);

        //                    ArrayMath.GetMaxAndMinIndex(newP.ToArray(), out maxIndex, out minIndex);
        //                    data.AC1_H_Deg = angle[maxIndex];
        //                }
        //                else if (rawData.Key.Split('_')[2] == "V")//FF_I{0}_V_raw
        //                {
        //                    data.AW1_V_FWHM_Deg = this.CalculateAngleWidth(angle, newP, 0.5);//50%
        //                    data.AnglularWidth_V_e2_Deg = this.CalculateAngleWidth(angle, newP, 0.135);
        //                    data.AnglularWidth_V_95Percen_Deg = this.CalculateAngleWidth(angle, newP, 0.05);

        //                    ArrayMath.GetMaxAndMinIndex(newP.ToArray(), out maxIndex, out minIndex);
        //                    data.AC1_V_Deg = angle[maxIndex];
        //                }
        //            }
        //            else
        //            {
        //                if (rawData.Key.Split('_')[2] == "H")//FF_I{0}_H_raw
        //                {
        //                    data.AW1_H_FWHM_Deg = this.CalculateAngleWidthPLUSPLUS(angle, newP, 0.5 * (newP[maxIndex]));//max 20   0.5    20*0.5 =10 
        //                    data.AnglularWidth_H_e2_Deg = this.CalculateAngleWidthPLUSPLUS(angle, newP, 0.135 * (newP[maxIndex]));
        //                    data.AnglularWidth_H_95Percent_Deg = this.CalculateAngleWidthPLUSPLUS(angle, newP, 0.05 * (newP[maxIndex]));
        //                    double Center, leftPos, rightPos, width, ripple;
        //                    SummaryFileWriter.Get_Data(angle, power, out leftPos, out rightPos, out width, out Center, out ripple);
        //                    data.center_H = Center;
        //                    data.leftPos_H = leftPos;
        //                    data.rightPos_H = rightPos;
        //                    data.width_H = width;
        //                    data.ripple_H = ripple;
        //                    ArrayMath.GetMaxAndMinIndex(newP.ToArray(), out maxIndex, out minIndex);
        //                    data.AC1_H_Deg = angle[maxIndex];
        //                }
        //                else if (rawData.Key.Split('_')[2] == "V")//FF_I{0}_V_raw
        //                {
        //                    data.AW1_V_FWHM_Deg = this.CalculateAngleWidthPLUSPLUS(angle, newP, 0.5 * (newP[maxIndex]));//50%
        //                    data.AnglularWidth_V_e2_Deg = this.CalculateAngleWidthPLUSPLUS(angle, newP, 0.135 * (newP[maxIndex]));
        //                    data.AnglularWidth_V_95Percen_Deg = this.CalculateAngleWidthPLUSPLUS(angle, newP, 0.05 * (newP[maxIndex]));
        //                    double Center, leftPos, rightPos, width, ripple;
        //                    SummaryFileWriter.Get_Data(angle, power, out leftPos, out rightPos, out width, out Center, out ripple);
        //                    data.center_V = Center;
        //                    data.leftPos_V = leftPos;
        //                    data.rightPos_V = rightPos;
        //                    data.width_V = width;
        //                    data.ripple_V = ripple;
        //                    ArrayMath.GetMaxAndMinIndex(newP.ToArray(), out maxIndex, out minIndex);
        //                    data.AC1_V_Deg = angle[maxIndex];
        //                }
        //            }                   
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionManager.HandleException(ex);
        //    }
        //}
        #endregion
    }
}
