using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SolveWare_TestPackage
{
    [Serializable]
    public class TestCalculator_Spect_FWHM : TestCalculatorBase
    {
        public TestCalculator_Spect_FWHM() : base() { }

        public CalcRecipe_Spect_FWHM CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_Spect_FWHM);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_Spect_FWHM)testRecipe;
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}FWHM{CalcRecipe.CalcData_PostFix}");
                }
      
                dataSummary_1.Value = double.NaN;

                const string WavelengthTag = "Wavelength_nm";
                const string PowerTag = "Power";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(WavelengthTag, PowerTag);
 
                if (dict[WavelengthTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[WavelengthTag]?.Count != dict[PowerTag]?.Count)
                {
                    throw new Exception($"FWHM: xArray and yArray are of unequal size!");
                }
                double level = this.CalcRecipe.level;
                double FWHM = CalculateHalfMaxPowerWidth(dict[WavelengthTag], dict[PowerTag], level);
                dataSummary_1.Value = FWHM;
                summaryDataWithoutSpec.Add(dataSummary_1);

            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_SPECT_FWHM_ParamError, ex);
            }
        }

        private double CalculateHalfMaxPowerWidth(List<double> wavelengths_nm ,List<double> powers_mw,double level)
        {
            if (wavelengths_nm.Count != powers_mw.Count) return 0;
            int maxIndex;
            int minIndex;
            ArrayMath.GetMaxAndMinIndex(powers_mw.ToArray(), out maxIndex, out minIndex);
            List<double> wavelengthUp = new List<double>();
            List<double> wavelengthDown = new List<double>();
            List<double> powerUp = new List<double>();
            List<double> powerDown = new List<double>();
            for (int i = 0; i < maxIndex; i++)
            {
                wavelengthUp.Add(wavelengths_nm[i]);
                powerUp.Add(powers_mw[i]);
            }
            for (int i = maxIndex; i < wavelengths_nm.Count; i++)
            {
                wavelengthDown.Add(wavelengths_nm[i]);
                powerDown.Add(powers_mw[i]);
            }
            double halfMaxPower = powers_mw.Max() * level;
            double left_wavelength;
            double right_wavelength;
            if (halfMaxPower < powerUp.Min()) left_wavelength = wavelengthUp.First();
            else if (powerUp.Max() < halfMaxPower) left_wavelength = wavelengthUp.Last();
            else
            {
                left_wavelength = Interpolator.DoPiecewiseLinearInterpolation(halfMaxPower, powerUp.ToArray(), wavelengthUp.ToArray());

            }

            if (halfMaxPower < powerDown.Min()) right_wavelength = wavelengthDown.Last();
            else if (powerDown.Max() < halfMaxPower) right_wavelength = wavelengthDown.First();
            else
            {
                right_wavelength = Interpolator.DoPiecewiseLinearInterpolation(halfMaxPower, powerDown.ToArray(), wavelengthDown.ToArray());
            }

            var ret = Math.Abs(right_wavelength - left_wavelength);
            return ret;
        }
    }
}