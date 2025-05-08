using SolveWare_BurnInCommon;
using System.Collections.Generic;

namespace SolveWare_BinSorter
{


    public class BinSettingCollectionList : CURDBaseLite<BinSettingCollection>
    {
      
        public const string ConfigFileName = @"BinSettingCollectionList.xml";
        public BinSettingCollectionList()
        {
            this.ItemCollection = new List<BinSettingCollection>();
        }
        //判断重名
        public bool IsExistedDupAxis()
        {
            bool anyDup = false;
            foreach (var item in this.ItemCollection)
            {
                var dupItems = this.ItemCollection.FindAll(colItem =>
                                           colItem.Name == item.Name);
                if (dupItems?.Count >= 1)
                {
                    anyDup = true;
                }
            }
            return anyDup;

        }
        //查找测试规格
        public BinSettingCollection GetBinSettingCollectionByName(string binCollectionName)
        {
            return this.ItemCollection.Find(item => item.Name == binCollectionName);
        }
        public override bool UpdateSingleItem(BinSettingCollection item, ref string sErr)
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
        public bool UpdateSingleItem(string binCollectionName, BinSettingCollection item, ref string sErr)
        {
            //Check Name
            var checkObj = this.ItemCollection.FindAll(x => x.Name == binCollectionName);
            if (checkObj.Count > 1)
            {
                sErr = "已有相同名字的物件";
                return false;
            }

            //无相同的物件 直接存
            if (checkObj.Count == 0)
            {
                item.Name = binCollectionName;
                this.ItemCollection.Add(item);
            }
            else
            {
                //有相同的物件 抓出来
                //int index = this.ItemCollection.FindIndex(x => x.Name == item.Name &&
                //                                            x.Version == item.Version);
                int index = this.ItemCollection.FindIndex(x => x.Name == binCollectionName);
                if (index < 0)
                {
                    sErr = "储存 失败";
                    return false;
                }
                item.Name = binCollectionName;
                this.ItemCollection[index] = item;
            }
            return true;
        }
    }
}