using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_BurnInMessage;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_TestFrameBoard : Form, ITesterAppUI, IAccessPermissionLevel
    {
        ITesterCoreInteration _core;
        TestFrameManager _appInteration;
        const string TEXE_BUILDER_PAGE = "tp_TexeBuilder";   
        const string TEXE_COMBO_BUILDER_PAGE = "tp_TexeComboBuilder";
        const string TEXE_COMBO_PROFILER_PAGE = "tp_TexeComboProfiler";
        const string TEST_RECIPE_BUILDER_PAGE = "tp_TestRecipeBuilder";
        const string CALC_RECIPE_BUILDER_PAGE = "tp_TcalcRecipeBuilder";
        const string TEXE_COMBO_INSTR_EDITOR_PAGE = "tp_TexeComboInstrEditor";
        
        Dictionary<string, Form> FrameFormDict = new Dictionary<string, Form>();
        public Form_TestFrameBoard()
        {
            InitializeComponent();
        }
        public void RefreshOnce()
        {
            try
            {
                foreach (var subForm in FrameFormDict.Values)
                {
                    (subForm as ITesterAppUI).RefreshOnce();
                }
            }
            catch (Exception ex)
            {
                var msg = $"初始化窗体异常[{ex.Message}-{ex.StackTrace}]!";
                //this._core.Log_Global()
                this._appInteration.Log_Global(msg);
                MessageBox.Show(msg);
            }
        }
        private void Form_TestFrameBoard_Load(object sender, EventArgs e)
        {
            try
            {
                FrameFormDict.Add(TEXE_BUILDER_PAGE, new Form_TestExecutor_Builder_SimpleMode());
                //FrameFormDict.Add(TEXE_BUILDER_PAGE, new Form_TestExecutor_Builder());
                //FrameFormDict.Add(TEXE_COMBO_BUILDER_PAGE, new Form_TestExecutorCombo_Builder());
                FrameFormDict.Add(TEXE_COMBO_PROFILER_PAGE, new Form_TestExecutorCombo_Builder_SimpleMode());
                //FrameFormDict.Add(TEST_RECIPE_BUILDER_PAGE, new Form_TestRecipe_Builder());
                //FrameFormDict.Add(CALC_RECIPE_BUILDER_PAGE, new Form_CalculatorRecipe_Builder());
                //FrameFormDict.Add(TEXE_COMBO_INSTR_EDITOR_PAGE, new Form_TestExecutorCombo_InstrumentEditor());
                FrameFormDict.Add(TEXE_COMBO_INSTR_EDITOR_PAGE, new  Form_TestExecutorCombo_InstrumentEditor_SimpleMode());


                foreach (var kvp in FrameFormDict)
                {
                    //可dock ui
                    this._core.ModifyDockableUI(kvp.Value, true);
                    //链接app与core
                    ITesterAppUI ui = kvp.Value as ITesterAppUI;
                    this._core.LinkToCore(ui);
                    ui.ConnectToAppInteration(_appInteration);
                    //显示ui
                    kvp.Value.Show();
                    //dock ui 到 page
                    this.tb_TestFrames.TabPages[kvp.Key].Controls.Clear();
                    this.tb_TestFrames.TabPages[kvp.Key].Controls.Add(kvp.Value);
                }
                this.tb_TestFrames.TabPages[TEXE_COMBO_BUILDER_PAGE].Parent = null; 
                this.tb_TestFrames.TabPages[TEST_RECIPE_BUILDER_PAGE].Parent = null;
                this.tb_TestFrames.TabPages[CALC_RECIPE_BUILDER_PAGE].Parent = null;
            }
            catch (Exception ex)
            {
                var msg = $"初始化窗体异常[{ex.Message}-{ex.StackTrace}]!";
                //this._core.Log_Global()
                this._appInteration.Log_Global(msg);
                MessageBox.Show(msg);
            }
        }
     
        public AccessPermissionLevel APL
        {
            get
            {
                return AccessPermissionLevel.None;
            }
        }
        public void ConnectToAppInteration(ITesterAppPluginInteration app)
        {
            _appInteration = (TestFrameManager)app;
        }
        public void ConnectToCore(ITesterCoreInteration core)
        {
            _core = core as ITesterCoreInteration;
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core.SendOutFormCoreToGUIEvent += ReceiveMessageFromCore;
        }
        public void DisconnectFromCore(ITesterCoreInteration core)
        {
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core = null;
        }
        private void ReceiveMessageFromCore(IMessage message)
        {
            //预留消息处理
        }
        private void 浮动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _appInteration.PopUI();
        }

        private void 还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _appInteration.DockUI();
        }

        private void tb_TestFrames_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                foreach (var subForm in FrameFormDict.Values)
                {
                    (subForm as ITesterAppUI).RefreshOnce();
                }
            }
            catch (Exception ex)
            {
                var msg = $"初始化窗体异常[{ex.Message}-{ex.StackTrace}]!";
                //this._core.Log_Global()
                this._appInteration.Log_Global(msg);
                MessageBox.Show(msg);
            }
        }

        private void Form_TestFrameBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            _appInteration.DockUI();
        }
    }
}