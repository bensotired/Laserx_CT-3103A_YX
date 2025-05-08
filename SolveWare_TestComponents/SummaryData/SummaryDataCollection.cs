using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SolveWare_TestComponents
{
    [Serializable]

    public class SummaryDataCollection : CURDBaseLite<SummaryDatumItemBase>
    {
        public override void AddSingleItem(SummaryDatumItemBase item)
        {
            if (this.ItemCollection.Exists(x => x.Name == item.Name))
            {
                //MessageBox.Show($"算子[{item.Name}]有重复，请检查");
                throw new Exception($"Duplicate items with Name ={item.Name} !");
            }
            this.ItemCollection.Add(item);
        }
        public virtual Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (var item in this.ItemCollection)
            {
                dict.Add(item.Name, item.Value);
            }
            return dict;
        }
        public override string ToString()
        {
            StringBuilder strb = new StringBuilder();
            try
            {
                string beNames = string.Empty;
                string beValues = string.Empty;
                foreach (var item in this.ItemCollection)
                {
                    var name = item.Name;
                    beNames += $"{name},";

                    if (item.Value == null)
                    {
                        beValues += ",";
                    }
                    else
                    {
                        var dblVal = 0.0;
                        if (double.TryParse(item.Value.ToString(), out dblVal))
                        {
                            if (double.IsNaN(dblVal) ||
                                double.IsInfinity(dblVal) ||
                                double.IsNegativeInfinity(dblVal) ||
                                double.IsPositiveInfinity(dblVal))
                            {
                                beValues += $"{0.0},";

                            }
                            else
                            {
                                beValues += $"{dblVal},";

                            }
                        }
                        else
                        {
                            //beValues += $"{0.0},";
                            beValues += $"{item.Value},";
                        }

                    }
                }
                beNames = beNames.Trim(',');
                beValues = beValues.Trim(',');
                strb.AppendLine(beNames);
                strb.AppendLine(beValues);
            }
            catch (Exception ex)
            {

            }

            return strb.ToString();
        }
        public virtual string ValueToString()
        {
            StringBuilder strb = new StringBuilder();
            try
            {
                //string beNames = string.Empty;
                string beValues = string.Empty;
                foreach (var item in this.ItemCollection)
                {
                    //var name = item.Name;
                    // beNames += $"{name},";

                    if (item.Value == null)
                    {
                        beValues += ",";
                    }
                    else
                    {
                        var dblVal = 0.0;
                        if (double.TryParse(item.Value.ToString(), out dblVal))
                        {
                            if (double.IsNaN(dblVal) ||
                                double.IsInfinity(dblVal) ||
                                double.IsNegativeInfinity(dblVal) ||
                                double.IsPositiveInfinity(dblVal))
                            {
                                beValues += $"{0.0},";

                            }
                            else
                            {
                                beValues += $"{dblVal},";

                            }
                        }
                        else
                        {
                            beValues += $"{0.0},";

                        }

                    }
                }
                //beNames = beNames.Trim(',');
                beValues = beValues.Trim(',');
                // strb.AppendLine(beNames);
                strb.AppendLine(beValues);
            }
            catch (Exception ex)
            {

            }

            return strb.ToString();
        }
        public string ToCSV(string StartTime, string SN,string OperatorID)
        {
            StringBuilder strb = new StringBuilder();
            try
            {
                var EndTime = DateTime.Now.ToString();
                string beNames = string.Empty;
                string beValues = string.Empty;
                beNames += "SN,OperatorID,StartTime,EndTime,Result,FailureCode,";

                string isPassed = "PASS";
                string FailureCode = string.Empty;
                foreach (var item in this.ItemCollection)
                {
                    if (item.Judegment == SummaryDatumJudegment.FAIL)
                    {
                        isPassed = "FAIL";
                        FailureCode += $"{item.FailureCode};";
                    }
                }
                beValues += $"{SN},{OperatorID},{StartTime},{EndTime},{isPassed},{FailureCode},";

                foreach (var item in this.ItemCollection)
                {
                    var name = item.Name;
                    beNames += $"{name},";

                    if (item.Value == null)
                    {
                        beValues += ",";
                    }
                    else
                    {
                        var dblVal = 0.0;
                        if (double.TryParse(item.Value.ToString(), out dblVal))
                        {
                            if (double.IsNaN(dblVal) ||
                                double.IsInfinity(dblVal) ||
                                double.IsNegativeInfinity(dblVal) ||
                                double.IsPositiveInfinity(dblVal))
                            {
                                beValues += $"{0.0},";

                            }
                            else
                            {
                                beValues += $"{dblVal},";

                            }
                        }
                        else
                        {
                            beValues += $"{0.0},";

                        }

                    }

                }
                beNames = beNames.Trim(',');
                beValues = beValues.Trim(',');
                strb.AppendLine(beNames);
                strb.Append(beValues);
            }
            catch (Exception ex)
            {
            }

            return strb.ToString();
        }

        public string AppendToCSV(string StartTime, string SN,string OperatorID)
        {
            StringBuilder strb = new StringBuilder();
            string beValues = string.Empty;
            try
            {
                var EndTime = DateTime.Now.ToString();

                string isPassed = "PASS";
                string FailureCode = string.Empty;
                foreach (var item in this.ItemCollection)
                {
                    if (item.Judegment == SummaryDatumJudegment.FAIL)
                    {
                        isPassed = "FAIL";
                        FailureCode += $"{item.FailureCode};";
                    }
                }
                beValues += $"{SN},{OperatorID},{StartTime},{EndTime},{isPassed},{FailureCode},";

                foreach (var item in this.ItemCollection)
                {
                    if (item.Value == null)
                    {
                        beValues += ",";
                    }
                    else
                    {
                        var dblVal = 0.0;
                        if (double.TryParse(item.Value.ToString(), out dblVal))
                        {
                            if (double.IsNaN(dblVal) ||
                                double.IsInfinity(dblVal) ||
                                double.IsNegativeInfinity(dblVal) ||
                                double.IsPositiveInfinity(dblVal))
                            {
                                beValues += $"{0.0},";

                            }
                            else
                            {
                                beValues += $"{dblVal},";

                            }
                        }
                        else
                        {
                            beValues += $"{0.0},";

                        }

                    }
                }
                beValues = beValues.Trim(',');
            }
            catch (Exception ex)
            {
            }
            return beValues;
        }
        public virtual void PrintToCSV(string filePath, bool writeHeader, bool append = false)
        {
            using (StreamWriter sw = new StreamWriter(filePath, append, Encoding.UTF8))
            {
                if (writeHeader == true)
                {
                    sw.Write(this.ToString());
                }
                else
                {
                    sw.Write(this.ValueToString());
                }
            }
        }
    }
}