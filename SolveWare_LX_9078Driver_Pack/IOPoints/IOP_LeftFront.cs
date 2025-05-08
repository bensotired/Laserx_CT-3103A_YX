using LX_BurnInSolution.Utilities;
using SolveWare_BurnInInstruments;
using System;
using System.Linq;
using System.Threading;

namespace SolveWare_IO
{
    public class IOP_LeftFront : IOC_LaserX_9078
    {
        public IOP_LeftFront(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
        {
        }

        public bool OnOff
        {
            get
            {
                return GetIO("Probe_LeftFront_Enabled").Interation.IsActive;
            }
        }
    }
}
