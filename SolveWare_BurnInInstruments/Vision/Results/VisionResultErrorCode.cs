using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public enum VisionResultErrorCode : int
    {
        NoError = 0,
        General = 1,
        CameraDisconnected = 2,
    }
}