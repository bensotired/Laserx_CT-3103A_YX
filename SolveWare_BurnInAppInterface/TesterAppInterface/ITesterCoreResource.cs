using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;

namespace SolveWare_BurnInAppInterface
{
    public interface ITesterCoreResource  
    {
        ITesterAppPluginInteration GetAppPlugin(string apKey);
        IEnumerable<ITesterAppPluginInteration> GetAppPlugins();
        //bool TryAllocateResourceToPlugin();
        bool TryAllocateResourceToPlugin(string name);
        bool TryAllocateResourceToPlatform();


    }
}