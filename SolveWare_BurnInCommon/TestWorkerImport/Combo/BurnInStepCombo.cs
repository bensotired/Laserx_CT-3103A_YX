using LX_BurnInSolution.Utilities;
using MessagePack;
using System;
using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class TestStepCombo
    {
        public TestStepCombo()
        {
            this.MainStreamCombo = new List<string>();
            this.Name = "StepCombo";
            this.ProductType = "NoUse";
            this.CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.LastModifyTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public virtual string CreateTime { get; set; }
        public virtual string LastModifyTime { get; set; }
        public virtual string Name { get; set; }
        public virtual string ProductType { get; set; }
        public virtual string ApplicableTestPlugin { get; set; }
        public List<string> MainStreamCombo { get; set; }

        public virtual void Save(string path)
        {
            this.LastModifyTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            XmlHelper.SerializeFile<TestStepCombo>(path, this);
        }
    }
    [MessagePackObject(keyAsPropertyName: true)]
    public class TestStepComboInstance:TestStepCombo
    {
          public List<executorconfi>
    }
}