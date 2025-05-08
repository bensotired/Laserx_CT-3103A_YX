using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Data;

namespace SolveWare_BurnInCommon
{
 
    public class SweepChartsRawData
    {
        public Dictionary<CEClass, Dictionary<CEAxisXY, Dictionary<string, Dictionary<int, List<object>>>>> YDataComplexDict =
           new Dictionary<CEClass, Dictionary<CEAxisXY, Dictionary<string, Dictionary<int, List<object>>>>>();
        public Dictionary<int, List<object>> XData = new Dictionary<int, List<object>>();
        public int SlotCount { get; set; }
        public void Clear()
        {
            YDataComplexDict.Clear();
            XData.Clear();
            SlotCount = 0;
        }
    }
}