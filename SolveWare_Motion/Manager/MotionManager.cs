using SolveWare_BurnInAppInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_Motion
{
    public class MotionManager : TesterAppPluginUIModel, ITesterAppPluginInteration, IAxisResourceProvider, IAxesPositionResourceProvider
    {

        string default_PluginExecutor = string.Empty;
        public IMotionController DefaultController { get; private set; }
        MotionManagerConfig _interalManagerConfig { get; set; }
        MotionPositionConfig _interalPositionConfig { get; set; }
        public MotionManager()
        {
            //this._AxesCollection = new List<MotorAxisBase>();
            this._interalManagerConfig = new MotionManagerConfig();
            this._interalPositionConfig = new MotionPositionConfig();
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
        public MotionManagerConfig Config
        {
            get
            {
                return this._interalManagerConfig;
                //return MotionManagerConfig.Instance;
            }
        }
        public override bool SwitchProductConfig()
        {
            this.InitializePositionConfig(this._myTokenSource.Token);
            (this._MainPageUI as ITesterAppUI).RefreshOnce();
            return true;
        }
        public override void RefreshOnceUI()
        {
            (this._MainPageUI as ITesterAppUI).RefreshOnce();
        }
        public override void StartUp()
        {
            this._myTokenSource = new CancellationTokenSource();
            this.Initialize(this._myTokenSource.Token);
            this.InitializePositionConfig(this._myTokenSource.Token);
            this.CreateMainUI();

            this._coreInteration.GUIRunUIInvokeAction(() =>
            {
                this._MainPageUI.Show();
                this._MainPageUI.Hide();
            });
            //MotionController_GUGAOEtherCAT
            //根据指定的文字可以转换成实际的接口 
            DefaultController = (IMotionController)this._coreInteration.GetStationHardwareObject(this.ConfigItem.PluginExecutor.Trim());
            DefaultController.LoadSpecificConfig(this.ConfigItem);
        }
        public override void Close()
        {
            DefaultController.StopAxesReading();
            DefaultController.ClearAxisInstances();
        }
 
        public object GetAxis_Object(string name)
        {
            //轴名字是最外面的
            return DefaultController.GetAxis(name);
        }
        public object GetAxesPosition_Object(string name)
        {
            return this._interalPositionConfig.AxesPositionCollection.GetSingleItem(name);

        }
        public List<MotorRuntimeInteration> AxesPositionMonitor
        {
            get
            {
                if (this.DefaultController != null)
                {
                    return this.DefaultController.AxesPositionMonitor;
                }
                else
                {
                    return new List<MotorRuntimeInteration>();
                }
            }
        }
        public void UpdatePositionConfig(object positionName, ref string errMsg)
        {
            this._interalPositionConfig.AxesPositionCollection.UpdateSingleItem((AxesPosition)positionName, ref errMsg);
        }


        public object AxesPositionCollection
        {
            get
            {
                return this._interalPositionConfig.AxesPositionCollection;
            }
        }

        public virtual bool AddNewAxis(MotorSetting axisSetting)
        {
            bool isOk = false;
            try
            {
                string errMsg = string.Empty;
                isOk = this.Config.MotorGeneralSetting.AddSingleItem(axisSetting, ref errMsg);
                //轴名字是最外面的
                //return defaultController.GetAxis(axisName);
                if (isOk)
                {
                    var axisInstance = DefaultController.CreateAxisInstance(axisSetting);
                    DefaultController.AddAxisInstanceToCollection(axisInstance);
                }
                else
                {
                    MessageBox.Show($"新增轴失败:[{errMsg}]!");
                }
                return isOk;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新增轴失败:[{ex.Message}-{ex.StackTrace}]!");
            }
            return isOk;
        }
        public MotorAxisBase GetAxis(string axisName)
        {
            //轴名字是最外面的
            return DefaultController.GetAxis(axisName);
        }
        internal MotorAxisBase GetAxis(long axisId)
        {
            return DefaultController.GetAxis(axisId);
        }
        internal MotorAxisBase GetAxis(short CardNo, short AxisNo)
        {
            return DefaultController.GetAxis(CardNo, AxisNo);
        }
        internal List<MotorAxisBase> GetAxesCollection()
        {
            return DefaultController.GetAxesCollection();
        }
        public List<string> GetAxesNameCollection()
        {
            List<string> temp = new List<string>();
            try
            {
                var axColt = DefaultController.GetAxesCollection();
                foreach (var ax in axColt)
                {
                    temp.Add(ax.MotorGeneralSetting.MotorTable.Name);
                }
            }
            catch
            {

            }

            return temp;
        }
        public List<string> GetPositionNameCollection()
        {
            List<string> temp = new List<string>();
            try
            {
                temp = this._interalPositionConfig.AxesPositionCollection.GetDataListByPropName<string>("Name");
            }
            catch
            {
               
            }
            return temp;
        }
        //获取已经存在的positionNames
        internal List<MotorAxisBase> GetAxesCollection_ByAxesPosition( AxesPosition ap)
        {
            List<MotorAxisBase> temp = new List<MotorAxisBase>();

            foreach(var item in ap)
            {
               temp.Add( this.GetAxis(item.Name));
            }
            return temp;
        }
        internal AxesPositionCollection GetPositionCollection()
        {
            return this._interalPositionConfig.AxesPositionCollection;
        }
        internal List<string> GetPositionNames()
        {
            return this._interalPositionConfig.AxesPositionCollection.GetDataListByPropName<string>("Name");
            //return MotionPositionConfig.Instance.AxesPositionCollection.ItemCollection.Select(item=>item.Name).ToList();
        }

        public AxesPosition GetAxesPosition(string positionName)
        {
            return this._interalPositionConfig.AxesPositionCollection.GetSingleItem(positionName);
            // return MotionPositionConfig.Instance.AxesPositionCollection.ItemCollection.Find(item => item.Name== positionName);
        }
        public override void ReinstallController()
        {
            DefaultController.CreateAxisInstances(this.Config.MotorGeneralSetting);
        }
       
        public override void Dev()
        {
            try
            {
                var path = @"D:\tfs_tp\BT-0632\LaserX_TesterLibrary\Playground\hello1111.xml";

                //MotionManagerConfig.Instance.Save(path);
                this._interalManagerConfig.Save(path);

            }
            catch (Exception ex)
            {

            }

        }
        public override string SaveConfig()
        {
            string errMsg = string.Empty;
            try
            {
                this._interalManagerConfig.Save(this.ConfigItem.PluginConfigFile);
                //MotionManagerConfig.Instance.Save(this.ConfigItem.PluginConfigFile);

            }
            catch (Exception ex)
            {
                errMsg = ex.Message + ex.StackTrace;
            }
            return errMsg;
        }
        protected override void Initialize(CancellationToken token)
        {
            if (string.IsNullOrEmpty(this.ConfigItem.PluginConfigFile))
            {
                throw new FileNotFoundException($"{this.Name} 主配置文件为空!");
            }
            try
            {
                this._interalManagerConfig.Load(this.ConfigItem.PluginConfigFile);
                //MotionManagerConfig.Instance.Load(this.ConfigItem.PluginConfigFile);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void CreateMainUI()
        {
            if (this._MainPageUI == null || this._MainPageUI.IsDisposed)
            {
                this._MainPageUI = new Form_MotionManager();
                this._coreInteration.LinkToCore(_MainPageUI as ITesterAppUI);
                (this._MainPageUI as ITesterAppUI).ConnectToAppInteration(this);

            }
        }
        public void RemovePositionConfig(string positionName)
        {
            this._interalPositionConfig.AxesPositionCollection.RemoveItem(positionName);
        }
        public override bool CreateProductConfig()
        {
            string errMsg = string.Empty;
            var fPath = this._coreInteration.Get_Create_ProductConfigFileFullPath(Path.GetFileName(this.ConfigItem.SlaverConfigFile)); //MotionPositionConfig.ConfigFileName);
            if (string.IsNullOrEmpty(fPath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置文件为空!");
            }
            try
            {
                this._interalPositionConfig.Save(fPath);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + ex.StackTrace;
                return false;
            }
            return true; 
        }
      
        public string SavePositionConfig()
        {
    
            string errMsg = string.Empty;
            var fPath = this._coreInteration.GetProductConfigFileFullPath(Path.GetFileName(this.ConfigItem.SlaverConfigFile)); //MotionPositionConfig.ConfigFileName);
            if (string.IsNullOrEmpty(fPath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置文件为空!");
            }
            try
            {
                this._interalPositionConfig.Save(fPath);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + ex.StackTrace;
            }
            return errMsg;
        }
        public void InitializePositionConfig(CancellationToken token)
        {
            var fPath = this._coreInteration.GetProductConfigFileFullPath(Path.GetFileName(this.ConfigItem.SlaverConfigFile)); //MotionPositionConfig.ConfigFileName);
            if (string.IsNullOrEmpty(fPath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置文件为空!");
            }
            try
            {
                this._interalPositionConfig.Load(fPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}