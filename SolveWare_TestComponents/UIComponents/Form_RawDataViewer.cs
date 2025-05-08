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
    public partial class Form_RawDataViewer : Form, IForm_RawDataViewer
    {
        Form_ModuleChart chartUI;
        //Form_ModuleFFChart ffchartUI;
        public string  SaveImagePath { set; get; }
        public bool FFChartSaveImage_SW { set; get; } = false;
        public string SN { set; get; } = "123";

        public Form_RawDataViewer()
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
                    if (rawData is IRawDataMenuCollection)
                    {
                        var rawDataMenuCollection = (IRawDataMenuCollection)rawData;
                        if (rawDataMenuCollection.Count <= 0)
                        {

                        }
                        else
                        {
                            this.UpdateRawDataCollection_DGV_V2(rawDataMenuCollection);
                            this.UpdateRawDataChart_V3(rawDataMenuCollection);
                        }
                    }else if (rawData is IRawDataCollectionBase)
                    {
                        var rawDataCollection = (IRawDataCollectionBase)rawData;
                        if (rawDataCollection.Count <= 0)
                        {

                        }
                        else
                        {
                            this.UpdateRawDataCollection_DGV(rawDataCollection);
                            this.UpdateRawDataChart_V2(rawDataCollection);
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
            if (rawData is IRawDataMenuCollection)
            {
                var rawDataMenuCollection = (IRawDataMenuCollection)rawData;
                foreach (var rdata in rawDataMenuCollection.GetDataMenuCollection())
                {
                    var rawProps = rdata.GetType().GetProperties();
                    var bowProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(rawProps);
                    foreach (var bp in bowProps)
                    {
                        var rowIndex = this.dgv_RawDataParameters.Rows.Add();
                        this.dgv_RawDataParameters.Rows[rowIndex].SetValues(bp.Name, bp.GetValue(rdata));
                    }
                }
            }
            else
            {
                var rawProps = rawData.GetType().GetProperties();
                var bowProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(rawProps);
                foreach (var bp in bowProps)
                {
                    var rowIndex = this.dgv_RawDataParameters.Rows.Add();
                    this.dgv_RawDataParameters.Rows[rowIndex].SetValues(bp.Name, bp.GetValue(rawData));
                }
            }

            //var rawProps = rawData.GetType().GetProperties();
            //var bowProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(rawProps);
            //foreach (var bp in bowProps)
            //{
            //    var rowIndex = this.dgv_RawDataParameters.Rows.Add();
            //    this.dgv_RawDataParameters.Rows[rowIndex].SetValues(bp.Name, bp.GetValue(rawData));
            //}
        
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
                    newCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
        private void UpdateRawDataCollection_DGV_V2(IRawDataMenuCollection rawDataMenuCollection)
        {
            try
            {
                Dictionary<string, List<object>> colValDict = new Dictionary<string, List<object>>();
                List<int> count = new List<int>();

                foreach (var rdata in rawDataMenuCollection.GetDataMenuCollection())
                {

                    var rawDataCollection = rdata as IRawDataCollectionBase;
                    var itemProps = rawDataCollection.Peek().GetType().GetProperties();

                    foreach (var prop in itemProps)
                    {
                        if (PropHelper.IsPropertyBelongs<RawDataCollectionItemElementAttribute>(prop))
                        {
                            var propValue = PropHelper.GetAttributeValue<RawDataCollectionItemElementAttribute>(prop);

                            var colVals = rawDataCollection.GetDataListByPropName_V2(prop.Name);

                            var colHeader = propValue.ElementTag;
                            if (!colValDict.ContainsKey(colHeader))
                            {
                                colValDict.Add(colHeader, colVals);
                            }
                            else
                            {
                                colValDict[colHeader].AddRange(colVals);
                            }

                        }
                    }

                }

                foreach (var kvp in colValDict)
                {
                    DataGridViewColumn newCol = new DataGridViewTextBoxColumn();
                    newCol.Name = kvp.Key;
                    newCol.HeaderText = kvp.Key;
                    newCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.dgv_RawDataCollection.Columns.Add(newCol);
                    count.Add(kvp.Value.Count);
                }
                var colWidth = this.dgv_RawDataCollection.Width / this.dgv_RawDataCollection.ColumnCount;
                foreach (DataGridViewColumn col in this.dgv_RawDataCollection.Columns)
                {
                    col.Width = colWidth;
                }

                this.dgv_RawDataCollection.RowCount = count.Max();

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
        private void UpdateRawDataChart_V3(IRawDataMenuCollection rawDataMenuCollection)
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
                                    this.chartUI.UpdateChartSeries(AxisType.Primary, legendName, xValues, values[item.Key]);
                                }
                                break;
                            case CEAxisXY.Y2:
                                {
                                    var legendName = $"{na[0]}_Y2";
                                    this.chartUI.UpdateChartSeries(AxisType.Secondary, legendName, xValues, values[item.Key]);
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

      


    }
}