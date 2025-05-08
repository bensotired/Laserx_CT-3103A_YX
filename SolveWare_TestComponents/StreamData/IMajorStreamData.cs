using System.Collections.Generic;

namespace SolveWare_TestComponents.Data
{

    public interface IMajorStreamData : IStreamInfoBase
    {
       void SaveDirectly();
        int MinorStreamDataCollectionCount { get; }
        List<IMinorStreamData> GetMinorStreamDataCollection();
        IMinorStreamData  GetMinorStreamData (string dataTag);
    }
}