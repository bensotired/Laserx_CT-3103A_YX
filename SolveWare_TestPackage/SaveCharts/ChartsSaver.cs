using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace SolveWare_TestPackage
{
    public static class ChartsSaver
    {
        static void UpdateRawDataMenuChart_V3(Form_ModuleChart chartUI, IRawDataMenuCollection rawDataMenuCollection)
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
        static void UpdateRawDataChart_V3(Form_ModuleChart chartUI, IRawDataCollectionBase rawDataCollection)
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
        static bool IsUpdateRawDataChart_V2(IRawDataCollectionBase rawDataCollection)
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

        public static void SaveCharts(RawDataBaseLite rawData , string chartImagePath)
        {
            Form_ModuleChart chartUI = new Form_ModuleChart();
            try
            {

                chartUI.Show();
                chartUI.Hide();
                chartUI.ClearChart();
                if (rawData is IRawDataMenuCollection)
                {
                    var rawDataMenuCollection = (IRawDataMenuCollection)rawData;
                    if (rawDataMenuCollection.Count <= 0)
                    {

                    }
                    else
                    {
                        UpdateRawDataMenuChart_V3(chartUI, rawDataMenuCollection);
                        chartUI.SaveChart(chartImagePath);
                    }
                }
                else if (rawData is IRawDataCollectionBase)
                {
                    if (IsUpdateRawDataChart_V2((IRawDataCollectionBase)rawData)) //先判断要不要画图
                    {
                        UpdateRawDataChart_V3(chartUI, (IRawDataCollectionBase)rawData); 
                        chartUI.SaveChart(chartImagePath);
                    }
                }

            }
            catch(Exception ex)
            {

            }
            finally
            {
                try
                {
                    chartUI.Close();
                    chartUI.Dispose();
                }
                catch
                {

                }
            }
            
        }
    }
}