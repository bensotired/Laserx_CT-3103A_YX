
using SolveWare_BurnInCommon;

namespace SolveWare_BurnInMessage
{
    public class InternalMessage : MessageBase, IMessage
    {
        public InternalOperationType OperationType { get; private set; }

        public InternalMessage(string message, InternalOperationType operaType, object context)
            : base(message, context)
        {
            this.Type = EnumMessageType.Internal;
            this.OperationType = operaType;
        }
    }
}