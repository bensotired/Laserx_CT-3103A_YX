using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    /// <summary>
    /// LD设置电流量程设置(1:500mA  2:50mA)
    /// </summary>

    public enum DMS_SourceRange:int
    {
        _500mA =1,
        _50mA =2,
    }
}
