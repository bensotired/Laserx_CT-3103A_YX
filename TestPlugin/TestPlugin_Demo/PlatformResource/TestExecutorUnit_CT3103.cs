using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System.Collections.Generic;

namespace TestPlugin_Demo
{
    /// <summary>
    /// 测试执行者   每个就是一个复合测试头
    /// </summary>
    public class TestExecutorUnit_CT3103 : TestExecutorUnit
    {
        public TestExecutorUnit_CT3103(string name, TestExecutorUnitConfig unitConf/*, ITestPluginResourceProvider resourceProvider*/)
            : base(name, unitConf/*, resourceProvider*/)
        {
        }

    }
}