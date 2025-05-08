using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestPackage;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_LIV : RawDataCollectionBase<RawDatumItem_LIV>
    {
        public RawData_LIV()
        {
            LIV_Temperature_degC = 25.0;

        }
        [RawDataBrowsableElement] 
        [RawDataPrintableElement]
        public double LIV_Temperature_degC { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double I_Start_mA { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double I_Stop_mA { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double I_Step_mA { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double PDComplianceCurrent_mA { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double Period_ms { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double pulseWidth_ms { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double SourceDelay_ms { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double SenseDelay_ms { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double NPLC_ms { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public bool PulsedMode { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public int PulseCount { get; set; }

        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double LIV_Factor_K { get; set; }
        [RawDataBrowsableElement]
        [RawDataPrintableElement]
        public double LIV_Factor_B { get; set; } 
        public override bool ReloadFormString(string reloadDataString)
        {
            try
            {
                this.DataCollection.Clear();
                var lines = reloadDataString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                var propArr = lines.First().Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);

                for (int rowIndex = 1; rowIndex < lines.Length; rowIndex++)
                {
                    RawDatumItem_LIV temp = new RawDatumItem_LIV();
                    var rdType = typeof(RawDatumItem_LIV);
                    var arr = lines[rowIndex].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int propIndex = 0; propIndex < propArr.Length; propIndex++)
                    {
                        var currentProp = rdType.GetProperty(propArr[propIndex]);
                        currentProp.SetValue(temp, Converter.ConvertObjectTo(arr[propIndex], currentProp.PropertyType));
                    }

                    this.DataCollection.Add(temp);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public override bool ReloadFormString(string[] reloadDataString)
        {
            try
            {
                this.DataCollection.Clear();
                //var lines = reloadDataString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                //var propArr = lines.First().Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                var lines = reloadDataString;
                var propArr = lines.First().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                for (int rowIndex = 1; rowIndex < lines.Length; rowIndex++)
                {
                    RawDatumItem_LIV temp = new RawDatumItem_LIV();
                    var rdType = typeof(RawDatumItem_LIV);
                    var arr = lines[rowIndex].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    for (int propIndex = 0; propIndex < propArr.Length; propIndex++)
                    {
                        var currentProp = rdType.GetProperty(propArr[propIndex]);
                        currentProp.SetValue(temp, Converter.ConvertObjectTo(arr[propIndex], currentProp.PropertyType));
                    }

                    this.DataCollection.Add(temp);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}