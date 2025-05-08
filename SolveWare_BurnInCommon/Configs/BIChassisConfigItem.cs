using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolveWare_BurnInCommon;

namespace SolveWare_BurnInCommon
{
    public sealed class BIChassisConfigItem
    {
        public BIChassisConfigItem()
        {
            this.UnitHarewareChassisList = new List<string>();
            this.UnitControllerGroupList = new DataBook<string, List<string>>();
            this.ChassisInstrumentList = new DataBook<string, string>();
        }
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        public string Type { get; set; }
        public List<string>  UnitHarewareChassisList { get; set; }
        public DataBook<string, List<string>> UnitControllerGroupList { get; set; }
        public DataBook<string, string> ChassisInstrumentList { get; set; }
    }
}