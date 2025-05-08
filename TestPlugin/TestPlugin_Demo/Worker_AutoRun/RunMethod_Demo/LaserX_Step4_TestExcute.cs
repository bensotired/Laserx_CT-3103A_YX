using SolveWare_BurnInCommon;
using SolveWare_BinSorter;
using SolveWare_Motion;
using System;
using System.Collections.Generic;
using System.Threading;
using SolveWare_TestPlugin;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using SolveWare_Vision;
using LX_BurnInSolution.Utilities;
using System.IO;
using System.Linq;
using System.Text;
using Thorlabs.TLPM_32.Interop;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT40410A
    {
        public void LaserX_Step4_TestExcute(CancellationTokenSource tokenSource)
        {
            this.Log_Global("[Step4]Load");
            do
            {
                if (tokenSource.IsCancellationRequested)
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    return;
                }
                switch (this.Bridges_WithPauseFunc[ARECT40410A.Step3Finish].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                {
                    case EventResult.SUCCEED:
                        {
                            this.Bridges_WithPauseFunc[ARECT40410A.Step4Finish].Set();
                            this.Log_Global("[Step4]Step4Finish");
                            return;
                        }
                        break;
                    case EventResult.CANCEL:
                        {
                            this.Log_Global("[Step4]用户取消运行");
                            tokenSource.Token.ThrowIfCancellationRequested();
                            return;
                        }
                        break;
                    case EventResult.TIMEOUT:
                        {
                            if (tokenSource.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                                return;
                            }
                            switch (this.Bridges_WithPauseFunc[ARECT40410A.CanTest].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT40410A.NanoTimeout_ms, tokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {

                                        //测试及所需动作
                                        this.Nozzle_Left_Run_To_LoadDut_PreparePosition(tokenSource);
                                        if (TrunOn)
                                        {
                                            //this.Pro_Light(tokenSource);
                                        }
                                        this.Log_Global("[Step4]收到开始测试信号");
                                        this.RunMain(tokenSource);
                                        this.Bridges_WithPauseFunc[ARECT40410A.FinishTest].Set();
                                    }
                                    break;
                                case EventResult.CANCEL:
                                    {
                                        this.Log_Global("[Step4]用户取消运行");
                                        tokenSource.Token.ThrowIfCancellationRequested();
                                        return;
                                    }
                                    break;
                                case EventResult.TIMEOUT:
                                    break;
                            }
                        }
                        break;
                }

            } while (true);

        }
        //public void Pro_Light(CancellationTokenSource tokenSource)
        //{
        //    try
        //    {
        //        Ting(tokenSource);
        //        TLPM tlpm = null;
        //        double CalibPower = 0;
        //        double wavelen = 1;
        //        double power = 0;
        //        this.LocalResource.IOs[IONameEnum_CT40410A.Output_Middle_ShadCylinder].TurnOn(true);
        //        if (this.LocalResource.TMPL_Master.InitializePM(ref tlpm) == false)
        //        {
        //            while (true)
        //            {
        //                if (tokenSource.IsCancellationRequested)
        //                {
        //                    tokenSource.Token.ThrowIfCancellationRequested();
        //                    return;
        //                }
        //                this.UserRequest_Pause(MT.吸嘴1模组);
        //                Form_TLPM form_TLPM = new Form_TLPM();
        //                form_TLPM.ShowDialog();
        //                this.UserRequest_Resume(MT.吸嘴1模组);
        //                if (form_TLPM.DialogResult == DialogResult.OK)
        //                {
        //                    if (!this.LocalResource.TMPL_Master.InitializePM(ref tlpm) == false)
        //                    {
        //                        break;
        //                    }
        //                }
        //                else if (form_TLPM.DialogResult == DialogResult.Cancel)
        //                {
        //                    this.Log_Global("[Step2]未发现 Thorlabs 功率计探头");
        //                    tokenSource.Cancel();
        //                    return;
        //                }
        //            }
        //        }
        //        if (double.TryParse(this.Step2Wavelen, out wavelen) == false)
        //        {
        //            MessageBox.Show("目标波长值格式错误!");
        //            tokenSource.Cancel();
        //            return;
        //        }
        //        for (int i = 0; i < 5; i++)
        //        {
        //            Ting(tokenSource);
        //            if (tokenSource.IsCancellationRequested)
        //            {
        //                tokenSource.Token.ThrowIfCancellationRequested();
        //                return;
        //            }
        //            tlpm.setWavelength(wavelen);
        //            tlpm.setPowerAutoRange(true);

        //            //读取功率
        //            tlpm.measPower(out CalibPower);
        //            CalibPower *= 1000000;  //W->uW
        //            power += CalibPower;
        //            Thread.Sleep(100);
        //        }
        //        this.Pro_Light_uW = power / 5;
        //        tlpm.Dispose();
        //        //this.LocalResource.IOs[IONameEnum_CT40410A.Output_Middle_ShadCylinder].TurnOn(false);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Log_Global($"{ex.Message}-{ex.StackTrace}");
        //        throw new Exception($"{ex.Message}-{ex.StackTrace}");
        //    }
        //}
        public bool RunMain(CancellationTokenSource tokenSource)
        {
            try
            {
                this.Log_Global($"%%%%%%%%%%%开始测试%%%%%%%%%%!");
                //测试模块 
                DeviceStreamData_CT40410A data_demo = new DeviceStreamData_CT40410A();
                data_demo.SerialNumber = $"{this.loadbox}_{this.index[0]}_{BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)}"; //BaseDataConverter.ConvertDateTimeToCommentString(DateTime.Now);
                //data_demo.DeviceInfo.PartNumber = "PartNumber";
                data_demo.OperatorID = this.OperatorID;

                bool isTest = true;

                this.index.RemoveAt(0);
                if (TrunOn)
                {
                    var rate = Math.Round((Light_uW - Pro_Light_uW) / Light_uW, 2);

                    data_demo.AtritionRate = (rate * 100).ToString();
                    //_mainStreamData.Clear();
                    _mainStreamData.AddToDataCollection(data_demo);
                    if (rate * 100 <= this.AtritionRate)
                    {
                        this.TestUnits[MT.测试站1.ToString()].RunMainExecutorCombo
                                            (
                                                data_demo,
                                                new Action(this.UpdateMainStreamDataToUI),
                                                null,
                                                null,
                                                tokenSource
                                            );

                        //this.UpdateMainStreamDataToUI_FocusInTargetDevice(data_demo.SerialNumber);
                        //this.ReCalibrateSummaryData(data_demo.SummaryDataCollection.ItemCollection, (this._mainStreamData.TestImportProfile as TestPluginImportProfile_CT40410A).UserDefinedCalibrationData_Loader);

                        string binCollectionName = this.binCollectionName;
                        var BinSettingCollection = this.LocalResource.Local_BinSortList_ResourceProvider.GetBinSettingCollectionObject(binCollectionName) as BinSettingCollection;
                        data_demo.BIN = data_demo.GetBinGrade(BinSettingCollection);

                        var box = this.parameterInformation.gearBox[(Gear)Enum.Parse(typeof(Gear), data_demo.BIN)];
                        data_demo.OutPutScann = this.parameterInformation.UnLoadScann[box];
                        data_demo.BinNumber = this.parameterInformation.PositinIndex[box].ToString();

                        this.TestUnits[MT.测试站1.ToString()].RunPostExecutorCombo(data_demo, null, null, null, tokenSource);
                    }
                    else
                    {
                        isTest = false;
                        this.LocalResource.IOs[IONameEnum_CT40410A.Output_Middle_ShadCylinder].TurnOn(false);
                        data_demo.BIN = "NG";
                        var box = this.parameterInformation.gearBox[(Gear)Enum.Parse(typeof(Gear), data_demo.BIN)];
                        data_demo.OutPutScann = this.parameterInformation.UnLoadScann[box];
                        data_demo.BinNumber = this.parameterInformation.PositinIndex[box].ToString();
                    }
                }
                else
                {
                    _mainStreamData.AddToDataCollection(data_demo);
                    this.TestUnits[MT.测试站1.ToString()].RunMainExecutorCombo
                                            (
                                                data_demo,
                                                new Action(this.UpdateMainStreamDataToUI),
                                                null,
                                                null,
                                                tokenSource
                                            );

                    //this.UpdateMainStreamDataToUI_FocusInTargetDevice(data_demo.SerialNumber);
                    //this.ReCalibrateSummaryData(data_demo.SummaryDataCollection.ItemCollection, (this._mainStreamData.TestImportProfile as TestPluginImportProfile_CT40410A).UserDefinedCalibrationData_Loader);

                    string binCollectionName = this.binCollectionName;
                    var BinSettingCollection = this.LocalResource.Local_BinSortList_ResourceProvider.GetBinSettingCollectionObject(binCollectionName) as BinSettingCollection;
                    data_demo.BIN = data_demo.GetBinGrade(BinSettingCollection);

                    var box = this.parameterInformation.gearBox[(Gear)Enum.Parse(typeof(Gear), data_demo.BIN)];
                    data_demo.OutPutScann = this.parameterInformation.UnLoadScann[box];
                    data_demo.BinNumber = this.parameterInformation.PositinIndex[box].ToString();

                    this.TestUnits[MT.测试站1.ToString()].RunPostExecutorCombo(data_demo, null, null, null, tokenSource);
                }



                this._mainStreamData.Station_1_Move_To_OutUP(data_demo);

                #region 存表格
                string dir = Path.GetFullPath(Application.StartupPath) + @"\Data\" + DateTime.Now.ToString("yyyyMMdd");


                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string fulldir = dir + @"\" + data_demo.OutPutScann;
                if (!Directory.Exists(fulldir))
                {
                    Directory.CreateDirectory(fulldir);
                }

                var defaultFileName = string.Concat(data_demo.BIN + "_" + data_demo.BinNumber + "_" + data_demo.SerialNumber + "_", BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now), FileExtension.CSV);

                var finalFileName = $@"{fulldir}\{defaultFileName}";

                string name = "序列号,损耗率,BIN,BinNumber,出料信息";
                string infor = $"{data_demo.SerialNumber},{data_demo.AtritionRate},{data_demo.BIN},{data_demo.BinNumber},{data_demo.OutPutScann}";
                using (StreamWriter sw = new StreamWriter(finalFileName, false, Encoding.GetEncoding("gb2312")))
                {
                    sw.WriteLine(name);
                    sw.WriteLine(infor);
                    if (isTest)
                    {
                        sw.WriteLine();
                        sw.WriteLine(data_demo.SummaryDataCollection.ToString());

                        sw.WriteLine(data_demo.RawDataCollection[0].ToString());
                        sw.WriteLine();
                    }
                    

                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                this.Log_Global($"{ex.Message}-{ex.StackTrace}");
                throw new Exception($"{ex.Message}-{ex.StackTrace}");
            }

        }

        public void ToTestPlace(CancellationTokenSource tokenSource)
        {
            this.Log_Global("[Step4]左侧吸嘴移动到测试位");
            //左吸嘴Z原点
            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSZ_左吸嘴_原点位_Z, SequenceOrder.Normal);

            this.Sequence_MoveToAxesPositionByOrder(AxesPositionEnum_CT40410A.LSX_左吸嘴_测试位_X, SequenceOrder.Normal);

        }



    }

}
