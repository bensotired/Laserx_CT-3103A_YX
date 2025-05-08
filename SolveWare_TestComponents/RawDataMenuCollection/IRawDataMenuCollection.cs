using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestComponents
{
    public interface IRawDataMenuCollection: IRawDataCollectionBase
    {
        IEnumerable<IRawDataCollectionBase> GetDataMenuCollection();
    }
}
