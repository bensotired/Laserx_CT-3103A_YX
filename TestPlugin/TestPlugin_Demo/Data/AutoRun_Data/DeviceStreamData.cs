using LX_BurnInSolution.Utilities;
using SolveWare_BinSorter;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace TestPlugin_Demo
{

    //[XmlInclude(typeof(RawDataBaseLite))]
    [Serializable]
    public class DeviceStreamData_CT3103 : DeviceStreamDataBase<DeviceInfo_CT3103>, IDeviceStreamDataBase
    {
        public DeviceStreamData_CT3103() : base()
        {
            this.DeviceInfo = new DeviceInfo_CT3103();
        }
     
        public override string GetSQL_Header()
        {
            //irwin请更新详细条件
            var header =
                "WorkOrder," +
                "SubOrder," +
                "PartNumber," +
                "CarrierID," +
                "ChipID," +
                "Station," +
                "Purpose," +
                "Result," +
                "StartTime"+
                "EndTime";
            return header;
        }
        public override string GetSQL_HeaderValues()
        {
            var headervalues = $"" +
                $"'{this.DeviceInfo.WorkOrder}'," +
                //$"'{this.DeviceInfo.SubOrder}'," +
                $"'{this.DeviceInfo.PartNumber}'," +
                $"'{this.DeviceInfo.CarrierID}'," +
                $"'{this.DeviceInfo.ChipID}'," +
                $"'{this.DeviceInfo.Station}'," +
                $"'{this.CreateTime}',"+
                $"'{this.DeviceInfo.EndTime}'";
            return headervalues;
        }
        public override string Information
        {
            get
            {

                if (this.SerialNumber == null)
                {
                    return "0";
                }
                return $"{this.SerialNumber}";
            }
        }
        public string GetWaferSummaryFileLineValue(List<string> dynamicValueHeader)
        {
            StringBuilder strb = new StringBuilder();
            try
            {
                var thisProps = this.GetType().GetProperties();
                Dictionary<string, PropertyInfo> finalNamePropDict = new Dictionary<string, PropertyInfo>();
                foreach (var prop in thisProps)
                {
                    if (PropHelper.IsPropertyBelongs<DisplayNameAttribute>(prop))
                    {
                        var headerName = PropHelper.GetAttributeValue<DisplayNameAttribute>(prop).DisplayName;

                        finalNamePropDict.Add(headerName, prop);
                    }
                    else
                    {
                        var propName = prop.Name;
                        finalNamePropDict.Add(propName, prop);
                    }
                }

                foreach (var header in dynamicValueHeader)
                {
                    //固有属性 带别名 寻找所需表头对应的值
                    if (finalNamePropDict.ContainsKey(header))
                    {
                        var value = finalNamePropDict[header].GetValue(this);
                        strb.Append($"{value},");
                        continue;
                    }
                    //测试数据 寻找所需表头对应的值
                    //if (this.SummaryDataCollection.ItemCollection.Exists(item => item.Name == header))
                    //{
                    //    var value = this.SummaryDataCollection.ItemCollection.Find(item => item.Name == header).Value;
                    //    strb.Append($"{value},");
                    //    continue;
                    //}
                    if (this.SummaryDataCollection.ItemCollection.Exists(item => item.Name.ToLower() == header.ToLower()))
                    {
                        var value = this.SummaryDataCollection.ItemCollection.Find(item => item.Name.ToLower() == header.ToLower()).Value;
                        var dblVal = 0.0;
                        if (double.TryParse(value.ToString(), out dblVal))
                        {
                            if (double.IsNaN(dblVal) ||
                                double.IsInfinity(dblVal) ||
                                double.IsNegativeInfinity(dblVal) ||
                                double.IsPositiveInfinity(dblVal))
                            {
                                strb.Append($"{0.0},");
                            }
                            else
                            {
                                strb.Append($"{dblVal},");
                            }
                        }
                        else
                        {
                            strb.Append($"{0.0},");
                        }
                        continue;
                    }
                    //以上两个都没有找到 则为空数据
                    strb.Append($",");
                }
            }
            catch (Exception ex)
            {

            }
            return strb.ToString();
        }
        public string GetBinGrade(BinSettingCollection binSettingCollection)
        {
            bool isInBinGrade = true;
            string binname = "NG";
            //多方案
            foreach (var binsetting in binSettingCollection)
            {
                isInBinGrade = true;
                var currentBinName = binsetting.Name;

                //单方案
                foreach (var binJudgeItem in binsetting)
                {
                    if (binJudgeItem.Enable == false)
                    {
                        break;
                    }
                    var binItemName = binJudgeItem.Name;

                    if (this.SummaryDataCollection.ItemCollection.Exists(item => item.Name.ToLower() == binItemName.ToLower()))
                    {
                        var summaryData = this.SummaryDataCollection.ItemCollection.Find(item => item.Name.ToLower() == binItemName.ToLower());

                        var sValue = summaryData.Value;
                        if (JuniorMath.IsValueInLimitRange(Convert.ToDouble(sValue), binJudgeItem.LL, binJudgeItem.UL))
                        {
                            //在该bin单项参数范围内
                        }
                        else
                        {
                            isInBinGrade = false;
                            //不在该bin单项参数范围内
                            //break;
                        }
                    }
                    else
                    {
                        isInBinGrade = false;
                        //该bin设置内所需参数 在总测试结果参数内不存在 则流向下一bin设置
                        break;
                    }
                    if (isInBinGrade == true)
                    {
                        return currentBinName;
                    }
                }

                //if (isInBinGrade == true)
                //{
                //    binname = currentBinName;
                //}
            }
            //if (isInBinGrade == true)
            //{
            //    return binname;
            //}
            return "NG";
        }

        public SummaryDataCollection GetBinSummary(List<string> binSummaryNames)
        {
            SummaryDataCollection temp = new SummaryDataCollection();
            try
            {
                var tempItems = this.SummaryDataCollection.ItemCollection.FindAll(item => binSummaryNames.Contains(item.Name));
                temp.ItemCollection.AddRange(tempItems);
            }
            catch
            {

            }
            return temp;
        }
    }
}