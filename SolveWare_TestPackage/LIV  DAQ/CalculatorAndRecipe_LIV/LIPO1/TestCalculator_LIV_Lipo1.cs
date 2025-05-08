using System;
using System.Collections.Generic;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System.Linq;

namespace SolveWare_TestPackage
{
    [Serializable]
    /// <summary>
    /// 
    /// 结束电流
    /// </summary>
    public class TestCalculator_LIV_Lipo1 : TestCalculatorBase
    {
        public TestCalculator_LIV_Lipo1() : base() { }

        public CalcRecipe_LIV_Lipo1 CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Lipo1);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Lipo1)testRecipe;
        }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                //x array current
                //y array power
                //z array voltage
                //拟合
                //某个电流点对应的拟合后的功率值

                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                if (CalcRecipe.IsForceRename == true)
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                }
                else
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Lipo1{CalcRecipe.CalcData_PostFix}");
                }
      
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);

               

                //大的功率-小得功率
                double powerBigger = 0;
                double powerSmaller = 0;
                if (this.CalcRecipe.PowerStart <= this.CalcRecipe.PowerEnd)
                {
                    powerSmaller = this.CalcRecipe.PowerStart;
                    powerBigger = this.CalcRecipe.PowerEnd;
                }
                else
                {
                    powerSmaller = this.CalcRecipe.PowerEnd;
                    powerBigger = this.CalcRecipe.PowerStart;
                }
                dataSummary_1.Value =Math.Round( 100.0* (powerBigger- powerSmaller)/ powerSmaller,3);//计算输出需要百分比
     
             
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Lipo1_ParamError, ex);
            }
        }
    }
}