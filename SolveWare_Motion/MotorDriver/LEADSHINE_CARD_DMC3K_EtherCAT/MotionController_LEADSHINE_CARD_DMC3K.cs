using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace SolveWare_Motion
{
    public class MotionController_LEADSHINE_CARD_DMC3K : MotionControllerBase, IMotionController
    {
        public MotionController_LEADSHINE_CARD_DMC3K(string name, string address, IInstrumentChassis chassis) 
            : base(name, address, chassis)
        {
            _AxesCollection = new List<MotorAxisBase>();
        }
        //将所有轴实例化的时候使用到
        //虚拟方法必须要实现
        //深揪根据类的名称实例化类，并且调用
        public override MotorAxisBase CreateAxisInstance(MotorSetting setting)
        {
            var axis = new Motor_LEADSHINE_DMC3k(setting);
            if (axis.Interation.IsSimulation == false) //不是模拟的阶段
            {
                if (this.IsOnline)//并且轴是在线的状态
                {
                    axis.Init();
                }
            }
            return axis;
        }
        private string DefaultStationConfigFilePath = @"SystemConfig";
        public override void LoadSpecificConfig(object configObj)
        {
            if (!_chassis.IsOnline) return;

            int rc = 0;
            string exMsg = "";
            AppPluginConfigItem config = configObj as AppPluginConfigItem;
            var pathTable = ReadINITable(config.PluginArguments);//板卡参数导入文件在这里

            foreach (var item in pathTable)
            {
                string path = Path.Combine(Path.GetFullPath(DefaultStationConfigFilePath), item.Value);
                if (File.Exists(path))
                {
                    int carditem = item.Key;
                    //板卡的参数文件导入
                    rc = LTDMC.dmc_download_configfile((ushort)carditem, path);
                    if (rc != 0)
                    {
                        exMsg = $"控制卡ID[{carditem}] PN_AxisHandleErrorMode设置失败]";
                        //throw new Exception($"{exMsg}!");
                    }
                }



            }

        }
        public Dictionary<int, string> ReadINITable(string para)
        {
            Dictionary<int, string> p = new Dictionary<int, string>();

            string pattern = @"(?<=\[)(.*?),(.*?)(?=\])";
            try
            {
                //进行正则表达式分解
                Regex r = new Regex(pattern);

                //获得组号码的清单
                int[] gnums = r.GetGroupNumbers();

                //首次匹配
                Match m = r.Match(para);

                while (m.Success)
                {
                    // string N1 = m.Groups[0].ToString();
                    int CardIndex = 0;
                    if (int.TryParse(m.Groups[1].ToString(), out CardIndex) == false)
                    {
                        return p;
                    }
                    string path = m.Groups[2].ToString();

                    p.Add(CardIndex, path);

                    //下一个匹配
                    m = m.NextMatch();
                }
            }
            catch
            {
                return p;
            }

            return p;
        }
        public override void GenerateFakeDataOnceCycle(CancellationToken token)//生成一次循环的假数据
        {
            //throw new NotImplementedException();
        }

        public override void RefreshDataOnceCycle(CancellationToken token)//一次周期刷新数据
        {
            //throw new NotImplementedException();
        }
    }
}