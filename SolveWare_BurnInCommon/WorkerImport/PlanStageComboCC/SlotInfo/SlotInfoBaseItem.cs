using MessagePack;
using System;

namespace SolveWare_BurnInCommon
{
    [Serializable]
    [MessagePackObject(keyAsPropertyName: true)]
    public class SlotInfoBaseItem
    {
        [PropIndex(1)]
      
        public int Slot { get; set; }
        [PropIndex(2)]
       
        public bool IsValid { get; set; }
        [PropIndex(3)]
       
        public string SerialNumber { get; set; }
        /// <summary>
        /// 装载产品的底座
        /// </summary>
        [PropIndex(4)]
 
        public string CarrierNumber { get; set; }
        /// <summary>
        /// 装载产品的底座的底座
        /// </summary>
        [PropIndex(5)]
 
        public string FixtureNumber { get; set; }

        public SlotInfoBaseItem()
        {
        }
    }
}