using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;

namespace SolveWare_Motion
{

    public class MotorSettingCollection : CURDBase<MotorSetting>//, IEnumerable<MotorSetting>
    {

        public MotorSettingCollection()
        {
            this.ItemCollection = new List<MotorSetting>();

        }
        public override void AddSingleItem(MotorSetting item)
        {
            if (this.ItemCollection.Exists(x => (x.MotorTable.AxisNo == item.MotorTable.AxisNo) && (x.MotorTable.CardNo == item.MotorTable.CardNo)))
            {
                throw new Exception($"新增失败,Duplicate items with AxisNo ={item.MotorTable.AxisNo} CardNo ={item.MotorTable.CardNo}!");
            }
            if (this.ItemCollection.Exists(x => x.ID == item.ID || x.Name == item.Name))
            {
                throw new Exception($"新增失败,Duplicate items with Name ={item.Name} ID ={item.ID}!");
            }
            base.AddSingleItem(item);
        }
        public override bool AddSingleItem(MotorSetting item, ref string sErr)
        {
            if (this.ItemCollection.Exists(x => (x.MotorTable.AxisNo == item.MotorTable.AxisNo) && (x.MotorTable.CardNo == item.MotorTable.CardNo)))
            {
                sErr = ($"新增失败,Duplicate items with AxisNo ={item.MotorTable.AxisNo} CardNo ={item.MotorTable.CardNo}!");
                return false;
            }
            if (this.ItemCollection.Exists(x => x.ID == item.ID || x.Name == item.Name))
            {
                sErr = ($"新增失败,Duplicate items with Name ={item.Name} ID ={item.ID}!");
                return false;
            }
            return base.AddSingleItem(item, ref sErr);
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

        //public IEnumerator<MotorSetting> GetEnumerator()
        //{
        //    return this.ItemCollection.GetEnumerator();
        //}

       
    }
}