
using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SolveWare_BurnInCommon
{

    public class BurnInSweepPlanBase<TStage, TSweepItem, TContinuityCheckItem> : BurnInSweepPlanLite, IBurnInSweepPlanLite
        where TStage : class, IBurnInStageLite
        where TSweepItem : class, ISweepItem
        where TContinuityCheckItem : class, IContinuityCheckItem

    {
        public BurnInSweepPlanBase() : base()
        {
            this.Stages = new List<TStage>();
            this.CCSteps = new List<TContinuityCheckItem>();
            this.SweepSteps = new List<TSweepItem>();

        }
        #region CCSteps
        public List<TContinuityCheckItem> CCSteps { get; set; }
        [XmlIgnore]
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
        public List<TStage> Stages { get; set; }
        [XmlIgnore]

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
        #region SweepSteps
        public override void AddSweepStep()
        {
            throw new NotImplementedException();
        }
        public List<TSweepItem> SweepSteps { get; set; }
        [XmlIgnore]
        public override List<ISweepItem> CommonSweepSteps
        {
            get
            {
                List<ISweepItem> sitems = new List<ISweepItem>();
                SweepSteps.ForEach(sitem => sitems.Add(sitem));
                return sitems;
            }
        }
        [XmlIgnore]

        public override List<object> CommonSweepStepObjects
        {
            get
            {
                List<object> sitems = new List<object>();
                SweepSteps.ForEach(sitem => sitems.Add(sitem));
                return sitems;
            }
        }


        public override void SetupSweepSteps(object sourObj)
        {

            List<Dictionary<string, object>> sourDictList = sourObj as List<Dictionary<string, object>>;

            this.SweepSteps.Clear();
            int stageId = 0;
            foreach (var sourDict in sourDictList)
            {
                var destItem = AssemblyManager.CreateInstance<TSweepItem>($"{typeof(TSweepItem).Namespace}.{typeof(TSweepItem).Name}");
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
                this.SweepSteps.Add(destItem);
            }
        }
        #endregion
    }
}