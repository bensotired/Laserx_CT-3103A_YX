using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SolveWare_BinSorter
{

    [Serializable]
    public class BinSetting : CURDBaseLite<BinJudgeItem>, ICURDItemLite
    {
        public string Name { get; set; }
        //public string Version { get; set; }
        [XmlIgnore]
        public string BinTag
        {
            get
            {
                return $"{this.Name}";
                //return $"{this.Name} - ver.{Version}";
            }
        }
        public BinSetting()
        {

        }
        public BinSetting(BinSetting source):this()
        {
            foreach(var item in source.ItemCollection)
            {
                this.ItemCollection.Add(item);
            }
        }
        public static List<BinJudgeItem> CreateDefaultCollection()
        {
            List<BinJudgeItem> tempList = new List<BinJudgeItem>();
            tempList.Add(new BinJudgeItem { Name = "Ith2", Visible = true, Enable = true, LL = 0.0, UL = 60.0, Unit = "mA" });
            tempList.Add(new BinJudgeItem { Name = "Pf1", Visible = true, Enable = true, LL = 25.0, UL = 300.0, Unit = "mW" });
            tempList.Add(new BinJudgeItem { Name = "Pf3", Visible = true, Enable = true, LL = 0.0, UL = 300.0, Unit = "mW" });
            tempList.Add(new BinJudgeItem { Name = "Vf1", Visible = true, Enable = true, LL = 0.0, UL = 8.5, Unit = "V" });
            tempList.Add(new BinJudgeItem { Name = "Vf3", Visible = true, Enable = true, LL = 5.0, UL = 15.0, Unit = "V" });
            tempList.Add(new BinJudgeItem { Name = "Pmax", Visible = true, Enable = true, LL = 20.0, UL = 300.0, Unit = "mW" });
            tempList.Add(new BinJudgeItem { Name = "Isa", Visible = true, Enable = true, LL = 99.0, UL = 100.0, Unit = "mA" });
            tempList.Add(new BinJudgeItem { Name = "Ir", Visible = true, Enable = true, LL = 0.0, UL = 0.0, Unit = "uA" });
            tempList.Add(new BinJudgeItem { Name = "λp1", Visible = true, Enable = true, LL = 480.0, UL = 520.0, Unit = "nm" });
            return tempList;
        }
        public bool Check(out string errMsg)
        {
            bool isok = true;
            errMsg = string.Empty;
            if (string.IsNullOrEmpty(Name))
            {
                errMsg += $"BinSetting Name [{nameof(Name)}] is empty!{Environment.NewLine}";
                isok = false;
            }
            //if (string.IsNullOrEmpty(Version))
            //{
            //    errMsg += $"BinSetting Version [{nameof(Version)}] is empty!{Environment.NewLine}";
            //    isok = false;
            //}
            foreach (var item in this.ItemCollection)
            {
                if (item.Check() == false)
                {
                    errMsg += $"BinJudgeItem [{item.Name}] is invalid!{Environment.NewLine}";
                    isok = false;
                }
            }
            return isok;
        }
    }
}