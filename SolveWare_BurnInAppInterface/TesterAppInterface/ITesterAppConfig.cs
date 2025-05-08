using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;

namespace SolveWare_BurnInAppInterface
{

    public interface ITesterAppConfig 
    {
        void Save(string configFile);
        void Load(string configFile);
        void CreateDefaultInstance();
    }
}