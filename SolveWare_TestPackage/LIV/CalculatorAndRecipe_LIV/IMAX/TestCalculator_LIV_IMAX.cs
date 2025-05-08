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
    public class TestCalculator_LIV_Imax : TestCalculatorBase
    {
        public TestCalculator_LIV_Imax() : base() { }

        public CalcRecipe_LIV_Imax CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Imax);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Imax)testRecipe;
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Imax{CalcRecipe.CalcData_PostFix}");
                }
      
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag);

                if (dict[CurrentTag]?.Count <= 0)
                {
                    throw new Exception($"Imax: xArray and yArray are of unequal size!");
                }
                dataSummary_1.Value = dict[CurrentTag].Max();
     
             
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Imax_ParamError, ex);
            }
        }
    }
}