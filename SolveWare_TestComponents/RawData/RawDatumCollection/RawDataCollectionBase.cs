
using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawDataCollectionBase<TRawDatumItem> : RawDataBaseLite, 
        IRawDataCollectionBase where TRawDatumItem : class, IRawDatumItemBase
    {
        public RawDataCollectionBase()
        {
            this.DataCollection = new List<TRawDatumItem>();
        }
        [RawDataCollection]
        public List<TRawDatumItem> DataCollection { get; set; }
        public IRawDatumItemBase Peek()
        {
            return DataCollection.First()  ;
        }
        public void Add(TRawDatumItem datum)
        {
            this.DataCollection.Add(datum);
        }
        public void Clear()
        {
            this.DataCollection.Clear();
        }
        public int Count
        {
            get
            {
                return this.DataCollection.Count;
            }
        }
        public IEnumerator<TRawDatumItem> GetEnumerator()
        {
            return this.DataCollection.GetEnumerator();
        }
        public virtual List<object> GetChartSeriesDataListByPropName(string enumerableElementName) //where TRawDatumItemBase:class,IRawDatumItemBase
        {
            List<object> temp = new List<object>();

            var rdType = this.GetType();
            var rdProps = rdType.GetProperties();
            foreach (var prop in rdProps)
            {
                if (PropHelper.IsPropertyBelongs<RawDataCollectionAttribute>(prop))
                {
                    var propVal = prop.GetValue(this);


                    var listVal = (propVal as List<TRawDatumItem>);
                    var itemProps = typeof(TRawDatumItem).GetProperties();

                    foreach (var itemProp in itemProps)
                    {
                        if (itemProp.Name.Equals(enumerableElementName))
                        {
                            for (int i = 0; i < listVal.Count; i++)
                            {
                                temp.Add(itemProp.GetValue(listVal[i]));
                            }
                        }
                    }
                }
            }
            return temp;
        }
        public virtual List<TObject> GetChartSeriesDataListByPropName<TObject>(string enumerableElementName) //where TRawDatumItemBase:class,IRawDatumItemBase
        {
            List<TObject> temp = new List<TObject>();

            var rdType = this.GetType();
            var rdProps = rdType.GetProperties();
            foreach (var prop in rdProps)
            {
                if (PropHelper.IsPropertyBelongs<RawDataCollectionAttribute>(prop))
                {
                    var propVal = prop.GetValue(this);


                    var listVal = (propVal as List<TRawDatumItem>);
                    var itemProps = typeof(TRawDatumItem).GetProperties();

                    foreach (var itemProp in itemProps)
                    {
                        if (itemProp.Name.Equals(enumerableElementName))
                        {
                            for (int i = 0; i < listVal.Count; i++)
                            {
                                var val = (TObject)Converter.ConvertObjectTo(itemProp.GetValue(listVal[i]), typeof(TObject));
                                temp.Add(val);
                            }
                        }
                    }
                }
            }
            return temp;
        }
        public virtual List<double> GetDataListByPropName(string enumerableElementName)  
        {
            List<double> temp = new List<double>();

            var rdType = this.GetType();
            var rdProps = rdType.GetProperties();
            foreach (var prop in rdProps)
            {
                if (PropHelper.IsPropertyBelongs<RawDataCollectionAttribute>(prop))
                {
                    var propVal = prop.GetValue(this);


                    var listVal = (propVal as List<TRawDatumItem>);
                    var itemProps = typeof(TRawDatumItem).GetProperties();

                    foreach (var itemProp in itemProps)
                    {
                        if (PropHelper.IsPropertyBelongs<RawDataCollectionItemElementAttribute>(itemProp))
                        {
                            var att = itemProp.GetCustomAttributes(typeof(RawDataCollectionItemElementAttribute), false);
                            if (att != null && att.Length > 0)
                            {
                                if ((att[0] as RawDataCollectionItemElementAttribute).ElementTag.Equals(enumerableElementName))
                                {
                                    for (int i = 0; i < listVal.Count; i++)
                                    {
                                        temp.Add(Convert.ToDouble(itemProp.GetValue(listVal[i])));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return temp;
        }
        public virtual List<object> GetDataListByPropName_V2(string enumerableElementName) 
        {
            List<object> temp = new List<object>();

            var rdType = this.GetType();
            var rdProps = rdType.GetProperties();
            foreach (var prop in rdProps)
            {
                if (PropHelper.IsPropertyBelongs<RawDataCollectionAttribute>(prop))
                {
                    var propVal = prop.GetValue(this);


                    var listVal = (propVal as List<TRawDatumItem>);
                    var itemProps = typeof(TRawDatumItem).GetProperties();

                    foreach (var itemProp in itemProps)
                    {
                        if (PropHelper.IsPropertyBelongs<RawDataCollectionItemElementAttribute>(itemProp))
                        {
                            var att = itemProp.GetCustomAttributes(typeof(RawDataCollectionItemElementAttribute), false);
                            if (att != null && att.Length > 0)
                            {
                                if ((att[0] as RawDataCollectionItemElementAttribute).ElementTag.Equals(enumerableElementName))
                                {
                                    for (int i = 0; i < listVal.Count; i++)
                                    {
                                        temp.Add( itemProp.GetValue(listVal[i]) );
                                        //temp.Add(Convert.ToDouble(itemProp.GetValue(listVal[i])));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return temp;
        }
        public virtual bool SetDataValueListByPropName (string enumerableElementName, List<object> values)
        {

            var rdType = this.GetType();
            var rdProps = rdType.GetProperties();
            foreach (var prop in rdProps)
            {
                if (PropHelper.IsPropertyBelongs<RawDataCollectionAttribute>(prop))
                {
                    var propVal = prop.GetValue(this);


                    var listVal = (propVal as List<TRawDatumItem>);
                    if(listVal?.Count != values?.Count)
                    {
                        return false;
                    }
                    var itemProps = typeof(TRawDatumItem).GetProperties();

                    foreach (var itemProp in itemProps)
                    {
                        if (PropHelper.IsPropertyBelongs<RawDataCollectionItemElementAttribute>(itemProp))
                        {
                            var att = itemProp.GetCustomAttributes(typeof(RawDataCollectionItemElementAttribute), false);
                            if (att != null && att.Length > 0)
                            {
                                if ((att[0] as RawDataCollectionItemElementAttribute).ElementTag.Equals(enumerableElementName))
                                {
                                    for (int i = 0; i < listVal.Count; i++)
                                    {
                                        //temp.Add(itemProp.GetValue(listVal[i]));
                                        itemProp.SetValue(listVal[i], values[i]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
        public virtual Dictionary<string, List<object>> GetDataDictByPropNames(params string[] enumerableElementNames)
        {

            Dictionary<string, List<object>> dict = new Dictionary<string, List<object>>();
            if (enumerableElementNames.Length <= 0)
            {
                return dict;
            }
            foreach (var item in enumerableElementNames)
            {
                dict.Add(item, new List<object>());
            }

            List<object> temp = new List<object>();

            var rdType = this.GetType();
            var rdProps = rdType.GetProperties();
            foreach (var prop in rdProps)
            {
                if (PropHelper.IsPropertyBelongs<RawDataCollectionAttribute>(prop))
                {
                    var propVal = prop.GetValue(this);

                    var listVal = (propVal as List<TRawDatumItem>);
                    //var listVal = (propVal as List<RawDatumItemBase>);

                    var itemProps = typeof(TRawDatumItem).GetProperties();


                    foreach (var kvp in dict)
                    {
                        foreach (var itemProp in itemProps)
                        {
                            if (PropHelper.IsPropertyBelongs<RawDataCollectionItemElementAttribute>(itemProp))
                            {
                                var att = itemProp.GetCustomAttributes(typeof(RawDataCollectionItemElementAttribute), false);
                                if (att != null && att.Length > 0)
                                {
                                    if ((att[0] as RawDataCollectionItemElementAttribute).ElementTag.Equals(kvp.Key))
                                    {
                                        for (int i = 0; i < listVal.Count; i++)
                                        {
                                            kvp.Value.Add(itemProp.GetValue(listVal[i]));
                                            //kvp.Value.Add(Convert.ToDouble(itemProp.GetValue(listVal[i])));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return dict;
        }

        public virtual Dictionary<string, List<TObject>> GetDataDictByPropNames<TObject>(params string[] enumerableElementNames)
        {
            var destObjDict = this.GetDataDictByPropNames(enumerableElementNames);
            Dictionary<string, List<TObject>> destTypeDict = new Dictionary<string, List<TObject>>();
            foreach (var objKvp in destObjDict)
            {
                var destTypeList = new List<TObject>();
                foreach (var val in objKvp.Value)
                {
                    destTypeList.Add((TObject)Converter.ConvertObjectTo(val, typeof(TObject)));
                }
                destTypeDict.Add(objKvp.Key, destTypeList);
            }


            return destTypeDict;
        }
        public virtual object GetDataByAttributePropName<TObject>(string enumerableElementName)
        {
            var rdType = this.GetType();
            var rdProps = rdType.GetProperties();

            object obj = null;

            foreach (var prop in rdProps)
            {
                if (PropHelper.IsPropertyBelongs<TObject>(prop))
                {
                    if (prop.Name == enumerableElementName)
                    {
                        obj = prop.GetValue(this);
                        break;
                    }
                }
            }
            return obj;
        }


        public virtual object GetDataByPropName(string enumerableElementName)
        {
            var rdType = this.GetType();
            var rdProps = rdType.GetProperties();
            object obj = null;
            foreach (var prop in rdProps)
            {
                if (prop.Name == enumerableElementName)
                {
                    obj = prop.GetValue(this);
                    break;
                }
            }
            return obj;
        }
    }
}