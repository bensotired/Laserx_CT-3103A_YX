using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments 
{
    public class LineCollection
    {
        public List<Line> Lines { get; set; }
        public string Name { get; set; }
        public LineCollection()
        {
            Lines = new List<Line>();
        }
    }
}
