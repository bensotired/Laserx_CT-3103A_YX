using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_BurnInInstruments
{
    public class PowerWavelength
    {
        /// <summary>
        /// Initializes an instance.
        /// </summary>
        public PowerWavelength()
        {
        }
        /// <summary>
        /// Gets or sets wavelenth in nm.
        /// </summary>
        public double Wavelength_nm { get; set; }

        /// <summary>
        /// Gets or sets power in dBm.
        /// </summary>
        public double Power_dBm { get; set; }
    }
}