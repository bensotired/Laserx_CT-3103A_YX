using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SolveWare_BurnInCommon
{
    public class CURDBase<TCURDItem> : ICURD<TCURDItem> where TCURDItem : class, ICURDItem  
    {
        public CURDBase()
        {
            ItemCollection = new List<TCURDItem>(); 
        }

        public List<TCURDItem> ItemCollection { get; set; }

        public virtual List<TPropVal> GetDataListByPropName<TPropVal>(string propName) //where TRawDatumItemBase:class,IRawDatumItemBase
        {
            List<TPropVal> vals = new List<TPropVal>();

            var itemProps = typeof(TCURDItem).GetProperties().ToList();
            if (itemProps.Exists(p => p.Name == propName) == false)
            {
                return vals;
            }

            PropertyInfo pInfo = itemProps.Find(p => p.Name == propName);
            this.ItemCollection.ForEach(item => vals.Add((TPropVal)pInfo.GetValue(item)));
            return vals;
        }
        public virtual bool AddSingleItem(TCURDItem item, ref string sErr)
        {
            if (this.ItemCollection.Exists(x => x.ID == item.ID || x.Name == item.Name))
            {
                sErr = $"新增失败,Duplicate items with Name ={item.Name} ID ={item.ID}!";
                return false;
            }
            this.ItemCollection.Add(item);
            return true;
        }
        public virtual void AddSingleItem(TCURDItem item)
        {
            if (this.ItemCollection.Exists(x => x.ID == item.ID || x.Name == item.Name))
            {
                throw new Exception($"Duplicate items with Name ={item.Name} ID ={item.ID}!");
            }
            this.ItemCollection.Add(item);
        }
        public virtual bool ExistSingleItem(long id)
        {
            if (this.ItemCollection.Exists(x => x.ID == id))
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        public virtual bool DeleteSingleItem(TCURDItem item, ref string sErr)
        {
            int index = this.ItemCollection.FindIndex(x => x.ID == item.ID);
            if (index < 0)
            {
                sErr = "无可删除物件";
                return false;
            }

            this.ItemCollection.RemoveAt(index);
            return true;
        }
        public virtual bool DeleteSingleItem(long Id, ref string sErr)
        {
            int index = this.ItemCollection.FindIndex(x => x.ID == Id);
            if (index < 0)
            {
                sErr = "无可删除物件";
                return false;
            }

            this.ItemCollection.RemoveAt(index);
            return true;
        }

        public virtual TCURDItem GetSingleItem(string name)
        {
            TCURDItem item = default(TCURDItem);
            if (this.ItemCollection.Count == 0) return item;

            item = this.ItemCollection.Find(x => x.Name == name);
            return item;
        }
        public virtual TCURDItem GetSingleItem(long id)
        {
            TCURDItem item = default(TCURDItem);
            if (this.ItemCollection.Count == 0) return item;

            item = this.ItemCollection.Find(x => x.ID == id);
            return item;
        }
       

        public virtual bool UpdateSingleItem(TCURDItem item, ref string sErr)
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
                int index = this.ItemCollection.FindIndex(x => x.ID == item.ID);
                if (index < 0)
                {
                    sErr = "储存 失败";
                    return false;
                }

                this.ItemCollection[index] = item;
            }
            return true;
        }

        public virtual IEnumerator<TCURDItem> GetEnumerator()
        {
            return this.ItemCollection.GetEnumerator();
        }

        public virtual void Clear()
        {
            this.ItemCollection.Clear();
        }
    }
}