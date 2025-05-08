using MessagePack;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SolveWare_BurnInCommon
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class BurnInPlanLite : IBurnInPlanLite
    {
        public BurnInPlanLite()
        {
            this.Name = "Plan";
            //this.ProductType = "NoUse";
            this.CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.LastModifyTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
        [PropEditable(false)]
   
        public virtual string CreateTime { get; set; }
        [PropEditable(false)]
     
        public virtual string LastModifyTime { get; set; }
        [PropEditable(true)]
     
        public virtual string Name { get; set; }
        //[PropEditable(true)]
        //public virtual string ProductType { get; set; }
        public virtual void AddStage()
        {
            throw new NotImplementedException();
        }
        public virtual void AddCCStep()
        {
            throw new NotImplementedException();
        }
        public virtual void Save(string path)
        {
            throw new NotImplementedException();
        }

        public virtual void SetupCCSteps(object sourObj)
        {
            throw new NotImplementedException();
        }
        public virtual void SetupStages(object sourObj)
        {
            throw new NotImplementedException();
        }
        public virtual bool Check(out string checkLog)
        {
            checkLog = "";
            return true;
        }

        [XmlIgnore]
        [PropEditable(false)]
        [IgnoreMember]
        public virtual List<IBurnInStageLite> CommonStages
        {
            get;
        }
        [XmlIgnore]
        [IgnoreMember]
        public virtual List<object> CommonStageObjects
        {
            get;
        }
        [XmlIgnore]
        [PropEditable(false)]
        [IgnoreMember]
        public virtual List<IContinuityCheckItem> CommonCCSteps
        {
            get;
        }
        [XmlIgnore]
        [IgnoreMember]
        public virtual List<object> CommonCCStepObjects
        {
            get;
        }
    }
}