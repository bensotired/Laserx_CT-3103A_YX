using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInCommon
{
    [Serializable]
    public class CURDItem : ICURDItem  //: IModel, IRESTFul<TData>
    {
        public CURDItem()
        {
            this.ID = IdentityGenerator.IG.GetIdentity();
        }
        public long ID { get; set; }
        public string Name { get; set; }
    }

}
