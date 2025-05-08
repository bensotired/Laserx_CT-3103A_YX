using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInAppInterface
{
    public interface IAppPluginInteration : ICoreLink, IAccessPermissionLevel
    {
        void StartUp();
        void ShowGui();
        void Dev();
        void SetConfigItem(AppPluginConfigItem configItem);
    }
}