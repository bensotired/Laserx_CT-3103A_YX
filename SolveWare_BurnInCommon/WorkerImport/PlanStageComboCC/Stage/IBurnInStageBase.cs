using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{

    public interface IBurnInStageBase/*<TContinuityCheckItem>*/ : IBurnInStageLite
    {
        double Duration_Hours { get; set; }
        double SamplingInterval_Mins { get; set; }
        double SoakingTime_Mins { get; set; }
    }
}