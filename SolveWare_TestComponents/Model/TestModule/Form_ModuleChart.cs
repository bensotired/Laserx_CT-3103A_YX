using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SolveWare_TestComponents.Model
{
    public partial class Form_ModuleChart : Form, IForm_ModuleChart
    {
        public Form_ModuleChart()
        {
            InitializeComponent();
        }
        public void SaveChart(string path)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }
                this.chart_module.SaveImage(path, ChartImageFormat.Png);
            }
            catch
            {

            }
        }
        public void ClearChart()
        {
            this.chart_module.Series.Clear();
        }
        public void UpdateTitle(string title)
        {
            this.chart_module.Titles.Clear();
            this.chart_module.Titles.Add(title);
        }
        public void UpdateChartSeries(AxisType axisType, string legendName, IEnumerable<object> xData, IEnumerable<object> yData)
        {
            this.AddSeries(this.chart_module, axisType, legendName, xData, yData);
        }
        void AddSeries(Chart chart, AxisType axisType, string legendName, IEnumerable<object> xData, IEnumerable<object> yData)
        {
            try
            {
                double maxY = 0.0;
                double minY = 0.0;              

                Series _series = new Series(legendName);
                //_series.ChartType = SeriesChartType.Spline;
                _series.ChartType = SeriesChartType.Line;
                _series.YAxisType = axisType;
                _series.XValueType = ChartValueType.Auto;
                _series.YValueType = ChartValueType.Auto;
            
                _series.Points.DataBindXY(xData, yData);
         

              

                _series.IsVisibleInLegend = true;
                chart.Series.Add(_series);
           
                switch (axisType)
                {
                    case AxisType.Primary:
                        {                          
                            #region 主Y

                            chart.ChartAreas[0].AxisY.LabelStyle.Format = "f3";
                            List<object> yTemp = new List<object>(yData);

                            maxY = Convert.ToDouble(yTemp.Max());
                            minY = Convert.ToDouble(yTemp.Min());

                            if (JuniorMath.IsEdgeValue(maxY) || JuniorMath.IsEdgeValue(minY))
                            {
                                return;
                            }
                            if (maxY == minY)
                            {
                                maxY = minY + 0.5;
                                minY = minY - 0.5;
                            }
                            else
                            {
                                maxY += 0.002 * maxY;
                                minY -= 0.002 * minY;
                            }

                            #region goddamn it Y
                            var currentMaxY = ChartBusiness.Get_Max_Y_ValueInChartSeries(chart, AxisType.Primary);

                            if (double.IsNaN(chart.ChartAreas[0].AxisY.Maximum))
                            {
                                chart.ChartAreas[0].AxisY.Maximum = maxY;
                            }
                            else
                            {

                                chart.ChartAreas[0].AxisY.Maximum = Math.Max(maxY, currentMaxY);
                            }

                            var currentMinY = ChartBusiness.Get_Min_Y_ValueInChartSeries(chart, AxisType.Primary);

                            if (double.IsNaN(chart.ChartAreas[0].AxisY.Minimum))
                            {
                                chart.ChartAreas[0].AxisY.Minimum = minY;
                            }
                            else
                            {


                                chart.ChartAreas[0].AxisY.Minimum = Math.Min(minY, currentMinY);
                            }
                            #endregion
                            #endregion
                        }
                        break;
                    case AxisType.Secondary:
                        {
                            chart.ChartAreas[0].AxisY2.LabelStyle.Format = "f3";
                            List<object> yTemp = new List<object>(yData);

                            maxY = Convert.ToDouble(yTemp.Max());
                            minY = Convert.ToDouble(yTemp.Min());

                            if (JuniorMath.IsEdgeValue(maxY) || JuniorMath.IsEdgeValue(minY))
                            {
                                return;
                            }

                            if (maxY == minY)
                            {
                                maxY = minY + 0.5;
                                minY = minY - 0.5;
                            }
                            else
                            {
                                maxY += 0.002 * maxY;
                                minY -= 0.002 * minY;
                            }
                            #region goddamn it Y2
                            var currentMaxY = ChartBusiness.Get_Max_Y_ValueInChartSeries(chart, AxisType.Secondary);

                            if (double.IsNaN(chart.ChartAreas[0].AxisY2.Maximum))
                            {
                                chart.ChartAreas[0].AxisY2.Maximum = maxY;
                            }
                            else
                            {

                                chart.ChartAreas[0].AxisY2.Maximum = Math.Max(maxY, currentMaxY);
                            }

                            var currentMinY = ChartBusiness.Get_Min_Y_ValueInChartSeries(chart, AxisType.Secondary);

                            if (double.IsNaN(chart.ChartAreas[0].AxisY2.Minimum))
                            {
                                chart.ChartAreas[0].AxisY2.Minimum = minY;
                            }
                            else
                            {
                                chart.ChartAreas[0].AxisY2.Minimum = Math.Min(minY, currentMinY);
                            }
                            #endregion

                        }
                        break;
                }
                //刷新x轴上下限

                chart.ChartAreas[0].AxisX.Minimum = ChartBusiness.Get_Min_X_ValueInChartSeries(chart, AxisType.Primary);
                chart.ChartAreas[0].AxisX.Maximum = ChartBusiness.Get_Max_X_ValueInChartSeries(chart, AxisType.Primary);



            }
            catch (Exception ex)
            {

            }
        }
    }
}