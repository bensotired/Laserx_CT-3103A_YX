using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SolveWare_BurnInInstruments
{

    public class GolightSourceMeter_PIV450_001 : GolightSourceMeterBase, ISourceMeter_Golight
    {

        public GolightSourceMeter_PIV450_001(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {

        }
 
    }
}


