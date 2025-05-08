using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using LX_Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;

namespace SolveWare_TestPackage
{
    public class TestCalculator_FF_Centroid : TestCalculatorBase
    {
        public TestCalculator_FF_Centroid() : base()
        {
        }

        public CalcRecipe_FF_Centroid CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_FF_Centroid);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_FF_Centroid)(testRecipe);
        }

        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {
                SummaryDatumItemBase dataSummary = new SummaryDatumItemBase();
                dataSummary.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Centroid{CalcRecipe.CalcData_PostFix}");
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

                List<double> findCentroid = new List<double>();
                for (int i = 0; i < angle.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        findCentroid.Add((angle[i + 1] - angle[i]) * power[i]);
                    }
                    else
                    {
                        findCentroid.Add(findCentroid[i - 1] + (angle[i + 1] - angle[i]) * power[i]);
                    }
                }

                var up = angle.Last();
                var down = angle.First();
                var halfSquare = findCentroid.Last() / 2;

                var centroidIndex = findCentroid.IndexOf(findCentroid.Aggregate((x, y) => Math.Abs(x - halfSquare) < Math.Abs(y - halfSquare) ? x : y));

                dataSummary.Value = angle[centroidIndex];
                summaryDataWithoutSpec.Add(dataSummary);
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_FF_Centroid_ParamError, ex);
            }
        }
    }
}