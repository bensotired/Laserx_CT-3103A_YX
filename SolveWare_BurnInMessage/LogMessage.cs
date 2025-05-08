namespace SolveWare_BurnInMessage
{
    public class LogMessage : MessageBase, IMessage
    {
        public LogMessage(string message)
            : base(message)
        {
            this.Type = EnumMessageType.LogOnly;
        }
    }
}
