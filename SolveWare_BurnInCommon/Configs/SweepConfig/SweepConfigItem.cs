using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    public class SweepConfigItem
    {
        public float StartValue { get; set; }
        public float StopValue { get; set; }
        public float StepValue { get; set; }
        public int Point { get; set; }
        public bool IsEnable { get; set; }
        public DMS_SourceRange SourceRange { get; set; }
        public DMS_SenseRange SenseRange { get; set; }
    }
}