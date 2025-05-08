using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;

namespace SolveWare_BurnInAppInterface
{

    public interface IIOResourceProvider  
    {
        object GetIO_Object(string name);
    }
}