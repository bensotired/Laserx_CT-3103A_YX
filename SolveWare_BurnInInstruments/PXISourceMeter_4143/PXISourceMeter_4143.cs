using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LX_BurnInSolution.Utilities;
using NationalInstruments;
using NationalInstruments.ModularInstruments.NIDCPower;
using NationalInstruments.OptoelectronicComponentTest;

namespace SolveWare_BurnInInstruments
{
    public class PXISourceMeter_4143 : InstrumentBase
    {
        public PXISourceMeter_4143(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {

            measuredResults_CurrentMeasurements = new double[] { };
            measuredResults_VoltageMeasurements = new double[] { };

        }
        //string ResourceName;
        public string SLOT_TAG
        {
            get; set;
        }
        public void AssignmentMode_Current(double curr_mA, double complaince_voltage_V)
        {
            if (this.IsOnline == false) { return; }
            var current = curr_mA * 0.95;
            this.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(current, complaince_voltage_V);
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(50);
                current += curr_mA * 0.01;
 


                 this.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(current, complaince_voltage_V);
            }
            Thread.Sleep(100);
            //补充输出一次
            this.SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(curr_mA, complaince_voltage_V);

        }
        public void AssignmentMode_Current_FastMode(double curr_mA , bool isSetSourceRange)
        {
            if (this.IsOnline == false) { return; }


            if (curr_mA == 0)
            {
            }
            else
            {
                if (isSetSourceRange == true)
                {
                    this.CmdHandler.Outputs[SLOT_TAG].Source.Current.CurrentLevelRange = Math.Abs(curr_mA / 1000.0);
                }
            }
            this.CmdHandler.Outputs[SLOT_TAG].Source.Current.CurrentLevel = curr_mA / 1000.0;
           // Thread.Sleep(1);
        }
        public void AssignmentMode_Voltage(double voltage_V, double complaince_curr_mA)
        {
            if (this.IsOnline == false) { return; }
            this.SetupAndEnableSourceOutput_SinglePoint_Voltage_V_For_Test(voltage_V, complaince_curr_mA);

        }


        //static string channel_number_of_slot = "";    //PXI1Slot3/0
        public double[] measuredResults_CurrentMeasurements { get; set; }
        public double[] measuredResults_VoltageMeasurements { get; set; }

        public NIDCPower CmdHandler;

        public override void Initialize()
        {
            if (this.IsOnline)
            {
                var slot_resourceName = this._chassis.Resource;
                SLOT_TAG = Address;
                var pxi_resource = new ResourceChannelSettings(slot_resourceName, SLOT_TAG);
                CmdHandler = SmuUtility.CreateSession(pxi_resource);
            }

        }
        public string GetDefaultTermialName()
        {
            string termial = string.Empty;
            if (this.IsOnline)
            {
                termial = this.CmdHandler.Events.SourceCompleteEvent.OutputTerminal;
            }
            return termial;
        }
        public void SetDefaultTermialName(string name)
        {
            if (this.IsOnline)
            {
                this.CmdHandler.Events.SourceCompleteEvent.OutputTerminal = name;
            }
        }
        public void ClearTriggerTermialName()
        {
            if (this.IsOnline)
            {
                this.CmdHandler.Events.SourceCompleteEvent.OutputTerminal = string.Empty;
            }
        }
        public string BuildTermialName()//int triggerOutline)
        {
            string termial = string.Empty;
            if (this.IsOnline)
            {
                termial = $"/{this._chassis.Resource}/PXI_Trig{Address}";
            }
            return termial;
        }

        private string _instrumentIdn;
        /// <summary>
        /// Gets IDN of the instrument.
        /// </summary>
        public string InstrumentIDN
        {
            get
            {
                if (string.IsNullOrEmpty(this._instrumentIdn))
                {
                    if (!this.IsOnline)
                    {
                        this._instrumentIdn = "PXISourceMeter_4143 offline";
                    }
                    else
                    {
                        this._instrumentIdn = CmdHandler.Identity.InstrumentModel;
                    }
                }
                return this._instrumentIdn;
            }

        }

        float _dev = 0.0f;
        public float dev
        {
            get
            {
                if (this.IsOnline == true)
                {
                    _dev = 0.0f;
                }

                return _dev;
            }
            set
            {
                if (this.IsOnline == true)
                {

                }
                else
                {
                    _dev = value;
                }
            }
        }

        float _currentSetpoint_A;
        public float CurrentSetpoint_A
        {
            get
            {
                if (this.IsOnline == true)
                {
                    _currentSetpoint_A = Convert.ToSingle(this.CmdHandler.Outputs[SLOT_TAG].Source.Current.CurrentLevel);
                }

                return _currentSetpoint_A;
            }
            set
            {
                _currentSetpoint_A = value;
                if (this.IsOnline == true)
                {
                    const double VOLTAGE_LIMIT = 4.0;
                    const double VOLTAGE_LIMIT_RANGE = 6.0;

                    this.CmdHandler.Outputs[SLOT_TAG].Measurement.Sense = DCPowerMeasurementSense.Remote;
                    Thread.Sleep(20);
                    this.CmdHandler.Source.Mode = DCPowerSourceMode.SinglePoint;
                    this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function = DCPowerSourceOutputFunction.DCCurrent;
                    this.CmdHandler.Outputs[SLOT_TAG].Source.Current.CurrentLevel = _currentSetpoint_A;
                    this.CmdHandler.Outputs[SLOT_TAG].Source.Current.VoltageLimit = VOLTAGE_LIMIT;
                    this.CmdHandler.Outputs[SLOT_TAG].Source.Current.CurrentLevelRange = Math.Abs(_currentSetpoint_A);
                    this.CmdHandler.Outputs[SLOT_TAG].Source.Current.VoltageLimitRange = Math.Abs(VOLTAGE_LIMIT_RANGE);
                    this.CmdHandler.Outputs[SLOT_TAG].Source.SourceDelay = new PrecisionTimeSpan(50.0 / 1000.0);
                }
            }
        }
        public void Reset()
        {
            if (this.IsOnline == true)
            {
                this.CmdHandler.Utility.Reset();
                //this.IsOutputOn = false;
            }
        }
        public void SetupAndEnableSourceOutput_SinglePoint_Current_mA(double currentSetpoint_mA, double complianceVoltage_V)
        {

            if (this.IsOnline == true)
            {
                this.Reset();
                this.IsOutputOn = false;
                this.CmdHandler.Source.Mode = DCPowerSourceMode.SinglePoint;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function = DCPowerSourceOutputFunction.DCCurrent;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Current.CurrentLevelRange = Math.Abs(currentSetpoint_mA / 1000.0);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Current.VoltageLimitRange = Math.Abs(complianceVoltage_V);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Current.CurrentLevel = currentSetpoint_mA / 1000.0;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Current.VoltageLimit = complianceVoltage_V;
                this.CmdHandler.Outputs[SLOT_TAG].Measurement.Sense = DCPowerMeasurementSense.Remote;
                this.CmdHandler.Outputs[SLOT_TAG].Source.SourceDelay = new PrecisionTimeSpan(10.0 / 1000.0);
                this.IsOutputOn = true;
            }
        }
        public void SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(double currentSetpoint_mA, double complianceVoltage_V)
        {

            if (this.IsOnline == true)
            {
                this.Reset();
                //this.IsOutputOn = false;
                this.CmdHandler.Source.Mode = DCPowerSourceMode.SinglePoint;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function = DCPowerSourceOutputFunction.DCCurrent;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Current.CurrentLevelRange = Math.Abs(currentSetpoint_mA / 1000.0);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Current.VoltageLimitRange = Math.Abs(complianceVoltage_V);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Current.CurrentLevel = currentSetpoint_mA / 1000.0;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Current.VoltageLimit = complianceVoltage_V;
                this.CmdHandler.Outputs[SLOT_TAG].Measurement.Sense = DCPowerMeasurementSense.Remote;

                this.CmdHandler.Outputs[SLOT_TAG].Source.SourceDelay = new PrecisionTimeSpan(10.0 / 1000.0);
                this.IsOutputOn = true;
            }
        }
        public void SetupAndEnableSourceOutput_SinglePoint_Current_mA_For_Test(double currentSetpoint_mA,double sourceCurrentRange_mA, double complianceVoltage_V)
        {

            if (this.IsOnline == true)
            {
                this.Reset();
                //this.IsOutputOn = false;
                this.CmdHandler.Source.Mode = DCPowerSourceMode.SinglePoint;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function = DCPowerSourceOutputFunction.DCCurrent;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Current.CurrentLevelRange = Math.Abs(sourceCurrentRange_mA / 1000.0);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Current.VoltageLimitRange = Math.Abs(complianceVoltage_V);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Current.CurrentLevel = currentSetpoint_mA / 1000.0;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Current.VoltageLimit = complianceVoltage_V;
                this.CmdHandler.Outputs[SLOT_TAG].Measurement.Sense = DCPowerMeasurementSense.Remote;

                this.CmdHandler.Outputs[SLOT_TAG].Source.SourceDelay = new PrecisionTimeSpan(10.0 / 1000.0);
                this.IsOutputOn = true;
            }
        }
        public void SetupAndEnableSourceOutput_SinglePoint_Voltage_V(double voltageSetpoint_V, double complianceCurrent_mA)
        {

            if (this.IsOnline == true)
            {
                this.Reset();
                this.IsOutputOn = false;

                this.CmdHandler.Source.Mode = DCPowerSourceMode.SinglePoint;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.VoltageLevelRange = Math.Abs(voltageSetpoint_V);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimitRange = Math.Abs(complianceCurrent_mA / 1000.0);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.VoltageLevel = voltageSetpoint_V;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimit = Math.Abs(complianceCurrent_mA / 1000.0);
                this.CmdHandler.Outputs[SLOT_TAG].Measurement.Sense = DCPowerMeasurementSense.Remote;

                this.CmdHandler.Outputs[SLOT_TAG].Source.SourceDelay = new PrecisionTimeSpan(10.0 / 1000.0); ;

                this.IsOutputOn = true;
            }
        }
        //public void SetMeasureCurrentRange(double complianceCurrent_mA, double measureCurrentRange_mA)
        //{
        //    if (this.IsOnline == true)
        //    {


        //        var complianceCurrent_A = Math.Round(complianceCurrent_mA / 1000.0, 6);
        //        var measureCurrentRange_A = Math.Round(measureCurrentRange_mA / 1000.0, 6);

        //        this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.On;
        //        this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimit = complianceCurrent_A;
        //        this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimitRange = measureCurrentRange_A;

        //    }
        //}
        public void SetupAndEnableSourceOutput_SinglePoint_Voltage_V(double voltageSetpoint_V, double complianceCurrent_mA , double measureCurrentRange_mA)
        {

            if (this.IsOnline == true)
            {
                this.Reset();
                this.IsOutputOn = false;

                this.CmdHandler.Source.Mode = DCPowerSourceMode.SinglePoint;
       
                this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.VoltageLevelRange = Math.Abs(voltageSetpoint_V);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.Off;

                //电流范围 CurrentLimitRange
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimitRange = Math.Round(Math.Abs(measureCurrentRange_mA / 1000.0), 6);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.VoltageLevel = voltageSetpoint_V;
                //牵制电流是 CurrentLimit 即回读电流不会大于 CurrentLimit
                //this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimit =  Math.Round( Math.Abs(complianceCurrent_mA / 1000.0),6);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimit = Math.Round(Math.Abs(measureCurrentRange_mA / 1000.0), 6);

                this.CmdHandler.Outputs[SLOT_TAG].Measurement.Sense = DCPowerMeasurementSense.Remote;

                this.CmdHandler.Outputs[SLOT_TAG].Source.SourceDelay = new PrecisionTimeSpan(10.0 / 1000.0); ;

                this.IsOutputOn = true;
            }
        }
        public void SetupAndEnableSourceOutput_SinglePoint_Voltage_V_For_Test(double voltageSetpoint_V, double complianceCurrent_mA)
        {

            if (this.IsOnline == true)
            {
                this.Reset();
                //this.IsOutputOn = false;

                this.CmdHandler.Source.Mode = DCPowerSourceMode.SinglePoint;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.VoltageLevelRange = Math.Abs(voltageSetpoint_V);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimitRange = Math.Abs(complianceCurrent_mA / 1000.0);
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.VoltageLevel = voltageSetpoint_V;
                this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimit = Math.Abs(complianceCurrent_mA / 1000.0);
                this.CmdHandler.Outputs[SLOT_TAG].Measurement.Sense = DCPowerMeasurementSense.Remote;

                this.CmdHandler.Outputs[SLOT_TAG].Source.SourceDelay = new PrecisionTimeSpan(10.0 / 1000.0); ;

                //this.IsOutputOn = true;
            }
        }
        float _currentSenseRange_A = 0.0f;
        public float CurrentSenseRange_A
        {
            get
            {
                if (this.IsOnline == true)
                {
                    _currentSenseRange_A = Convert.ToSingle(this.CmdHandler.Outputs[SLOT_TAG].Measurement.AutorangeMinimumCurrentRange);

                }

                return _currentSenseRange_A;
            }
            set
            {
                _currentSenseRange_A = value;
                if (this.IsOnline == true)
                {
                    this.CmdHandler.Outputs[SLOT_TAG].Measurement.AutorangeMinimumCurrentRange = _currentSenseRange_A;
                }
            }
        }

        bool _isOutputOn = false;
        public bool IsOutputOn
        {
            get
            {
                if (this.IsOnline == true)
                {
                    _isOutputOn = this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Enabled;



                }

                return _isOutputOn;
            }
            set
            {
                _isOutputOn = value;
                if (this.IsOnline == true)
                {

                    this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Enabled = _isOutputOn;

                    if (value == true)
                    {
                        this.CmdHandler.Control.Initiate();
                        this.CmdHandler.Events.SourceCompleteEvent.WaitForEvent(new PrecisionTimeSpan(5.0));
                    }
                    else
                    {
                        this.CmdHandler.Outputs[SLOT_TAG].Control.Abort();

                    }
                }
            }
        }

        public bool TriggerOutputOn
        {

            set
            {
                _isOutputOn = value;
                if (this.IsOnline == true)
                {

                    this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Enabled = _isOutputOn;


                }
            }
        }

        float _voltageSetpoint_V = 0.0f;
        public float VoltageSetpoint_V
        {
            get
            {
                if (this.IsOnline == true)
                {

                    _voltageSetpoint_V = (float)this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.VoltageLevel;

                }

                return _voltageSetpoint_V;
            }
            set
            {
                _voltageSetpoint_V = value;
                if (this.IsOnline == true)
                {
                    const double CURRENT_LIMIT = 200.0 / 1000.0;//200mA
                    const double CURRENT_LIMIT_RANGE = 200.0 / 1000.0;//200mA

                    this.CmdHandler.Source.Mode = DCPowerSourceMode.SinglePoint;
                    this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
                    this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.VoltageLevel = _voltageSetpoint_V;
                    this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimit = CURRENT_LIMIT;
                    this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.VoltageLevelRange = _voltageSetpoint_V;
                    this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimitRange = CURRENT_LIMIT_RANGE;
                    this.CmdHandler.Outputs[SLOT_TAG].Source.SourceDelay = new PrecisionTimeSpan(50.0 / 1000.0); ;

                }
            }
        }


        SourceModeTypes _sourceMode = SourceModeTypes.Current;

        public SourceModeTypes SourceMode
        {
            get
            {
                if (this.IsOnline == true)
                {
                    var pxiActuallMode = this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function;

                    switch (pxiActuallMode)
                    {
                        case DCPowerSourceOutputFunction.DCCurrent:
                        case DCPowerSourceOutputFunction.PulseCurrent:
                            {
                                _sourceMode = SourceModeTypes.Current;
                            }
                            break;
                        case DCPowerSourceOutputFunction.DCVoltage:
                        case DCPowerSourceOutputFunction.PulseVoltage:
                            {
                                _sourceMode = SourceModeTypes.Voltage;
                            }
                            break;
                    }
                }
                return _sourceMode;
            }
            set
            {
                _sourceMode = value;
                if (this.IsOnline == true)
                {
                    switch (_sourceMode)
                    {
                        case SourceModeTypes.Current:
                            {
                                this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function = DCPowerSourceOutputFunction.DCCurrent;
                            }
                            break;
                        case SourceModeTypes.Voltage:
                            {
                                this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
                            }
                            break;
                    }
                }
            }
        }

        public DCPowerFetchResult Fetch_MeasureVals(int dataCount, double timeout_ms)
        {

            if (this.IsOnline == false)
            {
                return new DCPowerFetchResult();
            }
            DCPowerFetchResult measuredResults = this.CmdHandler.Measurement.Fetch(SLOT_TAG, new PrecisionTimeSpan(timeout_ms / 1000.0), dataCount);
            return measuredResults;
        }

        public double[] Fetch_SenseCurrents(int dataCount, double timeout_ms)
        {
            if (this.IsOnline == false)
            {
                return new double[0];
            }
            DCPowerFetchResult measuredResults = this.CmdHandler.Measurement.Fetch(SLOT_TAG, new PrecisionTimeSpan(timeout_ms / 1000.0), dataCount);
            measuredResults_CurrentMeasurements = measuredResults.CurrentMeasurements;
            return measuredResults.CurrentMeasurements;
        }

        public double[] Fetch_SenseVoltages(int dataCount, double timeout_ms)
        {
            if (this.IsOnline == false)
            {
                return new double[0];
            }
            DCPowerFetchResult measuredResults = this.CmdHandler.Measurement.Fetch(SLOT_TAG, new PrecisionTimeSpan(timeout_ms / 1000.0), dataCount);
            measuredResults_VoltageMeasurements = measuredResults.VoltageMeasurements;
            return measuredResults.VoltageMeasurements;
        }

        public float ReadCurrent_A()
        {
            if (this.IsOnline == true)
            {
                DCPowerMeasureResult result = this.CmdHandler.Measurement.Measure(SLOT_TAG);
                return Convert.ToSingle(result.CurrentMeasurements[0]);
            }
            return 0.0f;
        }
     
        public float ReadCurrent_A_Once(double source_voltage_V)
        {
            if (this.IsOnline == true)
            {

                const double CURRENT_SENCE_LIMIT_A = 0.001; //1mA
                const double CURRENT_SENCE_LIMIT_RANGE_A = 0.001;//1mA
                const double SOURCE_DELAY_SECOND = 5 / 1000.0;//5ms
                const double APERTURE_TIME_SECOND = 10 / 1000.0;//10mA
                const int SAMPLE_COUNT = 10;
                var DCVoltageSourceSettings = new DCVoltageSourceSettings()
                {
                    CurrentLimit = CURRENT_SENCE_LIMIT_A,
                    CurrentLimitRange = CURRENT_SENCE_LIMIT_RANGE_A,
                    SourceDelay = SOURCE_DELAY_SECOND,
                    ApertureTime = APERTURE_TIME_SECOND
                };
                var sourceSequenceSettings = new ConstantSequenceSettings(source_voltage_V, SAMPLE_COUNT);
                SmuUtility.ConfigureDCVoltageSequence(this.CmdHandler, DCVoltageSourceSettings, sourceSequenceSettings.GetSequence());

                this.CmdHandler.Outputs[SLOT_TAG].Control.Commit();
                this.CmdHandler.Outputs[SLOT_TAG].Control.Initiate();

                DCPowerFetchResult result = this.CmdHandler.Measurement.Fetch(SLOT_TAG, new PrecisionTimeSpan(10), SAMPLE_COUNT);
                var currVal_A = result.CurrentMeasurements.Average();

                return Convert.ToSingle(currVal_A);

            }
            return 0.0f;
        }

        public float ReadVoltage_V_Once(double source_current_A)
        {
            if (this.IsOnline == true)
            {

                const double VOLTAGE_SENCE_LIMIT_A = 4.0; //1mA
                const double VOLTAGE_SENCE_LIMIT_RANGE_A = 4.0;//1mA
                const double SOURCE_DELAY_SECOND = 5 / 1000.0;//5ms
                const double APERTURE_TIME_SECOND = 10 / 1000.0;//10mA
                const int SAMPLE_COUNT = 10;
                var DCCurrentSourceSettings = new DCCurrentSourceSettings()
                {
                    VoltageLimit = VOLTAGE_SENCE_LIMIT_A,
                    VoltageLimitRange = VOLTAGE_SENCE_LIMIT_RANGE_A,
                    SourceDelay = SOURCE_DELAY_SECOND,
                    ApertureTime = APERTURE_TIME_SECOND
                };
                var sourceSequenceSettings = new ConstantSequenceSettings(source_current_A, SAMPLE_COUNT);
                SmuUtility.ConfigureDCCurrentSequence(/*SLOT_TAG,*/ this.CmdHandler, DCCurrentSourceSettings, sourceSequenceSettings.GetSequence());

                this.CmdHandler.Outputs[SLOT_TAG].Control.Commit();
                this.CmdHandler.Outputs[SLOT_TAG].Control.Initiate();

                DCPowerFetchResult result = this.CmdHandler.Measurement.Fetch(SLOT_TAG, new PrecisionTimeSpan(10), SAMPLE_COUNT);
                var voltVal_A = result.VoltageMeasurements.Average();

                return Convert.ToSingle(voltVal_A);

            }
            return 0.0f;
        }

        public float ReadVoltage_V()
        {
            if (this.IsOnline == true)
            {
                DCPowerMeasureResult result = this.CmdHandler.Measurement.Measure(this._chassis.Resource + "/" + SLOT_TAG);
                return Convert.ToSingle(result.VoltageMeasurements[0]);


            }
            return 0.0f;
        }
        public void SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning2(float startCurrent_mA,
                          float stopCurrent_mA,
                          double[] CurrentArray_mA,
                          float complianceVoltage_V,
                          float ApertureTime_s,
                          bool IsFourWireOn)
        {
            var currentSourceSettingsLD = new DCCurrentSourceSettings();
            currentSourceSettingsLD.SourceDelay = 0.01;
            currentSourceSettingsLD.ApertureTime = ApertureTime_s;
            currentSourceSettingsLD.VoltageLimit = complianceVoltage_V;
            currentSourceSettingsLD.VoltageLimitRange = complianceVoltage_V;

            currentSourceSettingsLD.SenseModel = IsFourWireOn ? DCPowerMeasurementSense.Remote : DCPowerMeasurementSense.Local;
            Thread.Sleep(20);
            if (startCurrent_mA > stopCurrent_mA == true)
            {
                currentSourceSettingsLD.CurrentLevelRange = startCurrent_mA / 1000.0f;
            }
            else
            {
                currentSourceSettingsLD.CurrentLevelRange = stopCurrent_mA / 1000.0f;
            }

            for (int i = 0; i < CurrentArray_mA.Length; i++)
            {
                CurrentArray_mA[i] /= 1000.0;
            }
            //var primarySweepSequenceSettings = new SweepSequenceSettings
            //(
            //    startCurrent_mA / 1000.0,
            //    stopCurrent_mA / 1000.0,
            //    stepCurrent_mA / 1000.0
            //);
            SmuUtility.ConfigureDCCurrentSequence2(/*SLOT_TAG,*/ this.CmdHandler, currentSourceSettingsLD, CurrentArray_mA, 0.01);
        }
        public void SetupMaster_Sequence_SourceCurrent_SenseVoltage_MTuning(float startCurrent_mA,
                          float stopCurrent_mA,
                          double[] CurrentArray_mA,
                          float complianceVoltage_V,
                          float ApertureTime_s,
                          bool IsFourWireOn)
        {
            var currentSourceSettingsLD = new DCCurrentSourceSettings();
            currentSourceSettingsLD.SourceDelay = 0.001;
            currentSourceSettingsLD.ApertureTime = ApertureTime_s;
            currentSourceSettingsLD.VoltageLimit = complianceVoltage_V;
            currentSourceSettingsLD.VoltageLimitRange = complianceVoltage_V;

            currentSourceSettingsLD.SenseModel = IsFourWireOn ? DCPowerMeasurementSense.Remote : DCPowerMeasurementSense.Local;
            Thread.Sleep(20);
            if (startCurrent_mA > stopCurrent_mA == true)
            {
                currentSourceSettingsLD.CurrentLevelRange = startCurrent_mA / 1000.0f;
            }
            else
            {
                currentSourceSettingsLD.CurrentLevelRange = stopCurrent_mA / 1000.0f;
            }

            for (int i = 0; i < CurrentArray_mA.Length; i++)
            {
                CurrentArray_mA[i] /= 1000.0;
            }
            //var primarySweepSequenceSettings = new SweepSequenceSettings
            //(
            //    startCurrent_mA / 1000.0,
            //    stopCurrent_mA / 1000.0,
            //    stepCurrent_mA / 1000.0
            //);
            SmuUtility.ConfigureDCCurrentSequence(/*SLOT_TAG,*/ this.CmdHandler, currentSourceSettingsLD, CurrentArray_mA);
        }
        public void SetupMaster_Sequence_SourceCurrent_SenseVoltage(float startCurrent_mA,
                           float stepCurrent_mA,
                           float stopCurrent_mA,
                           float complianceVoltage_V,
                          double sourceDelay_s,
                           double apertureTime_s,
                           bool IsFourWireOn)
        {
            var currentSourceSettingsLD = new DCCurrentSourceSettings();
            //currentSourceSettingsLD.SourceDelay = 0.01;
            //currentSourceSettingsLD.ApertureTime = 0.005;
            currentSourceSettingsLD.SourceDelay = sourceDelay_s;
            currentSourceSettingsLD.ApertureTime = apertureTime_s;
            currentSourceSettingsLD.VoltageLimit = complianceVoltage_V;
            currentSourceSettingsLD.VoltageLimitRange = complianceVoltage_V;

            currentSourceSettingsLD.SenseModel = IsFourWireOn ? DCPowerMeasurementSense.Remote : DCPowerMeasurementSense.Local;
            Thread.Sleep(20);
            if (startCurrent_mA > stopCurrent_mA == true)
            {
                currentSourceSettingsLD.CurrentLevelRange = startCurrent_mA / 1000.0f;
            }
            else
            {
                currentSourceSettingsLD.CurrentLevelRange = stopCurrent_mA / 1000.0f;
            }


            var primarySweepSequenceSettings = new SweepSequenceSettings
            (
                startCurrent_mA / 1000.0,
                stopCurrent_mA / 1000.0,
                stepCurrent_mA / 1000.0
            );
            SmuUtility.ConfigureDCCurrentSequence(/*SLOT_TAG,*/ this.CmdHandler, currentSourceSettingsLD, primarySweepSequenceSettings.GetSequence());
        }






        //public void SetupMaster_Sequence_SourceVoltage_SenseCurrent(float startVoltage_V,
        //            float stepVoltage_V,
        //            float stopVoltage_V,
        //            float complianceCurrent_mA, 
        //            bool CurrentAutoRange,
        //            bool IsFourWireOn
                   
        //             )
        //{
        //    var voltageSourceSettingsEPD = new DCVoltageSourceSettings();
        //    voltageSourceSettingsEPD.SourceDelay = 0.01;
        //    voltageSourceSettingsEPD.CurrentLimit = complianceCurrent_mA;
        //    voltageSourceSettingsEPD.ApertureTime = 0.005;
        //    if (CurrentAutoRange)
        //    {
        //        voltageSourceSettingsEPD.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.On;
        //    }
        //    else
        //    {
        //        voltageSourceSettingsEPD.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.Off;
        //        voltageSourceSettingsEPD.CurrentLimitRange = complianceCurrent_mA;
        //    }
        //    voltageSourceSettingsEPD.SenseModel = IsFourWireOn ? DCPowerMeasurementSense.Remote : DCPowerMeasurementSense.Local;
        //    Thread.Sleep(20);

        //    var primarySweepSequenceSettings = new SweepSequenceSettings
        //    (
        //        startVoltage_V,
        //        stopVoltage_V,
        //        stepVoltage_V
        //    );
        //    SmuUtility.ConfigureDCVoltageSequence(this.CmdHandler, voltageSourceSettingsEPD, primarySweepSequenceSettings.GetSequence());
        //}
        public void SetupMaster_Sequence_SourceVoltage_SenseCurrent(float startVoltage_V,
                    float stepVoltage_V,
                    float stopVoltage_V,
                    float complianceCurrent_mA,
                    double SourceDelay_s,
                    double ApertureTime_s,
                    bool CurrentAutoRange,
                    bool IsFourWireOn)
        {
            var voltageSourceSettingsEPD = new DCVoltageSourceSettings();
            voltageSourceSettingsEPD.SourceDelay = SourceDelay_s;
            voltageSourceSettingsEPD.ApertureTime = ApertureTime_s;
            voltageSourceSettingsEPD.CurrentLimit = complianceCurrent_mA;
         
            if (CurrentAutoRange)
            {
                voltageSourceSettingsEPD.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.On;
            }
            else
            {
                voltageSourceSettingsEPD.CurrentLimitAutorange = DCPowerSourceCurrentLimitAutorange.Off;
                voltageSourceSettingsEPD.CurrentLimitRange = complianceCurrent_mA;
            }
            voltageSourceSettingsEPD.SenseModel = IsFourWireOn ? DCPowerMeasurementSense.Remote : DCPowerMeasurementSense.Local;
            Thread.Sleep(20);

            var primarySweepSequenceSettings = new SweepSequenceSettings
            (
                startVoltage_V,
                stopVoltage_V,
                stepVoltage_V
            );
            SmuUtility.ConfigureDCVoltageSequence(this.CmdHandler, voltageSourceSettingsEPD, primarySweepSequenceSettings.GetSequence());
        }
        public void SetupSlaver_Sequence_SourceVoltage_SenceCurrent
        (
             float sourceVoltage_V,
             float complianceCurrent_mA,
             int dataCount,
                double sourceDelay_s,
             double apertureTime_s,
             bool IsFourWireOn
             )
        {
            var voltageSourceSettingsEPD = new DCVoltageSourceSettings();
            voltageSourceSettingsEPD.SourceDelay = sourceDelay_s;
            voltageSourceSettingsEPD.ApertureTime = apertureTime_s;
            voltageSourceSettingsEPD.CurrentLimit = complianceCurrent_mA / 1000.0;
            voltageSourceSettingsEPD.CurrentLimitRange = complianceCurrent_mA / 1000.0;
            voltageSourceSettingsEPD.SenseModel = IsFourWireOn ? DCPowerMeasurementSense.Remote : DCPowerMeasurementSense.Local;
            Thread.Sleep(20);
            var secondarySequenceSettingsEPD = new ConstantSequenceSettings(sourceVoltage_V, dataCount);

            SmuUtility.ConfigureDCVoltageSequence(this.CmdHandler, voltageSourceSettingsEPD, secondarySequenceSettingsEPD.GetSequence());
        }

        //电流源, 读电压
        public void SetupSlaver_Sequence_SourceCurrent_SenceVoltage
        (
             float sourceCurrent_mA,
             float complianceVoltage_V,
             int dataCount,
             double sourceDelay_s,
             double apertureTime_s,
             bool IsFourWireOn
             )
        {
            var currentSourceSettingsLD = new DCCurrentSourceSettings();
            currentSourceSettingsLD.CurrentLevelRange = sourceCurrent_mA / 1000.0f;
            currentSourceSettingsLD.SourceDelay = sourceDelay_s;
            currentSourceSettingsLD.ApertureTime = apertureTime_s;
            currentSourceSettingsLD.VoltageLimit = complianceVoltage_V;
            currentSourceSettingsLD.VoltageLimitRange = complianceVoltage_V;
            currentSourceSettingsLD.SenseModel = IsFourWireOn ? DCPowerMeasurementSense.Remote : DCPowerMeasurementSense.Local;
            Thread.Sleep(20);

            var secondarySequenceSettingsEPD = new ConstantSequenceSettings(sourceCurrent_mA / 1000.0f, dataCount);

            SmuUtility.ConfigureDCCurrentSequence(this.CmdHandler, currentSourceSettingsLD, secondarySequenceSettingsEPD.GetSequence());
        }
        //public void SetupSlaver_Sequence_SourceCurrent_SenceVoltage
        //(
        //   float sourceCurrent_mA,
        //   float complianceVoltage_V,
        //   int dataCount
        //)
        //{
        //    var currentSourceSettingsLD = new DCCurrentSourceSettings();
        //    currentSourceSettingsLD.SourceDelay = 0.001;
        //    currentSourceSettingsLD.VoltageLimit = complianceVoltage_V;
        //    currentSourceSettingsLD.VoltageLimitRange = complianceVoltage_V;
        //    //设置量程
        //    currentSourceSettingsLD.CurrentLevelRange = sourceCurrent_mA / 1000.0f;

        //    var secondarySequenceSettingsEPD = new ConstantSequenceSettings(sourceCurrent_mA / 1000.0, dataCount);
        //    SmuUtility.ConfigureDCCurrentSequence(/*SLOT_TAG,*/ this.CmdHandler, currentSourceSettingsLD, secondarySequenceSettingsEPD.GetSequence());
        //}



        public void AbortTrigger()
        {
            if (this.IsOnline == false)
            {
                return;
            }
            this.CmdHandler.Outputs[SLOT_TAG].Control.Abort();
        }

        #region pulse function
        public void SetupMaster_Sequence_Pulse_SourceCurrent_SenseVoltage(float startCurrent_mA,
                  float stepCurrent_mA,
                  float stopCurrent_mA,
                  float complianceVoltage_V,
                  float period_ms,
                  float duty_rate_percent)
        {
            PulseCurrentSourceSettings currentSourceSettingsLD = new PulseCurrentSourceSettings();
            currentSourceSettingsLD.SourceDelay = 0.0;
            currentSourceSettingsLD.PulseBiasCurrentLevel = 0.0;
            currentSourceSettingsLD.SenseModel = DCPowerMeasurementSense.Remote;//1120 四线制
            Thread.Sleep(20);

            currentSourceSettingsLD.PulseVoltageLimit = complianceVoltage_V;
            currentSourceSettingsLD.PulseVoltageLimitRange = complianceVoltage_V;


            double PulseOnTime = (period_ms / 1000.0) * (duty_rate_percent / 100.0);

            currentSourceSettingsLD.PulseOnTime = PulseOnTime;
            currentSourceSettingsLD.PulseOffTime = (period_ms / 1000.0) * (1.0 - (duty_rate_percent / 100.0));
            //设置采样时间为脉宽的10%
            //currentSourceSettingsLD.ApertureTime = PulseOnTime * 0.30;
            //currentSourceSettingsLD.SourceDelay = PulseOnTime * 0.20;

            //20231102 林斌重新规划采样时间
            //去掉最后的5%不用
            //从后向前取65%到95%做采样
            //Delay掉前面的65%
            currentSourceSettingsLD.ApertureTime = PulseOnTime * 0.25;
            currentSourceSettingsLD.SourceDelay = PulseOnTime * 0.7;

            //currentSourceSettingsLD.SourceDelay = 0.0003;
            //currentSourceSettingsLD.PulseOnTime = 0.001;
            //currentSourceSettingsLD.PulseOffTime = 0.010;


            if (startCurrent_mA > stopCurrent_mA == true)
            {
                currentSourceSettingsLD.PulseCurrentLevelRange = startCurrent_mA / 1000.0f;
            }
            else
            {
                currentSourceSettingsLD.PulseCurrentLevelRange = stopCurrent_mA / 1000.0f;
            }


            var primarySweepSequenceSettings = new SweepSequenceSettings
            (
                startCurrent_mA / 1000.0,
                stopCurrent_mA / 1000.0,
                stepCurrent_mA / 1000.0
            );
            //SmuUtility.ConfigureDCCurrentSequence(this.Chassis, currentSourceSettingsLD, primarySweepSequenceSettings.GetSequence());

            SmuUtility.ConfigurePulseCurrentSequence(this.CmdHandler, currentSourceSettingsLD, primarySweepSequenceSettings.GetSequence());
        }
        public void SetupMaster_Sequence_Pulse_SourceVoltage_SenseCurrent(float startVoltage_V,
         float stepVoltage_V,
         float stopVoltage_V,
         float complianceCurrent_mA,
         float period_ms,
         float duty_rate_percent)
        {

            var voltageSourceSettingsEPD = new PulseVoltageSourceSettings();
            voltageSourceSettingsEPD.SourceDelay = 0.0;
            voltageSourceSettingsEPD.PulseOnTime = (period_ms / 1000.0) * (duty_rate_percent / 100.0);
            voltageSourceSettingsEPD.PulseOffTime = (period_ms / 1000.0) * (1.0 - (duty_rate_percent / 100.0));
            voltageSourceSettingsEPD.PulseCurrentLimit = complianceCurrent_mA / 1000.0;
            voltageSourceSettingsEPD.PulseCurrentLimitRange = complianceCurrent_mA / 1000.0;

            var primarySweepSequenceSettings = new SweepSequenceSettings
            (
                startVoltage_V,
                stopVoltage_V,
                stepVoltage_V
            );
            SmuUtility.ConfigurePulseVoltageSequence(this.CmdHandler, voltageSourceSettingsEPD, primarySweepSequenceSettings.GetSequence());
        }
        public void SetupSlaver_Sequence_Pulse_SourceCurrent_SenceVoltage
      (
         float sourceCurrent_mA,
         float complianceVoltage_V,
         int dataCount,
         float period_ms,
         float duty_rate_percent
      )
        {
            var currentSourceSettingsLD = new PulseCurrentSourceSettings();
            currentSourceSettingsLD.SourceDelay = 0.0;
            currentSourceSettingsLD.PulseBiasCurrentLevel = 0.0;
            currentSourceSettingsLD.PulseOnTime = (period_ms / 1000.0) * (duty_rate_percent / 100.0);
            currentSourceSettingsLD.PulseOffTime = (period_ms / 1000.0) * (1.0 - (duty_rate_percent / 100.0));
            currentSourceSettingsLD.PulseVoltageLimit = complianceVoltage_V;
            currentSourceSettingsLD.PulseVoltageLimitRange = complianceVoltage_V;

            //设置量程
            currentSourceSettingsLD.PulseCurrentLevelRange = sourceCurrent_mA / 1000.0f;

            var secondarySequenceSettingsEPD = new ConstantSequenceSettings(sourceCurrent_mA / 1000.0, dataCount);
            SmuUtility.ConfigurePulseCurrentSequence(this.CmdHandler, currentSourceSettingsLD, secondarySequenceSettingsEPD.GetSequence());
        }

        public void SetupSlaver_Sequence_Pulse_SourceVoltage_SenceCurrent
       (
            float sourceVoltage_V,
            float complianceCurrent_mA,
            int dataCount,
         float period_ms,
         float duty_rate_percent)
        {


            var voltageSourceSettingsEPD = new PulseVoltageSourceSettings();
            //voltageSourceSettingsEPD.SourceDelay = 0.0;

            double PulseOnTime = (period_ms / 1000.0) * (duty_rate_percent / 100.0);

            voltageSourceSettingsEPD.PulseOnTime = PulseOnTime;
            voltageSourceSettingsEPD.PulseOffTime = (period_ms / 1000.0) * (1.0 - (duty_rate_percent / 100.0));
            //设置采样时间为脉宽的10%
            //voltageSourceSettingsEPD.ApertureTime = PulseOnTime * 0.10;

            //20231102 林斌重新规划采样时间
            //收到脉冲的时候, 后面只剩35%时间, 去掉最后的5%不用
            //voltageSourceSettingsEPD.ApertureTime = PulseOnTime * 0.30;

            voltageSourceSettingsEPD.ApertureTime = PulseOnTime * 0.25;
            voltageSourceSettingsEPD.SourceDelay = PulseOnTime * 0.7;

            voltageSourceSettingsEPD.PulseBiasCurrentLimit = complianceCurrent_mA / 1000.0;
            voltageSourceSettingsEPD.PulseCurrentLimitRange = complianceCurrent_mA / 1000.0;
            voltageSourceSettingsEPD.PulseCurrentLimit = complianceCurrent_mA / 1000.0;
            //voltageSourceSettingsEPD.SenseModel = DCPowerMeasurementSense.Local;//2线
            voltageSourceSettingsEPD.SenseModel = DCPowerMeasurementSense.Remote;//4线
            Thread.Sleep(20);
            var secondarySequenceSettingsEPD = new ConstantSequenceSettings(sourceVoltage_V, dataCount);


            SmuUtility.ConfigurePulseVoltageSequence(this.CmdHandler, voltageSourceSettingsEPD, secondarySequenceSettingsEPD.GetSequence());
        }

        #endregion

        public DCPowerFetchResult Sweep_LD_once(float startCurrent_mA,
                            float stepCurrent_mA,
                            float endCurrent_mA,
                            float complianceVoltage_V,
                            float ApertureTime_s)
        {
            double[] dfbCurrents_A = ArrayMath.CalculateArray(startCurrent_mA, endCurrent_mA, stepCurrent_mA);

            this.CmdHandler.Source.Mode = DCPowerSourceMode.Sequence;
            this.CmdHandler.Outputs[SLOT_TAG].Measurement.ConfigureApertureTime(ApertureTime_s, DCPowerMeasureApertureTimeUnits.Seconds);

            this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function = DCPowerSourceOutputFunction.DCCurrent;
            this.CmdHandler.Outputs[SLOT_TAG].Source.Current.CurrentLevelRange = endCurrent_mA;
            this.CmdHandler.Outputs[SLOT_TAG].Source.Current.VoltageLimit = complianceVoltage_V;
            this.CmdHandler.Outputs[SLOT_TAG].Source.Current.VoltageLimitRange = complianceVoltage_V;
            this.CmdHandler.Outputs[SLOT_TAG].Source.TransientResponse = DCPowerSourceTransientResponse.Normal;
            this.CmdHandler.Outputs[SLOT_TAG].Source.SourceDelay = new PrecisionTimeSpan(0.0);
            this.CmdHandler.Outputs[SLOT_TAG].Measurement.RecordLength = 200;
            this.CmdHandler.Measurement.Configuration.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete;
            this.CmdHandler.Outputs[SLOT_TAG].Source.SetSequence(dfbCurrents_A);
            this.CmdHandler.Control.Commit();
            double measureRecordDeltaTime = this.CmdHandler.Outputs[SLOT_TAG].Measurement.RecordDeltaTime;

            this.CmdHandler.Control.Initiate();

            PrecisionTimeSpan fetchTimeout = new PrecisionTimeSpan(measureRecordDeltaTime * 200 * 2 * 2);
            DCPowerFetchResult result = this.CmdHandler.Measurement.Fetch(SLOT_TAG, fetchTimeout, dfbCurrents_A.Length);
            return result;
        }

        public DCPowerFetchResult Sweep_EA_once(float startVoltage_V,
                    float stepVoltage_V,
                    float stopVoltage_V,
                    float complianceCurrent_mA,
                    float ApertureTime_s)
        {
            double[] dfbCurrents_A = ArrayMath.CalculateArray(startVoltage_V, stepVoltage_V, stopVoltage_V);

            this.CmdHandler.Source.Mode = DCPowerSourceMode.Sequence;
            this.CmdHandler.Outputs[SLOT_TAG].Measurement.ConfigureApertureTime(ApertureTime_s / 1000000, DCPowerMeasureApertureTimeUnits.Seconds);

            this.CmdHandler.Outputs[SLOT_TAG].Source.Output.Function = DCPowerSourceOutputFunction.DCVoltage;
            this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimit = complianceCurrent_mA;
            this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.VoltageLevelRange = stopVoltage_V;
            this.CmdHandler.Outputs[SLOT_TAG].Source.Voltage.CurrentLimitRange = complianceCurrent_mA;
            this.CmdHandler.Outputs[SLOT_TAG].Source.TransientResponse = DCPowerSourceTransientResponse.Normal;
            this.CmdHandler.Outputs[SLOT_TAG].Source.SourceDelay = new PrecisionTimeSpan(0.0);
            this.CmdHandler.Outputs[SLOT_TAG].Measurement.RecordLength = 200;
            this.CmdHandler.Measurement.Configuration.MeasureWhen = DCPowerMeasurementWhen.AutomaticallyAfterSourceComplete;
            this.CmdHandler.Outputs[SLOT_TAG].Source.SetSequence(dfbCurrents_A);
            this.CmdHandler.Control.Commit();
            double measureRecordDeltaTime = this.CmdHandler.Outputs[SLOT_TAG].Measurement.RecordDeltaTime;

            this.CmdHandler.Control.Initiate();

            PrecisionTimeSpan fetchTimeout = new PrecisionTimeSpan(measureRecordDeltaTime * 200 * 2 * 2);
            DCPowerFetchResult result = this.CmdHandler.Measurement.Fetch(SLOT_TAG, fetchTimeout, dfbCurrents_A.Length);
            return result;
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
           
            //throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
            //throw new NotImplementedException();
        }
    }
}
