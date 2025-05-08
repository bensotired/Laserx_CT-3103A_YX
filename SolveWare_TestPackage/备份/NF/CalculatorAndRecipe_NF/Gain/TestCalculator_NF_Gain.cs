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
    public class TestCalculator_NF_Gain : TestCalculatorBase
    {
        public TestCalculator_NF_Gain() : base() { }

        public CalcRecipe_NF_Gain CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_NF_Gain);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_NF_Gain)testRecipe;
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}NF_Gain{CalcRecipe.CalcData_PostFix}");
                }

                dataSummary_1.Value = double.NaN;
                //dataSummary_1.Value = (rawData as RawData_NF).Gain;
                summaryDataWithoutSpec.Add(dataSummary_1);

                const string Wavelength_nmTag = "Wavelength_nm";
                const string Gain_dBTag = "Gain_dB";

                var localRawData = rawData as IRawDataCollectionBase;
                var dict = localRawData.GetDataDictByPropNames<double>(Wavelength_nmTag, Gain_dBTag);

                if (dict[Wavelength_nmTag]?.Count <= 0 || dict[Gain_dBTag]?.Count <= 0 | dict[Wavelength_nmTag]?.Count != dict[Gain_dBTag]?.Count)
                {
                    throw new Exception($"Iop: xArray and yArray are of unequal size!");
                }
                if (this.CalcRecipe.USE_Specified_Wavelength_nm)
                {
                    double Wavelength_nm = this.CalcRecipe.Specified_Wavelength_nm;
                    double resultCurrent = GetCurrentForSpecifiedPower(dict[Wavelength_nmTag], dict[Gain_dBTag], Wavelength_nm);
                    // double resultCurrent = Interpolator.DoPiecewiseLinearInterpolation(Wavelength_nm, dict[Wavelength_nmTag].ToArray(), dict[Gain_dBTag].ToArray());
                    dataSummary_1.Value = resultCurrent;
                }
                else
                {
                    double Wavelength_nm = (rawData as RawData_NF).TraceA_CenterWavelength_nm;
                    //double resultCurrent = GetCurrentForSpecifiedPower(dict[Gain_dBTag], dict[Wavelength_nmTag], Wavelength_nm);
                    double resultCurrent = Interpolator.DoPiecewiseLinearInterpolation(Wavelength_nm, dict[Wavelength_nmTag].ToArray(), dict[Gain_dBTag].ToArray());
                   // double resultCurrent = GetNFForSpecifiedWavelength(dict[Wavelength_nmTag], dict[NF_dBTag], Wavelength_nm);
                    dataSummary_1.Value = resultCurrent;
                }
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_NF_Gain_ParamError, ex);
            }
        }
        public double GetCurrentForSpecifiedPower(List<double> currents, List<double> powers, double power_mW)
        {
            try
            {
                int indexS;
                int indexE;

                //找到比需求功率大的功率
                for (indexE = 0; indexE < currents.Count; indexE++)
                {
                    if (powers[indexE] == 0)
                    {
                        break;
                    }
                    if (powers[indexE] >= power_mW)
                    {
                        break;
                    }
                }
                if (indexE == currents.Count) return 0;

                //找到比需求功率小的功率
                for (indexS = indexE; indexS > 0; indexS--)
                {
                    if (powers[indexE] == 0)
                    {
                        break;
                    }
                    if (powers[indexS] < power_mW)
                    {
                        break;
                    }
                }
                if (indexS == 0) return 0;


                //将该区间的数据进行直线拟合
                double[] powerArr = new double[indexE - indexS + 1];  //功率组成的数组
                double[] currentArr = new double[indexE - indexS + 1]; //电流点组成的数组

                //把数据装到数组中
                for (int i = indexS; i <= indexE; i++)
                {
                    currentArr[i - indexS] = currents[i];
                    powerArr[i - indexS] = powers[i];

                }

                //拟合成函数, 设定为1阶拟合 
                //X:功率
                //Y:电流                       
                PolyFitData polyData = PolyFitMath.PolynomialFit(powerArr, currentArr, 1);

                //计算出电流
                double CalcCurrent = polyData.Coeffs[1] * power_mW + polyData.Coeffs[0];
                return CalcCurrent;

            }
            catch (Exception ex)
            {
                throw new Exception("GetCurrentForSpecifiedPower:", ex);
            }

        }
        public double GetGainForSpecifiedWavelength(List<double> wavelengthlist, List<double> gainlist, double wavelength_nm) 
        {
             double gain = 0;
            try
            {
                if (wavelengthlist.Count==gainlist.Count)
                {
                    for (int i = 0; i < wavelengthlist.Count; i++)
                    {
                        if (wavelengthlist[i]== wavelength_nm)
                        {
                            gain = gainlist[i];
                        }
                    }
                }
                return gain;
            }
            catch (Exception)
            {
                return gain;
                // throw;
            }
        }
    }
}