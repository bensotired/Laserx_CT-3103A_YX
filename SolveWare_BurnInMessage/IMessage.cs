namespace SolveWare_BurnInMessage
{
    public interface IMessage
    {
        string Message { get; set; }
        EnumMessageType Type { get; }
    }
}
