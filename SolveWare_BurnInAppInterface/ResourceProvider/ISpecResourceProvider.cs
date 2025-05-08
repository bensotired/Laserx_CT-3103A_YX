using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;

namespace SolveWare_BurnInAppInterface
{

    public interface ISpecResourceProvider  
    {
        object GetSpecResource(string specTag);
        List<string> GetSpecificationTags();
    }
}