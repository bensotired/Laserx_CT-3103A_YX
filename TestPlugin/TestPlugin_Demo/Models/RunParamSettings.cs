using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{

    [Serializable]
    public class RunParamSettings
    {
 
        public double 相机1允许像素误差_X { get; set; }
        public double 相机1允许像素误差_Y { get; set; }


        public double 相机2允许角度误差 { get; set; }
        public double 左吸嘴相机间距_X { get; set; }
        public double 左吸嘴相机间距_Y { get; set; }
        public double 中吸嘴相机间距_X { get; set; }
        public double 中吸嘴相机间距_Y { get; set; }
        public double 右吸嘴相机间距_X { get; set; }
        public double 右吸嘴相机间距_Y { get; set; }

        public double KT15_Angle { get; set; }

        public double 真空判定标准 { get; set; }

        public DataBook<string, DataBook<Gear, List<FeedBox_Out>>> BIN { get; set; }

        public RunParamSettings()
        {
            this.相机1允许像素误差_X = 2;
            this.相机1允许像素误差_Y = 2;
            this.相机2允许角度误差 = 0.3;
            this.左吸嘴相机间距_X = 100;
            this.左吸嘴相机间距_Y = 100;
            this.中吸嘴相机间距_X = 100;
            this.中吸嘴相机间距_Y = 100;
            this.右吸嘴相机间距_X = 100;
            this.右吸嘴相机间距_Y = 100;
            this.KT15_Angle = 45;
            this.真空判定标准 = 50;
            this.BIN = new DataBook<string, DataBook<Gear, List<FeedBox_Out>>>();
        }
    }
}
