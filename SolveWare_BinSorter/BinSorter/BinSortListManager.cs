using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SolveWare_BinSorter
{
    public class BinSortListManager : TesterAppPluginUIModel, ITesterAppPluginInteration , IBinSortListResourceProvider
    {
        static BinSortListManager _instance;
        static object _mutex = new object();
        static string Properties_Resources = "SolveWare_BinSorter.Properties.Resources";
        internal void LoadBinJudgeItemNames()
        {
            try
            {

                Assembly asm = Assembly.GetExecutingAssembly();
                ResourceManager rm = new ResourceManager(Properties_Resources, asm);
                var wftRes = rm.GetString("BinJudgeItemNames");
                XmlSerializer se = new XmlSerializer(typeof(List<BinJudgeItem>));
                XmlReader xr = XElement.Parse(wftRes).CreateReader();
               this.BinJudgeItemPool = (List<BinJudgeItem>)se.Deserialize(xr);


            }
            catch (Exception ex)
            {
            }
        }
        public Action< string> SetBinCollectionToApplication { get; set; }
        public List<BinJudgeItem> BinJudgeItemPool
        {
            get; private set; 
        }
        BinSettingCollectionList _CurrentBinSettingCollectionList;
        public BinSettingCollectionList CurrentBinSettingCollection
        {
            get { return _CurrentBinSettingCollectionList; }
        }
        public object GetBinSettingCollectionList()
        {
            return _CurrentBinSettingCollectionList;
        }
        public override void StartUp()
        {
            this._myTokenSource = new CancellationTokenSource();
            this.Initialize(this._myTokenSource.Token);
            this.LoadBinJudgeItemNames();
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
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(BinSettingCollectionList.ConfigFileName);
                try
                {
                    this._CurrentBinSettingCollectionList = this.Load_CollectionList(fullpath);
                    if (this._CurrentBinSettingCollectionList == null)
                    {
                        this._CurrentBinSettingCollectionList = new BinSettingCollectionList();
                        this.Save_CollectionList(fullpath, _CurrentBinSettingCollectionList);
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
        public virtual bool UpdateBinSettingCollection_Of_MasterList(BinSettingCollection item, ref string sErr)
        {
            try
            {
                if (_CurrentBinSettingCollectionList == null)
                {
                    return false;
                }
                return (bool)_CurrentBinSettingCollectionList.UpdateSingleItem(item, ref sErr);
            }
            catch
            {
                return false;
            }
        }
        public virtual bool UpdateBinSettingCollection_Of_MasterList(string binName, BinSettingCollection item, ref string sErr)
        {
            try
            {
                if (_CurrentBinSettingCollectionList == null)
                {
                    return false;
                }
                return (bool)_CurrentBinSettingCollectionList.UpdateSingleItem(binName, item, ref sErr);
            }
            catch
            {
                return false;
            }
        }
        public virtual List<string> GetBinSettingCollectionNames()
        {
            if (_CurrentBinSettingCollectionList == null)
            {
                return new List<string>();
            }
            return _CurrentBinSettingCollectionList.GetDataListByPropName<string>("Name");
        }
        public virtual bool DeleteBinSettingCollection_Of_MasterList(BinSettingCollection bin, ref string sErr)
        {
            if (_CurrentBinSettingCollectionList == null)
            {
                return false;
            }
            int index = _CurrentBinSettingCollectionList.ItemCollection.FindIndex(item =>
                                         item.Name == bin.Name);
            if (index < 0)
            {
                sErr = "无可删除物件";
                return false;
            }
            _CurrentBinSettingCollectionList.ItemCollection.RemoveAt(index);
            return true;
        }
        public virtual bool DeleteBinSettingCollection_Of_MasterList(string binCollectionName, ref string sErr)
        {
            if (_CurrentBinSettingCollectionList == null)
            {
                return false;
            }
            int index = _CurrentBinSettingCollectionList.ItemCollection.FindIndex(item =>
                                         item.Name == binCollectionName);
            if (index < 0)
            {
                sErr = "无可删除物件";
                return false;
            }
            _CurrentBinSettingCollectionList.ItemCollection.RemoveAt(index);
            return true;
        }
        public virtual void AddBinSettingCollection_Of_MasterList(BinSettingCollection item)
        {
            try
            {
                if (_CurrentBinSettingCollectionList == null)
                {
                    MessageBox.Show($"当前并未加载任何BinSettingCollection!!");
                    return;
                }
                _CurrentBinSettingCollectionList.AddSingleItem(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"AddBinSettingToCollection ex:{ex.Message}-{ex.StackTrace}！");
            }
        }
        public BinSettingCollection GetBinSettingCollection(string binSettingCollectionName)
        {
            return _CurrentBinSettingCollectionList.ItemCollection.Find(item => item.Name == binSettingCollectionName);
        }
        public object GetBinSettingCollectionObject(string binSettingCollectionName)
        {
            return _CurrentBinSettingCollectionList.ItemCollection.Find(item => item.Name == binSettingCollectionName);
        }

        public BinSettingCollectionList Load_CollectionList(string filePath)
        {
            try
            {
                var obj = XmlHelper.DeserializeFile<BinSettingCollectionList>(filePath);
                return obj;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public void Save_CollectionList(string filePath, BinSettingCollectionList bscObj)
        {
            try
            {
                XmlHelper.SerializeFile<BinSettingCollectionList>(filePath, bscObj);

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
                this._MainPageUI = new Form_BinSorterListManager();
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
                    var fullpath = this._coreInteration.GetProductConfigFileFullPath(BinSettingCollectionList.ConfigFileName);
                    this.Save_CollectionList(fullpath, _CurrentBinSettingCollectionList);
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
                //"D:\\chiptester_Main\\TestPlatform_SourceCode_v2.0.0.0_20230428_格恩送样现场_本地化测试方案\\LaserX_TesterLibrary\\ProductConfig\\GAN_小功率\\BinSettingCollectionList.xml"
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(BinSettingCollectionList.ConfigFileName);
                try
                {
                    this._CurrentBinSettingCollectionList = this.Load_CollectionList(fullpath);
                    if (this._CurrentBinSettingCollectionList == null)
                    {
                        this._CurrentBinSettingCollectionList = new BinSettingCollectionList();
                        this.Save_CollectionList(fullpath, _CurrentBinSettingCollectionList);
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
            {//BinSettingCollection.xml
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(BinSettingCollectionList.ConfigFileName);
                try
                {
                    this._CurrentBinSettingCollectionList = this.Load_CollectionList(fullpath);
                    if (this._CurrentBinSettingCollectionList == null)
                    {
                        this._CurrentBinSettingCollectionList = new BinSettingCollectionList();
                        this.Save_CollectionList(fullpath, _CurrentBinSettingCollectionList);
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
                var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(BinSettingCollectionList.ConfigFileName);
                if (string.IsNullOrEmpty(fullpath))
                {
                    throw new FileNotFoundException($"{this.Name} 子配置 BinSettingCollection 文件为空!");
                }
                try
                {
                    if (this._CurrentBinSettingCollectionList == null)
                    {
                        this._CurrentBinSettingCollectionList = new BinSettingCollectionList();
                    }
                    this.Save_CollectionList(fullpath, this._CurrentBinSettingCollectionList);
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