using LX_BurnInSolution.Utilities;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.OptoelectronicComponentTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public static class Merged_PXIe_4143
    {
        public static PXISourceMeter_4143 PD { get; set; }
        public static PXISourceMeter_4143 BIAS2 { get; set; }
        public static PXISourceMeter_4143 SOA1 { get; set; }
        public static PXISourceMeter_4143 SOA2 { get; set; }
        public static PXISourceMeter_4143 MIRROR2 { get; set; }
        public static PXISourceMeter_4143 LP { get; set; }
        public static PXISourceMeter_4143 MPD1 { get; set; }
        public static PXISourceMeter_4143 MIRROR1 { get; set; }
        public static PXISourceMeter_4143 BIAS1 { get; set; }
        public static PXISourceMeter_4143 MPD2 { get; set; }
        public static PXISourceMeter_4143 PH2 { get; set; }
        public static PXISourceMeter_4143 PH1 { get; set; }
        public static PXISourceMeter_4143 GAIN { get; set; }
        public static PXISourceMeter_6683H S_6683H { get; set; }
        public static double[] measuredResults_CurrentMeasurements { get; set; }
        public static double[] measuredResults_VoltageMeasurements { get; set; }

        public static void MergedSource(PXISourceMeter_4143 PD, PXISourceMeter_4143 BIAS2, PXISourceMeter_4143 SOA1, PXISourceMeter_4143 SOA2,
            PXISourceMeter_4143 MIRROR2, PXISourceMeter_4143 LP, PXISourceMeter_4143 MPD1, PXISourceMeter_4143 MIRROR1, PXISourceMeter_4143 BIAS1, PXISourceMeter_4143 MPD2,
            PXISourceMeter_4143 PH2, PXISourceMeter_4143 PH1, PXISourceMeter_4143 GAIN, PXISourceMeter_6683H S_6683H)
        {
            Merged_PXIe_4143.PD = PD;
            Merged_PXIe_4143.BIAS2 = BIAS2;
            Merged_PXIe_4143.SOA1 = SOA1;
            Merged_PXIe_4143.SOA2 = SOA2;
            Merged_PXIe_4143.MIRROR2 = MIRROR2;
            Merged_PXIe_4143.LP = LP;
            Merged_PXIe_4143.MPD1 = MPD1;
            Merged_PXIe_4143.MIRROR1 = MIRROR1;
            Merged_PXIe_4143.BIAS1 = BIAS1;
            Merged_PXIe_4143.MPD2 = MPD2;
            Merged_PXIe_4143.PH2 = PH2;
            Merged_PXIe_4143.PH1 = PH1;
            Merged_PXIe_4143.GAIN = GAIN;
            Merged_PXIe_4143.S_6683H = S_6683H;
            measuredResults_CurrentMeasurements = new double[] { };
            measuredResults_VoltageMeasurements = new double[] { };
        }
      
        public static void DisableAllSectionOutput()
        {
            try
            {
                if (PD != null &&
                    PD.IsOnline)
                {
                    PD.IsOutputOn = false;
                }
                if (BIAS2 != null &&
                    BIAS2.IsOnline)
                {
                    BIAS2.IsOutputOn = false;
                }
                if (MPD1 != null &&
                    MPD1.IsOnline)
                {
                    MPD1.IsOutputOn = false;
                }
                
                if (BIAS1 != null &&
                    BIAS1.IsOnline)
                {
                    BIAS1.IsOutputOn = false;
                }
                if (MPD2 != null &&
                    MPD2.IsOnline)
                {
                    MPD2.IsOutputOn = false;
                }

                if (SOA2 != null &&
                    SOA2.IsOnline)
                {
                    SOA2.IsOutputOn = false;
                }
                if (SOA1 != null &&
                    SOA1.IsOnline)
                {
                    SOA1.IsOutputOn = false;
                }
                if (PH2 != null &&
                    PH2.IsOnline)
                {
                    PH2.IsOutputOn = false;
                }
                if (PH1 != null &&
                   PH1.IsOnline)
                {
                    PH1.IsOutputOn = false;
                }
                if (MIRROR2 != null &&
                    MIRROR2.IsOnline)
                {
                    MIRROR2.IsOutputOn = false;
                }
                if (MIRROR1 != null &&
                    MIRROR1.IsOnline)
                {
                    MIRROR1.IsOutputOn = false;
                }
                if (LP != null &&
                    LP.IsOnline)
                {
                    LP.IsOutputOn = false;
                }
                if (GAIN != null &&
                   GAIN.IsOnline)
                {
                    GAIN.IsOutputOn = false;
                }
            }
            catch
            {

            }
        }
        public static void Reset()
        {
            try
            {
                if (PD != null &&
                    PD.IsOnline)
                {
                    PD.Reset(); 
                }
                if (BIAS2 != null &&
                    BIAS2.IsOnline)
                {
                    BIAS2.Reset();
                }
                if (MPD1 != null &&
                    MPD1.IsOnline)
                {
                    MPD1.Reset();
                }
                
                if (BIAS1 != null &&
                    BIAS1.IsOnline)
                {
                    BIAS1.Reset();
                }
                if (MPD2 != null &&
                    MPD2.IsOnline)
                {
                    MPD2.Reset();
                }
                if (SOA2 != null &&
                   SOA2.IsOnline)
                {
                    SOA2.Reset();
                }
                if (SOA1 != null &&
                  SOA1.IsOnline)
                {
                    SOA1.Reset();
                }
                if (PH2 != null &&
                    PH2.IsOnline)
                {
                    PH2.Reset();
                }
                if (PH1 != null &&
                   PH1.IsOnline)
                {
                    PH1.Reset();
                }

                if (MIRROR2 != null &&
                   MIRROR2.IsOnline)
                {
                    MIRROR2.Reset();
                }
                if (MIRROR1 != null &&
                    MIRROR1.IsOnline)
                {
                    MIRROR1.Reset();
                }
                if (LP != null &&
                   LP.IsOnline)
                {
                    LP.Reset();
                }
                if (GAIN != null &&
                   GAIN.IsOnline)
                {
                    GAIN.Reset();
                }
            }
            catch
            {

            }
        }
        static double[] m_lastLDSweepDriveCurrents = null;
        /// <summary>
        /// LIV Sweep Gain PD
        /// </summary>
        public static void Sweep_LD_PD(float startCurrent_mA,
                            float stepCurrent_mA,
                            float endCurrent_mA,
                            float complianceVoltage_V,
                            float pdBiasVoltage_V,
                            float pdComplianceCurrent_mA,
                            double K2400_NPLC)
        {
            if (!PD.IsOnline || !GAIN.IsOnline)
            {
                return;
            }

            double[] dfbCurrents_A = ArrayMath.CalculateArray(startCurrent_mA, endCurrent_mA, stepCurrent_mA);
            List<double> dfbCurrents_A_new = new List<double>();
            foreach (var item in dfbCurrents_A)
            {
                var val_mA = Math.Round(item / 1000.0, 4);
                dfbCurrents_A_new.Add(val_mA); //(单位 A)
            }
            m_lastLDSweepDriveCurrents = dfbCurrents_A_new.ToArray();
            double SourceDelay_s = 0.01;
            double ApertureTime_s =0.005;

            int StepCount = (int)(Math.Abs((endCurrent_mA - startCurrent_mA) / stepCurrent_mA)) + 1;
            GAIN.SetupMaster_Sequence_SourceCurrent_SenseVoltage(startCurrent_mA, stepCurrent_mA, endCurrent_mA, complianceVoltage_V, SourceDelay_s, ApertureTime_s, true);
            PD.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(pdBiasVoltage_V, pdComplianceCurrent_mA, StepCount, SourceDelay_s, ApertureTime_s, true);

            GAIN.TriggerOutputOn = true;
            PD.TriggerOutputOn = true;

            var slavers = new PXISourceMeter_4143[] { PD };
            Merged_PXIe_4143.ConfigureMultiChannelSynchronization(GAIN, slavers);
            Merged_PXIe_4143.Trigger(GAIN, slavers/*, StepCount, Timeout_ms / 1000.0*/);
        }
        /// <summary>
        /// LIV Sweep Gain SOA1,SOA2
        /// </summary>
        //public static void SweepSOA(float startCurrent_mA,
        //                    float stepCurrent_mA,
        //                    float endCurrent_mA,
        //                    float complianceVoltage_V,
        //                    float pdBiasVoltage_V,
        //                    float pdComplianceCurrent_mA,
        //                    double K2400_NPLC)
        //{
        //    if (!SOA1.IsOnline || !SOA2.IsOnline || !GAIN.IsOnline)
        //    {
        //        return;
        //    }
        //    double[] dfbCurrents_A = ArrayMath.CalculateArray(startCurrent_mA, endCurrent_mA, stepCurrent_mA);
        //    List<double> dfbCurrents_A_new = new List<double>();
        //    foreach (var item in dfbCurrents_A)
        //    {
        //        var val_mA = Math.Round(item / 1000.0, 4);
        //        dfbCurrents_A_new.Add(val_mA); //(单位 A)
        //    }
        //    m_lastLDSweepDriveCurrents = dfbCurrents_A_new.ToArray();

        //    float ApertureTime_s = (float)(0.02 * K2400_NPLC);

        //    int StepCount = (int)(Math.Abs((endCurrent_mA - startCurrent_mA) / stepCurrent_mA)) + 1;
        //    GAIN.SetupMaster_Sequence_SourceCurrent_SenseVoltage(startCurrent_mA, stepCurrent_mA, endCurrent_mA, complianceVoltage_V, ApertureTime_s, true);
        //    SOA1.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(pdBiasVoltage_V, pdComplianceCurrent_mA, StepCount, ApertureTime_s, false);
        //    SOA2.SetupSlaver_Sequence_SourceVoltage_SenceCurrent(pdBiasVoltage_V, pdComplianceCurrent_mA, StepCount, ApertureTime_s, false);

        //    GAIN.TriggerOutputOn = true;
        //    SOA1.TriggerOutputOn = true;
        //    SOA2.TriggerOutputOn = true;

        //    var slavers = new PXISourceMeter_4143[] { SOA1, SOA2 };
        //    Merged_PXIe_4143.ConfigureMultiChannelSynchronization(GAIN, slavers);
        //    Merged_PXIe_4143.Trigger(GAIN, slavers);
        //}

        public static int Timeout_ms { get; set; } = 50;

        //public static float[] FetchLDSweepData(SweepDataType dataType)
        //{
        //    if (dataType == SweepDataType.LD_Drive_Current_mA ||
        //        dataType == SweepDataType.LD_Drive_Voltage_V ||
        //        dataType == SweepDataType.EA_Drive_Current_mA ||
        //        dataType == SweepDataType.PD_Ch1_Current_mA ||
        //        dataType == SweepDataType.PD_Ch2_Current_mA ||
        //        dataType == SweepDataType.LD_Sense_Current_mA)
        //    {

        //    }
        //    else
        //    {
        //        //this.FormatedLog("Invalid sweep data type [{0}] for FetchLDSweepData", dataType);
        //        return null;
        //    }
        //    var dataCount = 0;
        //    if (m_lastLDSweepDriveCurrents == null ||
        //        m_lastLDSweepDriveCurrents.Length <= 0)
        //    {
        //        return new float[0];
        //    }
        //    else
        //    {
        //        dataCount = m_lastLDSweepDriveCurrents.Length;
        //    }

        //    switch (dataType)
        //    {
        //        case SweepDataType.LD_Sense_Current_mA:
        //            {
        //                if (Source4143_2.IsOnline)
        //                {
        //                    double[] dfbCurrents_A = null;
        //                    if (measuredResults_CurrentMeasurements.Length > 0)
        //                    {
        //                        dfbCurrents_A = measuredResults_CurrentMeasurements;
        //                    }
        //                    else
        //                    {
        //                        //measuredResults_CurrentMeasurements = this.SourceLd.measuredResults_CurrentMeasurements;
        //                        dfbCurrents_A = Source4143_2.Fetch_SenseCurrents(dataCount, Timeout_ms);
        //                        measuredResults_VoltageMeasurements = measuredResults_VoltageMeasurements;
        //                    }

        //                    List<float> ret = new List<float>();
        //                    foreach (var val in dfbCurrents_A)
        //                    {
        //                        ret.Add(Convert.ToSingle(val * 1000.0));
        //                    }
        //                    measuredResults_CurrentMeasurements = new double[] { };
        //                    return ret.ToArray();
        //                }
        //            }
        //            break;
        //        case SweepDataType.LD_Drive_Current_mA:
        //            {
        //                List<float> ret = new List<float>();
        //                foreach (var val in m_lastLDSweepDriveCurrents)
        //                {
        //                    ret.Add(Convert.ToSingle(val) * 1000);
        //                }
        //                return ret.ToArray();
        //            }
        //            break;
        //        case SweepDataType.LD_Drive_Voltage_V:
        //            {
        //                if (Source4143_2.IsOnline)
        //                {
        //                    double[] dfbVoltages_V = null;
        //                    if (measuredResults_VoltageMeasurements.Length > 0)
        //                    {
        //                        dfbVoltages_V = measuredResults_VoltageMeasurements;
        //                    }
        //                    else
        //                    {
        //                        dfbVoltages_V = Source4143_2.Fetch_SenseVoltages(dataCount, Timeout_ms);
        //                        measuredResults_CurrentMeasurements = Source4143_2.measuredResults_CurrentMeasurements;
        //                    }
        //                    //var dfbVoltages_V = this.SourceLd.Fetch_SenseVoltages(dataCount, this.Timeout_ms);
        //                    List<float> ret = new List<float>();
        //                    foreach (var val in dfbVoltages_V)
        //                    {
        //                        ret.Add(Convert.ToSingle(val));
        //                    }
        //                    measuredResults_VoltageMeasurements = new double[] { };
        //                    return ret.ToArray();
        //                }
        //            }
        //            break;
        //        case SweepDataType.EA_Drive_Current_mA:
        //            {
        //                if (Source4130_5.IsOnline)
        //                {

        //                    var pdCurrs = Source4130_5.Fetch_SenseCurrents(dataCount, Timeout_ms);
        //                    List<float> ret = new List<float>();
        //                    foreach (var val in pdCurrs)
        //                    {
        //                        ret.Add(Convert.ToSingle(val) * 1000.0f);
        //                    }
        //                    return ret.ToArray();
        //                }
        //            }
        //            break;
        //        case SweepDataType.PD_Ch1_Current_mA:
        //            {
        //                if (Source4143_2.IsOnline)
        //                {
        //                    var pdCurrs = Source4143_2.Fetch_SenseCurrents(dataCount, Timeout_ms);
        //                    List<float> ret = new List<float>();
        //                    foreach (var val in pdCurrs)
        //                    {
        //                        ret.Add(Convert.ToSingle(val) * 1000.0f);
        //                    }
        //                    return ret.ToArray();
        //                }
        //            }
        //            break;
        //        case SweepDataType.PD_Ch2_Current_mA:
        //            {
        //                if (Source4130_5.IsOnline)
        //                {
        //                    var mpdCurrs = Source4130_5.Fetch_SenseCurrents(dataCount, Timeout_ms);
        //                    List<float> ret = new List<float>();
        //                    foreach (var val in mpdCurrs)
        //                    {
        //                        ret.Add(Convert.ToSingle(val) * 1000.0f);
        //                    }
        //                    return ret.ToArray();
        //                }
        //            }
        //            break;
        //    }
        //    return null;
        //}

        public static float[] FetchLDSweepData(PXISourceMeter_4143 pXISource, SweepData dataType, int dataCount)
        {
            try
            {
                if (!pXISource.IsOnline || pXISource == null)
                {
                    return null;
                }
                var resut = pXISource.Fetch_MeasureVals(dataCount, 10 * 1000.0);
                switch (dataType)
                {
                    case SweepData.Sense_Current_mA:
                        {
                            double[] dfbCurrents_A = null;
                            dfbCurrents_A = resut.CurrentMeasurements;

                            List<float> ret = new List<float>();
                            foreach (var val in dfbCurrents_A)
                            {
                                ret.Add(Convert.ToSingle(val * 1000.0));
                            }
                            return ret.ToArray();

                        }
                        break;
                    case SweepData.Sense_Voltage_V:
                        {
                            double[] dfbVoltages_V = null;
                            dfbVoltages_V = resut.VoltageMeasurements;

                            List<float> ret = new List<float>();
                            foreach (var val in dfbVoltages_V)
                            {
                                ret.Add(Convert.ToSingle(val));
                            }
                            return ret.ToArray();
                        }
                        break;
                    default:
                        return null;
                        break;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static void ConfigureMultiChannelSynchronization
            (
               PXISourceMeter_4143 master,
               PXISourceMeter_4143[] slavers,
               double measureCompleteEventDelay = 0.000
           )
        {
            if (master.IsOnline == false)
            {
                return;
            }

            var masterChassis = master.CmdHandler;
            List<NIDCPower> slaverChassis = new List<NIDCPower>();
            if (slavers != null && slavers.Length > 0)
            {
                foreach (var slaver in slavers)
                {
                    if (slaver.IsOnline == false)
                    {
                        return;
                    }
                    slaverChassis.Add(slaver.CmdHandler);
                }
            }
            SmuUtility.ConfigureMultiChannelSynchronization(masterChassis, slaverChassis.ToArray(), measureCompleteEventDelay);
        }

        public static void Trigger(PXISourceMeter_4143 master,PXISourceMeter_4143[] slavers/*, int dataCount, double timeout_ms*/)
        {
            if (master.IsOnline == false)
            {
                return;
            }
            var masterChassis = master.CmdHandler;
            List<NIDCPower> slaverChassis = new List<NIDCPower>();
            if (slavers != null && slavers.Length > 0)
            {
                foreach (var slaver in slavers)
                {
                    if (slaver.IsOnline == false)
                    {
                        return;
                    }
                    slaverChassis.Add(slaver.CmdHandler);
                }
            }
            InitiateMultiChannel(masterChassis, slaverChassis.ToArray());
        }

        static void InitiateMultiChannel(NIDCPower primarySession, NIDCPower[] secondarySessions)
        {
            int secondaryCount = secondarySessions.GetLength(0);
            for (int i = 0; i < secondaryCount; i++)
            {
                secondarySessions[i].Outputs[""].Control.Initiate();
            }
            primarySession.Outputs[""].Control.Initiate();
        }
    }
}
