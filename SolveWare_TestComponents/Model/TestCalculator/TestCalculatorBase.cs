using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SolveWare_TestComponents.Model
{

    public abstract class TestCalculatorBase : ITestCalculator
    {
        protected ITesterCoreInteration _core;
        protected ITestExecutorUnitInteration _exeUnitInteration;
        public string Name { get; protected set; }
        public string ExecutorName { get; set; }
        public TestCalculatorBase()
        {
        }
        public abstract Type GetCalcRecipeType();
        public virtual void ConnectToCore(ITesterCoreInteration core)
        {
            _core = core;
        }
        public virtual void DisconnectFromCore(ITesterCoreInteration core)
        {
            _core = null;
        }
        public void LinkToExcecutorUnit(ITestExecutorUnitInteration exeUnitInteration)
        {
            _exeUnitInteration = exeUnitInteration;
        }

        protected virtual TReturn ConvertObjectTo<TReturn>(object sourceObject)
        {
            return (TReturn)Converter.ConvertObjectTo(sourceObject, typeof(TReturn));
        }
        public abstract void Localization(ICalcRecipe testRecipe);
        public abstract void Run(IRawDataBaseLite rawData, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token);

      
    }
}