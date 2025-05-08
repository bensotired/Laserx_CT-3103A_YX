using SolveWare_BurnInCommon;
using System.Collections.Generic;

namespace SolveWare_Motion
{

    public class MotorSettingCollection : CURDBase<MotorSetting>//, IEnumerable<MotorSetting>
    {

        public MotorSettingCollection()
        {
            this.ItemCollection = new List<MotorSetting>();

        }
        public bool IsExistedDupAxis()
        {
            bool anyDup = false;
            foreach (var item in this.ItemCollection)
            {

                var dupItems = this.ItemCollection.FindAll(mst => mst.ID != item.ID &&
                                                    mst.MotorTable.CardNo == item.MotorTable.CardNo &&
                                                    mst.MotorTable.AxisNo == item.MotorTable.AxisNo);
                if (dupItems?.Count >= 1)
                {
                    anyDup = true;
                }
            }
            return anyDup;
 
        }

        public IEnumerator<MotorSetting> GetEnumerator()
        {
            return this.ItemCollection.GetEnumerator();
        }

       
    }
}