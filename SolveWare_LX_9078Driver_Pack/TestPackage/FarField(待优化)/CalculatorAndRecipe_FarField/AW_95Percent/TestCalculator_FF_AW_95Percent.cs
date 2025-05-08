using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;

namespace SolveWare_TestPackage
{
    public class TestCalculator_FF_AW_95Percent : TestCalculatorBase
    {
        public TestCalculator_FF_AW_95Percent() : base()
        {
        }

        public CalcRecipe_FF_AW_95Percent CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_FF_AW_95Percent);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_FF_AW_95Percent)(testRecipe);
        }

        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                SummaryDatumItemBase dataSummary = new SummaryDatumItemBase();
                dataSummary.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}AW_95Percent{CalcRecipe.CalcData_PostFix}");
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

                var ret = this.CalculateAngleWidth(angle, newP, 0.05 * (maxPower-minPower)+minPower);
                dataSummary.Value = ret;
                summaryDataWithoutSpec.Add(dataSummary);
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_FF_AW_95Percent_ParamError, ex);
            }
        }

        private double CalculateAngleWidth(List<double> angle, List<double> power, double level)
        {
            if (angle.Count != power.Count) return 0;
            int maxIndex;
            int minIndex;
            ArrayMath.GetMaxAndMinIndex(power.ToArray(), out maxIndex, out minIndex);
            List<double> angleUp = new List<double>();
            List<double> angleDown = new List<double>();
            List<double> powerUp = new List<double>();
            List<double> powerDown = new List<double>();
            for (int i = 0; i < maxIndex; i++)
            {
                angleUp.Add(angle[i]);
                powerUp.Add(power[i]);
            }
            for (int i = maxIndex; i < angle.Count; i++)
            {
                angleDown.Add(angle[i]);
                powerDown.Add(power[i]);
            }

            double x1;
            double x2;
            if (level < powerUp.Min()) x1 = angleUp.First();
            else if (powerUp.Max() < level) x1 = angleUp.Last();
            else
            {
                x1 = Interpolator.DoPiecewiseLinearInterpolation(level, powerUp.ToArray(), angleUp.ToArray());

            }

            if (level < powerDown.Min()) x2 = angleDown.Last();
            else if (powerDown.Max() < level) x2 = angleDown.First();
            else
            {
                x2 = Interpolator.DoPiecewiseLinearInterpolation(level, powerDown.ToArray(), angleDown.ToArray());
            }
            var ret = Math.Abs(x2 - x1);
            return ret;
        }
    }
}