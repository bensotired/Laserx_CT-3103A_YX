using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SolveWare_Analog
{
    //[Serializable]
    //[StructLayout(LayoutKind.Sequential)]
    public class AnalogRuntimeInteration
    {
        public AnalogRuntimeInteration()
        {
        }

        public double Value
        {
            get;
            internal set;
        }

        public string IOTag
        {
            get;
            internal set;
        }

        public AnalogType AnalogType
        {
            get;
            internal set;
        }

        public bool IsSimulation { get; internal set; }
    }
}