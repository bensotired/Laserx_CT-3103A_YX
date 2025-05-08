using SolveWare_TestComponents.Attributes;
using System;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_AlignmentDemo : RawDataCollectionBase<RawDatumItem_AlignmentDemo>
    {
        public RawData_AlignmentDemo()
        {
            
        }
        [RawDataBrowsableElement]
        public int IO_Channel { get; set; }

    }
}