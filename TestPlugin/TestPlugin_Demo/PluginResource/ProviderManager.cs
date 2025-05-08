using SolveWare_BurnInAppInterface;
using SolveWare_Motion;
using SolveWare_SlaveStation;
using SolveWare_Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    public class ProviderManager_CT3103 : TesterAppPluginUIModel
    {
        VisionComboCommand_Provider _VisionComboCommand_Provider;
        public VisionComboCommand_Provider VisionComboCommand_Provider
        {
            get
            {
                return this._VisionComboCommand_Provider;
            }
        }
        SlaveStation_Provider _SlaveStation_Provider;
        public SlaveStation_Provider SlaveStation_Provider
        {
            get
            {
                return this._SlaveStation_Provider;
            }
        }
        DeltaPix2Point_Provider_CT3103 _Dp2p_Provider;
        public DeltaPix2Point_Provider_CT3103 Dp2p_Provider
        {
            get
            {
                return this._Dp2p_Provider;
            }
        }

        MotionOffset_Provider_CT3103 _MotionOffset_Provider;
        public MotionOffset_Provider_CT3103 MotionOffset_Provider
        {
            get
            {
                return this._MotionOffset_Provider;
            }
        }

        PixelPoint_Provider_CT3103 _PixPoint_Provider;
        public PixelPoint_Provider_CT3103 PixPoint_Provider
        {
            get
            {
                return this._PixPoint_Provider;
            }
        }

        PPM_Provider _PPM_Provider;
        public PPM_Provider PPM_Provider
        {
            get
            {
                return this._PPM_Provider;
            }
        }

        RunSettings_Provider_CT3103 _RunSettings_Provider;
        public RunSettings_Provider_CT3103 RunSettings_Provider
        {
            get
            {
                return this._RunSettings_Provider;
            }
        }

        DelayTimeSettings_Provider_CT3103 _DelayTimeSettings_Provider;
        public DelayTimeSettings_Provider_CT3103 DelayTimeSettings_Provider
        {
            get
            {
                return this._DelayTimeSettings_Provider;
            }
        }


        InPutSettings_Provider_CT3103 _InPutSettings_Provider;
        public InPutSettings_Provider_CT3103 InPutSettings_Provider
        {
            get
            {
                return this._InPutSettings_Provider;
            }
        }


        OutPutSettings_Provider_CT3103 _OutPutSettings_Provider;
        public OutPutSettings_Provider_CT3103 OutPutSettings_Provider
        {
            get
            {
                return this._OutPutSettings_Provider;
            }
        }


        public override void ReinstallController()
        {
            //this.LoadProduct_Ppm_Config();
            ////this.LoadProduct_Dp2p_Config();
            ////this.LoadProduct_PixPoint_Config();
            //this.LoadProduct_MotionOffset_Config();
            //this.LoadProduct_RunParam_Config();
            //this.LoadProduct_DelayTimeSettings_Config();
            this.LoadProduct_SlaveStation_Config();
            //this.LoadProduct_VisionComboCommand_Config();
            //this.LoadProduct_OutPutSettings_Config();
            //this.LoadProduct_InPutSettings_Config();
        }

        public override bool SwitchProductConfig()
        {
            try
            {
                //this.LoadProduct_Ppm_Config();
                ////this.LoadProduct_Dp2p_Config();
                ////this.LoadProduct_PixPoint_Config();
                //this.LoadProduct_MotionOffset_Config();
                //this.LoadProduct_RunParam_Config();
                //this.LoadProduct_DelayTimeSettings_Config();
                this.LoadProduct_SlaveStation_Config();
                //this.LoadProduct_VisionComboCommand_Config();
                //this.LoadProduct_OutPutSettings_Config();
                //this.LoadProduct_InPutSettings_Config();
                return true;

            }
            catch
            {
                return false;
            }
        }

        public override bool CreateProductConfig()
        {
            try
            {
                //this.CreateProduct_Dp2p_Config();
                this.CreateProduct_Ppm_Config();
                //this.CreateProduct_PixPoint_Config();

                this.CreateProduct_MotionOffset_Config();

                this.CreateProduct_RunParam_Config();

                this.CreateProduct_DelayTimeSettings_Config();
                this.CreateProduct_SlaveStation_Config();
                this.CreateProduct_VisionComboCommand_Config();
                this.CreateProduct_OutPutSettings_Config();
                this.CreateProduct_InPutSettings_Config();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region 新增复合视觉命令配置
        public bool LoadProduct_VisionComboCommand_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(VisionComboCommand_ConfigBase.ConfigFileName);
                try
                {
                    this._VisionComboCommand_Provider = VisionComboCommand_ConfigBase.Load<VisionComboCommand_Provider>(fullpath);
                }
                catch
                {
                    this._VisionComboCommand_Provider = new VisionComboCommand_Provider();
                    this._VisionComboCommand_Provider.Save(fullpath);
                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] VisionComboCommand 配置参数失败!");
            }

        }
        public bool SaveProduct_VisionComboCommand_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(VisionComboCommand_ConfigBase.ConfigFileName);
                this._VisionComboCommand_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"保存产品[{this._coreInteration.CurrentProductName}] VisionComboCommand 配置参数失败!");

            }

        }
        public bool CreateProduct_VisionComboCommand_Config()
        {
            var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(VisionComboCommand_ConfigBase.ConfigFileName);
            if (string.IsNullOrEmpty(fullpath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置VisionComboCommand文件为空!");
            }
            try
            {
                this._VisionComboCommand_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"新建产品[{this._coreInteration.CreateProductName}] VisionComboCommand 配置参数失败!");
            }

        }
        #endregion


        #region 新增小工站配置
        public bool LoadProduct_SlaveStation_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(SlaveStation_ProviderBase.ConfigFileName);
                try
                {
                    this._SlaveStation_Provider = SlaveStation_ProviderBase.Load<SlaveStation_Provider>(fullpath);
                }
                catch
                {
                    this._SlaveStation_Provider = new SlaveStation_Provider();
                    this._SlaveStation_Provider.Save(fullpath);
                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] SlaveStation 配置参数失败!");
            }

        }
        public bool SaveProduct_SlaveStation_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(SlaveStation_ProviderBase.ConfigFileName);
                this._SlaveStation_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"保存产品[{this._coreInteration.CurrentProductName}] SlaveStation 配置参数失败!");

            }

        }
        public bool CreateProduct_SlaveStation_Config()
        {
            var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(SlaveStation_ProviderBase.ConfigFileName);
            if (string.IsNullOrEmpty(fullpath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置SlaveStation文件为空!");
            }
            try
            {
                this._SlaveStation_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"新建产品[{this._coreInteration.CreateProductName}] SlaveStation 配置参数失败!");
            }

        }
        #endregion

        #region 新增出料方式配置
        public bool LoadProduct_OutPutSettings_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(OutPutSettings_Provider_CT3103.ConfigFileName);
                try
                {
                    this._OutPutSettings_Provider = OutPutSettings_Provider_CT3103.Load<OutPutSettings_Provider_CT3103>(fullpath);
                }
                catch
                {
                    this._OutPutSettings_Provider = new OutPutSettings_Provider_CT3103();
                    this._OutPutSettings_Provider.Save(fullpath);
                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] OutPutSettings 配置参数失败!");
            }

        }
        public bool SaveProduct_OutPutSettings_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(OutPutSettings_Provider_CT3103.ConfigFileName);
                this._OutPutSettings_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"保存产品[{this._coreInteration.CurrentProductName}] OutPutSettings 配置参数失败!");

            }

        }
        public bool CreateProduct_OutPutSettings_Config()
        {
            var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(OutPutSettings_Provider_CT3103.ConfigFileName);
            if (string.IsNullOrEmpty(fullpath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置OutPutSettings文件为空!");
            }
            try
            {
                //this._OutPutSettings_Provider = new OutPutSettings_Provider_CT3103();
                this._OutPutSettings_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"新建产品[{this._coreInteration.CreateProductName}] OutPutSettings 配置参数失败!");
            }
        }
        #endregion


        #region 新增进料方式配置

        public bool SaveProduct_InPutSettings_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(InPutSettings_Provider_CT3103.ConfigFileName);
                this._InPutSettings_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"保存产品[{this._coreInteration.CurrentProductName}] InPutSettings 配置参数失败!");
            }

        }


        public bool LoadProduct_InPutSettings_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(InPutSettings_Provider_CT3103.ConfigFileName);
                try
                {
                    this._InPutSettings_Provider = InPutSettings_Provider_CT3103.Load<InPutSettings_Provider_CT3103>(fullpath);
                }
                catch
                {
                    this._InPutSettings_Provider = new InPutSettings_Provider_CT3103();
                    this._InPutSettings_Provider.Save(fullpath);
                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] InPutSettings 配置参数失败!");
            }
        }

        public bool CreateProduct_InPutSettings_Config()
        {
            var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(InPutSettings_Provider_CT3103.ConfigFileName);
            if (string.IsNullOrEmpty(fullpath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置 InPutSettings 文件为空!");
            }
            try
            {
                //this._InPutSettings_Provider = new InPutSettings_Provider_CT3103();
                this._InPutSettings_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"新建产品[{this._coreInteration.CreateProductName}] InPutSettings 配置参数失败!");
            }
        }

        #endregion










        public bool LoadProduct_Dp2p_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(DeltaPix2Point_ProviderBase.ConfigFileName);
                try
                {
                    this._Dp2p_Provider = DeltaPix2Point_ProviderBase.Load<DeltaPix2Point_Provider_CT3103>(fullpath);
                }
                catch
                {
                    this._Dp2p_Provider = new DeltaPix2Point_Provider_CT3103();
                    this._Dp2p_Provider.Save(fullpath);
                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] Dp2p 配置参数失败!");
            }

        }
        public bool SaveProduct_Dp2p_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(DeltaPix2Point_ProviderBase.ConfigFileName);
                this._Dp2p_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"保存产品[{this._coreInteration.CurrentProductName}] Dp2p 配置参数失败!");

            }

        }
        public bool CreateProduct_Dp2p_Config()
        {
            var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(DeltaPix2Point_ProviderBase.ConfigFileName);
            if (string.IsNullOrEmpty(fullpath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置Dp2p文件为空!");
            }
            try
            {
                this._Dp2p_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"新建产品[{this._coreInteration.CreateProductName}] Dp2p 配置参数失败!");
            }

        }


        public bool SaveProduct_MotionOffset_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(MotionOffset_Provider_CT3103.ConfigFileName);
                this._MotionOffset_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"保存产品[{this._coreInteration.CurrentProductName}] MotionOffset 配置参数失败!");
            }

        }


        public bool LoadProduct_MotionOffset_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(MotionOffset_Provider_CT3103.ConfigFileName);
                try
                {
                    this._MotionOffset_Provider = MotionOffset_Provider_CT3103.Load<MotionOffset_Provider_CT3103>(fullpath);
                }
                catch
                {
                    this._MotionOffset_Provider = new MotionOffset_Provider_CT3103();
                    this._MotionOffset_Provider.Save(fullpath);
                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] MotionOffset 配置参数失败!");
            }
        }

        public bool CreateProduct_MotionOffset_Config()
        {
            var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(MotionOffset_Provider_CT3103.ConfigFileName);
            if (string.IsNullOrEmpty(fullpath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置 MotionOffset 文件为空!");
            }
            try
            {
                this._MotionOffset_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"新建产品[{this._coreInteration.CreateProductName}] MotionOffset 配置参数失败!");
            }
        }




        public bool LoadProduct_PixPoint_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(PixelPoint_ProviderBase.ConfigFileName);
                try
                {
                    this._PixPoint_Provider = PixelPoint_ProviderBase.Load<PixelPoint_Provider_CT3103>(fullpath);
                }
                catch
                {
                    this._PixPoint_Provider = new PixelPoint_Provider_CT3103();
                    this._PixPoint_Provider.Save(fullpath);
                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] PixPoint 配置参数失败!");
            }

        }
        public bool SaveProduct_PixPoint_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(PixelPoint_ProviderBase.ConfigFileName);
                this._PixPoint_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"保存产品[{this._coreInteration.CurrentProductName}] PixPoint 配置参数失败!");
            }
        }
        public bool CreateProduct_PixPoint_Config()
        {
            var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(PixelPoint_ProviderBase.ConfigFileName);
            if (string.IsNullOrEmpty(fullpath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置PixPoint文件为空!");
            }
            try
            {
                this._PixPoint_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"新建产品[{this._coreInteration.CreateProductName}] PixPoint 配置参数失败!");
            }
        }


        public bool LoadProduct_Ppm_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(PPM_ProviderBase.ConfigFileName);
                try
                {
                    this._PPM_Provider = PPM_ProviderBase.Load<PPM_Provider>(fullpath);
                }
                catch
                {
                    this._PPM_Provider = new PPM_Provider();
                    this._PPM_Provider.Save(fullpath);
                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] PPM 配置参数失败!");
            }

        }
        public bool SaveProduct_Ppm_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(PPM_ProviderBase.ConfigFileName);
                this._PPM_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"保存产品[{this._coreInteration.CurrentProductName}] PPM 配置参数失败!");
            }

        }
        public bool CreateProduct_Ppm_Config()
        {
            var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(PPM_ProviderBase.ConfigFileName);
            if (string.IsNullOrEmpty(fullpath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置PPM文件为空!");
            }
            try
            {
                this._PPM_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"新建产品[{this._coreInteration.CreateProductName}] PPM 配置参数失败!");
            }

        }


        public bool LoadProduct_RunParam_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(RunSettings_Provider_CT3103.ConfigFileName);
                try
                {
                    this._RunSettings_Provider = RunSettings_Provider_CT3103.Load<RunSettings_Provider_CT3103>(fullpath);
                }
                catch
                {
                    this._RunSettings_Provider = new RunSettings_Provider_CT3103();
                    this._RunSettings_Provider.Save(fullpath);
                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] 配置参数失败!");
            }

        }
        public bool SaveProduct_RunParam_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(RunSettings_Provider_CT3103.ConfigFileName);
                this._RunSettings_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"保存产品[{this._coreInteration.CurrentProductName}] 配置参数失败!");
            }

        }
        public bool CreateProduct_RunParam_Config()
        {
            var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(RunSettings_Provider_CT3103.ConfigFileName);
            if (string.IsNullOrEmpty(fullpath))
            {
                throw new FileNotFoundException($"{this.Name}  配置文件为空!");
            }
            try
            {
                this._RunSettings_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"新建产品[{this._coreInteration.CreateProductName}]  配置参数失败!");
            }

        }


        public bool LoadProduct_DelayTimeSettings_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(DelayTimeSettings_Provider_CT3103.ConfigFileName);
                try
                {
                    this._DelayTimeSettings_Provider = DelayTimeSettings_Provider_CT3103.Load<DelayTimeSettings_Provider_CT3103>(fullpath);
                }
                catch
                {
                    this._DelayTimeSettings_Provider = new DelayTimeSettings_Provider_CT3103();
                    this._DelayTimeSettings_Provider.Save(fullpath);
                }
                return true;
            }
            catch
            {
                throw new Exception($"加载产品[{this._coreInteration.CurrentProductName}] DelayTimeSettings 配置参数失败!");
            }

        }
        public bool SaveProduct_DelayTimeSettings_Config()
        {
            try
            {
                var fullpath = this._coreInteration.GetProductConfigFileFullPath(DelayTimeSettings_Provider_CT3103.ConfigFileName);
                this._DelayTimeSettings_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"保存产品[{this._coreInteration.CurrentProductName}] DelayTimeSettings 配置参数失败!");
            }

        }
        public bool CreateProduct_DelayTimeSettings_Config()
        {
            var fullpath = this._coreInteration.Get_Create_ProductConfigFileFullPath(DelayTimeSettings_Provider_CT3103.ConfigFileName);
            if (string.IsNullOrEmpty(fullpath))
            {
                throw new FileNotFoundException($"{this.Name} 子配置DelayTimeSettings文件为空!");
            }
            try
            {
                this._DelayTimeSettings_Provider.Save(fullpath);
                return true;
            }
            catch
            {
                throw new Exception($"新建产品[{this._coreInteration.CreateProductName}] DelayTimeSettings 配置参数失败!");
            }

        }









        public override bool CanCurrentOwnerAccessResource(GenernalResourceOwner currnetResourceOwner, string resourceOwnerName)
        {
            return true;
        }

        public override void CreateMainUI()
        {

        }

    }
}
