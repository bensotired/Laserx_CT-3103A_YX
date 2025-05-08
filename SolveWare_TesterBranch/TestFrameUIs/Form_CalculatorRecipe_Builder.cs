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
    public partial class Form_CalculatorRecipe_Builder : Form, ITesterAppUI
    {
        ITesterCoreInteration _core;

        TestFrameManager _myFrameManager;

        //string TestRecipe_directoryPath
        //{
        //    get
        //    {
        //        return this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_RECIPE_PATH);
        //    }
        //}
        string CalcRecipe_directoryPath
        {
            get
            {
                var prodDir = this._core.GetProductConfigFileDirectory();
                var fileDir = this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CALC_RECIPE_PATH);
                var finalDir = $@"{prodDir}\{fileDir}";
                return finalDir;
            }
            //get
            //{
            //    return this._core?.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.CALC_RECIPE_PATH);
            //}
        }

        const int INFO_COL_INDEX = 0;
        const int KEY_COL_INDEX = 1;
        const int VAL_COL_INDEX = 2;


        //计算类型
        List<string> c_recipeTypeList;

        //当前文件中计算recipe
        CalcRecipe last_c_recipeFile;

        //临时新建计算recipe
        CalcRecipe temp_c_recipe;

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
        public Form_CalculatorRecipe_Builder()
        {
            InitializeComponent();
        }
        private void Form_CalculatorRecipe_Load(object sender,EventArgs e)
        {
            RefreshOnce();
        }
        public void RefreshOnce()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((EventHandler)delegate
                {
                    var c_recipeTypes = this._core.GetAssignableTypesFromPreLoadDlls(typeof(CalcRecipe));
                    c_recipeTypeList = new List<string>();
                    foreach (var item in c_recipeTypes)
                    {
                        c_recipeTypeList.Add(item.Name);
                    }
                    cmb_CalcRecipeType.Items.Clear();
                    cmb_CalcRecipeType.Items.AddRange(c_recipeTypeList.ToArray());
                    UpdateCRecipeFileSelector();
                });
            }
            else
            {
                var c_recipeTypes = this._core.GetAssignableTypesFromPreLoadDlls(typeof(CalcRecipe));
                c_recipeTypeList = new List<string>();
                foreach (var item in c_recipeTypes)
                {
                    c_recipeTypeList.Add(item.Name);
                }
                cmb_CalcRecipeType.Items.Clear();
                cmb_CalcRecipeType.Items.AddRange(c_recipeTypeList.ToArray());
                UpdateCRecipeFileSelector();
            }
        }
        private void UpdateCRecipeFileSelector()
        {
            try
            {
                var calc_recipeFile = _myFrameManager.GetLocalCalcRecipeFileDict();
                //var calc_recipeFile = _myFrameManager.GetSpecXmlFileNameFromDirect(CalcRecipe_directoryPath, c_recipeTypeList);
                this.cmb_CalculateRecipeList.Items.Clear();
                this.cmb_CalculateRecipeList.Items.AddRange(calc_recipeFile.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception("更新现有Calc-Recipe文件目录失败！异常原因：" + ex.Message);
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
            if (!_myFrameManager.IsFileExist(directoryPath, fileName, out filepath))
            {
                throw new Exception($"当前目录[{directoryPath}]下,不存在该文件[{fileName}]，请检查！");
            }
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
                throw new Exception($"加载文件异常，异常原因：[{ex.Message}]");
            }
        }

        #region  新建计算Recipe


        private void btn_CreateNewCRec_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmb_CalcRecipeType.SelectedItem == null)
                {
                    MessageBox.Show("请选择新建类型!!!");
                    return;
                }

                if (temp_c_recipe != null)
                {//当前存在编辑类型
                    var dr = MessageBox.Show("当前页面存在Calc-Recipe，是否重建", "提示", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)
                    {
                        var dr1 = MessageBox.Show("当前页面的Calc-Recipe是否保存", "提示", MessageBoxButtons.OKCancel);

                        if (dr1 == DialogResult.OK)
                        {
                            var editDict = _myFrameManager.GetDataFromPdgv(dgv_NewCalcRec, KEY_COL_INDEX, VAL_COL_INDEX);
                            _myFrameManager.UpdateLastRecipe(temp_c_recipe, editDict);
                            //校验数据
                            var saveResult = SaveNewRecipe(temp_c_recipe, CalcRecipe_directoryPath);
                            if (saveResult)
                            {
                                MessageBox.Show("保存成功！！！", "提示");
                            }
                        }
                    }
                    else
                    {
                        cmb_CalcRecipeType.SelectedItem = temp_c_recipe.GetType().Name;
                        return;
                    }
                }
                if (string.IsNullOrEmpty(cmb_CalcRecipeType.SelectedItem.ToString()) == false)
                {
                    var recipeInstance = this._core.CreateCalcRecipe(cmb_CalcRecipeType.SelectedItem.ToString());
                    temp_c_recipe = (CalcRecipe)recipeInstance;
                    _myFrameManager.Updatedgv(dgv_NewCalcRec, recipeInstance, INFO_COL_INDEX, KEY_COL_INDEX, VAL_COL_INDEX);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新建新建Calc-Recipe异常！异常原因：[{ex.Message}]", "提示");
            }
        }
        private bool SaveNewRecipe(object sourceObject, string directoryPath)
        {
            try
            {
                string fileName = string.Empty;

                if (!_myFrameManager.IsDirectExist(directoryPath))
                {
                    throw new Exception($"需要保存的文件目录[{directoryPath}]不存在，请检查！！！");
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

        private void btn_AddNewCRec_Click(object sender, EventArgs e)
        {
            try
            {
                if (temp_c_recipe == null)
                {
                    MessageBox.Show("无新建Calc-Recipe，请先创建!!!", "提示");
                    return;
                }

                var dr = MessageBox.Show("确认保存新建Calc-Recipe?", "提示", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }

                var editDict = _myFrameManager.GetDataFromPdgv(dgv_NewCalcRec, KEY_COL_INDEX, VAL_COL_INDEX);
                //if (string.IsNullOrEmpty(editDict["CalcData_PreFix"]?.ToString()) &&
                //    string.IsNullOrEmpty(editDict["CalcData_PostFix"]?.ToString()))
                //{
                //    MessageBox.Show("新建Calc-Recipe请至少设置[前缀]或[后缀]中的一个", "提示");
                //    return;
                //}
 



                _myFrameManager.UpdateLastRecipe(temp_c_recipe, editDict);
                //校验数据
                var saveResult = SaveNewRecipe(temp_c_recipe, CalcRecipe_directoryPath);
                if (saveResult)
                {
                    MessageBox.Show("保存成功！！！", "提示");
                }
                else
                {
                    return;
                }

                this.cmb_CalcRecipeType.SelectedIndex = -1;
                this.dgv_NewCalcRec.Rows.Clear();
                temp_c_recipe = null;
                UpdateCRecipeFileSelector();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存新建Calc-Recipe异常！异常原因：[{ex.Message}]", "提示");
            }
        }




        #endregion


        #region 计算Recipe编辑
        private void cmb_CalculateRecipe_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (last_c_recipeFile != null)
                {//当前下拉框有值
                    string errMsg = string.Empty;
                    Dictionary<string, object> editDict = new Dictionary<string, object>();
                    try
                    {
                        //获取当前界面的值
                        editDict = _myFrameManager.GetDataFromPdgv(dgv_CalculateRecipe, KEY_COL_INDEX, VAL_COL_INDEX);
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex.Message;
                    }
                    if (string.IsNullOrEmpty(errMsg))
                    {//无异常
                        var compareResult = _myFrameManager.CompareDgvValuesAndRecipeValues(last_c_recipeFile, editDict);
                        if (compareResult)
                        {//数据变动
                            var drs = MessageBox.Show("当前界面Calc-Recipe发生变动，是否保存？", "提示", MessageBoxButtons.OKCancel);
                            if (drs == DialogResult.OK)
                            {
                                //更新数据
                                _myFrameManager.UpdateLastRecipe(last_c_recipeFile, editDict);
                                //保存
                                var fileName = lbl_CurrentCRec.Text;
                                var saveResult = _myFrameManager.SaveRecipe(last_c_recipeFile, CalcRecipe_directoryPath, fileName);
                                if (saveResult)
                                {
                                    last_c_recipeFile = null;
                                    this.lbl_CurrentCRec.Text = string.Empty;
                                    this.lbl_CurrentCRec.Text = string.Empty;
                                    MessageBox.Show("保存成功！！！", "提示");
                                }
                            }
                        }
                    }
                    else
                    {//有异常
                        var dr = MessageBox.Show("当前界面Calc-Recipe发生变动，是否保存？", "提示", MessageBoxButtons.OKCancel);
                        if (dr == DialogResult.OK)
                        {
                            throw new Exception($"保存当前界面Calc-Recipe异常，异常原因：[{errMsg}]");
                        }
                    }
                }
                var recipeFileName = cmb_CalculateRecipeList.SelectedItem.ToString();
                last_c_recipeFile = (CalcRecipe)LoadContentFromFile(CalcRecipe_directoryPath, recipeFileName);
                _myFrameManager.Updatedgv(dgv_CalculateRecipe, last_c_recipeFile, INFO_COL_INDEX, KEY_COL_INDEX, VAL_COL_INDEX);
                this.lbl_CurrentCRec.Text = recipeFileName;
                this.lbl_CRecipeType.Text = last_c_recipeFile.GetType().Name;
            }
            catch (Exception ex)
            {
                this.cmb_CalculateRecipeList.Text = this.lbl_CurrentCRec.Text;
                MessageBox.Show(ex.Message, "提示");
            }
        }
        private void btn_Save_CRecipe_Click(object sender, EventArgs e)
        {

            if (last_c_recipeFile == null)
            {
                MessageBox.Show("请选择需要保存的测试规格");
                return;
            }
            try
            {
                //从dgv获取数据
                //var editDict = _myFrameManager.GetDataFromPdgv(dgv_CalculateRecipe, KEY_COL_INDEX, VAL_COL_INDEX);


                var editDict = _myFrameManager.GetDataFromPdgv(dgv_CalculateRecipe, KEY_COL_INDEX, VAL_COL_INDEX);
                //if (string.IsNullOrEmpty(editDict["CalcData_PreFix"]?.ToString()) &&
                //    string.IsNullOrEmpty(editDict["CalcData_PostFix"]?.ToString()))
                //{
                //    MessageBox.Show("Calc-Recipe请至少设置[前缀]或[后缀]中的一个", "提示");
                //    return;
                //}



                //更新
                _myFrameManager.UpdateLastRecipe(last_c_recipeFile, editDict);
                //保存
                var fileName = lbl_CurrentCRec.Text;
                var saveResult = _myFrameManager.SaveRecipe(last_c_recipeFile, CalcRecipe_directoryPath, fileName);

                if (saveResult)
                {
 
                    MessageBox.Show("保存成功！！！", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存当前Calc-Recipe失败，异常原因：[{ex.Message}]", "提示");
                return;
            }
        }

        private void btn_SaveAs_CRecipe_Click(object sender, EventArgs e)
        {
            if (last_c_recipeFile == null)
            {
                MessageBox.Show("请选择需要另存的Calc-Recipe！！！", "提示");
                return;
            }
            try
            {
                //根据当前类型创建一个新实例
                var typeName = last_c_recipeFile.GetType().Name;
                var tempRecipe = this._core.CreateCalcRecipe(typeName);
                //var editDict = _myFrameManager.GetDataFromPdgv(dgv_CalculateRecipe, KEY_COL_INDEX, VAL_COL_INDEX);


                var editDict = _myFrameManager.GetDataFromPdgv(dgv_CalculateRecipe, KEY_COL_INDEX, VAL_COL_INDEX);
                //if (string.IsNullOrEmpty(editDict["CalcData_PreFix"]?.ToString()) &&
                //    string.IsNullOrEmpty(editDict["CalcData_PostFix"]?.ToString()))
                //{
                //    MessageBox.Show("Calc-Recipe请至少设置[前缀]或[后缀]中的一个", "提示");
                //    return;
                //}


                _myFrameManager.UpdateLastRecipe(tempRecipe, editDict);
                var rs = _myFrameManager.SaveAsRecipe(tempRecipe, CalcRecipe_directoryPath);
                if (rs)
                {
                    UpdateCRecipeFileSelector();
                    MessageBox.Show("当前界面Calc-Recipe另存成功！", "提示");
                }
                else
                {//用户取消
                    var dr = MessageBox.Show("确认取消另存当前界面Calc-Recipe", "提示", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)
                    {
                        return;
                    }
                    else
                    {
                        var rs1 = _myFrameManager.SaveAsRecipe(tempRecipe, CalcRecipe_directoryPath);
                        if (rs1)
                        {
                            UpdateCRecipeFileSelector();
                            MessageBox.Show("当前界面Calc-Recipe另存成功！", "提示");
                        }
                        else
                        {
                            MessageBox.Show("用户取消另存当前界面Calc-Recipe操作！", "提示");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("另存Calc-Recipe，操作异常！异常原因：" + ex.Message, "提示");
            }
        }

        private void btn_Del_CRecipe_Click(object sender, EventArgs e)
        {
            string fileName = string.Empty;
            if (last_c_recipeFile == null)
            {
                MessageBox.Show("请选择需要删除的Calc-Recipe");
                return;
            }
            var dr = MessageBox.Show("确认删除当前Calc-Recipe吗", "提示", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.Cancel)
            {
                return;
            }

            try
            {
                fileName = cmb_CalculateRecipeList.SelectedItem.ToString();
                _myFrameManager.DelFile(CalcRecipe_directoryPath, fileName);
                UpdateCRecipeFileSelector();
                this.lbl_CurrentCRec.Text = string.Empty;
                this.lbl_CRecipeType.Text = string.Empty;
                this.cmb_CalculateRecipeList.SelectedIndex = -1;
                this.dgv_CalculateRecipe.Rows.Clear();
                last_c_recipeFile = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除Calc-Recipe[{fileName}]异常！异常原因：" + ex.Message, "提示");
            }
        }

     
        #endregion
    }
}
