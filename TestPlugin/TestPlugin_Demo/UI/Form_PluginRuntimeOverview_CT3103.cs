using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.UIComponents;
using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TestPlugin_CoarseTuning;

namespace TestPlugin_Demo
{
    public partial class Form_PluginRuntimeOverview_CT3103 : Form, ITestDataViewer//, ITesterCoreLink, IAccessPermissionLevel// where TTestPlugin : class, ITesterAppPluginInteration
    {
        //System.Timers.Timer refreshTimer = new System.Timers.Timer(2000);
        protected ITesterCoreInteration _core;
        protected IMajorStreamData _localMajorStreamData;
        //protected ITestPluginRuntimeOverview _viewer;
        const string SUMMARY_PAGE = "tp_SummaryData";
        const string DATA_CHARTS_PAGE = "tp_DataCharts";
        const int MAX_RAW_DATA_PAGE = 50;//20;
        const string PAGE_TEXT_FORMAT = "TP_RAWDATA_{0}";
        //Dictionary<int, Form_RawDataViewer> RawDataViewerForms = new Dictionary<int, Form_RawDataViewer>();  
        Dictionary<int, Form_RawDataViewer> RawDataViewerForms = new Dictionary<int, Form_RawDataViewer>();
        Dictionary<int, TabPage> RawDataViewerFormPages = new Dictionary<int, TabPage>();
        List<Form_ModuleChart> _myChartUIs = new List<Form_ModuleChart>();
        List<Form_ModuleFFChart> _myFFChartUIs = new List<Form_ModuleFFChart>();

        private Form_Flow_layer _dataChartsLayer;
        private List<string> FFSn { get; set; } //防止重复存图 
        public Form_PluginRuntimeOverview_CT3103()
        {
            InitializeComponent();
            RawDataViewerForms.Clear();
            RawDataViewerFormPages.Clear();
            this.FFSn = new List<string>();
            //refreshTimer.Elapsed += RefreshTimer_Elapsed;
            //refreshTimer.Start();
            //tp_FFDataCharts.Parent = null;
        }
        public virtual void Clear()
        {
            try
            {
                this.treeView_MainStreamData.Nodes.Clear();

                for (int rawDataIndex = 0; rawDataIndex < MAX_RAW_DATA_PAGE; rawDataIndex++)
                {
                    RawDataViewerForms[rawDataIndex].ClearContext();
                    RawDataViewerFormPages[rawDataIndex].Parent = null;
                }
                //this._dataChartsLayer.ClearCharts();

                //this.tb_totalDeviceCount.Text = "0";
                //this.tb_testingDeviceSn.Text = string.Empty;
                foreach (var ui in _myChartUIs)
                {
                    ui.ClearChart();
                }
                //foreach (var ui in _myFFChartUIs)
                //{
                //    ui.ClearffChart();
                //}

                this.pdgv_summaryData.Clear();
                //this.pdgv_BinSummaryData.Clear();
            }
            catch
            {
            }
        }
        private void UpdateRawDataChart(Form_ModuleChart chartUI, IRawDataCollectionBase rawDataCollection)
        {
            try
            {
                Dictionary<CEAxisXY, string> legends = new Dictionary<CEAxisXY, string>();
                Dictionary<CEAxisXY, List<object>> values = new Dictionary<CEAxisXY, List<object>>();
                //List<double> y_Array = new List<double>();
                //List<double> y2_Array = new List<double>();

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
                    chartUI.UpdateChartSeries(AxisType.Primary, legendName, values[CEAxisXY.X], values[CEAxisXY.Y]);
                }
                if (legends.ContainsKey(CEAxisXY.Y2))
                {
                    var legendName = $"{legends[CEAxisXY.Y2]} vs {legends[CEAxisXY.X]}";
                    chartUI.UpdateChartSeries(AxisType.Secondary, legendName, values[CEAxisXY.X], values[CEAxisXY.Y2]);
                }
                chartUI.UpdateTitle(rawDataCollection.Name);
            }
            catch (Exception ex)
            {

            }
        }
        private void UpdateRawDataMenuChart_V3(Form_ModuleChart chartUI, IRawDataMenuCollection rawDataMenuCollection)
        {
            try
            {
                //int inType = 1;
                Dictionary<string, CEAxisXY> legends = new Dictionary<string, CEAxisXY>();
                Dictionary<string, List<object>> values = new Dictionary<string, List<object>>();

                foreach (var rdata in rawDataMenuCollection.GetDataMenuCollection())
                {
                    var lrd = rdata as IRawDataCollectionBase;
                    var itemProps = lrd.Peek().GetType().GetProperties();
                    var na = lrd.GetChartSeriesDataListByPropName(itemProps[0].Name);
                    foreach (var prop in itemProps)
                    {
                        if (PropHelper.IsPropertyBelongs<RawDataChartAxisElementAttribute>(prop))
                        {
                            var propValue = PropHelper.GetAttributeValue<RawDataChartAxisElementAttribute>(prop);


                            legends.Add(prop.Name, propValue.ChartAxisXY);



                            values.Add(prop.Name, rdata.GetChartSeriesDataListByPropName(prop.Name));




                        }
                    }
                    if (legends.ContainsValue(CEAxisXY.X) == false)
                    {
                        //没有x轴  不画图
                        return;
                    }
                    if (legends.Values.ToList().FindAll(it => it == CEAxisXY.X).Count > 1)
                    {
                        //多个x轴  不画图
                        return;
                    }
                    List<object> xValues = new List<object>();
                    string xName = string.Empty;
                    foreach (var item in legends)
                    {
                        if (item.Value == CEAxisXY.X)
                        {
                            xValues = values[item.Key];
                            xName = item.Key;
                        }
                    }
                    foreach (var item in legends)
                    {
                        //if (item.Value== CEAxisXY.Y)
                        //{
                        //    if (inType<=1)
                        //    {
                        //        var legendName = $"{item.Key} vs {xName}_{inType}";
                        //        this.chartUI.UpdateChartSeries(AxisType.Primary, legendName, xValues, values[item.Key]);
                        //    }
                        //    else
                        //    {
                        //        var legendName = $"{item.Key} vs {xName}_{inType}";
                        //        this.chartUI.UpdateChartSeries(AxisType.Secondary, legendName, xValues, values[item.Key]);
                        //    }
                        //}

                        switch (item.Value)
                        {
                            case CEAxisXY.Y:
                                {
                                    var legendName = $"{na[0]}_Y";
                                    chartUI.UpdateChartSeries(AxisType.Primary, legendName, xValues, values[item.Key]);
                                }
                                break;
                            case CEAxisXY.Y2:
                                {
                                    var legendName = $"{na[0]}_Y2";
                                    chartUI.UpdateChartSeries(AxisType.Secondary, legendName, xValues, values[item.Key]);
                                }
                                break;
                        }
                    }
                    //inType++;
                    legends.Clear();
                    values.Clear();
                }




            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateRawDataChart_V2(Form_ModuleChart chartUI, IRawDataCollectionBase rawDataCollection)
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
                    if (item.Value == CEAxisXY.X)
                    {
                        xValues = values[item.Key];
                        xName = item.Key;
                    }
                }
                foreach (var item in legends)
                {
                    switch (item.Value)
                    {
                        case CEAxisXY.Y:
                            {
                                var legendName = $"{item.Key} vs {xName}";
                                chartUI.UpdateChartSeries(AxisType.Primary, legendName, xValues, values[item.Key]);
                            }
                            break;
                        case CEAxisXY.Y2:
                            {
                                var legendName = $"{item.Key} vs {xName}";
                                chartUI.UpdateChartSeries(AxisType.Secondary, legendName, xValues, values[item.Key]);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void UpdateRawDataChart_V3(Form_ModuleChart chartUI, IRawDataCollectionBase rawDataCollection)
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
                    if (item.Value == CEAxisXY.X)
                    {
                        xValues = values[item.Key];
                        xName = item.Key;
                    }
                }
                foreach (var item in legends)
                {
                    switch (item.Value)
                    {
                        case CEAxisXY.Y:
                            {
                                var legendName = $"{item.Key} vs {xName}";
                                chartUI.UpdateChartSeries(AxisType.Primary, legendName, xValues, values[item.Key]);
                            }
                            break;
                        case CEAxisXY.Y2:
                            {
                                var legendName = $"{item.Key} vs {xName}";
                                chartUI.UpdateChartSeries(AxisType.Secondary, legendName, xValues, values[item.Key]);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        //是否需要更新FF的图
        private bool IsUpdateRawDataFFChart(IRawDataCollectionBase rawDataCollection)
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
                if (DicBeamWidth.Count <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //是否需要更新图
        private bool IsUpdateRawDataChart_V2(IRawDataCollectionBase rawDataCollection)
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
                    return false;
                }
                if (legends.Values.ToList().FindAll(item => item == CEAxisXY.X).Count > 1)
                {
                    //多个x轴  不画图
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void UpdateRawDataFFChart(Form_ModuleFFChart ffchartUI, IRawDataCollectionBase rawDataCollection, string sn)
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
                if (DicBeamWidth.Count == 0)
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
                this.CalculateKB(true, DicBeamWidth["BeamWidth_13p5_X_1st"], DicBeamWidth["BeamWidth_13p5_X_2nd"], DicBeamWidth["MoveDistance_mm"], values["X_Position_1st"], values["X_Amplitude_1st"], ref K_1st, ref B_1st);
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
                    string SN_1st = $"{sn}_1st";// + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    //ffchartUI.Setdata(true, SN_1st, values["X_Position_1st"].ToArray(), values["X_Amplitude_1st"].ToArray(), values["Y_Amplitude_1st"].ToArray());
                    ffchartUI.Setdata(true, SN_1st, pos_1st_new.ToArray(), values["X_Amplitude_1st"].ToArray(), values["Y_Amplitude_1st"].ToArray());
                    //if (FFChartSaveImage_SW)//这个里面不存图了
                    //{
                    //    string filename = SaveImagePath + $@"\{SN}_1st_{BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)}.jpg";
                    //    ffchartUI.SaveImage(true, filename);
                    //}
                    string SN_2nd = $"{sn}_2nd";// + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    //ffchartUI.Setdata(false, SN_2nd, values["X_Position_1st"].ToArray(), values["X_Amplitude_2nd"].ToArray(), values["Y_Amplitude_2nd"].ToArray());
                    ffchartUI.Setdata(false, SN_2nd, pos_2nd_new.ToArray(), values["X_Amplitude_2nd"].ToArray(), values["Y_Amplitude_2nd"].ToArray());
                    //if (FFChartSaveImage_SW)//这个里面不存图了
                    //{
                    //    string filename = SaveImagePath + $@"\{SN}_2nd_{BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)}.jpg";
                    //    ffchartUI.SaveImage(false, filename);
                    //}
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void CalculateKB(bool _1stOR2nd, double BW_1st, double BW_2nd, double MoveDistance, List<double> pos, List<double> Amplitude, ref double k, ref double b)
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
                    if (Amplitude[i] == MaxValue)
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

        public void InitializeDataChartsPage(int chartsCount, int ffchartsCount)
        {
            this.FFSn.Clear();
            if (chartsCount <= 0 || ffchartsCount <= 0)
            {
                return;
            }
            this._myChartUIs.Clear();
            this._myFFChartUIs.Clear();


            for (int i = 0; i < chartsCount; i++)
            {
                Form_ModuleChart chartUI = new Form_ModuleChart();
                chartUI.ClearChart();

                _myChartUIs.Add(chartUI);
            }
            for (int i = 0; i < ffchartsCount; i++)  //建多了，小心爆内存
            {
                Form_ModuleFFChart ffchartUI = new Form_ModuleFFChart();
                //ffchartUI.ClearffChart();
                _myFFChartUIs.Add(ffchartUI);
            }
            if (_myChartUIs.Count <= 0)
            {
                return;
            }
            else
            {
                LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 6, 6);
            }
            if (_myFFChartUIs.Count <= 0)
            {
                return;
            }
            else
            {
                //LayoutSubForms_FF(this.tlp_FFDataViewlayer, _myFFChartUIs.ToArray(), 6, 6);
            }
            #region 后面不管他数量了
            //if (_myChartUIs.Count <= 0)
            //{
            //    return;
            //}
            //LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 2, 3);

            //if (_myChartUIs.Count <= 0)
            //{
            //    return;
            //}
            //else if (_myChartUIs.Count <= 4 && _myChartUIs.Count > 0)
            //{
            //   // _dataChartsLayer.LayoutSubForms(_myChartUIs.ToArray(), 2);

            //    LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 1, 4);
            //}
            //else if (_myChartUIs.Count <= 8 && _myChartUIs.Count > 4)
            //{
            //    LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 2, 4);
            //}
            //else if (_myChartUIs.Count <= 12 && _myChartUIs.Count > 8)
            //{
            //    LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 3, 4);
            //}
            //else if (_myChartUIs.Count <= 16 && _myChartUIs.Count > 12)
            //{
            //    LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 4, 4);
            //}
            //else if (_myChartUIs.Count <= 20 && _myChartUIs.Count > 16)
            //{
            //    LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 5, 4);
            //}
            //else if (_myChartUIs.Count <= 24 && _myChartUIs.Count > 20)
            //{
            //    LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 6, 4);
            //}
            //else if (_myChartUIs.Count <= 28 && _myChartUIs.Count > 24)
            //{
            //    LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 7, 4);
            //}
            //else if (_myChartUIs.Count <= 32 && _myChartUIs.Count > 28)
            //{
            //    LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 8, 4);
            //}
            //else if (_myChartUIs.Count <= 36 && _myChartUIs.Count > 32)
            //{
            //    LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 9, 4);
            //}
            //else if (_myChartUIs.Count <= 40 && _myChartUIs.Count > 36)
            //{
            //    LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 10, 4);
            //}
            //else if (_myChartUIs.Count <= 44 && _myChartUIs.Count > 40)
            //{
            //    LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 11, 4);
            //}
            //else if (_myChartUIs.Count <= 48 && _myChartUIs.Count > 44)
            //{
            //    LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 12, 4);
            //}
            //else if (_myChartUIs.Count <= 52 && _myChartUIs.Count > 48)
            //{
            //   LayoutSubForms(this.tlp_fullDataViewlayer, _myChartUIs.ToArray(), 13, 4);
            //   // _dataChartsLayer.LayoutSubForms(_myChartUIs.ToArray(), 13, 4);
            //}
            #endregion

        }
        int gcolumnCount = 2;
        int growCount = 2;
        public void LayoutSubForms(FlowLayoutPanel tlp_layer, Form[] UIs, int rowCount, int columnCount)
        {
            try
            {
                //20230302 不理这个参数
                // gcolumnCount = columnCount = 2;
                // growCount = rowCount = 2;
                gcolumnCount = columnCount = 3;
                growCount = rowCount = 3;


                this.SuspendLayout();

                //int flow_w = this.flow_layer.Width - 30;
                //int flow_h = this.flow_layer.Height;

                int flow_w = tlp_layer.Width - 30;
                int flow_h = tlp_layer.Height;

                int plane_w = flow_w / columnCount;
                int plane_h = flow_h / rowCount;

                for (int i = 0; i < UIs.Length; i++)
                {
                    Panel p = new Panel();


                    UIs[i].Hide();
                    UIs[i].TopLevel = false;
                    UIs[i].FormBorderStyle = FormBorderStyle.None;
                    UIs[i].Dock = DockStyle.Fill;
                    UIs[i].Show();
                    p.Controls.Add(UIs[i]);
                    p.Width = plane_w;
                    p.Height = plane_h;

                    //this.flow_layer.Controls.Add(p);
                    tlp_layer.Controls.Add(p);
                }
                this.ResumeLayout(true);
            }
            catch (Exception ex)
            {
            }
        }
        int gcolumnCount_ff = 1;
        int growCount_ff = 1;
        public void LayoutSubForms_FF(FlowLayoutPanel tlp_layer, Form[] UIs, int rowCount, int columnCount)
        {
            try
            {
                //20230302 不理这个参数
                // gcolumnCount = columnCount = 2;
                // growCount = rowCount = 2;
                gcolumnCount_ff = columnCount = 1;
                growCount_ff = rowCount = 1;


                this.SuspendLayout();

                //int flow_w = this.flow_layer.Width - 30;
                //int flow_h = this.flow_layer.Height;

                int flow_w = tlp_layer.Width - 30;
                int flow_h = tlp_layer.Height;

                int plane_w = flow_w / columnCount;
                int plane_h = flow_h / rowCount;

                for (int i = 0; i < UIs.Length; i++)
                {
                    Panel p = new Panel();


                    UIs[i].Hide();
                    UIs[i].TopLevel = false;
                    UIs[i].FormBorderStyle = FormBorderStyle.None;
                    UIs[i].Dock = DockStyle.Fill;
                    UIs[i].Show();
                    p.Controls.Add(UIs[i]);
                    p.Width = plane_w;
                    p.Height = plane_h;

                    //this.flow_layer.Controls.Add(p);
                    tlp_layer.Controls.Add(p);
                    GC.Collect();
                }
                this.ResumeLayout(true);
            }
            catch (Exception ex)
            {
            }
        }

        private void flow_layer_Resize(object sender, EventArgs e)
        {
            try
            {
                this.SuspendLayout();

                int flow_w = this.tlp_fullDataViewlayer.Width - 50;
                int flow_h = this.tlp_fullDataViewlayer.Height;

                int plane_w = flow_w / gcolumnCount;
                int plane_h = flow_h / growCount;

                for (int i = 0; i < this.tlp_fullDataViewlayer.Controls.Count; i++)
                {
                    this.tlp_fullDataViewlayer.Controls[i].Width = plane_w;
                    this.tlp_fullDataViewlayer.Controls[i].Height = plane_h;
                }
                this.ResumeLayout(true);
            }
            catch (Exception ex)
            {
            }
        }
        private void flow_layer_Resize_FF(object sender, EventArgs e)
        {
            //try
            //{
            //    this.SuspendLayout();

            //    int flow_w = this.tlp_FFDataViewlayer.Width - 30;
            //    int flow_h = this.tlp_FFDataViewlayer.Height;

            //    int plane_w = flow_w / gcolumnCount_ff;
            //    int plane_h = flow_h / growCount_ff;

            //    for (int i = 0; i < this.tlp_FFDataViewlayer.Controls.Count; i++)
            //    {
            //        this.tlp_FFDataViewlayer.Controls[i].Width = plane_w;
            //        this.tlp_FFDataViewlayer.Controls[i].Height = plane_h;
            //    }
            //    this.ResumeLayout(true);
            //}
            //catch (Exception ex)
            //{
            //}
        }
        //public void LayoutSubForms(FlowLayoutPanel tlp_layer,  Form[] UIs, int rowCount, int columnCount)
        //{
        //    //try
        //    //{
        //    //    const float COL_WIDTH = 50f;
        //    //    this.SuspendLayout();
        //    //    tlp_layer.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        //    //    tlp_layer.RowCount = rowCount;
        //    //    tlp_layer.RowStyles.Clear();

        //    //    tlp_layer.ColumnCount = columnCount;
        //    //    tlp_layer.ColumnStyles.Clear();


        //    //    //var colCount = EXE_CHARTS_SEED;
        //    //    //var rowCount = UIs.Length / columnCount + UIs.Length % columnCount;
        //    //    var rowH = 100.0f / rowCount;
        //    //    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
        //    //    {
        //    //         tlp_layer.RowStyles.Add(new RowStyle(SizeType.Percent, rowH));
        //    //    }
        //    //    for (int colIndex = 0; colIndex < columnCount; colIndex++)
        //    //    {
        //    //         tlp_layer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, COL_WIDTH));
        //    //    }
        //    //     tlp_layer.Location = new Point(0, 0);
        //    //     tlp_layer.Dock = DockStyle.Fill;

        //    //    for (int i = 0; i < UIs.Length; i++)
        //    //    {
        //    //        UIs[i].Hide();
        //    //        UIs[i].TopLevel = false;
        //    //        UIs[i].FormBorderStyle = FormBorderStyle.None;
        //    //        UIs[i].Dock = DockStyle.Fill;
        //    //        UIs[i].Show();
        //    //        var colIndex = i % columnCount;
        //    //        var rowIndex = i / columnCount;
        //    //        tlp_layer.Controls.Add(UIs[i], i % columnCount, i / columnCount);
        //    //    }

        //    //    this.ResumeLayout(false);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //}
        //}

        void UpdateDataChartsPage(List<RawDataBaseLite> rawDataCollection, string sn)
        {
            //this._core.GUIRunUIInvokeAction(() =>
            //{
            //_dataChartsLayer.ClearCharts();

            if (rawDataCollection.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < this._myChartUIs.Count; i++)
            {
                this._myChartUIs[i].ClearChart();
            }

            //List<Form_ModuleChart> chartUIs = new List<Form_ModuleChart>();
            int uiIndex = -1;
            int uiffIndex = -1;
            foreach (var rawData in rawDataCollection)
            {
                if (rawData is IRawDataMenuCollection)
                {
                    var rawDataMenuCollection = (IRawDataMenuCollection)rawData;
                    if (rawDataMenuCollection.Count <= 0)
                    {

                    }
                    //else if(rawDataMenuCollection.Peek()==null)  //20241121 没有数据不显示
                    //{

                    //}
                    else
                    {
                        uiIndex++;
                        var chartUI = this._myChartUIs[uiIndex];
                        chartUI.ClearChart();
                        this.UpdateRawDataMenuChart_V3(chartUI, rawDataMenuCollection);
                    }
                }
                else if (rawData is IRawDataCollectionBase)
                {
                    //if (rawData.Name.Contains("ThreeAxis"))
                    //{
                    //    continue;
                    //}
                    if (IsUpdateRawDataChart_V2((IRawDataCollectionBase)rawData)) //先判断要不要画图
                    {
                        uiIndex++;
                        var chartUI = this._myChartUIs[uiIndex];
                        chartUI.ClearChart();

                        UpdateRawDataChart_V3(chartUI, (IRawDataCollectionBase)rawData);
                    }



                    //if (rawData.Name.Contains("NanoScanAnalyse"))
                    if (IsUpdateRawDataFFChart((IRawDataCollectionBase)rawData))//先判断有没有FF的图
                    {
                        uiffIndex++;
                        var chartffUI = this._myFFChartUIs[uiffIndex];
                        string _sn = sn + "_" + rawData.RawDataFixFormat;
                        UpdateRawDataFFChart(chartffUI, (IRawDataCollectionBase)rawData, _sn);
                    }

                    //this.UpdateRawDataChart(chartUI, (IRawDataCollectionBase)rawData);
                    //chartUIs.Add(chartUI);
                }
                else
                {
                }
            }
        }

        protected virtual void UpdateMainStreamData_TreeView(TreeView tview, IMajorStreamData data)
        {
            try
            {
                tview.Nodes.Clear();
                //major
                var majorNode = Convert_MainStreamDataToTreeNode(data);
                if (data.MinorStreamDataCollectionCount > 0)
                {
                    var minorDataCollection = data.GetMinorStreamDataCollection();
                    foreach (var minorData in minorDataCollection)
                    {
                        var minorNode = Convert_MinorStreamDataToTreeNode(minorData);
                        if (minorData.DeviceStreamDataCollectionCount > 0)
                        {
                            var deviceDataCollection = minorData.GetDeviceStreamDataCollection();
                            foreach (var deviceData in deviceDataCollection)
                            {
                                var deviceNode = Convert_DeviceStreamDataToTreeNode(deviceData);
                                minorNode.Nodes.Add(deviceNode);
                            }
                        }
                        majorNode.Nodes.Add(minorNode);
                    }
                }
                tview.Nodes.Add(majorNode);
            }
            catch (Exception ex)
            {
            }
        }
        protected virtual TreeNode Convert_MainStreamDataToTreeNode(IMajorStreamData data)
        {
            TreeNode node = new TreeNode(data.Information);
            node.Name = data.Information;
            return node;
        }
        protected virtual TreeNode Convert_MinorStreamDataToTreeNode(IMinorStreamData data)
        {
            TreeNode node = new TreeNode(data.Information);
            node.Name = data.Information;
            return node;
        }
        protected virtual TreeNode Convert_DeviceStreamDataToTreeNode(IDeviceStreamDataBase data)
        {
            TreeNode node = new TreeNode(data.Information);
            node.Name = data.Information;
            return node;
        }
        public virtual void UpdateMainStreamData(IMajorStreamData majorData, string targetDeviceSn)
        {
            try
            {
                this.BeginInvoke((EventHandler)delegate
                {
                    #region MyRegion

                    this._localMajorStreamData = majorData;

                    //这里去存图 
                    this.ManageFFImageData(majorData, targetDeviceSn);

                    if (this.chk_autoRefresh.Checked == false)
                    {
                        //这里去存图 
                        //this.ManageFFImageData(majorData, targetDeviceSn);
                        return;
                    }

                    //var majorData = this._viewer.GetMajorStreamData();

                    if (majorData == null)
                    {
                        return;
                    }

                    //转换majordata to tree view 

                    UpdateMainStreamData_TreeView(this.treeView_MainStreamData, majorData);
                    this.treeView_MainStreamData.ExpandAll();

                    string CSVPath = majorData.GetType().GetProperty("CsvSaveDataPath").GetValue(majorData, null).ToString();

                    if (majorData.MinorStreamDataCollectionCount <= 0)
                    {
                        return;
                    }


                    var lastMinorData = majorData.GetMinorStreamDataCollection().Last();

                    if (lastMinorData.DeviceStreamDataCollectionCount <= 0)
                    {
                        return;
                    }

                    //var deviceData = lastMinorData.GetDeviceStreamDataCollection().Last();

                    var deviceData = lastMinorData.GetDeviceStreamDataCollection().Find(item => item.SerialNumber == targetDeviceSn);

                    if (deviceData == null)
                    {
                        return;
                    }
                    //var rawDataCollection = deviceData as IRawDataCollectionBase;
                    //if (rawDataCollection.Count <= 0)
                    //{
                    //    return;
                    //}
                    //更新物料看板 
                    //this.tb_testingDeviceSn.Text = deviceData.SerialNumber;
                    //this.tb_totalDeviceCount.Text = lastMinorData.DeviceStreamDataCollectionCount.ToString();



                    if (deviceData.RawDataCollecetionCount <= 0)
                    {
                        foreach (var ui in this._myChartUIs)
                        {
                            ui.ClearChart();
                        }
                        //foreach (var ui in this._myFFChartUIs)
                        //{
                        //    ui.ClearffChart();
                        //}
                    }
                    else
                    {
                        var rawDataCount = deviceData.RawDataCollecetionCount;

                        this.UpdateDataChartsPage(deviceData.RawDataCollection, deviceData.SerialNumber);

                        int ViewerFormsCount = 0;
                        foreach (var rawData in deviceData.RawDataCollection)
                        {
                            if (rawData is IRawDataMenuCollection)
                            {

                            }
                            else if (rawData is IRawDataCollectionBase)
                            {
                                if (IsUpdateRawDataChart_V2((IRawDataCollectionBase)rawData)) //先判断要不要画图
                                {
                                    ViewerFormsCount++;
                                }
                            }
                        }
                        for (int pageIndex = 0; pageIndex <= MAX_RAW_DATA_PAGE; pageIndex++)
                        {
                            if (pageIndex >= ViewerFormsCount)
                            {
                                RawDataViewerFormPages[pageIndex].Parent = null;
                            }
                            else
                            {
                                if (RawDataViewerFormPages[pageIndex].Parent == null)
                                {
                                    RawDataViewerFormPages[pageIndex].Parent = this.tb_dataViewer;
                                }
                            }
                        }

                        int ViewerFormsIndex = 0;
                        int RawDataIndex = 0;
                        foreach (var rawData in deviceData.RawDataCollection)
                        {
                            if (rawData is IRawDataMenuCollection)
                            {
                                RawDataViewerForms[ViewerFormsIndex].ClearContext();
                                RawDataViewerForms[ViewerFormsIndex].ImportRawData(deviceData.RawDataCollection[RawDataIndex]);

                                string _name = deviceData.RawDataCollection[RawDataIndex].Name.Split(']')[deviceData.RawDataCollection[RawDataIndex].Name.Split(']').Length - 2].Remove(0, 12);
                                RawDataViewerFormPages[ViewerFormsIndex].Text = $"{deviceData.Information}_{_name}";// string.Format(PAGE_TEXT_FORMAT, deviceData.RawDataCollection[rawDataIndex].Name);
                                ViewerFormsIndex++;
                            }
                            else if (rawData is IRawDataCollectionBase)
                            {
                                if (IsUpdateRawDataChart_V2((IRawDataCollectionBase)rawData)) //先判断要不要画图
                                {
                                    RawDataViewerForms[ViewerFormsIndex].ClearContext();
                                    RawDataViewerForms[ViewerFormsIndex].ImportRawData(deviceData.RawDataCollection[RawDataIndex]);

                                    string _name = deviceData.RawDataCollection[RawDataIndex].Name.Split(']')[deviceData.RawDataCollection[RawDataIndex].Name.Split(']').Length - 2].Remove(0, 12);
                                    RawDataViewerFormPages[ViewerFormsIndex].Text = $"{deviceData.Information}_{_name}";// string.Format(PAGE_TEXT_FORMAT, deviceData.RawDataCollection[rawDataIndex].Name);
                                    ViewerFormsIndex++;
                                }
                            }
                            RawDataIndex++;
                        }
                        #region bak20231105
                        //for (int rawDataIndex = 0; rawDataIndex < rawDataCount; rawDataIndex++)
                        //{
                        //    //传文件路径和SN号进去 //这里也不要去存图，把他拿出去
                        //    //string ImagePath = CSVPath.Remove(CSVPath.LastIndexOf('\\')) + $@"\Image\{JobInformation.WorkOrder}\{deviceData.SerialNumber}\";
                        //    //string ImagePath = CSVPath + $@"\Image\{JobInformation.WorkOrder}\{deviceData.SerialNumber}\";
                        //    string ImagePath = CSVPath + $@"\Image\{deviceData.SerialNumber}\";
                        //    //if (Directory.Exists(ImagePath) == false)
                        //    //{
                        //    //    Directory.CreateDirectory(ImagePath);
                        //    //}
                        //    RawDataViewerForms[rawDataIndex].SaveImagePath = ImagePath;
                        //    RawDataViewerForms[rawDataIndex].SN = deviceData.SerialNumber + "_" + deviceData.RawDataCollection[rawDataIndex].RawDataFixFormat;
                        //    RawDataViewerForms[rawDataIndex].FFChartSaveImage_SW = true;

                        //    RawDataViewerForms[rawDataIndex].ClearContext();
                        //    RawDataViewerForms[rawDataIndex].ImportRawData(deviceData.RawDataCollection[rawDataIndex]);

                        //    string _name = deviceData.RawDataCollection[rawDataIndex].Name.Split(']')[deviceData.RawDataCollection[rawDataIndex].Name.Split(']').Length - 2].Remove(0, 12);
                        //    // string ModeleName = _name.Remove(0, 12);
                        //    RawDataViewerFormPages[rawDataIndex].Text = $"{deviceData.Information}_{_name}";// string.Format(PAGE_TEXT_FORMAT, deviceData.RawDataCollection[rawDataIndex].Name);

                        //    // RawDataViewerFormPages[rawDataIndex].Text = $"{deviceData.Information}{deviceData.RawDataCollection[rawDataIndex].Name}";// string.Format(PAGE_TEXT_FORMAT, deviceData.RawDataCollection[rawDataIndex].Name);
                        //}
                        #endregion
                        if (deviceData.SummaryDataCollection.Count <= 0)
                        {
                            this.tb_dataViewer.TabPages["tp_SummaryData"].Text = "SummaryData";
                            this.pdgv_summaryData.Clear();
                            //this.pdgv_BinSummaryData.Clear();
                        }
                        else
                        {
                            this.tb_dataViewer.TabPages["tp_SummaryData"].Text = $"{deviceData.Information}_SummaryData";
                            this.pdgv_summaryData.Clear();
                            this.pdgv_summaryData.ImportSourceData(deviceData.SummaryDataCollection, true);

                            //this.pdgv_BinSummaryData.Clear();
                            //var binSummayName = (majorData as MajorStreamData_20050A).BinSummaryDataNames;
                            //var binSummary = (deviceData as DeviceStreamData_20050A).GetBinSummary(binSummayName);
                            //this.pdgv_BinSummaryData.ImportSourceData(binSummary, true);
                        }

                    }
                    #endregion
                });
            }
            catch (Exception ex)
            {

            }
        }
        private void ManageFFImageData(IMajorStreamData majorData, string targetDeviceSn)
        {    //先处理数据
            try
            {
                string CSVPath = majorData.GetType().GetProperty("CsvSaveDataPath").GetValue(majorData, null).ToString();

                var lastMinorData = majorData.GetMinorStreamDataCollection().Last();

                if (lastMinorData.DeviceStreamDataCollectionCount <= 0)
                {
                    return;
                }

                var deviceData = lastMinorData.GetDeviceStreamDataCollection().Find(item => item.SerialNumber == targetDeviceSn);

                if (deviceData == null)
                {
                    return;
                }

                if (deviceData.RawDataCollecetionCount <= 0)
                {
                    //foreach (var ui in this._myChartUIs)
                    //{
                    //    ui.ClearChart();
                    //}
                    //foreach (var ui in this._myFFChartUIs)
                    //{
                    //    ui.ClearffChart();
                    //}
                }
                else
                {
                    var rawDataCount = deviceData.RawDataCollecetionCount;


                    //this.UpdateDataChartsPage(deviceData.RawDataCollection);


                    //for (int pageIndex = 0; pageIndex <= MAX_RAW_DATA_PAGE; pageIndex++)
                    //{
                    //    if (pageIndex >= rawDataCount)
                    //    {
                    //        RawDataViewerFormPages[pageIndex].Parent = null;
                    //    }
                    //    else
                    //    {
                    //        RawDataViewerFormPages[pageIndex].Parent = this.tb_dataViewer;
                    //    }
                    //}
                    for (int rawDataIndex = 0; rawDataIndex < rawDataCount; rawDataIndex++)
                    {
                        //传文件路径和SN号进去 //这里也不要去存图，把他拿出去
                        //string ImagePath = CSVPath.Remove(CSVPath.LastIndexOf('\\')) + $@"\Image\{JobInformation.WorkOrder}\{deviceData.SerialNumber}\";
                        //string ImagePath = CSVPath + $@"\Image\{JobInformation.WorkOrder}\{deviceData.SerialNumber}\";
                        string ImagePath = CSVPath + $@"\Image\{deviceData.SerialNumber}\";
                        if (Directory.Exists(ImagePath) == false)
                        {
                            Directory.CreateDirectory(ImagePath);
                        }
                        //不显示，去存图
                        string _sn = deviceData.SerialNumber + "_" + deviceData.RawDataCollection[rawDataIndex].RawDataFixFormat;
                        bool bRet = FFSn.Exists(t => t == _sn);
                        if (!bRet) //存过的图，就不存了
                        {
                            this.SaveFFImage(_sn, ImagePath, deviceData.RawDataCollection[rawDataIndex]);
                        }

                        //RawDataViewerForms[rawDataIndex].SaveImagePath = ImagePath;
                        //RawDataViewerForms[rawDataIndex].SN = deviceData.SerialNumber;
                        //RawDataViewerForms[rawDataIndex].FFChartSaveImage_SW = true;
                        //RawDataViewerForms[rawDataIndex].ClearContext();
                        //RawDataViewerForms[rawDataIndex].ImportRawData(deviceData.RawDataCollection[rawDataIndex]);
                        //RawDataViewerFormPages[rawDataIndex].Text = $"{deviceData.Information}{deviceData.RawDataCollection[rawDataIndex].Name}";// string.Format(PAGE_TEXT_FORMAT, deviceData.RawDataCollection[rawDataIndex].Name);
                    }
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
        }
        private void SaveFFImage(string SN, string SaveImagePath, IRawDataBaseLite rawData)
        {
            try
            {
                if (rawData is IRawDataCollectionBase)
                {
                    var rawDataCollection = (IRawDataCollectionBase)rawData;

                    if (rawDataCollection.Count > 0 /*&& rawDataCollection.Name.Contains("NanoScanAnalyse")*/)
                    {
                        Dictionary<string, CEAxisXY> legends = new Dictionary<string, CEAxisXY>();
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
                        if (DicBeamWidth.Count == 0)
                        {
                            return;
                        }
                        var itemProps = rawDataCollection.Peek().GetType().GetProperties();
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
                        double K_1st = 0; double B_1st = 0;
                        this.CalculateKB(true, DicBeamWidth["BeamWidth_13p5_X_1st"], DicBeamWidth["BeamWidth_13p5_X_2nd"], DicBeamWidth["MoveDistance_mm"], values["X_Position_1st"], values["X_Amplitude_1st"], ref K_1st, ref B_1st);
                        //得到新的pos
                        List<double> pos_1st_new = new List<double>();
                        for (int i = 0; i < values["X_Position_1st"].Count; i++)
                        {
                            pos_1st_new.Add(values["X_Position_1st"][i] * K_1st + B_1st);
                        }

                        double K_2nd = 0; double B_2nd = 0;
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
                            Form_ModuleFFChart ffchartUI = new Form_ModuleFFChart();
                            string SN_1st = $"{SN}_1st";// + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                            //ffchartUI.Setdata(true, SN_1st, values["X_Position_1st"].ToArray(), values["X_Amplitude_1st"].ToArray(), values["Y_Amplitude_1st"].ToArray());
                            ffchartUI.Setdata(true, SN_1st, pos_1st_new.ToArray(), values["X_Amplitude_1st"].ToArray(), values["Y_Amplitude_1st"].ToArray());

                            string filename = SaveImagePath + $@"\{SN}_1st_{BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)}.jpg";
                            ffchartUI.SaveImage(true, filename);

                            string SN_2nd = $"{SN}_2nd";// + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                            //ffchartUI.Setdata(false, SN_2nd, values["X_Position_1st"].ToArray(), values["X_Amplitude_2nd"].ToArray(), values["Y_Amplitude_2nd"].ToArray());
                            ffchartUI.Setdata(false, SN_2nd, pos_2nd_new.ToArray(), values["X_Amplitude_2nd"].ToArray(), values["Y_Amplitude_2nd"].ToArray());

                            filename = SaveImagePath + $@"\{SN}_2nd_{BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)}.jpg";
                            ffchartUI.SaveImage(false, filename);
                            // ffchartUI.Close();
                            ffchartUI.Dispose();
                            this.FFSn.Add(SN);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                // throw;
            }
        }
        public virtual void UpdateMainStreamData(IMajorStreamData majorData)
        {
            try
            {
                this.Invoke((EventHandler)delegate
                {
                    #region MyRegion

                    this._localMajorStreamData = majorData;

                    if (this.chk_autoRefresh.Checked == false)
                    {
                        return;
                    }

                    //var majorData = this._viewer.GetMajorStreamData();

                    if (majorData == null)
                    {
                        return;
                    }

                    //转换majordata to tree view 

                    UpdateMainStreamData_TreeView(this.treeView_MainStreamData, majorData);
                    this.treeView_MainStreamData.ExpandAll();

                    if (majorData.MinorStreamDataCollectionCount <= 0)
                    {
                        return;
                    }


                    var lastMinorData = majorData.GetMinorStreamDataCollection().Last();

                    if (lastMinorData.DeviceStreamDataCollectionCount <= 0)
                    {
                        return;
                    }

                    var deviceData = lastMinorData.GetDeviceStreamDataCollection().Last();

                    if (deviceData.RawDataCollecetionCount <= 0)
                    {

                    }
                    else
                    {
                        var rawDataCount = deviceData.RawDataCollecetionCount;


                        this.UpdateDataChartsPage(deviceData.RawDataCollection, deviceData.SerialNumber);


                        for (int pageIndex = 0; pageIndex <= MAX_RAW_DATA_PAGE; pageIndex++)
                        {
                            if (pageIndex >= rawDataCount)
                            {
                                RawDataViewerFormPages[pageIndex].Parent = null;
                            }
                            else
                            {
                                RawDataViewerFormPages[pageIndex].Parent = this.tb_dataViewer;
                            }
                        }
                        for (int rawDataIndex = 0; rawDataIndex < rawDataCount; rawDataIndex++)
                        {
                            RawDataViewerForms[rawDataIndex].ClearContext();

                            RawDataViewerForms[rawDataIndex].ImportRawData(deviceData.RawDataCollection[rawDataIndex]);

                            string _name = deviceData.RawDataCollection[rawDataIndex].Name.Split(']')[deviceData.RawDataCollection[rawDataIndex].Name.Split(']').Length - 2].Remove(0, 12);
                            // string ModeleName = _name.Remove(0, 12);
                            RawDataViewerFormPages[rawDataIndex].Text = $"{deviceData.Information}_{_name}";// string.Format(PAGE_TEXT_FORMAT, deviceData.RawDataCollection[rawDataIndex].Name);

                            // RawDataViewerFormPages[rawDataIndex].Text = $"{deviceData.Information}{deviceData.RawDataCollection[rawDataIndex].Name}";// string.Format(PAGE_TEXT_FORMAT, deviceData.RawDataCollection[rawDataIndex].Name);
                        }



                    }
                    if (deviceData.SummaryDataCollection.Count <= 0)
                    {
                        this.tb_dataViewer.TabPages["tp_SummaryData"].Text = "SummaryData";
                        this.pdgv_summaryData.Clear();
                    }
                    else
                    {
                        this.tb_dataViewer.TabPages["tp_SummaryData"].Text = $"{deviceData.Information}_SummaryData";
                        this.pdgv_summaryData.Clear();
                        this.pdgv_summaryData.ImportSourceData(deviceData.SummaryDataCollection, true);
                    }
                    //Irwin 20240412 忆芯定制
                    this.tp_CoarseTuning.Text = $"{deviceData.Information}_CoarseTuning";
                    this.tp_coarse.Text = $"{deviceData.Information}_coarseTuning";
                    if (panel_coarsetuning.Controls.Count > 0)
                    {
                        foreach (Control item in panel_coarsetuning.Controls)
                        {
                            item.Dispose();
                        }
                    }
                    if (panel_coarse.Controls.Count > 0)
                    {
                        foreach (Control item in panel_coarse.Controls)
                        {
                            item.Dispose();
                        }
                    }
                    if (deviceData.RawDataCollecetionCount > 1)
                    {
                        //var rawData = deviceData.RawDataCollection[7];
                        foreach (var rawData in deviceData.RawDataCollection)
                        {
                            var type = rawData.GetType();
                            if (type.Name == "RawData_MTuning")
                            {
                                var rawProps = rawData.GetType().GetProperties();
                                var bowProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(rawProps);
                                if (bowProps.Length != 4)
                                {
                                }
                                else
                                {
                                    if (bowProps[0].Name == "Path")
                                    {
                                        var PathObject = bowProps[0].GetValue(rawData);//"D:\\CoarseTuning_demo_镭神机台\\WindowsForms_CoarseTuningDemo_dotNet\\bin\\Debug\\MirrorTuning_格式化后.csv";// 
                                        string Path = PathObject.ToString();
                                        if (!string.IsNullOrEmpty(Path))
                                        {
                                            //Task.Factory.StartNew(() =>
                                            //{
                                            if (string.IsNullOrEmpty(deviceData.CoarseTuningPath))
                                            {
                                                //    this.Invoke((EventHandler)delegate
                                                //    {
                                                panel_coarsetuning.Controls.Clear();
                                                CoaresTuningOverView coares = new CoaresTuningOverView(Path);
                                                coares.TopLevel = false;
                                                coares.Dock = DockStyle.Fill;
                                                panel_coarsetuning.Controls.Add(coares);
                                                coares.Show();
                                                var CoarseTuningPath = coares.SaveCVS(deviceData.SerialNumber, deviceData.MaskName, deviceData.WaferName,
                                                    deviceData.ChipName, deviceData.OeskID, deviceData.Tec1ActualTemp, deviceData.CurrentDateTime);
                                                deviceData.CoarseTuningPath = CoarseTuningPath;

                                                panel_coarse.Controls.Clear();
                                                //string CoarseTuningPath = @"D:\CT-3103\LaserX_TesterLibrary\Data\Coarse_tuning\DO123\TM346\T7891\(SW_EXAMPLE)\CoarseTuning\DO123_TM346_T7891_CoarseTuning#Deviations@55.00C_2024-04-11_14-50-22.csv";
                                                Form_CoarseTuning form_Coarse = new Form_CoarseTuning(CoarseTuningPath);
                                                form_Coarse.TopLevel = false;
                                                form_Coarse.FormBorderStyle = FormBorderStyle.None;
                                                form_Coarse.Dock = DockStyle.Fill;
                                                panel_coarse.Controls.Add(form_Coarse);
                                                form_Coarse.Show();
                                                //        });
                                            }
                                            //});
                                        }


                                    }
                                }
                            }
                        }
                    }
                    #region
                    //foreach (var rawData in deviceData.RawDataCollection)
                    //{
                    //    if (rawData is IRawDataMenuCollection)
                    //    {
                    //        //var rawDataMenuCollection = (IRawDataMenuCollection)rawData;
                    //        //foreach (var rdata in rawDataMenuCollection.GetDataMenuCollection())
                    //        //{
                    //        //    var rawProps = rdata.GetType().GetProperties();
                    //        //    var bowProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(rawProps);
                    //        //    if (bowProps.Length != 1)
                    //        //    {
                    //        //        break;
                    //        //    }
                    //        //    if (bowProps[0].Name == "Path")
                    //        //    {
                    //        //        var Path = bowProps[0].GetValue(rawDataMenuCollection);
                    //        //        panel_coarsetuning.Controls.Clear();
                    //        //        CoaresTuningOverView coares = new CoaresTuningOverView(Path.ToString());
                    //        //        coares.TopLevel = false;
                    //        //        coares.Dock = DockStyle.Fill;
                    //        //        panel_coarsetuning.Controls.Add(coares);
                    //        //        coares.Show();
                    //        //        //coarseTuning.ReadMirrorMapWavelengthFileAndSetupItuHelper(Path.ToString());
                    //        //        //coarseTuning.GroupWavelegnthValues();
                    //        //        //coarseTuning.PopulateMidlines();
                    //        //        //coarseTuning.GetAllItuChannels();
                    //        //        //coarseTuning.PlotWavelengthGroups(ref this.Chart_Groups);
                    //        //        //coarseTuning.PlotMidlines(ref this.Chart_Midlines);
                    //        //        //coarseTuning.PlotLabeledItuChannels(ref this.Chart_LabeledPoints);
                    //        //    }
                    //        //}
                    //    }
                    //    else
                    //    {
                    //        var rawProps = rawData.GetType().GetProperties();
                    //        var bowProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(rawProps);
                    //        if (bowProps.Length != 4)
                    //        {
                    //            continue;
                    //        }
                    //        if (bowProps[0].Name == "Path")
                    //        {
                    //            var Path = bowProps[0].GetValue(rawData);//"D:\\CoarseTuning_demo_镭神机台\\WindowsForms_CoarseTuningDemo_dotNet\\bin\\Debug\\MirrorTuning_格式化后.csv";// 

                    //            panel_coarsetuning.Controls.Clear();
                    //            CoaresTuningOverView coares = new CoaresTuningOverView(Path.ToString());
                    //            coares.TopLevel = false;
                    //            coares.Dock = DockStyle.Fill;
                    //            panel_coarsetuning.Controls.Add(coares);
                    //            coares.Show();
                    //            //coarseTuning.ReadMirrorMapWavelengthFileAndSetupItuHelper(Path.ToString());
                    //            //coarseTuning.GroupWavelegnthValues();
                    //            //coarseTuning.PopulateMidlines();
                    //            //coarseTuning.GetAllItuChannels();
                    //            //coarseTuning.PlotWavelengthGroups(ref this.Chart_Groups);
                    //            //coarseTuning.PlotMidlines(ref this.Chart_Midlines);
                    //            //coarseTuning.PlotLabeledItuChannels(ref this.Chart_LabeledPoints);
                    //        }
                    //    }
                    //}
                    #endregion

                    #endregion
                });
            }
            catch (Exception ex)
            {
                this._core.Log_Global(ex.Message);
            }
        }

        private void Form_PluginRuntimeOverview_Load(object sender, EventArgs e)
        {

            for (int pageIndex = 0; pageIndex <= MAX_RAW_DATA_PAGE; pageIndex++)
            {
                Form_RawDataViewer dataViewer = new Form_RawDataViewer();
                // Form_RawDataViewer_FF dataViewer = new Form_RawDataViewer_FF();

                UIGeneric.ModifyDockableUI(dataViewer, true);

                TabPage tp = new TabPage(string.Format(PAGE_TEXT_FORMAT, pageIndex));
                tp.Controls.Add(dataViewer);
                dataViewer.Show();

                RawDataViewerForms.Add(pageIndex, dataViewer);
                RawDataViewerFormPages.Add(pageIndex, tp);
            }


        }
        public virtual AccessPermissionLevel APL
        {
            get { return AccessPermissionLevel.None; }
        }
        //public virtual void ConnectToRuntimeOverviewer(ITestPluginRuntimeOverview viewer)
        //{
        //    this._viewer = viewer;
        //}

        public virtual void ConnectToCore(ITesterCoreInteration core)
        {
            _core = core as ITesterCoreInteration;
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core.SendOutFormCoreToGUIEvent += ReceiveMessageFromCore;
        }

        public virtual void DisconnectFromCore(ITesterCoreInteration core)
        {
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core = null;
        }
        protected virtual void ReceiveMessageFromCore(IMessage message)
        {
        }

        private void Form_PluginRuntimeOverview_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            //this.refreshTimer.Stop();
        }
        private void treeView_MainStreamData_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (this.chk_autoRefresh.Checked == true)
            {
                return;
            }

            if (this.treeView_MainStreamData.SelectedNode == null)
            {
                return;
            }
            this.Invoke((EventHandler)delegate
            {
                try
                {
                    var destDeviceNodeLevel = 2;

                    if (this.treeView_MainStreamData.SelectedNode.Level != destDeviceNodeLevel)
                    {
                        return;
                    }
                    //var majorData = this._viewer.GetMajorStreamData();
                    var majorData = this._localMajorStreamData;
                    if (majorData == null)
                    {
                        return;
                    }
                    if (majorData.MinorStreamDataCollectionCount <= 0)
                    {
                        return;
                    }


                    var nodePath = this.treeView_MainStreamData.SelectedNode.FullPath;
                    var nodePathArr = nodePath.Split(new string[] { "$$" }, StringSplitOptions.None);


                    var lastMinorData = majorData.GetMinorStreamData(nodePathArr[1]);

                    if (lastMinorData == null ||
                        lastMinorData.DeviceStreamDataCollectionCount <= 0)
                    {
                        return;
                    }

                    var deviceData = lastMinorData.GetDeviceStreamData(nodePathArr[2]);
                    if (deviceData == null)
                    {
                        return;
                    }
                    if (deviceData.RawDataCollecetionCount <= 0)
                    {
                        //return;
                    }
                    else
                    {
                        var rawDataCount = deviceData.RawDataCollecetionCount;

                        this.UpdateDataChartsPage(deviceData.RawDataCollection, deviceData.SerialNumber);


                        for (int pageIndex = 0; pageIndex <= MAX_RAW_DATA_PAGE; pageIndex++)
                        {
                            if (pageIndex >= rawDataCount)
                            {
                                RawDataViewerFormPages[pageIndex].Parent = null;
                            }
                            else
                            {
                                RawDataViewerFormPages[pageIndex].Parent = this.tb_dataViewer;
                            }
                        }
                        for (int rawDataIndex = 0; rawDataIndex < rawDataCount; rawDataIndex++)
                        {
                            RawDataViewerForms[rawDataIndex].ClearContext();
                            //RawDataViewerForms[rawDataIndex].FFChartSaveImage_SW = false;//不能再写图片了会重复
                            RawDataViewerForms[rawDataIndex].ImportRawData(deviceData.RawDataCollection[rawDataIndex]);

                            string _name = deviceData.RawDataCollection[rawDataIndex].Name.Split(']')[deviceData.RawDataCollection[rawDataIndex].Name.Split(']').Length - 2].Remove(0, 12);
                            // string ModeleName = _name.Remove(0, 12);
                            RawDataViewerFormPages[rawDataIndex].Text = $"{deviceData.Information}_{_name}";// string.Format(PAGE_TEXT_FORMAT, deviceData.RawDataCollection[rawDataIndex].Name);

                            // RawDataViewerFormPages[rawDataIndex].Text = $"{deviceData.Information}{deviceData.RawDataCollection[rawDataIndex].Name}";// string.Format(PAGE_TEXT_FORMAT, deviceData.RawDataCollection[rawDataIndex].Name);                            
                        }
                    }

                    if (deviceData.SummaryDataCollection.Count <= 0)
                    {
                        this.tb_dataViewer.TabPages["tp_SummaryData"].Text = "SummaryData";
                        //return;
                    }
                    else
                    {
                        this.tb_dataViewer.TabPages["tp_SummaryData"].Text = $"{deviceData.Information}_SummaryData";
                        this.pdgv_summaryData.Clear();
                        this.pdgv_summaryData.ImportSourceData(deviceData.SummaryDataCollection, true);
                    }

                    this.tp_CoarseTuning.Text = $"{deviceData.Information}_CoarseTuning";
                    this.tp_coarse.Text = $"{deviceData.Information}_CoarseTuning";
                    if (panel_coarsetuning.Controls.Count > 0)
                    {
                        foreach (Control item in panel_coarsetuning.Controls)
                        {
                            item.Dispose();
                        }
                    }
                    panel_coarsetuning.Controls.Clear();
                    if (panel_coarse.Controls.Count > 0)
                    {
                        foreach (Control item in panel_coarse.Controls)
                        {
                            item.Dispose();
                        }
                    }
                    panel_coarse.Controls.Clear();
                    //Irwin 20240407 忆芯定制
                    if (deviceData.RawDataCollecetionCount > 1)
                    {
                        //var rawData = deviceData.RawDataCollection[7];
                        foreach (var rawData in deviceData.RawDataCollection)
                        {
                            var type = rawData.GetType();
                            if (type.Name == "RawData_MTuning")
                            {
                                var rawProps = rawData.GetType().GetProperties();
                                var bowProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(rawProps);
                                if (bowProps.Length != 4)
                                {
                                }
                                else
                                {
                                    if (bowProps[0].Name == "Path")
                                    {
                                        var PathObject = bowProps[0].GetValue(rawData);//"D:\\CoarseTuning_demo_镭神机台\\WindowsForms_CoarseTuningDemo_dotNet\\bin\\Debug\\MirrorTuning_格式化后.csv";// 
                                        string Path = PathObject.ToString();
                                        if (!string.IsNullOrEmpty(Path))
                                        {
                                            panel_coarsetuning.Controls.Clear();
                                            CoaresTuningOverView coares = new CoaresTuningOverView(Path);
                                            coares.TopLevel = false;
                                            coares.Dock = DockStyle.Fill;
                                            panel_coarsetuning.Controls.Add(coares);
                                            coares.Show();
                                            //var CoarseTuningPath = coares.SaveCVS(deviceData.MaskName, deviceData.WaferName,
                                            //    deviceData.ChipName, deviceData.OeskID, deviceData.Tec1ActualTemp, deviceData.CurrentDateTime);
                                            //deviceData.CoarseTuningPath = CoarseTuningPath;

                                            panel_coarse.Controls.Clear();
                                            //string CoarseTuningPath = @"D:\CT-3103\LaserX_TesterLibrary\Data\Coarse_tuning\DO123\TM346\T7891\(SW_EXAMPLE)\CoarseTuning\DO123_TM346_T7891_CoarseTuning#Deviations@55.00C_2024-04-11_14-50-22.csv";
                                            Form_CoarseTuning form_Coarse = new Form_CoarseTuning(deviceData.CoarseTuningPath);
                                            form_Coarse.TopLevel = false;
                                            form_Coarse.FormBorderStyle = FormBorderStyle.None;
                                            form_Coarse.Dock = DockStyle.Fill;
                                            panel_coarse.Controls.Add(form_Coarse);
                                            form_Coarse.Show();
                                        }
                                    }
                                }
                            }
                        }
                    }



                }
                catch (Exception ex)
                {
                    this._core.Log_Global(ex.Message);
                }
            });
        }


        private void chk_autoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chk_autoRefresh.Checked == true)
            {
                this.UpdateMainStreamData(this._localMajorStreamData);
            }
        }


    }
}