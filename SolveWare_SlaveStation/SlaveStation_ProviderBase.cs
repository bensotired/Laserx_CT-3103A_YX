using LX_BurnInSolution.Utilities;
using SolveWare_BurnInCommon;
using System.IO;

namespace SolveWare_SlaveStation
{
    public class SlaveStation_ProviderBase
    {
        public const string ConfigFileName = @"SlaveStations.xml";
        public SlaveStation_ProviderBase()
        {
            Configs = new DataBook<string, SlaveStation_Config>();
        }
        public DataBook<string, SlaveStation_Config> Configs { get; set; }


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
        public static TSlaveStation_Provider Load<TSlaveStation_Provider>(string path) where TSlaveStation_Provider : SlaveStation_ProviderBase
        {
            return XmlHelper.DeserializeFile<TSlaveStation_Provider>(path);
        }
    }

}
