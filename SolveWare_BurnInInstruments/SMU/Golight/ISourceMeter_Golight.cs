namespace SolveWare_BurnInInstruments
{
    public interface ISourceMeter_Golight : IInstrumentBase
    {
        bool CheckIsSweeping(ref int count);
        //void Connect();
        float CurrentSetpoint_A { get; set; }
        float[] FetchEASweepData(SweepDataType dataType);
        float[] FetchLDSweepData(SweepDataType dataType);
        //byte[] GetCommonReturnData(byte[] retBytes);
        GolightSource_PD_TEST_CHANNEL_RANGE GetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL pdTestChannel);
        string InstrumentIDN { get; }
        string InstrumentIP { get; set; }
        int InstrumentNetworkPort { get; set; }
        string InstrumentSerialNumber { get; }
        string InstrumentVersion { get; }
        bool IsOutputEnable { get; set; }
        bool IsSweeping { get; }
        int PivAvailableTestChannelCount { get; }
        int PivTestChannelPDCount { get; }
        float ReadCurrent_A();
        float ReadCurrent_EA_A();
        float ReadCurrent_PD_A(GolightSource_PD_TEST_CHANNEL pdTestChannel);
        float ReadVoltage_EA_V();
        float ReadVoltage_PD_V();
        double ReadVoltage_V();
        void SetPDTestChannelRange(GolightSource_PD_TEST_CHANNEL pdTestChannel, GolightSource_PD_TEST_CHANNEL_RANGE channelRange);
        void StopSweeping();
        void SweepEA
                    (
                        float ldCurrent_mA,
                        float ldComplianceVoltage_V,
                        float startVoltage_V,
                        float stepVoltage_V,
                        float stopVoltage_V,
                        float eaComplianceCurrent_mA,
                        float pdBiasVoltage_V,
                        float pdTestCh1_complianceCurrent_mA,
                        double K2400_NPLC
                    );
        void SweepLD
                    (
                        float startCurrent_mA,
                        float stepCurrent_mA,
                        float ldCurrentUpperLimit_mA,
                        float ldVoltageUpperLimit_V,
                        bool enableEAOutput,
                        float eaVoltage_V,
                        float pdTestCh1_complianceCurrent_mA,
                        float pdTestCh2_complianceCurrent_mA,
                        float pdTestCh3_complianceCurrent_mA,
                        float pdTestCh4_complianceCurrent_mA,
                        double K2400_NPLC
                    );
        void Sweep_LD_PD_Pulse(float startCurrent_mA,
                        float stepCurrent_mA,
                        float endCurrent_mA,
                        float ldVoltageUpperLimit_V,
                        float pdBiasVoltage_V,
                        float pdTestCh1_complianceCurrent_mA,
                        int Pulsewidth,//脉冲宽度
                        int Pulseperiod,//脉冲周期
                        double K2400_NPLC
                        );
        void Sweep_LD_PD
                    (
                        float startCurrent_mA,
                        float stepCurrent_mA,
                        float endCurrent_mA,
                        float ldVoltageUpperLimit_V,
                        float pdBiasVoltage_V,
                        float pdTestCh1_complianceCurrent_mA,
                        double K2400_NPLC
                    );
        void Sweep_LD_EA_PD
                    (
                        float startCurrent_mA,
                        float stepCurrent_mA,
                        float endCurrent_mA,
                        float ldVoltageUpperLimit_V,
                        float eaVoltage_V,
                        float eaComplianceCurrent_mA,
                        float pdBiasVoltage_V,
                        float pdTestCh1_complianceCurrent_mA,
                        double K2400_NPLC
                    );
        void Sweep_LD_MPD_PD
                    (
                        float startCurrent_mA,
                        float stepCurrent_mA,
                        float endCurrent_mA,
                        float ldVoltageUpperLimit_V,
                        float mpdVoltage_V,
                        float mpdComplianceCurrent_mA,
                        float pdBiasVoltage_V,
                        float pdTestCh1_complianceCurrent_mA,
                        double K2400_NPLC
                    );
        int Timeout_ms { get; set; }
        float VoltageSetpoint_EA_V { get; set; }
        float VoltageSetpoint_PD_V { get; set; }
        float VoltageSetpoint_V { get; set; }
        void SetupSMU_EA(double voltageSetpoint_V, double complianceCurrent_mA);
        void SetupSMU_PD(double voltageSetpoint_V, double complianceCurrent_mA);
        void SetupSMU_LD(double currentSetpoint_mA, double complianceVoltage_V);
        void SetupSMU_MPD(double voltageSetpoint_V, double complianceCurrent_mA);
    }
}