using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;

namespace SolveWare_BurnInAppInterface
{

    public interface IAxisResourceProvider
    {
        List<string> GetAxesNameCollection();
        object GetAxis_Object(string name);
    }
}