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
    public class DeviceBaseInfo
    {
        public DeviceBaseInfo()
        {
            Batch = string.Empty;
            FixtureID = string.Empty;
            Station = string.Empty;
            Operator = string.Empty;
            PartNumber = string.Empty;
            SerialNumber = string.Empty;
            CarrierID = string.Empty;
            WorkOrder = string.Empty;
            FailureCode = string.Empty;
            Position = -1;
            IsActive = false;
        }
        [PropIndex(1)]
        public string Batch { get; set; }

        [PropIndex(2)]
        public string FixtureID { get; set; }

 

        [PropIndex(4)]
        public string Station { get; set; }

        [PropIndex(5)]
        public string Operator { get; set; }

        [PropIndex(6)]
        public string PartNumber { get; set; }

        //[PropIndex(7)]
        //public string Wafer { get; set; }

        [PropIndex(8)]
        public string SerialNumber { get; set; }     //SN

        [PropIndex(9)]
        public string CarrierID { get; set; }// 夹具

        [PropIndex(10)]
        public string WorkOrder { get; set; }// 任务令

        [PropIndex(11)]
        public string FailureCode { get; set; }

        [PropIndex(12)]
        public int Position { get; set; }

        [PropIndex(13)]
        public bool IsActive { get; set; }

 
    }
}