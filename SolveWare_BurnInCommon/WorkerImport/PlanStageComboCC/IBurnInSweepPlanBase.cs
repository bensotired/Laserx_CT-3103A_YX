using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{
    public interface IBurnInSweepPlanBase<TStage, TSweepItem, TContinuityCheckItem> : IBurnInPlanBase<TStage, TContinuityCheckItem>, IBurnInSweepPlanLite, IBurnInPlanLite
        where TStage : class 
        where TSweepItem : class
        where TContinuityCheckItem : class
     
    {

        List<TSweepItem> SweepSteps { get; set; }
    }
}