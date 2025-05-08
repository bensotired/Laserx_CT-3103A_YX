using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SolveWare_BurnInInstruments;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestCalculator_Spect_SMSR : TestCalculatorBase
    {
        public TestCalculator_Spect_SMSR() : base() { }

        public CalcRecipe_Spect_SMSR CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_Spect_SMSR);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_Spect_SMSR)testRecipe;
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}SMSR{CalcRecipe.CalcData_PostFix}");
                }
          
                dataSummary_1.Value = double.NaN;

                //const string WavelengthTag = "Wavelength_nm";
                //const string PowerTag = "Power";

                //var localRawData = rawData as IRawDataCollectionBase;
                //var dict = localRawData.GetDataDictByPropNames<double>(WavelengthTag, PowerTag);

                //if (dict[WavelengthTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[WavelengthTag]?.Count != dict[PowerTag]?.Count)
                //{
                //    throw new Exception($" xArray and yArray are of unequal size!");
                //}

                //int wavelengthOffsetFromCenter1 = this.CalcRecipe.SearchMin_nm;
                //int wavelengthOffsetFromCenter2 = this.CalcRecipe.SearchMax_nm;

                //double SMSR = CalSMSR(dict[WavelengthTag], dict[PowerTag], wavelengthOffsetFromCenter1, wavelengthOffsetFromCenter2); ;
                //dataSummary_1.Value = SMSR;
                //直接从osa里面读
                dataSummary_1.Value = (rawData as RawData_SPECT).Smsr_dB;
                summaryDataWithoutSpec.Add(dataSummary_1);

            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_SPECT_SMSR_ParamError, ex);
            }
        }

        private double CalSMSR(List<double> wavelengths, List<double> powers,int SearchMin_nm,int SearchMax_nm)
        {
            if (wavelengths.Count < 10)
            {
                return 0;
            }

            //1. find center wl
            double peakPower = double.MinValue;
            int peakIndex = 0;
            for (int i = 2; i < wavelengths.Count - 2; i++)
            {
                if (powers[i] > peakPower)
                {
                    peakPower = powers[i];
                    peakIndex = i;
                }
            }

            double MaxPower = peakPower;
            double CenterWavelength = wavelengths[peakIndex];


            int rightPeakIndex = 0;
            int index = peakIndex;
            double rightPeakPower = double.MinValue;
            //2. right SMSR
            while (true)
            {
                if (wavelengths[index] > CenterWavelength + SearchMax_nm)// 3                 center   1  3
                {
                    break;
                }

                if (wavelengths[index] >= CenterWavelength + SearchMin_nm)//1    右边的波峰
                {
                    if (rightPeakPower < powers[index])
                    {
                        rightPeakPower = powers[index];
                        rightPeakIndex = index;
                    }
                }

                index++;

                if (index > wavelengths.Count - 1)
                {
                    break;
                }
            }

            if (rightPeakPower == double.MinValue)
            {
                return -999;
            }
         
            //3. left SMSR
            index = peakIndex;
            int leftPeakIndex = 0;
            double leftPeakPower = double.MinValue;
            while (true)
            {
                if (wavelengths[index] < CenterWavelength - SearchMax_nm)
                {
                    break;
                }

                if (wavelengths[index] <= CenterWavelength - SearchMin_nm)
                {
                    if (leftPeakPower < powers[index])
                    {
                        leftPeakPower = powers[index];
                        leftPeakIndex = index;
                    }
                }

                index--;

                if (index < 0)
                {
                    break;
                }
            }

            if (leftPeakPower == double.MinValue)
            {
                return -888;
            }

            double minpeakPower = Math.Max(leftPeakPower, rightPeakPower);
            double smsr = 10 * Math.Log10(peakPower / minpeakPower);
            return smsr;


        }

    }
}