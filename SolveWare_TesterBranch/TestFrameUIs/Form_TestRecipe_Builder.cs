using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SolveWare_TesterCore
{
    public partial class Form_TestRecipe_Builder : Form, ITesterAppUI
    {
        ITesterCoreInteration _core;

        TestFrameManager _myFrameManager;

        string TestRecipe_directoryPath
        {
            get
            {
                var prodDir = this._core.GetProductConfigFileDirectory();
                var fileDir = this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_RECIPE_PATH);
                var finalDir = $@"{prodDir}\{fileDir}";
                return finalDir;
            }
        }


        const int INFO_COL_INDEX = 0;
        const int KEY_COL_INDEX = 1;
        const int VAL_COL_INDEX = 2;

        //测试类型
        List<string> t_recipeTypeList;

        //当前文件中测试recipe
        TestRecipeBase last_t_recipeFile;

        //临时新建测试recipe
        TestRecipeBase temp_t_recipe;


        public void ConnectToAppInteration(ITesterAppPluginInteration app)
        {
            _myFrameManager = (TestFrameManager)app;
        }

        public void ConnectToCore(ITesterCoreInteration core)
        {
            _core = core as ITesterCoreInteration;
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core.SendOutFormCoreToGUIEvent += ReceiveMessageFromCore;
        }

        private void ReceiveMessageFromCore(IMessage message)
        {
            //预留消息处理
        }

        public void DisconnectFromCore(ITesterCoreInteration core)
        {
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core = null;
        }

        public Form_TestRecipe_Builder()
        {
            InitializeComponent();
        }
        private void Form_TestRecipe_Load(object secder, EventArgs e)
        {
            RefreshOnce();

        }
        public void RefreshOnce()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((EventHandler)delegate
                {
                    var t_recipeTypes = this._core.GetAssignableTypesFromPreLoadDlls(typeof(TestRecipeBase));
                    t_recipeTypeList = new List<string>();
                    foreach (var item in t_recipeTypes)
                    {
                        t_recipeTypeList.Add(item.Name);
                    }
                    cmb_TestRecipeType.Items.Clear();
                    cmb_TestRecipeType.Items.AddRange(t_recipeTypeList.ToArray());
                    UpdateTRecipeFileSelector();
                });
            }
            else
            {
                var t_recipeTypes = this._core.GetAssignableTypesFromPreLoadDlls(typeof(TestRecipeBase));
                t_recipeTypeList = new List<string>();
                foreach (var item in t_recipeTypes)
                {
                    t_recipeTypeList.Add(item.Name);
                }
                cmb_TestRecipeType.Items.Clear();
                cmb_TestRecipeType.Items.AddRange(t_recipeTypeList.ToArray());
                UpdateTRecipeFileSelector();
            }
        }
        #region  新建测试Recipe
        private void btn_CreateNewTRec_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmb_TestRecipeType.SelectedItem == null)
                {
                    MessageBox.Show("请选择新建Test-Recipe类型!!!");
                    return;
                }

                if (temp_t_recipe != null)
                {//当前存在编辑类型
                    var dr = MessageBox.Show("当前页面存在新建Test-Recipe，是否重建", "提示", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)
                    {
                        var dr1 = MessageBox.Show("当前页面的Test-Recipe是否保存", "提示", MessageBoxButtons.OKCancel);

                        if (dr1 == DialogResult.OK)
                        {
                            var editDict = _myFrameManager.GetDataFromPdgv(dgv_NewTestRec, KEY_COL_INDEX, VAL_COL_INDEX);
                            _myFrameManager.UpdateLastRecipe(temp_t_recipe, editDict);
                            //校验数据
                            var saveResult = SaveNewRecipe(temp_t_recipe, TestRecipe_directoryPath);
                            if (saveResult)
                            {
                                MessageBox.Show("保存成功！！！", "提示");
                            }
                        }
                    }
                    else
                    {
                        cmb_TestRecipeType.SelectedItem = temp_t_recipe.GetType().Name;
                        return;
                    }
                }

                if (string.IsNullOrEmpty(cmb_TestRecipeType.SelectedItem.ToString()) == false)
                {
                    var recipeInstance = this._core.CreateTestRecipe(cmb_TestRecipeType.SelectedItem.ToString());
                    temp_t_recipe = (TestRecipeBase)recipeInstance;
                    _myFrameManager.Updatedgv(this.dgv_NewTestRec, recipeInstance, INFO_COL_INDEX, KEY_COL_INDEX, VAL_COL_INDEX);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新建Test-Recipe异常！异常原因：[{ex.Message}]", "提示");
            }
        }

        private bool SaveNewRecipe(object sourceObject, string directoryPath)
        {
            try
            {
                string fileName = string.Empty;

                if (!_myFrameManager.IsDirectExist(directoryPath))
                {
                    throw new Exception($"需要保存的Test-Recipe文件目录[{directoryPath}]不存在，请检查！！！");
                }
                var files = Directory.GetFiles(directoryPath);
                List<string> fileNameList = new List<string>();
                if (files.Length > 0)
                {
                    foreach (var item in files)
                    {
                        fileNameList.Add(Path.GetFileNameWithoutExtension(item));
                    }
                }

                Form_RecipeInfo frm = new Form_RecipeInfo();
                frm.FileNameList = fileNameList;
                var dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    fileName = frm.FileName;
                }
                else
                {
                    return false;
                }
                var result = _myFrameManager.SaveRecipe(sourceObject, directoryPath, fileName);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion


        #region 测试Recipe编辑

        private void cmb_TestRecipeList_SelectionChangeCommitted(object sender, EventArgs e)
        {

            try
            {
                if (last_t_recipeFile != null)
                {//当前下拉框有值
                    string errMsg = string.Empty;
                    Dictionary<string, object> editDict = new Dictionary<string, object>();
                    try
                    {
                        //获取当前界面的值
                        editDict = _myFrameManager.GetDataFromPdgv(dgv_TestRecipe, KEY_COL_INDEX, VAL_COL_INDEX);
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex.Message;
                    }
                    if (string.IsNullOrEmpty(errMsg))
                    {//无异常
                        var compareResult = _myFrameManager.CompareDgvValuesAndRecipeValues(last_t_recipeFile, editDict);
                        if (compareResult)
                        {//数据变动
                            var drs = MessageBox.Show("当前界面Test-Recipe发生变动，是否保存？", "提示", MessageBoxButtons.OKCancel);
                            if (drs == DialogResult.OK)
                            {
                                //更新数据
                                _myFrameManager.UpdateLastRecipe(last_t_recipeFile, editDict);
                                //保存
                                var fileName = lbl_CurrentTRec.Text;
                                var saveResult = _myFrameManager.SaveRecipe(last_t_recipeFile, TestRecipe_directoryPath, fileName);
                                if (saveResult)
                                {
                                    last_t_recipeFile = null;
                                    this.lbl_CurrentTRec.Text = string.Empty;
                                    this.lbl_TRecipeType.Text = string.Empty;
                                    MessageBox.Show("保存成功！！！", "提示");
                                }
                            }
                        }
                    }
                    else
                    {//有异常
                        var dr = MessageBox.Show("当前界面Test-Recipe发生变动，是否保存？", "提示", MessageBoxButtons.OKCancel);
                        if (dr == DialogResult.OK)
                        {
                            throw new Exception($"保存当前界面Test-Recipe异常，异常原因：[{errMsg}]");
                        }
                    }
                }
                var recipeFileName = cmb_TestRecipeList.SelectedItem.ToString();
                last_t_recipeFile = (TestRecipeBase)LoadContentFromFile(TestRecipe_directoryPath, recipeFileName);
                _myFrameManager.Updatedgv(this.dgv_TestRecipe, last_t_recipeFile, INFO_COL_INDEX, KEY_COL_INDEX, VAL_COL_INDEX);
                this.lbl_CurrentTRec.Text = recipeFileName;
                this.lbl_TRecipeType.Text = last_t_recipeFile.GetType().Name;
            }
            catch (Exception ex)
            {
                this.cmb_TestRecipeList.Text = this.lbl_CurrentTRec.Text;
                MessageBox.Show(ex.Message, "提示");
            }

        }
        private void btn_AddNewTRec_Click(object sender, EventArgs e)
        {
            try
            {
                if (temp_t_recipe == null)
                {
                    MessageBox.Show("无新建Test-recipe，请先创建！！！", "提示");
                    return;
                }

                var dr = MessageBox.Show("确认保存新建Test-Recipe?", "提示", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }

                //var editDict = _myFrameManager.GetDataFromPdgv(dgv_NewTestRec, KEY_COL_INDEX, VAL_COL_INDEX);

                var editDict = _myFrameManager.GetDataFromPdgv(dgv_NewTestRec, KEY_COL_INDEX, VAL_COL_INDEX);
                //if (string.IsNullOrEmpty(editDict["SummaryData_PreFix"]?.ToString()) &&
                //    string.IsNullOrEmpty(editDict["SummaryData_PostFix"]?.ToString()))
                //{
                //    MessageBox.Show("新建Test-Recipe请至少设置[前缀]或[后缀]中的一个", "提示");
                //    return;
                //}




                _myFrameManager.UpdateLastRecipe(temp_t_recipe, editDict);
                //校验数据
                var saveResult = SaveNewRecipe(temp_t_recipe, TestRecipe_directoryPath);
                if (saveResult)
                {
                    MessageBox.Show("保存成功！！！", "提示");
                }
                else
                {
                    return;
                }

                this.cmb_TestRecipeType.SelectedIndex = -1;
                this.dgv_NewTestRec.Rows.Clear();
                temp_t_recipe = null;
                UpdateTRecipeFileSelector();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存新建Test-Recipe异常！异常原因：[{ex.Message}]", "提示");
            }

        }


        private void btn_Save_TRecipe_Click(object sender, EventArgs e)
        {
            if (last_t_recipeFile == null)
            {
                MessageBox.Show("请选择需要保存的Test-Recipe");
                return;
            }
            try
            {
                //从dgv获取数据
                //var editDict = _myFrameManager.GetDataFromPdgv(dgv_TestRecipe, KEY_COL_INDEX, VAL_COL_INDEX);
                var editDict = _myFrameManager.GetDataFromPdgv(dgv_TestRecipe, KEY_COL_INDEX, VAL_COL_INDEX);
                //if (string.IsNullOrEmpty(editDict["SummaryData_PreFix"]?.ToString()) &&
                //    string.IsNullOrEmpty(editDict["SummaryData_PostFix"]?.ToString()))
                //{
                //    MessageBox.Show("Test-Recipe请至少设置[前缀]或[后缀]中的一个", "提示");
                //    return;
                //}

                //更新
                _myFrameManager.UpdateLastRecipe(last_t_recipeFile, editDict);
                //保存
                var fileName = lbl_CurrentTRec.Text;
                var saveResult = _myFrameManager.SaveRecipe(last_t_recipeFile, TestRecipe_directoryPath, fileName);

                if (saveResult)
                {
                    MessageBox.Show("保存成功！！！", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存当前Test-Recipe失败，异常原因：[{ex.Message}]", "提示");
                return;
            }
        }

        private void btn_SaveAs_TRecipe_Click(object sender, EventArgs e)
        {
            if (last_t_recipeFile == null)
            {
                MessageBox.Show("请选择需要另存的Test-Recipe！！！", "提示");
                return;
            }
            try
            {
                //根据当前类型创建一个新实例
                var typeName = last_t_recipeFile.GetType().Name;
                var tempRecipe = this._core.CreateTestRecipe(typeName);
                //var editDict = _myFrameManager.GetDataFromPdgv(dgv_TestRecipe, KEY_COL_INDEX, VAL_COL_INDEX);

                var editDict = _myFrameManager.GetDataFromPdgv(dgv_TestRecipe, KEY_COL_INDEX, VAL_COL_INDEX);
                //if (string.IsNullOrEmpty(editDict["SummaryData_PreFix"]?.ToString()) &&
                //    string.IsNullOrEmpty(editDict["SummaryData_PostFix"]?.ToString()))
                //{
                //    MessageBox.Show("Test-Recipe请至少设置[前缀]或[后缀]中的一个", "提示");
                //    return;
                //}



                _myFrameManager.UpdateLastRecipe(tempRecipe, editDict);
                var rs = _myFrameManager.SaveAsRecipe(tempRecipe, TestRecipe_directoryPath);
                if (rs)
                {
                    UpdateTRecipeFileSelector();
                    MessageBox.Show("当前界面Test-Recipe另存成功！", "提示");
                }
                else
                {//用户取消
                    var dr = MessageBox.Show("确认取消另存当前界面Test-Recipe", "提示", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)
                    {
                        return;
                    }
                    else
                    {
                        var rs1 = _myFrameManager.SaveAsRecipe(tempRecipe, TestRecipe_directoryPath);
                        if (rs1)
                        {
                            UpdateTRecipeFileSelector();
                            MessageBox.Show("当前界面Test-Recipe另存成功！", "提示");
                        }
                        else
                        {
                            MessageBox.Show("用户取消另存当前界面Test-Recipe操作！", "提示");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("另存Test-Recipe，操作异常！异常原因：" + ex.Message, "提示");
            }
        }

        private void btn_Del_TRecipe_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            if (last_t_recipeFile == null)
            {
                MessageBox.Show("请选择需要删除的Test-Recipe");
                return;
            }
            var dr = MessageBox.Show("确认删除当前Test-Recipe吗", "提示", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            try
            {
                fileName = cmb_TestRecipeList.SelectedItem.ToString();
                _myFrameManager.DelFile(TestRecipe_directoryPath, fileName);
                UpdateTRecipeFileSelector();
                this.lbl_CurrentTRec.Text = string.Empty;
                this.lbl_TRecipeType.Text = string.Empty;
                this.cmb_TestRecipeList.SelectedIndex = -1;
                this.dgv_TestRecipe.Rows.Clear();
                last_t_recipeFile = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除Test-Recipe[{fileName}]异常！异常原因：" + ex.Message, "提示");
            }
        }
        #endregion


        private void UpdateTRecipeFileSelector()
        {
            try
            {
                var test_recipeFile = _myFrameManager.GetLocalTestRecipeFileDict();
                //var t_recipeFile = _myFrameManager.GetSpecXmlFileNameFromDirect(TestRecipe_directoryPath, t_recipeTypeList);
                this.cmb_TestRecipeList.Items.Clear();
                this.cmb_TestRecipeList.Items.AddRange(test_recipeFile.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception("更新现有Test-Recipe文件目录失败！异常原因：" + ex.Message);
            }
        }
        private void Dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                var dgv = sender as DataGridView;
                if (e.Exception is System.ArgumentException)
                {
                    if (dgv[e.ColumnIndex, e.RowIndex] is DataGridViewComboBoxCell)
                    {
                        var value = dgv[e.ColumnIndex, e.RowIndex].Value;
                        var valueType = (dgv[e.ColumnIndex, e.RowIndex].Tag as PropertyInfo).PropertyType;

                        if (valueType.IsEnum)
                        {
                            dgv[e.ColumnIndex, e.RowIndex].Value = Converter.ConvertObjectTo(value, valueType).ToString();
                        }
                        else
                        {
                            dgv[e.ColumnIndex, e.RowIndex].Value = Converter.ConvertObjectTo(value, valueType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private object LoadContentFromFile(string directoryPath, string fileName)
        {
            string filepath = string.Empty;

           
            if (!_myFrameManager.IsFileExist($@" {directoryPath}", fileName, out filepath))
            {
                throw new Exception($"当前目录[{directoryPath}]下,不存在该文件[{fileName}]，请检查！");
            }

            //var tempPrdPath = this._core.GetProductConfigFileDirectory();
            //if (!_myFrameManager.IsFileExist($@"{tempPrdPath}\{directoryPath}", fileName, out filepath))
            //{
            //    throw new Exception($"当前目录[{directoryPath}]下,不存在该文件[{fileName}]，请检查！");
            //}
            object recipeInstance;
            try
            {
                var temp = XElement.Load(filepath);
                var rootNode = temp.Name.ToString();
                var type = this._core.GetTypeFromClassInPreLoadDlls(rootNode);
                recipeInstance = XmlHelper.DeserializeXElement(temp, type);
                return recipeInstance;
            }
            catch (Exception ex)
            {
                throw new Exception($"加载文件Test-Recipe异常，异常原因：[{ex.Message}]");
            }
        }
    }
}