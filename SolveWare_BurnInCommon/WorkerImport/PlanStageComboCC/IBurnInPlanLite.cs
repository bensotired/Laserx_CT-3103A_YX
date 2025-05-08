using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{
    public interface IBurnInPlanLite
    {
        string CreateTime { get; set; }
        string LastModifyTime { get; set; }
        string Name { get; set; }
        //string ProductType { get; set; }
        List<IContinuityCheckItem> CommonCCSteps { get; }
        List<object> CommonCCStepObjects { get; }
        List<IBurnInStageLite> CommonStages { get; }
        List<object> CommonStageObjects { get; }
        void SetupCCSteps(object sourObj);
        void SetupStages(object sourObj);
        void AddCCStep();
        void AddStage();
        void Save(string path);

        bool Check(out string checkLog);
    }
}