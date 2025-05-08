
using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SolveWare_BurnInCommon
{
    public class BurnInSweepPlanLite : BurnInPlanLite, IBurnInSweepPlanLite
    {
        public BurnInSweepPlanLite() : base()
        {

        }
        public virtual void AddSweepStep()
        {
            throw new NotImplementedException();
        }
        public virtual void SetupSweepSteps(object sourObj)
        {
            throw new NotImplementedException();
        }
        [XmlIgnore]
        [PropEditable(false)]
        public virtual List<ISweepItem> CommonSweepSteps
        {
            get;
        }
        [XmlIgnore]
        public virtual List<object> CommonSweepStepObjects
        {
            get;
        }
    }
}