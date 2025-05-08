using SolveWare_BurnInCommon;
using System.Collections.Generic;

namespace SolveWare_BinSorter
{


    public class BinSettingCollection : CURDBaseLite<BinSetting>, ICURDItemLite
    {
        public const string ConfigFileName = @"BinSettingCollection.xml";

        public string Name { get; set; }

        public BinSettingCollection()
        {
            this.ItemCollection = new List<BinSetting>();
        }
        public BinSettingCollection(BinSettingCollection binSettingCollection)
        {
            this.ItemCollection = new List<BinSetting>(binSettingCollection.ItemCollection);
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
        public BinSetting GetSpecByTag(string binTag)
        {
            return this.ItemCollection.Find(spec => spec.BinTag == binTag);
        }
        public override bool UpdateSingleItem(BinSetting item, ref string sErr)
        {
            //Check Name
            //var checkObj = this.ItemCollection.FindAll(x => x.Name == item.Name &&
            //                                                x.Version == item.Version);
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
                //int index = this.ItemCollection.FindIndex(x => x.Name == item.Name &&
                //                                            x.Version == item.Version);
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
        public bool UpdateSingleItem(string binName, BinSetting item, ref string sErr)
        {
            //Check Name
            //var checkObj = this.ItemCollection.FindAll(x => x.Name == item.Name &&
            //                                                x.Version == item.Version);
            var checkObj = this.ItemCollection.FindAll(x => x.Name == binName);
            if (checkObj.Count > 1)
            {
                sErr = "已有相同名字的物件";
                return false;
            }

            //无相同的物件 直接存
            if (checkObj.Count == 0)
            {
                item.Name = binName;
                this.ItemCollection.Add(item);
            }
            else
            {
                //有相同的物件 抓出来
                //int index = this.ItemCollection.FindIndex(x => x.Name == item.Name &&
                //                                            x.Version == item.Version);
                int index = this.ItemCollection.FindIndex(x => x.Name == binName);
                if (index < 0)
                {
                    sErr = "储存 失败";
                    return false;
                }
                item.Name = binName;
                this.ItemCollection[index] = item;
            }
            return true;
        }
    }
}