using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SolveWare_BurnInAppInterface
{
    public abstract class TesterAppPluginUIModel : TesterAppPluginModel, ITesterAppPluginInteration, ITesterCoreLink, IAccessPermissionLevel
    {
 
        public abstract bool CanCurrentOwnerAccessResource(GenernalResourceOwner currnetResourceOwner, string resourceOwnerName);
    
        AppPluginConfigItem _configItem;
        protected AppPluginConfigItem ConfigItem
        {
            get
            {
                return _configItem;
            }
            set
            {
                _configItem = value;
            }
        }
        public virtual bool IsPluginEnable
        {
            get
            {
                return (bool)this._configItem?.PluginEnable;
            }
        }

        public virtual bool IsPlugin_PF_UIVisible
        {
            get
            {
                return (bool)this._configItem?.Plugin_PF_UIVisible;
            }
        }


        public override AccessPermissionLevel APL
        {
            get
            {
                return (AccessPermissionLevel)this._configItem?.PluginAccessLevel;
            }
        }
        string _versionInfo;
        public string VersionInfo
        {
            get
            {
                if (string.IsNullOrEmpty(_versionInfo))
                {
                    _versionInfo = this.GetType().Assembly.GetName().Version.ToString();
                    try
                    {
                        var temp = this.GetType().Assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute));
                        var bdate = (new List<Attribute>(temp).First() as AssemblyFileVersionAttribute).Version;
                        _versionInfo += "-" + bdate;
                    }
                    catch
                    {

                    }

                }
                return _versionInfo;
            }
        }
        protected Form _MainPageUI;
   
        public virtual void SetConfigItem(AppPluginConfigItem configItem)
        {
            this.ConfigItem = configItem;
            this.Name = configItem.PluginName;
        }
        public virtual void Dev()
        {

        }
        public abstract void CreateMainUI();

        public virtual void PopUI()
        {
            try
            {
                if (this._MainPageUI == null) { return; }
                this._coreInteration.PopUI(this._MainPageUI);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{this.Name}]窗口浮动错误:{ex.Message}-{ex.StackTrace}!");
            }
        }

        public virtual void DockUI()
        {
            try
            {
                if (this._MainPageUI == null) { return; }
                this._coreInteration.DockUIForAppPlugin(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{this.Name}]窗口还原错误:{ex.Message}-{ex.StackTrace}!");
            }
        }

        public virtual object GetUI()
        {
            return this._MainPageUI;
        }
        public virtual void RefreshOnceUI()
        {
       
        }

        public virtual string SaveConfig()
        {
            throw new NotImplementedException();
        }

        public virtual string SaveData()
        {
            throw new NotImplementedException();
        }

        public virtual string LoadConfig()
        {
            throw new NotImplementedException();
        }

        public virtual string LoadData()
        {
            throw new NotImplementedException();
        }

        public abstract void ReinstallController();

        public virtual bool SwitchProductConfig()
        {
            return true;
        }

        public virtual bool CreateProductConfig()
        {
            return true;
        }

        
    }
}