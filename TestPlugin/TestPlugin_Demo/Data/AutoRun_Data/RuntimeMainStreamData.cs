using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TestPlugin_Demo
{
    [XmlType("MajorStreamData")]
    [XmlInclude(typeof(TestPluginImportProfile_CT3103))]
    public class MajorStreamData_CT3103 : MajorStreamDataBase, IMajorStreamData// : ICollection<DeviceStreamData_CT3103>
    {
        //public string FileName { set; get; } = string.Empty;
        //public string TesterNumber { set; get; } = string.Empty;
        //public string DeviceNumber { set; get; } = string.Empty;
        //public string Specification { set; get; } = string.Empty;
        //public int TotalTested { set; get; } = 0;
        //public int Samples { set; get; } = 0;
        //public string CustomerNote1 { get; set; } = string.Empty;
        //public string CustomerRemark { get; set; } = string.Empty;
        //public string LotNumber { get; set; } = string.Empty;
        //public string Operator { get; set; } = string.Empty;
        //public string TestTime { get; set; } = string.Empty;
        //public string EndTime { get; set; } = string.Empty;

        public List<string> BinSummaryDataNames { get; set; } = new List<string>();
        public MajorStreamData_CT3103()
        {
            this.Information = $"MajorStreamData_{BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)}";
           
            this.OutputQueue_1 = new Queue<DeviceStreamData_CT3103>();
            this.OutputQueue_2 = new Queue<DeviceStreamData_CT3103>();
            this.MinorStreamDataCollection = new List<MinorStreamData_CT3103>();
            //this.SequenceFailedQueue = new Queue<DeviceStreamData_CT3103>();

        }
        [XmlIgnore]
        Queue<DeviceStreamData_CT3103> OutputQueue_1 { get; set; }
        [XmlIgnore]
        Queue<DeviceStreamData_CT3103> OutputQueue_2 { get; set; }


        public List<MinorStreamData_CT3103> MinorStreamDataCollection { get; set; }
       
        public override int MinorStreamDataCollectionCount
        {
            get
            {
                return this.MinorStreamDataCollection.Count;
            }
        }
        public override List<IMinorStreamData> GetMinorStreamDataCollection()
        {
            return this.MinorStreamDataCollection.ConvertAll(item => item as IMinorStreamData);
        }
        public override IMinorStreamData GetMinorStreamData(string dataTag)
        {
            return this.MinorStreamDataCollection.Find(item => item.Information.Equals(dataTag));
        }
        public override void AttachedTestImportProfile(TestPluginImportProfileBase profile)
        {
            base.AttachedTestImportProfile(profile);
        }

 
        public void AddToDataCollection(DeviceStreamData_CT3103 deviceData)
        {
            if (this.MinorStreamDataCollection.Count <= 0)
            {
                MinorStreamData_CT3103 minorStreamData = new MinorStreamData_CT3103();
                minorStreamData.Information = $"WF001";
                this.MinorStreamDataCollection.Add(minorStreamData);
            }
            this.MinorStreamDataCollection.First().Add(deviceData);
        }
        public void Clear()
        {
            //this.InputQueue.Clear();
            //this.TestZoneQueue_1st.Clear();
            //this.TestZoneQueue_2nd.Clear();
            this.OutputQueue_1.Clear();

            this.MinorStreamDataCollection.Clear();
            //this.SequenceFailedQueue.Clear();
        }
      
        public DeviceStreamData_CT3103 CurrentOne_outputZone()
        {
            return this.OutputQueue_1.Peek();
        }





        public DeviceStreamData_CT3103 GetCurrentOne_Station_1()
        {
            return this.OutputQueue_1.Dequeue();
        }

        public void Station_1_Move_To_OutUP(DeviceStreamData_CT3103 data_CT3103)
        {
            this.OutputQueue_1.Enqueue(data_CT3103);
        }
        public void Station_2_Move_To_OutUP(DeviceStreamData_CT3103 data_CT3103)
        {
            this.OutputQueue_2.Enqueue(data_CT3103); 
        }
        public DeviceStreamData_CT3103 GetCurrentOne_Station_2()
        {
            return this.OutputQueue_2.Dequeue();
        }

        public override void SaveDirectly()
        {
            this.Save(this.LastSaveDataPath);
        }
    }
}