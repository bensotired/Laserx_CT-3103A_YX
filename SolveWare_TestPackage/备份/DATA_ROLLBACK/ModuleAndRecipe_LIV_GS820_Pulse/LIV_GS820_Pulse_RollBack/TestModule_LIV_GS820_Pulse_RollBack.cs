using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TestPackage
{
    [SupportedCalculator
     (
         "TestCalculator_LIV_VF",
         "TestCalculator_LIV_Iop",
         "TestCalculator_LIV_Vop",
         "TestCalculator_LIV_Ith1",
         "TestCalculator_LIV_Ith2",
         "TestCalculator_LIV_Ith3",
         "TestCalculator_LIV_Kink_Current",
         "TestCalculator_LIV_Kink_Percentage",
         "TestCalculator_LIV_Kink_Power",
         "TestCalculator_LIV_Pout",
         "TestCalculator_LIV_Rs",
         "TestCalculator_LIV_Rs_2Point",
         "TestCalculator_LIV_SE_mWpermA",
         "TestCalculator_LIV_SE_mWpermW"
     )]
 
    public class TestModule_LIV_GS820_Pulse_RollBack : TestModuleBase
    {

        public TestModule_LIV_GS820_Pulse_RollBack() : base()
        {
        }



        TestRecipe_LIV_GS820_Pulse_RollBack TestRecipe { get; set; }
        RawData_LIV_RollBack RawData { get; set; }
        public override Type GetTestRecipeType()
        {
            return typeof(TestRecipe_LIV_GS820_Pulse_RollBack);
        }
        public override IRawDataBaseLite CreateRawData()
        {
            RawData = new RawData_LIV_RollBack();
            return RawData;
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            TestRecipe = ConvertObjectTo<TestRecipe_LIV_GS820_Pulse_RollBack>(testRecipe);
        }
        public override void Run(CancellationToken token)
        {
            try
            {
                //根据1100A
                this.Log_Global($"开始测试!");
                //这里还没有实例化  需要制作方法得到smu的实例
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
                    RawData.Add(new RawDatumItem_LIV_RollBack()
                    {
                        Current_mA = Convert.ToDouble(lineArr[0]) ,
                        Voltage_V = Convert.ToDouble(lineArr[1]),
                        PDCurrent_mA = Convert.ToDouble(lineArr[2])  ,
                        //Power_mW = (Convert.ToDouble(lineArr[2]) * 33881.89739)
                        Power_mW = Convert.ToDouble(lineArr[3])
                    });
                }
                return;
            }
            catch (Exception ex)
            {
            }
        }


    }
}