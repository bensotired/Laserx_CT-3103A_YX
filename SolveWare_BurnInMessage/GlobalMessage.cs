namespace SolveWare_BurnInMessage
{
    public class GlobalMessage : MessageBase, IMessage
    {
        public GlobalMessage(string message)
            : base(message)
        {
            this.Type = EnumMessageType.Global;
        }
    }
}
