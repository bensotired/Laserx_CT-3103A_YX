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
    /// Wavelength手动计算
    /// 找出目标功率点下对应的驱动电流值
    /// </summary>
    public class TestCalculator_Spect_PeakWavelength : TestCalculatorBase
    {
        public TestCalculator_Spect_PeakWavelength() : base() { }

        public CalcRecipe_Spect_PeakWavelength CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_Spect_PeakWavelength);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_Spect_PeakWavelength)testRecipe;
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
                    dataSummary_1.Name = string.Format(rawData.RawDataFixFormat, $"{CalcRecipe.CalcData_PreFix}PeakWavelength{CalcRecipe.CalcData_PostFix}");
                }

     
                dataSummary_1.Value = double.NaN;
                
                
                //const string WavelengthTag = "Wavelength_nm";
                //const string PowerTag = "Power";

                //var localRawData = rawData as IRawDataCollectionBase;
                //var dict = localRawData.GetDataDictByPropNames<double>(WavelengthTag, PowerTag);

                //if (dict[WavelengthTag]?.Count <= 0 || dict[PowerTag]?.Count <= 0 | dict[WavelengthTag]?.Count != dict[PowerTag]?.Count)
                //{
                //    throw new Exception($"PeakWavelength: xArray and yArray are of unequal size!");
                //}

                //double maxPow_dbm = double.NegativeInfinity;//负无穷
                //double cenWl_nm = double.NaN;

                //for (int i = 0; i < dict[PowerTag]?.Count; i++)
                //{
                //    if (dict[PowerTag][i] > maxPow_dbm)//对比的是功率
                //    {
                //        maxPow_dbm = dict[PowerTag][i];
                //        cenWl_nm = dict[WavelengthTag][i];
                //    }
                //}

                ////中心波长
                //dataSummary_1.Value = cenWl_nm;

                //直接从OSA里面获取
                dataSummary_1.Value = (rawData as RawData_SPECT).PeakWavelength_nm;
                summaryDataWithoutSpec.Add(dataSummary_1);
            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_SPECT_PeakWavelength_mA_ParamError, ex); 
            }
        }
    }
}