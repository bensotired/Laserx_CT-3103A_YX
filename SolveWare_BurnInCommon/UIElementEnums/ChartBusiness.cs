using LX_BurnInSolution.Utilities;
using System;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace SolveWare_BurnInCommon
{

    public static class ChartBusiness
    {
        public static float Get_Max_Y_ValueInChartSeries(Chart sourceChart, AxisType yAxisType)
        {
            float maxY = float.MinValue;

            if (sourceChart.Series == null || sourceChart.Series.Count == 0)
            {
                maxY = 0.5f;
            }
            else
            {
                foreach (var series in sourceChart.Series)
                {
                    if (series.YAxisType == yAxisType)
                    {
                        var tempMaxY = series.Points.Max(new Func<DataPoint, float>((dp) => { return Convert.ToSingle(dp.YValues.Max()); }));
                        if (tempMaxY > maxY)
                        {
                            maxY = tempMaxY;
                        }
                    }
                }
            }

            if (JuniorMath.IsEdgeValue(maxY))
            {
                maxY = 0.5f;
            }

            return maxY;
        }
        public static float Get_Min_Y_ValueInChartSeries(Chart sourceChart, AxisType yAxisType)
        {
            float minY = float.MaxValue;

            if (sourceChart.Series == null || sourceChart.Series.Count == 0)
            {
                minY = 0.0f;
            }
            else
            {
                foreach (var series in sourceChart.Series)
                {
                    if (series.YAxisType == yAxisType)
                    {
                        var tempMinY = series.Points.Min(new Func<DataPoint, float>((dp) => { return Convert.ToSingle(dp.YValues.Min()); }));
                        if (tempMinY < minY)
                        {
                            minY = tempMinY;
                        }
                    }
                }
            }

            if (JuniorMath.IsEdgeValue(minY))
            {
                minY = 0.0f;
            }

            return minY;
        }
        public static float Get_Max_X_ValueInChartSeries(Chart sourceChart, AxisType xAxisType)
        {
            float maxX = float.MinValue;

            if (sourceChart.Series == null || sourceChart.Series.Count == 0)
            {
                maxX = 0.5f;
            }
            else
            {
                foreach (var series in sourceChart.Series)
                {
                    if (series.XAxisType == xAxisType)
                    {
                        var tempMaxX = series.Points.Max(new Func<DataPoint, float>((dp) => { return Convert.ToSingle(dp.XValue ); }));
                        if (tempMaxX > maxX)
                        {
                            maxX = tempMaxX;
                        }
                    }
                }
            }

            if (JuniorMath.IsEdgeValue(maxX))
            {
                maxX = 0.5f;
            }

            return maxX;
        }
        public static float Get_Min_X_ValueInChartSeries(Chart sourceChart, AxisType xAxisType)
        {
            float minX = float.MaxValue;

            if (sourceChart.Series == null || sourceChart.Series.Count == 0)
            {
                minX = 0.0f;
            }
            else
            {
                foreach (var series in sourceChart.Series)
                {
                    if (series.XAxisType == xAxisType)
                    {
                        var tempMinX = series.Points.Min(new Func<DataPoint, float>((dp) => { return Convert.ToSingle(dp.XValue); }));
                        if (tempMinX < minX)
                        {
                            minX = tempMinX;
                        }
                    }
                }
            }

            if (JuniorMath.IsEdgeValue(minX))
            {
                minX = 0.0f;
            }

            return minX;
        }
    }
}