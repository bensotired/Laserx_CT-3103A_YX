using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public interface IScanner : IInstrumentBase
    {
        string Scanning();
    }
}
