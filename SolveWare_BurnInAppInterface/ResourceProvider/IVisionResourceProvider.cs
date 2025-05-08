using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;

namespace SolveWare_BurnInAppInterface
{

    public interface IVisionResourceProvider 
    {
        object GetVisionController_Object(string name);
    }
} 