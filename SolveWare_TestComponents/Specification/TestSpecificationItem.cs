using SolveWare_BurnInCommon;
using LX_BurnInSolution.Utilities;

namespace SolveWare_TestComponents.Specification
{

    public class TestSpecificationItem : CURDItemLite, ICURDItemLite
    {
        public TestSpecificationItem() : base()
        {
            Max = "99";
            Min = "0";
            //IsCirtcal = true;
            FailureCode = "FC_***";
            DataType = SpecDataType.Double;
            Unit = "NA";
        }
        [PropEditableIndexer(true, 1)]
        public string Unit { get; set; }
        [PropEditableIndexer(true, 2)]
        public string Min { get; set; }
        [PropEditableIndexer(true, 3)]
        public string Max { get; set; }
        [PropEditableIndexer(true, 4)]
        public SpecDataType DataType { get; set; }
        [PropEditableIndexer(true, 5)]
        public string FailureCode { get; set; }
        public bool Check()
        {
            bool isOk = true;


            switch (DataType)
            {
                case SpecDataType.Double:
                    {
                        double dblKey = 0.0;
                        var maxOk = double.TryParse(Max, out dblKey);
                        var minOk = double.TryParse(Min, out dblKey);
                        isOk = maxOk && minOk;
                    }
                    break;
                case SpecDataType.Integer:
                    {
                        int intKey = 0;
                        var maxOk = int.TryParse(Max, out intKey);
                        var minOk = int.TryParse(Min, out intKey);
                        isOk = maxOk && minOk;
                    }
                    break;
            }
            return isOk;
        }
    }
}