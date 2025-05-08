using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Serialization;

namespace SolveWare_TestComponents.Model
{
    public partial class Form_ZedGraphChart : Form, IForm_ModuleChart
    {
        public Form_ZedGraphChart()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Y轴
        /// </summary>
        /// <param name="titleName"></param>
        /// <param name="xAxisName"></param>
        /// <param name="xData"></param>
        /// <param name="yDatas">Y轴名称+Y轴数据</param>
        /// <param name="yCurveParams">Y轴参数</param>
        public void UpdateDataToChart(string titleName, string xAxisName, double[] xData, IDictionary<string, double[]> yDatas, CurveParam[] yCurveParams)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateChart(this.zedGraph_Chart, titleName, xAxisName, xData, yDatas, yCurveParams);
                }));
            }
            else
            {
                UpdateChart(this.zedGraph_Chart, titleName, xAxisName, xData, yDatas, yCurveParams);
            }
        }

        public void UpdateDataToChart(string titleName, string xAxisName, IList<double[]> xDatas, IDictionary<string, double[]> yDatas, CurveParam[] yCurveParams)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateChart(this.zedGraph_Chart, titleName, xAxisName, xDatas, yDatas, yCurveParams);
                }));
            }
            else
            {
                UpdateChart(this.zedGraph_Chart, titleName, xAxisName, xDatas, yDatas, yCurveParams);
            }
        }






        /// <summary>
        /// Y轴+Y2轴
        /// </summary>
        /// <param name="titleName"></param>
        /// <param name="xAxisName"></param>
        /// <param name="xData"></param>
        /// <param name="yDatas">Y轴名称+Y轴数据</param>
        /// <param name="yCurveParams">Y轴参数</param>
        /// <param name="y2Datas">Y2轴名称+Y2轴数据</param>
        /// <param name="y2CurveParams">Y2轴参数</param>
        public void UpdateDataToChart(string titleName, string xAxisName, double[] xData, IDictionary<string, double[]> yDatas, CurveParam[] yCurveParams, IDictionary<string, double[]> y2Datas, CurveParam[] y2CurveParams)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateChart(this.zedGraph_Chart, titleName, xAxisName, xData, yDatas, yCurveParams, y2Datas, y2CurveParams);
                }));
            }
            else
            {
                UpdateChart(this.zedGraph_Chart, titleName, xAxisName, xData, yDatas, yCurveParams, y2Datas, y2CurveParams);
            }
        }



        public void UpdateDataToChart(string titleName, string xAxisName, IList<double[]> xDatas, IDictionary<string, double[]> yDatas, CurveParam[] yCurveParams, IDictionary<string, double[]> y2Datas, CurveParam[] y2CurveParams)
        {

            UpdateChart(this.zedGraph_Chart, titleName, xAxisName, xDatas, yDatas, yCurveParams, y2Datas, y2CurveParams);

        }








        void UpdateChart(ZedGraph.ZedGraphControl chart, string titleName, string xAxisName, double[] xData, IDictionary<string, double[]> yDatas, CurveParam[] yCurveParams)
        {
            const float FontSize = 9f;
            const float LegendFontSize = 6f;

            chart.GraphPane.Title.Text = titleName;
            chart.GraphPane.Title.FontSpec.Size = FontSize + 2;

            //chart.GraphPane.XAxis.Type = ZedGraph.AxisType.Text;
            chart.GraphPane.XAxis.Title.Text = xAxisName;
            chart.GraphPane.XAxis.Title.FontSpec.Size = FontSize;
            chart.GraphPane.XAxis.Scale.FontSpec.Size = FontSize;
            chart.GraphPane.XAxis.MajorGrid.IsVisible = true;

            //chart.GraphPane.XAxis.MinorGrid.IsVisible = true;     
            //chart.GraphPane.XAxis.Scale.MinorStepAuto = true;
            //chart.GraphPane.XAxis.Scale.MajorStepAuto = true;
            //chart.GraphPane.XAxis.Scale.MinorStep = 20;
            //chart.GraphPane.XAxis.Scale.MajorStep = 100;
            //chart.GraphPane.XAxis.MajorTic.IsOpposite = false;       

            chart.GraphPane.Legend.FontSpec.Size = LegendFontSize;
            //chart.GraphPane.Legend.Position = ZedGraph.LegendPos.Top;

            chart.GraphPane.CurveList.Clear();
            chart.GraphPane.YAxisList.Clear();
            chart.GraphPane.Y2AxisList.Clear();

            UpdateChartCurve(chart, xAxisName, xData, yDatas, yCurveParams);

            chart.AxisChange();
            chart.Invalidate();
            chart.Refresh();
        }

        void UpdateChart(ZedGraph.ZedGraphControl chart, string titleName, string xAxisName, double[] xData, IDictionary<string, double[]> yDatas, CurveParam[] yCurveParams, IDictionary<string, double[]> y2Datas, CurveParam[] y2CurveParams)
        {
            const float FontSize = 9f;
            const float LegendFontSize = 6f;

            chart.GraphPane.Title.Text = titleName;
            chart.GraphPane.Title.FontSpec.Size = FontSize + 2;

            //chart.GraphPane.XAxis.Type = ZedGraph.AxisType.Text;
            chart.GraphPane.XAxis.Title.Text = xAxisName;
            chart.GraphPane.XAxis.Title.FontSpec.Size = FontSize;
            chart.GraphPane.XAxis.Scale.FontSpec.Size = FontSize;
            chart.GraphPane.XAxis.MajorGrid.IsVisible = true;
            //chart.GraphPane.XAxis.MinorGrid.IsVisible = true;     
            //chart.GraphPane.XAxis.Scale.MinorStepAuto = true;
            //chart.GraphPane.XAxis.Scale.MajorStepAuto = true;
            //chart.GraphPane.XAxis.Scale.MinorStep = 20;
            //chart.GraphPane.XAxis.Scale.MajorStep = 100;
            //chart.GraphPane.XAxis.MajorTic.IsOpposite = false;       

            chart.GraphPane.Legend.FontSpec.Size = LegendFontSize;
            //chart.GraphPane.Legend.Position = ZedGraph.LegendPos.Top;

            chart.GraphPane.CurveList.Clear();
            chart.GraphPane.YAxisList.Clear();
            chart.GraphPane.Y2AxisList.Clear();

            UpdateChartCurve(chart, xAxisName, xData, yDatas, yCurveParams, y2Datas, y2CurveParams);

            chart.AxisChange();
            chart.Invalidate();
            chart.Refresh();
        }


        void UpdateChart(ZedGraph.ZedGraphControl chart, string titleName, string xAxisName, IList<double[]> xDatas, IDictionary<string, double[]> yDatas, CurveParam[] yCurveParams)
        {
            const float FontSize = 9f;
            const float LegendFontSize = 6f;

            chart.GraphPane.Title.Text = titleName;
            chart.GraphPane.Title.FontSpec.Size = FontSize + 2;

            //chart.GraphPane.XAxis.Type = ZedGraph.AxisType.Text;
            chart.GraphPane.XAxis.Title.Text = xAxisName;
            chart.GraphPane.XAxis.Title.FontSpec.Size = FontSize;
            chart.GraphPane.XAxis.Scale.FontSpec.Size = FontSize;
            chart.GraphPane.XAxis.MajorGrid.IsVisible = true;

            //chart.GraphPane.XAxis.MinorGrid.IsVisible = true;     
            //chart.GraphPane.XAxis.Scale.MinorStepAuto = true;
            //chart.GraphPane.XAxis.Scale.MajorStepAuto = true;
            //chart.GraphPane.XAxis.Scale.MinorStep = 20;
            //chart.GraphPane.XAxis.Scale.MajorStep = 100;
            //chart.GraphPane.XAxis.MajorTic.IsOpposite = false;       

            chart.GraphPane.Legend.FontSpec.Size = LegendFontSize;
            //chart.GraphPane.Legend.Position = ZedGraph.LegendPos.Top;

            chart.GraphPane.CurveList.Clear();
            chart.GraphPane.YAxisList.Clear();
            chart.GraphPane.Y2AxisList.Clear();

            UpdateChartCurve(chart, xAxisName, xDatas, yDatas, yCurveParams);

            chart.AxisChange();
            chart.Invalidate();
            chart.Refresh();
        }

        void UpdateChart(ZedGraph.ZedGraphControl chart, string titleName, string xAxisName, IList<double[]> xDatas, IDictionary<string, double[]> yDatas, CurveParam[] yCurveParams, IDictionary<string, double[]> y2Datas, CurveParam[] y2CurveParams)
        {
            const float FontSize = 9f;
            const float LegendFontSize = 6f;

            chart.GraphPane.Title.Text = titleName;
            chart.GraphPane.Title.FontSpec.Size = FontSize + 2;

            //chart.GraphPane.XAxis.Type = ZedGraph.AxisType.Text;
            chart.GraphPane.XAxis.Title.Text = xAxisName;
            chart.GraphPane.XAxis.Title.FontSpec.Size = FontSize;
            chart.GraphPane.XAxis.Scale.FontSpec.Size = FontSize;
            chart.GraphPane.XAxis.MajorGrid.IsVisible = true;
            //chart.GraphPane.XAxis.MinorGrid.IsVisible = true;     
            //chart.GraphPane.XAxis.Scale.MinorStepAuto = true;
            //chart.GraphPane.XAxis.Scale.MajorStepAuto = true;
            //chart.GraphPane.XAxis.Scale.MinorStep = 20;
            //chart.GraphPane.XAxis.Scale.MajorStep = 100;
            //chart.GraphPane.XAxis.MajorTic.IsOpposite = false;       

            chart.GraphPane.Legend.FontSpec.Size = LegendFontSize;
            //chart.GraphPane.Legend.Position = ZedGraph.LegendPos.Top;

            chart.GraphPane.CurveList.Clear();
            chart.GraphPane.YAxisList.Clear();
            chart.GraphPane.Y2AxisList.Clear();

            UpdateChartCurve(chart, xAxisName, xDatas, yDatas, yCurveParams, y2Datas, y2CurveParams);

            chart.AxisChange();
            chart.Invalidate();
            chart.Refresh();
        }







        //同X数据
        //X  Y
        void UpdateChartCurve(ZedGraph.ZedGraphControl chart, string xAxisName, double[] xData, IDictionary<string, double[]> yDatas, CurveParam[] yCurveParams)
        {
            if (yDatas.Count != yCurveParams.Length)
            {
                throw new Exception("Y曲线参数数量不匹配！");
            }
            int i = 0;
            foreach (var yData in yDatas)
            {
                var curveParam = yCurveParams[i];
                int axisIndex = chart.GraphPane.AddYAxis(yData.Key);
                var y_Axis = chart.GraphPane.YAxisList[axisIndex];
                y_Axis.Color = curveParam.CurveColor;
                //y_Axis.Title.Text = YAxesValue.Key;
                y_Axis.Title.FontSpec.FontColor = curveParam.CurveColor;
                y_Axis.Title.FontSpec.Size = curveParam.FontSize;
                //y_Axis.Title.IsOmitMag = true;  
                y_Axis.Scale.FontSpec.FontColor = curveParam.CurveColor;
                y_Axis.Scale.FontSpec.Size = curveParam.FontSize;
                y_Axis.Scale.MaxAuto = curveParam.IsMaxAuto;
                if (y_Axis.Scale.MaxAuto == false)
                {
                    y_Axis.Scale.Max = curveParam.MaxValue;
                }
                y_Axis.Scale.MinAuto = curveParam.IsMinAuto;
                if (y_Axis.Scale.MinAuto == false)
                {
                    y_Axis.Scale.Min = curveParam.MinValue;
                }
                y_Axis.Scale.IsUseTenPower = false;
                y_Axis.Scale.MagAuto = false;
                string curveName = $"{yData.Key} vs {xAxisName}";
                var curve = chart.GraphPane.AddCurve(curveName, xData, yData.Value, curveParam.CurveColor, ZedGraph.SymbolType.None);
                curve.Line.IsAntiAlias = true;
                if (curveParam.IsBindYAxis == false)
                {
                    curve.YAxisIndex = axisIndex;
                }
                i++;
            }
            if (chart.GraphPane.YAxisList.Count > 0)
            {
                chart.GraphPane.YAxis.MajorGrid.IsVisible = true;
                chart.GraphPane.YAxis.MajorTic.IsOpposite = false;
            }
        }

        //不同X数据
        //X  Y
        void UpdateChartCurve(ZedGraph.ZedGraphControl chart, string xAxisName, IList<double[]> xDatas, IDictionary<string, double[]> yDatas, CurveParam[] yCurveParams)
        {
            if (xDatas.Count != yDatas.Count || yDatas.Count != yCurveParams.Length)
            {
                throw new Exception("XY曲线参数数量不匹配！");
            }
            int i = 0;
            foreach (var yData in yDatas)
            {
                var curveParam = yCurveParams[i];
                int axisIndex = chart.GraphPane.AddYAxis(yData.Key);
                var y_Axis = chart.GraphPane.YAxisList[axisIndex];
                y_Axis.Color = curveParam.CurveColor;
                //y_Axis.Title.Text = YAxesValue.Key;
                y_Axis.Title.FontSpec.FontColor = curveParam.CurveColor;
                y_Axis.Title.FontSpec.Size = curveParam.FontSize;
                //y_Axis.Title.IsOmitMag = true;  
                y_Axis.Scale.FontSpec.FontColor = curveParam.CurveColor;
                y_Axis.Scale.FontSpec.Size = curveParam.FontSize;
                y_Axis.Scale.MaxAuto = curveParam.IsMaxAuto;
                if (y_Axis.Scale.MaxAuto == false)
                {
                    y_Axis.Scale.Max = curveParam.MaxValue;
                }
                y_Axis.Scale.MinAuto = curveParam.IsMinAuto;
                if (y_Axis.Scale.MinAuto == false)
                {
                    y_Axis.Scale.Min = curveParam.MinValue;
                }
                y_Axis.Scale.IsUseTenPower = false;
                y_Axis.Scale.MagAuto = false;
                string curveName = $"{yData.Key} vs {xAxisName}";
                var curve = chart.GraphPane.AddCurve(curveName, xDatas[i], yData.Value, curveParam.CurveColor, ZedGraph.SymbolType.None);
                curve.Line.IsAntiAlias = true;
                if (curveParam.IsBindYAxis == false)
                {
                    curve.YAxisIndex = axisIndex;
                }
                i++;
            }
            if (chart.GraphPane.YAxisList.Count > 0)
            {
                chart.GraphPane.YAxis.MajorGrid.IsVisible = true;
                chart.GraphPane.YAxis.MajorTic.IsOpposite = false;
            }
        }



        //同X轴数据
        //X  Y  Y2
        void UpdateChartCurve(ZedGraph.ZedGraphControl chart, string xAxisName, double[] xData, IDictionary<string, double[]> yDatas, CurveParam[] yCurveParams, IDictionary<string, double[]> y2Datas, CurveParam[] y2CurveParams)
        {
            if (yDatas.Count != yCurveParams.Length)
            {
                throw new Exception("Y曲线参数数量不匹配！");
            }

            if (y2Datas.Count != y2CurveParams.Length)
            {
                throw new Exception("Y2曲线参数数量不匹配！");
            }

            int i = 0;
            foreach (var yData in yDatas)
            {
                var curveParam = yCurveParams[i];
                int axisIndex = chart.GraphPane.AddYAxis(yData.Key);
                var y_Axis = chart.GraphPane.YAxisList[axisIndex];
                y_Axis.Color = curveParam.CurveColor;
                //y_Axis.Title.Text = YAxesValue.Key;
                y_Axis.Title.FontSpec.FontColor = curveParam.CurveColor;
                y_Axis.Title.FontSpec.Size = curveParam.FontSize;
                //y_Axis.Title.IsOmitMag = true;  
                y_Axis.Scale.FontSpec.FontColor = curveParam.CurveColor;
                y_Axis.Scale.FontSpec.Size = curveParam.FontSize;
                y_Axis.Scale.MaxAuto = curveParam.IsMaxAuto;
                if (y_Axis.Scale.MaxAuto == false)
                {
                    y_Axis.Scale.Max = curveParam.MaxValue;
                }
                y_Axis.Scale.MinAuto = curveParam.IsMinAuto;
                if (y_Axis.Scale.MinAuto == false)
                {
                    y_Axis.Scale.Min = curveParam.MinValue;
                }
                y_Axis.Scale.IsUseTenPower = false;
                y_Axis.Scale.MagAuto = false;
                string curveName = $"{yData.Key} vs {xAxisName}";
                var curve = chart.GraphPane.AddCurve(curveName, xData, yData.Value, curveParam.CurveColor, ZedGraph.SymbolType.None);
                curve.Line.IsAntiAlias = true;
                if (curveParam.IsBindYAxis == false)
                {
                    curve.YAxisIndex = axisIndex;
                }
                i++;
            }
            if (chart.GraphPane.YAxisList.Count > 0)
            {
                chart.GraphPane.YAxis.MajorGrid.IsVisible = true;
                chart.GraphPane.YAxis.MajorTic.IsOpposite = false;
            }

            i = 0;
            foreach (var yData in y2Datas)
            {
                var curveParam = y2CurveParams[i];
                int axisIndex = chart.GraphPane.AddY2Axis(yData.Key);
                var y_Axis = chart.GraphPane.Y2AxisList[axisIndex];
                y_Axis.Color = curveParam.CurveColor;
                //y_Axis.Title.Text = YAxesValue.Key;
                y_Axis.Title.FontSpec.FontColor = curveParam.CurveColor;
                y_Axis.Title.FontSpec.Size = curveParam.FontSize;
                //y_Axis.Title.IsOmitMag = true;  
                y_Axis.Scale.FontSpec.FontColor = curveParam.CurveColor;
                y_Axis.Scale.FontSpec.Size = curveParam.FontSize;
                y_Axis.Scale.MaxAuto = curveParam.IsMaxAuto;
                if (y_Axis.Scale.MaxAuto == false)
                {
                    y_Axis.Scale.Max = curveParam.MaxValue;
                }
                y_Axis.Scale.MinAuto = curveParam.IsMinAuto;
                if (y_Axis.Scale.MinAuto == false)
                {
                    y_Axis.Scale.Min = curveParam.MinValue;
                }
                y_Axis.Scale.IsUseTenPower = false;
                y_Axis.Scale.MagAuto = false;
                y_Axis.IsVisible = true;
                string curveName = $"{yData.Key} vs {xAxisName}";
                var curve = chart.GraphPane.AddCurve(curveName, xData, yData.Value, curveParam.CurveColor, ZedGraph.SymbolType.None);
                curve.IsY2Axis = true;
                curve.Line.IsAntiAlias = true;
                if (curveParam.IsBindYAxis == false)
                {
                    curve.YAxisIndex = axisIndex;
                }
                i++;
            }
            if (chart.GraphPane.Y2AxisList.Count > 0)
            {
                chart.GraphPane.Y2Axis.MajorGrid.IsVisible = true;
                chart.GraphPane.Y2Axis.MajorTic.IsOpposite = false;
            }
        }


        //不同X轴数据
        //X  Y  Y2
        void UpdateChartCurve(ZedGraph.ZedGraphControl chart, string xAxisName, IList<double[]> xDatas, IDictionary<string, double[]> yDatas, CurveParam[] yCurveParams, IDictionary<string, double[]> y2Datas, CurveParam[] y2CurveParams)
        {
            if (yDatas.Count != yCurveParams.Length)
            {
                throw new Exception("Y曲线参数数量不匹配！");
            }

            if (y2Datas.Count != y2CurveParams.Length)
            {
                throw new Exception("Y2曲线参数数量不匹配！");
            }

            if (xDatas.Count != (yDatas.Count + y2Datas.Count))
            {
                throw new Exception("X曲线参数数量不匹配！");
            }




            int i = 0;
            foreach (var yData in yDatas)
            {
                var curveParam = yCurveParams[i];
                int axisIndex = chart.GraphPane.AddYAxis(yData.Key);
                var y_Axis = chart.GraphPane.YAxisList[axisIndex];
                y_Axis.Color = curveParam.CurveColor;
                //y_Axis.Title.Text = YAxesValue.Key;
                y_Axis.Title.FontSpec.FontColor = curveParam.CurveColor;
                y_Axis.Title.FontSpec.Size = curveParam.FontSize;
                //y_Axis.Title.IsOmitMag = true;  
                y_Axis.Scale.FontSpec.FontColor = curveParam.CurveColor;
                y_Axis.Scale.FontSpec.Size = curveParam.FontSize;
                y_Axis.Scale.MaxAuto = curveParam.IsMaxAuto;
                if (y_Axis.Scale.MaxAuto == false)
                {
                    y_Axis.Scale.Max = curveParam.MaxValue;
                }
                y_Axis.Scale.MinAuto = curveParam.IsMinAuto;
                if (y_Axis.Scale.MinAuto == false)
                {
                    y_Axis.Scale.Min = curveParam.MinValue;
                }
                y_Axis.Scale.IsUseTenPower = false;
                y_Axis.Scale.MagAuto = false;
                string curveName = $"{yData.Key} vs {xAxisName}";
                var curve = chart.GraphPane.AddCurve(curveName, xDatas[i], yData.Value, curveParam.CurveColor, ZedGraph.SymbolType.None);
                curve.Line.IsAntiAlias = true;
                if (curveParam.IsBindYAxis == false)
                {
                    curve.YAxisIndex = axisIndex;
                }
                i++;
            }

            var count = chart.GraphPane.YAxisList.Count;
            if (count > 0)
            {
                chart.GraphPane.YAxis.MajorGrid.IsVisible = true;
                chart.GraphPane.YAxis.MajorTic.IsOpposite = false;
            }

            i = 0;
            foreach (var yData in y2Datas)
            {
                var curveParam = y2CurveParams[i];
                int axisIndex = chart.GraphPane.AddY2Axis(yData.Key);
                var y_Axis = chart.GraphPane.Y2AxisList[axisIndex];
                y_Axis.Color = curveParam.CurveColor;
                //y_Axis.Title.Text = YAxesValue.Key;
                y_Axis.Title.FontSpec.FontColor = curveParam.CurveColor;
                y_Axis.Title.FontSpec.Size = curveParam.FontSize;
                //y_Axis.Title.IsOmitMag = true;  
                y_Axis.Scale.FontSpec.FontColor = curveParam.CurveColor;
                y_Axis.Scale.FontSpec.Size = curveParam.FontSize;
                y_Axis.Scale.MaxAuto = curveParam.IsMaxAuto;
                if (y_Axis.Scale.MaxAuto == false)
                {
                    y_Axis.Scale.Max = curveParam.MaxValue;
                }
                y_Axis.Scale.MinAuto = curveParam.IsMinAuto;
                if (y_Axis.Scale.MinAuto == false)
                {
                    y_Axis.Scale.Min = curveParam.MinValue;
                }
                y_Axis.Scale.IsUseTenPower = false;
                y_Axis.Scale.MagAuto = false;
                y_Axis.IsVisible = true;
                string curveName = $"{yData.Key} vs {xAxisName}";
                var curve = chart.GraphPane.AddCurve(curveName, xDatas[i + count], yData.Value, curveParam.CurveColor, ZedGraph.SymbolType.None);
                curve.IsY2Axis = true;
                curve.Line.IsAntiAlias = true;
                if (curveParam.IsBindYAxis == false)
                {
                    curve.YAxisIndex = axisIndex;
                }
                i++;
            }
            if (chart.GraphPane.Y2AxisList.Count > 0)
            {
                chart.GraphPane.Y2Axis.MajorGrid.IsVisible = true;
                chart.GraphPane.Y2Axis.MajorTic.IsOpposite = false;
            }
        }



        public void ClearChart()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    Clear(this.zedGraph_Chart);
                }));
            }
            else
            {
                Clear(this.zedGraph_Chart);
            }
        }


        void Clear(ZedGraph.ZedGraphControl chart)
        {
            chart.GraphPane.CurveList.Clear();
            chart.GraphPane.YAxisList.Clear();
            chart.GraphPane.Y2AxisList.Clear();
            chart.GraphPane.GraphObjList.Clear();
            chart.GraphPane.Title.Text = "";

            chart.AxisChange();
            chart.Invalidate();
            chart.Refresh();
        }




        public void UpdateChartSeries(AxisType axisType, string legendName, IEnumerable<object> xData, IEnumerable<object> yData)
        {

        }

        public void UpdateTitle(string title)
        {

        }
    }
    public struct CurveParam
    {
        [XmlIgnore]
        public Color CurveColor { get; set; }

        [XmlIgnore]
        public float FontSize { get; set; }

        /// <summary>
        /// Max量程自动
        /// </summary>
        public bool IsMaxAuto { get; set; }             //w

        /// <summary>
        /// 不启用自动，MaxValue
        /// </summary>
        public double MaxValue { get; set; }            //w

        /// <summary>
        ///  Min量程自动
        /// </summary>
        public bool IsMinAuto { get; set; }             //w       

        /// <summary>
        /// 不启用自动，MinValue
        /// </summary>
        public double MinValue { get; set; }            //w

        ///// <summary>
        ///// 平滑
        ///// </summary>
        //public bool IsAntiAlias { get; set; }
        /// <summary>
        /// 其它Y/Y2轴是否绑定第一条Y/Y2轴
        /// </summary>
        public bool IsBindYAxis { get; set; }           //w

    }
}