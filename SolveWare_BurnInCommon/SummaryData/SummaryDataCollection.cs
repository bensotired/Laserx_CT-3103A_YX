using System;

namespace SolveWare_BurnInCommon
{
    [Serializable]
    //public class SummaryDataCollection : CURDBase<SummaryDatumItemBase>//, IEnumerable<MotorSetting>
    public class SummaryDataCollection : CURDBaseLite<SummaryDatumItemBase>//, IEnumerable<MotorSetting>
    {
        public override void AddSingleItem(SummaryDatumItemBase item)
        {
            if (this.ItemCollection.Exists(x => /*x.ID == item.ID ||*/ x.Name == item.Name))
            {
                throw new Exception($"Duplicate items with Name ={item.Name} !");
            }
            this.ItemCollection.Add(item);

        }
    }
}