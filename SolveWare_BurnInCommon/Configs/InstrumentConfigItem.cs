using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{

    public class InstrumentConfigItem
    {
        public InstrumentConfigItem()
        {

        }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsOnline { get; set; }
        //public bool IsSimulation { get; set; }
        public string ChassisName { get; set; }
        public string InstrumentType { get; set; }
        //public string Group { get; set; }
    }
}