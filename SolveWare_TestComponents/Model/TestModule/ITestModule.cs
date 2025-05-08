using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TestComponents.Model
{

    public interface ITestModule : ITestExecutiveMember, ITesterCoreLink, ITestExecutorRuntime
    {
        //Dictionary<Type, string> GetRequireInstruments();
        string Name { get; set; }
        Type GetTestRecipeType();
        //  bool SetupInstruments(DataBook<string, string> userDefineInstrumentConfig, Dictionary<string, IInstrumentBase> unitInstruments);
        bool SetupResources
               (
                   DataBook<string, string> userDefineInstrumentConfig,
                   DataBook<string, string> userDefineAxisConfig,
                   DataBook<string, string> userDefinePositionConfig,
                   ITestPluginResourceProvider resourceProvider
               );
        void Localization(ITestRecipe testRecipe);
        IRawDataBaseLite CreateRawData();
        void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData);
        void RunRreAction(CancellationToken token);
        void RunPostAction(CancellationToken token);
        //void Run(ref IRawDataBaseLite rawData, CancellationToken token);
        void Run(CancellationToken token);

    }
}