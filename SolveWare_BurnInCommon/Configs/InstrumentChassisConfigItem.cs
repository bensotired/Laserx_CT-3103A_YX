using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    public class InstrumentChassisConfigItem
    {
        public InstrumentChassisConfigItem()
        {
            InitTimeout_ms = 5 * 1000;
        }
   
        public string Resource { get; set; }
        public bool IsOnline { get; set; }
        public string Name { get; set; }
        public string ChassisType { get; set; }

        public int InitTimeout_ms { get; set; }

    }
}