using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public interface IInstrumentBase
    {
        string Address { get; }
        string Name { get; }
        bool IsOnline { get; }
        //bool IsSimulation { get; }
        object HandleAllocateChassisResouce();
        object HandleChassisOnline();
        object HandleChassisOffline();
        void GenerateFakeDataLoop(CancellationToken token);
        void GenerateFakeDataOnceCycle(CancellationToken token);
        void Initialize();
        void HandleGroupOperations(GroupOperation operaType);
        void RefreshDataLoop(CancellationToken token);
        void RefreshDataOnceCycle(CancellationToken token);
        void TurnOnline(bool isOnline);
        //void EnableSimulation(bool isEnable);
        void WaitRuning();
        //void ResetToken();
        //void CancelRefreshing();
    }
}