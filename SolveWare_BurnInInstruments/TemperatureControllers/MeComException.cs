using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class MeComException : Exception
    {
        public MeComException() : base()
        { }
        public MeComException(string message) : base(message)
        { }
        public MeComException(string message, Exception innerEx) : base(message, innerEx)
        { }
    }
}