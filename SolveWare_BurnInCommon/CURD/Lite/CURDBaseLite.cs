using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SolveWare_BurnInCommon
{
    public class CURDBaseLite<TCURDItemLite> : ICURDLite<TCURDItemLite> where TCURDItemLite : class, ICURDItemLite
    {
        public CURDBaseLite()
        {
            ItemCollection = new List<TCURDItemLite>();
        }
        public List<TCURDItemLite> ItemCollection { get; set; }
        public int Count
        {
            get
            {
                return (int)this.ItemCollection?.Count;
            }
        }
        public virtual List<TPropVal> GetDataListByPropName<TPropVal>(string propName) //where TRawDatumItemBase:class,IRawDatumItemBase
        {
            List<TPropVal> vals = new List<TPropVal>();

            var itemProps = typeof(TCURDItemLite).GetProperties().ToList();
            if (itemProps.Exists(p => p.Name == propName) == false)
            {
                return vals;
            }

            PropertyInfo pInfo = itemProps.Find(p => p.Name == propName);

            this.ItemCollection.ForEach(item => vals.Add((TPropVal)pInfo.GetValue(item)));

            return vals;
        }
        public virtual bool AddSingleItem(TCURDItemLite item, ref string sErr)
        {
            if (this.ItemCollection.Exists(x => x.Name == item.Name))
            {
                sErr = "新增失败";
                return false;
            }

            this.ItemCollection.Add(item);
            return true;
        }
        public virtual void AddSingleItem(TCURDItemLite item)
        {
            if (this.ItemCollection.Exists(x => x.Name == item.Name))
            {
                throw new Exception($"Duplicate items with Name ={item.Name}  !");
            }
            this.ItemCollection.Add(item);
        }

        public virtual bool DeleteSingleItem(TCURDItemLite item, ref string sErr)
        {
            int index = this.ItemCollection.FindIndex(x => x.Name == item.Name);
            if (index < 0)
            {
                sErr = "无可删除物件";
                return false;
            }

            this.ItemCollection.RemoveAt(index);
            return true;
        }
        public virtual TCURDItemLite GetSingleItem(string name)
        {
            TCURDItemLite item = default(TCURDItemLite);
            if (this.ItemCollection.Count == 0) return item;

            item = this.ItemCollection.Find(x => x.Name == name);
            return item;
        }


        public virtual bool UpdateSingleItem(TCURDItemLite item, ref string sErr)
        {
            //Check Name
            var checkObj = this.ItemCollection.FindAll(x => x.Name == item.Name);
            if (checkObj.Count > 1)
            {
                sErr = "已有相同名字的物件";
                return false;
            }

            //无相同的物件 直接存
            if (checkObj.Count == 0)
            {
                this.ItemCollection.Add(item);
            }
            else
            {
                //有相同的物件 抓出来
                int index = this.ItemCollection.FindIndex(x => x.Name == item.Name);
                if (index < 0)
                {
                    sErr = "储存 失败";
                    return false;
                }

                this.ItemCollection[index] = item;
            }
            return true;
        }

        public virtual IEnumerator<TCURDItemLite> GetEnumerator()
        {
            return this.ItemCollection.GetEnumerator();
        }
        public virtual void Clear()
        {
            this.ItemCollection.Clear();
        }
    }
}