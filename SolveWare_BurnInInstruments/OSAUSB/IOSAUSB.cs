using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public interface IOSAUSB : IInstrumentBase
    {
        void InitialAndSupprtReconnct();
        void SetParameters(int integrationTime, int BoxcarWidth, int ScansToAverage);
        double[] GetOpticalData();
        double[] GetWaveLength();
        int GetSpectrometersCount();
    }
}