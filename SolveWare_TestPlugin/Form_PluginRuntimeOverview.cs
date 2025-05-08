using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.UIComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SolveWare_TestPlugin
{
    public partial class Form_PluginRuntimeOverview : Form, ITestDataViewer//, ITesterCoreLink, IAccessPermissionLevel// where TTestPlugin : class, ITesterAppPluginInteration
    {
        //System.Timers.Timer refreshTimer = new System.Timers.Timer(2000);
        protected ITesterCoreInteration _core;
        protected IMajorStreamData _localMajorStreamData;
        //protected ITestPluginRuntimeOverview _viewer;
        const string SUMMARY_PAGE = "tp_SummaryData";
        const string DATA_CHARTS_PAGE = "tp_DataCharts";
        const int MAX_RAW_DATA_PAGE = 20;
        const string PAGE_TEXT_FORMAT = "TP_RAWDATA_{0}";
        Dictionary<int, Form_RawDataViewer> RawDataViewerForms = new Dictionary<int, Form_RawDataViewer>();
        Dictionary<int, TabPage> RawDataViewerFormPages = new Dictionary<int, TabPage>();
        Form_TLP_layer _dataChartsLayer;
        public Form_PluginRuntimeOverview()
        {
            InitializeComponent();
            RawDataViewerForms.Clear();
            RawDataViewerFormPages.Clear();
            //refreshTimer.Elapsed += RefreshTimer_Elapsed;
            //refreshTimer.Start();

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
                this._dataChartsLayer.ClearCharts();
                this.pdgv_summaryData.Clear();
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

        void UpdateDataChartsPage(List<RawDataBaseLite> rawDataCollection)
        {
            //this._core.GUIRunUIInvokeAction(() =>
            //{
            _dataChartsLayer.ClearCharts();

            if (rawDataCollection.Count <= 0)
            {
                return;
            }

            List<Form_ModuleChart> chartUIs = new List<Form_ModuleChart>();
            foreach (var rawData in rawDataCollection)
            {
                if (rawData is IRawDataCollectionBase)
                {
                    Form_ModuleChart chartUI = new Form_ModuleChart();
                    chartUI.ClearChart();
                    UpdateRawDataChart_V2(chartUI, (IRawDataCollectionBase)rawData);
                    //this.UpdateRawDataChart(chartUI, (IRawDataCollectionBase)rawData);
                    chartUIs.Add(chartUI);
                }
                else
                {
                }
            }

            if (chartUIs.Count <= 0)
            {
                return;
            }
            if (chartUIs.Count > 4)
            {
                _dataChartsLayer.LayoutSubForms(chartUIs.ToArray(), 2);
       
            }
            else
            {
                _dataChartsLayer.LayoutSubForms(chartUIs.ToArray(), 2, 2);
              
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
        public virtual void UpdateMainStreamData(IMajorStreamData majorData , string targetDeviceSn)
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

                    //var deviceData = lastMinorData.GetDeviceStreamDataCollection().Last();

                    var deviceData = lastMinorData.GetDeviceStreamDataCollection().Find(item => item.SerialNumber == targetDeviceSn);

                    if(deviceData == null)
                    {
                        return;
                    }
                 
                    if (deviceData.RawDataCollecetionCount <= 0)
                    {

                    }
                    else
                    {
                        var rawDataCount = deviceData.RawDataCollecetionCount;


                        this.UpdateDataChartsPage(deviceData.RawDataCollection);


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

                            RawDataViewerFormPages[rawDataIndex].Text = $"{deviceData.Information}{deviceData.RawDataCollection[rawDataIndex].Name}";// string.Format(PAGE_TEXT_FORMAT, deviceData.RawDataCollection[rawDataIndex].Name);
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

                    #endregion
                });
            }
            catch (Exception ex)
            {

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


                        this.UpdateDataChartsPage(deviceData.RawDataCollection);


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

                            RawDataViewerFormPages[rawDataIndex].Text = $"{deviceData.Information}{deviceData.RawDataCollection[rawDataIndex].Name}";// string.Format(PAGE_TEXT_FORMAT, deviceData.RawDataCollection[rawDataIndex].Name);
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

                    #endregion
                });
            }
            catch (Exception ex)
            {

            }
        }
   
        private void Form_PluginRuntimeOverview_Load(object sender, EventArgs e)
        {
            _dataChartsLayer = new Form_TLP_layer();
            _dataChartsLayer.Show();
            this.tb_dataViewer.TabPages[DATA_CHARTS_PAGE].Controls.Clear();
            this.tb_dataViewer.TabPages[DATA_CHARTS_PAGE].Controls.Add(_dataChartsLayer);

            for (int pageIndex = 0; pageIndex <= MAX_RAW_DATA_PAGE; pageIndex++)
            {
                Form_RawDataViewer dataViewer = new Form_RawDataViewer();

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

                        this.UpdateDataChartsPage(deviceData.RawDataCollection);


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
                            RawDataViewerFormPages[rawDataIndex].Text = $"{deviceData.Information}{deviceData.RawDataCollection[rawDataIndex].Name}";// string.Format(PAGE_TEXT_FORMAT, deviceData.RawDataCollection[rawDataIndex].Name);
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
                        this.pdgv_summaryData.ImportSourceData (deviceData.SummaryDataCollection, true);
                    }
                }
                catch
                {

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

        //private void chk_autoRefresh_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.chk_autoRefresh.Checked == true)
        //    {
        //        refreshTimer.Enabled = true;
        //    }
        //    else if (this.chk_autoRefresh.Checked == false)
        //    {
        //        refreshTimer.Enabled = false;
        //    }
        //}


    }
}