using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SolveWare_TestPackage
{
    [Serializable]
    /// <summary>
    /// RS算子 拟合  根据CT1100A
    /// 找出两个功率点之间所有的点的集合  然后进行一阶拟合
    /// </summary>
    public class TestCalculator_LIV_Rs_I : TestCalculatorBase
    {
        public TestCalculator_LIV_Rs_I() : base() { }

        public CalcRecipe_LIV_Rs_I CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_Rs_I);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_Rs_I)(testRecipe);
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Rs_I{CalcRecipe.CalcData_PostFix}");
                }
                dataSummary_1.Value = double.NaN;
                summaryDataWithoutSpec.Add(dataSummary_1);
                const string CurrentTag = "Current_mA";
                const string VoltageTag = "Voltage_V";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(CurrentTag, VoltageTag);

                if (dict[CurrentTag]?.Count <= 0 || dict[VoltageTag]?.Count <= 0 | dict[CurrentTag]?.Count != dict[VoltageTag]?.Count)
                {
                    throw new Exception($"Rs_I: xArray and yArray are of unequal size!");
                }

                //需要将范围内的点全部都拿到集合中  然后进行拟合计算
                List<double> laserCurrentList = new List<double>();
                List<double> laserVoltageList = new List<double>();
                for (int i = 0; i < dict[CurrentTag].Count; i++)
                {
                    if (dict[CurrentTag][i] < this.CalcRecipe.StartCurrent_mA || dict[CurrentTag][i] > this.CalcRecipe.StopCurrent_mA)
                    {
                        continue;
                    }
                    laserCurrentList.Add(dict[CurrentTag][i]);
                    laserVoltageList.Add(dict[VoltageTag][i]);
                }

                if (laserCurrentList.Count > 0)
                {
                    //一阶拟合
                    PolyFitData polyFitData = PolyFitMath.PolynomialFit(laserCurrentList.ToArray(), laserVoltageList.ToArray(), 1);
                    dataSummary_1.Value = polyFitData.Coeffs[1] * 1000.0;
                }
                else
                {
                    dataSummary_1.Value = -1;
                }
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Rs_I_ParamError, ex);
            }
        }
    }
}