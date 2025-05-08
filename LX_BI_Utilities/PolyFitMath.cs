using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LX_BurnInSolution.Utilities

{
    /// <summary>
    /// Math for polynomial fit.
    /// </summary>
    public static class PolyFitMath
    {
        /// <summary>PolyFitMath
        /// Fits a polynomial curve to the X,Y data.
        /// </summary>
        /// <param name="xArray">X axis data</param>
        /// <param name="yArray">Y axis data</param>
        /// <param name="order">The order of the polynomial fit.</param>
        /// <returns>The coefficients of the fit and a set of fitted data.</returns>
        public static PolyFitData PolynomialFit(double[] xArray, double[] yArray, int order)
        {
            InternalPolyFitMath math = new InternalPolyFitMath();
            return math.PolynomialFit(xArray, yArray, order);
        }

         /// <summary>
        /// Performs a polynomial fit over a subset of the array.
        /// </summary>
        /// <param name="xArray">X axis data</param>
        /// <param name="yArray">Y axis data</param>
        /// <param name="fromIndex">Index specifying the start of the range of data to be fitted</param>
        /// <param name="toIndex">Index specifying the end of the range of data to be fitted</param>
        /// <param name="order">The order of the polynomial fit.</param>
        /// <returns>The coefficients of the fit and a set of Y data fitted over the entire range of X.</returns>
        public static PolyFitData PolynomialFitWithIndex(double[] xArray, double[] yArray, int fromIndex, int toIndex, int order)
        {
            InternalPolyFitMath math = new InternalPolyFitMath();
            return math.PolynomialFitWithIndex(xArray, yArray, fromIndex, toIndex, order);
        }

         /// <summary>
        /// Performs a polynomial fit over a subset of the array which specifies x start and stop value.
        /// </summary>
        /// <param name="xArray">X axis data</param>
        /// <param name="yArray">Y axis data</param>
        /// <param name="fromXValue">X value specifying the start of the range of data to be fitted</param>
        /// <param name="toXValue">X value specifying the end of the range of data to be fitted</param>
        /// <param name="order">The order of the polynomial fit.</param>
        /// <returns>The coefficients of the fit and a set of Y data fitted over the entire range of X.</returns>
        public static PolyFitData PolynomialFitWithXValueRange(double[] xArray, double[] yArray, double fromXValue, double toXValue, int order)
        {
            InternalPolyFitMath math = new InternalPolyFitMath();
            return math.PolynomialFitWithXValueRange(xArray, yArray, fromXValue, toXValue, order);
        }

        /// <summary>
            /// Performs a polynomial fit over a subset of the array which specifies y start and stop value.
            /// </summary>
            /// <param name="xArray">X axis data</param>
            /// <param name="yArray">Y axis data</param>
            /// <param name="fromYValue">Y value specifying the start of the range of data to be fitted</param>
            /// <param name="toYValue">Y value specifying the end of the range of data to be fitted</param>
            /// <param name="order">The order of the polynomial fit.</param>
            /// <returns>The coefficients of the fit and a set of Y data fitted over the entire range of X.</returns>
        public static PolyFitData PolynomialFitWithYValueRange(double[] xArray, double[] yArray, double fromYValue, double toYValue, int order)
        {
            InternalPolyFitMath math = new InternalPolyFitMath();
            return math.PolynomialFitWithYValueRange(xArray, yArray, fromYValue, toYValue, order);
        }

        private class InternalPolyFitMath
        {
            #region Public Methods

            /// <summary>
            /// Fits a polynomial curve to the X,Y data.
            /// </summary>
            /// <param name="xArray">X axis data</param>
            /// <param name="yArray">Y axis data</param>
            /// <param name="order">The order of the polynomial fit.</param>
            /// <returns>The coefficients of the fit and a set of fitted data.</returns>
            public PolyFitData PolynomialFit(double[] xArray, double[] yArray, int order)
            {
                if (xArray.Length != yArray.Length)
                {
                    throw new ArgumentException("PolyFitMath.PolynomialFit : X and Y data arrays are of unequal size");
                }
                if (xArray.Length == 0)
                {
                    throw new ArgumentException("PolyFitMath.PolynomialFit : Missing X and Y data");
                }
                if (order > MaximumOrder)
                {
                    throw new ArgumentException("PolyFitMath.PolynomialFit : Fit order too high");
                }

                initialise(order);

                Fit(xArray, yArray);

                PolyFitData polyFit = new PolyFitData();
                polyFit.Coeffs = Coefficients;
                polyFit.FittedYArray = FittedY(xArray);
                return polyFit;
            }

            /// <summary>
            /// Performs a polynomial fit over a subset of the array.
            /// </summary>
            /// <param name="xArray">X axis data</param>
            /// <param name="yArray">Y axis data</param>
            /// <param name="fromIndex">Index specifying the start of the range of data to be fitted</param>
            /// <param name="toIndex">Index specifying the end of the range of data to be fitted</param>
            /// <param name="order">The order of the polynomial fit.</param>
            /// <returns>The coefficients of the fit and a set of Y data fitted over the entire range of X.</returns>
            public PolyFitData PolynomialFitWithIndex(double[] xArray, double[] yArray, int fromIndex, int toIndex, int order)
            {
                if (xArray.Length != yArray.Length)
                {
                    throw new ArgumentException("PolyFitMath.PolynomialFit_indexArgs : X and Y data arrays are of unequal size");
                }
                if (xArray.Length == 0)
                {
                    throw new ArgumentException("PolyFitMath.PolynomialFit_indexArgs : Missing X and Y data");
                }
                if (order > MaximumOrder)
                {
                    throw new ArgumentException("PolyFitMath.PolynomialFit_indexArgs : Fit order too high");
                }
                if (fromIndex >= toIndex)
                {
                    throw new ArgumentException("PolyFitMath.PolynomialFit_indexArgs : fromIndex must be > toIndex");
                }

                initialise(order);

                double[] trimmedXdata = ArrayMath.ExtractSubArray(xArray, fromIndex, toIndex);
                double[] trimmedYdata = ArrayMath.ExtractSubArray(yArray, fromIndex, toIndex);

                Fit(trimmedXdata, trimmedYdata);

                PolyFitData polyFit = new PolyFitData();
                polyFit.Coeffs = Coefficients;
                polyFit.FittedYArray = FittedY(xArray);
                return polyFit;
            }


            /// <summary>
            /// Performs a polynomial fit over a subset of the array which specifies x start and stop value.
            /// </summary>
            /// <param name="xArray">X axis data</param>
            /// <param name="yArray">Y axis data</param>
            /// <param name="fromXValue">X value specifying the start of the range of data to be fitted</param>
            /// <param name="toXValue">X value specifying the end of the range of data to be fitted</param>
            /// <param name="order">The order of the polynomial fit.</param>
            /// <returns>The coefficients of the fit and a set of Y data fitted over the entire range of X.</returns>
            public PolyFitData PolynomialFitWithXValueRange(double[] xArray, double[] yArray, double fromXValue, double toXValue, int order)
            {
                int startIndex = ArrayMath.FindIndexOfNearestElement(xArray, fromXValue);
                int stopIndex = ArrayMath.FindIndexOfNearestElement(xArray, toXValue);

                return PolynomialFitWithIndex(xArray, yArray, startIndex, stopIndex, order);
            }

            /// <summary>
            /// Performs a polynomial fit over a subset of the array which specifies y start and stop value.
            /// </summary>
            /// <param name="xArray">X axis data</param>
            /// <param name="yArray">Y axis data</param>
            /// <param name="fromYValue">Y value specifying the start of the range of data to be fitted</param>
            /// <param name="toYValue">Y value specifying the end of the range of data to be fitted</param>
            /// <param name="order">The order of the polynomial fit.</param>
            /// <returns>The coefficients of the fit and a set of Y data fitted over the entire range of X.</returns>
            public PolyFitData PolynomialFitWithYValueRange(double[] xArray, double[] yArray, double fromYValue, double toYValue, int order)
            {
                int startIndex = ArrayMath.FindIndexOfNearestElement(yArray, fromYValue);
                int stopIndex = ArrayMath.FindIndexOfNearestElement(yArray, toYValue);

                return PolynomialFitWithIndex(xArray, yArray, startIndex, stopIndex, order);
            }

            #endregion

            #region Private Methods
            /// <summary>
            /// Performs the fit
            /// </summary>
            /// <param name="xArray">X axis data</param>
            /// <param name="yArray">Y axis data</param>
            private void Fit(double[] xArray, double[] yArray)
            {
                int i = 0;
                for (i = xArray.GetLowerBound(0); i <= xArray.GetUpperBound(0); i++)
                {
                    PolyXYAdd(xArray[i], yArray[i]);
                }
                double[] dCoefficients = new double[Order + 1];
                for (i = 0; i <= Order; i++)
                {
                    dCoefficients[i] = Coeff(i);
                }
            }

            /// <summary>
            /// Generate a fitted Y axis by applying the fit coefficients to the X axis data
            /// </summary>
            /// <param name="xArray">X Axis data</param>
            /// <returns>Fitted Y axis data</returns>
            private double[] FittedY(double[] xArray)
            {
                double[] fittedY = new double[xArray.Length];

                for (int i = 0; i < xArray.Length; i++)
                {
                    double yValue = 0;
                    for (int powerOfX = 0; powerOfX <= Order; powerOfX++)
                    {
                        yValue += Coefficients[powerOfX] * Math.Pow(xArray[i], powerOfX);
                    }
                    fittedY[i] = yValue;
                }
                return fittedY;
            }

            /// <summary>
            /// Extract as many coefficients as are used from the array
            /// </summary>
            private double[] Coefficients
            {
                get
                {
                    double[] Coeff = new double[Order + 1];
                    for (int i = 0; i <= Order; i++)
                    {
                        Coeff[i] = C[i];
                    }
                    return (Coeff);
                }
            }

            /// <summary>
            /// Initialises internal variables to a size suited to the order of fit
            /// </summary>
            /// <param name="order">The order of the fit.</param>
            private void initialise(int order)
            {
                int matrixSize = order * 2 + 1;

                SumX = new double[matrixSize];
                SumYX = new double[matrixSize];
                M = new double[matrixSize, matrixSize + 1];
                C = new double[matrixSize];
                Order = order;
            }

            /// <summary>
            /// Process the XY value pair
            /// </summary>
            /// <param name="xValue">The X coordinate</param>
            /// <param name="yValue">The Y coordinate</param>
            private void PolyXYAdd(double xValue, double yValue)
            {
                int i = 0;
                //int nTX;
                double nTX;
                int nMax2O;

                nMax2O = 2 * Order;

                bFinished = false;
                nTX = 1;
                SumX[0] = SumX[0] + 1;
                SumYX[0] = SumYX[0] + yValue;
                for (i = 1; i <= Order; i++)
                {
                    //nTX = nTX * Convert.ToInt16(xValue);
                    nTX = nTX * xValue;
                    SumX[i] = SumX[i] + nTX;
                    SumYX[i] = SumYX[i] + yValue * nTX;
                }
                for (i = Order + 1; i <= nMax2O; i++)
                {
                    //nTX = nTX * Convert.ToInt16(xValue);
                    nTX = nTX * xValue;
                    SumX[i] = SumX[i] + nTX;
                }
            }

            /// <summary>
            /// Gets the count of the numbers of parametes added using PolyXYAdd
            /// </summary>
            private double XYCount
            {
                get
                {
                    return (SumX[0]);
                }
            }

            /// <summary>
            /// Retrieves one of the fit coefficients
            /// </summary>
            /// <param name="exp">The order of the</param>
            /// <returns>A coefficient value</returns>
            private double Coeff(int exp)
            {
                if (!bFinished) PolySolve();
                int Ex = Math.Abs(exp);
                double dLocalOrder = Order;
                if (XYCount <= dLocalOrder)
                {
                    dLocalOrder = XYCount - 1;
                }
                if (dLocalOrder < Ex)
                {
                    return (0);
                }
                else
                {
                    return (C[Ex]);
                }
            }

            /// <summary>
            /// Solves the equation
            /// </summary>
            private void PolySolve()
            {
                int LocalOrder = Order;
                if (XYCount <= LocalOrder)
                {
                    LocalOrder = Convert.ToInt16(XYCount) - 1;
                }
                if (LocalOrder < 0) return;
                PolyBuildMatrix(LocalOrder);
                //Gauss Solve
                try
                {
                    PolyGaussSolve(LocalOrder);
                }
                catch (Exception)
                {
                    // This was in the original implementation of the algorithm.
                    // It is not used during the normal course of events.
                    while (Order > 1)
                    {
                        C[0] = 0;
                        Order -= 1;
                        PolyFinaliseMatrix(Order);
                    }
                }
            }

            /// <summary>
            /// Build a matrix
            /// </summary>
            /// <param name="OrderToUse">Order of fit</param>
            private void PolyBuildMatrix(int OrderToUse)
            {
                int nOrd1 = OrderToUse + 1;
                for (int i = 0; i <= OrderToUse; i++)
                {
                    for (int k = 0; k <= OrderToUse; k++)
                    {
                        M[i, k] = SumX[i + k];
                    }
                    M[i, nOrd1] = SumYX[i];
                }
            }


            /// <summary>
            /// Solve the polynomial
            /// </summary>
            /// <param name="OrderToUse">Order of fit</param>
            private void PolyGaussSolve(int OrderToUse)
            {
                //Gauss Algorithm Implementation
                int LocalOrder = OrderToUse + 1;
                int nMax = 0;
                int k = 0;
                int j = 0;
                int i = 0;
                double T = 0;

                for (i = 0; i <= OrderToUse; i++)
                {
                    nMax = i;
                    T = Math.Abs(M[nMax, i]);
                    for (j = i + 1; j <= OrderToUse; j++)
                    {
                        if (T < Math.Abs(M[j, i]))
                        {
                            nMax = j;
                            T = Math.Abs(M[nMax, i]);
                        }
                    }
                    if (i < nMax)
                    {
                        for (k = i; k <= LocalOrder; k++)
                        {
                            T = M[i, k];
                            M[i, k] = M[nMax, k];
                            M[nMax, k] = T;
                        }
                    }
                    for (j = i + 1; j <= OrderToUse; j++)
                    {
                        T = M[j, i] / M[i, i];
                        M[j, i] = 0;
                        for (k = i + 1; k <= LocalOrder; k++)
                        {
                            M[j, k] = M[j, k] - M[i, k] * T;
                        }
                    }
                }
                //Now substitute the Coefficients
                for (j = OrderToUse; j >= 0; j--)
                {
                    T = M[j, LocalOrder];
                    for (k = j + 1; k <= OrderToUse; k++)
                    {
                        T = T - M[j, k] * C[k];
                    }
                    C[j] = T / M[j, j];
                }
                bFinished = true;

            }

            /// <summary>
            /// Tidy up the matrix. 
            /// This is only called in the event that an exception is caught during solving the polynomial.
            /// </summary>
            /// <param name="OrderToUse">Order of fit</param>
            private void PolyFinaliseMatrix(int OrderToUse)
            {
                int i = 0;
                int LocalOrder = OrderToUse + 1;
                for (i = 0; i <= OrderToUse; i++)
                {
                    M[i, LocalOrder] = SumYX[i];
                }
            }

            #endregion

            #region Private Members
            private int Order;
            private bool bFinished;
            private double[] SumX;
            private double[] SumYX;
            private double[,] M;
            private double[] C;
            private const int MaximumOrder = 300;
            #endregion
        }
    }

    /// <summary>
    /// Polynomial Fit results structure
    /// </summary>
    public struct PolyFitData
    {
        /// <summary>
        /// Array of Y points that were fitted
        /// </summary>
        public double[] FittedYArray;
        /// <summary>
        /// Polynomial coefficients
        /// </summary>
        public double[] Coeffs;
        //public double MeanSquaredError;   // Not implemented
    }
}
