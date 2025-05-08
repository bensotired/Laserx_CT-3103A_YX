using SolveWare_BurnInCommon;
using System.Collections.Generic;

namespace SolveWare_Motion
{

    public class AxesPositionCollection : CURDBase<AxesPosition>//, IEnumerable<MotorSetting>
    {

        public AxesPositionCollection()
        {
            this.ItemCollection = new List<AxesPosition>();
        }
        public bool IsExistedDupAxis()
        {
            bool anyDup = false;
            foreach (var item in this.ItemCollection)
            {

                var dupItems = this.ItemCollection.FindAll(mst => mst.ID != item.ID &&
                                                    mst.Name == item.Name);

                if (dupItems?.Count >= 1)
                {
                    anyDup = true;
                }
            }
            return anyDup;

        }


        public override bool UpdateSingleItem(AxesPosition item, ref string sErr)
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
        public void RemoveItem(string positionName)
        {
            this.ItemCollection.RemoveAll(item=>item.Name== positionName);
        }
      
    }
}