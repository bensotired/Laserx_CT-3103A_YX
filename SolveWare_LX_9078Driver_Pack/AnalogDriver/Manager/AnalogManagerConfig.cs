using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_Analog
{
    [Serializable]
    public class AnalogManagerConfig : ITesterAppConfig
    {
        //private static AnalogManagerConfig _instance;
        private static object _mutex = new object();

        //public static AnalogManagerConfig Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (_mutex)
        //            {
        //                if (_instance == null)
        //                {
        //                    _instance = new AnalogManagerConfig();
        //                }
        //            }
        //        }
        //        return _instance;
        //    }
        //}

        public AnalogManagerConfig()
        {
            this.AnalogGeneralSetting = new AnalogSettingCollection();
        }

        [Category("Motor Config")]
        [Description("Simulation")]
        [DisplayName("摸拟")]
        public bool Simulation { get; set; } = true;

        [Category("Analog Config")]
        [Description("Analog General Setting")]
        [DisplayName("Analog设置列表")]
        public AnalogSettingCollection AnalogGeneralSetting { get; set; }

        public void Load(string configFile)
        {
            if (File.Exists(configFile) == false)
            {
                if (Directory.Exists(Path.GetDirectoryName(configFile)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(configFile));
                }
                this.CreateDefaultInstance();
                XmlHelper.SerializeFile<AnalogManagerConfig>(configFile, this);
            }
            else
            {
                var obj = XmlHelper.DeserializeFile<AnalogManagerConfig>(configFile);
                this.Simulation = obj.Simulation;
                this.AnalogGeneralSetting = obj.AnalogGeneralSetting;
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
            XmlHelper.SerializeFile<AnalogManagerConfig>(configFile, this);
        }

        //虚拟增加五个IO点
        public void CreateDefaultInstance()
        {
            //_instance = new AnalogManagerConfig();
            this.AnalogGeneralSetting = new AnalogSettingCollection();

            string errMsg = string.Empty;

            short IONo = 18;
            var motorSettingItem = new AnalogSetting();
            motorSettingItem.CardNo = 1;
            motorSettingItem.SlaveNo = 1;
            motorSettingItem.Bit = IONo;
            motorSettingItem.AnalogType = AnalogType.ADC;
            motorSettingItem.Name = $"IO_{AnalogType.ADC.ToString()}_{IONo}";
            this.AnalogGeneralSetting.AddSingleItem(motorSettingItem, ref errMsg);
        }

        //[Category("Motor Data Config")]
        //[Description("Motion Data List")]
        //[DisplayName("Teach Pos 列表")]
        //public MotionDataConfig MotionDataCfg { get; set; }
    }
}