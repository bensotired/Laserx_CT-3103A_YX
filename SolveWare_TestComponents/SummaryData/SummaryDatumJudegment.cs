using System;

namespace SolveWare_TestComponents
{
    [Serializable]
    public enum SummaryDatumJudegment : int
    {
        FAIL = 0,
        PASS = 1,
        NO_SEPC = 2,
        TYPE_NOT_MATCH = 3
    }
}