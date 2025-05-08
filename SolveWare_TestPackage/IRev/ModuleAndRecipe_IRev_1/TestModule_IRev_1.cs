using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_IRev")]
    [ConfigurableInstrument("ISourceMeter_Golight", "GoLight_1", "用于驱动器件")]
    public class TestModule_IRev_1 : TestModuleBase
    {
        ISourceMeter_Golight SourceMeter_Master { get { return (ISourceMeter_Golight)this.ModuleResource["GoLight_1"]; } }
        public TestModule_IRev_1() : base() { }
        TestRecipe_IRev_1 TestRecipe { get; set; }
        RawData_IRev RawData { get; set; }

        public override Type GetTestRecipeType() { return typeof(TestRecipe_IRev_1); }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_IRev_1>(testRecipe);
        }


        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_IRev();
            return RawData;
        }

        public override void Run(CancellationToken token)
        {
            try
            {
                if (SourceMeter_Master == null)
                {
                    return;
                }
                if (!SourceMeter_Master.IsOnline)
                {
                    SourceMeter_Master.Timeout_ms = 1000;
                    SourceMeter_Master.CurrentSetpoint_A = 0;
                    SourceMeter_Master.VoltageSetpoint_V = 0;
                    SourceMeter_Master.VoltageSetpoint_PD_V = 0;
                    SourceMeter_Master.VoltageSetpoint_EA_V = 0;
                    SourceMeter_Master.IsOutputEnable = false;
                }
                SourceMeter_Master.Timeout_ms = 60 * 1000;

                this.Log_Global($"开始测试!");
                SourceMeter_Master.IsOutputEnable = true;

                //float startc = Convert.ToSingle(this.TestRecipe.Start_voltage_V);
                //float stepc = Convert.ToSingle(this.TestRecipe.Step_voltage_V);
                //float stopc = Convert.ToSingle(this.TestRecipe.Stop_voltage_V);
                List<float> voltage = new List<float>();
                List<float> current = new List<float>();

                SourceMeter_Master.VoltageSetpoint_V = Convert.ToSingle(this.TestRecipe.Start_voltage_V);
                Thread.Sleep(50);
                voltage.Add(Convert.ToSingle(this.TestRecipe.Start_voltage_V));
                current.Add(SourceMeter_Master.ReadCurrent_A());
                Thread.Sleep(50);

                SourceMeter_Master.VoltageSetpoint_V = Convert.ToSingle(this.TestRecipe.Step_voltage_V);
                Thread.Sleep(50);
                voltage.Add(Convert.ToSingle(this.TestRecipe.Step_voltage_V));
                current.Add(SourceMeter_Master.ReadCurrent_A());
                Thread.Sleep(50);

                SourceMeter_Master.VoltageSetpoint_V = Convert.ToSingle(this.TestRecipe.Stop_voltage_V);
                Thread.Sleep(50);
                voltage.Add(Convert.ToSingle(this.TestRecipe.Stop_voltage_V));
                current.Add(SourceMeter_Master.ReadCurrent_A());
                Thread.Sleep(50);

                for (int i = 0; i < voltage.Count; i++)
                {
                    RawData.Add(new RawDatumItem_IRev()
                    {
                        Voltage_V = voltage[i],
                        Current_A = current[i],
                    });
                }





                //for (float i = startc; i <= stopc; i += stepc)
                //{
                //    SourceMeter_Master.VoltageSetpoint_V = i;
                //    Thread.Sleep(50);
                //    voltage.Add(i);
                //    current.Add(SourceMeter_Master.ReadCurrent_A());
                //}



            }
            catch (Exception ex)
            {

            }
            finally
            {
                SourceMeter_Master.IsOutputEnable = false;
            }
        }
    }
}
