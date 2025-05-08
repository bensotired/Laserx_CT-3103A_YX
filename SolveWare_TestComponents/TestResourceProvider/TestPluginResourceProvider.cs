using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SolveWare_TestComponents.ResourceProvider
{

    public abstract class TestPluginResourceProvider : TesterAppPluginModel, ITesterCoreLink, ITestPluginResourceProvider, IDisposable //单元运行状态
    {
        //protected TestPluginConfigItem _resourceItems;
        //protected Dictionary<string, TestExecutorUnit> _exeUnits = new Dictionary<string, TestExecutorUnit>();
        public Dictionary<string, object> Resource_Instruments { get; private set; } = new Dictionary<string, object>();
        public Dictionary<string, object> Resource_Axes { get; private set; } = new Dictionary<string, object>();
        public Dictionary<string, object> Resource_IO { get; private set; } = new Dictionary<string, object>();
        public Dictionary<string, object> Resource_Posititon { get; private set; } = new Dictionary<string, object>();
        public Dictionary<string, object> Resource_VisionController { get; private set; } = new Dictionary<string, object>();
        public ISpecResourceProvider Local_Spec_ResourceProvider { get; private set; }
        public IAxisResourceProvider Local_Axis_ResourceProvider { get; private set; }
        public IIOResourceProvider Local_IO_ResourceProvider { get; private set; }
        public IAxesPositionResourceProvider Local_AxesPosition_ResourceProvider { get; private set; }
        public IVisionResourceProvider Local_Vision_ResourceProvider { get; private set; }
        public virtual void Setup(TestPluginConfigItem resourceItems)
        {
            foreach (var item in resourceItems.ResourcePluginItems)
            {

                switch (item.ItemType)
                {
                    case ResourceItemType.AXIS:
                        {
                            var app = this.CoreResource.GetAppPlugin(item.AppPluginName);
                            if (app == null)
                            {
                                throw new Exception($"未能为测试组件加载APP插件[{item.AppPluginName}]!");
                            }
                            Local_Axis_ResourceProvider = (IAxisResourceProvider)app;
                            foreach (var resName in item.ResourceItemList)
                            {
                                var obj = Local_Axis_ResourceProvider.GetAxis_Object(resName);
                                if (obj == null)
                                {
                                    throw new Exception($"未能为测试组件加载资源[{resName}]!");
                                }
                                Resource_Axes.Add(resName, obj);
                                //Resource_MotionAction.Add(resName, new MotionActionV2());
                            }
                        }
                        break;
                    case ResourceItemType.IO:
                        {
                            var app = this.CoreResource.GetAppPlugin(item.AppPluginName);
                            if (app == null)
                            {
                                throw new Exception($"未能为测试组件加载APP插件[{item.AppPluginName}]!");
                            }
                            Local_IO_ResourceProvider = (IIOResourceProvider)app;
                            foreach (var resName in item.ResourceItemList)
                            {
                                var obj = Local_IO_ResourceProvider.GetIO_Object(resName);
                                if (obj == null)
                                {
                                    throw new Exception($"未能为测试组件加载资源[{resName}]!");
                                }
                                Resource_IO.Add(resName, obj);
                            }
                        }
                        break;
                    case ResourceItemType.VICAL:
                    case ResourceItemType.POS:
                        {
                            var app = this.CoreResource.GetAppPlugin(item.AppPluginName);
                            if (app == null)
                            {
                                throw new Exception($"未能为测试组件加载APP插件[{item.AppPluginName}]!");
                            }
                            Local_AxesPosition_ResourceProvider = (IAxesPositionResourceProvider)app;
                            foreach (var resName in item.ResourceItemList)
                            {
                                var obj = Local_AxesPosition_ResourceProvider.GetAxesPosition_Object(resName);
                                if (obj == null)
                                {
                                    throw new Exception($"未能为测试组件加载资源[{resName}]!");
                                }
                                Resource_Posititon.Add(resName, obj);
                            }
                        }
                        break;

                    case ResourceItemType.VISION:
                        {
                            var app = this.CoreResource.GetAppPlugin(item.AppPluginName);
                            if (app == null)
                            {
                                throw new Exception($"未能为测试组件加载APP插件[{item.AppPluginName}]!");
                            }
                            //return (TResource)this.Local_Vision_ResourceProvider.GetVisionController_Object(name);
                            Local_Vision_ResourceProvider = (IVisionResourceProvider)app;
                            foreach (var resName in item.ResourceItemList)
                            {
                                var obj = Local_Vision_ResourceProvider.GetVisionController_Object(resName);
                                if (obj == null)
                                {
                                    throw new Exception($"未能为测试组件加载资源[{resName}]!");
                                }
                                Resource_VisionController.Add(resName, obj);
                            }
                        }
                        break;
                    case ResourceItemType.SPEC:
                        {
                            var app = this.CoreResource.GetAppPlugin(item.AppPluginName);
                            if (app == null)
                            {
                                throw new Exception($"未能为测试组件加载APP插件[{item.AppPluginName}]!");
                            }
                            Local_Spec_ResourceProvider = (ISpecResourceProvider)app;
                        }
                        break;
                    case ResourceItemType.INSTR:
                        {
                            foreach (var resName in item.ResourceItemList)
                            {
                                var obj = this._coreInteration.GetStationHardwareObject(resName);
                                if (obj == null)
                                {
                                    throw new Exception($"未能为测试组件加载资源[{resName}]!");
                                }
                                Resource_Instruments.Add(resName, obj);
                            }
                        }
                        break;

                }
            }


        }

        public virtual void ClearResource()
        {
            this.Resource_Axes.Clear();
            this.Resource_IO.Clear();
            this.Resource_Posititon.Clear();
            this.Resource_VisionController.Clear();
            this.Resource_Instruments.Clear();

            this.Local_Vision_ResourceProvider = null;
            this.Local_Spec_ResourceProvider = null;
            this.Local_Axis_ResourceProvider = null;
            this.Local_IO_ResourceProvider = null;
            this.Local_AxesPosition_ResourceProvider = null;
        }
        public abstract bool MonitorKeyResourceStatus(CancellationTokenSource tokenSource);
        protected virtual TResource GetLocalResource<TResource>(ResourceItemType resT, string name)
        {
            switch (resT)
            {
                case ResourceItemType.IO:
                    {
                        if (this.Resource_IO.ContainsKey(name))
                        {
                            return (TResource)this.Resource_IO[name];
                        }
                        else
                        {
                            throw new Exception($"未能为测试组件本地化资源[{resT}][{name}]!");
                        }
                    }
                    break;
                case ResourceItemType.AXIS:
                    {
                        if (this.Resource_Axes.ContainsKey(name))
                        {
                            return (TResource)this.Resource_Axes[name];
                        }
                        else
                        {
                            throw new Exception($"未能为测试组件本地化资源[{resT}][{name}]!");
                        }
                    }
                    break;
                case ResourceItemType.VICAL:
                case ResourceItemType.POS:
                    {
                        if (this.Resource_Posititon.ContainsKey(name))
                        {
                            return (TResource)this.Resource_Posititon[name];
                        }
                        else
                        {
                            throw new Exception($"未能为测试组件本地化资源[{resT}][{name}]!");
                        }
                    }
                    break;
                case ResourceItemType.INSTR:
                    {
                        if (this.Resource_Instruments.ContainsKey(name))
                        {
                            return (TResource)this.Resource_Instruments[name];
                        }
                        else
                        {
                            throw new Exception($"未能为测试组件本地化资源[{resT}][{name}]!");
                        }
                    }
                    break;
                case ResourceItemType.VISION:
                    {
                        if (this.Resource_VisionController.ContainsKey(name))
                        {
                            return (TResource)this.Resource_VisionController[name];
                        }
                        else
                        {
                            throw new Exception($"未能为测试组件本地化资源[{resT}][{name}]!");
                        }
                    }
                    break;
            }
            return default(TResource);
        }

        //protected abstract TestExecutorUnit GetTestUnit(string unitName);

        public abstract void LocalizeResource();

        public virtual void Dispose()
        {
            Resource_Instruments = null;
            Resource_Axes = null;
            Resource_IO = null;
            Resource_Posititon = null;
            Resource_VisionController = null;
            Local_Spec_ResourceProvider = null;
            Local_Axis_ResourceProvider = null;
            Local_IO_ResourceProvider = null;
            Local_AxesPosition_ResourceProvider = null;
            Local_Vision_ResourceProvider = null;
        }
    }
}