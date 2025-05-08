using SolveWare_BurnInAppInterface;
using SolveWare_IO;
using SolveWare_Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_SlaveStation
{
    public class SlaveStation_Config
    {
        public SlaveStation_Config()
        {
            this.Axes = new List<string>();
            this.Positions = new List<string>();
            this.AxisDirections = new List<string>();
            this.IOs = new List<string>();
        }
        public List<string> Axes { get; set; }
        public List<string> Positions { get; set; }
        public List<string> AxisDirections { get; set; }
        public List<string> IOs { get; set; }
    }

}
