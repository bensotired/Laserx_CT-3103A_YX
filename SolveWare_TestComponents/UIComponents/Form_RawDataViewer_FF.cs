using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SolveWare_TestComponents.UIComponents
{
    public partial class Form_RawDataViewer_FF : Form, IForm_RawDataViewer
    {
        Form_ModuleChart chartUI;
        Form_ModuleFFChart ffchartUI;
        public string  SaveImagePath { set; get; }
        public bool FFChartSaveImage_SW { set; get; } = false;
        public string SN { set; get; } = "123";

        public Form_RawDataViewer_FF()
        {
            InitializeComponent();
        }
      
        private void Form_RawDataViewer_Load(object sender, System.EventArgs e)
        {
 

            chartUI = new Form_ModuleChart();
            UIGeneric.ModifyDockableUI(chartUI, true);
            tp_DataCharts.Controls.Clear();
            tp_DataCharts.Controls.Add(chartUI);
            chartUI.Show();
          
            ffchartUI = new Form_ModuleFFChart();
            UIGeneric.ModifyDockableUI(ffchartUI, true);
            tp_FFDataCharts.Controls.Clear();
            tp_FFDataCharts.Controls.Add(ffchartUI);
            ffchartUI.Show();
            GC.Collect();
        }

        private void Form_RawDataViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            pnl_chart.Controls.Clear();
            if (this.chartUI != null && this.chartUI.IsDisposed == false)
            {
                this.chartUI.Dispose();
            }
        }
        public void ClearContext()
        {
            this.Invoke((EventHandler)delegate
            {
                try
                {
                    this.dgv_RawDataCollection.Rows.Clear();
                    this.dgv_RawDataCollection.Columns.Clear();

                    this.dgv_RawDataParameters.Rows.Clear();

                    //this.chartUI.Rest_AxisX();
                    this.chartUI.ClearChart();
                }
                catch (Exception ex)
                {

                }
            });
        }
        public void ImportRawData(IRawDataBaseLite rawData)
        {
            this.Invoke((EventHandler)delegate
            {
                try
                {
                    this.UpdateRawDataBrowsable_DGV(rawData);

                    if (rawData is IRawDataCollectionBase)
                    {
                        var rawDataCollection = (IRawDataCollectionBase)rawData;
                        if (rawDataCollection.Count <= 0)
                        {

                        }
                        else
                        {
                            this.UpdateRawDataCollection_DGV(rawDataCollection);
                            this.UpdateRawDataChart_V2(rawDataCollection);
                            //if (rawDataCollection.Name.Contains("RawData_NanoScanAnalyse"))
                            //{
                                this.UpdateRawDataChart_FF(rawDataCollection);
                            //}                           
                        }
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }
        private void UpdateRawDataBrowsable_DGV(IRawDataBaseLite rawData)
        {
            var rawProps = rawData.GetType().GetProperties();
            var bowProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(rawProps);
            foreach (var bp in bowProps)
            {
                var rowIndex = this.dgv_RawDataParameters.Rows.Add();
                this.dgv_RawDataParameters.Rows[rowIndex].SetValues(bp.Name, bp.GetValue(rawData));
            }
        
        }
        private void UpdateRawDataChart(IRawDataCollectionBase rawDataCollection)
        {
            try
            {
                Dictionary<CEAxisXY, string> legends = new Dictionary<CEAxisXY, string>();
                Dictionary<CEAxisXY, List<object>> values = new Dictionary<CEAxisXY, List<object>>();
 

                var itemProps = rawDataCollection.Peek().GetType().GetProperties();
                foreach (var prop in itemProps)
                {
                    if (PropHelper.IsPropertyBelongs<RawDataChartAxisElementAttribute>(prop))
                    {
                        var propValue = PropHelper.GetAttributeValue<RawDataChartAxisElementAttribute>(prop);
                        legends.Add(propValue.ChartAxisXY, prop.Name);
                        values.Add(propValue.ChartAxisXY, rawDataCollection.GetChartSeriesDataListByPropName(prop.Name));
                    }
                }
                if (legends.ContainsKey(CEAxisXY.X) == false)
                {
                    //没有x轴  不画图
                    return;
                }
                if (legends.ContainsKey(CEAxisXY.Y))
                {
                    var legendName = $"{legends[CEAxisXY.Y]} vs {legends[CEAxisXY.X]}";
                    this.chartUI.UpdateChartSeries(AxisType.Primary, legendName, values[CEAxisXY.X], values[CEAxisXY.Y]);
                }
                if (legends.ContainsKey(CEAxisXY.Y2))
                {
                    var legendName = $"{legends[CEAxisXY.Y2]} vs {legends[CEAxisXY.X]}";
                    this.chartUI.UpdateChartSeries(AxisType.Secondary, legendName, values[CEAxisXY.X], values[CEAxisXY.Y2]);
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void UpdateRawDataChart_V2(IRawDataCollectionBase rawDataCollection)
        {
            try
            {
                Dictionary<string, CEAxisXY> legends = new Dictionary<string, CEAxisXY>();
                Dictionary<string, List<object>> values = new Dictionary<string, List<object>>();


                var itemProps = rawDataCollection.Peek().GetType().GetProperties();
                foreach (var prop in itemProps)
                {
                    if (PropHelper.IsPropertyBelongs<RawDataChartAxisElementAttribute>(prop))
                    {
                        var propValue = PropHelper.GetAttributeValue<RawDataChartAxisElementAttribute>(prop);
                        legends.Add(prop.Name, propValue.ChartAxisXY);
                        values.Add(prop.Name, rawDataCollection.GetChartSeriesDataListByPropName(prop.Name));
                    }
                }
                if (legends.ContainsValue(CEAxisXY.X) == false)
                {
                    //没有x轴  不画图
                    return;
                }
                if (legends.Values.ToList().FindAll(item => item == CEAxisXY.X).Count > 1)
                {
                    //多个x轴  不画图
                    return;
                }
                List<object> xValues = new List<object>();
                string xName = string.Empty;
                foreach (var item in legends)
                {
                    if(item.Value == CEAxisXY.X)
                    {
                        xValues = values[item.Key];
                        xName = item.Key;
                    }
                }
                foreach(var item in legends)
                {
                    switch (item.Value)
                    {
                        case CEAxisXY.Y:
                            {
                                var legendName = $"{item.Key} vs {xName}";
                                this.chartUI.UpdateChartSeries(AxisType.Primary, legendName, xValues, values[item.Key]);
                            }
                            break;
                        case CEAxisXY.Y2:
                            {
                                var legendName = $"{item.Key} vs {xName}";
                                this.chartUI.UpdateChartSeries(AxisType.Secondary, legendName, xValues, values[item.Key]);
                            }
                            break;
                    }
                }


                //if (legends.ContainsValue(CEAxisXY.Y))
                //{

                //    var legendName = $"{legends[CEAxisXY.Y]} vs {legends[CEAxisXY.X]}";
                //    this.chartUI.UpdateChartSeries(AxisType.Primary, legendName, values[CEAxisXY.X], values[CEAxisXY.Y]);
                //}
                //if (legends.ContainsKey(CEAxisXY.Y2))
                //{
                //    var legendName = $"{legends[CEAxisXY.Y2]} vs {legends[CEAxisXY.X]}";
                //    this.chartUI.UpdateChartSeries(AxisType.Secondary, legendName, values[CEAxisXY.X], values[CEAxisXY.Y2]);
                //}

            }
            catch (Exception ex)
            {

            }
        }
        private void UpdateRawDataChart_FF(IRawDataCollectionBase rawDataCollection)
        {
            try
            {
                // (rawDataCollection as RawData_NanoScanAnalyse)
                //if (!rawDataCollection.Name.Contains("RawData_NanoScanAnalyse"))
                //{
                //    return;
                //}
                Dictionary<string, CEAxisXY> legends = new Dictionary<string, CEAxisXY>();
                // Dictionary<string, List<object>> values = new Dictionary<string, List<object>>();
                Dictionary<string, List<double>> values = new Dictionary<string, List<double>>();
                Dictionary<string, double> DicBeamWidth = new Dictionary<string, double>();

                var AllitemProps = rawDataCollection.GetType().GetProperties();
                foreach (var prop in AllitemProps)
                {
                    if (PropHelper.IsPropertyBelongs<RawDataBrowsableElementAttribute>(prop))
                    {
                        if ((prop.GetValue(rawDataCollection) is double) && (prop.Name.Contains("BeamWidth") || prop.Name.Contains("MoveDistance_mm")))
                        {
                            DicBeamWidth.Add(prop.Name, (double)prop.GetValue(rawDataCollection));
                        }
                    }
                }
                if (DicBeamWidth.Count==0)
                {
                    return;
                }
                var itemProps = rawDataCollection.Peek().GetType().GetProperties();
                /// rawDataCollection.Name=  
                foreach (var prop in itemProps)
                {
                    if (PropHelper.IsPropertyBelongs<RawDataChartAxisElementAttribute>(prop))
                    {
                        var propValue = PropHelper.GetAttributeValue<RawDataChartAxisElementAttribute>(prop);
                        legends.Add(prop.Name, propValue.ChartAxisXY);
                        //values.Add(prop.Name, rawDataCollection.GetChartSeriesDataListByPropName(prop.Name)); //GetDataListByPropName
                        values.Add(prop.Name, rawDataCollection.GetDataListByPropName(prop.Name));
                    }
                }
                //算K和B
                Double K_1st = 0; Double B_1st = 0;
                this.CalculateKB(true,DicBeamWidth["BeamWidth_13p5_X_1st"], DicBeamWidth["BeamWidth_13p5_X_2nd"], DicBeamWidth["MoveDistance_mm"], values["X_Position_1st"], values["X_Amplitude_1st"], ref K_1st, ref B_1st);
                //得到新的pos
                List<double> pos_1st_new = new List<double>();
                for (int i = 0; i < values["X_Position_1st"].Count; i++)
                {
                    pos_1st_new.Add(values["X_Position_1st"][i] * K_1st + B_1st);
                }

                Double K_2nd = 0; Double B_2nd = 0; 
                this.CalculateKB(false, DicBeamWidth["BeamWidth_13p5_X_1st"], DicBeamWidth["BeamWidth_13p5_X_2nd"], DicBeamWidth["MoveDistance_mm"], values["X_Position_1st"], values["X_Amplitude_2nd"], ref K_2nd, ref B_2nd);
                //得到新的pos
                List<double> pos_2nd_new = new List<double>(); 
                for (int i = 0; i < values["X_Position_1st"].Count; i++)
                {
                    pos_2nd_new.Add(values["X_Position_1st"][i] * K_2nd + B_2nd);
                }

                if (values.Count == 5 && values.ContainsKey("X_Position_1st") &&
                    values.ContainsKey("X_Amplitude_1st") && values.ContainsKey("Y_Amplitude_1st") && values.ContainsKey("X_Amplitude_2nd") && values.ContainsKey("Y_Amplitude_2nd"))
                {
                    if (Directory.Exists(SaveImagePath) == false)
                    {
                        Directory.CreateDirectory(SaveImagePath);
                    }
                    string SN_1st = $"{SN}_1st";// + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    //ffchartUI.Setdata(true, SN_1st, values["X_Position_1st"].ToArray(), values["X_Amplitude_1st"].ToArray(), values["Y_Amplitude_1st"].ToArray());
                    ffchartUI.Setdata(true, SN_1st, pos_1st_new.ToArray(), values["X_Amplitude_1st"].ToArray(), values["Y_Amplitude_1st"].ToArray());
                    if (FFChartSaveImage_SW)
                    {
                        string filename = SaveImagePath + $@"\{SN}_1st_{BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)}.jpg";
                        ffchartUI.SaveImage(true, filename);
                    }
                    string SN_2nd = $"{SN}_2nd";// + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    //ffchartUI.Setdata(false, SN_2nd, values["X_Position_1st"].ToArray(), values["X_Amplitude_2nd"].ToArray(), values["Y_Amplitude_2nd"].ToArray());
                    ffchartUI.Setdata(false, SN_2nd, pos_2nd_new.ToArray(), values["X_Amplitude_2nd"].ToArray(), values["Y_Amplitude_2nd"].ToArray());
                    if (FFChartSaveImage_SW)
                    {
                        string filename = SaveImagePath + $@"\{SN}_2nd_{BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)}.jpg";
                        ffchartUI.SaveImage(false, filename);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void CalculateKB(bool _1stOR2nd,double BW_1st, double BW_2nd, double MoveDistance, List<double> pos, List<double> Amplitude, ref double k, ref double b)
        {
            try
            {
                //先算出发散角
                double width = (BW_2nd - BW_1st) / 2 / (MoveDistance * 1000);
                double Deg = Math.Atan(width) * 180 / Math.PI * 2;
                //找到最大点
                int index = 0;
                double MaxValue = Amplitude.Max();
                for (int i = 1; i < Amplitude.Count; i++)
                {
                    if (Amplitude[i]== MaxValue)
                    {
                        index = i;
                        break;
                    }

                    //if (Amplitude[i - 1] < MaxValue && MaxValue > Amplitude[i + 1])
                    //{
                    //    index = i;
                    //    //break;
                    //}
                }
                //算KB

                //将该区间的数据进行直线拟合               
                double BW_pos_end = 0; //半宽结束点对应的pos
                if (_1stOR2nd)
                {
                     BW_pos_end = pos[index] + BW_1st / 2;
                }
                else
                {
                    BW_pos_end = pos[index] + BW_2nd / 2;
                }               
                double[] YArr = new double[] { 0, Deg / 2 };
                //double[] XArr = new double[] { pos[index], pos[pos.Count - 1] };
                double[] XArr = new double[] { pos[index], BW_pos_end };

                //拟合成函数, 设定为1阶拟合 
                //X:距离
                //Y:角度                       
                PolyFitData polyData = PolyFitMath.PolynomialFit(XArr, YArr, 1);

                //得出K和B
                k = polyData.Coeffs[1];
                b = polyData.Coeffs[0];
            }
            catch (Exception ex)
            {

               // throw;
            }
            
        }
        private void UpdateRawDataCollection_DGV(IRawDataCollectionBase rawDataCollection)
        {
            try
            {
                Dictionary<string, List<object>> colValDict = new Dictionary<string, List<object>>();
                //Dictionary<string, List<double>> colValDict = new Dictionary<string, List<double>>();
                var itemProps = rawDataCollection.Peek().GetType().GetProperties();

                foreach (var prop in itemProps)
                {
                    if (PropHelper.IsPropertyBelongs<RawDataCollectionItemElementAttribute>(prop))
                    {
                        var propValue = PropHelper.GetAttributeValue<RawDataCollectionItemElementAttribute>(prop);
                        //var colVals = rawDataCollection.GetDataListByPropName(prop.Name);
                        var colVals = rawDataCollection.GetDataListByPropName_V2(prop.Name);
                        var colHeader = propValue.ElementTag;
                        colValDict.Add(colHeader, colVals);
                    }
                }
                foreach (var kvp in colValDict)
                {
                    DataGridViewColumn newCol = new DataGridViewTextBoxColumn();
                    newCol.Name = kvp.Key;
                    newCol.HeaderText = kvp.Key;
                    this.dgv_RawDataCollection.Columns.Add(newCol);
                }

                var colWidth = this.dgv_RawDataCollection.Width / this.dgv_RawDataCollection.ColumnCount;
                foreach(DataGridViewColumn col in this.dgv_RawDataCollection.Columns)
                {
                    col.Width = colWidth;
                }

                this.dgv_RawDataCollection.RowCount = rawDataCollection.Count;

                foreach (var kvp in colValDict)
                {
                    for (int rowIndex = 0; rowIndex < this.dgv_RawDataCollection.RowCount; rowIndex++)
                    {
                        this.dgv_RawDataCollection.Rows[rowIndex].Cells[kvp.Key].Value = kvp.Value[rowIndex];
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}