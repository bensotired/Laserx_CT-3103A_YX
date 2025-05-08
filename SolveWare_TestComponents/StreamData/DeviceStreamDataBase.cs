
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class DeviceStreamDataBase<TDeviceInfo> : StreamInfoBase,
        IDeviceStreamDataBase
        where TDeviceInfo : class, IDeviceInfoBase
    {
        [DisplayName("SerialNumber")]
        public string SerialNumber { get; set; } = string.Empty;
        public IDeviceInfoBase DeviceInfoBase
        {
            get
            {
                return DeviceInfo;
            }
        }
        public TDeviceInfo DeviceInfo { get; set; }
        [RawDataCollection]
        public List<RawDataBaseLite> RawDataCollection { get; set; }
        public SummaryDataCollection SummaryDataCollection { get; set; }
        public DeviceStreamDataBase()
        {
            this.DeviceInfo = default(TDeviceInfo);
            this.RawDataCollection = new List<RawDataBaseLite>();
            this.SummaryDataCollection = new SummaryDataCollection();
        }
        public virtual string GetSQL_Header() { throw new NotImplementedException(); }
        public virtual string GetSQL_HeaderValues() { throw new NotImplementedException(); }
        public void AddRawData(IRawDataBaseLite datum)
        {
            this.RawDataCollection.Add((RawDataBaseLite)datum);
        }
        public void AddSummaryDataCollection(List<SummaryDatumItemBase> summaryDataCollection)
        {
            foreach (var item in summaryDataCollection)
            {
                this.SummaryDataCollection.AddSingleItem(item);
            }
        }
        public void AddSingleSummaryData(SummaryDatumItemBase summaryData)
        {
            this.SummaryDataCollection.AddSingleItem(summaryData);
        }
        public override Type[] GetIncludingTypes()
        {
            List<Type> types = new List<Type>();
            foreach (var item in RawDataCollection)
            {
                var itemType = item.GetType();
                if (types.Contains(itemType) == false)
                {
                    types.Add(itemType);
                }
            }
            return types.ToArray();
        }
        public void Clear()
        {
            this.RawDataCollection.Clear();
            this.SummaryDataCollection.Clear();
        }
        public int RawDataCollecetionCount
        {
            get
            {
                return this.RawDataCollection.Count;
            }
        }

        public string MaskName { get; set; }
        public string WaferName { get; set; }
        public string ChipName { get; set; }
        public string OeskID { get; set; }
        public double Tec1ActualTemp { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public string CoarseTuningPath { get; set; }

        public IEnumerator<RawDataBaseLite> GetEnumerator()
        {
            return this.RawDataCollection.GetEnumerator();
        }


    }
}