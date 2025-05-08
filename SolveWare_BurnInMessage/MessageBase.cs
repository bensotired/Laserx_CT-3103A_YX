namespace SolveWare_BurnInMessage
{
    public abstract class MessageBase : IMessage
    {
        public MessageBase()
        {

        }
        public MessageBase(string message)
        {
            this.Message =  message;
        }
        public MessageBase(string message ,object context)
        {
            this.Message = message;
            this.Context = context;
        }
        public virtual string Message
        {
            get;
            set;
        }
        public virtual object Context
        {
            get;
            protected set;
        }
        public EnumMessageType Type
        {
            get;
            protected set;
        }
    }
}