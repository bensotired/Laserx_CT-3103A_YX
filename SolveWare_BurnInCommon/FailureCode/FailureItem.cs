namespace SolveWare_BurnInCommon
{
    public class FailureItem
    {
        public FailureItem()
        {
            this.Code = FailureCode.UNEXCEPT_FAIL;
            this.Discription = string.Empty;
        }
        public FailureCode Code { get; set; }
        public string  Discription { get; set; }
        public int Slot { get; set; }
        public string DataSnap { get; set; }
    }
}