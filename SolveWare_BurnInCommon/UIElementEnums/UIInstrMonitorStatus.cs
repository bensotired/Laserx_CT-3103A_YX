using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    public enum InstrMonitorStatus
    {
        Normal,
        Abnormal,
        Info
    }
    public class InstrMonitor
    {
        public InstrMonitor(string monitorName, object instrObj, PropertyInfo propInfo, string postFix,
            bool isValueType, double maxValue, double minValue)
        {
            this.MonitorName = monitorName;
            this.InstrObj = instrObj;
            this.PropInfo = propInfo;
            this.PostFix = postFix;
            this.IsValueType = isValueType;
            this.MaxValue = maxValue;
            this.MinValue = minValue;
        }
        public string MonitorName { get; private set; }
        public object InstrObj { get; private set; }
        public PropertyInfo PropInfo { get; private set; }
        public string PostFix { get; private set; }
        public bool IsValueType { get; private set; }
        public double MaxValue { get; private set; }
        public double MinValue { get; private set; }
        public string Info { get; private set; }
        public InstrMonitorStatus MonitorStatus { get; private set; }
        public object GetValue()
        {
            return PropInfo.GetValue(this.InstrObj);
        }
        public string GetRangeString()
        {
            var val = $"[{MinValue},{MaxValue}]"; 
            return val;
        }
        
        public void Run()
        {
            object valObj = PropInfo.GetValue(this.InstrObj);
            Info = $"[{MonitorName}:{valObj} {PostFix }]";
            if (this.IsValueType)
            {
                var val = Convert.ToDouble(valObj);
                if (JuniorMath.IsValueInLimitRange(val, MinValue, MaxValue))
                {
                    MonitorStatus = InstrMonitorStatus.Normal;
                }
                else
                {
                    MonitorStatus = InstrMonitorStatus.Abnormal;
                }
            }
            else
            {
                MonitorStatus = InstrMonitorStatus.Info;
            }
        }
    }
}