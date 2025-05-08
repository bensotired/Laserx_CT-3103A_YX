using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;

namespace SolveWare_BurnInAppInterface
{

    public interface ITesterCoreLink
    {
        void ConnectToCore(ITesterCoreInteration core);
        void DisconnectFromCore(ITesterCoreInteration core);

        //AccessPermissionLevel APL { get; }
    }
}