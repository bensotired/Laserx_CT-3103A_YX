using LX_BurnInSolution.Utilities;
using SolveWare_TestComponents.Attributes;
using System;
using System.Linq;

namespace SolveWare_TestComponents.Data
{
    [Serializable]
    public class RawDataMenu_TED : RawDataMenuCollection<RawData_TED>
    {
        public RawDataMenu_TED()
        {

        }
        [RawDataBrowsableElement]
        public double FinishTemp_DegC { get; set; }

    }
}