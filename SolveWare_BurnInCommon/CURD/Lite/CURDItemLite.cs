using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    [Serializable]
    public class CURDItemLite : ICURDItemLite  //: IModel, IRESTFul<TData>
    {
        public CURDItemLite()
        {
            Name = string.Empty;
        }
        [PropEditableIndexer(true, 0)]
        public string Name { get; set; }
    }

}
