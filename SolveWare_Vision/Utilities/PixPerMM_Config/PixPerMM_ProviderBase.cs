using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System.IO;

namespace SolveWare_Vision
{

    public class PPM_ProviderBase
    {
        public const string ConfigFileName = @"ppm.xml";
        public PPM_ProviderBase()
        {
            this.PpmDict = new DataBook<string, PixPerMM>();
        }

        public DataBook<string, PixPerMM> PpmDict { get; set; }

        protected virtual void Add(string ppmName, PixPerMM ppm)
        {
            if (this.PpmDict.ContainsKey(ppmName))
            {
                this.PpmDict[ppmName] = ppm;
            }
            else
            {
                this.PpmDict.Add(ppmName, ppm);
            }
        }
        protected virtual PixPerMM this[string ppmName]
        {
            get
            {
                if (this.PpmDict.ContainsKey(ppmName))
                {
                    return this.PpmDict[ppmName];
                }
                else
                {
                    return null;
                }
            }
        }
        public virtual void Save(string path)
        {
            if (File.Exists(path) == false)
            {
                if (Directory.Exists(Path.GetDirectoryName(path)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                //this.CreateDefaultInstance();
            }
            XmlHelper.SerializeFile(path, this);
        }
        public static TPPM_Provider Load<TPPM_Provider>(string path) where TPPM_Provider : PPM_ProviderBase
        {
            return XmlHelper.DeserializeFile<TPPM_Provider>(path);
        }
    }
}