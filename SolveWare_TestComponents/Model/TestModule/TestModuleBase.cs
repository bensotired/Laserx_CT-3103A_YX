using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_TestComponents.Data;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System;
using SolveWare_BurnInInstruments;
using SolveWare_TestComponents.Attributes;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.ResourceProvider;

namespace SolveWare_TestComponents.Model
{
    public abstract class TestModuleBase : ITestExecutiveMember,ITestModule
    {
        protected Form _UI;
        protected ITesterCoreInteration _core;
        protected ITestExecutorUnitInteration _myUnitInteration;
        protected TestModuleUIType UIType { get; set; } = TestModuleUIType.Stardard;
        public string Name { get;   set; }
        protected Dictionary<string, object> ModuleResource = new Dictionary<string, object>();
       
        public TestModuleBase()
        {
        }
        public virtual void LinkToExcecutorUnit(ITestExecutorUnitInteration exeUnitInteration)
        {
            _myUnitInteration = exeUnitInteration;
        }
        public virtual void ConnectToCore(ITesterCoreInteration core)
        {
            _core = core;
        }
        public virtual void DisconnectFromCore(ITesterCoreInteration core)
        {
            _core = null;
        }
 
        protected virtual TReturn ConvertObjectTo<TReturn>(object sourceObject)
        {
            return (TReturn)Converter.ConvertObjectTo(sourceObject, typeof(TReturn));
        }
        public abstract void Localization(ITestRecipe testRecipe);
        public abstract IRawDataBaseLite CreateRawData();
      
        public virtual bool SetupResources
            (
                DataBook<string, string> userDefineInstrumentConfig,
                DataBook<string, string> userDefineAxisConfig, 
                DataBook<string, string> userDefinePositionConfig,
                ITestPluginResourceProvider resourceProvider
            )
        {
            var instrAtts = PropHelper.GetAttributeValueCollection<ConfigurableInstrumentAttribute>(this.GetType());
            foreach (var item in instrAtts)
            {
                var instrName = item.ModuleDefineInstrName;
                if (userDefineInstrumentConfig.ContainsKey(instrName) == false)
                {
                    this.Log_Global($"用户定义仪器列表内未包含测试项目{this.Name}所需仪器[{instrName}]");
                    return false;
                }
                var userDefineInstrKey = userDefineInstrumentConfig[instrName];

                if (resourceProvider.Resource_Instruments.ContainsKey(userDefineInstrKey) == false)
                {
                    this.Log_Global($"仪器资源列表内未包含测试项目{this.Name}所需仪器[{instrName}]指定的仪器实例[{userDefineInstrKey}]");
                    return false;
                }
                ModuleResource.Add(instrName, resourceProvider.Resource_Instruments[userDefineInstrKey]);
            }
            var axisAtts = PropHelper.GetAttributeValueCollection<ConfigurableAxisAttribute>(this.GetType());
            foreach (var item in axisAtts)
            {
                var axisName = item.ModuleDefineAxisName;
                if (userDefineAxisConfig.ContainsKey(axisName) == false)
                {
                    this.Log_Global($"用户定义<轴>列表内未包含测试项目{this.Name}所需轴[{axisName}]");
                    return false;
                }
                var userDefineAxisKey = userDefineAxisConfig[axisName];

                if (resourceProvider.Resource_Axes.ContainsKey(userDefineAxisKey) == false)
                {
                    this.Log_Global($"<轴>资源列表内未包含测试项目{this.Name}所需轴[{axisName}]指定的轴实例[{userDefineAxisKey}]");
                    return false;
                }
                ModuleResource.Add(axisName, resourceProvider.Resource_Axes[userDefineAxisKey]);
            }
            var posAtts = PropHelper.GetAttributeValueCollection<ConfigurablePositionAttribute>(this.GetType());
            foreach (var item in posAtts)
            {
                var posName = item.ModuleDefinePositionName;
                if (userDefinePositionConfig.ContainsKey(posName) == false)
                {
                    this.Log_Global($"用户定义<位置>列表内未包含测试项目{this.Name}所需位置[{posName}]");
                    return false;
                }
                var userDefinePosKey = userDefinePositionConfig[posName];

                if (resourceProvider.Resource_Posititon.ContainsKey(userDefinePosKey) == false)
                {
                    this.Log_Global($"<位置>资源列表内未包含测试项目{this.Name}所需位置[{posName}]指定的位置实例[{userDefinePosKey}]");
                    return false;
                }
                ModuleResource.Add(posName, resourceProvider.Resource_Posititon[userDefinePosKey]);
            }

            var staticResrouces = PropHelper.GetAttributeValueCollection<StaticResourceAttribute>(this.GetType());

            foreach (var staItem in staticResrouces)
            {
                object staticResObj = null;
                switch (staItem.ResourceType)
                {
                    case ResourceItemType.AXIS:
                        {
                            staticResObj = resourceProvider.Local_Axis_ResourceProvider.GetAxis_Object(staItem.ResourceName);
                        }
                        break;
                    case ResourceItemType.IO:
                        {
                            staticResObj = resourceProvider.Local_IO_ResourceProvider.GetIO_Object(staItem.ResourceName);
                        }
                        break;
                    case ResourceItemType.VICAL:
                    case ResourceItemType.POS:
                        {
                            staticResObj = resourceProvider.Local_AxesPosition_ResourceProvider.GetAxesPosition_Object(staItem.ResourceName);
                        }
                        break;
                    case ResourceItemType.VISION:
                        {
                            staticResObj = resourceProvider.Local_Vision_ResourceProvider.GetVisionController_Object(staItem.ResourceName);
                        }
                        break;
                }
                if (staticResObj == null)
                {
                    this.Log_Global($"未找到测试模块[{this.Name}]所需资源[{staItem.ResourceType} - {staItem.ResourceName}]!");
                    return false;
                }
                ModuleResource.Add(staItem.ResourceName, staticResObj);
            }
            return true;
        }
        public abstract void Run(CancellationToken token);
        public virtual void RunPostAction(CancellationToken token) { }
        public virtual void RunRreAction(CancellationToken token) { }
        public virtual Form CreateCustomizeUI() { throw new NotImplementedException(); }
        public virtual Form GetUI()
        {
            if (this._UI == null || this._UI.IsDisposed)
            {
                switch (UIType)
                {
                    //Form_RawDataViewer_FF
                    case TestModuleUIType.Stardard:
                        this._UI = new Form_ModuleChart();
                        break;
                    case TestModuleUIType.Customize:
                        this._UI = CreateCustomizeUI();
                        break;
                    case TestModuleUIType.None:
                        this._UI = null;
                        break;
                }
            }
            return this._UI;
        }
        //public abstract void UpdateUI();
        public virtual void ClearUI()
        {
            if (this._UI == null || this._UI.IsDisposed)
            {
                return;
            }
            switch (UIType)
            {
                case TestModuleUIType.Stardard:
                    (this._UI as IForm_ModuleChart).ClearChart();
                    break;
                case TestModuleUIType.Customize:
                case TestModuleUIType.None:
                    break;
            }
        }
        public virtual void UpdateChartSeries(AxisType axisType, string legendName, IEnumerable<object> xData, IEnumerable<object> yData)
        {
            if (this._UI == null || this._UI.IsDisposed)
            {
                return;
            }
            switch (UIType)
            {
                case TestModuleUIType.Stardard:
                    (this._UI as IForm_ModuleChart).UpdateChartSeries(axisType, legendName, xData, yData);
                    break;
                case TestModuleUIType.Customize:
                case TestModuleUIType.None:
                    break;
            }
        }

        #region  log
        protected virtual void Log_Global(string log)
        {
            this._core?.Log_Global($"{this.Name} {log}");
            //this._core?.Log_Global($"[{this.Name} @ {this._exeInteration.Name}] {log}.");
        }
        protected virtual void Log_File(string log)
        {
            this._core?.Log_File($"{this.Name} {log}");
            //this._core?.Log_File($"[ {this.Name} @ {this._exeInteration.Name}] {log}.");
        }
        protected virtual void ReportException(string message, int errorCode, Exception e)
        {
            this._core?.ReportException($"{this.Name} {message}", errorCode, e);
            //this._core?.ReportException($"[{this.Name} @ {this._exeInteration.Name}] {message}.", errorCode, e);
        }

        public virtual Type GetTestRecipeType()
        {
            throw new NotImplementedException();
        }




        #endregion


        public virtual void GetReferenceFromDeviceStreamData(IDeviceStreamDataBase dutStreamData ) { }

    }
}