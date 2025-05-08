using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SolveWare_BurnInCommon
{
    [Serializable]
    //[XmlType("Pair")]
 
    public class DataPair<TKey, TValue>
    {
        public DataPair()
        {

        }
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }

    [Serializable]
    public class DataBook<TKey, TValue>
    {
        public List<DataPair<TKey, TValue>> Book { get; set; }
        public DataBook()
        {
            this.Book = new List<DataPair<TKey, TValue>>();
        }
        public int Count
        {
            get
            {
                return this.Book.Count;
            }
        }
        public void Clear()
        {
            if (this.Book != null && this.Book.Count > 0)
            {
                this.Book.Clear();
            }
        }
        public void Add(DataPair<TKey, TValue> item)
        {
            this.Book.Add(item);
        }
        public void Add(TKey key, TValue value)
        {
            this.Book.Add(new DataPair<TKey, TValue>() { Key = key, Value = value });
        }
        public void InsertItem(int index , TKey key, TValue value)
        {
            if (this.Book.Count <= 0)
            {
                return  ;
            }
            if (index > this.Book.Count - 1)
            {
                return  ;
            }
            else
            {
            
            }
            this.Book.Insert(index, new DataPair<TKey, TValue>() { Key = key, Value = value });
        }
        public void InsertItem_V2(int index, TKey key, TValue value)
        {
            this.Book.Insert(index, new DataPair<TKey, TValue>() { Key = key, Value = value });
        }
        public bool IsIndexInRange(int index)
        {
            if (this.Book.Count <= 0)
            {
                return false;
            }
            if (index > this.Book.Count - 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void RemoveAtIndex(int index)
        {
            if(this.Book.Count <= 0)
            {
                return;
            }
            if(index > this.Book.Count - 1)
            {
                return;
            }
            this.Book.RemoveAt(index);
        }
        public TValue this[TKey key]
        {
            get
            {
                foreach (var item in this.Book)
                {
                    if (item.Key.Equals(key))
                    {
                        return item.Value;
                    }
                }
                return default(TValue);
            }
            set
            {
                foreach (var item in this.Book)
                {
                    if (item.Key.Equals(key))
                    {
                        item.Value = value;
                    }
                }
            }
        }
        public bool Contains(TKey key)
        {
            if (this.Book == null || this.Book.Count == 0)
            {
                return false;
            }
            return this.Book.Exists(item => item.Key.Equals(key));
        }
        public IEnumerator<DataPair<TKey, TValue>> GetEnumerator()
        {
            return this.Book.GetEnumerator();
        }
        public bool ContainsKey(TKey key)
        {
            return this.Book.Exists(item => item.Key.Equals(key));
        }
        public bool ContainsValue(TValue val)
        {
            return this.Book.Exists(item => item.Value.Equals(val));
        }
        public bool Contains(DataPair<TKey,TValue> dItem)
        {
            return this.Book.Exists(item => item.Value.Equals(dItem));
        }
    }
}