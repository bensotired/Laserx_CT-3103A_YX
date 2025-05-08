using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public enum InstrumentChassisArgsType
    { 
        TurnOffline,
        TurnOnline,
        TurnOnSimnulation,
        AllocateChassisResouce,
        EnableSimulation,
        DisableSimulation,
    }
    public class InstrumentChassisArgs : EventArgs
    {
        public InstrumentChassisArgsType EventType { get; protected set; }
        public InstrumentChassisArgs(InstrumentChassisArgsType eventType) : base()
        {
            this.EventType = eventType;
        }
    }

    public delegate object InstrumentChassisEventHandler(object sender, InstrumentChassisArgs e);
 
}