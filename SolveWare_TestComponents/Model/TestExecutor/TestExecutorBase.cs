using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.ResourceProvider;
using SolveWare_TestComponents.Specification;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_TestComponents.Model
{

    /// <summary>
    /// step执行者基类
    /// </summary>
    /// <typeparam name="TUnit"></typeparam>
    /// <typeparam name="TSlotDatum"></typeparam>
    /// <typeparam name="TStreamData"></typeparam>
    public class TestExecutorBase : ITestExecutorInteration, IDisposable
    {
        protected ITesterCoreInteration _myCore { get; set; }
        protected ISpecification _mySpec;
        protected ITestModule _myTestModule;
        protected ITestRecipe _myTestRecipe;
        protected List<TestCalculatorComboItem> _myTestCalculatorCombo;
        protected Action _alertAction { get; set; }
        protected ITestExecutorUnitInteration _unitInteration { get; set; }
        public virtual string Name
        {
            get; protected set;
        }
        public TestExecutorBase(string name)
        {
            this.Name = name;
            //this.RealtimeStageStepInfo = new RealtimeStageStepInfo();
        }
        public bool IsDebugModeReady { get; protected set; }
        #region 交互接口实现
        public virtual void Setup_DebugMode(ITesterCoreInteration core, ITestPluginResourceProvider resourceProvider, ExecutorConfigItem exeItem)
        {
            this.IsDebugModeReady = false;

            this._myCore = core;
            var testModule = (ITestModule)this._myCore.CreateTestModule(exeItem.TestModuleClassName);
            testModule.Name = this.Name;
            this._myCore.LinkToCore(testModule);

            if (testModule.SetupResources(exeItem.UserDefineInstrumentConfig, exeItem.UserDefineAxisConfig,exeItem.UserDefinePositionConfig, resourceProvider) == false)
            {

                throw new Exception($"测试项目{testModule.Name}加载仪器错误");
            }
            var testRecipe = (ITestRecipe)this._myCore.LoadTestRecipeInstance(exeItem.TestRecipeFileName);
            testModule.Localization(testRecipe);
            List<TestCalculatorComboItem> tempCalculatorCombo = new List<TestCalculatorComboItem>();
            foreach (var calcKvp in exeItem.CalculatorCollection)
            {
                var tempCalculator = (ITestCalculator)this._myCore.CreateCalculator(calcKvp.Key);
                var tempCalcRecipe = (ICalcRecipe)this._myCore.LoadCalcRecipeInstance(calcKvp.Value);
                tempCalculator.Localization(tempCalcRecipe);
                this._myCore.LinkToCore(tempCalculator);
                tempCalculatorCombo.Add(new TestCalculatorComboItem(tempCalculator, tempCalcRecipe));
            }
            this._myTestModule = testModule;
            this._myTestRecipe = testRecipe;

            this._myTestCalculatorCombo = new List<TestCalculatorComboItem>(tempCalculatorCombo);
            this.IsDebugModeReady = true;
        }
        public virtual void Setup_DebugMode_DynamicParamRecipes
         (
             ITesterCoreInteration core,
             ITestPluginResourceProvider resourceProvider,
             ExecutorConfigItem exeItem,
             ExecutorConfigItem_TestParamsConfig exeItemParamValueBook
        )
        {
            this.IsDebugModeReady = false;
            this._myCore = core;
 
            var testModule = (ITestModule)this._myCore.CreateTestModule(exeItem.TestModuleClassName);
            testModule.Name = this.Name;
            this._myCore.LinkToCore(testModule);

            if (testModule.SetupResources(exeItem.UserDefineInstrumentConfig, exeItem.UserDefineAxisConfig, exeItem.UserDefinePositionConfig, resourceProvider) == false)
            {
                throw new Exception($"测试项目{testModule.Name}加载仪器错误");
            }
            var testRecipe = (ITestRecipe)exeItemParamValueBook.TestRecipe;
            testModule.Localization(testRecipe);

            List<TestCalculatorComboItem> tempCalculatorCombo = new List<TestCalculatorComboItem>();
            foreach (var calcKvp in exeItem.CalculatorCollection)
            {
                var calcParamName = calcKvp.Value;
                if (exeItemParamValueBook.CalcRecipeBook.ContainsKey(calcParamName) == false)
                {
                    throw new Exception($"测试项目{testModule.Name}加载算子条件错误:未包含[{calcKvp.Key}]相关计算参数!");
                }
                var tempCalculator = (ITestCalculator)this._myCore.CreateCalculator(calcKvp.Key);
                var tempCalcRecipe = exeItemParamValueBook.CalcRecipeBook[calcParamName];

                tempCalcRecipe.CalcData_Rename = calcParamName;

                tempCalculator.Localization(tempCalcRecipe);

                this._myCore.LinkToCore(tempCalculator);
                tempCalculatorCombo.Add(new TestCalculatorComboItem(tempCalculator, tempCalcRecipe));
            }

            this._myTestModule = testModule;
            this._myTestRecipe = testRecipe;
 
            this._myTestCalculatorCombo = new List<TestCalculatorComboItem>(tempCalculatorCombo);
            this.IsDebugModeReady = true;
        }


        public virtual DeviceStreamDataLite Execute_DebugMode(CancellationToken token)
        {
            DeviceStreamDataLite debugData = new DeviceStreamDataLite();
            IRawDataBaseLite rawData = this._myTestModule.CreateRawData();
            rawData.Name = $"RAWDATA_{ this.Name}";
            rawData.SetRawDataFixFormat(this._myTestRecipe.SummaryData_PreFix, this._myTestRecipe.SummaryData_PostFix);
            List<SummaryDatumItemBase> summaryDataWithoutSpec = new List<SummaryDatumItemBase>();
            //设置测试开始时间
            rawData.TestStepStartTime = DateTime.Now;
            try
            {
                token.ThrowIfCancellationRequested();
                //测试前动作
                this._myTestModule.RunRreAction(token);
                //测试执行
                this._myTestModule.Run(token);
                //测试后动作
                this._myTestModule.RunPostAction(token);
                //执行计算
                foreach (var comboItem in this._myTestCalculatorCombo)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    comboItem.Calculator.Run(rawData, ref summaryDataWithoutSpec, token);
                }
                debugData.RawData = rawData;
                debugData.AddSummaryDataCollection(summaryDataWithoutSpec);
            }
            #region 异常处理
            catch (OperationCanceledException oce)
            {
                try
                {
                    MessageBox.Show($"测试项目调试运行被取消!");
                }
                catch
                {

                }
                throw oce;
            }

            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show($"测试项目调试运行错误:[{ex.Message}-{ex.StackTrace}]!");
                }
                catch
                {
                }
                throw ex;
            }
            #endregion
            finally
            {
                rawData.TestStepEndTime = DateTime.Now;//设置测试结束时间
                rawData.TestCostTimeSpan = rawData.TestStepEndTime - rawData.TestStepStartTime;
            }
            return debugData;
        }
        public virtual DeviceStreamDataLite CalculateReloadData_DebugMode(string reloadDataSource, CancellationToken token)
        {
            DeviceStreamDataLite debugData = new DeviceStreamDataLite();
            IRawDataBaseLite rawData = this._myTestModule.CreateRawData();
            rawData.Name = $"RAWDATA_{ this.Name}";
            rawData.SetRawDataFixFormat(this._myTestRecipe.SummaryData_PreFix, this._myTestRecipe.SummaryData_PostFix);
            List<SummaryDatumItemBase> summaryDataWithoutSpec = new List<SummaryDatumItemBase>();
            //设置测试开始时间
            rawData.TestStepStartTime = DateTime.Now;
            try
            {
                token.ThrowIfCancellationRequested();
                if (rawData.ReloadFormString(reloadDataSource) == true)
                {
                    //执行计算
                    foreach (var comboItem in this._myTestCalculatorCombo)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }
                        comboItem.Calculator.Run(rawData, ref summaryDataWithoutSpec, token);
                    }
                    debugData.RawData = rawData;
                    debugData.AddSummaryDataCollection(summaryDataWithoutSpec);
                }
            }
            #region 异常处理
            catch (OperationCanceledException oce)
            {
                try
                {
                    MessageBox.Show($"测试项目调试运行被取消!");
                }
                catch
                {

                }
                throw oce;
            }

            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show($"测试项目调试运行错误:[{ex.Message}-{ex.StackTrace}]!");
                }
                catch
                {
                }
                throw ex;
            }
            #endregion
            finally
            {
                rawData.TestStepEndTime = DateTime.Now;//设置测试结束时间
                rawData.TestCostTimeSpan = rawData.TestStepEndTime - rawData.TestStepStartTime;
            }
            return debugData;
        }
        public virtual DeviceStreamDataLite CalculateReloadData_DebugMode(string[] reloadDataSource, CancellationToken token)
        {
            DeviceStreamDataLite debugData = new DeviceStreamDataLite();
            IRawDataBaseLite rawData = this._myTestModule.CreateRawData();
            rawData.Name = $"RAWDATA_{ this.Name}";
            rawData.SetRawDataFixFormat(this._myTestRecipe.SummaryData_PreFix, this._myTestRecipe.SummaryData_PostFix);
            List<SummaryDatumItemBase> summaryDataWithoutSpec = new List<SummaryDatumItemBase>();
            //设置测试开始时间
            rawData.TestStepStartTime = DateTime.Now;
            try
            {
                token.ThrowIfCancellationRequested();
                if (rawData.ReloadFormString(reloadDataSource) == true)
                {
                    //执行计算
                    foreach (var comboItem in this._myTestCalculatorCombo)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }
                        comboItem.Calculator.Run(rawData, ref summaryDataWithoutSpec, token);
                    }
                    debugData.RawData = rawData;
                    debugData.AddSummaryDataCollection(summaryDataWithoutSpec);
                }
            }
            #region 异常处理
            catch (OperationCanceledException oce)
            {
                try
                {
                    MessageBox.Show($"测试项目调试运行被取消!");
                }
                catch
                {

                }
                throw oce;
            }

            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show($"测试项目调试运行错误:[{ex.Message}-{ex.StackTrace}]!");
                }
                catch
                {
                }
                throw ex;
            }
            #endregion
            finally
            {
                rawData.TestStepEndTime = DateTime.Now;//设置测试结束时间
                rawData.TestCostTimeSpan = rawData.TestStepEndTime - rawData.TestStepStartTime;
            }
            return debugData;
        }
        #endregion

        public virtual void Setup
        (
            ITesterCoreInteration core,
            ISpecification spec,
            ITestPluginResourceProvider resourceProvider,
            ExecutorConfigItem exeItem,
            //Dictionary<string, IInstrumentBase> _unitInstruments,
            ITestExecutorUnitInteration interationData
        )
        {
            this._myCore = core;
            this._unitInteration = interationData;

            var testModule = (ITestModule)this._myCore.CreateTestModule(exeItem.TestModuleClassName);
            testModule.Name = this.Name;
            this._myCore.LinkToCore(testModule);

            if (testModule.SetupResources(exeItem.UserDefineInstrumentConfig, exeItem.UserDefineAxisConfig, exeItem.UserDefinePositionConfig, resourceProvider) == false)
            {
                throw new Exception($"测试项目{testModule.Name}加载仪器错误");
            }
            var testRecipe = (ITestRecipe)this._myCore.LoadTestRecipeInstance(exeItem.TestRecipeFileName);
            testModule.Localization(testRecipe);

            List<TestCalculatorComboItem> tempCalculatorCombo = new List<TestCalculatorComboItem>();
            foreach (var calcKvp in exeItem.CalculatorCollection)
            {
                var tempCalculator = (ITestCalculator)this._myCore.CreateCalculator(calcKvp.Key);
                var tempCalcRecipe = (ICalcRecipe)this._myCore.LoadCalcRecipeInstance(calcKvp.Value);
                tempCalculator.Localization(tempCalcRecipe);
                this._myCore.LinkToCore(tempCalculator);
                tempCalculatorCombo.Add(new TestCalculatorComboItem(tempCalculator, tempCalcRecipe));
            }

            this._myTestModule = testModule;
            this._myTestRecipe = testRecipe;
            //这是executor的名字如 [测试站1号]_[主要]_[项目1][TestModule_LIV] @ [LIV测试Rcp_45C]

            this._myTestModule.LinkToExcecutorUnit(this._unitInteration);

            this._mySpec = spec;

            this._myTestCalculatorCombo = new List<TestCalculatorComboItem>(tempCalculatorCombo);
            this._myTestCalculatorCombo.ForEach(item => item.Calculator.LinkToExcecutorUnit(this._unitInteration));
        }

        public virtual void Setup_DynamicParamRecipes
        (
            ITesterCoreInteration core,
            ISpecification spec,
            ITestPluginResourceProvider resourceProvider,
            ExecutorConfigItem exeItem,
            ExecutorConfigItem_TestParamsConfig exeItemParamValueBook,
            ITestExecutorUnitInteration interationData
        )
        {
            this._myCore = core;
            this._unitInteration = interationData;

            var testModule = (ITestModule)this._myCore.CreateTestModule(exeItem.TestModuleClassName);
            testModule.Name = this.Name;
            this._myCore.LinkToCore(testModule);

            if (testModule.SetupResources(exeItem.UserDefineInstrumentConfig, exeItem.UserDefineAxisConfig, exeItem.UserDefinePositionConfig, resourceProvider) == false)
            {
                throw new Exception($"测试项目{testModule.Name}加载仪器错误");
            }

            //var testRecipe = (ITestRecipe)this._myCore.CreateTestRecipe(testModule.GetTestRecipeType());
            var testRecipe = (ITestRecipe)exeItemParamValueBook.TestRecipe ;
            testModule.Localization(testRecipe);
 
            List<TestCalculatorComboItem> tempCalculatorCombo = new List<TestCalculatorComboItem>();
            foreach (var calcKvp in exeItem.CalculatorCollection)
            {
                var calcParamName = calcKvp.Value;
                if (exeItemParamValueBook.CalcRecipeBook.ContainsKey(calcParamName) == false)
                {
                    throw new Exception($"测试项目{testModule.Name}加载算子条件错误:未包含[{calcKvp.Key}]相关计算参数!");
                }
                var tempCalculator = (ITestCalculator)this._myCore.CreateCalculator(calcKvp.Key);
                var tempCalcRecipe = exeItemParamValueBook.CalcRecipeBook[calcParamName];

                tempCalcRecipe.CalcData_Rename = calcParamName;

                tempCalculator.Localization(tempCalcRecipe);

                this._myCore.LinkToCore(tempCalculator);
                tempCalculatorCombo.Add(new TestCalculatorComboItem(tempCalculator, tempCalcRecipe));
            }

            this._myTestModule = testModule;
            this._myTestRecipe = testRecipe;
            //这是executor的名字如 [测试站1号]_[主要]_[项目1][TestModule_LIV] @ [LIV测试Rcp_45C]

            this._myTestModule.LinkToExcecutorUnit(this._unitInteration);

            this._mySpec = spec;

            this._myTestCalculatorCombo = new List<TestCalculatorComboItem>(tempCalculatorCombo);
            this._myTestCalculatorCombo.ForEach(item => item.Calculator.LinkToExcecutorUnit(this._unitInteration));
        }
        public virtual void Execute(IDeviceStreamDataBase streamData, Action runtimeDataUpdateToUIAction, Action saveDataAction, Action alertAction, CancellationToken token)
        {
            IRawDataBaseLite rawData = this._myTestModule.CreateRawData();
            rawData.Name = $"RAWDATA_{ this.Name}";
            rawData.SetRawDataFixFormat(this._myTestRecipe.SummaryData_PreFix, this._myTestRecipe.SummaryData_PostFix);
            List<SummaryDatumItemBase> summaryDataWithoutSpec = new List<SummaryDatumItemBase>();
            //设置测试开始时间
            rawData.TestStepStartTime = DateTime.Now;
            try
            {
                token.ThrowIfCancellationRequested();
                this._myTestModule.GetReferenceFromDeviceStreamData(streamData);
                //输出曲线到窗口
                //测试前动作
                this._myTestModule.RunRreAction(token);
                //测试执行
                this._myTestModule.Run(token);
                //输出曲线到窗口
                //测试后动作
                this._myTestModule.RunPostAction(token);
                //执行计算
                foreach (var comboItem in this._myTestCalculatorCombo)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    comboItem.Calculator.Run(rawData, ref summaryDataWithoutSpec, token);
                }
                this.Judgement(this._mySpec, ref summaryDataWithoutSpec, token);

                try
                {
                    rawData.TestStepEndTime = DateTime.Now;//设置测试结束时间
                    rawData.TestCostTimeSpan = rawData.TestStepEndTime - rawData.TestStepStartTime;
                    streamData.AddRawData(rawData);
                    streamData.AddSummaryDataCollection(summaryDataWithoutSpec);
                }
                catch { }
                this.InvokeRuntimeDataUpdateToUIAction(runtimeDataUpdateToUIAction);
                //执行规格判断
                this.InvokeSaveDataAction(saveDataAction);
                //用户奇怪想法Action
                this.InvokeAlertUserAction(_alertAction);
            }
            #region 异常处理
            catch (OperationCanceledException oce)
            {
                try
                {
                    //this.StreamData.ModifyStepResultItemObject(procStepDataObject);
                    //this.StreamData.SaveXML();
                }
                catch
                {

                }
                throw oce;
            }
            catch (InvalidProgramException ipe)
            {
                try
                {
                    //this.StreamData.ModifyStepResultItemObject(procStepDataObject);
                    //this.StreamData.SaveXML();
                }
                catch
                {

                }
                throw ipe;

            }
            catch (Exception ex)
            {
                try
                {
                    //this.ReportException(ex.Message, ExpectedException.BURNIN_RUNTIME_ALARM, ex, null);
                    //this.StreamData.ModifyStepResultItemObject(procStepDataObject);
                    //this.StreamData.SaveXML();
                }
                catch
                {
                }
                throw ex;
            }
            #endregion
            finally
            {

                this.UploadDataToDataBase(streamData);
            }
        }
        public virtual void ExecuteWithCalibrationAction(IDeviceStreamDataBase streamData,
            Action<IRawDataBaseLite> RawDataCalibrateAction,
            Action<List<SummaryDatumItemBase>> SummaryDataCalibrateAction,
            Action runtimeDataUpdateToUIAction, Action saveDataAction, Action alertAction, CancellationToken token)
        {
            IRawDataBaseLite rawData = this._myTestModule.CreateRawData();
            rawData.Name = $"RAWDATA_{ this.Name}";
            rawData.SetRawDataFixFormat(this._myTestRecipe.SummaryData_PreFix, this._myTestRecipe.SummaryData_PostFix);
            List<SummaryDatumItemBase> summaryDataWithoutSpec = new List<SummaryDatumItemBase>();
            //设置测试开始时间
            rawData.TestStepStartTime = DateTime.Now;
            try
            {
                token.ThrowIfCancellationRequested();

                this._myTestModule.GetReferenceFromDeviceStreamData(streamData);
                //输出曲线到窗口
                //测试前动作
                this._myTestModule.RunRreAction(token);
                //测试执行
                this._myTestModule.Run(token);
                //输出曲线到窗口
                //测试后动作
                this._myTestModule.RunPostAction(token);

                RawDataCalibrateAction?.Invoke(rawData);

                //执行计算
                foreach (var comboItem in this._myTestCalculatorCombo)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    comboItem.Calculator.Run(rawData, ref summaryDataWithoutSpec, token);
                }

                SummaryDataCalibrateAction?.Invoke(summaryDataWithoutSpec);

                this.Judgement(this._mySpec, ref summaryDataWithoutSpec, token);

                try
                {
                    rawData.TestStepEndTime = DateTime.Now;//设置测试结束时间
                    rawData.TestCostTimeSpan = rawData.TestStepEndTime - rawData.TestStepStartTime;
                    streamData.AddRawData(rawData);
                    streamData.AddSummaryDataCollection(summaryDataWithoutSpec);
                }
                catch { }
                this.InvokeRuntimeDataUpdateToUIAction(runtimeDataUpdateToUIAction);
                //执行规格判断
                this.InvokeSaveDataAction(saveDataAction);
                //用户奇怪想法Action
                this.InvokeAlertUserAction(_alertAction);
            }
            #region 异常处理
            catch (OperationCanceledException oce)
            {
                try
                {
                    //this.StreamData.ModifyStepResultItemObject(procStepDataObject);
                    //this.StreamData.SaveXML();
                }
                catch
                {

                }
                throw oce;
            }
            catch (InvalidProgramException ipe)
            {
                try
                {
                    //this.StreamData.ModifyStepResultItemObject(procStepDataObject);
                    //this.StreamData.SaveXML();
                }
                catch
                {

                }
                throw ipe;

            }
            catch (Exception ex)
            {
                try
                {
                    //this.ReportException(ex.Message, ExpectedException.BURNIN_RUNTIME_ALARM, ex, null);
                    //this.StreamData.ModifyStepResultItemObject(procStepDataObject);
                    //this.StreamData.SaveXML();
                }
                catch
                {
                }
                throw ex;
            }
            #endregion
            finally
            {

                this.UploadDataToDataBase(streamData);
            }
        }
        protected virtual bool UploadDataToDataBase(IDeviceStreamDataBase deviceStreamData)
        {
            try
            {
                if (this._myCore.IsDataBaseEnable == false)
                {
                    return false;
                }

                //请将数据写入本地数据库
                var sqlHeader = deviceStreamData.GetSQL_Header();
                var sqlHeaderValues = deviceStreamData.GetSQL_HeaderValues();
                var sn = deviceStreamData.SerialNumber;
                var startTime = deviceStreamData.CreateTime;
                //数据列表
                var summarydata = deviceStreamData.SummaryDataCollection.ToDictionary();
                // database 写入操作
                string DataPath = this._myCore.DataBaseConnectionString;//@"D:\test.mdb";
                string TableName = this._myCore.DataBaseTableName;//"NewTable";
                                                                  //this._myCore.NewDataBase(DataPath);

                string[] namekt = sqlHeader.Split(",".ToCharArray());
                //List<string> list = new List<string>(namekt);
                //this._myCore.CreateAccessDataTable(TableName, list, out string errMsg);
                this._myCore.NewCreateTable(DataPath, TableName, namekt);
                this._myCore.NewUpDataTable(DataPath, TableName, sqlHeader, sqlHeaderValues, summarydata, sn, startTime);
                return true;
            }
            catch (Exception ex)
            {
                //写入数据错误
                this._myCore.ReportException("写入产品数据错误", ErrorCodes.UploadDataToDatabaseFailed, ex);
            }
            return false;
        }
        protected virtual void Judgement(ISpecification spec, ref List<SummaryDatumItemBase> summaryDataWithoutSpec, CancellationToken token)
        {
            foreach (var sData in summaryDataWithoutSpec)
            {
                try
                {
                    TestSpecificationItem specItem = null;
                    if(spec == null)
                    {

                    }
                    else
                    {
                        specItem = spec.GetSingleItem(sData.Name);
                    }
 
                    if (specItem == null)
                    {
                        sData.Min = SummaryDatumItemBase.NA;
                        sData.Max = SummaryDatumItemBase.NA;
                        sData.Judegment = SummaryDatumJudegment.NO_SEPC;
                        sData.Unit = SummaryDatumItemBase.NA;
                        sData.FailureCode = SummaryDatumItemBase.NA;
                    }
                    else
                    {
                        sData.Min = specItem.Min;
                        sData.Max = specItem.Max;
                        sData.Unit = specItem.Unit;

                        switch (specItem.DataType)
                        {
                            case SpecDataType.String:
                                if (sData.GetType() == typeof(string))
                                {
                                    if (sData.Value.ToString().Equals(specItem.Min) ||
                                        sData.Value.ToString().Equals(specItem.Max))
                                    {
                                        sData.Judegment = SummaryDatumJudegment.PASS;
                                    }
                                    else
                                    {
                                        sData.FailureCode = specItem.FailureCode;
                                        sData.Judegment = SummaryDatumJudegment.FAIL;
                                    }
                                }
                                else
                                {
                                    sData.FailureCode = specItem.FailureCode;
                                    sData.Judegment = SummaryDatumJudegment.TYPE_NOT_MATCH;
                                }
                                break;
                            case SpecDataType.Double:
                                {
                                    if (sData.GetType() == typeof(double))
                                    {
                                        if (JuniorMath.IsValueInLimitRange
                                         (
                                             Convert.ToDouble(sData.Value),
                                             Convert.ToDouble(specItem.Min),
                                             Convert.ToDouble(specItem.Max)
                                         ))
                                        {
                                            sData.Judegment = SummaryDatumJudegment.PASS;
                                        }
                                        else
                                        {
                                            sData.FailureCode = specItem.FailureCode;
                                            sData.Judegment = SummaryDatumJudegment.FAIL;
                                        }
                                    }
                                    else
                                    {
                                        sData.FailureCode = specItem.FailureCode;
                                        sData.Judegment = SummaryDatumJudegment.TYPE_NOT_MATCH;
                                    }
                                }
                                break;
                            case SpecDataType.Integer:
                                {
                                    if (sData.GetType() == typeof(int))
                                    {
                                        if (JuniorMath.IsValueInLimitRange
                                         (
                                             Convert.ToInt32(sData.Value),
                                             Convert.ToInt32(specItem.Min),
                                             Convert.ToInt32(specItem.Max)
                                         ))
                                        {
                                            sData.Judegment = SummaryDatumJudegment.PASS;
                                        }
                                        else
                                        {
                                            sData.FailureCode = specItem.FailureCode;
                                            sData.Judegment = SummaryDatumJudegment.FAIL;
                                        }
                                    }
                                    else
                                    {
                                        sData.FailureCode = specItem.FailureCode;
                                        sData.Judegment = SummaryDatumJudegment.TYPE_NOT_MATCH;
                                    }
                                }
                                break;
                                //case SpecDataType.Object:
                                //    {

                                //    }
                                //    break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this._myCore.ReportException($"产品输出参数[{sData.Name}]规格判断异常", ErrorCodes.JudgeDataError, ex);
                }
            }
        }

        protected virtual void InvokeSaveDataAction(Action saveDataAction)
        {
            try
            {
                saveDataAction?.Invoke();
            }
            catch
            {

            }

        }
        protected virtual void InvokeAlertUserAction(Action alertAction)
        {
            try
            {
                alertAction?.Invoke();
            }
            catch
            {

            }

        }
        protected virtual void InvokeRuntimeDataUpdateToUIAction(Action runtimeDataUpdateToUIAction)
        {
            try
            {
                runtimeDataUpdateToUIAction?.Invoke();
            }
            catch
            {

            }

        }
        public void UpdateCurrentExecutorRuntimeInfo(string deviceSN)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _myCore = null;
            _mySpec = null;
            _myTestModule = null;
            _myTestRecipe = null;
            _myTestCalculatorCombo = null;
        }
    }
}