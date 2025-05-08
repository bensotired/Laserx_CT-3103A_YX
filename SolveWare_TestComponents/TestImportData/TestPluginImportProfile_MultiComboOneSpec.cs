using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Specification;
using System.Collections.Generic;

namespace SolveWare_TestComponents.Data
{

    public class TestPluginImportProfile_MultiComboOneSpec : TestPluginImportProfileBase
    {
        public TestPluginImportProfile_MultiComboOneSpec()
        {
            TestExecutorComboDict = new DataBook<string, TestExecutorCombo>();
        }
 
        public TestSpecification Spec { get; set; }
        public DataBook<string ,TestExecutorCombo> TestExecutorComboDict { get; set; }
    }
}