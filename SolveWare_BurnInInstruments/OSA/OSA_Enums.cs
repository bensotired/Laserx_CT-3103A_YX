using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_BurnInInstruments
{
    public enum OsaTrace
    {
        TRA,
        TRB,
        TRC,
        TRD,
        TRE,
        TRF,
        TRG
    }
    public enum PowerUnit
    {
        /// <summary>
        /// dBm.
        /// </summary>
        dBm,
        /// <summary>
        /// W.
        /// </summary>
        W,
        /// <summary>
        /// mW.
        /// </summary>
        mW,

        /// <summary>
        /// dBm/nm
        /// </summary>
        dBmPerNm,
        /// <summary>
        /// W/nm
        /// </summary>
        WPerNm
    }
    public enum YokogawaAQ6370SweepModes
    {
        SegmentMeasure = 4,
        Auto = 3,
        Repeat = 2,
        Single = 1,
    }


    /// <summary>
    /// Sensitivity modes.
    /// </summary>
    public enum YokogawaAQ6370SensitivityModes
    {
        NormHold = 0,
        NormAuto = 1,
        Norm = 6,
        Mid = 2,
        High1 = 3,
        High2 = 4,
        High3 = 5
    }

    /// <summary>
    /// The measurement algorithm
    ///applied to noise level measurements made by
    ///the WDM analysis function.
    /// </summary>
    public enum YokogawaAQ6370NoiseAlgorithms
    {
        /// <summary>
        /// Auto fix.
        /// </summary>
        Afix = 0,
        /// <summary>
        /// Manual fix.
        /// </summary>
        Mfix = 1,
        /// <summary>
        /// Auto center.
        /// </summary>
        Acen = 2,
        /// <summary>
        /// Manual center.
        /// </summary>
        Mcen = 3,
        /// <summary>
        /// PIT.
        /// </summary>
        Pit = 4
    }

    /// <summary>
    /// The fitting function during
    ///level measurement applied to noise level
    ///measurements made by the WDM analysis
    ///function.
    /// </summary>
    public enum YokogawaAQ6370NoiseFittingAlgorithms
    {
        /// <summary>
        /// Linear.
        /// </summary>
        Linear = 0,
        /// <summary>
        /// Gauss.
        /// </summary>
        Gauss = 1,
        /// <summary>
        /// Lorenz.
        /// </summary>
        Lorenz = 2,
        /// <summary>
        /// 3rd poly.
        /// </summary>
        Poly3rd = 3,
        /// <summary>
        /// 4th poly.
        /// </summary>
        Poly4th = 4,
        /// <summary>
        /// 5th poly.
        /// </summary>
        Poly5th = 5
    }
}