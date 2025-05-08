using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_Motion;
using SolveWare_Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    /// <summary>
    /// 封装CT40410A的业务运行逻辑
    /// </summary>
    public partial class PluginBizManager : TesterAppPluginModel
    {
        TestPluginResourceProvider_CT40410A pluginResource;

        ProviderManager_CT40410A providerResourse;

        //public List<UnLoadInfo> unLoadPosi_ed { get; set; }
        //public List<UnLoadInfo> unLoadPosi_ing { get; set; }


        //RunParamSettings runParamSettings
        //{
        //    get
        //    {
        //        return providerResourse.RunSettings_Provider._RunParamSettings;
        //    }

        //}

        public PluginBizService pluginBizService { get; set; }


        //static PluginBizManager _instance;
        //static object _mutex = new object();
        //public static PluginBizManager Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (_mutex)
        //            {
        //                if (_instance == null)
        //                {
        //                    _instance = new PluginBizManager();
        //                }
        //            }
        //        }
        //        return _instance;
        //    }
        //}

        public void Setup(TestPluginResourceProvider_CT40410A pluResource, ProviderManager_CT40410A proResource)
        {

            pluginBizService = new PluginBizService();
            pluginBizService.ConnectToCore(this._coreInteration);
            pluginBizService.Setup(pluResource, proResource);
            pluginResource = pluResource;
            providerResourse = proResource;

            //后期这两个数据要改动
            //runParamSettings = this.providerResourse.RunSettings_Provider._RunParamSettings;
        }


        public void ResetAllMotionActionToken()
        {
            pluginBizService.ResetAllMotionActionToken();
        }

        public void StopAllMotionAction()
        {
            pluginBizService.StopAllMotionAction();
        }



        #region 视觉命令集识别

        public OperationResult<VisionResult_LaserX_Image_Universal> VisionCmdList_Identity(string visionCmdPurpose)
        {
            VisionResult_LaserX_Image_Universal visionResult = new VisionResult_LaserX_Image_Universal();
            try
            {
                var cmdList = this.providerResourse.VisionComboCommand_Provider[visionCmdPurpose];

                foreach (var cmd in cmdList)
                {
                    visionResult = this.pluginBizService.CameraRecognize(cmd, false);
                    if (visionResult.Success)
                    {
                        break;
                    }
                    else
                    {
                    }
                    Thread.Sleep(20);
                }
                if (visionResult.Success)
                {
                    return OperationResult.SuccessResult(visionResult);
                }
                else
                {
                    Thread.Sleep(200);
                }

                return OperationResult.FailResult(visionResult, $"{visionCmdPurpose} 视觉命令集执行后没有找到匹配项!");
            }
            catch (Exception ex)
            {
                return OperationResult.FailResult(visionResult, ex.Message);
            }
        }

        #endregion



    }
}
