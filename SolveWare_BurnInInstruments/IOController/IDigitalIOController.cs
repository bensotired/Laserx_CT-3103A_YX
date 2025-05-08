namespace SolveWare_BurnInInstruments
{
    public interface IDigitalIOController:IInstrumentBase
    {
        float[] AD_Value { get; }
        bool[] InputStatus { get; }
        bool[] OutputStatus { get; }

        void OffChannel(int channel);
        bool DIOModBusFunc1Extend(int address);
        void DIOModBusFunc5Extend(int address, int val);
        void DIOModBusFunc6Extend(int address, int val);
        void FlashChannel(int channel, int interval);
        int GetChannelValue(int channel);
        float[] Get_AD_Value();
        void OnChannel(int channel);
    }
}