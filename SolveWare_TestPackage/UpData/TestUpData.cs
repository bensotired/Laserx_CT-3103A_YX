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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace SolveWare_TestPackage
{
    [SupportedCalculator("TestCalculator_FF")]
    public class TestUpData : TestModuleBase
    {
        public TestUpData() : base() { }
        //public override Type GetTestRecipeType() { return typeof(TestRecipe_Optica); }
        //TestRecipe_Optica TestRecipe { get; set; }
        //RawData_Optical RawData { get; set; }
        public override IRawDataBaseLite CreateRawData()
        {
            //RawData = new RawData_Optical(); return RawData;
            return new RawDataBaseLite();
        }

        public override void Localization(ITestRecipe testRecipe)
        {
            //TestRecipe = ConvertObjectTo<TestRecipe_Optica>(testRecipe);
        }

        public override void Run(CancellationToken token)
        {
            try
            {
            }
            catch (Exception ex)
            {
                this.Log_Global($"{ex.Message}-{ex.StackTrace}");
                throw new Exception($"{ex.Message}-{ex.StackTrace}");
            }
        }
    }
}
