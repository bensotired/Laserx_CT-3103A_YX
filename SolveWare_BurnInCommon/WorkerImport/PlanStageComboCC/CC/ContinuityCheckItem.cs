using MessagePack;
using System;

namespace SolveWare_BurnInCommon
{
    [Serializable]
    [MessagePackObject(keyAsPropertyName: true)]
    public class ContinuityCheckItem : IContinuityCheckItem
    {
        public ContinuityCheckItem()
        {

        }
   
    }
}