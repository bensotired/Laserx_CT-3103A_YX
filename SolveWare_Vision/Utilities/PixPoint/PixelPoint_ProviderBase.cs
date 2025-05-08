using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System.IO;

namespace SolveWare_Vision
{
    public class PixelPoint_ProviderBase
    {
        public const string ConfigFileName = @"pixPoint.xml";
        public PixelPoint_ProviderBase()
        {
            this.PixPointDict = new DataBook<string, PixPoint>();
        }

        public DataBook<string, PixPoint> PixPointDict { get; set; }

        protected virtual void Add(string ppmName, PixPoint ppm)
        {
            if (this.PixPointDict.ContainsKey(ppmName))
            {
                this.PixPointDict[ppmName] = ppm;
            }
            else
            {
                this.PixPointDict.Add(ppmName, ppm);
            }
        }
        protected virtual PixPoint this[string ppmName]
        {
            get
            {
                if (this.PixPointDict.ContainsKey(ppmName))
                {
                    return this.PixPointDict[ppmName];
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
         
            }
            XmlHelper.SerializeFile (path, this);
        }
        public static TPixelPoint_Provider Load<TPixelPoint_Provider>(string path)where TPixelPoint_Provider : PixelPoint_ProviderBase
        {
            return XmlHelper.DeserializeFile<TPixelPoint_Provider>(path);
        }
    }
}
