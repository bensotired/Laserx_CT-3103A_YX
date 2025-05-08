using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInException;
using SolveWare_BurnInLog;
using SolveWare_BurnInMessage;
using SolveWare_Data_AccessDatabase.Business;
using SolveWare_Permission;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.Specification;
using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SolveWare_TesterCore
{
    /// <summary>
    /// 2022.07.26 14:47pm
    /// BEN
    /// How can a man be brave if he is afraid?
    /// -That is the only time a man can be brave.
    /// </summary>
    public partial class TesterCore : PluginModel, ITesterCoreInteration, ITesterCoreHandleUIAction, ITesterCoreResource, ITesterAssembly, ITesterDataBaseOperation
    {
        const int GUI_Refresh_Interval_ms = 2 * 1000;
        const int CallGC_Interval_ms = 5 * 1000;
        static TesterCore _instance;
        static object _mutex = new object();
        string _versionInfo = string.Empty;
        ILogManager _LogManager;
        IExceptionHandle _ExceptionManager;
        protected System.Timers.Timer RefreshGuiElementTimer { get; set; }
        protected System.Timers.Timer CallGCTimer { get; set; }
        GenernalResourceOwner _genernalResourceOwner = GenernalResourceOwner.Platform;
        string _genernalResourceOwnerName = $"{GenernalResourceOwner.Platform}";
        string _currentProductName = $"未知";
        string _createProductName = $"未知";
        public static TesterCore Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new TesterCore();
                        }
                    }
                }
                return _instance;
            }
        }
        public GenernalResourceOwner ResourceOwner
        {
            get
            {
                return _genernalResourceOwner;
            }
        }
        public string ResourceOwnerName
        {
            get
            {
                return _genernalResourceOwnerName;
            }
        }
        public string CurrentProductName
        {
            get
            {
                return _currentProductName;
            }
        }
        public string CreateProductName
        {
            get
            {
                return _createProductName;
            }
        }

        public TestStationConfig StationConfig
        {
            get
            {
                return TestStationConfig.Instance;
            }
        }
        public string PlatformVersionInfomation
        {
            get { return _versionInfo; }
        }
        public Action<IMessage> SendMessageToGuiAction
        {
            get;
            set;
        }
        public Action<IMessage> SendExceptionMessageToGuiAction
        {
            get;
            set;
        }
        public Action<IMessage> SendToCore
        {
            get;
            set;
        }
        public Action<IMessage> GUISendToCore
        {
            get;
            set;
        }
        public AccessPermissionLevel CurrentAPL
        {
            get { return PermissionManager.Instance.APL; }
        }
        public string CurrentUserInfo
        {
            get
            {
                return PermissionManager.Instance.UserInfo;
            }
        }
        public string CurrentUserID
        {
            get
            {
                return PermissionManager.Instance.CurrentUserID;
            }
        }
        public event SendMessageOutEventHandler SendOutFormCoreEvent;
        public event SendMessageOutEventHandler SendOutFormCoreToGUIEvent;
        public event SendMessageOutEventHandlerWithReturn SendOutFormCoreToGUIEventWithReturn;
        public TesterCore()
        {
            var wAsm = this.GetType().Assembly.GetName();
            _versionInfo = $"版本号[{ wAsm.Version}]";
            try
            {
                IEnumerable<Attribute> bDateAtt = this.GetType().Assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute));
                _versionInfo += $"创建时间[{ (new List<Attribute>(bDateAtt).First() as AssemblyFileVersionAttribute).Version}]";
            }
            catch
            {
            }
        }


        protected override void Initialize(CancellationToken token)
        {
            try
            {
                this.SendToCore += new Action<IMessage>(ReceiveMessage);
                //暂时保留主界面独立action  我已经忘记当初为什么要这样做 2022.01.10 BEN 
                this.GUISendToCore += new Action<IMessage>(ReceiveMessage);

                LogManager.Instance.Initialize(Application.StartupPath);
                ExceptionManager.Instance.Initialize(Application.StartupPath);
                this._LogManager = LogManager.Instance;
                this._ExceptionManager = ExceptionManager.Instance;
                TestStationConfig.Instance.Load();
                this._currentProductName = TestStationConfig.Instance.StartupProductName;

                AssemblyManager.Initialize(TestStationConfig.Instance.AppPluginPackDlls);

                this.LinkToCore(GUIFactory.Instance);
                this.DockingMessageBoard();

                this.LinkToCore(PermissionManager.Instance);
                PermissionManager.Instance.StartUp();

                this.LinkToCore(TestStationManager.Instance);
                TestStationManager.Instance.StartUp();

                this.LinkToCore(TestFrameManager.Instance);
                TestFrameManager.Instance.StartUp();

                this.LinkToCore(AccessDatabaseManager.Instance);
                AccessDatabaseManager.Instance.StartUp();

                this.Log_Global($"默认使用产品[{this._currentProductName}]相关配置...");
                this.Log_Global("正在初始化功能组件...");
                this.LinkToCore(TesterAppPluginManager.Instance);

                TesterAppPluginManager.Instance.Setup(TestStationConfig.Instance);
 

                TestStationManager.Instance.InitializeInstruments();
                TestStationManager.Instance.InitializeMonitors();

                //加载app组件页面
                this.LayoutAppPluginTabPage();
                //加载测试综合配置页面
                this.DockingTestFrameBoard();
                //加载硬件配置页面
                this.DockingStationBoard();
                //最后才加载测试组件主页

                this.DockingTestPluginMainPages();
                this.NotifyUI_CreateTestPluginStatusInfo();
                this.NotifyUI_CreateMonitorStatusInfo();
                this.NotifyUI_TestStation_Initialized();
                this.NotifyUI_ProductConfigChanged();

                Thread.Sleep(2000);
                this.RefreshGuiElementTimer = new System.Timers.Timer(GUI_Refresh_Interval_ms);
                this.RefreshGuiElementTimer.Elapsed += RefreshGuiElementTimer_Elapsed;
                this.RefreshGuiElementTimer.Start();

                this.CallGCTimer = new System.Timers.Timer(CallGC_Interval_ms);
                this.CallGCTimer.Elapsed += CallGCTimer_Elapsed;
                this.CallGCTimer.Start();


                this.Log_Global($"测试系统初始化完成.");
            }
            catch (Exception ex)
            {
                this.Log_Global($"系统初始化错误:[{ex.Message}-{ex.StackTrace}]");
            }
        }

        private void RefreshGuiElementTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.NotifyUI_UpdateMonitorStatusInfo();
                this.NotifyUI_UpdateTestPluginStatusInfo();
            }
            catch
            {

            }
        }

        private void CallGCTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GCFunctions.ForceReleaseResouces();
        }

        public object GetStationHardwareObject(string hardwareName)
        {
            if (TestStationManager.Instance.InstrumentDict.ContainsKey(hardwareName))
            {
                return TestStationManager.Instance.InstrumentDict[hardwareName];
            }
            return new object();
        }
        public Dictionary<string, string> GetStationInstrumentSimpleInfos()
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            foreach (var instKvp in TestStationManager.Instance.InstrumentDict)
            {
                temp.Add(instKvp.Key, instKvp.Value.GetType().Name);
            }
            return temp;
        }
   
        public object GetAuxiliaryHardwareObject(string auxiHardwareName)
        {
            if (TestStationManager.Instance.AuxiliaryInstrumentDict.ContainsKey(auxiHardwareName))
            {
                return TestStationManager.Instance.AuxiliaryInstrumentDict[auxiHardwareName];
            }
            return new object();
        }


        public bool TryAllocateResourceToPlatform()
        {
            this._genernalResourceOwner = GenernalResourceOwner.Platform;
            this._genernalResourceOwnerName = $"{GenernalResourceOwner.Platform}";
            return true;
        }
        public bool TryAllocateResourceToPlugin(string name)
        {
            switch (this._genernalResourceOwner)
            {
                case GenernalResourceOwner.Platform:
                    {
                        if (TesterAppPluginManager.Instance.GetTestPlugInKeys().Contains(name))
                        {
                            this._genernalResourceOwner = GenernalResourceOwner.Plugin;
                            this._genernalResourceOwnerName = name;
                        }
                        return true;
                    }
                    break;
                case GenernalResourceOwner.Plugin:
                    {
                        if (this._genernalResourceOwnerName == name)
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
        public IEnumerable<ITesterAppPluginInteration> GetAppPlugins()
        {
            return TesterAppPluginManager.Instance.GetAppPlugins();
        }
        public ITesterAppPluginInteration GetAppPlugin(string apKey)
        {
            return TesterAppPluginManager.Instance.GetAppPlugin(apKey);
        }
        public string[] GetTestPlugInKeys()
        {
            return TesterAppPluginManager.Instance.GetTestPlugInKeys();
        }
        public ITesterAppPluginInteration GetTestPlugin(string apKey)
        {
            return TesterAppPluginManager.Instance.GetTestPlugin(apKey);
        }
        void LayoutAppPluginTabPage()
        {
            try
            {
                var appPlugins = TesterAppPluginManager.Instance.GetAppPlugins();
                foreach (var ap in appPlugins)
                {
                    if (ap.IsPlugin_PF_UIVisible == true)
                    {
                        if (this.CanUserAccessDomain(ap.APL))
                        {
                            DockUIForAppPlugin(ap);
                        }
                        else
                        {
                            HideUIForAppPlugin(ap);
                        }
                    }
                }
            }
            catch
            {

            }
        }
        public void ForceDockAppPluginTabPage()
        {
            try
            {
                var appPlugins = TesterAppPluginManager.Instance.GetAppPlugins();
                foreach (var ap in appPlugins)
                {
                    if (ap.IsPlugin_PF_UIVisible == true)
                    {
                        DockUIForAppPlugin(ap);
                    }
                }
            }
            catch
            {
            }
        }
        void NotifyUI_ProductConfigChanged()
        {
            this.SendToUI(new InternalMessage("", InternalOperationType.NotifyUI_ProductConfigChanged, this._currentProductName));
        }
        void NotifyUI_TestStation_Initialized()
        {
            this.SendToUI(new InternalMessage("", InternalOperationType.TestStation_Initialized, null));
        }
        void NotifyUI_CreateMonitorStatusInfo()
        {
            this.SendToUI(new InternalMessage("", InternalOperationType.CoreRequest_CreateMonitorStatusInfo, TestStationManager.Instance.MonitorStatus));
        }
        void NotifyUI_UpdateMonitorStatusInfo()
        {
            this.SendToUI(new InternalMessage("", InternalOperationType.CoreRequest_UpdateMonitorStatusInfo, TestStationManager.Instance.MonitorStatus));
        }

        void NotifyUI_CreateTestPluginStatusInfo()
        {
            Dictionary<string, Tuple<string, PluginOnlineStatus, PluginRunStatus>> temp = new Dictionary<string, Tuple<string, PluginOnlineStatus, PluginRunStatus>>();
            foreach (ITestPluginWorkerBase item in TesterAppPluginManager.Instance.GetTestPlugins().Cast<ITestPluginWorkerBase>())
            {
                temp.Add(item.Interation.Name,
                Tuple.Create
                (
                    item.Interation.PluginStatusInfo,
                    item.Interation.OnlineStatus,
                    item.Interation.RunStatus
                ));
            }
            this.SendToUI(new InternalMessage("", InternalOperationType.CoreRequest_CreateTestPluginStatusInfo, temp));
        }
        void NotifyUI_UpdateTestPluginStatusInfo()
        {
            Dictionary<string, Tuple<string, PluginOnlineStatus, PluginRunStatus>> temp = new Dictionary<string, Tuple<string, PluginOnlineStatus, PluginRunStatus>>();
            foreach (ITestPluginWorkerBase item in TesterAppPluginManager.Instance.GetTestPlugins().Cast<ITestPluginWorkerBase>())
            {
                temp.Add(item.Interation.Name,
                Tuple.Create
                (
                    item.Interation.PluginStatusInfo,
                    item.Interation.OnlineStatus,
                    item.Interation.RunStatus
                ));
            }
            this.SendToUI(new InternalMessage("", InternalOperationType.CoreRequest_UpdateTestPluginStatusInfo, temp));
        }
        public void DockUIForAppPlugin(ITesterAppPluginInteration plugin)
        {
            this.SendToUI(new InternalMessage(plugin.Name, InternalOperationType.Layout_CreateOrDockTesterAppTabPage, plugin));
        }
        public void HideUIForAppPlugin(ITesterAppPluginInteration plugin)
        {
            this.SendToUI(new InternalMessage(plugin.Name, InternalOperationType.Layout_HideTesterAppTabPage, plugin));
        }
        public void PopUIForAppPlugin(ITesterAppPluginInteration plugin)
        {
            this.PopUI(plugin.GetUI());
        }
        public void LinkToCore(ITesterCoreLink coreLinkObjcet)
        {
            coreLinkObjcet.ConnectToCore(this);
        }
        public void UnlinkFromCore(ITesterCoreLink coreLinkObjcet)
        {
            coreLinkObjcet.DisconnectFromCore(this);
        }
        public void DockingTestPluginMainPage()
        {
            try
            {//毁灭吧  就是第一个
                var testPlugin = TesterAppPluginManager.Instance.GetTestPlugins().First();

                this.SendToUI(new InternalMessage(testPlugin.Name, InternalOperationType.CoreRequest_GUITestPluginMainPageDockingInvokeAction, testPlugin));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取信息窗口失败:[{ex.Message}-{ex.StackTrace}]!", "界面元素错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void DockingTestPluginMainPages()
        {
            try
            {
                //毁灭吧  全家加载
                var testPlugins = TesterAppPluginManager.Instance.GetTestPlugins().ToArray();
                for (int i = testPlugins.Length - 1; i >= 0; i--)
                {
                    var plugin = testPlugins[i];
                    if (plugin.IsPlugin_PF_UIVisible == true)
                    {
                        this.SendToUI(new InternalMessage(plugin.Name, InternalOperationType.CoreRequest_GUIMulti_TestPluginMainPageDockingInvokeAction, plugin));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取信息窗口失败:[{ex.Message}-{ex.StackTrace}]!", "界面元素错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public bool TryCreateProductConfig()
        {
            try
            {
                TestFrameManager.Instance.CreateProductConfig();

                var appPlugins = TesterAppPluginManager.Instance.GetAppPlugins();
                foreach (var ap in appPlugins)
                {
                    if (ap.IsPluginEnable == true)
                    {
                        var isOk = ap.CreateProductConfig();
                        if (isOk == false)
                        {
                            var msg = $"组件[{ap.Name}]新建产品配置参数失败!";
                            this.Log_Global(msg);
                            MessageBox.Show(msg, "切换产品配置参数错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                }
                //毁灭吧  全家加载
                var testPlugins = TesterAppPluginManager.Instance.GetTestPlugins().ToArray();
                foreach (var tp in testPlugins)
                {
                    if (tp.IsPluginEnable == true)
                    {
                        var isOk = tp.CreateProductConfig();
                        if (isOk == false)
                        {
                            MessageBox.Show($"组件[{tp.Name}]新建产品配置参数失败!", "切换产品配置参数错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"组件新建产品配置参数失败:[{ex.Message}-{ex.StackTrace}]!", "切换产品配置参数错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;


        }
        public bool CreateProductConfig(string prodName)
        {
            try
            {
                var preDr = MessageBox.Show
                     (
                         $"确认将当前产品[{this.CurrentProductName}]相关配置参数新建为产品[{prodName}]?",
                         "确认设置产品配置参数",
                         MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Question
                     );
                if (preDr != DialogResult.Yes)
                {
                    return false;
                }


                this._createProductName = prodName;
                if (this.TryCreateProductConfig() == true)
                {
                    var msg = $"新建产品[{this.CreateProductName}]相关配置参数成功!!";
                    this.Log_Global(msg);
                    //切换成功 问是否设置为启动产品类型
                    MessageBox.Show
                       (
                            msg,
                           "设置产品配置参数",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information
                       );
                }
                else
                {
                    this.Log_Global($"新建产品[{this.CreateProductName}]相关配置参数失败!!");

                }
                //NotifyUI_ProductConfigChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"组件切换产品配置参数失败:[{ex.Message}-{ex.StackTrace}]!", "切换产品配置参数错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }
        public bool TrySwitchProductConfig()
        {
            try
            {
                TestFrameManager.Instance.SwitchProductConfig();

                var appPlugins = TesterAppPluginManager.Instance.GetAppPlugins();
                foreach (var ap in appPlugins)
                {
                    if (ap.IsPluginEnable == true)
                    {
                        var isOk = ap.SwitchProductConfig();
                        if (isOk == false)
                        {
                            MessageBox.Show($"组件[{ap.Name}]切换产品配置参数失败!", "切换产品配置参数错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                }
                //毁灭吧  全家加载
                var testPlugins = TesterAppPluginManager.Instance.GetTestPlugins().ToArray();
                foreach (var tp in testPlugins)
                {
                    if (tp.IsPluginEnable == true)
                    {
                        var isOk = tp.SwitchProductConfig();
                        if (isOk == false)
                        {
                            MessageBox.Show($"组件[{tp.Name}]切换产品配置参数失败!", "切换产品配置参数错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"组件切换产品配置参数失败:[{ex.Message}-{ex.StackTrace}]!", "切换产品配置参数错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;


        }
        public bool SwitchProductConfig(string prodName)
        {
            try
            {
                var preDr = MessageBox.Show
                     (
                         $"确认将当前产品[{this.CurrentProductName}]相关配置参数切换为产品[{prodName}]?",
                         "确认设置产品配置参数",
                         MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Question
                     );
                if (preDr != DialogResult.Yes)
                {
                    return false;
                }
                var orgProdName = _currentProductName;

                this._currentProductName = prodName;
                if (this.TrySwitchProductConfig() == true)
                {
                    this.Log_Global($"切换产品[{this.CurrentProductName}]相关配置参数成功!!");
                    //切换成功 问是否设置为启动产品类型
                    MessageBox.Show
                       (
                           $"切换产品[{this.CurrentProductName}]相关配置参数成功!!",
                           "设置产品配置参数",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information
                       );
                    var qryDefMsg = $"是否将产品[{this.CurrentProductName}]相关配置参数设置为启动默认参数?";
                    this.Log_Global(qryDefMsg);
                    var dr = MessageBox.Show
                     (
                         qryDefMsg,
                         "设置产品配置参数为默认参数",
                         MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Information
                     );
                    if (dr == DialogResult.Yes)
                    {
                        TestStationConfig.Instance.StartupProductName = prodName;
                        TestStationConfig.Instance.Save();
                    }
                }
                else
                {
                    this.Log_Global($"切换产品[{this.CurrentProductName}]相关配置参数失败!!将自动设置为原产品[{orgProdName}]相关配置参数");
                    //切换失败 换回原来的
                    this._currentProductName = orgProdName;
                    this.TrySwitchProductConfig();
                }
                NotifyUI_ProductConfigChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"组件切换产品配置参数失败:[{ex.Message}-{ex.StackTrace}]!", "切换产品配置参数错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        void DockCoreMamberBoard(GUIType gt, InternalOperationType iot)
        {
            var uiObj = GUIFactory.Instance.GetGUI(gt);
            DockCoreMamberBoard(uiObj, iot);
        }
        void DockCoreMamberBoard(object uiObj, InternalOperationType iot)
        {
            try
            {
                this.SendOutFormCoreToGUIEvent?.Invoke(new InternalMessage("", iot, uiObj));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取信息窗口失败:[{ex.Message}-{ex.StackTrace}]!", "界面元素错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void DockingMessageBoard()
        {
            DockCoreMamberBoard(GUIType.GUI_MESSAGE_BOARD, InternalOperationType.CoreRequest_GUIMessageBoardDockingInvokeAction);
        }
        //public void DockingNanoTrakBoard()
        //{
        //    DockCoreMamberBoard(GUIType.GUI_NanoTrak_BOARD, InternalOperationType.CoreRequest_GUINanoTrakBoardDockingInvokeAction);

        //}
        public void DockingStationBoard()
        {
            DockCoreMamberBoard(GUIType.GUI_STATION_BOARD, InternalOperationType.CoreRequest_GUIStationBoardDockingInvokeAction);
        }
        public void DockingTestFrameBoard()
        {
            var ui = TestFrameManager.Instance.GetUI();
            DockCoreMamberBoard(ui, InternalOperationType.CoreRequest_GUITestFrameBoardDockingInvokeAction);
        }
        public void SendToUI(IMessage messageToUI)
        {
            try
            {
                this.SendOutFormCoreToGUIEvent?.Invoke(messageToUI);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"中转信息到UI失败:[{ex.Message}-{ex.StackTrace}]!", "中转信息错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void PopUI(object uiObject)
        {
            GUIFactory.Instance.PopUI(uiObject);
        }
        public void PopUI_DefaultSize(object uiObject)
        {
            GUIFactory.Instance.PopUI_DefaultSize(uiObject);
        }


        public override void Log_Global(string message)
        {
            this._LogManager?.Log_Global(message);
        }
        Action<IMessage> ILogHandle.SendMessageToGuiAction
        {
            get
            {
                return this._LogManager.SendMessageToGuiAction;
            }
            set
            {
                this._LogManager.SendMessageToGuiAction = value;
            }
        }
        Action<IMessage> IExceptionHandle.SendExceptionMessageToGuiAction
        {
            get
            {
                return this._ExceptionManager.SendExceptionMessageToGuiAction;
            }
            set
            {
                this._ExceptionManager.SendExceptionMessageToGuiAction = value;
            }
        }

        public List<InstrMonitor> InstrumentsMonitors
        {
            get
            {
                return TestStationManager.Instance.InstrumentMonitos;
            }
        }


        public bool CanUserAccessDomain(AccessPermissionLevel requestApl)
        {
            return PermissionManager.Instance.CanUserAccessDomain(requestApl);
        }
        public bool CanUserAccessResourceProviderUI(string appName)
        {
            try
            {
                var app = TesterAppPluginManager.Instance.GetAppPlugin(appName);
                if (app != null)
                {
                    return app.CanCurrentOwnerAccessResource(this._genernalResourceOwner, this._genernalResourceOwnerName);
                }
                var tApp = TesterAppPluginManager.Instance.GetTestPlugin(appName);
                if (tApp != null)
                {
                    return tApp.CanCurrentOwnerAccessResource(this._genernalResourceOwner, this._genernalResourceOwnerName);
                }
                return false;
            }
            catch
            {

            }
            return false;
        }
        public void GUIRunUIInvokeAction(Action guiInvokeAction)
        {
            this.SendOutFormCoreToGUIEvent?.Invoke(new InternalMessage("", InternalOperationType.CoreRequest_GUIRunUIInvokeAction, guiInvokeAction));
        }
        public void GUIRunUIInvokeActionSYNC(Action guiInvokeAction)
        {
            this.SendOutFormCoreToGUIEvent?.Invoke(new InternalMessage("", InternalOperationType.CoreRequest_GUIRunUIInvokeActionSYNC, guiInvokeAction));
        }
        public object GUIRunUIInvokeFunction(Func<object> guiInvokeFunc)
        {
            return this.SendOutFormCoreToGUIEventWithReturn?.Invoke(new InternalMessage("", InternalOperationType.CoreRequest_GUIRunUIInvokeFunc, guiInvokeFunc));
        }

        public TInstance CreateInstanceClassInAppPluginDlls<TInstance>(string className, params object[] constructorParams)
        {
            TInstance Obj = default(TInstance);
            try
            {
                var objType = AssemblyManager.GetTypeFromClassInAppPluginDlls(className);
                Obj = AssemblyManager.CreateInstance<TInstance>(className, objType, constructorParams);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.CreateTestRecipeFailed, ex);
            }
            return Obj;
        }
        public object CreateTestModule(string className, params object[] constructorParams)
        {
            ITestModule Obj = null;
            try
            {
                var objType = AssemblyManager.GetTypeFromClassInAppPluginDlls(className);
                Obj = AssemblyManager.CreateInstance<ITestModule>(className, objType, constructorParams);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.CreateTestModuleFailed, ex);
            }
            return Obj;
        }
        public object CreateTestRecipe(string className, params object[] constructorParams)
        {
            ITestRecipe Obj = null;
            try
            {
                var objType = AssemblyManager.GetTypeFromClassInAppPluginDlls(className);
                Obj = AssemblyManager.CreateInstance<ITestRecipe>(className, objType, constructorParams);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.CreateTestRecipeFailed, ex);
            }
            return Obj;
        }
        public object CreateTestRecipe(Type testRecipeType, params object[] constructorParams)
        {
            ITestRecipe Obj = null;
            try
            {
                Obj = AssemblyManager.CreateInstance<ITestRecipe>(testRecipeType.Name, testRecipeType, constructorParams);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.CreateTestRecipeFailed, ex);
            }
            return Obj;
        }
        public object CreateCalcRecipe(string className, params object[] constructorParams)
        {
            ICalcRecipe Obj = null;
            try
            {
                var objType = AssemblyManager.GetTypeFromClassInAppPluginDlls(className);
                Obj = AssemblyManager.CreateInstance<ICalcRecipe>(className, objType, constructorParams);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.CreateCalculatorRecipeFailed, ex);
            }
            return Obj;
        }
        public object CreateCalcRecipe(Type calcRecipeType, params object[] constructorParams)
        {
            ICalcRecipe Obj = null;
            try
            {
                //var objType = AssemblyManager.GetTypeFromClassInAppPluginDlls(className);
                Obj = AssemblyManager.CreateInstance<ICalcRecipe>(calcRecipeType.Name, calcRecipeType, constructorParams);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.CreateCalculatorRecipeFailed, ex);
            }
            return Obj;
        }
        public object CreateCalculator(string className, params object[] constructorParams)
        {
            ITestCalculator Obj = null;
            try
            {
                var objType = AssemblyManager.GetTypeFromClassInAppPluginDlls(className);
                Obj = AssemblyManager.CreateInstance<ITestCalculator>(className, objType, constructorParams);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.CreateCalculatorFailed, ex);
            }
            return Obj;
        }

        public object CreateSpecification(string className, params object[] constructorParams)
        {
            ISpecification Obj = null;
            try
            {
                var objType = AssemblyManager.GetTypeFromClassInAppPluginDlls(className);
                Obj = AssemblyManager.CreateInstance<ISpecification>(className, objType, constructorParams);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.CreateSpecificationFailed, ex);
            }
            return Obj;
        }
        public object LoadSpecification(string className, params object[] constructorParams)
        {
            ISpecification Obj = null;
            try
            {
                var objType = AssemblyManager.GetTypeFromClassInAppPluginDlls(className);
                Obj = AssemblyManager.CreateInstance<ISpecification>(className, objType, constructorParams);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.CreateSpecificationFailed, ex);
            }
            return Obj;
        }
        public object LoadTestExecutorConfigItem(string configXmlFile)
        {
            return TestFrameManager.Instance.LoadTestExecutorConfigItem(configXmlFile);
        }
        public object LoadTestProfile<TProfile>(string configXmlFile)
        {
            return TestFrameManager.Instance.LoadTestProfile<TProfile>(configXmlFile);
        }
        public object LoadTestProfileWithExtraTypes<TProfile>(string configXmlFile)
        {
            return TestFrameManager.Instance.LoadTestProfileWithExtraTypes<TProfile>(configXmlFile);
        }
        public object LoadTestExecutorCombo(string configXmlFile)
        {
            return TestFrameManager.Instance.LoadTestExecutorCombo(configXmlFile);
        }
        public object LoadTestExecutorComboWithParams(string configXmlFile)
        {
            return TestFrameManager.Instance.LoadTestExecutorComboWithParams(configXmlFile);
        }
        public Dictionary<string, string> GetLocalTestProfileFiles()
        {
            return TestFrameManager.Instance.GetLocalTestProfileFiles();
        }
        public Dictionary<string, string> GetLocalExecutorComboFiles()
        {
            return TestFrameManager.Instance.GetLocalExecutorComboFiles();
        }
        public List<Type> GetAssignableTypesFromPreLoadDlls(Type baseType)
        {
            return AssemblyManager.GetAssignableTypesFromPreLoadDlls(baseType);
        }
        public Type GetTypeFromClassInPreLoadDlls(string className)
        {
            return AssemblyManager.GetTypeFromClassInPreLoadDlls(className);
        }
        public List<Type> GetTypeFromClassInPreLoadDlls(List<string> className)
        {
            List<Type> types = new List<Type>();
            foreach (var cls in className)
            {
                types.Add(AssemblyManager.GetTypeFromClassInPreLoadDlls(cls));
            }
            return types;
        }
        public bool PreLoadDllsContainsClass(string className)
        {
            return AssemblyManager.PreLoadDllsContainsClass(className);
        }

        public object LoadTestRecipeInstance(string fileNameWithoutExtension)
        {
            return this.LoadInstanceFromFile_ByXmlRoot
             (
                $@"{this.GetProductConfigFileDirectory()}\{this.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_RECIPE_PATH)}",
                 fileNameWithoutExtension
             );
        }

        public object LoadCalcRecipeInstance(string fileNameWithoutExtension)
        {
            return this.LoadInstanceFromFile_ByXmlRoot
                (
                $@"{this.GetProductConfigFileDirectory()}\{this.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CALC_RECIPE_PATH)}",
                    //this.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CALC_RECIPE_PATH),
                    fileNameWithoutExtension
                );
        }
        #region MyRegion     

        void IExceptionHandle.ReportException(ExceptionMessage exMsg)
        {
            this._ExceptionManager.ReportException(exMsg);
        }
        void IExceptionHandle.ReportException(string message, int errorCode)
        {
            this._ExceptionManager.ReportException(message, errorCode);
        }
        void IExceptionHandle.ReportException(string message, int errorCode, Exception e)
        {
            this._ExceptionManager.ReportException(message, errorCode, e);
        }
        void IExceptionHandle.ReportException(string message, int errorCode, Exception e, object context)
        {
            this._ExceptionManager.ReportException(message, errorCode, e, context);
        }
        void ILogHandle.FormattedLog_File(string format, params object[] args)
        {
            this._LogManager?.FormattedLog_File(format, args);
        }
        void ILogHandle.FormattedLog_Global(string format, params object[] args)
        {
            this._LogManager?.FormattedLog_Global(format, args);
        }
        void ILogHandle.Log_File(string message)
        {
            this._LogManager?.Log_File(message);
        }
        void ILogHandle.Log_Global(string message)
        {
            this._LogManager?.Log_Global(message);
        }

        #endregion
        public void ModifyDockableUI(Form _UI, bool isDockable)
        {
            UIGeneric.ModifyDockableUI(_UI, isDockable);
        }
        public void RefreshListView(ListView listView, Dictionary<string, string> files)
        {
            UIGeneric.RefreshListView(listView, files);
        }
        public TreeNode Convert_TestExecutorComboToTreeNode(object comboConfig)
        {
            return TestFrameManager.Instance.Convert_TestExecutorComboToTreeNode((TestExecutorCombo)comboConfig);
        }

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public object LoadInstanceFromFile_ByXmlRoot(string directoryPath, string fileNameWithoutExtension)
        {
            string filepath = string.Empty;
            if (Directory.Exists(directoryPath) == false)
            {
                throw new DirectoryNotFoundException(directoryPath);
            }

            filepath = $@"{directoryPath}\{fileNameWithoutExtension}{FileExtension.XML}";
            if (File.Exists(filepath) == false)
            {
                throw new FileNotFoundException(filepath);
            }

            object objectInstance;
            try
            {
                var temp = XElement.Load(filepath);
                var rootNode = temp.Name.ToString();
                var type = this.GetTypeFromClassInPreLoadDlls(rootNode);
                objectInstance = XmlHelper.DeserializeXElement(temp, type);
                return objectInstance;
            }
            catch (Exception ex)
            {
                throw new Exception($"加载文件异常，异常原因：[{ex.Message}]");
            }
        }

        public void ShowLoginUI()
        {
            try
            {
                PermissionManager.Instance.ShowLoginUI();
            }
            catch (Exception ex)
            {

            }
        }

        public void ShowUserManageUI()
        {
            try

            {
                if (this.CanUserAccessDomain(AccessPermissionLevel.Admin))
                {
                    PermissionManager.Instance.ShowUserManageUI();
                }
                else
                {
                    MessageBox.Show($"当用户组[{this.CurrentAPL}]不可访问该页面!");

                }
            }
            catch (Exception ex)
            {

            }

        }

        public void RefreshLoginInfo()
        {

            this.SendToUI(new InternalMessage("", InternalOperationType.UserRequest_LoginStatusChanged, PermissionManager.Instance.UserInfo));
            this.ForceDockAppPluginTabPage();
            this.LayoutAppPluginTabPage();
        }

        public void RunTestExecutorDebugger(object eciObj)
        {
            try
            {
                this.GUIRunUIInvokeAction(() =>
                {

                    Form_TestExecutorDebugger debugger = new Form_TestExecutorDebugger();

                    this.LinkToCore(debugger);
                    if (debugger.Setup(eciObj))
                    {
                        debugger.ShowDialog();
                    }
                    else
                    {

                    }
                });
            }
            catch (Exception ex)
            {

            }
        }
        //public void RunTestExecutorDebugger(object eciObj, object tstParamObj)
        //{
        //    try
        //    {
        //        this.GUIRunUIInvokeAction(() =>
        //        {

        //            Form_TestExecutorDebugger_SimpleMode debugger = new Form_TestExecutorDebugger_SimpleMode();

        //            this.LinkToCore(debugger);
        //            if (debugger.Setup(eciObj, tstParamObj))
        //            {
        //                debugger.Show();
        //            }
        //            else
        //            {

        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}i
        public void RunTestExecutorDebugger(object eciObj, object tstParamObj)
        {
            try
            {
                this.GUIRunUIInvokeAction(() =>
                {

                    Form_DynamicTestExecutorDebugger_SimpleMode debugger = new Form_DynamicTestExecutorDebugger_SimpleMode();

                    this.LinkToCore(debugger);
                    if (debugger.Setup(eciObj, tstParamObj))
                    {
                        debugger.Show();
                    }
                    else
                    {

                    }
                });
            }
            catch (Exception ex)
            {

            }
        }

        public void RunTestExecutorCombo_ParamsEditor(object testExecutorComboWithParams)
        {
            try
            {
                this.GUIRunUIInvokeAction(() =>
                {

                    Form_TestExecutorCombo_ParamsEditor_SimpleMode frm = new Form_TestExecutorCombo_ParamsEditor_SimpleMode();

                    this.LinkToCore(frm);
                    if (frm.Setup(testExecutorComboWithParams))
                    {
                        frm.ShowDialog();
                    }
                    else
                    {

                    }
                });
            }
            catch (Exception ex)
            {

            }
        }



        public string Get_Create_ProductConfigFileFullPath(string constConfigFileName)
        {
            string configFileFullPath = Path.Combine(Application.StartupPath, "ProductConfig", $"{ this._createProductName}", constConfigFileName);
            return configFileFullPath;
        }
        public string GetProductConfigFileFullPath(string constConfigFileName)
        {
            string configFileFullPath = Path.Combine(Application.StartupPath, "ProductConfig", $"{ this._currentProductName}", constConfigFileName);
            return configFileFullPath;
        }
        public string GetProductConfigFileDirectory()
        {
            string configFileDirectory = Path.Combine(Application.StartupPath, "ProductConfig", $"{ this._currentProductName}");
            return configFileDirectory;
        }
        public string Get_Create_ProductConfigFileDirectory()
        {
            string configFileDirectory = Path.Combine(Application.StartupPath, "ProductConfig", $"{ this._createProductName}");
            return configFileDirectory;
        }
        public string[] GetProductConfigFolderNames()
        {
            string[] folders = new string[0];
            try
            {
                var productFolder = Path.Combine(Application.StartupPath, "ProductConfig");
                folders = Directory.GetDirectories(productFolder);

                for (int i = 0; i < folders.Length; i++)
                {
                    folders[i] = Path.GetFileName(folders[i]);
                }
            }
            catch
            {

            }
            return folders;
        }
        public bool TryReleaseResourceBeforeClosing()
        {
            try
            {
                this.CallGCTimer.Enabled = false;
                do
                {
                    Thread.Sleep(100);
                } while (this.CallGCTimer.Enabled == true);

                TesterAppPluginManager.Instance.Close();
                TestStationManager.Instance.Close();
                Environment.Exit(0);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TryReleaseResourceBeforeClosing exception[{ex.Message}{ex.StackTrace}]");
            }
            return false;
        }
        public void CopyDirectory(string sourceDir, string destinationDir, bool recursive = true)
        {
            try
            {
                // Get information about the source directory
                var dir = new DirectoryInfo(sourceDir);

                // Check if the source directory exists
                if (!dir.Exists)
                    throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

                // Cache directories before we start copying
                DirectoryInfo[] dirs = dir.GetDirectories();

                // Create the destination directory
                Directory.CreateDirectory(destinationDir);

                // Get the files in the source directory and copy to the destination directory
                foreach (FileInfo file in dir.GetFiles())
                {
                    string targetFilePath = Path.Combine(destinationDir, file.Name);
                    file.CopyTo(targetFilePath);
                }

                // If recursive and copying subdirectories, recursively call this method
                if (recursive)
                {
                    foreach (DirectoryInfo subDir in dirs)
                    {
                        string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                        CopyDirectory(subDir.FullName, newDestinationDir, true);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = $"复制文件夹[{sourceDir}]到目标文件夹[{destinationDir}]失败:{ex.Message}-{ex.StackTrace}！";
                this.Log_Global(msg);
                MessageBox.Show(msg);
            }
        }


        public virtual PropertyInfo[] CreateExeItem_TestRecipe_ParamBook(object exeItemObj)
        {
            PropertyInfo[] props = new PropertyInfo[0];
            if (exeItemObj is ExecutorConfigItem)
            {
                ExecutorConfigItem exeItem = exeItemObj as ExecutorConfigItem;

                var testModule = (ITestModule)this.CreateTestModule(exeItem.TestModuleClassName);
                var testRecipe = (ITestRecipe)this.CreateTestRecipe(testModule.GetTestRecipeType());
                props = PropHelper.GetEditableProperties(testRecipe);
            }
            return props;
        }
        public virtual Dictionary<string, PropertyInfo[]> CreateExeItem_CalcRecipe_ParamBook(object exeItemObj)
        {
            Dictionary<string, PropertyInfo[]> temp = new Dictionary<string, PropertyInfo[]>();
            if (exeItemObj is ExecutorConfigItem)
            {
                ExecutorConfigItem exeItem = exeItemObj as ExecutorConfigItem;
                List<TestCalculatorComboItem> tempCalculatorCombo = new List<TestCalculatorComboItem>();
                foreach (var calcKvp in exeItem.CalculatorCollection)
                {
                    var tempCalculator = (ITestCalculator)this.CreateCalculator(calcKvp.Key);
                    var tempCalcRecipe = (ICalcRecipe)this.CreateCalcRecipe(tempCalculator.GetCalcRecipeType());
                    var props = PropHelper.GetEditableProperties(tempCalcRecipe);
                    temp.Add(calcKvp.Value, props);
                }
            }
            return temp;
        }
        public virtual object CreateExeItem_TestRecipeInstance(object exeItemObj)
        {
            ITestRecipe rcp = null;
            if (exeItemObj is ExecutorConfigItem)
            {
                ExecutorConfigItem exeItem = exeItemObj as ExecutorConfigItem;

                var testModule = (ITestModule)this.CreateTestModule(exeItem.TestModuleClassName);
                rcp = (ITestRecipe)this.CreateTestRecipe(testModule.GetTestRecipeType());
            }
            return rcp;
        }
        public virtual DataBook<string, object> CreateExeItem_CalcRecipeInstances(object exeItemObj)
        {
            DataBook<string, object> temp = new DataBook<string, object>();
            if (exeItemObj is ExecutorConfigItem)
            {
                ExecutorConfigItem exeItem = exeItemObj as ExecutorConfigItem;
                List<TestCalculatorComboItem> tempCalculatorCombo = new List<TestCalculatorComboItem>();
                foreach (var calcKvp in exeItem.CalculatorCollection)
                {
                    var tempCalculator = (ITestCalculator)this.CreateCalculator(calcKvp.Key);
                    var tempCalcRecipe = (ICalcRecipe)this.CreateCalcRecipe(tempCalculator.GetCalcRecipeType());

                    temp.Add(calcKvp.Value, tempCalcRecipe);
                }
            }
            return temp;
        }
        public virtual object CreateExeItem_CalcRecipeInstance(string calculatorTypeName)
        {

            var tempCalculator = (ITestCalculator)this.CreateCalculator(calculatorTypeName);
            var tempCalcRecipe = (ICalcRecipe)this.CreateCalcRecipe(tempCalculator.GetCalcRecipeType());

            return tempCalcRecipe;
        }
    
    }
}