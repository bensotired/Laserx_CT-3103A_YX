using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawData_LIV_RollBack : RawDataCollectionBase<RawDatumItem_LIV_RollBack>
    {
        public RawData_LIV_RollBack()
        {
            Temperature_degC = 25.0;

        }
        [RawDataBrowsableElement]
        public double Temperature_degC { get; set; }

        public override bool ReloadFormString(string reloadDataString)
        {
            try
            {
                this.DataCollection.Clear();
                var lines = reloadDataString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                var propArr = lines.First().Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);

                for (int rowIndex = 1; rowIndex < lines.Length; rowIndex++)
                {
                    RawDatumItem_LIV_RollBack temp = new RawDatumItem_LIV_RollBack();
                    var rdType = typeof(RawDatumItem_LIV_RollBack);
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
                    RawDatumItem_LIV_RollBack temp = new RawDatumItem_LIV_RollBack();
                    var rdType = typeof(RawDatumItem_LIV_RollBack);
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