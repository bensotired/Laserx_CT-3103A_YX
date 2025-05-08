using LX_BurnInSolution.Utilities;
using SolveWare_Motion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{

    public sealed partial class TestPluginWorker_CT3103
    {
        #region Home Axis Group


        internal AxisNameEnum_CT3103[] HomeGroup_1 = new AxisNameEnum_CT3103[]
        {
            AxisNameEnum_CT3103.T_IN_Z,
            AxisNameEnum_CT3103.T_OUT_Z,
            AxisNameEnum_CT3103.LZ,
            AxisNameEnum_CT3103.RZ,
            AxisNameEnum_CT3103.LNY,
            AxisNameEnum_CT3103.LNZ,

        };
        internal AxisNameEnum_CT3103[] HomeGroup_2 = new AxisNameEnum_CT3103[]
        {
            AxisNameEnum_CT3103.RX,
            AxisNameEnum_CT3103.LX,

            AxisNameEnum_CT3103.LNX,

        };
        internal AxisNameEnum_CT3103[] HomeGroup_3 = new AxisNameEnum_CT3103[]
        {
             AxisNameEnum_CT3103.LY,
            AxisNameEnum_CT3103.RY,
            AxisNameEnum_CT3103.Y,
        };
        //internal AxisNameEnum_CT3103[] HomeGroup_4 = new AxisNameEnum_CT3103[]
        //{
        //    AxisNameEnum_CT3103.LSX2_中间单元吸嘴左右,
        //};
        //internal AxisNameEnum_CT3103[] HomeGroup_5 = new AxisNameEnum_CT3103[]
        //{

        //};
        //internal AxisNameEnum_CT3103[] HomeGroup_6 = new AxisNameEnum_CT3103[]
        // {
        //    //AxisNameEnum_CT3103.TTP_θ_Z1_左测试工位旋转,
        //    //AxisNameEnum_CT3103.TTP_θ_Z2_右测试工位旋转,
        //    //AxisNameEnum_CT3103.TTP_θ_Z3_收料盘旋转
        // };

        #endregion

        /// <summary>
        /// 复位取消标志
        /// 如果在运行中，使轴取消运行，则再次运行需要清除取消标志
        /// </summary>
        internal void Reset_HomeStation()
        {
            this.Reset_HomeAxis_Part_X(this.HomeGroup_1);
            this.Reset_HomeAxis_Part_X(this.HomeGroup_2);
            this.Reset_HomeAxis_Part_X(this.HomeGroup_3);
            //this.Reset_HomeAxis_Part_X(this.HomeGroup_4);
            //this.Reset_HomeAxis_Part_X(this.HomeGroup_5);
            //this.Reset_HomeAxis_Part_X(this.HomeGroup_6);
        }

        /// <summary>
        /// 取消回零
        /// 通过取消轴的运动来使回零状态停止
        /// </summary>
        internal void Cancel_HomeStation()
        {
            this.Cancel_HomeAxis_Part_X_Signal(this.HomeGroup_1);
            this.Cancel_HomeAxis_Part_X_Signal(this.HomeGroup_2);
            this.Cancel_HomeAxis_Part_X_Signal(this.HomeGroup_3);
            //this.Cancel_HomeAxis_Part_X_Signal(this.HomeGroup_4);
            //this.Cancel_HomeAxis_Part_X_Signal(this.HomeGroup_5);
            //this.Cancel_HomeAxis_Part_X_Signal(this.HomeGroup_6);
        }

        /// <summary>
        /// 执行回零动作
        /// </summary>
        /// <returns></returns>
        internal bool Run_HomeStation()
        {
            bool homeSucceed = true;
            try
            {
                this.TEC_Controller_1_Close();
                this.TEC_Controller_2_Close();
                this.LocalResource.tED4015.IsOutPut(false);
                bool canHome = true;
                string homeLog = string.Empty;
                string movingAxisName = string.Empty;
                string cancelledAxisName = string.Empty;

                if (this.AnyAxisMoving(out movingAxisName) == true)
                {
                    canHome = false;
                    homeLog += $"轴[{movingAxisName}]正在移动!\r\n";
                }
                if (this.AnyAxisTokenIsCancellationRequested(out cancelledAxisName) == true)
                {
                    canHome = false;
                    homeLog += $"轴[{cancelledAxisName}]取消运行信号已激活!\r\n";
                }
                if (canHome == false)
                {
                    homeLog += $"不具备机台复位动作条件,机台复位将不会执行!";
                    homeSucceed = false;
                    return homeSucceed;
                }

                initIO();

                var hga_1 = this.GetHomeGroupAction(this.HomeGroup_1);
                var hga_2 = this.GetHomeGroupAction(this.HomeGroup_2);
                var hga_3 = this.GetHomeGroupAction(this.HomeGroup_3);
                //var hga_4 = this.GetHomeGroupAction(this.HomeGroup_4);
                //var hga_5 = this.GetHomeGroupAction(this.HomeGroup_5);
                //var hga_6 = this.GetHomeGroupAction(this.HomeGroup_6);

                const string splitor = "|";

                if (this.AnyAxisTokenIsCancellationRequested(out cancelledAxisName) == true)
                {
                    homeLog += $"用户取消机台复位!\r\n";
                    homeSucceed = false;
                    return homeSucceed;
                }
                else
                {
                    this.Log_Global($"复位@[{HG.复位组_0}]完成!..");
                }


                //复位组_1

                this.Log_Global(
                    $"正在进行复位@[{HG.复位组_1}]" +
                    $"[{BaseDataConverter.ConvertCollectionToString(this.HomeGroup_1, splitor)}].."
                );

                Parallel.Invoke(hga_1);

                if (this.AnyAxisTokenIsCancellationRequested(out cancelledAxisName) == true)
                {
                    homeLog += $"用户取消机台复位!\r\n";
                    homeSucceed = false;
                    return homeSucceed;
                }
                else
                {
                    this.Log_Global($"复位@[{HG.复位组_1}]完成!..");
                }

                //复位组_2

                this.Log_Global(
                    $"正在进行复位@[{HG.复位组_2}]" +
                    $"[{BaseDataConverter.ConvertCollectionToString(this.HomeGroup_2, splitor)}].."
                );

                Parallel.Invoke(hga_2);

                if (this.AnyAxisTokenIsCancellationRequested(out cancelledAxisName) == true)
                {
                    homeLog += $"用户取消机台复位!\r\n";
                    homeSucceed = false;
                    return homeSucceed;
                }
                else
                {
                    this.Log_Global($"复位@[{HG.复位组_2}]完成!..");
                }

                //复位组_3

                this.Log_Global(
                    $"正在进行复位@[{HG.复位组_3}]" +
                    $"[{BaseDataConverter.ConvertCollectionToString(this.HomeGroup_3, splitor)}].."
                );

                Parallel.Invoke(hga_3);

                if (this.AnyAxisTokenIsCancellationRequested(out cancelledAxisName) == true)
                {
                    homeLog += $"用户取消机台复位!\r\n";
                    homeSucceed = false;
                    return homeSucceed;
                }
                else
                {
                    this.Log_Global($"复位@[{HG.复位组_3}]完成!..");
                }


                //Parallel.Invoke(
                //() =>
                //{
                //    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.中吸嘴旋转_待机点_θ, SequenceOrder.Normal);
                //},
                //() =>
                //{
                //    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.右吸嘴旋转_待机点_θ, SequenceOrder.Normal);
                //},
                //() =>
                //{
                //    this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.左吸嘴旋转_待机点_θ, SequenceOrder.Normal);
                //});
                //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.测试台_正向_θ, SequenceOrder.Normal);
            }
            catch (AggregateException age)
            {
                homeSucceed = false;
            }
            return homeSucceed;
        }




        internal IONameEnum_CT3103[] IOGroup_OutPut_NeedOn = new IONameEnum_CT3103[]
      {
          IONameEnum_CT3103.TWR_YEL,
          IONameEnum_CT3103.LAMP_BTN_STR,
          IONameEnum_CT3103.LAMP_BTN_RST,
          IONameEnum_CT3103.LAMP_BTN_STP,
          IONameEnum_CT3103.CYL_PICKER_UP,
            //IONameEnum_CT3103.CYL_PICKER_GRAB_OFF,

      };

        internal IONameEnum_CT3103[] IOGroup_OutPut_NeedOff = new IONameEnum_CT3103[]
       {
            IONameEnum_CT3103.TWR_RED,
            IONameEnum_CT3103.TWR_GRN,
            IONameEnum_CT3103.CYL_PICKER_DN,
            IONameEnum_CT3103.Pedestal_Vacuum_L,
            IONameEnum_CT3103.Pedestal_Vacuum_R,
            IONameEnum_CT3103.TEC_Left,
            IONameEnum_CT3103.TEC_Right,
            IONameEnum_CT3103.PD_3,
       };

        internal IONameEnum_CT3103[] IOGroup_Input_NeedCheck_IsActive = new IONameEnum_CT3103[]
      {
              //IONameEnum_CT3103.Input_右PER上下移动气缸复位,
              //IONameEnum_CT3103.Input_右PER前后移动气缸复位,
              //IONameEnum_CT3103.Input_右PER避位气缸复位,
              //IONameEnum_CT3103.Input_右探针加电气缸复位,

              //IONameEnum_CT3103.Input_左PER上下移动气缸复位,
              //IONameEnum_CT3103.Input_左PER前后移动气缸复位,
              //IONameEnum_CT3103.Input_左PER避位气缸复位,
              //IONameEnum_CT3103.Input_左探针加电气缸复位,


      };

        internal IONameEnum_CT3103[] IOGroup_Input_NeedCheck_UnActive = new IONameEnum_CT3103[]
     {
            //IONameEnum_CT3103.Input_右PER上下移动气缸动作,
            //IONameEnum_CT3103.Input_右PER前后移动气缸动作,
            //IONameEnum_CT3103.Input_右PER避位气缸动作,
            //IONameEnum_CT3103.Input_右探针加电气缸动作,

            //IONameEnum_CT3103.Input_左PER上下移动气缸动作,
            //IONameEnum_CT3103.Input_左PER前后移动气缸动作,
            //IONameEnum_CT3103.Input_左PER避位气缸动作,
            //IONameEnum_CT3103.Input_左探针加电气缸动作,

            //IONameEnum_CT3103.Input_测试台右1工位光电传感器,
            //IONameEnum_CT3103.Input_测试台右2工位光电传感器,
            //IONameEnum_CT3103.Input_测试台左1工位光电传感器,
            //IONameEnum_CT3103.Input_测试台左2工位光电传感器,
     };

        internal void initIO()
        {
            Home_IO_Outputs_On();
            Home_IO_Outputs_Off();
            Home_IO_Inputs_Check();
        }
        internal bool Home_IO_Outputs_On()
        {
            foreach (var item in IOGroup_OutPut_NeedOn)
            {
                this.LocalResource.IOs[item].TurnOn(true);
                Thread.Sleep(100);
            }
            return true;
        }

        internal bool Home_IO_Outputs_Off()
        {
            foreach (var item in IOGroup_OutPut_NeedOff)
            {
                this.LocalResource.IOs[item].TurnOn(false);
                Thread.Sleep(100);
            }
            return true;
        }

        internal bool Home_IO_Inputs_Check()
        {
            foreach (var item in IOGroup_Input_NeedCheck_IsActive)
            {
                var result = this.LocalResource.IOs[item].Interation.IsActive;
                if (!result)
                {
                    this.Log_Global($"初始化准备工作异常，异常项[{item.ToString()}]请检查!");
                    throw new Exception($"初始化准备工作异常，异常项[{item.ToString()}]请检查!");
                }
            }
            foreach (var item in IOGroup_Input_NeedCheck_UnActive)
            {
                var result = this.LocalResource.IOs[item].Interation.IsActive;
                if (result)
                {
                    this.Log_Global($"初始化准备工作异常，异常项[{item.ToString()}]请检查!");
                    throw new Exception($"初始化准备工作异常，异常项[{item.ToString()}]请检查!");
                }
            }
            return true;
        }

        #region home group X actions


        /// <summary>
        /// 判断当前是否有轴运行
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        internal bool AnyAxisMoving(out string log)
        {
            bool anyAxisMoving = false;
            log = string.Empty;
            foreach (var axisResource in this.LocalResource.Axes)
            {
                if (axisResource.Value.Interation.IsMoving)
                {
                    anyAxisMoving = true;
                    log += string.Format($"{ axisResource.Key},");
                }
            }
            log = log.TrimEnd(',');
            return anyAxisMoving;
        }


        /// <summary>
        /// 判断当前是否存在轴，运行取消信号被激活
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        internal bool AnyAxisTokenIsCancellationRequested(out string log)
        {
            bool anyAxisTokenIsCancellationRequested = false;
            log = string.Empty;
            foreach (var amaResource in this.LocalResource.AxesMotionAction)
            {
                if (amaResource.Value.TokenIsCancellationRequested())
                {
                    anyAxisTokenIsCancellationRequested = true;
                    log += string.Format($"{ amaResource.Key},");
                }
            }
            log = log.TrimEnd(',');
            return anyAxisTokenIsCancellationRequested;
        }


        /// <summary>
        /// 复位运行信号取消标志
        /// </summary>
        /// <param name="homeGroup_x"></param>
        internal void Reset_HomeAxis_Part_X(AxisNameEnum_CT3103[] homeGroup_x)
        {
            foreach (var axis in homeGroup_x)
            {
                this.LocalResource.AxesMotionAction[axis].Reset();
            }
        }

        /// <summary>
        /// 运行取消信号激活
        /// </summary>
        /// <param name="homeGroup_x"></param>
        internal void Cancel_HomeAxis_Part_X_Signal(AxisNameEnum_CT3103[] homeGroup_x)
        {
            foreach (var axis in homeGroup_x)
            {
                this.LocalResource.AxesMotionAction[axis].Cancel();

                this.Log_Global($"轴{axis}取消动作!");
            }
        }

        internal void HomeAxis_Part_X_Action(AxisNameEnum_CT3103[] homeGroup_x)
        {
            Action[] homeActions = GetHomeGroupAction(homeGroup_x);

            Task.Factory.StartNew(() =>
            {
                Parallel.Invoke
                (
                   homeActions
                );
                this.Log_Global($"组\r\n{BaseDataConverter.ConvertCollectionToString(homeGroup_x, "|")}回零完成!");
            });
        }



        private Action[] GetHomeGroupAction(AxisNameEnum_CT3103[] homeGroup_x)
        {
            List<Action> homeActionList = new List<Action>();
            foreach (var axisName in homeGroup_x)
            {
                var axisInstance = this.LocalResource.Axes[axisName];
                Action homeAction = new Action(() =>
                {
                    var homeSucceed = this.LocalResource.AxesMotionAction[axisName].SingleAxisHome(axisInstance);
                    if (homeSucceed)
                    {
                        this.Log_Global($"轴{axisName}回零成功!");
                    }
                    else
                    {
                        this.Log_Global($"轴{axisName}回零失败!");
                        throw new Exception($"轴{axisName}回零失败!");
                    }
                });
                homeActionList.Add(homeAction);
            }

            return homeActionList.ToArray();
        }



        #endregion
    }
}