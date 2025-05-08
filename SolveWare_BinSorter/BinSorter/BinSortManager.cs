using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SolveWare_BinSorter
{
    public class BinSortManager : TesterAppPluginUIModel, ITesterAppPluginInteration ,IBinSortResourceProvider
    {
        static BinSortManager _instance;
        static object _mutex = new object();
        //public static BinSortManager Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            lock (_mutex)
        //            {
        //                if (_instance == null)
        //                {
        //                    _instance = new BinSortManager();
        //                }
        //            }
        //        }
        //        return _instance;
        //    }
        //}
        BinSettingCollection _CurrentBinSettingCollection;
        public BinSettingCollection CurrentBinSettingCollection
        {
            get { return _CurrentBinSettingCollection; }
        }
        public object GetBinSettingCollection()
        {
            return _CurrentBinSettingCollection;
        }
        public override void StartUp()
        {
            this._myTokenSource = new CancellationTokenSource();
            this.Initialize(this._myTokenSource.Token);

            this.CreateMainUI();

            this._coreInteration.GUIRunUIInvokeAction(() =>
            {
                this._MainPageUI.Show();
                this._MainPageUI.Hide();
            });
        }
        protected override void Initialize(CancellationToken token)
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(BinSettingCollection.ConfigFileName);
                try
                {
                    this._CurrentBinSettingCollection = this.Load_Collection(fullpath);
                    if (this._CurrentBinSettingCollection == null)
                    {
                        this._CurrentBinSettingCollection = new BinSettingCollection();
                        this.Save_Collection(fullpath, _CurrentBinSettingCollection);
                    }

                }
                catch
                {

                }
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] BinSettingCollection 配置参数失败!");
            }
          
        }
        public virtual bool UpdateTestSpecInCollection(BinSetting spec, ref string sErr)
        {
            try
            {
                if (_CurrentBinSettingCollection == null)
                {
                    return false;
                }
                return (bool)_CurrentBinSettingCollection.UpdateSingleItem(spec, ref sErr);
            }
            catch
            {
                return false;
            }
        }
        public virtual bool UpdateTestSpecInCollection(string binName, BinSetting spec, ref string sErr)
        {
            try
            {
                if (_CurrentBinSettingCollection == null)
                {
                    return false;
                }
                return (bool)_CurrentBinSettingCollection.UpdateSingleItem(binName,spec, ref sErr);
            }
            catch
            {
                return false;
            }
        }
        public virtual List<string> GetBinSettingTags()
        {
            if(_CurrentBinSettingCollection == null)
            {
                return new List<string>();
            }
            return _CurrentBinSettingCollection.GetDataListByPropName<string>("BinTag");
        }
        public virtual bool DeleteTestSpecFromCollection(BinSetting bin, ref string sErr)
        {
            if (_CurrentBinSettingCollection == null)
            {
                return false;
            }
            //int index = _CurrentBinSettingCollection.ItemCollection.FindIndex(item=> 
            //                                item.Name == bin.Name &&
            //                                item.Version == bin.Version);

            int index = _CurrentBinSettingCollection.ItemCollection.FindIndex(item =>
                                         item.Name == bin.Name  );
            if (index < 0)
            {
                sErr = "无可删除物件";
                return false;
            }
            _CurrentBinSettingCollection.ItemCollection.RemoveAt(index);
            return true;
        }
        public virtual bool DeleteTestSpecFromCollection(string binName, ref string sErr)
        {
            if (_CurrentBinSettingCollection == null)
            {
                return false;
            }
            //int index = _CurrentBinSettingCollection.ItemCollection.FindIndex(item=> 
            //                                item.Name == bin.Name &&
            //                                item.Version == bin.Version);

            int index = _CurrentBinSettingCollection.ItemCollection.FindIndex(item =>
                                         item.Name == binName);
            if (index < 0)
            {
                sErr = "无可删除物件";
                return false;
            }
            _CurrentBinSettingCollection.ItemCollection.RemoveAt(index);
            return true;
        }
        public virtual void AddBinSettingToCollection(BinSetting bin)
        {
            try
            {
                if (_CurrentBinSettingCollection == null)
                {
                    MessageBox.Show($"当前并未加载任何BinSettingCollection!!");
                    return;
                }
                _CurrentBinSettingCollection.AddSingleItem(bin);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"AddBinSettingToCollection ex:{ex.Message}-{ex.StackTrace}！");
            }
        }
        public BinSetting GetBinByTag(string binTag)
        {
            return _CurrentBinSettingCollection.ItemCollection.Find(item => item.BinTag == binTag);
        }
        public BinSetting Load_BinSetting(string filePath)
        {
            try
            {
                var obj = XmlHelper.DeserializeFile<BinSetting>(filePath);
                return obj;
            }
            catch (Exception ex)
            {

            }
            return new BinSetting();
        }
        public void Save_BinSetting(string filePath, BinSetting bscObj)
        {
            try
            {
                XmlHelper.SerializeFile<BinSetting>(filePath, bscObj);

            }
            catch (Exception ex)
            {

            }
        }

        public BinSettingCollection Load_Collection(string filePath)
        {
            try
            {
                var obj = XmlHelper.DeserializeFile<BinSettingCollection>(filePath);
                return obj;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public void Save_Collection(string filePath, BinSettingCollection bscObj)
        {
            try
            {
                XmlHelper.SerializeFile<BinSettingCollection>(filePath, bscObj);

            }
            catch (Exception ex)
            {

            }
        }

        public override bool CanCurrentOwnerAccessResource(GenernalResourceOwner currnetResourceOwner,
            string resourceOwnerName)
        {
            switch (currnetResourceOwner)
            {
                case GenernalResourceOwner.Platform:
                case GenernalResourceOwner.Plugin:
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }
        }
        public override void CreateMainUI()
        {
            if (this._MainPageUI == null || this._MainPageUI.IsDisposed)
            {
                this._MainPageUI = new Form_BinSorter();
                this._coreInteration.LinkToCore(_MainPageUI as ITesterAppUI);
                (this._MainPageUI as ITesterAppUI).ConnectToAppInteration(this);
            }
        }
        public void Save_Collection()
        {
            try
            {
                try
                {
                    var fullpath = this._coreInteration.GetProductConfigFileFullPath(BinSettingCollection.ConfigFileName);
                    this.Save_Collection(fullpath, _CurrentBinSettingCollection);
                }
                catch
                {
                    throw new Exception($"保存产品[{this._coreInteration.CurrentProductName}] BinSettingCollection 配置参数失败!");
                }
            }
            catch (Exception ex)
            {
            }
        }
        public override void ReinstallController()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(BinSettingCollection.ConfigFileName);
                try
                {
                    this._CurrentBinSettingCollection = this.Load_Collection(fullpath);
                    if (this._CurrentBinSettingCollection == null)
                    {
                        this._CurrentBinSettingCollection = new BinSettingCollection();
                        this.Save_Collection(fullpath, _CurrentBinSettingCollection);
                    }
                 
                }
                catch 
                {

                }
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] BinSettingCollection 配置参数失败!");
            }
        }
        public override bool SwitchProductConfig()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(BinSettingCollection.ConfigFileName);
                try
                {
                    this._CurrentBinSettingCollection = this.Load_Collection(fullpath);
                    if (this._CurrentBinSettingCollection == null)
                    {
                        this._CurrentBinSettingCollection = new BinSettingCollection();
                        this.Save_Collection(fullpath, _CurrentBinSettingCollection);
                    } 
                    (this._MainPageUI as ITesterAppUI).RefreshOnce();
                }
                catch
                {

                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] BinSettingCollection 配置参数失败!");
            }
        }

        public override bool CreateProductConfig()
        {
            try
            {
                var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(BinSettingCollection.ConfigFileName);
                if (string.IsNullOrEmpty(fullpath))
                {
                    throw new FileNotFoundException($"{this.Name} 子配置 BinSettingCollection 文件为空!");
                }
                try
                {
                    if(this._CurrentBinSettingCollection == null)
                    {
                        this._CurrentBinSettingCollection = new BinSettingCollection();
                    }
                    this.Save_Collection(fullpath, this._CurrentBinSettingCollection);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] BinSettingCollection 配置参数失败!");
            }
        }

       
    }
}