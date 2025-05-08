namespace SolveWare_TestComponents.Data
{
    public interface IDeviceInfoBase
    {
        string FailureCode { get; set; }
        bool IsActive { get; set; }
        string Operator { get; set; }
        string PartNumber { get; set; }
        int Position { get; set; }
        //string SerialNumber { get; set; }
        string Station { get; set; }
    }
}