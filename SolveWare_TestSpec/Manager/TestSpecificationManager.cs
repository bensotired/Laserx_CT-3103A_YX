using SolveWare_BurnInAppInterface;
using SolveWare_TestComponents.Specification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SolveWare_TestSpecification
{
    public class TestSpecManager : TesterAppPluginUIModel, ITesterAppPluginInteration, ISpecResourceProvider
    {
        public TestSpecManager()
        {

        }
        TestSpecManagerConfig Config
        {
            get
            {
                return TestSpecManagerConfig.Instance;
            }
        }
        public override bool CanCurrentOwnerAccessResource(GenernalResourceOwner currnetResourceOwner, string resourceOwnerName)
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
        public override void Close()
        {


        }
        public override void ReinstallController()
        {
            //  throw new NotImplementedException();
        }


        public override void Dev()
        {
            try
            {


            }
            catch (Exception ex)
            {

            }

        }
        public TestSpecCollection GetTestSpecCollection()
        {
            return TestSpecManagerConfig.Instance.TestSpecCollection;
        }
        public virtual bool DeleteTestSpecFromCollection(long Id, ref string sErr)
        {
            return TestSpecManagerConfig.Instance.TestSpecCollection.DeleteSingleItem(Id, ref sErr);
        }
        public virtual void AddTestSpecToCollection(TestSpecification spec)
        {
            TestSpecManagerConfig.Instance.TestSpecCollection.AddSingleItem(spec);
        }
        public virtual bool UpdateTestSpecInCollection(TestSpecification spec, ref string sErr)
        {
            return TestSpecManagerConfig.Instance.TestSpecCollection.UpdateSingleItem(spec, ref sErr);
        }
        public virtual List<string> GetSpecificationTags()
        {
            return TestSpecManagerConfig.Instance.TestSpecCollection.GetDataListByPropName<string>("SpecTag");
        }
        public virtual TestSpecification GetTestSpecByTag(string specTag)
        {
            return TestSpecManagerConfig.Instance.TestSpecCollection.GetSpecByTag(specTag);
        }

        public virtual object GetSpecResource(string specTag)
        {
            return GetTestSpecByTag(specTag);
        }
        //保存
        public override string SaveConfig()
        {
            string errMsg = string.Empty;
            try
            {
                TestSpecManagerConfig.Instance.Save(this.ConfigItem.PluginConfigFile);

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
                throw new FileNotFoundException($"{this.Name} 配置文件为空!");
            }
            try
            {
                TestSpecManagerConfig.Instance.Load(this.ConfigItem.PluginConfigFile);
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
                this._MainPageUI = new Form_TestSpecManager();
                this._coreInteration.LinkToCore(_MainPageUI as ITesterAppUI);
                (this._MainPageUI as ITesterAppUI).ConnectToAppInteration(this);
            }
        }
    }
}