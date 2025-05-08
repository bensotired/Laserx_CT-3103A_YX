using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using System;
using System.IO;

namespace SolveWare_Vision
{

    [Serializable]
    public class VisionManagerConfig : ITesterAppConfig
    {
        static VisionManagerConfig _instance;
        static object _mutex = new object();
        public static VisionManagerConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new VisionManagerConfig();
                        }
                    }
                }
                return _instance;
            }
        }
        public VisionManagerConfig()
        {
            VisionCMDBook = new DataBook<int, string>();
        }
        public DataBook<int, string> VisionCMDBook { get; set; }



        public void Load(string configFile)
        {

            if (File.Exists(configFile) == false)
            {
                if (Directory.Exists(Path.GetDirectoryName(configFile)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(configFile));
                }
                this.CreateDefaultInstance();
                XmlHelper.SerializeFile<VisionManagerConfig>(configFile, _instance);
            }
            else
            {
                _instance = XmlHelper.DeserializeFile<VisionManagerConfig>(configFile);
            }
        }
        public void Save(string configFile)
        {
            if (File.Exists(configFile) == false)
            {
                if (Directory.Exists(Path.GetDirectoryName(configFile)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(configFile));
                }
                this.CreateDefaultInstance();
            }
            XmlHelper.SerializeFile<VisionManagerConfig>(configFile, _instance);
        }

        //虚拟增加五个IO点
        public void CreateDefaultInstance()
        {
            _instance.VisionCMDBook = new DataBook<int, string>();
            for (int cmdIndex = 1; cmdIndex <= 5; cmdIndex++)
            {
                _instance.VisionCMDBook.Add(cmdIndex, $"vision CMD {cmdIndex}");
            }

        }
    }
}