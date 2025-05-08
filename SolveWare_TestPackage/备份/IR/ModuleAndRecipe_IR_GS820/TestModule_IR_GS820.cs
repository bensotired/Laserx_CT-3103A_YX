using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Threading;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
     (
         "TestCalculator_IRValue"

     )]
    [ConfigurableInstrument("ISourceMeter_GS820", "SourceMeter_GS820", "用于IR扫描")]
    public class TestModule_IR_GS820 : TestModuleBase
    {

        public TestModule_IR_GS820() : base()
        {
        }

        ISourceMeter_GS820 SourceMeter_GS820 { get { return (ISourceMeter_GS820)this.ModuleResource["SourceMeter_GS820"]; } }

        TestRecipe_IR_GS820 TestRecipe { get; set; }
        RawData_IR RawData { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_IR_GS820);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_IR();
            return RawData;
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_IR_GS820>(testRecipe);
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                if (SourceMeter_GS820 == null)
                {
                    return;
                }
                double workVoltage = this.TestRecipe.Voltage_V;

                double complianceCurrent_A = this.TestRecipe.ComplianceCurrent_A;
                double senseCurrent_A = this.TestRecipe.senseCurrent_uA / 1000000.0;

                this.SourceMeter_GS820.Reset();
                Thread.Sleep(50);//否则setmode时间很长
                this.SourceMeter_GS820.SetMode(Keithley2602BChannel.CHA, SourceMeterMode.SourceVoltageSenceCurrent);
                this.SourceMeter_GS820.SetVoltage_V(Keithley2602BChannel.CHA, workVoltage);
                this.SourceMeter_GS820.SetComplianceCurrent_A(Keithley2602BChannel.CHA, complianceCurrent_A);
                this.SourceMeter_GS820.SetMeasureCurrentRange_A(Keithley2602BChannel.CHA, senseCurrent_A);

                this.SourceMeter_GS820.OutputOn();
                Thread.Sleep(50);
                var ret = this.SourceMeter_GS820.MeasureCurrent_A(Keithley2602BChannel.CHA);
                double iR_Current_A = ret;
                this.RawData.Add(new RawDatumItem_IR() { IRIndex = 1, IR_Current_A = iR_Current_A });
            }
            catch (Exception ex)
            {
            }
            finally
            {
                this.SourceMeter_GS820.OutputOff();
            }
        }


    }
}