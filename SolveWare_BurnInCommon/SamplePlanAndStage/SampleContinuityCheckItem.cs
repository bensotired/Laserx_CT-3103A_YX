using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    public class SampleContinuityCheckItem: ContinuityCheckItem, IContinuityCheckItem
    {
        public double CustomizeParams_1 { get; set; }
        public bool CustomizeParams_2 { get; set; }
        public string CustomizeParams_3 { get; set; }
    }
}