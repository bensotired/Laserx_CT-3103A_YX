using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SolveWare_IO
{
    //[Serializable]
    //[StructLayout(LayoutKind.Sequential)]
    public class IORuntimeInteration
    {
        public IORuntimeInteration()
        {

        }
     
        public bool IsActive
        {
            get;
             set;
        }
    
        public string IOTag
        {
            get;
            internal set;
        }
  
        public IOType IOType
        {
            get;
            internal set;
        }
        public bool IsSimulation { get; internal set; }
    }
}