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
    public class TestCalculator_FF_PeakAngle : TestCalculatorBase
    {
        public TestCalculator_FF_PeakAngle() : base()
        {
        }

        public CalcRecipe_FF_PeakAngle CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_FF_PeakAngle);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_FF_PeakAngle)(testRecipe);
        }

        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                SummaryDatumItemBase dataSummary = new SummaryDatumItemBase();
                dataSummary.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}MaxPoint{CalcRecipe.CalcData_PostFix}");
                dataSummary.Value = double.NaN;

                const string ThetaTag = "Theta";
                const string PowerTag = "PDCurrent";

                var raw = rawData as IRawDataCollectionBase;
                var dict = raw.GetDataDictByPropNames<double>(ThetaTag, PowerTag);

                if (dict[ThetaTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[ThetaTag]?.Count != dict[PowerTag]?.Count)
                {
                    throw new Exception($" xArray and yArray are of unequal size!");
                }
                List<double> angle = new List<double>();
                List<double> power = new List<double>();
                for (int i = 0; i < dict[ThetaTag].Count; i++)
                {
                    angle.Add(dict[ThetaTag][i]);
                    power.Add(dict[PowerTag][i]);
                }

                int maxIndex, minIndex;
                List<double> newP = new List<double>();

                ArrayMath.GetMaxAndMinIndex(power.ToArray(), out maxIndex, out minIndex);
                double maxPower = power[maxIndex];
                double minPower = power[minIndex];

                foreach (double p in power)
                {
                    newP.Add((p));
                }

                dataSummary.Value = angle[maxIndex];
                summaryDataWithoutSpec.Add(dataSummary);
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_FF_MaxPoint_ParamError, ex);
            }
        }
    }
}