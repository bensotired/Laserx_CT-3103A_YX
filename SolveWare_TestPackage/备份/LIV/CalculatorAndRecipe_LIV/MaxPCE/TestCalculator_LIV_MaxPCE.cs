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
    /// 最大PCE(%)
    /// </summary>
    public class TestCalculator_LIV_MaxPCE : TestCalculatorBase
    {
        public TestCalculator_LIV_MaxPCE() : base() { }

        public CalcRecipe_LIV_MaxPCE CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_MaxPCE);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_MaxPCE)testRecipe;
        }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            
            try
            {                
                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();

                if (CalcRecipe.IsForceRename == true)
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}{CalcRecipe.CalcData_Rename}{CalcRecipe.CalcData_PostFix}");
                }
                else
                {
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}MaxPCE{CalcRecipe.CalcData_PostFix}");
                }
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string PCE = "PCE"; 

                var localRawData = rawData as IRawDataCollectionBase;
                List<double> PCEList = localRawData.GetDataListByPropName(PCE);
                List<double> PCEList_New = new List<double> (); 

                if (PCEList.Count <= 0 )
                {
                    throw new Exception($"Pout: xArray and yArray are of unequal size!");
                }

                //dataSummary_1.Value = PCEList.Max();
                for (int i = 1; i < PCEList.Count; i++)  //去掉第一个数值
                {
                    PCEList_New.Add(PCEList[i]);
                }
                dataSummary_1.Value = PCEList_New.Max();
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_MaxPCE_ParamError, ex);
            }
        }
    }
}