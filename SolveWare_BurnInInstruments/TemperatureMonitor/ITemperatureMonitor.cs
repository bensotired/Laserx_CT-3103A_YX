using System.Collections.Generic;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public interface ITemperatureMonitor: IInstrumentBase
    {
        float[] Temperatures { get; }
   
    }
}