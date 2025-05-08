using SolveWare_BinSorter;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestPlugin;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT3103 : TestPluginWorkerBase, ITesterCoreLink
    {

        public MajorStreamData_CT3103 _mainStreamData;

        public ProviderManager_CT3103 providerResourse;

        public SqlConnStr SqlConn;

        internal TestFlowAutoResetEvents_WithPauseFunc_CT3103 Bridges_WithPauseFunc { get; set; }
        internal TestPluginResourceProvider_CT3103 LocalResource
        {
            get
            {
                return (TestPluginResourceProvider_CT3103)this._resourceProvider;
            }
        }

        //测试链中Bin的名字
        public string binCollectionName { get; set; }

        public string Purpose { get; set; }
        public string OperatorID { get; set; }
        public string MaskName { get; set; }
        public string WaferName { get; set; }
        public string ChipName { get; set; }
        public string WorkOrder { get; set; }
        public TestPluginWorker_CT3103()
        {
            this._mainStreamData = new MajorStreamData_CT3103();

            providerResourse = new ProviderManager_CT3103();
            //bizManager_CT3103 = new PluginBizManager();

            //ErrorCode_CT3103.InitErrorMap();

        }


        public override void StartUp()
        {
            try
            {
                this._myTokenSource = new CancellationTokenSource();
                this._resourceProvider = CreateResourceProviderInstance();
                if (this.RunResourceProvider() == false)
                {
                    throw new Exception($"组件[{this.Name}]导入资源失败!");
                }

                this.Initialize(this._myTokenSource.Token);


                providerResourse.ConnectToCore(this._coreInteration);
                providerResourse.ReinstallController();

                //bizManager_CT3103.ConnectToCore(_coreInteration);
                //bizManager_CT3103.Setup(LocalResource, providerResourse);


                this.CreateTestUnits();
                this.CreateTestEnteranceUI();
                this.CreateRuntimeOverviewUI();
                this.CreateMainUI();
                //(this._MainPageUI as Form_MainPage_CT3103).AllocateBizManager(ref bizManager_CT3103);
                (this._MainPageUI as ITesterAppUI).RefreshOnce();
            }
            catch (Exception ex)
            {
                throw new Exception($"{this.Name}启动错误:{ex.Message}{ex.StackTrace}！");
            }
        }

        public Form GetBinSortEditorUI()
        {
            var frm_bin = (Form)(this.LocalResource.Local_BinSortList_ResourceProvider as ITesterAppPluginUIModel).GetUI();
            this._coreInteration.ModifyDockableUI(frm_bin, true);
            return frm_bin;
        }

        public override void CreateMainUI()
        {
            if (this._MainPageUI == null || this._MainPageUI.IsDisposed)
            {
                this._MainPageUI = new Form_MainPage_CT3103();
                this._coreInteration.LinkToCore(_MainPageUI as ITesterAppUI);
                (this._MainPageUI as ITesterAppUI).ConnectToAppInteration(this);
                this._coreInteration.GUIRunUIInvokeAction(() =>
                {
                    Thread.Sleep(100);
                    this._MainPageUI.Show();
                    Thread.Sleep(100);
                    this._MainPageUI.Hide();
                });
            }
        }

        protected override void CreateTestEnteranceUI()
        {
            if (this._TestEnteranceUI == null || this._TestEnteranceUI.IsDisposed)
            {
                this._TestEnteranceUI = new Form_TestEnterance_CT3103();
                (this._TestEnteranceUI as ITesterAppUI).ConnectToAppInteration(this);
                this._coreInteration.LinkToCore(_TestEnteranceUI as ITesterAppUI);
                this._coreInteration.GUIRunUIInvokeAction(() =>
                {
                    Thread.Sleep(100);
                    this._TestEnteranceUI.Show();
                    Thread.Sleep(100);
                    this._TestEnteranceUI.Hide();
                });
            }
        }


        public override IMajorStreamData GetMajorStreamData()
        {
            return this._mainStreamData;
        }


        public override bool StartTest()
        {
            if (this._interation.RunStatus == PluginRunStatus.Ready)
            {
                if (this._tokenSource.IsCancellationRequested)
                {
                    this._tokenSource = new CancellationTokenSource();
                }

                this.Log_Global($"测试方案导入完成!");


                SetSignal();

                return true;
            }
            else
            {
                this.Log_Global($"测试组件状态为[{this._interation.RunStatus}]!测试不会开始!");
                return false;
            }
        }


        public override bool StopTest()
        {
            try
            {
                PauseSignalManager.Instance.Force_Clear_AllPauseSignal();
                return base.StopTest();
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public override bool ResetWorker()
        {
            try
            {
                this._mainStreamData = new MajorStreamData_CT3103();
                foreach (var unit in this.TestUnits.Values)
                {
                    unit.Clear();
                }
                this.ClearMainStreamDataUI();

                this.Interation.RunStatus = PluginRunStatus.Idle;

                return true;
            }
            catch (Exception ex)
            {
                this.ReportException($"重置测试组件[{this.Name}]失败!", ErrorCodes.TestPluginResetError, ex);
            }
            return false;
        }


        public void Device_DryRun()
        {
            try
            {
                if (this._tokenSource.IsCancellationRequested)
                {
                    this._tokenSource = new CancellationTokenSource();
                }
                this._mainStreamData = new MajorStreamData_CT3103();

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        this._interation.RunStatus = PluginRunStatus.Running;
                        this.Log_Global($"初始化测试信号..");
                        this.RunInitialAction(_tokenSource);

                        this.Log_Global("开始运行[前置]测试步骤...");
                        this.RunPreAction(_tokenSource);

                        this.Log_Global("开始运行[主要]测试步骤...");
                        //刷新开始时间
                        this.RunMainAction_DryRun(_tokenSource);

                        this.Log_Global("开始运行[后置]测试步骤...");
                        this.RunPostAction(_tokenSource);

                        this.GetMajorStreamData().SaveDirectly();

                        this.RunFinishAction(_tokenSource);
                        this._interation.RunStatus = PluginRunStatus.Finished;
                    }
                    catch (OperationCanceledException oce)
                    {
                        Paused_IO(1.5);
                        this.Log_Global($"[{this.Name}]用户{this._coreInteration.CurrentUserInfo}停止测试!!!");
                        this._interation.RunStatus = PluginRunStatus.Error;
                    }

                });
            }
            catch (Exception)
            {


            }
        }

        public override bool CheckTestProfileContext(ITestPluginImportProfileBase obj, out string checkLog)
        {
            bool isOk = true;
            checkLog = string.Empty;
            try
            {

                this.Log_Global($"正在检查测试配置[{obj.Name}]...");

                bool checkExecutor_Pre = false;
                bool checkExecutor_Main = false;
                bool checkExecutor_Post = false;

                object[] args = new object[] { this.Name, checkExecutor_Pre, checkExecutor_Main, checkExecutor_Post };
                isOk = obj.Check(out checkLog, args);
                if (isOk == true)
                {
                    this.Log_Global($"测试配置[{obj.Name}]检查通过!");
                }
                else
                {
                    this.Log_Global($"测试配置[{obj.Name}]检查未通过:\r\n:[{checkLog}]!");
                }
            }
            catch (Exception ex)
            {
                isOk = false;
            }
            return isOk;
        }


        public override bool ImportTestProfile(ITestPluginImportProfileBase obj, out string importLog)
        {
            bool isOk = true;
            importLog = string.Empty;
            try
            {
                //var isNgBinReady = this.providerResourse.OutPutSettings_Provider.OutputSettings.IsNG_Include();
                //if (isNgBinReady == false)
                //{
                //    var msg = $"测试方案[{obj.Name}]本地化失败,未配置NG下料区域！";
                //    importLog += $@"{msg}{Environment.NewLine}";
                //    this.Log_Global(msg);
                //    return false;
                //}
                TestPluginImportProfile_CT3103 localProfile = obj as TestPluginImportProfile_CT3103;
                if (localProfile == null)
                {
                    var msg = $"测试方案[{obj.Name}]本地化失败!";
                    importLog += $@"{msg}{Environment.NewLine}";
                    this.Log_Global(msg);
                    return false;
                }
                var staion_1_key = MT.测试站1;

                //if (localProfile.UserDefinedCalibrationData_PosLoader.Exists(item => item.Name == Constant_CT3103.Station_1_PosLoader_CalibrationKey) == false)
                //{
                //    var msg = $"测试方案[{obj.Name}]本地化失败!未配置[{Constant_CT3103.Station_1_PosLoader_CalibrationKey}]!";
                //    importLog += $@"{msg}{Environment.NewLine}";
                //    this.Log_Global(msg);
                //    return false;
                //}
                //if (localProfile.UserDefinedCalibrationData_NegLoader.Exists(item => item.Name == Constant_CT3103.Station_1_NegLoader_CalibrationKey) == false)
                //{
                //    var msg = $"测试方案[{obj.Name}]本地化失败!未配置[{Constant_CT3103.Station_1_NegLoader_CalibrationKey}]!";
                //    importLog += $@"{msg}{Environment.NewLine}";
                //    this.Log_Global(msg);
                //    return false;
                //}
                #region 测试站1号

                if (localProfile.TestExecutorComboDict.ContainsKey(MT.测试站1.ToString()) == false)
                {
                    var msg = $"测试方案[{obj.Name}]并不包含[{staion_1_key}]上的测试项目!";
                    importLog += $@"{msg}{Environment.NewLine}";
                    this.Log_Global(msg);
                    return false;
                }
                if (this.TestUnits.ContainsKey(MT.测试站1.ToString()) == false)
                {
                    var msg = $"组件内并不包含[{staion_1_key}]上的测试执行器!";
                    importLog += $@"{msg}{Environment.NewLine}";
                    this.Log_Global(msg);
                    return false;
                }

                if (this.TestUnits[MT.测试站1.ToString()].SetupTestExecutors_WithDynamicTestParams(
                    this._resourceProvider,
                    localProfile.TestExecutorComboDict[staion_1_key.ToString()],
                    localProfile.TestParamsConfigComboDict[staion_1_key.ToString()],
                    localProfile.Spec) == false)

                {
                    var msg = $"测试执行器[{staion_1_key}]上的测试项目加载失败!";
                    importLog += $@"{msg}{Environment.NewLine}";
                    this.Log_Global(msg);
                    return false;
                }

                #endregion
                #region 测试站2号
                //var staion_2_key = MT.测试站2;
                //if (localProfile.TestExecutorComboDict.ContainsKey(MT.测试站2.ToString()) == false)
                //{
                //    var msg = $"测试方案[{obj.Name}]并不包含[{staion_2_key}]上的测试项目!";
                //    importLog += $@"{msg}{Environment.NewLine}";
                //    this.Log_Global(msg);
                //    return false;
                //}
                //if (this.TestUnits.ContainsKey(MT.测试站2.ToString()) == false)
                //{
                //    var msg = $"组件内并不包含[{staion_2_key}]上的测试执行器!";
                //    importLog += $@"{msg}{Environment.NewLine}";
                //    this.Log_Global(msg);
                //    return false;
                //}

                //if (this.TestUnits[MT.测试站2.ToString()].SetupTestExecutors_WithDynamicTestParams(
                //    this._resourceProvider,
                //    localProfile.TestExecutorComboDict[staion_2_key.ToString()],
                //    localProfile.TestParamsConfigComboDict[staion_2_key.ToString()],
                //    localProfile.Spec) == false)

                //{
                //    var msg = $"测试执行器[{staion_2_key}]上的测试项目加载失败!";
                //    importLog += $@"{msg}{Environment.NewLine}";
                //    this.Log_Global(msg);
                //    return false;
                //}

                #endregion

                //this._mainStreamData = new MajorStreamData_CT3103();


                //this._mainStreamData.FileName = localProfile.FileName;//文档名称

                //var userDefineTesterNumber = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TESTER_NUMBER);

                //if (string.IsNullOrEmpty(userDefineTesterNumber) == true)
                //{
                //    this._mainStreamData.TesterNumber = Environment.MachineName;
                //}
                //else
                //{
                //    this._mainStreamData.TesterNumber = userDefineTesterNumber;
                //}

                //this._mainStreamData.TesterNumber = Environment.MachineName;


                //this._mainStreamData.DeviceNumber = localProfile.TestProfileName;//测试档
                //this._mainStreamData.Specification = localProfile.TestProfileName;//测试档
                //this._mainStreamData.LotNumber = localProfile.TestProfileName;


                //this._mainStreamData.Operator = this._coreInteration.CurrentUserID;

                //this._mainStreamData.Operator = this.OperatorID;


                //this._mainStreamData.CustomerNote1 = "测试档内容_解析测试档后写入";//
                //this._mainStreamData.CustomerRemark = "机台Gain值系数";//
                //this._mainStreamData.TotalTested = 0;
                //this._mainStreamData.Samples = 0;

                if (string.IsNullOrEmpty(localProfile.BinCollectionName) == false)
                {
                    var colls = this.LocalResource.Local_BinSortList_ResourceProvider.GetBinSettingCollectionObject(localProfile.BinCollectionName) as BinSettingCollection;
                    this._mainStreamData.BinSummaryDataNames.AddRange(colls.ItemCollection.First().GetDataListByPropName<string>("Name"));
                }

                this._mainStreamData.AttachedTestImportProfile(localProfile);
                //以下部分移动到 测试开始按钮事件


                //var path = this.GetStreamDataPath();
                //this._mainStreamData.LastSaveDataPath = path;
                //this._mainStreamData.Save(path);

                //StreamDataHandler_GaN_V2.CreateWaferSummaryFile(  $@"{DataPaths.Instance.Path_WaferFile}\{this._mainStreamData.FileName}", this._mainStreamData);

                //this.Log_Global($"测试方案导入完成!");
                //this.Log_Global($"测试主数据文件已存为[{path}]!");
                this._interation.RunStatus = PluginRunStatus.Ready;
            }
            catch (Exception ex)
            {
                importLog += $@"测试方案导入错误:[{ex.Message}{ex.StackTrace}]";
                isOk = false;
            }
            return isOk;
        }




        public override void Dev()
        {
        }





        protected override bool AlertCancellation()
        {
            this.Log_Global($"%%%%%%%%%%%测试流程被取消%%%%%%%%%%!");
            (this._TestEnteranceUI as Form_TestEnterance_CT3103).Controls_Enable_True();
            Paused_IO(1.5);
            return true;
        }


        protected override bool AlertError()
        {
            this.Log_Global($"%%%%%%%%%%%测试流程发生错误%%%%%%%%%%!");
            (this._TestEnteranceUI as Form_TestEnterance_CT3103).Controls_Enable_True();
            Paused_IO(1.5);
            return true;
        }


        protected override bool AlertInvalidity()
        {
            this.Log_Global($"%%%%%%%%%%%测试流程发生AlertInvalidity%%%%%%%%%%!");
            (this._TestEnteranceUI as Form_TestEnterance_CT3103).Controls_Enable_True();
            Paused_IO(1.5);
            //throw new NotImplementedException();
            return true;
        }





        protected override void AuxiRun(ref CancellationTokenSource tokenSource)
        {
            const int heartBeatInterval_s = 2;
            do
            {
                try
                {
                    this.WaitSignal();
                    do
                    {
                        if (this._resourceProvider.MonitorKeyResourceStatus(tokenSource) == false)
                        {
                            break;
                        };
                        for (int second = 0; second < heartBeatInterval_s; second++)
                        {
                            Thread.Sleep(1000);
                            tokenSource.Token.ThrowIfCancellationRequested();
                        }
                    }
                    while (true);
                }
                catch (OperationCanceledException oce)
                {
                    //取消  任何动作都不做
                }
                catch (Exception ex)
                {
                    this.ReportException($"[{this.Name}]测试过程运行错误  Run(ref CancellationTokenSource tokenSource)!!!", ErrorCodes.TestPluginRuntimeUnexpectedError, ex);
                    tokenSource.Cancel();
                }
            } while (true);
        }


        public bool TestPauseFunc
        {
            get
            {
                return PauseSignalManager.Instance.IsAnyOwnerPaused;
            }
        }

        //初始化信号
        protected override bool RunInitialAction(CancellationTokenSource tokenSource)
        {
            //初始化所有公共桥梁（blocking zones)
            //this.Bridges = new TestFlowAutoResetEvents_CT3103();
            //this.Bridges.Initialize();

            PauseSignalManager.Instance.Initialize();

            this.Bridges_WithPauseFunc = new TestFlowAutoResetEvents_WithPauseFunc_CT3103();
            this.Bridges_WithPauseFunc.PauseFunc = () => { return TestPauseFunc; };
            this.Bridges_WithPauseFunc.Initialize();
            //this._InternalParams = new InternalParams();




            return true;
        }

        //复位设备，仪器仪表
        protected override bool RunPreAction(CancellationTokenSource tokenSource)
        {
            try
            {
                var pass = this.Run_HomeStation();
                if (!pass)
                {
                    tokenSource.Cancel();
                }
                this.LocalResource.IOs[IONameEnum_CT3103.TWR_GRN].TurnOn(true);
                return pass;
            }
            catch (Exception)
            {
                return false;
            }



        }


        protected override bool RunMainAction(CancellationTokenSource tokenSource)
        {
            this.Log_Global($"%%%%%%%%%%%测试流程开始%%%%%%%%%%!");

            Task task1 = Task.Factory.StartNew(() => { LaserX_Step1_Grad(tokenSource); }, TaskCreationOptions.LongRunning);
            Task task2 = Task.Factory.StartNew(() => { LaserX_Step2_TemperatureControl_To_Carrier_Left(tokenSource); }, TaskCreationOptions.LongRunning);
            Task task3 = Task.Factory.StartNew(() => { LaserX_Step3_TemperatureControl_To_Carrier_Right(tokenSource); }, TaskCreationOptions.LongRunning);
            Task task4 = Task.Factory.StartNew(() => { LaserX_Stpe4_Test_Stations(tokenSource); }, TaskCreationOptions.LongRunning);
            Task task5 = Task.Factory.StartNew(() => { LaserX_Step5_TurningOfMaterial(tokenSource); }, TaskCreationOptions.LongRunning);
            //Task task6 = Task.Factory.StartNew(() => { LaserX_Step6_UnLoadProduct(tokenSource); }, TaskCreationOptions.LongRunning);
            Thread.Sleep(500);
            Task.WaitAll(new Task[] { task1 , task2, task3 , task4, task5 });//, task6
            (this._TestEnteranceUI as Form_TestEnterance_CT3103).Controls_Enable_True();
            RefreshCarrierID("Finished");
            RefreshOeskID("Finished");
            this.Log_Global($"%%%%%%%%%%%测试流程结束%%%%%%%%%%!");
            return true;
        }


        protected override bool RunPostAction(CancellationTokenSource tokenSource)
        {
            return true;
        }

        protected override bool RunFinishAction(CancellationTokenSource tokenSource)
        {
            try
            {
                //var pass = this.Run_HomeStation();
                //if (!pass)
                //{
                //    tokenSource.Cancel();
                //}
                Frm_ResetPlatform frm = new Frm_ResetPlatform();
                this.Reset_HomeStation();
                frm.ConnectToAppInteration(this);
                frm.ConnectToCore(this._coreInteration);
                frm.Homestation += new Frm_ResetPlatform.HomeStation(this.Run_HomeStation);
                frm.CancelHome += new Frm_ResetPlatform.CancelHomeStation(this.Cancel_HomeStation);
                frm.LocalResource = this.LocalResource;
                frm.ShowDialog();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        //界面测试按钮，无动作
        public void test(DeviceStreamData_CT3103 data_demo, CancellationTokenSource tokenSource)
        {
            this.TestUnits[MT.测试站1.ToString()].RunMainExecutorCombo
                                        (
                                            data_demo,
                                            new Action(this.UpdateMainStreamDataToUI),
                                            null,
                                            null,
                                            tokenSource
                                        );
        }
        //空跑
        public bool RunMainAction_DryRun(CancellationTokenSource tokenSource)
        {

            this.Log_Global($"%%%%%%%%%%%设备空跑流程开始%%%%%%%%%%!");
            Task task1 = Task.Factory.StartNew(() => { LaserX_Step1_Grad(tokenSource); }, TaskCreationOptions.LongRunning);
            Task task2 = Task.Factory.StartNew(() => { LaserX_Step2_TemperatureControl_To_Carrier_Left(tokenSource); }, TaskCreationOptions.LongRunning);
            Task task3 = Task.Factory.StartNew(() => { LaserX_Step3_TemperatureControl_To_Carrier_Right(tokenSource); }, TaskCreationOptions.LongRunning);
            Task task4 = Task.Factory.StartNew(() => { LaserX_Stpe4_Test_Stations(tokenSource); }, TaskCreationOptions.LongRunning);
            Task task5 = Task.Factory.StartNew(() => { LaserX_Step5_TurningOfMaterial(tokenSource); }, TaskCreationOptions.LongRunning);
            //Task task6 = Task.Factory.StartNew(() => { LaserX_Step6_UnLoadProduct(tokenSource); }, TaskCreationOptions.LongRunning);
            Thread.Sleep(500);
            Task.WaitAll(new Task[] { task1, task2, task3, task4, task5 });//, task6
            (this._TestEnteranceUI as Form_TestEnterance_CT3103).Controls_Enable_True();
            this.Log_Global($"%%%%%%%%%%%设备空跑流程结束%%%%%%%%%%!");
            return true;
        }
        public override void EMERGENCY_STOP()
        {
            //StopTest();
            MessageBox.Show("EMERGENCY_STOP");
        }

        public override void Close()
        {
            LocalResource.IOs[IONameEnum_CT3103.TWR_YEL].TurnOn(false);
            LocalResource.IOs[IONameEnum_CT3103.TWR_RED].TurnOn(false);
            LocalResource.IOs[IONameEnum_CT3103.TWR_GRN].TurnOn(false);
            LocalResource.IOs[IONameEnum_CT3103.BEEP].TurnOn(false);
            LocalResource.IOs[IONameEnum_CT3103.LAMP_BTN_STR].TurnOn(false);
            LocalResource.IOs[IONameEnum_CT3103.LAMP_BTN_RST].TurnOn(false);
            LocalResource.IOs[IONameEnum_CT3103.LAMP_BTN_STP].TurnOn(false);
        }

        //--------------------------20230505新增
        /// <summary>
        /// 暂停
        /// </summary>
        public void UserRequest_MasterControl_Pause()
        {
            try
            {
                Paused_IO(3);
                PauseSignalManager.Instance.Request_Pause(MT.主控组);
                string msg = $"{MT.主控组}申请暂停运行成功!";
                this.Log_Global(msg);
                this.Log_Global(PauseSignalManager.Instance.PrintStatus());
            }
            catch (Exception ex)
            {
                string msg = $"{MT.主控组}申请暂停运行操作失败:{ex.Message}{ex.StackTrace}!";
                this.Log_Global(msg);
                MessageBox.Show(msg);
            }

        }
        /// <summary>
        /// 恢复
        /// </summary>
        public void UserRequest_MasterControl_Resume()
        {
            try
            {
                Resume_IO();
                PauseSignalManager.Instance.Force_Clear_AllPauseSignal();
                string msg = $"{MT.主控组}申请恢复运行成功!";
                this.Log_Global(msg);
                this.Log_Global(PauseSignalManager.Instance.PrintStatus());

            }
            catch (Exception ex)
            {
                string msg = $"{MT.主控组}申请恢复运行操作失败:{ex.Message}{ex.StackTrace}!";
                this.Log_Global(msg);
                MessageBox.Show(msg);
            }
        }
        //20230505新增-------------------------------//

        public bool IsAnyonePause()
        {
            return PauseSignalManager.Instance.IsAnyOwnerPaused;
        }

        public void RefreshCarrierID(string carrID)
        {
            (this._TestEnteranceUI as Form_TestEnterance_CT3103).RefreshCarrierID(carrID);
        }
        public void RefreshOeskID(string oeskID)
        {
            (this._TestEnteranceUI as Form_TestEnterance_CT3103).RefreshOeskID(oeskID);
        }


        #region 上下料人工操作
        public void UserRequest_Pause(MT mT)
        {
            try
            {
                Paused_IO(1.5);
                PauseSignalManager.Instance.Request_Pause(mT);
                string msg = $"{mT}申请暂停运行成功!";
                this.Log_Global(msg);
                this.Log_Global(PauseSignalManager.Instance.PrintStatus());
            }
            catch (Exception ex)
            {
                string msg = $"{mT}申请暂停运行操作失败:{ex.Message}{ex.StackTrace}!";
                this.Log_Global(msg);
                MessageBox.Show(msg);
            }
        }
        public void UserRequest_Resume(MT mT)
        {
            try
            {
                Resume_IO();
                PauseSignalManager.Instance.Request_Resume(mT);
                string msg = $"{mT}申请恢复运行成功!";
                this.Log_Global(msg);
                this.Log_Global(PauseSignalManager.Instance.PrintStatus());

            }
            catch (Exception ex)
            {
                string msg = $"{mT}申请恢复运行操作失败:{ex.Message}{ex.StackTrace}!";
                this.Log_Global(msg);
                MessageBox.Show(msg);
            }
        }


        #endregion 上下料人工操作
        #region MyRegion

        //public override void CreateTestUnits()
        //{
        //    var unitConfigs = (this.ConfigItem as TestPluginConfigItem).TestExecutorUnitConfigs;
        //    this.TestUnits.Clear();

        //    foreach (var config in unitConfigs)
        //    {
        //        TestExecutorUnit_CT3103 tUnit = new TestExecutorUnit_CT3103(config.UnitName, config/*, this._resourceProvider*/);
        //        tUnit.ConnectToCore(this._coreInteration);

        //        MT defaultMT = default(MT);
        //        if (Enum.TryParse(config.UnitName, out defaultMT))
        //        {
        //            this.TestUnits.Add(config.UnitName, tUnit);
        //            //this.TestUnits.Add(defaultMT, tUnit);
        //        }
        //        else
        //        {
        //            throw new Exception($"测试执行器创建失败:[{config.UnitName}]不能本地化为所需枚举项目!");
        //        }
        //    }
        //}

        //public override bool CanCurrentOwnerAccessResource(GenernalResourceOwner currnetResourceOwner, string resourceOwnerName)
        //{
        //    switch (currnetResourceOwner)
        //    {
        //        case GenernalResourceOwner.Platform:
        //        case GenernalResourceOwner.Plugin:
        //            {
        //                return true;
        //            }
        //            break;
        //        default:
        //            return false;
        //    }
        //}

        #endregion

        protected override void CreateRuntimeOverviewUI()
        {
            if (this._RuntimeOverviewUI == null || this._RuntimeOverviewUI.IsDisposed)
            {
                this._RuntimeOverviewUI = new Form_PluginRuntimeOverview_CT3103();
                this._coreInteration.LinkToCore((ITestDataViewer)_RuntimeOverviewUI);

                const int testModuleWithRawDataChart_Count = 30;
                const int testModuleWithRawDataChart_Count_ff = 20;
                this._coreInteration.GUIRunUIInvokeAction(() =>
                {
                    this._RuntimeOverviewUI.Show();
                    Thread.Sleep(100);
                    (this._RuntimeOverviewUI as Form_PluginRuntimeOverview_CT3103).InitializeDataChartsPage(testModuleWithRawDataChart_Count, testModuleWithRawDataChart_Count_ff);
                    Thread.Sleep(100);
                    this._RuntimeOverviewUI.Hide();

                });
            }
        }

    }
}