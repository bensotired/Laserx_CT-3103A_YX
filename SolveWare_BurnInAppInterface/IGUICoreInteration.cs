using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;

namespace SolveWare_BurnInAppInterface
{
    public interface IGUICoreInteration : ICoreInteration 
    {
        Action<IMessage> GUISendToCore { get; set; }
        event SendMessageOutEventHandler SendOutFormCoreToGUIEvent;

    }
}