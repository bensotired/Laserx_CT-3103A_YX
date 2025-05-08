using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_IO
{

    [Serializable]
    public class IOManagerConfig : ITesterAppConfig
    {

        static IOManagerConfig _instance;
        static object _mutex = new object();
        public static IOManagerConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new IOManagerConfig();
                        }
                    }
                }
                return _instance;
            }
        }
        public IOManagerConfig()
        {

        }


        [Category("Motor Config")]
        [Description("Simulation")]
        [DisplayName("摸拟")]
        public bool Simulation { get; set; } = true;

        [Category("IO Config")]
        [Description("IO General Setting")]
        [DisplayName("IO设置列表")]
        public IOSettingCollection IOGeneralSetting { get; set; }

        public void Load(string configFile)
        {

            if (File.Exists(configFile) == false)
            {
                if (Directory.Exists(Path.GetDirectoryName(configFile)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(configFile));
                }
                this.CreateDefaultInstance();
                XmlHelper.SerializeFile<IOManagerConfig>(configFile, _instance);
            }
            else
            {
                _instance = XmlHelper.DeserializeFile<IOManagerConfig>(configFile);
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
            XmlHelper.SerializeFile<IOManagerConfig>(configFile, _instance);
        }

        //虚拟增加五个IO点
        public void CreateDefaultInstance()
        {
            _instance = new IOManagerConfig();
            _instance.IOGeneralSetting = new IOSettingCollection();

            string errMsg = string.Empty;

            short IONo = 18;
            var motorSettingItem = new IOSetting();
            motorSettingItem.CardNo = 1;
            motorSettingItem.SlaveNo = 1;
            motorSettingItem.Bit = IONo;
            motorSettingItem.IOType = IOType.INPUT;
            motorSettingItem.Name = $"IO_{IOType.INPUT.ToString()}_{IONo}";
            _instance.IOGeneralSetting.AddSingleItem(motorSettingItem, ref errMsg);

        }
        //[Category("Motor Data Config")]
        //[Description("Motion Data List")]
        //[DisplayName("Teach Pos 列表")]
        //public MotionDataConfig MotionDataCfg { get; set; }
    }
}
