using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SolveWare_DataAnalayzer
{
    public class StandardAnalyzerResult
    {
        public double Max { get; set; }
        public double Min { get; set; }
        public double MaxDelta { get; set; }
        public int count_delBelow3 { get; set; }
        public int count_delBetween3_5 { get; set; }
        public int count_delBetween5_10 { get; set; }
        public int count_delAbove10 { get; set; }
    }
    public class StandardAnalyzer
    {
        public virtual List<object> Run(List<string> array)
        {
            var dblList = array.ConvertAll<double>((item) => { return Convert.ToDouble(item); });
            return this.Run(dblList);
        }
        public virtual List<object> Run(List<double> array)
        {
            var max = array.Max();
            var min = array.Min();
            var maxDelta = (max - min) / min * 100;
            List<double> deltaVals = new List<double>();
            int count_DelBelow3 = 0;
            int count_delBetween3_5 = 0;
            int count_delBetween5_10 = 0;
            int count_delAbove10 = 0;
            foreach (var dbl in array)
            {
                deltaVals.Add((max - dbl) / min * 100);
            }
            count_DelBelow3 = deltaVals.FindAll(item => item >= 0 && item <= 3).Count;
            count_delBetween3_5 = deltaVals.FindAll(item => item > 3 && item <= 5).Count;
            count_delBetween5_10 = deltaVals.FindAll(item => item > 5 && item <= 10).Count;
            count_delAbove10 = deltaVals.FindAll(item => item > 10).Count;
            List<object> result = new List<object>();
            result.Add(max);
            result.Add(min);
            result.Add(maxDelta);
            result.Add(count_DelBelow3);
            result.Add(count_delBetween3_5);
            result.Add(count_delBetween5_10);
            result.Add(count_delAbove10);
            return result;
        }
        public virtual StandardAnalyzerResult RunStardardResult(List<string> array)
        {
            var dblList = array.ConvertAll<double>((item) => { return Convert.ToDouble(item); });
            return this.RunStardardResult(dblList);
        }
        public virtual StandardAnalyzerResult RunStardardResult(List<double> array)
        {
            StandardAnalyzerResult result = new StandardAnalyzerResult();
            var max = array.Max();
            var min = array.Min();
            var maxDelta = (max - min) / min * 100;
            List<double> deltaVals = new List<double>();
            int count_DelBelow3 = 0;
            int count_delBetween3_5 = 0;
            int count_delBetween5_10 = 0;
            int count_delAbove10 = 0;
            foreach (var dbl in array)
            {
                deltaVals.Add((  dbl - min) / min * 100);
            }
            count_DelBelow3 = deltaVals.FindAll(item => item >= 0 && item <= 3).Count;
            count_delBetween3_5 = deltaVals.FindAll(item => item > 3 && item <= 5).Count;
            count_delBetween5_10 = deltaVals.FindAll(item => item > 5 && item <= 10).Count;
            count_delAbove10 = deltaVals.FindAll(item => item > 10).Count;

            result.Max = (max);
            result.Min = (min);
            result.MaxDelta = (maxDelta);
            result.count_delBelow3 = (count_DelBelow3);
            result.count_delBetween3_5 = (count_delBetween3_5);
            result.count_delBetween5_10 = (count_delBetween5_10);
            result.count_delAbove10 = (count_delAbove10);
            return result;
        }
    }
}