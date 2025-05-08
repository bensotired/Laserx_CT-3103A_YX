namespace SolveWare_BurnInInstruments
{
    public interface ISourceMeter_Keithley : IInstrumentBase
    {
        void Reset();
        void ZeroCorrection();
        bool IsCurrentSenseAutoRangeOn { get; set; }

        bool IsCurrentSourceAutoRangeOn { get; set; }

        bool IsVoltageSenseAutoRangeOn { get; set; }

        bool IsVoltageSourceAutoRangeOn { get; set; }

        double CurrentCompliance_A { get; set; }

        double CurrentSetpoint_A { get; set; }
        double CurrentSenseRange_A { get; set; }
        void SetupCurrentStairSweep(bool isMaster,
  double complianceVoltage_V, double measurementIntergrationPeriod,
  TriggerLine triggerInputLine, TriggerLine triggerOutputLine, int triggerCount, bool remoteSense, SelectTerminal terminal, double start, double stop, double step);
        void SetupTrigger(bool isMaster, TriggerLine triggerInputLine, TriggerLine triggerOutputLine, int triggerCount, double nplc = 1.0);
        void Trigger();
        void WaitForComplete();
        double ReadVoltage_V();
        double[] FetchRealData();
        bool IsOutputOn{get;set;}

        double ReadCurrent_A();
        SenseModeTypes SenseMode { get; set; }
        SourceModeTypes SourceMode { get; set; }
        SelectTerminal Terminal { get; set; }
        bool IsFourWireOn { get; set; }
        double VoltageCompliance_V { get; set; }

        double ReadCurrent6485_A();
    }
}