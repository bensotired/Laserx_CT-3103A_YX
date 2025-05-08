using SolveWare_TestComponents.Data;
using System;

namespace TestPlugin_Demo
{
    [Serializable]
    public class DeviceInfo_CT3103 : DeviceInfoBase
    {
        public DeviceInfo_CT3103() : base()
        {
            OperatorID = string.Empty;
            WorkOrder = string.Empty;
            //SubOrder = string.Empty;
            PartNumber = string.Empty;
            CarrierID = string.Empty;
            ChipID = string.Empty;
            Station = string.Empty;
            Purpose = string.Empty;
            Result = string.Empty;
            EndTime = string.Empty;
        }
        public string OperatorID { get; set; }
        public string WorkOrder { get; set; }//工单号
        //public string SubOrder { get; set; }//子工单
        public new string PartNumber { get; set; }//物料号
        public string CarrierID { get; set; }//夹具号
        public string ChipID { get; set; }
        public new string Station { get; set; }
        public string Purpose { get; set; }//老化状态
        public string Result { get; set; }//测试结果
        public string EndTime { get; set; }//结束时间

    }
}