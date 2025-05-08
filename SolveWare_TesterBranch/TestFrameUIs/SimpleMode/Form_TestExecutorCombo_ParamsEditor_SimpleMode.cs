using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using SolveWare_TestComponents.UIComponents;
using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_TestExecutorCombo_ParamsEditor_SimpleMode : Form, ITesterCoreLink
    {
        ITesterCoreInteration _core;
        public Form_TestExecutorCombo_ParamsEditor_SimpleMode()
        {
            InitializeComponent();
        }
        TestExecutorComboWithParams _editTestExecutorComboWithParams { get; set; }
        Dictionary<string, Form_TestAndCalcParamEditor> _TestAndCalcParamDict = new Dictionary<string, Form_TestAndCalcParamEditor>();


        public void ConnectToCore(ITesterCoreInteration core)
        {
            this._core = core;
        }

        public void DisconnectFromCore(ITesterCoreInteration core)
        {
            this._core = null;

        }
        private void Form_TestExecutorCombo_ParamsEditor_SimpleMode_Load(object sender, EventArgs e)
        {
            if (_editTestExecutorComboWithParams != null)
            {
                this.UpdateTestParamEditorLayerView(_editTestExecutorComboWithParams.ComboParams);
            }
        }
        public bool Setup(object editTestExecutorComboWithParamsObj)
        {
            TestExecutorComboWithParams temp = editTestExecutorComboWithParamsObj as TestExecutorComboWithParams;
            if (temp == null)
            {
                MessageBox.Show($"无法解析导入的测试项配置!");
                return false;
            }
            this.Text = $"测试参数编辑[{temp.Name}]";


            _editTestExecutorComboWithParams = temp;

            return true;
        }
        private void UpdateTestParamEditorLayerView(ExecutorConfigItem_TestParamsConfigCombo paramsCombo)
        {
            try
            {

                if (_TestAndCalcParamDict.Count > 0)
                {
                    foreach (var oldFrm in _TestAndCalcParamDict.Values)
                    {
                        oldFrm.Close();
                        oldFrm.Dispose();
                    }
                }
                _TestAndCalcParamDict.Clear();

                tab_TestParamEditorLayer.TabPages.Clear();

                //var paramsCombo = this._editTestProfile.TestParamsConfigComboDict[MT.PD测试模组1.ToString()];

                foreach (var testParamConfig in paramsCombo.Pre_TestParamsCollection)
                {
                    TabPage tp = new TabPage(testParamConfig.Name);

                    Form_TestAndCalcParamEditor frm = new Form_TestAndCalcParamEditor();
                    this._core.ModifyDockableUI(frm, true);
                    frm.Import(testParamConfig);
                    frm.Show();

                    tp.Controls.Add(frm);

                    _TestAndCalcParamDict.Add(testParamConfig.Name, frm);

                    tab_TestParamEditorLayer.TabPages.Add(tp);
                }
                foreach (var testParamConfig in paramsCombo.Main_TestParamsCollection)
                {
                    TabPage tp = new TabPage(testParamConfig.Name);

                    Form_TestAndCalcParamEditor frm = new Form_TestAndCalcParamEditor();
                    this._core.ModifyDockableUI(frm, true);
                    frm.Import(testParamConfig);
                    frm.Show();

                    tp.Controls.Add(frm);

                    _TestAndCalcParamDict.Add(testParamConfig.Name, frm);

                    tab_TestParamEditorLayer.TabPages.Add(tp);
                }
                foreach (var testParamConfig in paramsCombo.Post_TestParamsCollection)
                {
                    TabPage tp = new TabPage(testParamConfig.Name);

                    Form_TestAndCalcParamEditor frm = new Form_TestAndCalcParamEditor();
                    this._core.ModifyDockableUI(frm, true);
                    frm.Import(testParamConfig);
                    frm.Show();

                    tp.Controls.Add(frm);

                    _TestAndCalcParamDict.Add(testParamConfig.Name, frm);

                    tab_TestParamEditorLayer.TabPages.Add(tp);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_saveExecutorComboParams_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._editTestExecutorComboWithParams?.Combo == null)
                {
                    MessageBox.Show("没有正在编辑的测试链!");
                    return;
                }
 
                SaveFileDialog sfd = new SaveFileDialog();
                var prodPath = _core.GetProductConfigFileDirectory();
                var filePath = _core.StationConfig.GetSystemParamsValue(TesterKeyPathsAndFiles.TEST_EXECUTIVE_COMBO_PATH);
                sfd.InitialDirectory = Path.GetFullPath($@"{prodPath}\{filePath}");
                //var defaultFileName = string.Concat(this._editTestExecutorCombo.Name, FileExtension.XML);
                var defaultFileName = string.Concat(this._editTestExecutorComboWithParams.Name, FileExtension.XML);
                sfd.FileName = defaultFileName;
                var dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    string finalFileName = sfd.FileName;
                    finalFileName = Path.ChangeExtension(finalFileName, "xml");

                    if (finalFileName.Equals(defaultFileName))
                    {
                        foreach (var kvp in _TestAndCalcParamDict)
                        {
                            kvp.Value.Update();
                        }
                        this._editTestExecutorComboWithParams.Save(finalFileName);
                        MessageBox.Show("保存成功:\r\n\r\n" + finalFileName);
                    }
                    else
                    {
                        var finalExeComboName = Path.GetFileNameWithoutExtension(finalFileName);
                        var renameDR = MessageBox.Show($"是否重命名为[{finalExeComboName}]?" +
                              $"\r\n提示:文件名与测试项名必须保持一致" +
                              $"\r\n若选是 则将重命名为[{finalExeComboName}]并存储." +
                              $"\r\n若选否 则放弃保存.", "保存提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);

                        if (renameDR == DialogResult.Yes)
                        {
                            foreach (var kvp in _TestAndCalcParamDict)
                            {
                                kvp.Value.Update();
                            }
                            this._editTestExecutorComboWithParams.Name = finalExeComboName;
                            this._editTestExecutorComboWithParams.Save(finalFileName);
 
                            MessageBox.Show("保存成功:\r\n\r\n" + finalFileName);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败:[{ex.Message}+{ex.StackTrace}]!");
            }

        }

        private void btn_Close_Click(object sender, EventArgs e)
        {

            try
            {
                if (_TestAndCalcParamDict.Count > 0)
                {
                    foreach (var oldFrm in _TestAndCalcParamDict.Values)
                    {
                        oldFrm.Close();
                        oldFrm.Dispose();
                    }
                }
                _TestAndCalcParamDict.Clear();

                tab_TestParamEditorLayer.TabPages.Clear();
            }
            catch (Exception ex)
            {
            }
            this.Close();
        }
    }
}