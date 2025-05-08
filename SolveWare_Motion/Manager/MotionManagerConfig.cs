using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using System;
using System.ComponentModel;
using System.IO;

namespace SolveWare_Motion
{

    [Serializable]
    public class MotionManagerConfig : ITesterAppConfig
    {

        //static MotionManagerConfig _instance;
        static object _mutex = new object();
        //public static MotionManagerConfig Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (_mutex)
        //            {
        //                if (_instance == null)
        //                {
        //                    _instance = new MotionManagerConfig();
        //                }
        //            }
        //        }
        //        return _instance;
        //    }
        //}
        public MotionManagerConfig()
        {
            this.MotorGeneralSetting = new MotorSettingCollection();
        }




        [Category("Motor Config")]
        [Description("Axes Count")]
        [DisplayName("总轴数")]
        public int AxesCount { get; set; }

        [Category("Motor Config")]
        [Description("MasterDriver")]
        [DisplayName("MasterDriver")]
        public MotorMasterDriver MasterDriver { get; set; } = MotorMasterDriver.LeadSide_DMC;

        [Category("Motor Config")]
        [Description("Simulation")]
        [DisplayName("摸拟")]
        public bool Simulation { get; set; } = true;

        [Category("Motor Config")]
        [Description("Motor General Setting")]
        [DisplayName("马达设置列表")]
        public MotorSettingCollection MotorGeneralSetting { get; set; }

        public void  Load(string configFile)
        {

            if (File.Exists(configFile) == false)
            {
                if (Directory.Exists(Path.GetDirectoryName(configFile)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(configFile));
                }
                this.CreateDefaultInstance();
                XmlHelper.SerializeFile<MotionManagerConfig>(configFile, this);
            }
            else
            {
               var obj  = XmlHelper.DeserializeFile<MotionManagerConfig>(configFile);
                this.AxesCount = obj.AxesCount;
                this.MasterDriver = obj.MasterDriver;
                this.Simulation = obj.Simulation;
                this.MotorGeneralSetting = obj.MotorGeneralSetting;
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
            XmlHelper.SerializeFile<MotionManagerConfig>(configFile, this);
        }

        public void CreateDefaultInstance()
        {
            //_instance = new MotionManagerConfig();
            this.MotorGeneralSetting = new MotorSettingCollection();

            const int defaultMasterId = 1;
            const int defaultAxesCount = 10;

            string errMsg = string.Empty;

            for (short axisNo = 1; axisNo <= defaultAxesCount; axisNo++)
            {
                var motorSettingItem = new MotorSetting();
                motorSettingItem.MotorTable.AxisNo = axisNo;
                motorSettingItem.MotorTable.CardNo = defaultMasterId;
                motorSettingItem.Name = $"AXIS_{axisNo}";
                this.MotorGeneralSetting.AddSingleItem(motorSettingItem, ref errMsg);
            }
        }
    }
}