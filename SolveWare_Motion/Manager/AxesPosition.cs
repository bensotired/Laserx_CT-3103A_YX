using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System.Collections.Generic;

namespace SolveWare_Motion
{

    public partial class AxesPosition : CURDBase<AxisPosition>, ICURDItem
    {

        public AxesPosition()
        {
            this.ItemCollection = new List<AxisPosition>();
            this.ID = IdentityGenerator.IG.GetIdentity();
        }

        public long ID { get  ; set ; }
        public string Name { get; set; }

        public AxesPosition Reverse()
        {
            AxesPosition revAp = new AxesPosition();
            revAp.Name = $"rev_{this.Name}";
            for(int i = this.ItemCollection.Count - 1; i >= 0; i--)
            {
                revAp.AddSingleItem(this.ItemCollection[i]);
            }
            return revAp;

        }
    }
}