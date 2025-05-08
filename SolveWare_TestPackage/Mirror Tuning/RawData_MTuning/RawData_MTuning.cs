using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_MTuning : RawDataCollectionBase<RawDatumItem_MTuning>
    {
        public RawData_MTuning()
        {
            Path = string.Empty;
        }
        [RawDataBrowsableElement]
        public string Path { get; set; }
    }
}