using System;
using System.Collections.Generic;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;

namespace SolveWare_TestPackage
{
    [Serializable]
    /// <summary>
    /// Pout算子
    /// 找出目标功率点下对应的驱动电流值
    /// </summary>
    public class TestCalculator_LIV_Pout : TestCalculatorBase
    {
        public TestCalculator_LIV_Pout() : base() { }

        public CalcRecipe_LIV_Pout CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Pout);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Pout)testRecipe;
        }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            //注意 Pout 是定电流测功率  已检验
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Pout{CalcRecipe.CalcData_PostFix}");
                }
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string PowerTag = "Power_mW";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, PowerTag);
 
                if (dict[CurrentTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[CurrentTag]?.Count != dict[PowerTag]?.Count)
                {
                    throw new Exception($"Pout: xArray and yArray are of unequal size!");
                }

                //根据1100A的接近原则
                double current_mA = CalcRecipe.Current_mA;//传入的电流值
                double minOffset_mA = double.MaxValue;
                double power_mW = 0.0;

                for (int i = 0; i < dict[CurrentTag]?.Count; i++)
                {
                    double offset_mA = Math.Abs(dict[CurrentTag][i]- current_mA);
                    if (offset_mA < minOffset_mA)
                    {
                        minOffset_mA = offset_mA;
                        power_mW = dict[PowerTag][i];
                    }
                }
                
                //两电流点之间至少应该相差5mA 否则无效
                if (minOffset_mA < 5)
                {
                    dataSummary_1.Value = power_mW;
                }
                else
                {
                    dataSummary_1.Value = -1;
                }
          
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Pout_ParamError, ex);
            }
        }
    }
}