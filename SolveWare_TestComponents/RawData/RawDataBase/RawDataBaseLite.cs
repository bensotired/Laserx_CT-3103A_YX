using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SolveWare_TestComponents.Data
{
    [Serializable]

    public class RawDataBaseLite : IRawDataBaseLite
    {
        public RawDataBaseLite()
        {
            this.RawDataFixFormat = string.Empty;
        }
        string _name = string.Empty;
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        //public virtual string Tag { get; set; }
        public virtual void SetRawDataFixFormat(string preFix, string postFix)
        {
            this.RawDataFixFormat = preFix + "{0}" + postFix;
        }
        public virtual string RawDataFixFormat { get; set; }
        [RawDataBrowsableElement]
        public DateTime TestStepStartTime { get; set; }
        [RawDataBrowsableElement]
        public DateTime TestStepEndTime { get; set; }
        [RawDataBrowsableElement]
        public TimeSpan TestCostTimeSpan { get; set; }
        public override string ToString()
        {
            StringBuilder strb = new StringBuilder();
            try
            {
                var props = this.GetType().GetProperties();
                var broEleProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(props);

                string beNames = string.Empty;
                string beValues = string.Empty;
                foreach (var item in broEleProps)
                {
                    var name = item.Name;
                    beNames += $"{name},";

                    var value = item.GetValue(this);

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

                strb.AppendLine(beNames);
                strb.AppendLine(beValues);

                if (this is IRawDataCollectionBase)
                {
                    var rawDataCollection = this as IRawDataCollectionBase;
                    if (rawDataCollection.Count > 0)
                    {
                        strb.AppendLine();

                        var itemProps = rawDataCollection.Peek().GetType().GetProperties();
                        var colProps = PropHelper.GetAttributeProps<RawDataCollectionItemElementAttribute>(itemProps);
                        List<string> listname = new List<string>();
                        foreach (var item in colProps)
                        {
                            listname.Add(item.Name);
                        }
                        var dict = rawDataCollection.GetDataDictByPropNames<object>(listname.ToArray());

                        int count = dict.Values.First().Count;
                        List<string> keys = dict.Keys.ToList();
                        string colNameHeader = string.Empty;
                        foreach (var item in dict)
                        {
                            var name = item.Key;
                            colNameHeader += $"{name},";
                        }
                        colNameHeader = colNameHeader.Trim(',');
                        strb.AppendLine(colNameHeader);

                        for (int i = 0; i < count; i++)
                        {
                            string line = string.Empty;
                            foreach (var key in keys)
                            {
                                line += $"{dict[key][i] },";
                            }
                            line = line.Trim(',');
                            strb.AppendLine(line);
                        }
                        strb.AppendLine();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return strb.ToString();
        }
        public virtual void PrintToCSV(string filePath , bool append = false)
        {
            using (StreamWriter sw = new StreamWriter(filePath, append))
            {
                sw.Write(this.ToString());
            }
            return;
        }

        public virtual bool ReloadFormString(string reloadDataString)
        {
            return false;
        }
        public virtual bool ReloadFormString(string[] reloadDataString)
        {
            return false;
        }
    }
}