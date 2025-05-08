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
    /// LIV功率校准算子
    ///  
    /// </summary>
    public class TestCalculator_LIV_PowerCalibration : TestCalculatorBase
    {
        public TestCalculator_LIV_PowerCalibration() : base() { }

        public CalcRecipe_LIV_PowerCalibration CalcRecipe { get; private set; }

        public override Type GetCalcRecipeType()
        {
            return typeof(CalcRecipe_LIV_PowerCalibration);
        }

        public override void Localization(ICalcRecipe testRecipe)
        {
            CalcRecipe = (CalcRecipe_LIV_PowerCalibration)testRecipe;
        }
        public override void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
   
            try
            {
                const string PowerTag = "Power_mW";

                var localRawData = rawData as IRawDataCollectionBase;
                var orgVals = localRawData.GetDataListByPropName(PowerTag);
                var calcVals = new List<object>();

                foreach (var oVal in orgVals)
                {
                    calcVals.Add(oVal * this.CalcRecipe.Coeff_K + this.CalcRecipe.Coeff_B);
                }
                var updateSucceeded = localRawData.SetDataValueListByPropName(PowerTag, calcVals);

            }
            catch (Exception ex)
            {
                this._core.ReportException($"{this.Name} - {ex.Message}", ErrorCodes.Calc_LIV_PowerCalibration_ParamError, ex);
            }
        }
    }
}