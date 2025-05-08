using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
    (
        "TestCalculator_Spect_PeakWavelength",
        "TestCalculator_Spect_CenterWavelength",
        "TestCalculator_Spect_FWHM",
        "TestCalculator_Spect_SMSR",
        "TestCalculator_Spect_SMSR_FilterBaseNoise"
    )]
 
    public class TestModule_PC4000_Rollback : TestModuleBase
    {
        public TestModule_PC4000_Rollback() : base() { }

  
        TestRecipe_PC4000 TestRecipe { get; set; }
        RawData_SPECT_PC4000 RawData { get; set; }

      
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_PC4000);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_SPECT_PC4000();

            return RawData;
        }

    
        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_PC4000>(testRecipe);
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Restart();

                DialogResult dr = DialogResult.Cancel;
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.InitialDirectory = @"D:\laserx document\西安项目\格恩\格恩数据汇总";
                ofd.Multiselect = false;
                List<string> lines = new List<string>();

                this._core.GUIRunUIInvokeActionSYNC(() =>
                {
                    dr = ofd.ShowDialog();

                });

                if (dr == DialogResult.OK)
                {
                    if (File.Exists(ofd.FileName))
                    {
                        using (StreamReader sr = new StreamReader(ofd.FileName))
                        {
                            sr.ReadLine();
                            do
                            {
                                lines.Add(sr.ReadLine());
                            }
                            while (sr.EndOfStream == false);
                        }
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    return;
                }

            

                foreach (var line in lines)
                {
                    var lineArr = line.Split(',');
            
                    RawData.Add(new RawDatumItem_SPECT_PC4000()
                    {
                        Wavelength_nm = Convert.ToDouble(lineArr[0]),
                
                        Power =   Convert.ToDouble(lineArr[1]),
                    });
                }
                return;
            }
            catch (Exception ex)
            {
            }
        }
        public void outputOn()
        {

        }

        public void outputOff()
        {

        }

    }
}
