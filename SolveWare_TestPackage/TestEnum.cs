using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_TestPackage
{
    public enum Section_Curr
    {
        GAIN,
        SOA1,
        SOA2,
        MIRROR1,
        MIRROR2,
        PH1,
        PH2,
        LP,
    }
    public enum Section_Volt
    {
        GAIN,
        SOA1,
        SOA2,
        MIRROR1,
        MIRROR2,
        PH1,
        PH2,
        LP,
        MPD1,
        MPD2,
        BIAS1,
        BIAS2
    }
    public enum Section
    {
        PD,
        SOA1,
        SOA2,
        LP,
        PH1,
        PH2,
        MIRROR1,
        MIRROR2,
        BIAS1,
        BIAS2,
        MPD1,
        MPD2,
        GAIN,

    }
    public enum PH_MPD
    {
        MPD1,
        MPD2,
    }
}
