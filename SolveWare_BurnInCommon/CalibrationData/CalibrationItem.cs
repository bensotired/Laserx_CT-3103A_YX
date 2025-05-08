using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    public class CalibrationItem
    {
        /// <summary>
        /// gai
        /// </summary>
        public string Name { get; set; } 
        public double K { get; set; }
        public double B { get; set; }
        public CalibrationItem()
        {
            this.K = 1;
            this.B = 0;
        }
        public CalibrationItem(CalibrationItem sourceItem)
        {
            this.Name = sourceItem.Name;
            this.K = sourceItem.K;
            this.B = sourceItem.B;
        }
        public CalibrationItem(string name, double kVal, double bVal)
        {
            this.Name = name;
            this.K = kVal;
            this.B = bVal;
        }
    }
}