using SolveWare_BurnInAppInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SolveWare_IO
{
    public class IOManager : TesterAppPluginUIModel, ITesterAppPluginInteration, IIOResourceProvider
    {

        string default_PluginExecutor = string.Empty;
        IIOController defaultController { get; set; }
        public IOManager()
        {
        
        }
        public IOManagerConfig Config
        {
            get
            {
                return IOManagerConfig.Instance;
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

            defaultController = (IIOController)this._coreInteration.GetStationHardwareObject(this.ConfigItem.PluginExecutor.Trim());
        }

        internal bool DeleteIoSettingItem(string name, short cardNo, short slaveNo, short bit, short logic, IOType ioType, bool isExtIo)
        {
            return IOManagerConfig.Instance.IOGeneralSetting.DeleteSingleItem(name, cardNo, slaveNo, bit, logic, ioType, isExtIo);
        }

        public override void Close()
        {
            defaultController.ClearIOInstances();

        }
        public object GetIO_Object(string name)
        {
            return defaultController.GetIO(name);
        }
        internal IOBase GetIO(string ioName)
        {
            return defaultController.GetIO(ioName);
        }
        internal IOBase GetIO(long ioId)
        {
            return defaultController.GetIO(ioId);
        }
        internal IOBase GetIO(short slaveNo, short bit, IOType iOType)
        {
            return defaultController.GetIO(slaveNo, bit, iOType);
        }
  
        internal List<IOBase> GetIOBaseCollection()
        {
            return defaultController.GetIOBaseCollection();
        }
        //将配置文件全部都实例化成指定对象集合
        public override void ReinstallController()
        {
            defaultController.CreateIOInstances(this.Config.IOGeneralSetting);
        }
 
        //保存所有的参数
        public override void Dev()
        {
            try
            {
                var path = @"D:\tfs_tp\BT-0632\LaserX_TesterLibrary\Playground\hello1111.xml";
                IOManagerConfig.Instance.Save(path);
            }
            catch (Exception ex)
            {

            }

        }
        //保存所有的配置
        public override string SaveConfig()
        {
            string errMsg = string.Empty;
            try
            {
                IOManagerConfig.Instance.Save(this.ConfigItem.PluginConfigFile);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + ex.StackTrace;
            }
            return errMsg;
        }

        //将所有的配置反序列出来到变量
        protected override void Initialize(CancellationToken token)
        {
            if (string.IsNullOrEmpty(this.ConfigItem.PluginConfigFile))
            {
                throw new FileNotFoundException($"{this.Name} 配置文件为空!");
            }
            try
            {
                IOManagerConfig.Instance.Load(this.ConfigItem.PluginConfigFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //UI交互
        public override void CreateMainUI()
        {
            if (this._MainPageUI == null || this._MainPageUI.IsDisposed)
            {
                this._MainPageUI = new Form_IOManager();
                this._coreInteration.LinkToCore(_MainPageUI as ITesterAppUI);
                (this._MainPageUI as ITesterAppUI).ConnectToAppInteration(this);
            }
        }

    }
}