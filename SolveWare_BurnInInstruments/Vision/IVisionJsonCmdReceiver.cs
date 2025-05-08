namespace SolveWare_BurnInInstruments
{
    public interface IVisionJsonCmdReceiver
    {
        bool Success { get; set; }

        TResult GetResult<TResult>();
    }
}