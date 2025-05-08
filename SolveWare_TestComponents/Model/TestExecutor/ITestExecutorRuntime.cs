using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestComponents.Model
{

    public interface ITestExecutorRuntime
    {
        //void LinkToExcecutor(ITestExecutorInteration exeInteration);
        void LinkToExcecutorUnit(ITestExecutorUnitInteration exeUnitInteration);
    }
}