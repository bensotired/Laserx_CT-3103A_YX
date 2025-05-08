using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;
using System.Collections.Generic;
using System.IO;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class DeviceStreamDataLite : StreamInfoBase
    {
        public IRawDataBaseLite RawData { get; set; }
        public SummaryDataCollection SummaryDataCollection { get; set; }
        public DeviceStreamDataLite()
        {
            this.SummaryDataCollection = new SummaryDataCollection();
        }
        public void AddSummaryDataCollection(List<SummaryDatumItemBase> summaryDataCollection)
        {
            foreach (var item in summaryDataCollection)
            {
                this.SummaryDataCollection.AddSingleItem(item);
            }
        }
        public void AddSingleSummaryData(SummaryDatumItemBase summaryData)
        {
            this.SummaryDataCollection.AddSingleItem(summaryData);
        }

        public override Type[] GetIncludingTypes()
        {
            return new Type[] { RawData?.GetType() };
        }
        public virtual void PrintToCSV(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                sw.WriteLine(this.SummaryDataCollection.ToString());
                sw.WriteLine();
                if (this.RawData is IRawDataMenuCollection)
                {
                    var rawd = this.RawData as IRawDataMenuCollection;
                    var props = rawd.GetType().GetProperties();
                    var broEleProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(props);

                    string beNames = string.Empty;
                    string beValues = string.Empty;
                    foreach (var item in broEleProps)
                    {
                        var name = item.Name;
                        beNames += $"{name},";

                        var value = item.GetValue(rawd);

                        if (value == null)
                        {
                            beValues += ",";
                        }
                        else
                        {
                            beValues += $"{value},";
                        }
                    }
                    beNames = beNames.Trim(',');
                    beValues = beValues.Trim(',');
                   
                    sw.WriteLine(beNames);
                    sw.WriteLine(beValues);
                    sw.WriteLine();
                    foreach (var item in rawd.GetDataMenuCollection())
                    {
                        sw.WriteLine(item.ToString());
                        
                        sw.WriteLine();
                        
                    }
                }
                else
                {
                    sw.WriteLine(this.RawData.ToString());
                }

            }
        }
    }
}