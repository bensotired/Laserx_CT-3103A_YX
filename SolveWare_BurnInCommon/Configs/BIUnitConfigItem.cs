using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolveWare_BurnInCommon;

namespace SolveWare_BurnInCommon
{
  
    public sealed class BIUnitConfigItem
    {
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        public string Type { get; set; }

        public DataBook<string, string> UnitInstrumentList { get; set; }
        public DataBook<string, string> IoMap { get; set; }
        public DataBook<int, List<SectionItem>> SectionMap { get; set; }
        public DataBook<int, List<CalibrationItem>> CalibrationMap { get; set; }
        public BIUnitConfigItem()
        {
            this.UnitInstrumentList = new DataBook<string, string>();
            this.SectionMap = new DataBook<int, List<SectionItem>>();
            this.CalibrationMap = new DataBook<int, List<CalibrationItem>>();
            this.IoMap = new DataBook<string, string>();
        }
    }
}