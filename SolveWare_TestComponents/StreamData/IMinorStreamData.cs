using System.Collections.Generic;

namespace SolveWare_TestComponents.Data
{
    public interface IMinorStreamData : IStreamInfoBase
    {
        int DeviceStreamDataCollectionCount { get; }
        List<IDeviceStreamDataBase> GetDeviceStreamDataCollection();
       IDeviceStreamDataBase  GetDeviceStreamData (string dataTag);
    }
}