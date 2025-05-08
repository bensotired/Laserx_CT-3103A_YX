using System;
using System.Collections.Generic;

namespace SolveWare_TestComponents.Data
{
    public interface IStreamInfoBase
    {
        string CreateTime { get; set; }
        string Information { get; set; }
        Type[] GetIncludingTypes();
        void Save(string filePath);
        object Load(string path, Func<List<string>, List<Type>> convertFunc);
    }
}