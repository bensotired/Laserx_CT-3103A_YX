using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_BurnInInstruments
{
    public class PowerWavelengthTrace : IEnumerable<PowerWavelength>
    {
        List<PowerWavelength> PowerWavelenghList { get; set; }
        public double StartWavelength_nm { get; set; }
        public double StopWavelength_nm { get; set; }


        public PowerWavelengthTrace()
        {
            PowerWavelenghList = new List<PowerWavelength>();
        }

        public void Add(PowerWavelength item)
        {
            PowerWavelenghList.Add(item);
        }

        public PowerWavelength this[int index]
        {
            get
            {
                return this.PowerWavelenghList[index];
            }
        }

        public void Remove(int index)
        {
            this.Remove(index);
        }

        public IEnumerator<PowerWavelength> GetEnumerator()
        {
            return this.PowerWavelenghList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.PowerWavelenghList.GetEnumerator();
        }

        public int Count
        {
            get { return this.PowerWavelenghList.Count; }
        }

        public double[] GetWavelengthes()
        {
            if (this.PowerWavelenghList.Count <= 0)
            {
                return null;
            }

            double[] Wavelengthes_nm = new double[this.PowerWavelenghList.Count];
            for (int i = 0; i < this.PowerWavelenghList.Count; i++)
            {
                Wavelengthes_nm[i] = this[i].Wavelength_nm;
            }
            return Wavelengthes_nm;
        }
        public double[] GetPowers()
        {
            if (this.PowerWavelenghList.Count <= 0)
            {
                return null;
            }
            double[] Powers_dBm = new double[this.PowerWavelenghList.Count];
            for (int i = 0; i < this.PowerWavelenghList.Count; i++)
            {
                Powers_dBm[i] = this[i].Power_dBm;
            }
            return Powers_dBm;
        }
        public PowerWavelengthTrace SubTrace(double startWavelength_nm, double stopWavelength_nm)
        {
            PowerWavelengthTrace trace = new PowerWavelengthTrace();
            trace.StartWavelength_nm = startWavelength_nm;
            trace.StopWavelength_nm = stopWavelength_nm;
            foreach (PowerWavelength powerWavelength in this.PowerWavelenghList)
            {
                if ((powerWavelength.Wavelength_nm >= startWavelength_nm) && (powerWavelength.Wavelength_nm <= stopWavelength_nm))
                {
                    trace.Add(powerWavelength);
                }
            }
            return trace;
        }
    }
 
   
}
