using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LX_BurnInSolution.Utilities
{
    ///<summary>
    /// The Interpolator class defines DoPiecewiseLinearInterpolation()
    /// and several overrides.  All are static functions to do
    /// interpolation on piecewise linear functions.  All the functions
    /// extrapolate as well as they interpolate.  The data points don't
    /// have to be in order.
    ///</summary>
    public static class Interpolator
    {
        #region Methods
        ///<summary>
        /// Interpolates on a curve in two space defined by the x and y
        /// data.  Calculates the y value for the given x.
        ///</summary>
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

        ///<summary>
        /// Interpolates on a curve in two space defined by the x and y
        /// data.  Calculate all the y values for the given x values.
        /// The new x values can be an array of any type that implements
        /// the IConvertable interface.
        ///</summary>
        public static double[] DoPiecewiseLinearInterpolation(Array xPointsToInterpolate, double[] xData, double[] yData)
        {
            // Just convert to double. No data is being lost by this conversion
            double[] newYs = new double[xPointsToInterpolate.Length];
            for (int i = 0; i < xPointsToInterpolate.Length; i++)
            {
                double newX = Convert.ToDouble(xPointsToInterpolate.GetValue(i));
                newYs[i] = DoPiecewiseLinearInterpolation(newX, xData, yData);
            }
            return newYs;
        }
        ///<summary>
        /// Interpolates on a curve in two space defined by the x and y
        /// data.  Calculate all the y values for the given x values.
        ///</summary>
        public static double[] DoPiecewiseLinearInterpolation(double[] xPointsToInterpolate, double[] xData, double[] yData)
        {
            double[] newYs = new double[xPointsToInterpolate.Length];

            for (int i = 0; i < xPointsToInterpolate.Length; i++)
            {
                double newX = xPointsToInterpolate[i];
                newYs[i] = DoPiecewiseLinearInterpolation(newX, xData, yData);
            }
            return newYs;
        }

        ///<summary>
        /// Interpolates on a surface in 3 space defined by a 2D table.
        /// The From row and column headers define the x and y values.
        /// The maxtrix defines the z values.  The points on the
        /// surfaces are (X[i], Y[j], Z[i][j]).  The function returns
        /// new matrix of z values for the new x and y values in the To
        /// arrays.  The z values are found by bi-linear interpolation.
        ///</summary>
        public static double[][] DoPiecewiseLinearInterpolation(double[] colHeadersToInterpolateTo, double[] rowHeadersToInterpolateTo, double[][] matrixToInterpolate, double[] colHeadersToInterpolateFrom, double[] rowHeadersToInterpolateFrom)
        {
            int rowIndex = 0;
            int colIndex = 0;
            double[][] colInterpolatedMatrix = new double[matrixToInterpolate.Length][];
            foreach (double[] row in matrixToInterpolate)
            {
                colInterpolatedMatrix[rowIndex] = Interpolator.DoPiecewiseLinearInterpolation(colHeadersToInterpolateTo, colHeadersToInterpolateFrom, row);
                rowIndex++;
            }

            //transpose the matrix
            double[][] transposedColInterpolatedMatrix = new double[colInterpolatedMatrix[0].Length][];

            colIndex = 0;
            for (colIndex = 0; colIndex < transposedColInterpolatedMatrix.Length; colIndex++)
            {
                double[] colToAdd = new double[colInterpolatedMatrix.Length];
                rowIndex = 0;	//reset the rowIndex
                foreach (double[] row in colInterpolatedMatrix)
                {
                    colToAdd[rowIndex] = row[colIndex];
                    rowIndex++;
                }
                transposedColInterpolatedMatrix[colIndex] = colToAdd;
            }

            //interpolating all the columns
            double[][] rowInterpolatedTransposedColInterpolatedMatrix = new double[transposedColInterpolatedMatrix.Length][];
            colIndex = 0;
            foreach (double[] col in transposedColInterpolatedMatrix)
            {
                rowInterpolatedTransposedColInterpolatedMatrix[colIndex] = Interpolator.DoPiecewiseLinearInterpolation(rowHeadersToInterpolateTo, rowHeadersToInterpolateFrom, col);
                colIndex++;
            }

            //tranposed the full impterpolated matrix back to row representation
            double[][] interpolatedMatrix = new double[rowHeadersToInterpolateTo.Length][];
            for (rowIndex = 0; rowIndex < rowHeadersToInterpolateTo.Length; rowIndex++)
            {
                double[] rowToAdd = new double[colHeadersToInterpolateTo.Length];
                colIndex = 0;
                foreach (double[] col in rowInterpolatedTransposedColInterpolatedMatrix)
                {
                    rowToAdd[colIndex] = col[rowIndex];
                    colIndex++;
                }
                interpolatedMatrix[rowIndex] = rowToAdd;
            }

            return interpolatedMatrix;
        }

        ///<summary>
        /// Interpolates on a surface in 3 space defined by a 2D table.
        /// Description as with the other 3 space functions.  Function
        /// returns a single value: z = f(x, y)
        ///</summary>
        public static double DoPiecewiseLinearInterpolation(double columnValueToInterpolate, double rowValueToInterpolate, double[][] matrixToInterpolate, double[] colHeadersToInterpolateFrom, double[] rowHeadersToInterpolateFrom)
        {
            double[][] interpolatedMatrix = DoPiecewiseLinearInterpolation(new double[] { columnValueToInterpolate }, new double[] { rowValueToInterpolate }, matrixToInterpolate, colHeadersToInterpolateFrom, rowHeadersToInterpolateFrom);
            double interpolateValue = interpolatedMatrix[0][0];
            return interpolateValue;
        }

        ///<summary>
        /// Interpolates on a surface in 3 space defined by a 2D table.
        /// This is just an alternate name for the DoPiecewiseLinearInterpolation()
        /// that returns a matrix.
        ///</summary>
        public static double[][] Do2DPiecewiseLinearInterpolation(double[] colHeadersToInterpolateTo, double[] rowHeadersToInterpolateTo, double[][] matrixToInterpolate, double[] colHeadersToInterpolateFrom, double[] rowHeadersToInterpolateFrom)
        {
            return DoPiecewiseLinearInterpolation(colHeadersToInterpolateTo, rowHeadersToInterpolateTo, matrixToInterpolate, colHeadersToInterpolateFrom, rowHeadersToInterpolateFrom);
        }
        #endregion Methods
    }
}
