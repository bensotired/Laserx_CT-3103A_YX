using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestComponents.Model
{

    public interface ITestExecutorInteration 
    {
        string Name { get; }
        void UpdateCurrentExecutorRuntimeInfo(string deviceSN);
        

    }
}