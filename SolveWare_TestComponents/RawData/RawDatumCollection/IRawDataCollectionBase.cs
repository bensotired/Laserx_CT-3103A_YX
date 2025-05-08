using System.Collections.Generic;

namespace SolveWare_TestComponents.Data
{
    public interface IRawDataCollectionBase : IRawDataBaseLite// :  IRawDatumItemBase
    {
        IRawDatumItemBase Peek();
        int Count { get; }
        //List<TRawDatumItem> DataCollection { get; set; }
        Dictionary<string, List<object>> GetDataDictByPropNames(params string[] enumerableElementNames);
        Dictionary<string, List<TObject>> GetDataDictByPropNames<TObject>(params string[] enumerableElementNames);
        List<double> GetDataListByPropName(string enumerableElementName);
        List<object> GetDataListByPropName_V2(string enumerableElementName);
        List<object> GetChartSeriesDataListByPropName(string enumerableElementName);
        bool SetDataValueListByPropName(string enumerableElementName, List<object> values);

        object GetDataByAttributePropName<TObiect>(string enumerableElementName);
        object GetDataByPropName(string enumerableElementName);
    }
}