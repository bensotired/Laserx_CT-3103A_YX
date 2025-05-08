using SolveWare_BurnInCommon;
using System;

namespace SolveWare_BinSorter
{

    [Serializable]
    public class BinJudgeItem : CURDItemLite
    {
        [PropEditableIndexer(true, 1)]
        public bool Visible { get; set; }
        [PropEditableIndexer(true, 2)]
        public string Unit { get; set; }
        [PropEditableIndexer(true, 3)]
        public double UL { get; set; }
        [PropEditableIndexer(true, 4)]
        public double LL { get; set; }
        [PropEditableIndexer(true, 5)]
        public bool Enable { get; set; }

        public bool Check()
        {
            if (UL < LL)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}