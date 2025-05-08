using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;

namespace SolveWare_BurnInAppInterface
{

    public interface ICoreLink
    {
        void ConnectToCore(ICoreInteration core);
        void DisconnectFromCore(ICoreInteration core);

        //AccessPermissionLevel APL { get; }
    }
}