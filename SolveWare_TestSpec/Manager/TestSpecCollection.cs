using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Specification;
using System;
using System.Collections.Generic;

namespace SolveWare_TestSpecification
{
  

    public class TestSpecCollection : CURDBase<TestSpecification>//, IEnumerable<MotorSetting>
    {
        public TestSpecCollection()
        {
            this.ItemCollection =new List<TestSpecification>();
        }

        public TestSpecCollection(TestSpecCollection testSpecCollection)
        {
            this.ItemCollection = new List<TestSpecification>(testSpecCollection.ItemCollection);
        }


        //判断重名
        public bool IsExistedDupAxis()
        {
            bool anyDup = false;
            foreach (var item in this.ItemCollection)
            {

                var dupItems = this.ItemCollection.FindAll(colItem =>
                                                    colItem.Name  == item.Name  &&
                                                    colItem.Version  == item.Version );
                if (dupItems?.Count >= 1)
                {
                    anyDup = true;
                }
            }
            return anyDup;

        }

        //查找测试规格
        public TestSpecification GetSpecByTag(string specTag)
        {
            return this.ItemCollection.Find(spec => spec.SpecTag == specTag);
        }
 
        public override bool UpdateSingleItem(TestSpecification item, ref string sErr)
        {
            //Check Name
            var checkObj = this.ItemCollection.FindAll(x =>  x.Name == item.Name &&
                                                            x.Version == item.Version);
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
    }
}
