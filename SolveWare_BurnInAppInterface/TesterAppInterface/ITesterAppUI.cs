using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;

namespace SolveWare_BurnInAppInterface
{

    public interface ITesterAppUI: ITesterCoreLink
    {
        void ConnectToAppInteration(ITesterAppPluginInteration app);

        void RefreshOnce();
    }
}