using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SolveWare_TestComponents.Model
{
    public interface ITestCalculator : ITestExecutiveMember, ITesterCoreLink, ITestExecutorRuntime
    {
        Type GetCalcRecipeType();
        void Localization(ICalcRecipe testRecipe);
        void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token);
    }
}