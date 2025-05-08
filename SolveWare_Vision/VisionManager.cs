using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using System;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_Vision
{
    public class VisionManager : TesterAppPluginUIModel, ITesterAppPluginInteration, IVisionResourceProvider
    {
        string default_PluginExecutor = string.Empty;
        IVisionController defaultController { get; set; }
        public override void CreateMainUI()
        {
            if (this._MainPageUI == null || this._MainPageUI.IsDisposed)
            {
                this._MainPageUI = new Form_VisionManager();
                this._coreInteration.LinkToCore(_MainPageUI as ITesterAppUI);
                (this._MainPageUI as ITesterAppUI).ConnectToAppInteration(this);

            }
        }
        public override bool CanCurrentOwnerAccessResource(GenernalResourceOwner currnetResourceOwner, string resourceOwnerName)
        {
            switch (currnetResourceOwner)
            {
                case GenernalResourceOwner.Platform:
                    //case GenernalResourceOwner.Plugin:
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }
        }
        public override void StartUp()
        {
            this._myTokenSource = new CancellationTokenSource();
            //这里将config反序化出来的 IOManagerConfig  也就是IOManagerConfig.Instance
            this.Initialize(this._myTokenSource.Token);

            this.CreateMainUI();

            this._coreInteration.GUIRunUIInvokeAction(() =>
            {
                this._MainPageUI.Show();
                this._MainPageUI.Hide();
            });

            defaultController = (IVisionController)this._coreInteration.GetStationHardwareObject(this.ConfigItem.PluginExecutor.Trim());
        }
        public VisionManagerConfig Config
        {
            get
            {
                return VisionManagerConfig.Instance;
            }
        }
        public override void Dev()
        {
            try
            {
                this.defaultController.Dev();
            }
            catch (Exception ex)
            {

            }
        }
        public void UpdateConfigAndSave(DataBook<int, string> uiCmdValues)
        {
            try
            {
                VisionManagerConfig.Instance.VisionCMDBook = uiCmdValues;

                VisionManagerConfig.Instance.Save(this.ConfigItem.PluginConfigFile);
            }
            catch (Exception ex)
            {

            }
        }
        public string GetVisionResult_Universal(string cmd)
        {
            string result = string.Empty;
            try
            {
                result = this.defaultController.GetVisionResult_UniversalWithResponTime(cmd, 500);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"视觉命令[{cmd}]执行错误:[{ex.Message}{ex.StackTrace}]!");
            }
            return result;
        }

        public object GetVisionController_Object(string name)
        {

            if (defaultController.Name.Equals(name))
            {
                return defaultController;
            }
            return null;
        }

        public override void ReinstallController()
        {
            //throw new NotImplementedException();
        }
    }
}
