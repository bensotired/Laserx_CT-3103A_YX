using MessagePack;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SolveWare_BurnInCommon
{
    [Serializable]
    [MessagePackObject(keyAsPropertyName: true)]
    public class UnitSlotsInfo
    {
         public string Memo { get; set; }
         public string UnitName { get; set; }
         public List<SlotInfoBaseItem> SlotInfos { get; set; }
        public UnitSlotsInfo()
        {
            this.SlotInfos = new List<SlotInfoBaseItem>();
        }
        public override string ToString()
        {
            var props = new List<PropertyInfo>(typeof(SlotInfoBaseItem).GetProperties());
            Dictionary<int, PropertyInfo> valueProps = new Dictionary<int, PropertyInfo>();
            props.ForEach
            (
                p =>
                {
                    int pIndex = -1;
                    if (PropHelper.IsPropertyCanIndex(p, ref pIndex))
                    {
                        valueProps.Add(pIndex, p);
                    }
                }
            );
            StringBuilder header = new StringBuilder();
            StringBuilder strb = new StringBuilder();
            for (int slot = 1; slot <= this.Count; slot++)
            {
                //从2开始打印  1为固定的slotnumber 用于组成表头
                for (int propIndex = 2; propIndex <= props.Count; propIndex++)
                {
                    var prop = valueProps[propIndex];
                    strb.Append($"S{slot}_{prop.Name},");
                }
            }
            header.AppendLine(strb.ToString());
            strb.Clear();
            for (int slot = 1; slot <= this.Count; slot++)
            {
                var sInfo = this[slot];
                //从2开始打印  1为固定的slotnumber 用于组成表头
                for (int propIndex = 2; propIndex <= valueProps.Count; propIndex++)
                {
                    strb.Append($"{valueProps[propIndex].GetValue(sInfo)},");
                }
            }
            header.AppendLine(strb.ToString());
            return header.ToString();
        }



        public void Initialize(int slotCount)
        {
            this.SlotInfos.Clear();
            for (int slot = 1; slot <= slotCount; slot++)
            {
                this.SlotInfos.Add(new SlotInfoBaseItem()
                {
                    Slot = slot,
                    SerialNumber = $"SN_{slot}",
                });
            }
        }
        public bool ExistValidSlot 
        {
            get
            {
                if(SlotInfos.Count == 0)
                {
                    return false;
                }
                return SlotInfos.Exists(slot => slot.IsValid == true);
            }
        }
        public SlotInfoBaseItem this[int slot]
        {
            get
            {
                try
                {
                    if (this.SlotInfos.Exists(item => item.Slot == slot))
                    {
                        return this.SlotInfos.Find(item => item.Slot == slot);
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Add(SlotInfoBaseItem item)
        {
            this.SlotInfos.Add(item);
        }
        [IgnoreMember]
        public int Count
        {
            get
            {
                return this.SlotInfos.Count;
            }
        }
        public void SetSlotValidStatus(int slot, bool isValid)
        {
            if (this.SlotInfos.Exists(item => item.Slot == slot))
            {
                this.SlotInfos.Find(item => item.Slot == slot).IsValid = isValid;
            }
        }
        public void SetSlotValidStatus(int slot, string carrierNumber, bool isValid)
        {
            if (this.SlotInfos.Exists(item => item.Slot == slot &&
                                              item.CarrierNumber == carrierNumber))
            {
                this.SlotInfos.Find(item => item.Slot == slot &&
                                            item.CarrierNumber == carrierNumber).IsValid = isValid;
            }
        }
        public void SetSlotValidStatus(int slot, string carrierNumber, string fixtureNumber, bool isValid)
        {
            if (this.SlotInfos.Exists(item => item.Slot == slot &&
                                              item.CarrierNumber == carrierNumber &&
                                              item.FixtureNumber == fixtureNumber))
            {
                this.SlotInfos.Find(item => item.Slot == slot &&
                                            item.CarrierNumber == carrierNumber &&
                                            item.FixtureNumber == fixtureNumber).IsValid = isValid;
            }
        }

        public void Clear()
        {
            this.SlotInfos.Clear();
        }
        //public UnitSlotsInfo Clone()
        //{
        //    return (UnitSlotsInfo)this.MemberwiseClone();
        //}
        //public List<SlotInfoBaseItem> GetEnumerator()
        //{
        //    return this.SlotInfos.GetEnumerator();
        //}
        public IEnumerator<SlotInfoBaseItem> GetEnumerator()
        {
            return this.SlotInfos.GetEnumerator();
        }
    }
}