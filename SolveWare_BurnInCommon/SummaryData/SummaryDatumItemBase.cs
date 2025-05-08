using System;

namespace SolveWare_BurnInCommon
{
    [Serializable]
    public class SummaryDatumItemBase : CURDItemLite, ISummaryDatumItemBase
    {
        [PropEditableIndexer(true, 1)]
        public object Value { get; set; }
        [PropEditableIndexer(true, 2)]
        public SummaryDatumJudegment Judegment { get; set; }
        [PropEditableIndexer(true, 3)]
        public string Unit { get; set; }
        [PropEditableIndexer(true, 4)]
        public string Min { get; set; }
        [PropEditableIndexer(true, 5)]
        public string Max { get; set; }
        [PropEditableIndexer(true, 4)]
        public SpecDataType DataType { get; set; }
        [PropEditableIndexer(true, 5)]
        public string FailureCode { get; set; }
    }
}