using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_TestPlugin
{
    public abstract class TestPluginWorkerBase : TesterAppPluginUIModel, ITesterCoreLink, ITestPluginWorkerBase
    {
        const string _defaultStreamDataPath = "TEST_STREAM_DATA_PATH";
        protected Form _TestEnteranceUI;
        protected Form _RuntimeOverviewUI;
        protected TestPluginResourceProvider _resourceProvider;
        /// <summary>
        /// 线程取消控制源
        /// </summary>
        protected CancellationTokenSource _tokenSource = new CancellationTokenSource();
        protected TestPluginInteration _interation = new TestPluginInteration();
        protected Dictionary<string, TestExecutorUnit> TestUnits = new Dictionary<string, TestExecutorUnit>();
        /// <summary>
        /// 主线程-运行老化流程
        /// </summary>
        protected Task _mainTask;
        /// <summary>
        /// 辅助线程-运行硬件监控流程及报警
        /// </summary>
        protected Task _auxiTask;
        /// <summary>
        /// 线程锁
        /// </summary>s
        protected ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        public TestPluginWorkerBase()
        {

        }
        public virtual TestPluginInteration Interation
        {
            get
            {
                return this._interation;
            }
        }
        public abstract bool ResetWorker();

        /// <summary>
        /// 启动方法 由测试框架调用
        /// 建立通过配置建立测试单元
        /// 建立测试入口窗体
        /// 建立数据预览窗体
        /// 建立测试组件主窗体
        /// 建立测试组件资源收集器
        /// 导入测试组件所需资源
        /// 初始化组件主线程与辅助线程
        /// </summary>
        public override void StartUp()
        {
            this._myTokenSource = new CancellationTokenSource();
            this.CreateTestUnits();
            this.CreateTestEnteranceUI();
            this.CreateRuntimeOverviewUI();
            this.CreateMainUI();
            //this.CreateResourceProvider();
            this._resourceProvider = CreateResourceProviderInstance();
            if (this.RunResourceProvider() == false)
            {
                throw new Exception($"组件[{this.Name}]导入资源失败!");
            }
            this.Initialize(this._myTokenSource.Token);
        }
        /// <summary>
        /// 由start up 调用
        /// 初始化组件主线程与辅助线程
        /// </summary>
        /// <param name="token"></param>
        protected override void Initialize(CancellationToken token)
        {
            if (this._mainTask == null ||
             this._mainTask.Status == TaskStatus.RanToCompletion)
            {
                this._mainTask = Task.Factory.StartNew(() => this.Run(ref this._tokenSource));
            }
            if (this._auxiTask == null ||
                this._auxiTask.Status == TaskStatus.RanToCompletion)
            {
                this._auxiTask = Task.Factory.StartNew(() => this.AuxiRun(ref this._tokenSource));
            }
        }
        /// <summary>
        /// 组件访问权限设置供框架调用来查询是否能访问组件UI
        /// </summary>
        /// <param name="currnetResourceOwner"></param>
        /// <param name="ResourceOwnerName"></param>
        /// <returns></returns>
        public override bool CanCurrentOwnerAccessResource(GenernalResourceOwner currnetResourceOwner, string ResourceOwnerName)
        {
            switch (currnetResourceOwner)
            {
                case GenernalResourceOwner.Platform:
                    {
                        return true;
                    }
                    break;
                case GenernalResourceOwner.Plugin:
                    {
                        if (ResourceOwnerName == this.Name)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    break;
                default:
                    return false;
            }
        }
        /// <summary>
        /// 尝试获取平台资源使用权
        /// </summary>
        /// <returns></returns>
        public virtual bool TryAllocateResourceToPlugin()
        {
            return this._coreInteration.TryAllocateResourceToPlugin(this.Name);
        }
        /// <summary>
        /// 尝试释放平台资源使用权
        /// </summary>
        /// <returns></returns>
        public virtual bool TryAllocateResourceToPlatform()
        {
            return this._coreInteration.TryAllocateResourceToPlatform();
        }
        /// <summary>
        /// 赋值组件配置 由框架调用
        /// </summary>
        /// <param name="configItem"></param>
        public override void SetConfigItem(AppPluginConfigItem configItem)
        {
            base.SetConfigItem(configItem);

            this._interation.Name = this.Name;
  
            this._interation.RunStatus = PluginRunStatus.NotHomeYet;
            this._interation.OnlineStatus = this.ConfigItem.PluginEnable ? PluginOnlineStatus.Online : PluginOnlineStatus.Offline;
        }
        /// <summary>
        /// 获取stream data存储路径
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual string GetStreamDataPath(params object[] args)
        {
            var streamDataPath = this._coreInteration?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_STREAM_DATA_PATH);
            if (string.IsNullOrEmpty(streamDataPath))
            {
                streamDataPath = _defaultStreamDataPath;
            }
            string dir = $@"{streamDataPath}\StreamData_{BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)}";
            if (Directory.Exists(dir) == false)
            {
                Directory.CreateDirectory(dir);
            }

            string filePath = $@"{dir}\StreamData_{BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now)}{FileExtension.XML}";

            return filePath;
        }
        public virtual string GetStreamDataDirectory(params object[] args)
        {
            var streamDataPath = this._coreInteration?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_STREAM_DATA_PATH);
            if (string.IsNullOrEmpty(streamDataPath))
            {
                streamDataPath = _defaultStreamDataPath;
            }
            return streamDataPath;
        }
        /// <summary>
        /// 获取组件major  stream data
        /// </summary>
        /// <returns></returns>
        public abstract IMajorStreamData GetMajorStreamData();
        /// <summary>
        /// 创建测试入口ui
        /// </summary>
        protected abstract void CreateTestEnteranceUI();
        /// <summary>
        /// 获取测试入口UI
        /// </summary>
        /// <param name="isDockable"></param>
        /// <returns></returns>
        public virtual Form GetTestEnteranceUI(bool isDockable)
        {
            this.CreateTestEnteranceUI();
            this._coreInteration.ModifyDockableUI(this._TestEnteranceUI, isDockable);
            return this._TestEnteranceUI;
        }
        /// <summary>
        /// 创建测试数据浏览UI
        /// </summary>
        protected virtual void CreateRuntimeOverviewUI()
        {
            if (this._RuntimeOverviewUI == null || this._RuntimeOverviewUI.IsDisposed)
            {
                this._RuntimeOverviewUI = new Form_PluginRuntimeOverview();
                this._coreInteration.LinkToCore((ITestDataViewer)_RuntimeOverviewUI);
                this._coreInteration.GUIRunUIInvokeAction(() =>
                {
                    Thread.Sleep(100);
                    this._RuntimeOverviewUI.Show();
                    Thread.Sleep(100);
                    this._RuntimeOverviewUI.Hide();
                });
            }
        }
        /// <summary>
        /// 获取测试数据浏览UI
        /// </summary>
        /// <param name="isDockable"></param>
        /// <returns></returns>
        public virtual Form GetRuntimeOverviewUI(bool isDockable)
        {
            this.CreateRuntimeOverviewUI();
            this._coreInteration.ModifyDockableUI(this._RuntimeOverviewUI, isDockable);
            return this._RuntimeOverviewUI;
        }
        /// <summary>
        /// 更新数据到相关UI - 一般为数据浏览UI
        /// </summary>
        public virtual void UpdateMainStreamDataToUI()
        {
            ((ITestDataViewer)this._RuntimeOverviewUI).UpdateMainStreamData(this.GetMajorStreamData());
        }
        public virtual void UpdateMainStreamDataToUI_FocusInTargetDevice(string serialNum)
        {
            ((ITestDataViewer)this._RuntimeOverviewUI).UpdateMainStreamData(this.GetMajorStreamData(), serialNum);
        }


        public virtual void ClearMainStreamDataUI()
        {
            ((ITestDataViewer)this._RuntimeOverviewUI).Clear();// (this.GetMajorStreamData());
        }
        public abstract void EMERGENCY_STOP();

        /// <summary>
        /// 停止测试任务
        /// </summary>
        /// <returns></returns>
        public virtual bool StopTest()
        {
            //它将所有的worker的主控token source 调用cancel  
            if (this._tokenSource.IsCancellationRequested == false)
            {
                this._tokenSource.Cancel();
            }
            return true;
        }
        /// <summary>
        /// 开始测试任务
        /// </summary>
        /// <returns></returns>
        public virtual bool StartTest()
        {
            if (this._interation.RunStatus == PluginRunStatus.Ready)
            {
                if (this._tokenSource.IsCancellationRequested)
                {
                    this._tokenSource = new CancellationTokenSource();
                }
                SetSignal();
                return true;
            }
            else
            {
                this.Log_Global($"当前组件[{this.Name}]运行状态为[{this._interation.RunStatus}]开始测试任务动作将不会执行.");
            }
            return false;
        }
        /// <summary>
        /// 创建测试单元- 通过配置文件
        /// </summary>
        public virtual void CreateTestUnits()
        {
            var unitConfigs = (this.ConfigItem as TestPluginConfigItem).TestExecutorUnitConfigs;
            this.TestUnits.Clear();

            foreach (var config in unitConfigs)
            {
                var tUnit = this._coreInteration.CreateInstanceClassInAppPluginDlls<TestExecutorUnit>
                    (
                        config.UnitType,
                        config.UnitName, config
                    );

                tUnit.ConnectToCore(this._coreInteration);
                tUnit.Setup();
                this.TestUnits.Add(config.UnitName, tUnit);
            }
        }
        /// <summary>
        /// 检测测试方案内容
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="checkLog"></param>
        /// <returns></returns>
        public abstract bool CheckTestProfileContext(ITestPluginImportProfileBase obj, out string checkLog);
        /// <summary>
        /// 导入测试方案
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="importLog"></param>
        /// <returns></returns>
        public abstract bool ImportTestProfile(ITestPluginImportProfileBase obj, out string importLog);
        /// <summary>
        /// 获取本地化的资源供应器
        /// </summary>
        /// <returns></returns>
        public virtual TestPluginResourceProvider GetPluginLocalizedResourceProvider()
        {
            try
            {
                var rp = this.CreateResourceProviderInstance();
                rp?.ClearResource();
                rp?.Setup((TestPluginConfigItem)this.ConfigItem);
                rp?.LocalizeResource();

                return rp;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// 创建资源供应器实例
        /// </summary>
        /// <returns></returns>
        public virtual TestPluginResourceProvider CreateResourceProviderInstance()
        {
            var rp = this._coreInteration.CreateInstanceClassInAppPluginDlls<TestPluginResourceProvider>((this.ConfigItem as TestPluginConfigItem).PluginResourceProviderType);
            this._coreInteration.LinkToCore(rp);
            return rp;
        }
        /// <summary>
        /// 清空资源供应器内资源
        /// </summary>
        /// <returns></returns>
        public virtual bool ClearResourceProvider()
        {
            try
            {
                this._resourceProvider?.ClearResource();
            }
            catch (Exception ex)
            {
                var errMsg = $"RunResourceLauncher exception:[{ex.Message}]-[{ex.StackTrace}]";
                this.Log_Global(errMsg);
                this.ReportException(errMsg, ErrorCodes.TestPluginRuntimeUnexpectedError, ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 运行资源供应器
        /// </summary>
        /// <returns></returns>
        public virtual bool RunResourceProvider()
        {
            try
            {
                this._resourceProvider?.ClearResource();
                this._resourceProvider?.Setup((TestPluginConfigItem)this.ConfigItem);
                this._resourceProvider?.LocalizeResource();
            }
            catch (Exception ex)
            {
                var errMsg = $"RunResourceLauncher exception:[{ex.Message}]-[{ex.StackTrace}]";
                this.Log_Global(errMsg);
                this.ReportException(errMsg, ErrorCodes.TestPluginRuntimeUnexpectedError, ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 所有运行异常退出的后处理方法 - 一般为关闭硬件输出 并设置运行灯状态
        /// </summary>
        /// <returns></returns>
        protected abstract bool AlertError();
        /// <summary>
        /// 所有产品失效退出的后处理方法 - 一般为关闭硬件输出 并设置运行灯状态
        /// </summary>
        /// <returns></returns>
        protected abstract bool AlertInvalidity();
        /// <summary>
        /// 取消操作的后处理方法 - 一般为关闭硬件输出 并设置运行灯状态
        /// </summary>
        /// <returns></returns>
        protected abstract bool AlertCancellation();
        protected abstract bool RunInitialAction(CancellationTokenSource tokenSource);
        protected abstract bool RunFinishAction(CancellationTokenSource tokenSource);
        /// <summary>
        /// 测试之前的事务运行方法 - 复位机器/检查仪表
        /// </summary>
        /// <returns></returns>s
        protected abstract bool RunPreAction(CancellationTokenSource tokenSource);
        /// <summary>
        /// 程序主任务 - 生成产品数据流/运动/视觉/测试结合主部分/测试后向内核提交数据
        /// </summary>
        /// <returns></returns>
        protected abstract bool RunMainAction(CancellationTokenSource tokenSource);
        /// <summary>
        /// 测试之后的事务运行方法 - 一般为关闭所有硬件输出 设置单元运行信号灯等 - 可以发挥想象力
        /// </summary>
        /// <returns></returns>
        protected abstract bool RunPostAction(CancellationTokenSource tokenSource);
        /// <summary>
        /// 设置线程开启信号 - 让主副线程开始执行等待信号后方的流程
        /// </summary>
        protected virtual void SetSignal()
        {
            this._manualResetEvent.Set();
        }
        /// <summary>
        /// 线程等待方法
        /// </summary>
        protected virtual void WaitSignal()
        {
            do
            {
                if (this._manualResetEvent.WaitOne(10))
                {
                    this._manualResetEvent.Reset();
                    break;
                }

            } while (true);
        }
        /// <summary>
        /// 辅助线程运行内容
        /// </summary>
        /// <param name="tokenSource"></param>
        protected abstract void AuxiRun(ref CancellationTokenSource tokenSource);
        /// <summary>
        /// 主线程运行内容 - 不可修改
        /// </summary>
        /// <param name="tokenSource"></param>
        protected override void Run(ref CancellationTokenSource tokenSource)//也就是它  ， 所有分步内容都依靠这个token source的状态
        {
            bool isResourceAllocated = false;
            do
            {
                this.WaitSignal();
                //尝试获取系统资源使用权
                isResourceAllocated = this.TryAllocateResourceToPlugin();
                if (isResourceAllocated == false)
                {
                    this.Log_Global($"插件未取得平台资源使用权!测试任务将不会执行!");
                    continue;
                }
                this.Log_Global($"测试资源导入成功!");
                #region  测试运行主控 不建议修改 请联系BEN WANG.  and leave it alone

                try
                {
                    this._interation.RunStatus = PluginRunStatus.Running;
                    this.Log_Global($"初始化测试信号..");
                    this.RunInitialAction(tokenSource);

                    this.Log_Global("开始运行[前置]测试步骤...");
                    //if (!this.RunPreAction(tokenSource))
                    //{
                    //    return;
                    //}
                    this.RunPreAction(tokenSource);
                    this.Log_Global("开始运行[主要]测试步骤...");
                    //刷新开始时间
                    //if (!this.RunMainAction(tokenSource))
                    //{
                    //    return;
                    //}
                    this.RunMainAction(tokenSource);//这是main action ，
                    this.Log_Global("开始运行[后置]测试步骤...");
                    //if (!this.RunPostAction(tokenSource))
                    //{
                    //    return;
                    //}
                    this.RunPostAction(tokenSource);

                    this.GetMajorStreamData().SaveDirectly();

                    this.RunFinishAction(tokenSource);
                    this._interation.RunStatus = PluginRunStatus.Finished;
                }
                catch (AggregateException ae)
                {
                    try
                    {
                        //this.Log_Global($"[{this.Name}]测试过程运行发生未定义错误 Run(ref CancellationTokenSource tokenSource) exception:[{ae.Message}-{ae.StackTrace}]!!!");
                        this.ReportException($"[{this.Name}]测试过程运行发生未定义错误  Run(ref CancellationTokenSource tokenSource)!!!", ErrorCodes.TestPluginRuntimeUnexpectedError, ae);

                        this.GetMajorStreamData().SaveDirectly();
                        //this.CloseResourceProvider();
                        this.AlertError();
                    }
                    catch
                    {

                    }
                    this._interation.RunStatus = PluginRunStatus.Error;
                }
                catch (OperationCanceledException oce)
                {
                    try
                    {
                        this.Log_Global($"[{this.Name}]用户{this._coreInteration.CurrentUserInfo}停止测试!!!");
                        this.GetMajorStreamData().SaveDirectly();
                        //this.CloseResourceProvider();
                        this.AlertCancellation();
                    }
                    catch
                    {

                    }
                    this._interation.RunStatus = PluginRunStatus.Stopped;
                }
                catch (InvalidProgramException ipe)
                {
                    try
                    {
                        //this.Log_Global($"[{this.Name}]测试过程运行发生已定义错误 Run(ref CancellationTokenSource tokenSource) exception:[{ipe.Message}-{ipe.StackTrace}]!!!");
                        this.ReportException($"[{this.Name}]测试过程运行发生已定义错误  Run(ref CancellationTokenSource tokenSource)!!!", ErrorCodes.TestPluginRuntimeExpectedError, ipe);
                        this.GetMajorStreamData().SaveDirectly();
                        //this.CloseResourceProvider();
                        this.AlertInvalidity();
                    }
                    catch
                    {

                    }
                    this._interation.RunStatus = PluginRunStatus.Invalid;
                }
                catch (Exception ex)
                {
                    try
                    {
                        //this.Log_Global($"[{this.Name}]测试过程运行发生未定义错误 Run(ref CancellationTokenSource tokenSource) exception:[{ex.Message}-{ex.StackTrace}]!!!");
                        this.ReportException($"[{this.Name}]测试过程运行发生未定义错误  Run(ref CancellationTokenSource tokenSource)!!!", ErrorCodes.TestPluginRuntimeUnexpectedError, ex);
                        this.GetMajorStreamData().SaveDirectly();

                        this.AlertError();
                    }
                    catch
                    {
                    }
                    this._interation.RunStatus = PluginRunStatus.Error;
                }
                finally
                {
                    if (isResourceAllocated == true)
                    {
                        this.TryAllocateResourceToPlatform();
                    }
                }
                #endregion

            } while (true);
        }

        //public abstract bool SwitchProductConfig( );

    }
}