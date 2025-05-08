using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestComponents.Model
{
    public class TestExecutorUnitInteration : ITestExecutorUnitInteration
    {
        public string Name { get;   set; }
        public TestExecutorUnitStatus Status { get; internal set; }
    }
}