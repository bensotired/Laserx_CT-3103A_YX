using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Data;

namespace SolveWare_BurnInCommon
{
    /// <summary>
    /// 工作者关键信息呈现类
    /// </summary>
    public class ChartsRawData
    {
        public Dictionary<CEClass, Dictionary<CEAxisXY, Dictionary<string, Dictionary<int, List<object>>>>> YDataComplexDict =
           new Dictionary<CEClass, Dictionary<CEAxisXY, Dictionary<string, Dictionary<int, List<object>>>>>();
        public List<string> XData = new List<string>();
        public int SlotCount { get; set; }
        public void Clear()
        {
            YDataComplexDict.Clear();
            XData.Clear();
            SlotCount = 0;
        }
    }
    
}