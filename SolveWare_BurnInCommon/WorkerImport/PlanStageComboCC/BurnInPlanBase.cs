
using LX_BurnInSolution.Utilities;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SolveWare_BurnInCommon
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class BurnInPlanBase<TStage, TContinuityCheckItem> : BurnInPlanLite, IBurnInPlanBase<TStage , TContinuityCheckItem>
        where TStage : class, IBurnInStageLite
        where TContinuityCheckItem : class, IContinuityCheckItem
    {
        public BurnInPlanBase() : base()
        {
            this.Stages = new List<TStage>();
            this.CCSteps = new List<TContinuityCheckItem>();
        }
        /// <summary>
        /// 需要在泛型类里按泛型实现
        /// </summary>
        /// <param name="path"></param>
        public override void Save(string path)
        {
            throw new NotImplementedException();
        }

        #region CCSteps
     
        public List<TContinuityCheckItem> CCSteps
        {
            get;
            set;
        }
        [XmlIgnore]
        [IgnoreMember]
        public override List<IContinuityCheckItem> CommonCCSteps
        {
            get
            {
                List<IContinuityCheckItem> ccs = new List<IContinuityCheckItem>();
                CCSteps.ForEach(cc => ccs.Add(cc));
                return ccs;
            }
        }
        [XmlIgnore]
        [IgnoreMember]
        public override List<object> CommonCCStepObjects
        {
            get
            {
                List<object> ccs = new List<object>();
                CCSteps.ForEach(cc => ccs.Add(cc));
                return ccs;
            }
        }
        /// <summary>
        /// 需要在泛型类里按泛型实现
        /// </summary>
        /// <param name="path"></param>
        public override void AddCCStep()
        {
            throw new NotImplementedException();
        }
        public override void SetupCCSteps(object sourObj)
        {
            List<Dictionary<string, object>> sourDictList = sourObj as List<Dictionary<string, object>>;

            this.CCSteps.Clear();
            foreach (var sourDict in sourDictList)
            {
                var ccItem = AssemblyManager.CreateInstance<TContinuityCheckItem>($"{typeof(TContinuityCheckItem).Namespace}.{typeof(TContinuityCheckItem).Name}");
                foreach (var prop in ccItem.GetType().GetProperties())
                {
                    if (sourDict.ContainsKey(prop.Name))
                    {
                        prop.SetValue(ccItem, Converter.ConvertObjectTo(sourDict[prop.Name], prop.PropertyType));
                    }
                    else
                    {
                        continue;
                    }
                }
                this.CCSteps.Add(ccItem);
            }
        }
        #endregion
        #region stages
      
        public List<TStage> Stages
        {
            get;
            set;
        }
        [XmlIgnore]
        [IgnoreMember]
        public override List<IBurnInStageLite> CommonStages
        {
            get
            {
                List<IBurnInStageLite> cstg = new List<IBurnInStageLite>();
                Stages.ForEach(stg => cstg.Add(stg));
                return cstg;
            }
        }
        [XmlIgnore]
        [IgnoreMember]
        public override List<object> CommonStageObjects
        {
            get
            {
                List<object> cstg = new List<object>();
                Stages.ForEach(stg => cstg.Add(stg));
                return cstg;
            }
        }
        /// <summary>
        /// 需要在泛型类里按泛型实现
        /// </summary>
        /// <param name="path"></param>
        public override void AddStage()
        {
            throw new NotImplementedException();
        }

        public override void SetupStages(object sourObj)
        {

            List<Dictionary<string, object>> sourDictList = sourObj as List<Dictionary<string, object>>;

            this.Stages.Clear();
            int stageId = 0;
            foreach (var sourDict in sourDictList)
            {
                var destItem = AssemblyManager.CreateInstance<TStage>($"{typeof(TStage).Namespace}.{typeof(TStage).Name}");
                foreach (var prop in destItem.GetType().GetProperties())
                {
                    if (sourDict.ContainsKey(prop.Name))
                    {
                        prop.SetValue(destItem, Converter.ConvertObjectTo(sourDict[prop.Name], prop.PropertyType));
                    }
                    else
                    {
                        continue;
                    }
                }
                destItem.ID = ++stageId;
                this.Stages.Add(destItem);
            }
        }
        #endregion

    }
}