using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public interface IFuseIOController
    {
        void ConnectFuse(bool enable);
        bool[] Output { get; }
    }
}