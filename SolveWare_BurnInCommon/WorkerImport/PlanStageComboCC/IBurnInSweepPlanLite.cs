using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{
    public interface IBurnInSweepPlanLite : IBurnInPlanLite
    {
        void AddSweepStep();
        void SetupSweepSteps(object sourObj);
        List<ISweepItem> CommonSweepSteps { get; }
        List<object> CommonSweepStepObjects { get; }
    }
}