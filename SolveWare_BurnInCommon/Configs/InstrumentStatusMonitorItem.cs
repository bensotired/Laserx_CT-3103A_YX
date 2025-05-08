using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{

    public class InstrumentStatusMonitorItem
    {
        public InstrumentStatusMonitorItem()
        {

        }
        public string InstrumentName { get; set; }
        public string InstrumentPropertyName { get; set; }
        public string StatusMonitorName { get; set; }
        public string StatusInfoPostFix { get; set; }
        public bool IsValueType { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
    }
}