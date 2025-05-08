using SolveWare_Vision;
using System;
using System.IO;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    public enum PPM_Enum_CT3103
    {
        //20230602
        相机1_PPM,
        相机2_PPM,
    }

    public class PPM_Provider : PPM_ProviderBase
    {
        public PPM_Provider() : base()
        {
            this.Add(PPM_Enum_CT3103.相机1_PPM, new PixPerMM());
            this.Add(PPM_Enum_CT3103.相机2_PPM, new PixPerMM());
        }
        public void Add(PPM_Enum_CT3103 ppmName, PixPerMM ppm)
        {
            this.Add(ppmName.ToString(), ppm);
        }
        public PixPerMM this[PPM_Enum_CT3103 ppmName]
        {
            get
            {
                return this[ppmName.ToString()];
            }
        }
    }

}