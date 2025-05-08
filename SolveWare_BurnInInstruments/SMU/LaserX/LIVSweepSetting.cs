using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class LIVSweepSetting
    {
        public SweepMode SweepMode { get; set; }
        public double StartCurrent_A { get; set; }
        public double StopCurrent_A { get; set; }
        public double StepCurrent_A { get; set; }
        public double Period_s { get; set; }
        public double MeasureDelay_s { get; set; }
        public double IntegratingPeriod_s { get; set; }
        public double DutyCycle { get; set; }
    }
    public enum SweepMode
    {
        CW,
        Pulse,
    }
    public enum ReadingSection_6212
    {
        RawLD_Current,
        RawLD_Voltage,
        RawPD_Current_Rng80uA,
        RawMPD_Current_Rng300uA,
        LD_Current,
        LD_Voltage,
        PD_Current_Rng80uA,
        MPD_Current_Rng300uA,
    }
}
