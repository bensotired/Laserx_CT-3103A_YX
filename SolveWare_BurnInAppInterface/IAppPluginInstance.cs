using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInAppInterface
{
    public interface IAppPluginInstance : IAppPluginInteration
    {
        object GetGui();
    }
}