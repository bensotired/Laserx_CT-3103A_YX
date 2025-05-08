using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public interface IOSA : IInstrumentBase
    {
        bool CalibrationAutoZeroEnable { get; set; }
        double CenterWavelength_nm { get; set; }
        string InstrumentIDN { get; }
        bool IsPowerRangeAuto { get; set; }
        bool IsResolutionBandwidthCorrectionOn { get; set; }
        bool IsSweepContinuous { get; set; }
        bool IsSweepTimeAuto { get; set; }
        bool IsTraceLengthAuto { get; set; }
        double MaskThresholdLevel_dB { get; set; }
        double ModeDifference_dB { get; set; }
        YokogawaAQ6370NoiseAlgorithms NoiseAlgorithm { get; set; }
        double NoiseBandwidth_nm { get; set; }
        YokogawaAQ6370NoiseFittingAlgorithms NoiseFittingAlgorithms { get; set; }
        double NoiseFittingArea_nm { get; set; }
        double NoiseFittingMaskArea_nm { get; set; }
        double PeakExcursion_dB { get; set; }
        PowerUnit PowerUnit { get; set; }
        double ReferenceLevel_dBm { get; set; }
        double ResolutionBandwidth_nm { get; set; }
        YokogawaAQ6370SensitivityModes Sensitivity { get; set; }
        double Sensitivity_dBm { get; set; }
        double SmsrModeDiff { get; set; }
        double StartWavelength_nm { get; set; }
        double StopWavelength_nm { get; set; }
        YokogawaAQ6370SweepModes SweepMode { get; set; }
        int TraceLength { get; set; }
        string TraceLength_string { get; set; } 
        double WavelengthSpan_nm { get; set; }

        void CheckIDN();
        //void GenerateFakeDataOnceCycle(CancellationToken token);
        string getAmplitudeTraceData(OsaTrace trace);
        PowerWavelengthTrace GetOpticalSpectrumTrace(bool triggerNewSweep, OsaTrace traceType);
        double GetOsnr_dB();
        string GetRawSmsr_dB();
        OsaSmsrResults GetSmsrResults();
        double GetSmsr_dB();
        string getWavelengthTraceData(OsaTrace trace);
        double ReadPowerAtPeak_dbm();
        double GetPeakWavelength_nm(); 
        double ReadSpectrumWidth_nm(float db);
        double ReadWavelengthAtPeak_nm();
        //void RefreshDataOnceCycle(CancellationToken token);
        void Reset();
        void TriggerSweep();

        double SMSRMask_nm { get; set; }
        double ReadCenterWL_nm();
    }
}