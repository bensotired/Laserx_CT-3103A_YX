using SolveWare_BurnInCommon;

namespace SolveWare_BurnInCommon
{
    public interface IWorkerImportData
    {
        //object ImportPlan { get; }
        BurnInPlanLite ImportPlan { get; }
        UnitSlotsInfo UnitSlotsInfo_Original { get; } 
        UnitSlotsInfo UnitSlotsInfo_RunTime { get; }
    }
}