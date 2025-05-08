using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Data;
using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;

namespace TestPlugin_Demo
{
    [Serializable]
    public class MinorStreamData_CT3103 : MinorStreamDataBase
    {
        public MinorStreamData_CT3103()
        {
            this.DeviceStreamDataCollection = new List<DeviceStreamData_CT3103>();
        }
        public List<DeviceStreamData_CT3103> DeviceStreamDataCollection { get; set; }

        public override int DeviceStreamDataCollectionCount
        {
            get
            {
                return this.DeviceStreamDataCollection.Count;
            }
        }

        public override void Add(IDeviceStreamDataBase deviceData)
        {
            this.DeviceStreamDataCollection.Add((DeviceStreamData_CT3103)deviceData);
        }
        public override void Clear()
        {
            this.DeviceStreamDataCollection.Clear();
        }

        public override IDeviceStreamDataBase GetDeviceStreamData(string dataTag)
        {
            return this.DeviceStreamDataCollection.Find(item => item.Information == dataTag);
        }

        public override List<IDeviceStreamDataBase> GetDeviceStreamDataCollection()
        {
            return this.DeviceStreamDataCollection.ConvertAll(item => item as IDeviceStreamDataBase);
        }
 
    }
}