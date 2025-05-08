using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace SolveWare_Motion
{

    [Serializable]
    public class MotionPositionConfig  : ITesterAppConfig
    {
        //public const string ConfigFileName = @"PositionConfig.xml";    //变更为动态名
        //static MotionPositionConfig _instance;
        static object _mutex = new object();
        //public static MotionPositionConfig Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (_mutex)
        //            {
        //                if (_instance == null)
        //                {
        //                    _instance = new MotionPositionConfig();
        //                }
        //            }
        //        }
        //        return _instance;
        //    }
        //}
        public MotionPositionConfig()
        {

        }
        [Category("Position Config")]
        [Description("Motor General Position")]
        [DisplayName("点位设置列表")]
        //public List<AxesPosition> MotorGeneralPosition { get; set; }
        public AxesPositionCollection AxesPositionCollection { get; set; }
        public void Load(string configFile)
        {

            if (File.Exists(configFile) == false)
            {
                if (Directory.Exists(Path.GetDirectoryName(configFile)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(configFile));
                }
                this.CreateDefaultInstance();
                XmlHelper.SerializeFile<MotionPositionConfig>(configFile, this);
            }
            else
            {
                var obj = XmlHelper.DeserializeFile<MotionPositionConfig>(configFile);
                this.AxesPositionCollection = obj.AxesPositionCollection;
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
                //this.CreateDefaultInstance();
            }
            XmlHelper.SerializeFile<MotionPositionConfig>(configFile, this);
        }

        //创建默认的表格
        public void CreateDefaultInstance()
        {
           
     

            AxisPosition axisPosition = new AxisPosition();
            axisPosition.CardNo = "1";
            axisPosition.AxisNo = "1";
            axisPosition.Position = 1;
            axisPosition.Name = "1";

            AxisPosition axisPosition1 = new AxisPosition();
            axisPosition1.CardNo = "1";
            axisPosition1.AxisNo = "3";
            axisPosition1.Position = 1;
            axisPosition1.Name = "2";//轴名称


            AxesPosition axesPosition = new AxesPosition();
            axesPosition.AddSingleItem(axisPosition);
            axesPosition.AddSingleItem(axisPosition1);
            axesPosition.Name="123456";
            var a = Convert.ToInt64(DateTime.Now.Date.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmm") + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString());
            axesPosition.ID = a;

            AxisPosition axisPosition2 = new AxisPosition();
            axisPosition2.CardNo = "1";
            axisPosition2.AxisNo = "1";
            axisPosition2.Position = 1;
            axisPosition2.Name = "3";

            AxisPosition axisPosition3 = new AxisPosition();
            axisPosition3.CardNo = "1";
            axisPosition3.AxisNo = "3";
            axisPosition3.Position = 1;
            axisPosition2.Name = "4";

            AxesPosition axesPosition2 = new AxesPosition();
            axesPosition2.AddSingleItem(axisPosition2);
            axesPosition2.AddSingleItem(axisPosition3);
            axesPosition2.Name = "456";
            var a1 =Convert.ToInt64( DateTime.Now.Date.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmm") + DateTime.Now.Second.ToString()+DateTime.Now.Millisecond.ToString());
            axesPosition2.ID = a1;


            this.AxesPositionCollection = new AxesPositionCollection();
            this.AxesPositionCollection.AddSingleItem(axesPosition);
            this.AxesPositionCollection.AddSingleItem(axesPosition2);

        }
    }
}
