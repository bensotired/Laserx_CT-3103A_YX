using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_BurnInInstruments
{
    /// <summary>
    /// A class contains SMSR results.
    /// </summary>
    public class OsaSmsrResults
    {
        public OsaSmsrResults()
        {

        }
        /// <summary>
        /// Gets or sets SMSR in dB.
        /// </summary>
        public double Smsr_dB { get; set; }
        /// <summary>
        /// Gets or sets the side mode frequency offset in nm.
        /// </summary>
        public double SideModeFrequencyOffset_nm { get; set; }

        /// <summary>
        /// Gets or sets the wavelength at the peak in nm.
        /// </summary>
        public double PeakWavelength_nm { get; set; }

        /// <summary>
        /// Gets or sets the power at the peak in dBm.
        /// </summary>
        public double PeakPower_dBm { get; set; }

        /// <summary>
        /// Gets or sets the wavelenth at the second peak in nm.
        /// </summary>
        public double SecondPeakWavelenth_nm { get; set; }

        /// <summary>
        /// Gets or sets the power at the second peak in dBm.
        /// </summary>
        public double SecondPeakPower_dBm { get; set; }
    }
}