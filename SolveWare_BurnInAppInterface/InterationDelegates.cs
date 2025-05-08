using SolveWare_BurnInMessage;
using System;

namespace SolveWare_BurnInAppInterface
{
    public delegate void SendMessageOutEventHandler(IMessage message);
    public delegate object SendMessageOutEventHandlerWithReturn(IMessage message);
}