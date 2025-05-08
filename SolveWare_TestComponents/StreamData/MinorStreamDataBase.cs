using System;
using System.Collections.Generic;
using SolveWare_TestComponents.Data;

namespace SolveWare_TestComponents.Data
{
    public abstract class MinorStreamDataBase : StreamInfoBase, IMinorStreamData
    {
        public MinorStreamDataBase() : base()
        {

        }
        public abstract int DeviceStreamDataCollectionCount { get; }
        //public abstract object Load(string path);
        public abstract void Add(IDeviceStreamDataBase deviceData);
        public abstract List<IDeviceStreamDataBase> GetDeviceStreamDataCollection();
        public abstract void Clear();
        public abstract IDeviceStreamDataBase GetDeviceStreamData(string dataTag);
        public override  Type[] GetIncludingTypes()
        {
            List<Type> types = new List<Type>();
            foreach (var deviceStreamData in this.GetDeviceStreamDataCollection())
            {
                if (deviceStreamData.RawDataCollecetionCount <= 0)
                {
                    continue;
                }
                else
                {
                    foreach (var rawData in deviceStreamData.RawDataCollection)
                    {
                        if (types.Contains(rawData.GetType()))
                        {

                        }
                        else
                        {
                            types.Add(rawData.GetType());
                        }
                    }
                }
            }
            return types.ToArray();
        }
    }
}