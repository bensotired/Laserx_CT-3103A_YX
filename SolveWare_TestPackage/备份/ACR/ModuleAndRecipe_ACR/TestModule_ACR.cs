using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_ACR")]
    [ConfigurableInstrument("ITH2810D", "TH2810D_Master", "用于测量电阻")]
    public class TestModule_ACR : TestModuleBase
    {
        public TestModule_ACR() : base() { }


        ITH2810D TH2810D_Master { get { return (ITH2810D)this.ModuleResource["TH2810D_Master"]; } }
        TestRecipe_ACR TestRecipe { get; set; }
        RawData_ACR RawData { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_ACR);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_ACR();
            
            return RawData;
            
        }
     
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_ACR>(testRecipe);
        }
        public static Decimal ChangeDataToD(string strData)
        {
            Decimal dData = 0.0M;
            if (strData.Contains("e"))
            {
                dData = Convert.ToDecimal(Decimal.Parse(strData.ToString(), System.Globalization.NumberStyles.Float));
            }
            return dData;
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                ////虚拟数据
                //for (int i = 0; i < this.TestRecipe.Timeinterval_s; i++)
                //{
                //    RawData.Add(new RawDatumItem_TH()
                //    {
                //        Resistance_R = 1.5,
                //        SamplingTime_s = i,
                //    });
                //}
                //RawData.Timeinterval_s = this.TestRecipe.Timeinterval_s;
                //return;

                if (TH2810D_Master == null )
                {
                    return;
                }
                this.Log_Global($"开始测试!");

                if (TH2810D_Master.IsOnline)
                {
                    TH2810D_Master.TRIGger = this.TestRecipe.TRIGger;
                    Thread.Sleep(500);
                    TH2810D_Master.DISPlay = this.TestRecipe.DISPlay;
                    Thread.Sleep(500);
                    TH2810D_Master.SPEED = this.TestRecipe.SPEED;
                    Thread.Sleep(500);
                    TH2810D_Master.SRESistor = this.TestRecipe.SRESistor;
                    Thread.Sleep(500);
                    TH2810D_Master.FREQuency = this.TestRecipe.FREQuency;
                    Thread.Sleep(500);
                    TH2810D_Master.PARAmeter = this.TestRecipe.PARAmeter;
                    Thread.Sleep(500);
                    TH2810D_Master.LEVel = this.TestRecipe.LEVel;
                    Thread.Sleep(500);
                    TH2810D_Master.RANGe = this.TestRecipe.RANGe;
                    Thread.Sleep(500);
                    
                }

                ArrayList resistance = new ArrayList();
                ArrayList time = new ArrayList();
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (stopwatch.ElapsedMilliseconds < (this.TestRecipe.Timeinterval_s * 1000))
                {
                    Thread.Sleep(300);
                    var realityresistance = TH2810D_Master.GetFETCh();
                    var times = stopwatch.ElapsedMilliseconds / 1000.0;
                    var resistancearray = realityresistance.Split(',');
                    var resistancerequirement = ChangeDataToD(resistancearray[0]);
                    resistance.Add(resistancerequirement);
                    time.Add(times);

                }
                stopwatch.Reset();
                resistance.RemoveAt(0);
                time.RemoveAt(0);
                for (int i = 0; i < resistance.Count; i++)
                {
                    RawData.Add(new RawDatumItem_ACR()
                    {
                        Resistance_R = Convert.ToDouble( resistance[i]),
                        SamplingTime_s = (double)time[i],
                    }) ;
                }
                RawData.Timeinterval_s = this.TestRecipe.Timeinterval_s;
            }
            catch (Exception ex)
            {
            }
        }

    }
}
