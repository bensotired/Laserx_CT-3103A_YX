using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInInstruments;
using SolveWare_BurnInMessage;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.ResourceProvider;
using SolveWare_TestComponents.Specification;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TestComponents.Model
{
    /// <summary>
    /// 测试执行者   每个就是一个复合测试头
    /// </summary>
    public class TestExecutorUnit : ITesterCoreLink
    {
        protected TestExecutorUnitConfig _myConfig { get; set; }

        protected List<TestExecutorBase> _preExecutors = new List<TestExecutorBase>();
        protected List<TestExecutorBase> _mainExecutors = new List<TestExecutorBase>();
        protected List<TestExecutorBase> _postExecutors = new List<TestExecutorBase>();
        protected ITestExecutorUnitInteration _interation;
        protected ITesterCoreInteration _core;

        protected Form_TLP_layer _UI = null;

        public virtual string Name
        {
            get { return this._interation.Name; }
        }
        //public ITestExecutorUnitInteration Interation
        //{
        //    get
        //    {
        //        return _interation;
        //    }
        //}
        public TestExecutorUnit(string name, TestExecutorUnitConfig unitConf/*, ITestPluginResourceProvider resourceProvider*/)
        {
            _myConfig = unitConf;
            _interation = new TestExecutorUnitInteration();
            _interation.Name = name;
        }
        public virtual void SetupUnitInteration(params object[] args)
        {

        }
        /// <summary>
        /// 建立测试模块 - 软/硬资源 加载到模块
        /// </summary>
        /// <param name="combo"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        public virtual bool SetupTestExecutors(ITestPluginResourceProvider _resourceProvider, TestExecutorCombo combo, ISpecification spec)
        {
            try
            {
                this.Clear();
                int preExeIndex = 1;
                int mainExeIndex = 1;
                int postExeIndex = 1;
                foreach (var exeItem in combo.Pre_ExecutorConfigCollection)
                {
                    var executor = new TestExecutorBase($"[{this.Name}]_[前置]_[项目{preExeIndex++}][{exeItem.TestModuleClassName}] @ [{exeItem.TestRecipeFileName}]");
                    executor.Setup(this._core, spec, _resourceProvider, exeItem, _interation);
                    this._preExecutors.Add(executor);
                }
                foreach (var exeItem in combo.Main_ExecutorConfigCollection)
                {
                    var executor = new TestExecutorBase($"[{this.Name}]_[主要]_[项目{mainExeIndex++}][{exeItem.TestModuleClassName}] @ [{exeItem.TestRecipeFileName}]");
                    executor.Setup(this._core, spec, _resourceProvider, exeItem, _interation);
                    this._mainExecutors.Add(executor);
                }
                foreach (var exeItem in combo.Post_ExecutorConfigCollection)
                {
                    var executor = new TestExecutorBase($"[{this.Name}]_[后置]_[项目{postExeIndex++}][{exeItem.TestModuleClassName}] @ [{exeItem.TestRecipeFileName}]");
                    executor.Setup(this._core, spec, _resourceProvider, exeItem, _interation);
                    this._postExecutors.Add(executor);
                }
                return true;
            }
            catch (Exception ex)
            {
                this._core.Log_Global($"[{this.Name}]执行器加载测试项目错误[{ex.Message}-{ex.StackTrace}]!");
            }
            return false;
        }
        public virtual bool SetupTestExecutors_WithDynamicTestParams
            (
                ITestPluginResourceProvider _resourceProvider,
                TestExecutorCombo combo,
                ExecutorConfigItem_TestParamsConfigCombo paramCombo,
                ISpecification spec
            )
        {

            try
            {
                this.Clear();
                int preExeIndex = 1;
                int mainExeIndex = 1;
                int postExeIndex = 1;
                foreach (var exeItem in combo.Pre_ExecutorConfigCollection)
                {
                    string runtimeExeName = $"[{this.Name}]_[前置]_[项目{preExeIndex++}][{exeItem.TestModuleClassName}]";
                    var executor = new TestExecutorBase(runtimeExeName);

                    if (paramCombo.Pre_TestParamsCollection.Exists(item => item.Name == exeItem.TestExecutorName) == false)
                    {
                        throw new Exception($"不存在[{runtimeExeName}]对应参数配置!");
                    }

                    var testParams = paramCombo.Pre_TestParamsCollection.Find(item => item.Name == exeItem.TestExecutorName);
                    executor.Setup_DynamicParamRecipes(this._core, spec, _resourceProvider, exeItem, testParams, _interation);

                    this._preExecutors.Add(executor);
                }
                foreach (var exeItem in combo.Main_ExecutorConfigCollection)
                {
                    string runtimeExeName = $"[{this.Name}]_[主要]_[项目{mainExeIndex++}][{exeItem.TestModuleClassName}]";
                    var executor = new TestExecutorBase(runtimeExeName);

                    if (paramCombo.Main_TestParamsCollection.Exists(item => item.Name == exeItem.TestExecutorName) == false)
                    {
                        throw new Exception($"不存在[{runtimeExeName}]对应参数配置!");
                    }

                    var testParams = paramCombo.Main_TestParamsCollection.Find(item => item.Name == exeItem.TestExecutorName);
                    executor.Setup_DynamicParamRecipes(this._core, spec, _resourceProvider, exeItem, testParams, _interation);

                    this._mainExecutors.Add(executor);
                }
                foreach (var exeItem in combo.Post_ExecutorConfigCollection)
                {
                    string runtimeExeName = $"[{this.Name}]_[后置]_[项目{mainExeIndex++}][{exeItem.TestModuleClassName}]";

                    var executor = new TestExecutorBase($"[{this.Name}]_[后置]_[项目{postExeIndex++}][{exeItem.TestModuleClassName}] @ [{exeItem.TestRecipeFileName}]");

                    var testParams = paramCombo.Post_TestParamsCollection.Find(item => item.Name == exeItem.TestExecutorName);
                    executor.Setup_DynamicParamRecipes(this._core, spec, _resourceProvider, exeItem, testParams, _interation);

                    this._postExecutors.Add(executor);
                }
                return true;
            }
            catch (Exception ex)
            {
                this._core.Log_Global($"[{this.Name}]执行器加载测试项目错误[{ex.Message}-{ex.StackTrace}]!");
            }
            return false;
        }
        public virtual void ConnectToCore(ITesterCoreInteration core)
        {
            this._core = core;
            this._core.SendOutFormCoreEvent -= ReceiveMessage;
            this._core.SendOutFormCoreEvent += ReceiveMessage;
        }
        public virtual void DisconnectFromCore(ITesterCoreInteration core)
        {
            this._core.SendOutFormCoreEvent -= this.ReceiveMessage;
        }
        protected virtual void ReceiveMessage(IMessage message)
        {
        }
        public virtual void Setup()
        {

        }
        public virtual void Clear()
        {
            _preExecutors.Clear();
            _mainExecutors.Clear();
            _postExecutors.Clear();
        }

        /// <summary>
        /// 执行该executor unit里面已经加载的[前置]测试模块
        /// </summary>
        /// <param name="deviceData"></param>
        public virtual void RunPreExecutorCombo
            (
                IDeviceStreamDataBase deviceStreamData,
                Action runtimeDataUpdateToUIAction,
                Action saveDataAction,
                Action alertUserAction,
                CancellationTokenSource tokenSource
            )
        {
            foreach (var exe in _preExecutors)
            {
                exe.Execute(deviceStreamData, runtimeDataUpdateToUIAction, saveDataAction, alertUserAction, tokenSource.Token);
            }
        }
        /// <summary>
        /// 执行该executor unit里面已经加载的[主要]测试模块
        /// </summary>
        /// <param name="deviceData"></param>
        public virtual void RunMainExecutorCombo
            (
                IDeviceStreamDataBase deviceStreamData,
                Action runtimeDataUpdateToUIAction,
                Action saveDataAction,
                Action alertUserAction,
                CancellationTokenSource tokenSource
            )
        {
            try
            {
                foreach (var exe in _mainExecutors)
                {
                    exe.Execute(deviceStreamData, runtimeDataUpdateToUIAction, saveDataAction, alertUserAction, tokenSource.Token);
                }
            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains("扫描范围内无光..."))
                //{

                //}
                //else
                //{
                //    tokenSource.Cancel();
                //    throw ex;
                //}
            }
        }
        /// <summary>
        /// 执行该executor unit里面已经加载的测试模块
        /// </summary>
        /// <param name="deviceData"></param>
        public virtual void RunPostExecutorCombo
            (
                IDeviceStreamDataBase deviceStreamData,
                Action runtimeDataUpdateToUIAction,
                Action saveDataAction,
                Action alertUserAction,
                CancellationTokenSource tokenSource
            )
        {
            foreach (var exe in _postExecutors)
            {
                exe.Execute(deviceStreamData, runtimeDataUpdateToUIAction, saveDataAction, alertUserAction, tokenSource.Token);
            }
        }


        /// <summary>
        /// 执行该executor unit里面已经加载的[前置]测试模块
        /// </summary>
        /// <param name="deviceData"></param>
        public virtual void RunPreExecutorComboWithCalibrationAction
            (
                IDeviceStreamDataBase deviceStreamData,
                Action<IRawDataBaseLite> rawDataCalibrateAction,
                Action<List<SummaryDatumItemBase>> summaryDataCalibrateAction,
                Action runtimeDataUpdateToUIAction,
                Action saveDataAction,
                Action alertUserAction,
                CancellationTokenSource tokenSource
            )
        {
            foreach (var exe in _preExecutors)
            {
                exe.ExecuteWithCalibrationAction(
               deviceStreamData,
               rawDataCalibrateAction,
               summaryDataCalibrateAction,
               runtimeDataUpdateToUIAction,
               saveDataAction,
               alertUserAction,
               tokenSource.Token);
                // exe.Execute(deviceStreamData, runtimeDataUpdateToUIAction, saveDataAction, alertUserAction, tokenSource.Token);
            }
        }
        /// <summary>
        /// 执行该executor unit里面已经加载的[主要]测试模块
        /// </summary>
        /// <param name="deviceData"></param>
        public virtual void RunMainExecutorComboWithCalibrationAction
            (
                IDeviceStreamDataBase deviceStreamData,
                Action<IRawDataBaseLite> rawDataCalibrateAction,
                Action<List<SummaryDatumItemBase>> summaryDataCalibrateAction,
                Action runtimeDataUpdateToUIAction,
                Action saveDataAction,
                Action alertUserAction,
                CancellationTokenSource tokenSource
            )
        {
            foreach (var exe in _mainExecutors)
            {
                exe.ExecuteWithCalibrationAction(
                    deviceStreamData,
                    rawDataCalibrateAction,
                    summaryDataCalibrateAction,
                    runtimeDataUpdateToUIAction,
                    saveDataAction,
                    alertUserAction,
                    tokenSource.Token);
            }
        }
        /// <summary>
        /// 执行该executor unit里面已经加载的测试模块
        /// </summary>
        /// <param name="deviceData"></param>
        public virtual void RunPostExecutorComboWithCalibrationAction
            (
                IDeviceStreamDataBase deviceStreamData,
                Action<IRawDataBaseLite> rawDataCalibrateAction,
                Action<List<SummaryDatumItemBase>> summaryDataCalibrateAction,
                Action runtimeDataUpdateToUIAction,
                Action saveDataAction,
                Action alertUserAction,
                CancellationTokenSource tokenSource
            )
        {
            foreach (var exe in _postExecutors)
            {
                exe.ExecuteWithCalibrationAction(
                 deviceStreamData,
                 rawDataCalibrateAction,
                 summaryDataCalibrateAction,
                 runtimeDataUpdateToUIAction,
                 saveDataAction,
                 alertUserAction,
                 tokenSource.Token);
            }
        }
    }
}