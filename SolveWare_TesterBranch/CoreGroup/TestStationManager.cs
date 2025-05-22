using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SolveWare_TesterCore
{
    public sealed class TestStationManager : TesterAppPluginModel
    {

        static TestStationManager _instance;
        static object _mutex = new object();

        public Dictionary<string, IInstrumentChassis> InstrumentChassisDict { get; set; }
        public Dictionary<string, IInstrumentBase> AuxiliaryInstrumentDict { get; set; }
        public Dictionary<string, IInstrumentBase> InstrumentDict { get; set; }
        public List<InstrMonitor> InstrumentMonitos { get; set; }
        public Dictionary<string, Tuple<string, InstrMonitorStatus>> MonitorStatus { get; private set; }
        public TestStationManager()
        {
            this.InstrumentChassisDict = new Dictionary<string, IInstrumentChassis>();
            this.AuxiliaryInstrumentDict = new Dictionary<string, IInstrumentBase>();
            this.InstrumentDict = new Dictionary<string, IInstrumentBase>();
            this.InstrumentMonitos = new List<InstrMonitor>();
            this.MonitorStatus = new Dictionary<string, Tuple<string, InstrMonitorStatus>>();
        }
        public static TestStationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new TestStationManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public override void StartUp()
        {
            this.Log_Global("正在初始化硬件设备...");
            var stationConfig = this._coreInteration.StationConfig;

            //创建所有实例
            bool isLucky = true;

            isLucky = this.CreateInstumentChassis(stationConfig);
            if (isLucky == false)
            {
                throw new Exception("CreateInstumentChassis Exception");
            }
            isLucky = this.CreateAuxiliaryInstuments(stationConfig);
            if (isLucky == false)
            {
                throw new Exception("CreateAuxiliaryInstuments Exception");
            }
            isLucky = this.CreateInstuments(stationConfig);
            if (isLucky == false)
            {
                throw new Exception("CreateInstuments Exception");
            }

            //连接所有通信底层
            //this.Log_Global("正在连接通信底层...");
            //foreach (var comChas in this.InstrumentChassisDict.Values)
            //{
            //    this.Log_Global($"正在连接通信底层[{comChas.Name}][{comChas.Resource}]默认在线状态[{comChas.IsOnline}]...");
            //    comChas.Initialize(comChas.InitTimeout_ms);
            //}
            InitializeInstrumentsChassis();

            this.Log_Global("硬件设备初始化完成.");
        }

        public void InitializeInstrumentsChassis()
        {
            //连接所有通信底层
            this.Log_Global("正在连接通信底层...");
            foreach (var comChas in this.InstrumentChassisDict.Values)
            {
                this.Log_Global($"正在连接通信底层[{comChas.Name}][{comChas.Resource}]默认在线状态[{comChas.IsOnline}]...");
                comChas.Initialize(comChas.InitTimeout_ms);
            }
        }

        public void InitializeInstruments()
        {
            //分配通信底层到仪器
            this.Log_Global("正在分配通信底层到辅助仪器...");
            foreach (var auxInst in this.AuxiliaryInstrumentDict.Values)
            {
                this.Log_Global($"正在分配通信底层到辅助仪器[{auxInst.Name}]ID[{auxInst.Address}]...");
                auxInst.Initialize();
            }
            this.Log_Global("正在分配通信底层到仪器...");
            foreach (var inst in this.InstrumentDict.Values)
            {
                this.Log_Global($"正在分配通信底层到辅助仪器[{inst.Name}]ID[{inst.Address}]...");
                inst.Initialize();
            }
        }
        public void InitializeMonitors()
        {
            var stationConfig = this._coreInteration.StationConfig;
            this.Log_Global("正在建立仪器监听...");
            this.CreateInstrumentMonitors(stationConfig);
            this.StartUpMonitor();
        }

        private bool CreateAuxiliaryInstuments(TestStationConfig stationConfig)
        {
            try
            {
                this.Log_Global("正在初始化辅助仪器...");
                this.AuxiliaryInstrumentDict.Clear();
                foreach (var instConfig in stationConfig.AuxiliaryInstrumentConfigs)
                {
                    if (this.InstrumentChassisDict.Keys.Contains(instConfig.ChassisName) == false)
                    {
                        string msg = string.Format("Unable to find chassis [{0}] when creating instrument [{1}]", instConfig.ChassisName, instConfig.Name);
                        this.ReportException(msg, ErrorCodes.InstrumentChassisNotFound);
                        return false;
                    }
                    else
                    {
                        object[] paramsObjs = new object[] { instConfig.Name, instConfig.Address, this.InstrumentChassisDict[instConfig.ChassisName] };
                        this.Log_Global($"正在初始化辅助仪器[{instConfig.Name}]ID[{instConfig.Address}]通过底层[{instConfig.ChassisName}]默认在线状态[{instConfig.IsOnline}]...");
                        //var inst = factory.Create_InstrumentBase(instConfig.InstrumentType, paramsObjs);

                        var inst = AssemblyManager.CreateInstance<IInstrumentBase>(instConfig.InstrumentType, paramsObjs);
                        //inst.EnableSimulation(instConfig.IsSimulation);
                        inst.TurnOnline(instConfig.IsOnline);
                        this.AuxiliaryInstrumentDict.Add(inst.Name, inst);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                this.ReportException($"CreateAuxiliaryInstuments error", ErrorCodes.CreateInstrumentFailed, ex);
                return false;
            }
        }
        private bool CreateInstumentChassis(TestStationConfig stationConfig)
        {
            try
            {
                this.Log_Global("正在初始化通信底层...");
                this.InstrumentChassisDict.Clear();
                foreach (var chasConfig in stationConfig.InstrumentChassisConfigs)
                {
                    this.Log_Global($"正在初始化通信底层[{chasConfig.Name}][{chasConfig.Resource}]默认在线状态[{chasConfig.IsOnline}]...");
                    object[] paramsObjs = new object[] { chasConfig.Name, chasConfig.Resource, chasConfig.IsOnline };

                    var chas = AssemblyManager.CreateInstance<IInstrumentChassis>(chasConfig.ChassisType, paramsObjs);
                    chas.InitTimeout_ms = chasConfig.InitTimeout_ms;

                    chas.SetupLogger(this._coreInteration, this._coreInteration);
                    this.InstrumentChassisDict.Add(chas.Name, chas);
                }
                return true;
            }
            catch (Exception ex)
            {
                this.ReportException($"CreateInstumentChassis error", ErrorCodes.CreateInstrumentChassisFailed, ex);

                return false;
            }
        }
        private bool CreateInstuments(TestStationConfig stationConfig)
        {
            try
            {
                this.Log_Global("正在初始化仪器...");
                this.InstrumentDict.Clear();
                foreach (var instConfig in stationConfig.InstrumentConfigs)
                {
                    if (this.InstrumentChassisDict.Keys.Contains(instConfig.ChassisName) == false)
                    {
                        string msg = string.Format("Unable to find chassis [{0}] when creating instrument [{1}]", instConfig.ChassisName, instConfig.Name);
                        this.ReportException(msg, ErrorCodes.InstrumentChassisNotFound);
                        return false;
                    }
                    else
                    {
                        object[] paramsObjs = new object[] { instConfig.Name, instConfig.Address, this.InstrumentChassisDict[instConfig.ChassisName] };
                        this.Log_Global($"正在初始化仪器[{instConfig.Name}]ID[{instConfig.Address}]通过底层[{instConfig.ChassisName}]默认在线状态[{instConfig.IsOnline}]...");

                        var inst = AssemblyManager.CreateInstance<IInstrumentBase>(instConfig.InstrumentType, paramsObjs);


                        inst.TurnOnline(instConfig.IsOnline);
                        this.InstrumentDict.Add(inst.Name, inst);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                this.ReportException($"CreateInstuments error", ErrorCodes.CreateInstrumentFailed, ex);
                return false;
            }
        }

        private void CreateInstrumentMonitors(TestStationConfig stationConfig)
        {
            this.InstrumentMonitos.Clear();
            if (stationConfig.InstrumentStatusMonitorConfigs?.Count <= 0)
            {
                return;
            }

            foreach (var monConf in stationConfig.InstrumentStatusMonitorConfigs)
            {
                object instObj = null;
                if (this.InstrumentDict.Keys.Contains(monConf.InstrumentName) == true)
                {
                    instObj = this.InstrumentDict[monConf.InstrumentName];
                }
                else if (this.AuxiliaryInstrumentDict.Keys.Contains(monConf.InstrumentName) == true)
                {
                    instObj = this.AuxiliaryInstrumentDict[monConf.InstrumentName];
                }
                else
                {
                    throw new Exception($"建立监听错误:未能找到仪器[{monConf.InstrumentName}]!");
                }
                var instProps = instObj.GetType().GetProperties();
                PropertyInfo propInfo = null;
                foreach (var prop in instProps)
                {
                    if (prop.Name == monConf.InstrumentPropertyName)
                    {
                        propInfo = prop;
                        break;
                    }
                }
                if (propInfo == null)
                {
                    throw new Exception($"建立监听错误:仪器[{monConf.InstrumentName}]未包含所需监听参数[{monConf.InstrumentPropertyName}]!");
                }
                if (propInfo.CanRead == false)
                {
                    throw new Exception($"建立监听错误:仪器[{monConf.InstrumentName}]所需监听参数[{monConf.InstrumentPropertyName}]不支持监听!");
                }
                this.InstrumentMonitos.Add
                    (
                        new InstrMonitor
                        (
                            monConf.StatusMonitorName,
                            instObj,
                            propInfo,
                            monConf.StatusInfoPostFix,
                            monConf.IsValueType,
                            monConf.MaxValue,
                            monConf.MinValue
                        )
                    );
            }
        }

        private void StartUpMonitor()
        {
            this._myTokenSource = new CancellationTokenSource();
            this.UpdateMonitors();

            this.modelTask = Task.Factory.StartNew
                            (
                                () =>
                                {

                                    do
                                    {
                                        UpdateMonitors();
                                    } while (_myTokenSource.IsCancellationRequested == false);
                                },
                                TaskCreationOptions.LongRunning
                            );
        }
        private void ShutDownMonitor()
        {
            try
            {
                this._myTokenSource?.Cancel();
                Thread.Sleep(2000);
            }
            catch
            {

            }
        }
        private void UpdateMonitors()
        {
            //return;
            foreach (var item in this.InstrumentMonitos)
            {
                try
                {
                    item.Run();
                    if (this.MonitorStatus.ContainsKey(item.MonitorName))
                    {
                        this.MonitorStatus[item.MonitorName] = Tuple.Create(item.Info, item.MonitorStatus);
                    }
                    else
                    {
                        this.MonitorStatus.Add(item.MonitorName, Tuple.Create(item.Info, item.MonitorStatus));
                    }
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {

                    
                }
            }
            Thread.Sleep(1000);
        }
        public override void Close()
        {
            try
            {
                ShutDownMonitor();
                foreach (var chas in this.InstrumentChassisDict.Values)
                {
                    //20240711 不能因为某个不能关闭而后面的都关闭了
                    try
                    {
                        chas.ClearConnection();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                this.Log_Global($"释放资源错误:[{ex.Message}{ex.StackTrace}]!");
            }
        }
    }
}