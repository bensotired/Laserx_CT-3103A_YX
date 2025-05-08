using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Specification;
using System.Collections.Generic;

namespace SolveWare_TestComponents.Data
{

    public class TestPluginImportProfile_MultiComboWithTestParams : TestPluginImportProfileBase
    {
        public TestPluginImportProfile_MultiComboWithTestParams()
        {
            TestExecutorComboDict = new DataBook<string, TestExecutorCombo>();
            TestParamsConfigComboDict = new DataBook<string, ExecutorConfigItem_TestParamsConfigCombo>();
        }
 
        public TestSpecification Spec { get; set; }
        public DataBook<string ,TestExecutorCombo> TestExecutorComboDict { get; set; }
        public DataBook<string, ExecutorConfigItem_TestParamsConfigCombo> TestParamsConfigComboDict { get; set; }
    }
}