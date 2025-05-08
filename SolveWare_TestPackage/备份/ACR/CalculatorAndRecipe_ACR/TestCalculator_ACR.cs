using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace SolveWare_TestPackage
{
    /// <summary>
    /// 电阻计算
    /// </summary>
    public class TestCalculator_ACR : TestCalculatorBase
    {
        public TestCalculator_ACR() : base() { }

        public CalcRecipe_ACR CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_ACR);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_ACR)testRecipe;
        }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            try
            {


                SummaryDatumItemBase dataSummary_1 = new SummaryDatumItemBase();
                dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Resistance_{CalcRecipe.CalcData_PostFix}"); 
                dataSummary_1.Value = double.NaN;

                const string Resistance = "Resistance_R";
                const string SamplingTime = "SamplingTime_s";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(Resistance, SamplingTime);
 
                if (dict[Resistance]?.Count <= 0 | dict[Resistance]?.Count != dict[SamplingTime]?.Count)
                {
                    throw new Exception($" xArray and yArray are of unequal size!");
                }

                double slopemin_R= double.NaN;
                double[] resistance = new double[dict[Resistance].Count];
                double[] stime = new double[dict[Resistance].Count];
                for (int i = 0; i < dict[Resistance]?.Count; i++)
                {
                    resistance[i] = dict[Resistance][i];
                    if (resistance[i] < this.CalcRecipe.Resistance_LowerLimit || resistance[i] > this.CalcRecipe.Resistance_UpperLimit)
                    {
                        dataSummary_1.Value = -1;
                        summaryDataWithoutSpec.Add(dataSummary_1);
                        return;
                    }
                    stime[i] = dict[SamplingTime][i];
                }

                double[] firstDerivateArray = ArrayMath.CalculateFirstDerivate(resistance, stime);
                for (int i = 0; i < firstDerivateArray.Length; i++)
                {
                    firstDerivateArray[i] = Math.Abs(firstDerivateArray[i]);
                }
                double[] secondDerivateArray = ArrayMath.CalculateFirstDerivate(resistance, firstDerivateArray);
                for (int i = 0; i < secondDerivateArray.Length; i++)
                {
                    secondDerivateArray[i] = Math.Abs(secondDerivateArray[i]);
                }
                var minindex = GetMinAndIndex(secondDerivateArray);

                slopemin_R = resistance[minindex];

                //int maxIndex, minIndex;
                //ArrayMath.GetMaxMinIndexOf2ndDerivative(res, stime,out maxIndex,out minIndex);

                dataSummary_1.Value = slopemin_R;
                summaryDataWithoutSpec.Add(dataSummary_1);
                if (this.CalcRecipe.IsPass)
                {
                    SummaryDatumItemBase dataSummary_2 = new SummaryDatumItemBase();
                    dataSummary_2.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}Resistance_" +
                        $"Expected_{CalcRecipe.CalcData_PostFix}"); 
                    var idealResistance = this.CalcRecipe.StandardResistance_R + ((this.CalcRecipe.SamplingTemperature_C - this.CalcRecipe.Temperature_C) * this.CalcRecipe.ResistanceCompensation_R);
                    dataSummary_2.Value = idealResistance;
                    summaryDataWithoutSpec.Add(dataSummary_2);
                }
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_Iop_ParamError, ex);
            }
        }
        private int GetMinAndIndex(params double[] pa)
        {
            int index = -1;//定义变量存最小值的索引
            if (pa.Length != 0)
            {
                double Min = pa[0];
                index = 0;
                for (int i = 0; i < pa.Length; i++)
                {
                    if (Min > pa[i])
                    {
                        index = i;
                        Min = pa[i];
                    }
                }
            }
            return index;
        }
    }
}