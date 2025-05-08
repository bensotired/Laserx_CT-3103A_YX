using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{
    public interface IBurnInPlanBase<TStage,TContinuityCheckItem> :IBurnInPlanLite where TStage : class
         where TContinuityCheckItem : class
    {
        List<TStage> Stages { get; set; }
        List<TContinuityCheckItem> CCSteps { get; set; }
    }
}