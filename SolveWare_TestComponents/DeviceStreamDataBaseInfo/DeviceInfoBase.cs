using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class DeviceInfoBase : IDeviceInfoBase
    {
        public DeviceInfoBase()
        {
            Station = string.Empty;
            Operator = string.Empty;
            PartNumber = string.Empty;
            //SerialNumber = string.Empty;
            Position = -1;
            IsActive = false;
            FailureCode = string.Empty;
        
        }
      
        public string Station { get; set; }
        public string Operator { get; set; }
        public string PartNumber { get; set; }
        //public    string SerialNumber { get; set; }    
        public int Position { get; set; }
        public bool IsActive { get; set; }
        public string FailureCode { get; set; }
    }
}