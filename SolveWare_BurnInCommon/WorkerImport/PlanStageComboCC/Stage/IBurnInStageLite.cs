using System.Collections.Generic;

namespace SolveWare_BurnInCommon
{
    public interface IBurnInStageLite
    {
        int ID { get; set; }
 
        object CreateInstance();
    }
}