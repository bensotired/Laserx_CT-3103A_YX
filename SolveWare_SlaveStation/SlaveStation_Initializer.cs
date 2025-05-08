using SolveWare_BurnInAppInterface;
using SolveWare_IO;
using SolveWare_Motion;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SolveWare_SlaveStation
{
    public class SlaveStation_Initializer
    {
        public string Name { get; private set; }
        public List<MotorAxisBase> MotorAxisBases { get; private set; }
        public List<bool> DirectionReverseList { get; private set; }
        public List<AxesPosition> AxisPositions { get; private set; }
        public List<IOBase> IOBases { get; private set; }
        //public Action<AxesPosition> UpdatePluginAxesPositionAction { get;private set; }
        public SlaveStation_Initializer()
        {
            MotorAxisBases = new List<MotorAxisBase>();
            DirectionReverseList = new List<bool>();
            AxisPositions = new List<AxesPosition>();
            IOBases = new List<IOBase>();
        }
        public void GetSlaveStationResources( 
                                string name,  
                                SlaveStation_Config slaveStation_Config,
                                IAxisResourceProvider local_Axis_ResourceProvider,
                                IIOResourceProvider local_IO_ResourceProvider,
                                IAxesPositionResourceProvider local_AxesPosition_ResourceProvider 
                            /*    Action<AxesPosition> updatePluginAxesPositionAction*/)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Slave Station 名称为空！");
                }
                //if(updatePluginAxesPositionAction == null)
                //{
                //    throw new Exception("插件资源点位更新委托为空！");
                //}

                this.Name = name;
                //this.UpdatePluginAxesPositionAction = updatePluginAxesPositionAction;

                foreach (var cfg in slaveStation_Config.Axes)
                {
                    MotorAxisBases.Add((MotorAxisBase)local_Axis_ResourceProvider.GetAxis_Object(cfg));
                }
                foreach (var cfg in slaveStation_Config.Positions)
                {
                    AxisPositions.Add((AxesPosition)local_AxesPosition_ResourceProvider.GetAxesPosition_Object(cfg));
                }
                foreach (var cfg in slaveStation_Config.AxisDirections)
                {
                    DirectionReverseList.Add(Convert.ToBoolean((AxisDirection)Enum.Parse(typeof(AxisDirection), cfg)));
                }
                foreach (var cfg in slaveStation_Config.IOs)
                {
                    IOBases.Add((IOBase)local_IO_ResourceProvider.GetIO_Object(cfg));
                }
                if (MotorAxisBases.Count != DirectionReverseList.Count)
                {
                    throw new Exception("轴和方向数目不对应");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化子工站错误:{ex.Message} - {ex.StackTrace}!");
                throw new Exception("初始化子工站错误", ex);
            }
        }
    }
}
