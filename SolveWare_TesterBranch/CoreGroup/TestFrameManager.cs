using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Attributes;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SolveWare_TesterCore
{
    public class TestFrameManager : TesterAppPluginUIModel
    {
        Dictionary<string, List<string>> _supported_TMDL_vs_CALC_Dict = new Dictionary<string, List<string>>();
        Dictionary<string, string> _localExecutorFilesDict = new Dictionary<string, string>();
        Dictionary<string, string> _localExecutorComboFiles = new Dictionary<string, string>();
        Dictionary<string, string> _localExecutorTestRecipeFiles = new Dictionary<string, string>();
        Dictionary<string, string> _localExecutorCalcRecipeFiles = new Dictionary<string, string>();
        Dictionary<string, string> _localTestProfileFiles = new Dictionary<string, string>();
        static TestFrameManager _instance;
        static object _mutex = new object();
        public static TestFrameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new TestFrameManager();
                        }
                    }
                }
                return _instance;
            }
        }
        public TestFrameManager()
        {

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
        public override void ReinstallController()
        {
            try
            {
                (this._MainPageUI as ITesterAppUI).RefreshOnce();
            }
            catch(Exception ex)
            {

            }
          
        }
        public override bool CreateProductConfig()
        {
            try
            {
                this.CopyLocalExecutorFiles();
                //this.CopyLocalCalcRecipeFileDict();
                this.CopyLocalTestProfileFiles();
                //this.CopyLocalTestRecipeFileDict();
                this.CopyLocalExecutorComboFiles();
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        public override bool SwitchProductConfig()
        {
            try
            {
                (this._MainPageUI as ITesterAppUI).RefreshOnce();
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        public override void RefreshOnceUI()
        {
          
        }
        public Dictionary<string, List<string>> GetSupportedTestModuleClass()
        {
            _supported_TMDL_vs_CALC_Dict.Clear();
            var _supMdlCls = this._coreInteration.GetAssignableTypesFromPreLoadDlls(typeof(TestModuleBase));
            foreach (var mdlc in _supMdlCls)
            {
                var calculators = this.GetTestModuleSupportedCalculatorTypes(mdlc);
                if (calculators == null)
                {
                    _supported_TMDL_vs_CALC_Dict.Add(mdlc.Name, new List<string>());
                }
                else
                {
                    _supported_TMDL_vs_CALC_Dict.Add(mdlc.Name, calculators);
                }

            }
            return _supported_TMDL_vs_CALC_Dict;
        }
        public List<string> GetTestModuleSupportedCalculatorTypes(Type testModuleType)
        {
            var calccls = PropHelper.GetAttributeValue<SupportedCalculatorAttribute>(testModuleType);
            if (calccls == null)
            {
                return new List<string>();
            }
            else
            {
                return calccls.SupportedCalculatorCollection;
            }
        }
        public virtual List<ConfigurableInstrumentAttribute> GetTestModuleRequireInstrumentLables(Type testModuleType)
        {
            return PropHelper.GetAttributeValueCollection<ConfigurableInstrumentAttribute>(testModuleType);
        }
        public virtual List<ConfigurableAxisAttribute> GetTestModuleRequireAxisLables(Type testModuleType)
        {
            return PropHelper.GetAttributeValueCollection<ConfigurableAxisAttribute>(testModuleType);
        }
        public virtual List<ConfigurablePositionAttribute> GetTestModuleRequirePositionLables(Type testModuleType)
        {
            return PropHelper.GetAttributeValueCollection<ConfigurablePositionAttribute>(testModuleType);
        }
        public virtual List<StaticResourceAttribute> GetTestModuleRequireStaticResourceLables(Type testModuleType)
        {
            return PropHelper.GetAttributeValueCollection<StaticResourceAttribute>(testModuleType);
        }
        public List<string> GetAxesNameCollection()
        {
            List<string> temp = new List<string>();

            var appPlugins = this.CoreResource.GetAppPlugins();
            foreach (var ap in appPlugins)
            {
                if (ap is IAxisResourceProvider)
                {
                    temp.AddRange((ap as IAxisResourceProvider).GetAxesNameCollection());
                }
            }
            return temp;
        }
        public List<string> GetPositionNameCollection()
        {
            List<string> temp = new List<string>();

            var appPlugins = this.CoreResource.GetAppPlugins();
            foreach (var ap in appPlugins)
            {
                if (ap is IAxesPositionResourceProvider)
                {
                    temp.AddRange((ap as IAxesPositionResourceProvider).GetPositionNameCollection());
                }
            }
            return temp;
        }
        //需要切换为产品适配
        public Dictionary<string, string> GetLocalExecutorFiles()
        {
            var prodDir = this._coreInteration.GetProductConfigFileDirectory();
            var specialPath = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_EXECUTOR_FILES_PATH);
            var path = $@"{prodDir}\{specialPath}";
            //var path = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_EXECUTOR_FILES_PATH);
            if (Directory.Exists(path) == false)
            {
                var msg = $"本地测试执行项路径[{path}]不存在!";
                MessageBox.Show(msg);
                this.Log_Global(msg);
                return _localExecutorFilesDict;
            }

            _localExecutorFilesDict.Clear();

            var files = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var fileShortName = Path.GetFileNameWithoutExtension(file);
                if (_localExecutorFilesDict.ContainsKey(fileShortName))
                {
                    var msg = $"本地测试执行项路径有重名文件[{file}]!";
                    MessageBox.Show(msg);
                    this.Log_Global(msg);

                    break;
                }
                else
                {
                    _localExecutorFilesDict.Add(fileShortName, file);
                }
            }
            return _localExecutorFilesDict;
        }       
        //需要切换为产品适配
        public void CopyLocalExecutorFiles()
        {
            var prodDir = this._coreInteration.GetProductConfigFileDirectory();
            var specialPath = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_EXECUTOR_FILES_PATH);
            var path = $@"{prodDir}\{specialPath}";
            var prodDir_dest = this._coreInteration.Get_Create_ProductConfigFileDirectory();
            var path_dest = $@"{prodDir_dest}\{specialPath}";
            this._coreInteration.CopyDirectory(path, path_dest);
        }

        //需要切换为产品适配
        public Dictionary<string, string> GetLocalExecutorComboFiles()
        {
            var prodDir = this._coreInteration.GetProductConfigFileDirectory();
            var specialPath = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_EXECUTIVE_COMBO_PATH);
            var path = $@"{prodDir}\{specialPath}";
           // var path = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_EXECUTIVE_COMBO_PATH);
            if (Directory.Exists(path) == false)
            {
                var msg = $"本地测试链表路径[{path}]不存在!";
                MessageBox.Show(msg);
                this.Log_Global(msg);
                return _localExecutorComboFiles;
            }

            _localExecutorComboFiles.Clear();

            var files = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var fileShortName = Path.GetFileNameWithoutExtension(file);
                if (_localExecutorComboFiles.ContainsKey(fileShortName))
                {
                    var msg = $"本地测试链表路径有重名文件[{file}]!";
                    MessageBox.Show(msg);
                    this.Log_Global(msg);
                    //_localExecutorComboFiles.Clear();
                    break;
                }
                else
                {
                    _localExecutorComboFiles.Add(fileShortName, file);
                }
            }
            return _localExecutorComboFiles;
        }        
        //需要切换为产品适配
        public void CopyLocalExecutorComboFiles()
        {
            var prodDir = this._coreInteration.GetProductConfigFileDirectory();
            var specialPath = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_EXECUTIVE_COMBO_PATH);
            var path = $@"{prodDir}\{specialPath}";
            var prodDir_dest = this._coreInteration.Get_Create_ProductConfigFileDirectory();
            var path_dest = $@"{prodDir_dest}\{specialPath}";
            this._coreInteration.CopyDirectory(path, path_dest);
        }
        //需要切换为产品适配
        public Dictionary<string, string> GetLocalTestProfileFiles()
        {
            var prodDir = this._coreInteration.GetProductConfigFileDirectory();
            var specialPath = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_PROFILE_PATH);
            var path = $@"{prodDir}\{specialPath}";
            //var path = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_PROFILE_PATH);
            if (Directory.Exists(path) == false)
            {
                var msg = $"本地测试方案存储路径[{path}]不存在!";
                MessageBox.Show(msg);
                this.Log_Global(msg);
                return _localTestProfileFiles;
            }

            _localTestProfileFiles.Clear();

            var files = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var fileShortName = Path.GetFileNameWithoutExtension(file);
                if (_localTestProfileFiles.ContainsKey(fileShortName))
                {
                    var msg = $"本地测试方案存储路径有重名文件[{file}]!";
                    MessageBox.Show(msg);
                    this.Log_Global(msg);
                    //_localExecutorComboFiles.Clear();
                    break;
                }
                else
                {
                    _localTestProfileFiles.Add(fileShortName, file);
                }
            }
            return _localTestProfileFiles;
        }
        //需要切换为产品适配
        public void CopyLocalTestProfileFiles()
        {
            var prodDir = this._coreInteration.GetProductConfigFileDirectory();
            var specialPath = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_PROFILE_PATH);
            var path = $@"{prodDir}\{specialPath}";
            var prodDir_dest = this._coreInteration.Get_Create_ProductConfigFileDirectory();
            var path_dest = $@"{prodDir_dest}\{specialPath}";
            this._coreInteration.CopyDirectory(path, path_dest);
        }
        public object LoadTestProfile<TProfile>(string configXmlFile) 
        {
            TProfile confObj = default(TProfile);
            try
            {
                confObj = XmlHelper.DeserializeFile<TProfile> (configXmlFile );
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.LoadTestProfileFailed, ex);
            }
            return confObj;
        }
        public object LoadTestProfileWithExtraTypes<TProfile>(string configXmlFile)
        {
            TProfile confObj = default(TProfile);
            try
            {
                var temp = XElement.Load(configXmlFile);
                var typesEle = temp.GetElement("IncludingTypes");
                List<string> typeNames = new List<string>();
                foreach (var node in typesEle.Nodes())
                {
                    typeNames.Add(((XElement)node).Value);
                }
                
                var types = _coreInteration.GetTypeFromClassInPreLoadDlls(typeNames);

                confObj = (TProfile)XmlHelper.DeserializeFile(configXmlFile, typeof(TProfile), types.ToArray()) ;

                //confObj = XmlHelper.DeserializeFile<TProfile>(configXmlFile);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.LoadTestProfileFailed, ex);
            }
            return confObj;
        }
        public TestExecutorCombo LoadTestExecutorCombo(string configXmlFile)
        {
            TestExecutorCombo confObj = null;
            try
            {
                confObj = XmlHelper.DeserializeFile<TestExecutorCombo>(configXmlFile);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.LoadTestExecutorComboFailed, ex);
            }
            return confObj;
        }
        public TestExecutorComboWithParams LoadTestExecutorComboWithParams(string configXmlFile)
        {
            TestExecutorComboWithParams confObj = null;
            try
            {
                confObj = TestExecutorComboWithParams.Load(this._coreInteration,  configXmlFile);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.LoadTestExecutorComboFailed, ex);
            }
            return confObj;
        }

        public ExecutorConfigItem LoadTestExecutorConfigItem(string configXmlFile)
        {
            ExecutorConfigItem confObj = null;
            try
            {
                confObj = XmlHelper.DeserializeFile<ExecutorConfigItem>(configXmlFile);
            }
            catch (Exception ex)
            {
                this.ReportException(ex.Message, ErrorCodes.LoadTestExecutorConfigItemFailed, ex);
            }
            return confObj;
        }

        public ExecutorConfigItem CreateTestExecutorConfigItem(string executorName)
        {
            ExecutorConfigItem confObj = new ExecutorConfigItem();
            confObj.TestExecutorName = executorName;
            return confObj;
        }
        public TestExecutorCombo CreateTestExecutorCombo(string comboName)
        {
            TestExecutorCombo confObj = new TestExecutorCombo();
            confObj.Name = comboName;
            return confObj;
        }
        public TestExecutorComboWithParams CreateTestExecutorComboWithParams(string comboName)
        {
            TestExecutorComboWithParams confObj = new TestExecutorComboWithParams();
            confObj.Name = comboName;
            return confObj;
        }
        public void UpdateTestModuleClassOfExecutorConfigItem(ExecutorConfigItem sourceItem, string newTestModuleName)
        {
            sourceItem.TestModuleClassName = newTestModuleName;
            sourceItem.CalculatorCollection.Clear();
        }

        public bool TryAddCalculatorToExecutorConfigItem(ExecutorConfigItem configItem, string newCalculatorName)
        {
            try
            {
                var mdlType = this._coreInteration.GetTypeFromClassInPreLoadDlls(configItem.TestModuleClassName);
                List<string> supportedCalculatorTypeNames = GetTestModuleSupportedCalculatorTypes(mdlType);
                if (supportedCalculatorTypeNames.Contains(newCalculatorName))
                {
                    configItem.CalculatorCollection.Add(newCalculatorName, string.Empty);
                }
                else
                {
                    var msg = $"该测试模块[{configItem.TestModuleClassName}]不支持算子[{newCalculatorName }]!";
                    this.Log_Global(msg);
                    throw new Exception(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public ExecutorConfigItem DemoTestExecutorConfigItem()
        {
            ExecutorConfigItem confObj = new ExecutorConfigItem();
            confObj.TestExecutorName = "TestExecutorName 例子";
            confObj.TestModuleClassName = "TestModuleClassName 例子";

            for (int i = 0; i < 10; i++)
            {
                confObj.CalculatorCollection.Add($"CalculatorName 例子[{i}]", $"CalcRecipeName 例子[{i}]");
            }

            return confObj;
        }



        public override void StartUp()
        {
            this.CreateMainUI();
        }

        public override void CreateMainUI()
        {
            try
            {
                if (this._MainPageUI == null || this._MainPageUI.IsDisposed)
                {
                    this._MainPageUI = new Form_TestFrameBoard();
                    this._coreInteration.LinkToCore(this._MainPageUI as ITesterAppUI);
                    (this._MainPageUI as ITesterAppUI).ConnectToAppInteration(this);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{this.Name}]窗口还原错误:{ex.Message}-{ex.StackTrace}!");
            }
        }
        public override void DockUI()
        {
            try
            {
                if (this._MainPageUI == null) { return; }
                this._coreInteration.DockingTestFrameBoard();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[{this.Name}]窗口还原错误:{ex.Message}-{ex.StackTrace}!");
            }
        }
      
        //需要切换为产品适配
        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="typeList"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetLocalCalcRecipeFileDict()
        {
            var prodDir = this._coreInteration.GetProductConfigFileDirectory();
            var specialPath = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CALC_RECIPE_PATH);
            var path = $@"{prodDir}\{specialPath}";
            if (Directory.Exists(path) == false)
            {
                var msg = $"获取计算条件文件失败:本地路径[{path}]不存在!";
                MessageBox.Show(msg);
                this.Log_Global(msg);
                return _localExecutorCalcRecipeFiles;
            }

            _localExecutorCalcRecipeFiles.Clear();

            var files = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var fileShortName = Path.GetFileNameWithoutExtension(file);
                if (_localExecutorCalcRecipeFiles.ContainsKey(fileShortName))
                {
                    var msg = $"获取计算条件文件失败:本地路径有重名文件[{file}]!";
                    MessageBox.Show(msg);
                    this.Log_Global(msg);

                    break;
                }
                else
                {
                    _localExecutorCalcRecipeFiles.Add(fileShortName, file);
                }
            }
            return _localExecutorCalcRecipeFiles;
        }    
        //需要切换为产品适配
        public void CopyLocalCalcRecipeFileDict()
        {
            var prodDir = this._coreInteration.GetProductConfigFileDirectory();
            var specialPath = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CALC_RECIPE_PATH);
            var path = $@"{prodDir}\{specialPath}";
            var prodDir_dest = this._coreInteration.Get_Create_ProductConfigFileDirectory();
            var path_dest = $@"{prodDir_dest}\{specialPath}";
            this._coreInteration.CopyDirectory(path, path_dest);
        }
        //需要切换为产品适配
        public Dictionary<string, string> GetLocalTestRecipeFileDict()
        {
            var prodDir = this._coreInteration.GetProductConfigFileDirectory();
            var specialPath = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_RECIPE_PATH);
            var path = $@"{prodDir}\{specialPath}";
            if (Directory.Exists(path) == false)
            {
                var msg = $"获取测试条件文件失败:本地路径[{path}]不存在!";
                MessageBox.Show(msg);
                this.Log_Global(msg);
                return _localExecutorTestRecipeFiles;
            }

            _localExecutorTestRecipeFiles.Clear();

            var files = Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var fileShortName = Path.GetFileNameWithoutExtension(file);
                if (_localExecutorTestRecipeFiles.ContainsKey(fileShortName))
                {
                    var msg = $"获取测试条件文件失败:本地路径有重名文件[{file}]!";
                    MessageBox.Show(msg);
                    this.Log_Global(msg);

                    break;
                }
                else
                {
                    _localExecutorTestRecipeFiles.Add(fileShortName, file);
                }
            }
            return _localExecutorTestRecipeFiles;
        }  
        //需要切换为产品适配
        public void CopyLocalTestRecipeFileDict()
        {
            var prodDir = this._coreInteration.GetProductConfigFileDirectory();
            var specialPath = this._coreInteration.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_RECIPE_PATH);
            var path = $@"{prodDir}\{specialPath}";
            var prodDir_dest = this._coreInteration.Get_Create_ProductConfigFileDirectory();
            var path_dest = $@"{prodDir_dest}\{specialPath}";
            this._coreInteration.CopyDirectory(path, path_dest);
        }
        /// <summary>
        /// 特定DGV展示数据
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="sourceObject">数据源</param>
        /// <param name="infoColIndex">说明</param>
        /// <param name="keyColIndex">属性名称</param>
        /// <param name="valColIndex">属性值</param>
        public void Updatedgv(DataGridView dgv, object sourceObject, int infoColIndex, int keyColIndex, int valColIndex)
        {
            try
            {
                dgv.Rows.Clear();
                UIGeneric.FillListDGV_InfoKeyValue(dgv, sourceObject, infoColIndex, keyColIndex, valColIndex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        internal TreeNode Convert_TestExecutorComboToTreeNode(TestExecutorCombo comboConfig)
        {
            var rootNode = new TreeNode();
            rootNode.Name = comboConfig.Name;
            rootNode.Text = comboConfig.Name;
            var comboInfoNode = new TreeNode();
            comboInfoNode.Name = TestExecutorCombo.INFO_NODE_NAME;
            comboInfoNode.Text = TestExecutorCombo.INFO_NODE_NAME;

            var prodTypeNode = new TreeNode();
            prodTypeNode.Name = comboConfig.ProductType;
            prodTypeNode.Text = $"可支持产品类型[{comboConfig.ProductType}]";

            var applicableTestPluginNode = new TreeNode();
            //applicableTestPluginNode.Name = comboConfig.ApplicableTestPlugin;
            //applicableTestPluginNode.Text = $"可支持测试组件类型[{ comboConfig.ApplicableTestPlugin}]";

            comboInfoNode.Nodes.Add(prodTypeNode);
            comboInfoNode.Nodes.Add(applicableTestPluginNode);

            rootNode.Nodes.Add(comboInfoNode);

            #region 前置
            var pre_comboExecutorsRootNode = new TreeNode();
            pre_comboExecutorsRootNode.Name = TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME;
            pre_comboExecutorsRootNode.Text = TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME;

            foreach (var eci in comboConfig.Pre_ExecutorConfigCollection)
            {
                var eciNode = Convert_ExecutorConfigItemToTreeNode(eci);
                pre_comboExecutorsRootNode.Nodes.Add(eciNode);
            }

            rootNode.Nodes.Add(pre_comboExecutorsRootNode);
            #endregion
            #region 主要

            var comboExecutorsRootNode = new TreeNode();
            comboExecutorsRootNode.Name = TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME;
            comboExecutorsRootNode.Text = TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME;

            foreach (var eci in comboConfig.Main_ExecutorConfigCollection)
            {
                var eciNode = Convert_ExecutorConfigItemToTreeNode(eci);
                comboExecutorsRootNode.Nodes.Add(eciNode);
            }
            rootNode.Nodes.Add(comboExecutorsRootNode);

            #endregion
            #region 后置

            var post_comboExecutorsRootNode = new TreeNode();
            post_comboExecutorsRootNode.Name = TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME;
            post_comboExecutorsRootNode.Text = TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME;

            foreach (var eci in comboConfig.Post_ExecutorConfigCollection)
            {
                var eciNode = Convert_ExecutorConfigItemToTreeNode(eci);
                post_comboExecutorsRootNode.Nodes.Add(eciNode);
            }

            rootNode.Nodes.Add(post_comboExecutorsRootNode);
            #endregion
 
            return rootNode;
        }
        internal TreeNode Convert_ExecutorConfigItemToTreeNode(ExecutorConfigItem exeConfig)
        {
            TreeNode rootNode = new TreeNode();

            rootNode.Name = exeConfig.TestExecutorName;
            rootNode.Text = exeConfig.TestExecutorName;

            var testModuleNode = new TreeNode();
            testModuleNode.Name = exeConfig.TestModuleClassName;
            testModuleNode.Text = $"{exeConfig.TestModuleClassName} @ [{exeConfig.TestRecipeFileName}]";

            foreach (var calcKvp in exeConfig.CalculatorCollection)
            {
                var calcNode = new TreeNode();
                calcNode.Name = calcKvp.Key;
                calcNode.Text = $"{calcKvp.Key} @ [{calcKvp.Value}]";
                testModuleNode.Nodes.Add(calcNode);
            }
            rootNode.Nodes.Add(testModuleNode);

            return rootNode;
        }
        internal TreeNode Convert_TestExecutorComboToTreeNode_WithInstrumentConfig(TestExecutorCombo comboConfig)
        {
            var rootNode = new TreeNode();
            rootNode.Name = comboConfig.Name;
            rootNode.Text = comboConfig.Name;
            var comboInfoNode = new TreeNode();
            comboInfoNode.Name = TestExecutorCombo.INFO_NODE_NAME;
            comboInfoNode.Text = TestExecutorCombo.INFO_NODE_NAME;

            var prodTypeNode = new TreeNode();
            prodTypeNode.Name = comboConfig.ProductType;
            prodTypeNode.Text = $"可支持产品类型[{comboConfig.ProductType}]";

            var applicableTestPluginNode = new TreeNode();
            //applicableTestPluginNode.Name = comboConfig.ApplicableTestPlugin;
            //applicableTestPluginNode.Text = $"可支持测试组件类型[{ comboConfig.ApplicableTestPlugin}]";

            comboInfoNode.Nodes.Add(prodTypeNode);
            comboInfoNode.Nodes.Add(applicableTestPluginNode);

            rootNode.Nodes.Add(comboInfoNode);

            #region 前置
            var pre_comboExecutorsRootNode = new TreeNode();
            pre_comboExecutorsRootNode.Name = TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME;
            pre_comboExecutorsRootNode.Text = TestExecutorCombo.PRE_COMBO_EXECUTOR_ROOT_NODE_NAME;

            foreach (var eci in comboConfig.Pre_ExecutorConfigCollection)
            {
                var eciNode = this.Convert_ExecutorConfigItemToTreeNode_WithInstrumentConfig(eci);
                pre_comboExecutorsRootNode.Nodes.Add(eciNode);
            }

            rootNode.Nodes.Add(pre_comboExecutorsRootNode);
            #endregion
            #region 主要

            var comboExecutorsRootNode = new TreeNode();
            comboExecutorsRootNode.Name = TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME;
            comboExecutorsRootNode.Text = TestExecutorCombo.MAIN_COMBO_EXECUTOR_ROOT_NODE_NAME;

            foreach (var eci in comboConfig.Main_ExecutorConfigCollection)
            {
                var eciNode = Convert_ExecutorConfigItemToTreeNode_WithInstrumentConfig(eci);
                comboExecutorsRootNode.Nodes.Add(eciNode);
            }
            rootNode.Nodes.Add(comboExecutorsRootNode);

            #endregion
            #region 后置

            var post_comboExecutorsRootNode = new TreeNode();
            post_comboExecutorsRootNode.Name = TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME;
            post_comboExecutorsRootNode.Text = TestExecutorCombo.POST_COMBO_EXECUTOR_ROOT_NODE_NAME;

            foreach (var eci in comboConfig.Post_ExecutorConfigCollection)
            {
                var eciNode = Convert_ExecutorConfigItemToTreeNode_WithInstrumentConfig(eci);
                post_comboExecutorsRootNode.Nodes.Add(eciNode);
            }

            rootNode.Nodes.Add(post_comboExecutorsRootNode);
            #endregion

            return rootNode;
        }
        internal TreeNode Convert_ExecutorConfigItemToTreeNode_WithInstrumentConfig(ExecutorConfigItem exeConfig)
        {
            TreeNode rootNode = new TreeNode();
            try
            {
                rootNode.Name = exeConfig.TestExecutorName;
                rootNode.Text = exeConfig.TestExecutorName;
                rootNode.BackColor = Color.FromKnownColor(KnownColor.LightGreen);

                var moduleType = this._coreInteration.GetTypeFromClassInPreLoadDlls(exeConfig.TestModuleClassName);

                var staticResourceNode = new TreeNode();
                staticResourceNode.Name = ExecutorConfigItem.REQ_STATIC_RESOURCE_LIST_NODE_NAME;
                staticResourceNode.Text = ExecutorConfigItem.REQ_STATIC_RESOURCE_LIST_NODE_NAME;
                staticResourceNode.BackColor = Color.FromKnownColor(KnownColor.LightGray);
                var mdlStaResLbls = this.GetTestModuleRequireStaticResourceLables(moduleType);
                foreach (var reqStaRes in mdlStaResLbls)
                {
                    var reqStaResSubNode = new TreeNode();
                    reqStaResSubNode.Text = $"[{reqStaRes.ResourceType}]-[{reqStaRes.ResourceName}]-[{reqStaRes.UsageDescription}]";
                    reqStaResSubNode.Name = $"[{reqStaRes.ResourceType}]-[{reqStaRes.ResourceName}]-[{reqStaRes.UsageDescription}]";
                    staticResourceNode.Nodes.Add(reqStaResSubNode);
                }
                rootNode.Nodes.Add(staticResourceNode);

      

                #region instrument
                var reqInstrsNode = new TreeNode();
                reqInstrsNode.Name = ExecutorConfigItem.REQ_INSTR_LIST_NODE_NAME;
                reqInstrsNode.Text = ExecutorConfigItem.REQ_INSTR_LIST_NODE_NAME;
                reqInstrsNode.BackColor = Color.FromKnownColor(KnownColor.Yellow);

                var mdlReqInstrLbls = this.GetTestModuleRequireInstrumentLables(moduleType);

                foreach (var reqInstr in mdlReqInstrLbls)
                {
                    var reqInstrSubNode = new TreeNode();
                    reqInstrSubNode.Text = $"[{reqInstr.ModuleDefineInstrName}]-[{reqInstr.InstrType}]-[{reqInstr.UsageDescription}]";
                    reqInstrSubNode.Name = $"{reqInstr.ModuleDefineInstrName}$${reqInstr.InstrType}";
                    reqInstrsNode.Nodes.Add(reqInstrSubNode);
                }
                rootNode.Nodes.Add(reqInstrsNode);

                var userInstrsNode = new TreeNode();
                userInstrsNode.Name = ExecutorConfigItem.USER_INSTR_LIST_NODE_NAME;
                userInstrsNode.Text = ExecutorConfigItem.USER_INSTR_LIST_NODE_NAME;
                userInstrsNode.BackColor = Color.FromKnownColor(KnownColor.LightSkyBlue);

                foreach (var userDefInstr in exeConfig.UserDefineInstrumentConfig)
                {
                    var moduleDefineInstrName = userDefInstr.Key;
                    var userConfigInstrName = userDefInstr.Value;
                    var calcNode = new TreeNode();
                    calcNode.Name = $"[{moduleDefineInstrName}] @ [{userConfigInstrName}]";
                    calcNode.Text = $"[{moduleDefineInstrName}] @ [{userConfigInstrName}]";
                    userInstrsNode.Nodes.Add(calcNode);
                }
                rootNode.Nodes.Add(userInstrsNode);

                #endregion

                #region Axis
                var reqAxisNode = new TreeNode();
                reqAxisNode.Name = ExecutorConfigItem.REQ_AXIS_LIST_NODE_NAME;
                reqAxisNode.Text = ExecutorConfigItem.REQ_AXIS_LIST_NODE_NAME;
                reqAxisNode.BackColor = Color.FromKnownColor(KnownColor.Yellow);

                var mdlReqAxisLbls = this.GetTestModuleRequireAxisLables(moduleType);

                foreach (var reqAxis in mdlReqAxisLbls)
                {
                    var reqAxisSubNode = new TreeNode();
                    reqAxisSubNode.Text = $"[{reqAxis.ModuleDefineAxisName}]-[{reqAxis.UsageDescription}]";
                    reqAxisSubNode.Name = $"{reqAxis.ModuleDefineAxisName}$$";
                    reqAxisNode.Nodes.Add(reqAxisSubNode);
                }
                rootNode.Nodes.Add(reqAxisNode);

                var userAxisNode = new TreeNode();
                userAxisNode.Name = ExecutorConfigItem.USER_AXIS_LIST_NODE_NAME;
                userAxisNode.Text = ExecutorConfigItem.USER_AXIS_LIST_NODE_NAME;
                userAxisNode.BackColor = Color.FromKnownColor(KnownColor.LightSkyBlue);

                foreach (var userDefAxis in exeConfig.UserDefineAxisConfig)
                {
                    var moduleDefineAxisName = userDefAxis.Key;
                    var userConfigAxisName = userDefAxis.Value;
                    var calcNode = new TreeNode();
                    calcNode.Name = $"[{moduleDefineAxisName}] @ [{userConfigAxisName}]";
                    calcNode.Text = $"[{moduleDefineAxisName}] @ [{userConfigAxisName}]";
                    userAxisNode.Nodes.Add(calcNode);
                }
                rootNode.Nodes.Add(userAxisNode);

                #endregion


                #region Position
                var reqPositionNode = new TreeNode();
                reqPositionNode.Name = ExecutorConfigItem.REQ_POS_LIST_NODE_NAME;
                reqPositionNode.Text = ExecutorConfigItem.REQ_POS_LIST_NODE_NAME;
                reqPositionNode.BackColor = Color.FromKnownColor(KnownColor.Yellow);

                var mdlReqPositionLbls = this.GetTestModuleRequirePositionLables(moduleType);

                foreach (var reqPosition in mdlReqPositionLbls)
                {
                    var reqPositionSubNode = new TreeNode();
                    reqPositionSubNode.Text = $"[{reqPosition.ModuleDefinePositionName}]-[{reqPosition.UsageDescription}]";
                    reqPositionSubNode.Name = $"{reqPosition.ModuleDefinePositionName}$$";
                    reqPositionNode.Nodes.Add(reqPositionSubNode);
                }
                rootNode.Nodes.Add(reqPositionNode);

                var userPositionNode = new TreeNode();
                userPositionNode.Name = ExecutorConfigItem.USER_POS_LIST_NODE_NAME;
                userPositionNode.Text = ExecutorConfigItem.USER_POS_LIST_NODE_NAME;
                userPositionNode.BackColor = Color.FromKnownColor(KnownColor.LightSkyBlue);

                foreach (var userDefPosition in exeConfig.UserDefinePositionConfig)
                {
                    var moduleDefinePositionName = userDefPosition.Key;
                    var userConfigPositionName = userDefPosition.Value;
                    var calcNode = new TreeNode();
                    calcNode.Name = $"[{moduleDefinePositionName}] @ [{userConfigPositionName}]";
                    calcNode.Text = $"[{moduleDefinePositionName}] @ [{userConfigPositionName}]";
                    userPositionNode.Nodes.Add(calcNode);
                }
                rootNode.Nodes.Add(userPositionNode);

                #endregion
  
                var testModuleNode = new TreeNode();
                testModuleNode.Name = exeConfig.TestModuleClassName;
                testModuleNode.Text = $"{exeConfig.TestModuleClassName} @ [{exeConfig.TestRecipeFileName}]";

                foreach (var calcKvp in exeConfig.CalculatorCollection)
                {
                    var calcNode = new TreeNode();
                    calcNode.Name = calcKvp.Key;
                    calcNode.Text = $"{calcKvp.Key} @ [{calcKvp.Value}]";
                    testModuleNode.Nodes.Add(calcNode);
                }
                rootNode.Nodes.Add(testModuleNode);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"解析测试项节点错误:[{ex.Message}-{ex.StackTrace}]!");
            }

            return rootNode;
        }
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="filePath">返回文件路径</param>
        /// <param name="fileExtension">文件扩展名</param>
        /// <returns></returns>
        public bool IsFileExist(string directoryPath, string fileName, out string filePath, string fileExtension = ".xml")
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    throw new Exception("查询文件名为空！！！");
                }

                var rs = IsDirectExist(directoryPath);

                if (!rs)
                {
                    throw new Exception("当前查询目录不存在！！！");
                }
                string filepath = directoryPath + "\\" + fileName + fileExtension;
                var path = Path.GetFullPath(filepath);
                filePath = path;
                var rs1 = File.Exists(path);

                if (rs1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <returns></returns>
        public bool IsDirectExist(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                throw new Exception("查询目录路径为空！！！");
            }
            var result = Directory.Exists(directoryPath);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 从特定DGV获取数据
        /// 将dgv中数据装进集合中
        /// 获取数据同时判断数据类型
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="keyColIndex">dgv中的属性名称列，字典集合的键</param>
        /// <param name="valColIndex">dgv中的属性值列，字典集合的值</param>
        /// <returns>返回DGV中的数据</returns>

        public Dictionary<string, object> GetDataFromPdgv(DataGridView dgv, int keyColIndex, int valColIndex)
        {
            try
            {
                return UIGeneric.Grab_DGV_KeyValueDict(dgv, keyColIndex, valColIndex);
            }
            catch (Exception ex)
            {
                throw new Exception($"当前界面获取数据异常，异常原因：[{ex.Message}]");
            }
        }

        /// <summary>
        /// 比较数据
        /// 通过集合和对象进行比较
        /// </summary>
        /// <param name="sourceObject">数据源</param>
        /// <param name="dict">装有最新数据的集合</param>
        /// <returns></returns>
        public bool CompareDgvValuesAndRecipeValues(object sourceObject, Dictionary<string, object> dict)
        {
            try
            {
                return ReflectionTool.ComparePropertyValues(sourceObject, dict);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="sourceObject">数据</param>
        /// <param name="dict">数据池</param>
        public void UpdateLastRecipe(object sourceObject, Dictionary<string, object> dict)
        {
            try
            {
                ReflectionTool.SetPropertyValues(sourceObject, dict);
            }
            catch (Exception ex)
            {
                throw new Exception("更新Recipe数据异常，异常原因：" + ex.Message);
            }
        }

        public bool SaveRecipe(object sourceObject, string directoryPath, string fileName)
        {
            try
            {
                string filepath = directoryPath + "\\" + fileName + ".xml";
                XmlHelper.SerializeFile(filepath, sourceObject);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("保存失败！异常原因：" + ex.Message);
            }
        }
        /// <summary>
        ///数据另存为
        /// </summary>
        /// <param name="sourceObject">数据</param>
        /// <param name="directoryPath">初始目录</param>
        /// <returns></returns>
        public bool SaveAsRecipe(object sourceObject, string directoryPath)
        {

            SaveFileDialog sfd = new SaveFileDialog();
            //标题
            sfd.Title = "文件另存为";
            //初始目录
            sfd.InitialDirectory = Path.GetFullPath(directoryPath);
            //文件筛选
            sfd.Filter = "XML文件(*.xml)|*.xml";
            //默认后缀
            sfd.DefaultExt = ".xml";
            //无后缀时，自动添加默认后缀
            sfd.AddExtension = true;

            //打开最新目录
            sfd.RestoreDirectory = true;

            try
            {
                var dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {//确定，执行返回true
                    var filePath = "";
                    if (sfd.FileName.ToLower().EndsWith(".xml"))
                    {
                        filePath = sfd.FileName;
                    }
                    else
                    {
                        filePath = sfd.FileName + ".xml";
                    }
                    XmlHelper.SerializeFile(filePath, sourceObject);
                    return true;
                }
                else
                {//取消返回false
                    return false;
                }
            }
            catch (Exception ex)
            {//保存失败抛异常
                throw ex;
            }

        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="directoryPath">目录</param>
        /// <param name="fileName">文件名</param>
        public void DelFile(string directoryPath, string fileName)
        {
            try
            {
                string filePath = string.Empty;
                var result = IsFileExist(directoryPath, fileName, out filePath);
                if (result)
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

   
    }
}