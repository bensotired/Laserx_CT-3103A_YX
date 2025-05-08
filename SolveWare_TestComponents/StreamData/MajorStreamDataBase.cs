using System;
using System.Collections.Generic;
using System.IO;

namespace SolveWare_TestComponents.Data
{
    public abstract class MajorStreamDataBase : StreamInfoBase, IMajorStreamData
    {
        string _lastSaveDataPath;
        public string LastSaveDataPath
        {
            get { return _lastSaveDataPath; }
            set { _lastSaveDataPath = Path.GetFullPath(value); }
        }
        public MajorStreamDataBase() : base()
        {
        }
        public abstract void SaveDirectly();
        public abstract int MinorStreamDataCollectionCount { get; }
        public abstract List<IMinorStreamData> GetMinorStreamDataCollection();
        public abstract IMinorStreamData GetMinorStreamData(string dataTag);
        public string GetLastSaveDataDirectory()
        {
            if (string.IsNullOrEmpty(_lastSaveDataPath))
            {
                return Directory.GetCurrentDirectory();
            }
            else
            {
                var dirInfo = Path.GetDirectoryName(_lastSaveDataPath);
                return dirInfo;
            }
        }
        public TestPluginImportProfileBase TestImportProfile { get; set; }
        public virtual void AttachedTestImportProfile(TestPluginImportProfileBase profile)
        {
            this.TestImportProfile = profile;
        }

        public override Type[] GetIncludingTypes()
        {
            List<Type> types = new List<Type>();
            foreach (var minStreamData in this.GetMinorStreamDataCollection())
            {
                if (minStreamData.GetDeviceStreamDataCollection().Count <= 0)
                {
                    continue;
                }
                else
                {
                    foreach (var deviceStreamData in minStreamData.GetDeviceStreamDataCollection())
                    {
                        if (deviceStreamData.RawDataCollecetionCount <= 0)
                        {
                            continue;
                        }
                        else
                        {
                            foreach (var rawData in deviceStreamData.RawDataCollection)
                            {
                                if (types.Contains(rawData.GetType()) == false)
                                {
                                    types.Add(rawData.GetType());
                                }
                            }
                        }
                    }
                }
            }
            return types.ToArray();
        }
    }
}