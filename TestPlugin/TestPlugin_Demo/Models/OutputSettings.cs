using SolveWare_BurnInCommon;
using System;

namespace TestPlugin_Demo
{
    [Serializable]
    public class OutputSettingsItem //: CURDItemLite
    {
        public OutputSettingsItem()
        {
            this.OutFeed = new DataBook<FeedBox_Out, DataBook<ProductPosition, double>>();
            DataBook<ProductPosition, double> dataPairs = new DataBook<ProductPosition, double>();
            foreach (string name in Enum.GetNames(typeof(ProductPosition)))
            {
                dataPairs.Add((ProductPosition)Enum.Parse(typeof(ProductPosition), name), 0);
            }
            foreach (string name in Enum.GetNames(typeof(FeedBox_Out)))
            {
                this.OutFeed.Add((FeedBox_Out)Enum.Parse(typeof(FeedBox_Out), name), dataPairs);
            }
        }
        public DataBook<FeedBox_Out, DataBook<ProductPosition, double>> OutFeed { get; set; }

    }

    //[Serializable]
    //public class OutputSettingsCollection : CURDBaseLite<OutputSettingsItem>,ICURDItemLite
    //{
    //    public string Name { get; set; }

    //    public const string BIN_NG = "NG";
    //    public OutputSettingsCollection()
    //    {
    //        this.ItemCollection = new List<OutputSettingsItem>();
    //    }
    //    public virtual bool DeleteSingleItem(string itemName)
    //    {
    //        int index = this.ItemCollection.FindIndex(x => x.Name == itemName);
    //        if (index < 0)
    //        {
    //            return false;
    //        }

    //        this.ItemCollection.RemoveAt(index);
    //        return true;
    //    }
    //    public virtual void DeleteSetting(SorterType sorterT)
    //    {
    //        if (this.ItemCollection.Exists(item => item.Sorter == sorterT))
    //        {
    //            int index = this.ItemCollection.FindIndex(item => item.Sorter == sorterT);
    //            this.ItemCollection.RemoveAt(index);
    //        }
    //    }
    //    public override void AddSingleItem(OutputSettingsItem item)
    //    {
    //        if (this.ItemCollection.Exists(x => x.Name == item.Name))
    //        {
    //            throw new Exception($"Duplicate items with Name ={item.Name}  !");
    //        }
    //        if (this.ItemCollection.Exists(x => x.Sorter == item.Sorter))
    //        {
    //            throw new Exception($"Duplicate SORTER TYPE[{ item.Sorter}]  !");
    //        }
    //        this.ItemCollection.Add(item);
    //    }
    //    public override bool UpdateSingleItem(OutputSettingsItem item, ref string sErr)
    //    {
    //        //Check Name
    //        var checkObj = this.ItemCollection.FindAll(x => x.Sorter == item.Sorter);
    //        if (checkObj.Count > 1)
    //        {
    //            sErr = "已有相同名字的物件";
    //            return false;
    //        }

    //        //无相同的物件 直接存
    //        if (checkObj.Count == 0)
    //        {
    //            this.ItemCollection.Add(item);
    //        }
    //        else
    //        {
    //            //有相同的物件 抓出来
    //            int index = this.ItemCollection.FindIndex(x => x.Sorter == item.Sorter);
    //            if (index < 0)
    //            {
    //                sErr = "储存 失败";
    //                return false;
    //            }

    //            for (int orgIndex = 0; orgIndex < this.ItemCollection.Count; orgIndex++)
    //            {
    //                if (orgIndex == index)
    //                {
    //                    continue;
    //                }
    //                if (this.ItemCollection[orgIndex].Bin_Set == item.Bin_Set)
    //                {
    //                    sErr = $"储存 失败,已经存在相同的Bin配置[{item.Bin_Set}]分选环!";
    //                    return false;
    //                }
    //                //if (this.ItemCollection[orgIndex].Name == item.Name)
    //                //{
    //                //    sErr = $"储存 失败,已经存在相同的分选环号码[{item.Name}]!";
    //                //    return false;
    //                //}
    //            }
    //            this.ItemCollection[index] = item;
    //        }
    //        return true;
    //    }
    //    public bool IsNG_Include()
    //    {
    //        try
    //        {
    //            return this.ItemCollection.Exists(item => item.Bin_Set == BIN_NG);
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }
    //}

}
