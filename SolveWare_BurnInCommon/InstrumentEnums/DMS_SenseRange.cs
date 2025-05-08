using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    /// <summary>
    /// PD回读电流量程设置(1:5mA  2:500uA)
    /// </summary>
    public enum DMS_SenseRange : int
    {
        _5mA =1,
        _500uA =2,
    }
}
