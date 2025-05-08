using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolveWare_TestComponents
{
    [Serializable]
    public class RawDataMenuCollection<TRawDataCollectionBase> : RawDataBaseLite, 
        IRawDataMenuCollection where TRawDataCollectionBase : class, IRawDataCollectionBase
    {
        public RawDataMenuCollection()
        {
            this.DataMenuCollection = new List<TRawDataCollectionBase>();
        }
        public List<TRawDataCollectionBase> DataMenuCollection { get; set; }
        
        public IEnumerable<IRawDataCollectionBase> GetDataMenuCollection()
        {
            var rd = DataMenuCollection as IEnumerable<IRawDataCollectionBase> ;
            return rd;
        }
        public IRawDataCollectionBase Peek()
        {
            return DataMenuCollection.First();
        }

        public void Add(TRawDataCollectionBase datum)
        {
            this.DataMenuCollection.Add(datum);
        }
        public void Clear()
        {
            this.DataMenuCollection.Clear();
        }

        IRawDatumItemBase Data.IRawDataCollectionBase.Peek()
        {
            return null;
        }

        public virtual Dictionary<string, List<object>> GetDataDictByPropNames(params string[] enumerableElementNames)
        {
            Dictionary<string, List<object>> dict = new Dictionary<string, List<object>>();
            //if (enumerableElementNames.Length <= 0)
            //{
            //    return dict;
            //}
            //foreach (var item in enumerableElementNames)
            //{
            //    dict.Add(item, new List<object>());
            //}

            //List<object> temp = new List<object>();
            foreach (var item in DataMenuCollection)
            {

                var lrd = item as IRawDataCollectionBase;
                var D = lrd.GetDataDictByPropNames(enumerableElementNames);
                foreach (var d in D)
                {
                    if (!dict.ContainsKey(d.Key))
                    {
                        dict.Add(d.Key,d.Value);
                    }
                    else
                    {
                        dict[d.Key].AddRange(d.Value);
                    }
                }
                //dict = dict.Concat(D).GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.First().Value);

                #region
                //var itType = this.GetType();
                //var itProps = itType.GetProperties();
                //foreach (var iprops in itProps)
                //{
                //    if (PropHelper.IsPropertyBelongs<RawDataMenuCollectionAttribute>(iprops))
                //    {
                //        var rdType = this.GetType();
                //        var rdProps = rdType.GetProperties();
                //        foreach (var prop in rdProps)
                //        {
                //            if (PropHelper.IsPropertyBelongs<RawDataCollectionAttribute>(prop))
                //            {
                //                var propVal = prop.GetValue(this);

                //                var listVal = (propVal as List<IRawDatumItemBase>);
                //                //var listVal = (propVal as List<RawDatumItemBase>);

                //                var itemProps = typeof(IRawDatumItemBase).GetProperties();


                //                foreach (var kvp in dict)
                //                {
                //                    foreach (var itemProp in itemProps)
                //                    {
                //                        if (PropHelper.IsPropertyBelongs<RawDataCollectionItemElementAttribute>(itemProp))
                //                        {
                //                            var att = itemProp.GetCustomAttributes(typeof(RawDataCollectionItemElementAttribute), false);
                //                            if (att != null && att.Length > 0)
                //                            {
                //                                if ((att[0] as RawDataCollectionItemElementAttribute).ElementTag.Equals(kvp.Key))
                //                                {
                //                                    for (int i = 0; i < listVal.Count; i++)
                //                                    {
                //                                        kvp.Value.Add(itemProp.GetValue(listVal[i]));
                //                                        //kvp.Value.Add(Convert.ToDouble(itemProp.GetValue(listVal[i])));
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion

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

        public virtual List<double> GetDataListByPropName(string enumerableElementName)
        {
            List<double> temp = new List<double>();
            foreach (var item in DataMenuCollection)
            {
                var lrd = item as IRawDataCollectionBase;
                var L = lrd.GetDataListByPropName(enumerableElementName);
                temp.AddRange(L);


                //var rdType = this.GetType();
                //var rdProps = rdType.GetProperties();
                //foreach (var prop in rdProps)
                //{
                //    if (PropHelper.IsPropertyBelongs<RawDataCollectionAttribute>(prop))
                //    {
                //        var propVal = prop.GetValue(this);


                //        var listVal = (propVal as List<IRawDatumItemBase>);
                //        var itemProps = typeof(IRawDatumItemBase).GetProperties();

                //        foreach (var itemProp in itemProps)
                //        {
                //            if (PropHelper.IsPropertyBelongs<RawDataCollectionItemElementAttribute>(itemProp))
                //            {
                //                var att = itemProp.GetCustomAttributes(typeof(RawDataCollectionItemElementAttribute), false);
                //                if (att != null && att.Length > 0)
                //                {
                //                    if ((att[0] as RawDataCollectionItemElementAttribute).ElementTag.Equals(enumerableElementName))
                //                    {
                //                        for (int i = 0; i < listVal.Count; i++)
                //                        {
                //                            temp.Add(Convert.ToDouble(itemProp.GetValue(listVal[i])));
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
            }
           
            return temp;
        }

        public virtual List<object> GetDataListByPropName_V2(string enumerableElementName)
        {
            List<object> temp = new List<object>();
            foreach (var item in DataMenuCollection)
            {
                var lrd = item as IRawDataCollectionBase;
                var L = lrd.GetDataListByPropName_V2(enumerableElementName);
                temp.AddRange(L);

                //var rdType = this.GetType();
                //var rdProps = rdType.GetProperties();
                //foreach (var prop in rdProps)
                //{
                //    if (PropHelper.IsPropertyBelongs<RawDataCollectionAttribute>(prop))
                //    {
                //        var propVal = prop.GetValue(this);


                //        var listVal = (propVal as List<IRawDatumItemBase>);
                //        var itemProps = typeof(IRawDatumItemBase).GetProperties();

                //        foreach (var itemProp in itemProps)
                //        {
                //            if (PropHelper.IsPropertyBelongs<RawDataCollectionItemElementAttribute>(itemProp))
                //            {
                //                var att = itemProp.GetCustomAttributes(typeof(RawDataCollectionItemElementAttribute), false);
                //                if (att != null && att.Length > 0)
                //                {
                //                    if ((att[0] as RawDataCollectionItemElementAttribute).ElementTag.Equals(enumerableElementName))
                //                    {
                //                        for (int i = 0; i < listVal.Count; i++)
                //                        {
                //                            temp.Add(itemProp.GetValue(listVal[i]));
                //                            //temp.Add(Convert.ToDouble(itemProp.GetValue(listVal[i])));
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
            }
            
            return temp;
        }

        public virtual List<object> GetChartSeriesDataListByPropName(string enumerableElementName)
        {
            List<object> temp = new List<object>();
            foreach (var item in DataMenuCollection)
            {
                var lrd = item as IRawDataCollectionBase;
                var L = lrd.GetChartSeriesDataListByPropName(enumerableElementName);
                temp.AddRange(L);

                //var rdType = this.GetType();
                //var rdProps = rdType.GetProperties();
                //foreach (var prop in rdProps)
                //{
                //    if (PropHelper.IsPropertyBelongs<RawDataCollectionAttribute>(prop))
                //    {
                //        var propVal = prop.GetValue(this);


                //        var listVal = (propVal as List<IRawDatumItemBase>);
                //        var itemProps = typeof(IRawDatumItemBase).GetProperties();

                //        foreach (var itemProp in itemProps)
                //        {
                //            if (itemProp.Name.Equals(enumerableElementName))
                //            {
                //                for (int i = 0; i < listVal.Count; i++)
                //                {
                //                    temp.Add(itemProp.GetValue(listVal[i]));
                //                }
                //            }
                //        }
                //    }
                //}
            }
            
            return temp;
        }

        public virtual bool SetDataValueListByPropName(string enumerableElementName, List<object> values)
        {
            foreach (var item in DataMenuCollection)
            {

                var lrd = item as IRawDataCollectionBase;
                var T = lrd.SetDataValueListByPropName(enumerableElementName,values);

                //var rdType = item.GetType();
                //var rdProps = rdType.GetProperties();
                //foreach (var prop in rdProps)
                //{
                //    if (PropHelper.IsPropertyBelongs<RawDataCollectionAttribute>(prop))
                //    {
                //        var propVal = prop.GetValue(this);


                //        var listVal = (propVal as List<IRawDatumItemBase>);
                //        if (listVal?.Count != values?.Count)
                //        {
                //            return false;
                //        }
                //        var itemProps = typeof(IRawDatumItemBase).GetProperties();

                //        foreach (var itemProp in itemProps)
                //        {
                //            if (PropHelper.IsPropertyBelongs<RawDataCollectionItemElementAttribute>(itemProp))
                //            {
                //                var att = itemProp.GetCustomAttributes(typeof(RawDataCollectionItemElementAttribute), false);
                //                if (att != null && att.Length > 0)
                //                {
                //                    if ((att[0] as RawDataCollectionItemElementAttribute).ElementTag.Equals(enumerableElementName))
                //                    {
                //                        for (int i = 0; i < listVal.Count; i++)
                //                        {
                //                            //temp.Add(itemProp.GetValue(listVal[i]));
                //                            itemProp.SetValue(listVal[i], values[i]);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
            }
                
            return true;
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

        public int Count
        {
            get
            {
                return this.DataMenuCollection.Count;
            }
        }

        
    }
}