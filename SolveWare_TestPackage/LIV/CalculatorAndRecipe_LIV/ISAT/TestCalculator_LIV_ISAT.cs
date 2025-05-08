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
    /// powerMax算子 根据CT1100A/3102A 一样的
    /// 饱和电流 扫描LIV光最强的PMAX对应电流点
    /// </summary>
    public class TestCalculator_LIV_Isat : TestCalculatorBase
    {
        public TestCalculator_LIV_Isat() : base() { }

        public CalcRecipe_LIV_Isat CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Isat);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Isat)testRecipe;
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Isat{CalcRecipe.CalcData_PostFix}");
                }
      
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string VoltageTag = "Voltage_V";
                const string PowerTag = "Power_mW";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, VoltageTag, PowerTag);

                if (dict[CurrentTag]?.Count <= 0 || dict[VoltageTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0
                    |
                    dict[CurrentTag]?.Count != dict[VoltageTag]?.Count || dict[CurrentTag]?.Count != dict[PowerTag]?.Count || dict[PowerTag]?.Count != dict[VoltageTag]?.Count)
                {
                    throw new Exception($"Isat: xArray and yArray are of unequal size!");
                }
                
                double current_mA = 0;
                double vgain_V = 0;
                double power_mW = double.MinValue;

                for (int i = 0; i < dict[PowerTag].Count; i++)
                {
                    if (dict[PowerTag][i] > power_mW)
                    {
                        power_mW = dict[PowerTag][i];
                        vgain_V = dict[VoltageTag][i];
                        current_mA = dict[CurrentTag][i];
                    }
                }

                double Isat_PoutMax_mA = current_mA;
                double Pout_Max_mW = power_mW;
                double Vf_PoutMax_V = vgain_V;
                //找出目标功率点下对应的驱动电流值
                dataSummary_1.Value = Isat_PoutMax_mA;
     
             
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Isat_ParamError, ex);
            }
        }
    }
}