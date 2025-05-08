using SolveWare_BurnInCommon;
using SolveWare_BinSorter;
using SolveWare_Motion;
using System;
using System.Collections.Generic;
using System.Threading;
using SolveWare_TestPlugin;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using SolveWare_Vision;
using LX_BurnInSolution.Utilities;
using System.IO;
using System.Linq;
using System.Text;
using SolveWare_TestComponents;
using SolveWare_TestComponents.Attributes;
using System.Data;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT3103
    {
        private bool TurnOneRight { set; get; } //旋转过一次，就认为他测试过两面了
        private bool TurnOneLeft { set; get; }
        public void LaserX_Stpe4_Test_Stations(CancellationTokenSource tokenSource)
        {

            while (true)
            {
                switch (this.Bridges_WithPauseFunc[Action3103.Step4Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Log_Global("[Step4]Step4Finish");
                            return;
                        }
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step4]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }
                if (tokenSource.IsCancellationRequested) return;
                WaitMessage(tokenSource);
                switch (this.Bridges_WithPauseFunc[Action3103.AllowTestsLeft].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Status.IsTesting = Operation.Occupancy;//更改测试状态
                            this.Log_Global($"[Step4]LeftIsTesting：[{this.Status.IsTesting}]");
                            while (true)
                            {
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                if (this.Status.TempControlStateLeft == Operation.Done)
                                {
                                    break;//控温完成则跳出
                                }
                                Thread.Sleep(100);
                            }

                            this.RefreshCarrierID(CarrierNumberLeft);

                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            this.Log_Global("[Step4]Left测试前运动");

                            Parallel.Invoke(() =>
                            {
                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_测试, SequenceOrder.Normal);
                            },
                            () =>
                            {
                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_装卸, SequenceOrder.Normal);
                            });
                            AxesPositionEnum_CT3103 PositionLXY = AxesPositionEnum_CT3103.LXY_扎针1;
                            AxesPositionEnum_CT3103 PositionLZ = AxesPositionEnum_CT3103.LZ_扎针1;
                            this.LocalResource.Axes[AxisNameEnum_CT3103.LY].WaitMotionDone();
                            for (int i = 1; i <= 4; i++)
                            {
                                if (LeftChipNumberlist[LeftTestCount] == "NA" || string.IsNullOrEmpty(LeftChipNumberlist[LeftTestCount]))//对应位置没有产品时跳过
                                {
                                    this.Log_Global($"[Step4]Left测试:{LeftTestCount + 1} pos 无产品 跳过");
                                    LeftTestCount++;
                                    continue;
                                }
                                this.RefreshOeskID(LeftChipNumberlist[LeftTestCount]);

                                this.Log_Global($"[Step4]Left测试:{LeftTestCount + 1}");
                                switch (i)
                                {
                                    case 1:
                                        {
                                            PositionLXY = AxesPositionEnum_CT3103.LXY_扎针1;
                                            PositionLZ = AxesPositionEnum_CT3103.LZ_扎针1;
                                            break;
                                        }
                                    case 2:
                                        {
                                            PositionLXY = AxesPositionEnum_CT3103.LXY_扎针2;
                                            PositionLZ = AxesPositionEnum_CT3103.LZ_扎针2;
                                            break;
                                        }
                                    case 3:
                                        {
                                            PositionLXY = AxesPositionEnum_CT3103.LXY_扎针3;
                                            PositionLZ = AxesPositionEnum_CT3103.LZ_扎针3;
                                            break;
                                        }
                                    case 4:
                                        {
                                            PositionLXY = AxesPositionEnum_CT3103.LXY_扎针4;
                                            PositionLZ = AxesPositionEnum_CT3103.LZ_扎针4;
                                            break;
                                        }
                                }
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                this.Sequence_MoveToAxesPositionByOrder(PositionLXY, SequenceOrder.Normal);
                                this.LocalResource.Axes[AxisNameEnum_CT3103.LX].WaitMotionDone();
                                this.LocalResource.Axes[AxisNameEnum_CT3103.LY].WaitMotionDone();
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                this.Sequence_MoveToAxesPositionByOrder(PositionLZ, SequenceOrder.Normal, SpeedLevel.Low);   //20240627 客户希望将上升速度降低
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                //测试
                                RunStation_Left(tokenSource);
                                this.LocalResource.tED4015.IsOutPut(false);//下探针前需要关闭TED对产品的控温
                                //Thread.Sleep(3000);
                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LZ_待机, SequenceOrder.Normal);
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                LeftTestCount++;
                            }

                            this.Log_Global("[Step4]Left测试后运动");
                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_测试, SequenceOrder.Normal);
                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LX_避让, SequenceOrder.Normal);
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_待机, SequenceOrder.Normal);
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            //当夹具的两面都在同一温度下测试完成，则删除此温度
                            if (this.Status.LeftStationTest == TestStatusOnBoard.一次测试)
                            {
                                TurnOneLeft = true;//
                                this.Log_Global($"删除左载台控温[{ this.TempListLeft[0]}]");
                                this.TempListLeft.RemoveAt(0);
                                this.Status.LeftStationTest = TestStatusOnBoard.未测试;
                            }
                            else
                            {
                                this.Status.LeftStationTest = TestStatusOnBoard.一次测试;
                            }

                            this.Status.IsTesting = Operation.Idle;
                            this.Log_Global($"[Step4]LeftIsTesting：[{this.Status.IsTesting}]");
                            this.Bridges_WithPauseFunc[Action3103.TestCompleteLeft].Set();   //设置了

                            if (this.Status.LeftStationTest == TestStatusOnBoard.一次测试)
                            {
                                //Ben 2024/9/11==============================================================
                                var isTestCompleteLeft_MotionDone = this.Bridges_WithPauseFunc[Action3103.TestCompleteLeft_MotionDone].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.SuperLongTimeout_ms, tokenSource);

                                switch (isTestCompleteLeft_MotionDone)
                                {
                                    case EventResult.TIMEOUT:
                                        {
                                            this.Log_Global("[Step4] TestCompleteLeft_MotionDone 超时等待");
                                            throw new TimeoutException("[Step4] TestCompleteLeft_MotionDone 超时等待");
                                        }
                                        break;
                                    case EventResult.SUCCEED:
                                        {
                                            //什么都不做  往下do while
                                        }
                                        break;
                                    case EventResult.CANCEL:
                                        {
                                            this.Log_Global("[Step4]用户取消运行");
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                }
                            }

                            break;
                            //Ben 2024/9/11==============================================================
                        }

                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step4]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }
                if (tokenSource.IsCancellationRequested) return;
                WaitMessage(tokenSource);
                switch (this.Bridges_WithPauseFunc[Action3103.AllowTestsRight].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Status.IsTesting = Operation.Occupancy;
                            this.Log_Global($"[Step4]RightIsTesting：[{this.Status.IsTesting}]");
                            while (true)
                            {
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                if (this.Status.TempControlStateRight == Operation.Done)
                                {
                                    break;
                                }
                                Thread.Sleep(100);
                            }

                            this.RefreshCarrierID(CarrierNumberRight);

                            this.Log_Global("[Step4]Right测试前运动");
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            Parallel.Invoke(() =>
                            {
                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_测试, SequenceOrder.Normal);
                            },
                           () =>
                           {
                               this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.LY_装卸, SequenceOrder.Normal);
                           });
                            AxesPositionEnum_CT3103 PositionRXY = AxesPositionEnum_CT3103.RXY_扎针1;
                            AxesPositionEnum_CT3103 PositionRZ = AxesPositionEnum_CT3103.RZ_扎针1;
                            this.LocalResource.Axes[AxisNameEnum_CT3103.RY].WaitMotionDone();
                            for (int i = 1; i <= 4; i++)
                            {
                                if (RightChipNumberlist[RightTestCount] == "NA" || string.IsNullOrEmpty(RightChipNumberlist[RightTestCount]))
                                {
                                    this.Log_Global($"[Step4]Right测试:{RightTestCount + 1} pos 无产品 跳过");
                                    RightTestCount++;
                                    continue;
                                }
                                this.RefreshOeskID(RightChipNumberlist[RightTestCount]);
                                this.Log_Global($"[Step4]Right测试:{RightTestCount + 1}");
                                switch (i)
                                {
                                    case 1:
                                        {
                                            PositionRXY = AxesPositionEnum_CT3103.RXY_扎针1;
                                            PositionRZ = AxesPositionEnum_CT3103.RZ_扎针1;
                                            break;
                                        }
                                    case 2:
                                        {
                                            PositionRXY = AxesPositionEnum_CT3103.RXY_扎针2;
                                            PositionRZ = AxesPositionEnum_CT3103.RZ_扎针2;
                                            break;
                                        }
                                    case 3:
                                        {
                                            PositionRXY = AxesPositionEnum_CT3103.RXY_扎针3;
                                            PositionRZ = AxesPositionEnum_CT3103.RZ_扎针3;
                                            break;
                                        }
                                    case 4:
                                        {
                                            PositionRXY = AxesPositionEnum_CT3103.RXY_扎针4;
                                            PositionRZ = AxesPositionEnum_CT3103.RZ_扎针4;
                                            break;
                                        }
                                }
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                this.Sequence_MoveToAxesPositionByOrder(PositionRXY, SequenceOrder.Normal);
                                this.LocalResource.Axes[AxisNameEnum_CT3103.RX].WaitMotionDone();
                                this.LocalResource.Axes[AxisNameEnum_CT3103.RY].WaitMotionDone();
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                this.Sequence_MoveToAxesPositionByOrder(PositionRZ, SequenceOrder.Normal, SpeedLevel.Low);   //20240627 客户希望将上升速度降低

                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                //测试
                                RunStation_Right(tokenSource);
                                this.LocalResource.tED4015.IsOutPut(false);
                                //Thread.Sleep(3000);
                                this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RZ_待机, SequenceOrder.Normal);
                                if (tokenSource.IsCancellationRequested) return;
                                WaitMessage(tokenSource);
                                RightTestCount++;
                            }
                            this.Log_Global("[Step4]Right测试后运动");
                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_测试, SequenceOrder.Normal);
                            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RX_避让, SequenceOrder.Normal);
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);
                            //this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT3103.RY_待机, SequenceOrder.Normal);
                            if (tokenSource.IsCancellationRequested) return;
                            WaitMessage(tokenSource);

                            if (this.Status.RightStationTest == TestStatusOnBoard.一次测试)
                            {
                                this.Log_Global($"删除右载台控温[{ this.TempListRight[0]}]");
                                this.TempListRight.RemoveAt(0);
                                this.Status.RightStationTest = TestStatusOnBoard.未测试;
                                TurnOneRight = true;//
                            }
                            else
                            {
                                this.Status.RightStationTest = TestStatusOnBoard.一次测试;
                            }

                            this.Status.IsTesting = Operation.Idle;
                            this.Log_Global($"[Step4]RightIsTesting：[{this.Status.IsTesting}]");
                            this.Bridges_WithPauseFunc[Action3103.TestCompleteRight].Set();


                            if (this.Status.RightStationTest == TestStatusOnBoard.一次测试)
                            {
                                //Ben 2024/9/11==============================================================
                                var isTestCompleteRight_MotionDone = this.Bridges_WithPauseFunc[Action3103.TestCompleteRight_MotionDone].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.SuperLongTimeout_ms, tokenSource);

                                switch (isTestCompleteRight_MotionDone)
                                {
                                    case EventResult.TIMEOUT:
                                        {
                                            this.Log_Global("[Step4] TestCompleteRight_MotionDone 超时等待");
                                            throw new TimeoutException("[Step4] TestCompleteRight_MotionDone 超时等待");
                                        }
                                        break;
                                    case EventResult.SUCCEED:
                                        {
                                            //什么都不做  往下do while
                                        }
                                        break;
                                    case EventResult.CANCEL:
                                        {
                                            this.Log_Global("[Step4]用户取消运行");
                                            tokenSource.Token.ThrowIfCancellationRequested();
                                            return;
                                        }
                                }

                            }
                            break;
                            //Ben 2024/9/11==============================================================
                        }

                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step4]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }

                }


            }
        }
        public bool RunStation_Left(CancellationTokenSource tokenSource)
        {
            try
            {
                this.Log_Global($"%%%%%%%%%%%开始测试_Left%%%%%%%%%%!");
                //测试模块 
                DeviceStreamData_CT3103 data_demo = new DeviceStreamData_CT3103();
                data_demo.CurrentDateTime = DateTime.Now;
                //_{this.MaskName}_{this.WaferName}
                data_demo.SerialNumber = $"{CarrierNumberLeft}_{LeftTestCount + 1}_{LeftChipNumberlist[LeftTestCount]}_{this.Purpose}_{BaseDataConverter.ConvertDateTimeTo_FILE_string(data_demo.CurrentDateTime)}"; //BaseDataConverter.ConvertDateTimeToCommentString(DateTime.Now);
                this.Log_Global($"产品序列号：{data_demo.SerialNumber}");
                data_demo.DeviceInfo.Station = "CT-3103A";
                data_demo.DeviceInfo.OperatorID = this.OperatorID;
                data_demo.DeviceInfo.Purpose = this.Purpose;
                data_demo.DeviceInfo.CarrierID = CarrierNumberLeft;
                data_demo.MaskName = this.MaskName;
                data_demo.WaferName = this.WaferName;
                data_demo.ChipName = this.ChipName + $"{CarrierNumberLeft}_{LeftTestCount + 1}";
                data_demo.OeskID = LeftChipNumberlist[LeftTestCount];// this.OeskID;
                data_demo.DeviceInfo.WorkOrder = this.WorkOrder;
                data_demo.Tec1ActualTemp = this.parameter.TemperatureListLeft[0]; ;



                _mainStreamData.AddToDataCollection(data_demo);

                this.TestUnits[MT.测试站1.ToString()].RunMainExecutorCombo
                                    (
                                        data_demo,
                                        new Action(this.UpdateMainStreamDataToUI),
                                        null,
                                        null,
                                        tokenSource
                                    );
                this.Log_Global($"产品序列号：{data_demo.SerialNumber}");
                string dir = Path.GetFullPath(Application.StartupPath) + @"\Data\" + data_demo.SerialNumber;

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var defaultFileName = string.Concat("SummaryDataCollection_", BaseDataConverter.ConvertDateTimeTo_FILE_string(data_demo.CurrentDateTime), FileExtension.CSV);
                var finalFileName = $@"{dir}\{defaultFileName}";
                using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(data_demo.SummaryDataCollection.ToString());
                }
                Print(data_demo, "Left (A)", $"{LeftTestCount + 1}");
                #region 
                //var TargetTemperature = this.TempListLeft[0];

                //if (TargetTemperature <= this.parameter.LowTemperature)
                //{
                //    this.TestUnits[MT.测试站1.ToString()].RunPreExecutorCombo
                //                        (
                //                            data_demo,
                //                            new Action(this.UpdateMainStreamDataToUI),
                //                            null,
                //                            null,
                //                            tokenSource
                //                        );
                //}
                //else if (TargetTemperature <= this.parameter.HightTemperature)
                //{
                //    this.TestUnits[MT.测试站1.ToString()].RunMainExecutorCombo
                //                        (
                //                            data_demo,
                //                            new Action(this.UpdateMainStreamDataToUI),
                //                            null,
                //                            null,
                //                            tokenSource
                //                        );
                //}
                //else
                //{
                //    this.TestUnits[MT.测试站1.ToString()].RunPostExecutorCombo
                //                        (
                //                            data_demo,
                //                            new Action(this.UpdateMainStreamDataToUI),
                //                            null,
                //                            null,
                //                            tokenSource
                //                        );
                //}



                //this.UpdateMainStreamDataToUI_FocusInTargetDevice(data_demo.SerialNumber);
                //this.ReCalibrateSummaryData(data_demo.SummaryDataCollection.ItemCollection, (this._mainStreamData.TestImportProfile as TestPluginImportProfile_CT40410A).UserDefinedCalibrationData_Loader);

                //string binCollectionName = this.binCollectionName;
                //var BinSettingCollection = this.LocalResource.Local_BinSortList_ResourceProvider.GetBinSettingCollectionObject(binCollectionName) as BinSettingCollection;
                //data_demo.BIN = Gear.档位1.ToString();//data_demo.GetBinGrade(BinSettingCollection);

                //var box = this.parameterInformation.gearBox[(Gear)Enum.Parse(typeof(Gear), data_demo.BIN)];
                //data_demo.OutPutScann = this.parameterInformation.UnLoadScann[box];
                //data_demo.BinNumber = this.parameterInformation.PositinIndex[box].ToString();

                //this.TestUnits[MT.测试站1.ToString()].RunPostExecutorCombo(data_demo, null, null, null, tokenSource);


                //this._mainStreamData.Station_1_Move_To_OutUP(data_demo);

                //#region 存表格
                //string dir = Path.GetFullPath(Application.StartupPath) + @"\Data\" + DateTime.Now.ToString("yyyyMMdd");


                //if (!Directory.Exists(dir))
                //{
                //    Directory.CreateDirectory(dir);
                //}

                //string fulldir = dir + @"\" + data_demo.OutPutScann;
                //if (!Directory.Exists(fulldir))
                //{
                //    Directory.CreateDirectory(fulldir);
                //}

                //var defaultFileName = string.Concat(data_demo.BIN + "_" + data_demo.BinNumber + "_" + data_demo.SerialNumber + "_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);

                //var finalFileName = $@"{fulldir}\{defaultFileName}";

                //string name = "序列号,损耗率,BIN,BinNumber,出料信息";
                //string infor = $"{data_demo.SerialNumber},{data_demo.AtritionRate},{data_demo.BIN},{data_demo.BinNumber},{data_demo.OutPutScann}";
                //using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                //{
                //    sw.WriteLine(name);
                //    sw.WriteLine(infor);
                //    sw.WriteLine();
                //    sw.WriteLine(data_demo.SummaryDataCollection.ToString());
                //    sw.WriteLine(data_demo.RawDataCollection[0].ToString());
                //    sw.WriteLine();



                //}

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                this.Log_Global($"{ex.Message}-{ex.StackTrace}");
                throw new Exception($"{ex.Message}-{ex.StackTrace}");
            }

        }
        public bool RunStation_Right(CancellationTokenSource tokenSource)
        {
            try
            {
                this.Log_Global($"%%%%%%%%%%%开始测试_Right%%%%%%%%%%!");
                //测试模块 
                DeviceStreamData_CT3103 data_demo = new DeviceStreamData_CT3103();
                data_demo.CurrentDateTime = DateTime.Now;
                //_{this.MaskName}_{this.WaferName}
                data_demo.SerialNumber = $"{CarrierNumberRight}_{RightTestCount + 1}_{RightChipNumberlist[RightTestCount]}_{this.Purpose}_{BaseDataConverter.ConvertDateTimeTo_FILE_string(data_demo.CurrentDateTime)}"; //BaseDataConverter.ConvertDateTimeToCommentString(DateTime.Now);
                this.Log_Global($"产品序列号：{data_demo.SerialNumber}");
                data_demo.DeviceInfo.Station = "CT-3103A";
                data_demo.DeviceInfo.OperatorID = this.OperatorID;
                data_demo.DeviceInfo.Purpose = this.Purpose;
                data_demo.DeviceInfo.CarrierID = CarrierNumberRight;
                data_demo.MaskName = this.MaskName;
                data_demo.WaferName = this.WaferName;
                data_demo.ChipName = this.ChipName + $"{CarrierNumberRight}_{RightTestCount + 1}";
                data_demo.OeskID = RightChipNumberlist[RightTestCount];// this.OeskID;
                data_demo.DeviceInfo.WorkOrder = this.WorkOrder;
                data_demo.Tec1ActualTemp = this.parameter.TemperatureListRight[0];

                _mainStreamData.AddToDataCollection(data_demo);

                this.TestUnits[MT.测试站1.ToString()].RunMainExecutorCombo
                                    (
                                        data_demo,
                                        new Action(this.UpdateMainStreamDataToUI),
                                        null,
                                        null,
                                        tokenSource
                                    );
                this.Log_Global($"产品序列号：{data_demo.SerialNumber}");
                string dir = Path.GetFullPath(Application.StartupPath) + @"\Data\" + data_demo.SerialNumber;

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var defaultFileName = string.Concat("SummaryDataCollection_", BaseDataConverter.ConvertDateTimeTo_FILE_string(data_demo.CurrentDateTime), FileExtension.CSV);
                var finalFileName = $@"{dir}\{defaultFileName}";
                using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(data_demo.SummaryDataCollection.ToString());
                }
                Print(data_demo, "Rignt (B)", $"{RightTestCount + 1}");
                #region
                //var TargetTemperature = this.TempListLeft[0];

                //if (TargetTemperature <= this.parameter.LowTemperature)
                //{
                //    this.TestUnits[MT.测试站1.ToString()].RunPreExecutorCombo
                //                        (
                //                            data_demo,
                //                            new Action(this.UpdateMainStreamDataToUI),
                //                            null,
                //                            null,
                //                            tokenSource
                //                        );
                //}
                //else if (TargetTemperature <= this.parameter.HightTemperature)
                //{
                //    this.TestUnits[MT.测试站1.ToString()].RunMainExecutorCombo
                //                        (
                //                            data_demo,
                //                            new Action(this.UpdateMainStreamDataToUI),
                //                            null,
                //                            null,
                //                            tokenSource
                //                        );
                //}
                //else
                //{
                //    this.TestUnits[MT.测试站1.ToString()].RunPostExecutorCombo
                //                        (
                //                            data_demo,
                //                            new Action(this.UpdateMainStreamDataToUI),
                //                            null,
                //                            null,
                //                            tokenSource
                //                        );
                //}


                //this.UpdateMainStreamDataToUI_FocusInTargetDevice(data_demo.SerialNumber);
                //this.ReCalibrateSummaryData(data_demo.SummaryDataCollection.ItemCollection, (this._mainStreamData.TestImportProfile as TestPluginImportProfile_CT40410A).UserDefinedCalibrationData_Loader);

                //string binCollectionName = this.binCollectionName;
                //var BinSettingCollection = this.LocalResource.Local_BinSortList_ResourceProvider.GetBinSettingCollectionObject(binCollectionName) as BinSettingCollection;
                //data_demo.BIN = data_demo.GetBinGrade(BinSettingCollection);

                //var box = this.parameterInformation.gearBox[(Gear)Enum.Parse(typeof(Gear), data_demo.BIN)];
                //data_demo.OutPutScann = this.parameterInformation.UnLoadScann[box];
                //data_demo.BinNumber = this.parameterInformation.PositinIndex[box].ToString();

                //this.TestUnits[MT.测试站2.ToString()].RunPostExecutorCombo(data_demo, null, null, null, tokenSource);


                //this._mainStreamData.Station_2_Move_To_OutUP(data_demo);

                //#region 存表格
                //string dir = Path.GetFullPath(Application.StartupPath) + @"\Data\" + DateTime.Now.ToString("yyyyMMdd");


                //if (!Directory.Exists(dir))
                //{
                //    Directory.CreateDirectory(dir);
                //}

                //string fulldir = dir + @"\" + data_demo.OutPutScann;
                //if (!Directory.Exists(fulldir))
                //{
                //    Directory.CreateDirectory(fulldir);
                //}

                //var defaultFileName = string.Concat(data_demo.BIN + "_" + data_demo.BinNumber + "_" + data_demo.SerialNumber + "_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);

                //var finalFileName = $@"{fulldir}\{defaultFileName}";

                //string name = "序列号,损耗率,BIN,BinNumber,出料信息";
                //string infor = $"{data_demo.SerialNumber},{data_demo.AtritionRate},{data_demo.BIN},{data_demo.BinNumber},{data_demo.OutPutScann}";
                //using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                //{
                //    sw.WriteLine(name);
                //    sw.WriteLine(infor);
                //    sw.WriteLine();
                //    sw.WriteLine(data_demo.SummaryDataCollection.ToString());
                //    sw.WriteLine(data_demo.RawDataCollection[0].ToString());
                //    sw.WriteLine();



                //}

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                this.Log_Global($"{ex.Message}-{ex.StackTrace}");
                throw new Exception($"{ex.Message}-{ex.StackTrace}");
            }

        }

        public void Print(DeviceStreamData_CT3103 data, string testPlatform, string socketnumber)
        {
            try
            {
                string dir = Path.GetFullPath(Application.StartupPath) + @"\TestData\TestData.csv";
                //if (!Directory.Exists(dir))
                //{
                //    Directory.CreateDirectory(dir);
                //}
                if (!File.Exists(dir))//判断文件是否存在，不存在则创建
                {
                    //Machine name	Operater ID	Time Work order	Purpose	Test Platform	Fixture ID	Socket number	
                    //CoC information_ID			QWLT result															
                    //LIV result				Coarse Tune
                    string columnname = "Machine name,Operater ID,Time,Work order,Purpose,Test Platform,Fixture ID,Socket number," +
                                        "CoC information_ID, , ,QWLT result, , , , , , , , , , , , , , ,LIV result, , , ,Coarse Tune";
                    //Station name	worker ID							Mask	Wafer	PIC	
                    //Gain[mA]	SOA1[mA]	SOA2[mA]	Mrror1[mA]	Mirror2[mA]	L-phase[mA]	Phase1[mA]	Phase2[mA]	
                    //Wavelength[nm]	Deviation[nm]	SMSR[dB]	MZM Vbias[V]	mPD Bias [V]	MPD1[uA]	MPD2[uA]	
                    //Ith to SOA	Ith to Power	TapPD_Power	IS_Power	Missing Channel	Judgement	Failure code

                    string row1 = "Station name,worker ID, , , , , , ,Mask,Wafer,PIC,Gain[mA],SOA1[mA],SOA2[mA],Mrror1[mA],Mirror2[mA],L-phase[mA],Phase1[mA],Phase2[mA]," +
                                "Wavelength[nm],Deviation[nm],SMSR[dB],MZM Vbias[V],mPD Bias [V],MPD1[uA],MPD2[uA]," +
                                "Ith to SOA,Ith to Power,TapPD_Power,IS_Power[mW],Missing Channel,Judgement,Failure code,Test Temp[DegC]," +
                                "MZMRdiff[Ohm],Resistance1[Ohm],Resistance2[Ohm]";
                    using (StreamWriter sw = new StreamWriter(dir, false, Encoding.GetEncoding("gb2312")))
                    {
                        sw.WriteLine(columnname);
                        sw.WriteLine(row1);
                    }
                }
                #region Parameters
                string SerialNumber = data.SerialNumber;

                string Machinename = data.DeviceInfo.Station;
                string OperaterID = data.DeviceInfo.OperatorID;
                //Partnumber不再使用，改为时间
                string Partnumber = BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now);
                string Workorder = data.DeviceInfo.WorkOrder;
                string Purpose = data.DeviceInfo.Purpose;
                string TestPlatform = testPlatform;
                string FixtureID = data.DeviceInfo.CarrierID;
                string Socketnumber = socketnumber;// data.DeviceInfo.ChipID;
                string Mask = data.MaskName;
                string Wafer = data.WaferName;
                string PIC = data.OeskID;

                double Gain = 120;
                double SOA1 = 50;
                double SOA2 = 40;
                double MIRROR1 = double.NaN;
                double MIRROR2 = double.NaN;
                double LP = double.NaN;
                double PH1 = double.NaN;
                double PH2 = double.NaN;

                double Wavelength = double.NaN;
                double Deviation = double.NaN;
                double SMSR = double.NaN;

                double Bias1 = double.NaN;
                double Bias2 = double.NaN;
                double MPD1 = double.NaN;
                double MPD2 = double.NaN;

                double IthtoSOA = double.NaN;
                double IthtoPower = double.NaN;
                double TapPD_Power = double.NaN;
                double IS_Power = double.NaN;
                double IS_MaxPower = double.NaN;

                string missingch = string.Empty;
                string Judgement = "Fail";
                string Failurecode = string.Empty;

                double Test_DegC = -1;

                double MZMRdiff = double.NaN;
                double Resistance1 = double.NaN;
                double Resistance2 = double.NaN; 
                #endregion
                try
                {
                    //获取QWLT2中计算结果
                    foreach (var dataMenu in data.RawDataCollection)
                    {
                        if (dataMenu is IRawDataMenuCollection)
                        {

                            var rawd = dataMenu as IRawDataMenuCollection;
                            var type = rawd.GetType();
                            if (type.Name == "RawDataMenu_QWLT2")
                            {
                                var props = rawd.GetType().GetProperties();
                                var broEleProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(props);

                                foreach (var bp in broEleProps)
                                {
                                    if (bp.Name == "MIRROR1_mid_slope_val")
                                    {
                                        MIRROR1 = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    }
                                    if (bp.Name == "MIRROR2_mid_slope_val")
                                    {
                                        MIRROR2 = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    }
                                    if (bp.Name == "LP")
                                    {
                                        LP = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    }
                                    if (bp.Name == "PH_Max_Sec_1")
                                    {
                                        PH1 = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    }
                                    if (bp.Name == "PH_Max_Sec_2")
                                    {
                                        PH2 = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    }
                                    //if (bp.Name == "mPd1_V")
                                    //{
                                    //    MPD1 = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    //}
                                    //if (bp.Name == "mPd2_V")
                                    //{
                                    //    MPD2 = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    //}
                                    if (bp.Name == "Bais1_V")
                                    {
                                        Bias1 = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    }
                                    if (bp.Name == "Bais2_V")
                                    {
                                        Bias2 = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    }


                                }
                            }
                        }
                    }
                    //获取LIV_Normal中PD最大功率
                    foreach (var dataMenu in data.RawDataCollection)
                    {
                        if (dataMenu is IRawDataMenuCollection)
                        {
                            var rawDataMenuCollection = (IRawDataMenuCollection)dataMenu;
                            foreach (var rdata in rawDataMenuCollection.GetDataMenuCollection())
                            {

                                if (rdata.GetType().Name == "RawData_LIV_Normal")
                                {
                                    var rawProps = rdata.GetType().GetProperties();
                                    var bowProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(rawProps);
                                    foreach (var bp in bowProps)
                                    {
                                        if (bp.Name == "MAX_Power")//最大光功率
                                        {
                                            IS_MaxPower = Convert.ToDouble(bp.GetValue(rdata).ToString());
                                        }
                                        else if (bp.Name == "Pout_120mA_Power")//指定电流下的光功率
                                        {
                                            IS_Power = Convert.ToDouble(bp.GetValue(rdata).ToString());
                                        }
                                    }
                                }
                            }

                        }
                    }
                    //获取TapPD中读到的波长、功率、mpd电流
                    foreach (var dataMenu in data.RawDataCollection)
                    {
                        if (dataMenu is IRawDataMenuCollection)
                        {
                            var rawd = dataMenu as IRawDataMenuCollection;
                            var type = rawd.GetType();
                            if (type.Name == "RawDataMenu_Tap_PD")
                            {
                                var props = rawd.GetType().GetProperties();
                                var broEleProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(props);

                                foreach (var bp in broEleProps)
                                {
                                    if (bp.Name == "Wavelength")
                                    {
                                        Wavelength = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    }
                                    if (bp.Name == "Deviation")
                                    {
                                        Deviation = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    }
                                    if (bp.Name == "SMSR")
                                    {
                                        SMSR = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    }
                                    if (bp.Name == "mPd1_uA")
                                    {
                                        MPD1 = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    }
                                    if (bp.Name == "mPd2_uA")
                                    {
                                        MPD2 = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                    }


                                }
                            }
                        }
                    }
                    //获取SummaryData中所需计算结果
                    foreach (var Summary in data.SummaryDataCollection)
                    {
                        if (Summary.Name.Contains("SOA1_Ith1"))
                        {
                            IthtoSOA = Convert.ToDouble(Summary.Value.ToString());
                        }
                        if (Summary.Name.Contains("Tap_PD_Ith1"))
                        {
                            TapPD_Power = Convert.ToDouble(Summary.Value.ToString());
                        }
                        if (Summary.Name.Contains("LIV_Ith1"))
                        {
                            IthtoPower = Convert.ToDouble(Summary.Value.ToString());
                        }

                        if (Summary.Name.Contains("MZMRdiff"))
                        {
                            MZMRdiff = Convert.ToDouble(Summary.Value.ToString());
                        }
                        if (Summary.Name.Contains("Resistance1"))
                        {
                            Resistance1 = Convert.ToDouble(Summary.Value.ToString());
                        }
                        if (Summary.Name.Contains("Resistance2"))
                        {
                            Resistance2 = Convert.ToDouble(Summary.Value.ToString());
                        }
                    }


                    //获取TED控温中读到的温度
                    bool find = false;
                    foreach (var dataMenu in data.RawDataCollection)
                    {
                        if (dataMenu is IRawDataMenuCollection)
                        {
                            var rawd = dataMenu as IRawDataMenuCollection;
                            var type = rawd.GetType();
                            if (type.Name == "RawDataMenu_TED")
                            {
                                var props = rawd.GetType().GetProperties();
                                var broEleProps = PropHelper.GetAttributeProps<RawDataBrowsableElementAttribute>(props);

                                foreach (var bp in broEleProps)
                                {
                                    if (bp.Name == "FinishTemp_DegC")
                                    {
                                        Test_DegC = Convert.ToDouble(bp.GetValue(rawd).ToString());
                                        find = true;   //魔改,只取第一个 
                                    }
                                }
                            }
                        }
                        if (find) break;
                    }




                    //获取CoarseTuning后CSV文件地址，解析后得到CH：-2~97之间是否连续

                    if (!string.IsNullOrEmpty(data.CoarseTuningPath))
                    {
                        DataTable dataTable = new DataTable();

                        using (StreamReader reader = new StreamReader(data.CoarseTuningPath))
                        {
                            string[] headers = reader.ReadLine().Split(','); // 读取第一行作为表头

                            foreach (string header in headers)
                            {
                                dataTable.Columns.Add(header); // 设置表头列名
                            }

                            while (!reader.EndOfStream)
                            {
                                string[] rows = reader.ReadLine().Split(',');

                                DataRow dataRow = dataTable.NewRow();
                                for (int i = 0; i < headers.Length; i++)
                                {
                                    dataRow[i] = rows[i];
                                }
                                dataTable.Rows.Add(dataRow); // 添加行数据
                            }
                        }
                        string CH = "CH";
                        List<double> CHList = new List<double>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            CHList.Add(Convert.ToDouble(row[CH]));
                        }

                        int chMin = -2;// (int)CHList.Min();
                        int chMax = 97;// (int)CHList.Max();
                        for (int i = chMin; i <= chMax; i++)
                        {
                            if (!CHList.Contains(i))
                            {
                                missingch += $"_{i}";
                            }
                        }
                        missingch = missingch.Trim('_');
                    }


                    if (SMSR < 35)
                    {
                        Judgement = "Fail";
                        Failurecode += "[SMSR Fail]";
                    }
                    else
                    {
                        Judgement = "Pass";
                    }
                    if (IS_Power < 8)  //OE要求120mA下光功率小于8mW就不行
                    {
                        Judgement = "Fail";
                        Failurecode += "[Low Power]";
                    }
                    else
                    {
                        Judgement = "Pass";
                    }

                    if (!string.IsNullOrEmpty(missingch))
                    {
                        Judgement = "Fail";
                        Failurecode += "[Coarse Tuning fail]";
                    }
                    else
                    {
                        Judgement = "Pass";
                    }
                }
                catch (Exception ex)
                {
                    this.Log_Global($"[{ex.Message}]-[{ex.StackTrace}]");
                }
                if (SMSR == double.NaN)
                {
                    Judgement = "Fail";
                }

                //确认文件是否被占用
                while (true)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(dir, true, Encoding.GetEncoding("gb2312")))
                        {
                            string result = $"{Machinename},{OperaterID},{Partnumber},{Workorder},{Purpose}," +
                                            $"{TestPlatform},{FixtureID},{Socketnumber},{Mask},{Wafer},{PIC}," +
                                            $"{Gain},{SOA1},{SOA2},{MIRROR1},{MIRROR2},{LP},{PH1},{PH2}," +
                                            $"{Wavelength},{Deviation},{SMSR},{Bias1},{Bias2},{MPD1},{MPD2}," +
                                            $"{IthtoSOA},{IthtoPower},{TapPD_Power},{IS_Power}," +
                                            $"{missingch},{Judgement},{Failurecode},{Test_DegC}," +
                                            $"{MZMRdiff},{Resistance1},{Resistance2}";
                            sw.WriteLine(result);
                        }
                        break;

                    }
                    catch (Exception ex)
                    {
                        this.Log_Global($"文件被占用, 数据无法写入:{dir}");

                        var dr = MessageBox.Show($"文件被占用, 数据无法写入:{dir}",
                         "提示", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        if (dr == DialogResult.Retry)
                        {
                            this.Log_Global($"客户选择重试");
                            continue;
                        }
                        else
                        {
                            this.Log_Global($"客户选择取消");
                            throw new Exception($"文件被占用, 数据无法写入:{dir} ex[{ex.Message}]");
                        }
                    }
                }



                //按夹具再打印一遍，生产要求

                string dirCarrier = Path.GetFullPath(Application.StartupPath) + @"\TestData\" + FixtureID;
                string dirCSV = dirCarrier + @"\TestData.csv";

                if (!Directory.Exists(dirCarrier))
                {
                    Directory.CreateDirectory(dirCarrier);

                    string columnname = "Machine name,Operater ID,Time,Work order,Purpose,Test Platform,Fixture ID,Socket number," +
                                        "CoC information_ID, , ,QWLT result, , , , , , , , , , , , , , ,LIV result, , , ,Coarse Tune";

                    string row1 = "Station name,worker ID, , , , , , ,Mask,Wafer,PIC,Gain[mA],SOA1[mA],SOA2[mA],Mrror1[mA],Mirror2[mA],L-phase[mA],Phase1[mA],Phase2[mA]," +
                                "Wavelength[nm],Deviation[nm],SMSR[dB],MZM Vbias[V],mPD Bias [V],MPD1[uA],MPD2[uA]," +
                                "Ith to SOA,Ith to Power,TapPD_Power,IS_Power[mW],Missing Channel,Judgement,Failure code,Test Temp[DegC]";

                    using (StreamWriter sw = new StreamWriter(dirCSV, false, Encoding.GetEncoding("gb2312")))
                    {
                        sw.WriteLine(columnname);
                        sw.WriteLine(row1);
                    }
                }

                //确认文件是否被占用
                while (true)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(dirCSV, true, Encoding.GetEncoding("gb2312")))
                        {
                            string result = $"{Machinename},{OperaterID},{Partnumber},{Workorder},{Purpose}," +
                                            $"{TestPlatform},{FixtureID},{Socketnumber},{Mask},{Wafer},{PIC}," +
                                            $"{Gain},{SOA1},{SOA2},{MIRROR1},{MIRROR2},{LP},{PH1},{PH2}," +
                                            $"{Wavelength},{Deviation},{SMSR},{Bias1},{Bias2},{MPD1},{MPD2}," +
                                            $"{IthtoSOA},{IthtoPower},{TapPD_Power},{IS_Power}," +
                                            $"{missingch},{Judgement},{Failurecode},{Test_DegC}";
                            sw.WriteLine(result);
                        }
                        break;

                    }
                    catch (Exception ex)
                    {
                        this.Log_Global($"文件被占用, 数据无法写入:{dirCSV}");
                        var dr = MessageBox.Show($"文件被占用, 数据无法写入:{dirCSV}",
                         "提示", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        if (dr == DialogResult.Retry)
                        {
                            this.Log_Global($"客户选择重试");
                            continue;
                        }
                        else
                        {
                            this.Log_Global($"客户选择取消");
                            throw new Exception($"文件被占用, 数据无法写入:{dirCSV} ex[{ex.Message}]");
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                this.Log_Global($"[{ex.Message}]-[{ex.StackTrace}]");
            }

        }
    }

}
