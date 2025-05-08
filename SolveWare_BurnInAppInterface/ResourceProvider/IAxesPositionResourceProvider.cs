using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;

namespace SolveWare_BurnInAppInterface
{

    public interface IAxesPositionResourceProvider
    {
        void UpdatePositionConfig(object positionName, ref string errMsg);
        string SavePositionConfig();
        object GetAxesPosition_Object(string name);
        object AxesPositionCollection { get; }
        List<string> GetPositionNameCollection();
    }
}