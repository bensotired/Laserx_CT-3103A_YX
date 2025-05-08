using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{

    public class InstrumentRefreshGroupConfigItem
    {
        public InstrumentRefreshGroupConfigItem()
        {
            this.RefreshList = new List<string>();
        }
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        public bool IsSimulation { get; set; }
        public string InstrumentType { get; set; }
        public List<string> RefreshList { get; set; }
    }
}