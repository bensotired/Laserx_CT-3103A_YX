using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_IO;
using SolveWare_Motion;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Thorlabs.TLPM_32.Interop;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_Optica")]

    //[ConfigurableInstrument("TLB_6700Controllers", "TLB_6700", "用于驱动器件1")]
    [ConfigurableInstrument("TMPL_Controllers", "TMPL_Master", "用于驱动功率计")]


    [StaticResource(ResourceItemType.AXIS, "MTR_载物台_旋转", "载物台_旋转")]
    [StaticResource(ResourceItemType.IO, "Output_Middle_ShadCylinder", "遮光气缸")]
    public class TestModule_Optica : TestModuleBase
    {
        //TLB_6700Controllers TLB_6700 { get { return (TLB_6700Controllers)this.ModuleResource["TLB_6700"]; } }
        TMPL_Controllers TMPL_Master { get { return (TMPL_Controllers)this.ModuleResource["TMPL_Master"]; } }

        MotorAxisBase Axis_Taiwan_R { get { return (MotorAxisBase)this.ModuleResource["MTR_载物台_旋转"]; } }
        IOBase ShadCylinder { get { return (IOBase)this.ModuleResource["Output_Middle_ShadCylinder"]; } }

        TLPM tlpm = null;
        public TestModule_Optica() : base() { }
        public override Type GetTestRecipeType() { return typeof(TestRecipe_Optica); }
        TestRecipe_Optica TestRecipe { get; set; }
        RawData_Optical RawData { get; set; }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_Optical(); return RawData;
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_Optica>(testRecipe);
        }

        public override void Run(CancellationToken token)
        {
            try
            {
                string configFileFullPath = System.IO.Path.Combine(Application.StartupPath, "ConfigFiles", "AngleCompensation.xml");

                Compensation compensation = XmlHelper.DeserializeFile<Compensation>(configFileFullPath);
                if (compensation==null)
                {
                    var msg = "载入角度补偿参数失败！";
                    MessageBox.Show(msg);
                    throw new Exception(msg);
                }
                var AngleCompen = compensation.datas;


                this.ShadCylinder.TurnOn(false);

                //TLB_6700.SYSTem_MCONtrol = "REM";

                if (token.IsCancellationRequested)
                {
                    this.ShadCylinder.TurnOn(true);
                    //TLB_6700.OUTPut_STATe = "OFF";
                    TMPL_Master.Dispose();
                    token.ThrowIfCancellationRequested();
                }
                if (tlpm == null && TMPL_Master.InitializePM(ref tlpm) == false)
                {
                    var msg = "未发现 Thorlabs 功率计探头";
                    MessageBox.Show(msg);
                    throw new Exception(msg);
                }

                tlpm.setWavelength(TestRecipe.TMPL_Wavelength);
                tlpm.setPowerAutoRange(true);


                List<double> Angle = new List<double>();
                List<double> Power = new List<double>();

                for (double i = TestRecipe.StartAngle; i <= TestRecipe.StopAngle; i += TestRecipe.StepAngle)
                {
                    if (token.IsCancellationRequested)
                    {
                        this.ShadCylinder.TurnOn(true);
                        //TLB_6700.OUTPut_STATe = "OFF";
                        TMPL_Master.Dispose();
                        token.ThrowIfCancellationRequested();
                    }

                    this.Axis_Taiwan_R.MoveToV3(i, SolveWare_Motion.SpeedType.Auto, SpeedLevel.Normal);
                    double CalibPower = 0;
                    tlpm.measPower(out CalibPower);
                    CalibPower *= 1000; //W->mW
                    Angle.Add(i);
                    Power.Add(CalibPower);
                    
                }


                for (int i = 0; i < Angle.Count; i++)
                {
                    RawData.Add(new RawDatumItem_Optical()
                    {        
                        Angle = Angle[i],
                        Power_mW = Power[i],
                    });
                }



                this.ShadCylinder.TurnOn(true);
                //TLB_6700.OUTPut_STATe = "OFF";
                TMPL_Master.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        public class Compensation
        {
            public Compensation()
            {

            }
            public DataBook<double, double> datas { get; set; }
        }
    }
}
