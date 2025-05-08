using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    partial class Form_MainPage_CT3103
    {
        private void Temp(object sender)
        {
            var ctrl = sender as Control;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    ctrl.Invoke((EventHandler)delegate { ctrl.Enabled = false; });





               
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"异常！异常原因:{ex.Message} - {ex.StackTrace}!");
                }
                finally
                {
                    ctrl.Invoke((EventHandler)delegate { ctrl.Enabled = true; });

                }
            });
        }
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tb_MainPage = new System.Windows.Forms.TabControl();
            this.tabPage测试 = new System.Windows.Forms.TabPage();
            this.tab_TestPage = new System.Windows.Forms.TabControl();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.pnl_TestEnterance = new System.Windows.Forms.Panel();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.pnl_RuntimeOverviewPage = new System.Windows.Forms.Panel();
            this.tabPage调试 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panel_debug = new System.Windows.Forms.Panel();
            this.tabPage数据库 = new System.Windows.Forms.TabPage();
            this.panel7 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_PostBIColumn = new System.Windows.Forms.ComboBox();
            this.rb_PostBI = new System.Windows.Forms.RadioButton();
            this.rb_PreBI = new System.Windows.Forms.RadioButton();
            this.rb_GS = new System.Windows.Forms.RadioButton();
            this.label90 = new System.Windows.Forms.Label();
            this.label89 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btn_GetTestStations = new System.Windows.Forms.Button();
            this.lbl_teststation = new System.Windows.Forms.Label();
            this.cbo_TestStation = new System.Windows.Forms.ComboBox();
            this.tb_DataFile = new System.Windows.Forms.TextBox();
            this.btn_UploadData = new System.Windows.Forms.Button();
            this.btn_SelectData = new System.Windows.Forms.Button();
            this.btn_ExportData = new System.Windows.Forms.Button();
            this.cb_Fail = new System.Windows.Forms.CheckBox();
            this.cb_Pass = new System.Windows.Forms.CheckBox();
            this.btn_GetChipID = new System.Windows.Forms.Button();
            this.btn_GetCarrierID = new System.Windows.Forms.Button();
            this.btn_GetPN = new System.Windows.Forms.Button();
            this.btn_GetSubOrder = new System.Windows.Forms.Button();
            this.btn_QueryData = new System.Windows.Forms.Button();
            this.btn_GetWO = new System.Windows.Forms.Button();
            this.label36 = new System.Windows.Forms.Label();
            this.cbo_ChipID = new System.Windows.Forms.ComboBox();
            this.label35 = new System.Windows.Forms.Label();
            this.cbo_CarrierID = new System.Windows.Forms.ComboBox();
            this.label34 = new System.Windows.Forms.Label();
            this.cbo_PartNumber = new System.Windows.Forms.ComboBox();
            this.label33 = new System.Windows.Forms.Label();
            this.cbo_SubOrder = new System.Windows.Forms.ComboBox();
            this.label32 = new System.Windows.Forms.Label();
            this.cbo_WorkOrder = new System.Windows.Forms.ComboBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.dgv_TestData = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage视频 = new System.Windows.Forms.TabPage();
            this.panel_CV = new System.Windows.Forms.Panel();
            this.tabPage仪表 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.bt_TC_start = new System.Windows.Forms.Button();
            this.bt_TC_stop = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.bt_ted_start = new System.Windows.Forms.Button();
            this.bt_ted_stop = new System.Windows.Forms.Button();
            this.bt_gettemp = new System.Windows.Forms.Button();
            this.lab_ted = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label56 = new System.Windows.Forms.Label();
            this.txt_OpticalChannel = new System.Windows.Forms.TextBox();
            this.btn_SwitchCh = new System.Windows.Forms.Button();
            this.rb_right = new System.Windows.Forms.RadioButton();
            this.rb_left = new System.Windows.Forms.RadioButton();
            this.tb_temp = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label62 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.tb_liv_pdcompCurr = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.tb_liv_pdbiasV = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.tb_liv_compV = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.tb_liv_end = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.tb_liv_step = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.tb_liv_start = new System.Windows.Forms.TextBox();
            this.bt_LIV = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label55 = new System.Windows.Forms.Label();
            this.tb_wave = new System.Windows.Forms.TextBox();
            this.label54 = new System.Windows.Forms.Label();
            this.tb_PD_I = new System.Windows.Forms.TextBox();
            this.lab_GetPD_V = new System.Windows.Forms.Label();
            this.bt_GetPD = new System.Windows.Forms.Button();
            this.lab_GetPD_I = new System.Windows.Forms.Label();
            this.bt_PD_OFF = new System.Windows.Forms.Button();
            this.bt_PD_ON = new System.Windows.Forms.Button();
            this.label53 = new System.Windows.Forms.Label();
            this.lab_GetMPD2_V = new System.Windows.Forms.Label();
            this.lab_GetMPD1_V = new System.Windows.Forms.Label();
            this.lab_GetBIAS2_V = new System.Windows.Forms.Label();
            this.lab_GetBIAS1_V = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.bt_GetMPD2 = new System.Windows.Forms.Button();
            this.bt_GetMPD1 = new System.Windows.Forms.Button();
            this.bt_GetBIAS2 = new System.Windows.Forms.Button();
            this.bt_GetBIAS1 = new System.Windows.Forms.Button();
            this.lab_GetMPD2_I = new System.Windows.Forms.Label();
            this.lab_GetMPD1_I = new System.Windows.Forms.Label();
            this.lab_GetBIAS2_I = new System.Windows.Forms.Label();
            this.lab_GetBIAS1_I = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.bt_MPD2_OFF = new System.Windows.Forms.Button();
            this.bt_MPD2_ON = new System.Windows.Forms.Button();
            this.tb_MPD2_I = new System.Windows.Forms.TextBox();
            this.tb_MPD2_V = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.bt_MPD1_OFF = new System.Windows.Forms.Button();
            this.bt_MPD1_ON = new System.Windows.Forms.Button();
            this.tb_MPD1_I = new System.Windows.Forms.TextBox();
            this.tb_MPD1_V = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.bt_Bias2_OFF = new System.Windows.Forms.Button();
            this.bt_Bias2_ON = new System.Windows.Forms.Button();
            this.tb_Bias2_I = new System.Windows.Forms.TextBox();
            this.tb_Bias2_V = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.bt_Bias1_OFF = new System.Windows.Forms.Button();
            this.bt_Bias1_ON = new System.Windows.Forms.Button();
            this.tb_Bias1_I = new System.Windows.Forms.TextBox();
            this.tb_Bias1_V = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bt_OFF_All = new System.Windows.Forms.Button();
            this.lab_GetMIRROR2_I = new System.Windows.Forms.Label();
            this.lab_GetMIRROR2_V = new System.Windows.Forms.Label();
            this.lab_GetMIRROR1_I = new System.Windows.Forms.Label();
            this.lab_GetMIRROR1_V = new System.Windows.Forms.Label();
            this.lab_GetPH2_I = new System.Windows.Forms.Label();
            this.lab_GetPH2_V = new System.Windows.Forms.Label();
            this.lab_GetPH1_I = new System.Windows.Forms.Label();
            this.lab_GetPH1_V = new System.Windows.Forms.Label();
            this.lab_GetLP_I = new System.Windows.Forms.Label();
            this.lab_GetLP_V = new System.Windows.Forms.Label();
            this.lab_GetSOA2_I = new System.Windows.Forms.Label();
            this.lab_GetSOA2_V = new System.Windows.Forms.Label();
            this.lab_GetSOA1_I = new System.Windows.Forms.Label();
            this.lab_GetSOA1_V = new System.Windows.Forms.Label();
            this.lab_GetGAIN_I = new System.Windows.Forms.Label();
            this.lab_GetGAIN_V = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.bt_GetMIRROR2 = new System.Windows.Forms.Button();
            this.bt_GetMIRROR1 = new System.Windows.Forms.Button();
            this.bt_GetGAIN = new System.Windows.Forms.Button();
            this.bt_GetPH2 = new System.Windows.Forms.Button();
            this.bt_GetSOA1 = new System.Windows.Forms.Button();
            this.bt_GetPH1 = new System.Windows.Forms.Button();
            this.bt_GetSOA2 = new System.Windows.Forms.Button();
            this.bt_GetLP = new System.Windows.Forms.Button();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bt_MIRROR2_OFF = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_MIRROR2_ON = new System.Windows.Forms.Button();
            this.label51 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_MIRROR2_I = new System.Windows.Forms.TextBox();
            this.tb_GAIN_V = new System.Windows.Forms.TextBox();
            this.tb_MIRROR2_V = new System.Windows.Forms.TextBox();
            this.tb_GAIN_I = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.bt_GAIN_ON = new System.Windows.Forms.Button();
            this.bt_MIRROR1_OFF = new System.Windows.Forms.Button();
            this.bt_GAIN_OFF = new System.Windows.Forms.Button();
            this.bt_MIRROR1_ON = new System.Windows.Forms.Button();
            this.SOA1 = new System.Windows.Forms.Label();
            this.tb_MIRROR1_I = new System.Windows.Forms.TextBox();
            this.tb_SOA1_V = new System.Windows.Forms.TextBox();
            this.tb_MIRROR1_V = new System.Windows.Forms.TextBox();
            this.tb_SOA1_I = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.bt_SOA1_ON = new System.Windows.Forms.Button();
            this.bt_PH2_OFF = new System.Windows.Forms.Button();
            this.bt_SOA1_OFF = new System.Windows.Forms.Button();
            this.bt_PH2_ON = new System.Windows.Forms.Button();
            this.SOA2 = new System.Windows.Forms.Label();
            this.tb_PH2_I = new System.Windows.Forms.TextBox();
            this.tb_SOA2_V = new System.Windows.Forms.TextBox();
            this.tb_PH2_V = new System.Windows.Forms.TextBox();
            this.tb_SOA2_I = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.bt_SOA2_ON = new System.Windows.Forms.Button();
            this.bt_PH1_OFF = new System.Windows.Forms.Button();
            this.bt_SOA2_OFF = new System.Windows.Forms.Button();
            this.bt_PH1_ON = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_PH1_I = new System.Windows.Forms.TextBox();
            this.tb_LP_V = new System.Windows.Forms.TextBox();
            this.tb_PH1_V = new System.Windows.Forms.TextBox();
            this.tb_LP_I = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.bt_LP_ON = new System.Windows.Forms.Button();
            this.bt_LP_OFF = new System.Windows.Forms.Button();
            this.CoarseTuning = new System.Windows.Forms.TabPage();
            this.panel_coarsetuning = new System.Windows.Forms.Panel();
            this.tabPage_coarse = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel_coarse = new System.Windows.Forms.Panel();
            this.bt_choose = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label61 = new System.Windows.Forms.Label();
            this.txt_ResultFileCommon_AlternativeQWLT = new System.Windows.Forms.TextBox();
            this.btn_AnalyzeFileData_AlternativeQWLT = new System.Windows.Forms.Button();
            this.txt_SelectedFileList_AlternativeQWLT = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label60 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.txt_ResultFileCommon_Deviations = new System.Windows.Forms.TextBox();
            this.btn_AnalyzeFileData_Deviations = new System.Windows.Forms.Button();
            this.txt_SelectedChannel_Deviations = new System.Windows.Forms.TextBox();
            this.txt_SelectedFileList_Deviations = new System.Windows.Forms.TextBox();
            this.timer_Engineer = new System.Windows.Forms.Timer(this.components);
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label63 = new System.Windows.Forms.Label();
            this.tb_MainPage.SuspendLayout();
            this.tabPage测试.SuspendLayout();
            this.tab_TestPage.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage12.SuspendLayout();
            this.tabPage调试.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tabPage数据库.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TestData)).BeginInit();
            this.tabPage视频.SuspendLayout();
            this.tabPage仪表.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.CoarseTuning.SuspendLayout();
            this.tabPage_coarse.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_MainPage
            // 
            this.tb_MainPage.Controls.Add(this.tabPage测试);
            this.tb_MainPage.Controls.Add(this.tabPage调试);
            this.tb_MainPage.Controls.Add(this.tabPage数据库);
            this.tb_MainPage.Controls.Add(this.tabPage视频);
            this.tb_MainPage.Controls.Add(this.tabPage仪表);
            this.tb_MainPage.Controls.Add(this.CoarseTuning);
            this.tb_MainPage.Controls.Add(this.tabPage_coarse);
            this.tb_MainPage.Controls.Add(this.tabPage1);
            this.tb_MainPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_MainPage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_MainPage.ItemSize = new System.Drawing.Size(48, 38);
            this.tb_MainPage.Location = new System.Drawing.Point(0, 0);
            this.tb_MainPage.Name = "tb_MainPage";
            this.tb_MainPage.SelectedIndex = 0;
            this.tb_MainPage.Size = new System.Drawing.Size(1339, 858);
            this.tb_MainPage.TabIndex = 7;
            this.tb_MainPage.SelectedIndexChanged += new System.EventHandler(this.tb_MainPage_SelectedIndexChanged);
            // 
            // tabPage测试
            // 
            this.tabPage测试.Controls.Add(this.tab_TestPage);
            this.tabPage测试.Location = new System.Drawing.Point(4, 42);
            this.tabPage测试.Name = "tabPage测试";
            this.tabPage测试.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage测试.Size = new System.Drawing.Size(1331, 812);
            this.tabPage测试.TabIndex = 7;
            this.tabPage测试.Text = "测试";
            this.tabPage测试.UseVisualStyleBackColor = true;
            // 
            // tab_TestPage
            // 
            this.tab_TestPage.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tab_TestPage.Controls.Add(this.tabPage9);
            this.tab_TestPage.Controls.Add(this.tabPage12);
            this.tab_TestPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_TestPage.Location = new System.Drawing.Point(3, 3);
            this.tab_TestPage.Name = "tab_TestPage";
            this.tab_TestPage.SelectedIndex = 0;
            this.tab_TestPage.Size = new System.Drawing.Size(1325, 806);
            this.tab_TestPage.TabIndex = 1;
            // 
            // tabPage9
            // 
            this.tabPage9.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage9.Controls.Add(this.pnl_TestEnterance);
            this.tabPage9.Location = new System.Drawing.Point(4, 4);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(1317, 780);
            this.tabPage9.TabIndex = 0;
            this.tabPage9.Text = "测试配置";
            // 
            // pnl_TestEnterance
            // 
            this.pnl_TestEnterance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_TestEnterance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pnl_TestEnterance.Location = new System.Drawing.Point(3, 3);
            this.pnl_TestEnterance.Name = "pnl_TestEnterance";
            this.pnl_TestEnterance.Size = new System.Drawing.Size(1311, 774);
            this.pnl_TestEnterance.TabIndex = 0;
            // 
            // tabPage12
            // 
            this.tabPage12.Controls.Add(this.pnl_RuntimeOverviewPage);
            this.tabPage12.Location = new System.Drawing.Point(4, 4);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(1317, 780);
            this.tabPage12.TabIndex = 1;
            this.tabPage12.Text = "实时数据";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // pnl_RuntimeOverviewPage
            // 
            this.pnl_RuntimeOverviewPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_RuntimeOverviewPage.Location = new System.Drawing.Point(3, 3);
            this.pnl_RuntimeOverviewPage.Name = "pnl_RuntimeOverviewPage";
            this.pnl_RuntimeOverviewPage.Size = new System.Drawing.Size(1311, 774);
            this.pnl_RuntimeOverviewPage.TabIndex = 1;
            // 
            // tabPage调试
            // 
            this.tabPage调试.Controls.Add(this.tableLayoutPanel4);
            this.tabPage调试.Location = new System.Drawing.Point(4, 42);
            this.tabPage调试.Name = "tabPage调试";
            this.tabPage调试.Size = new System.Drawing.Size(1331, 812);
            this.tabPage调试.TabIndex = 17;
            this.tabPage调试.Text = "马达运动";
            this.tabPage调试.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.panel_debug, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 812F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1331, 812);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // panel_debug
            // 
            this.panel_debug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_debug.Location = new System.Drawing.Point(3, 3);
            this.panel_debug.Name = "panel_debug";
            this.panel_debug.Size = new System.Drawing.Size(1325, 806);
            this.panel_debug.TabIndex = 1;
            // 
            // tabPage数据库
            // 
            this.tabPage数据库.Controls.Add(this.panel7);
            this.tabPage数据库.Controls.Add(this.label2);
            this.tabPage数据库.Location = new System.Drawing.Point(4, 42);
            this.tabPage数据库.Name = "tabPage数据库";
            this.tabPage数据库.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage数据库.Size = new System.Drawing.Size(1331, 812);
            this.tabPage数据库.TabIndex = 0;
            this.tabPage数据库.Text = "数据库";
            this.tabPage数据库.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.tableLayoutPanel5);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1325, 806);
            this.panel7.TabIndex = 20;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel5.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.panel10, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.38441F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.06209F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1325, 806);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cb_PostBIColumn);
            this.panel1.Controls.Add(this.rb_PostBI);
            this.panel1.Controls.Add(this.rb_PreBI);
            this.panel1.Controls.Add(this.rb_GS);
            this.panel1.Controls.Add(this.label90);
            this.panel1.Controls.Add(this.label89);
            this.panel1.Controls.Add(this.dtpTo);
            this.panel1.Controls.Add(this.dtpFrom);
            this.panel1.Controls.Add(this.btn_GetTestStations);
            this.panel1.Controls.Add(this.lbl_teststation);
            this.panel1.Controls.Add(this.cbo_TestStation);
            this.panel1.Controls.Add(this.tb_DataFile);
            this.panel1.Controls.Add(this.btn_UploadData);
            this.panel1.Controls.Add(this.btn_SelectData);
            this.panel1.Controls.Add(this.btn_ExportData);
            this.panel1.Controls.Add(this.cb_Fail);
            this.panel1.Controls.Add(this.cb_Pass);
            this.panel1.Controls.Add(this.btn_GetChipID);
            this.panel1.Controls.Add(this.btn_GetCarrierID);
            this.panel1.Controls.Add(this.btn_GetPN);
            this.panel1.Controls.Add(this.btn_GetSubOrder);
            this.panel1.Controls.Add(this.btn_QueryData);
            this.panel1.Controls.Add(this.btn_GetWO);
            this.panel1.Controls.Add(this.label36);
            this.panel1.Controls.Add(this.cbo_ChipID);
            this.panel1.Controls.Add(this.label35);
            this.panel1.Controls.Add(this.cbo_CarrierID);
            this.panel1.Controls.Add(this.label34);
            this.panel1.Controls.Add(this.cbo_PartNumber);
            this.panel1.Controls.Add(this.label33);
            this.panel1.Controls.Add(this.cbo_SubOrder);
            this.panel1.Controls.Add(this.label32);
            this.panel1.Controls.Add(this.cbo_WorkOrder);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1319, 295);
            this.panel1.TabIndex = 0;
            // 
            // cb_PostBIColumn
            // 
            this.cb_PostBIColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_PostBIColumn.Enabled = false;
            this.cb_PostBIColumn.FormattingEnabled = true;
            this.cb_PostBIColumn.Location = new System.Drawing.Point(759, 17);
            this.cb_PostBIColumn.Name = "cb_PostBIColumn";
            this.cb_PostBIColumn.Size = new System.Drawing.Size(121, 20);
            this.cb_PostBIColumn.TabIndex = 41;
            this.cb_PostBIColumn.SelectedIndexChanged += new System.EventHandler(this.cb_PostBIColumn_SelectedIndexChanged);
            // 
            // rb_PostBI
            // 
            this.rb_PostBI.AutoSize = true;
            this.rb_PostBI.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb_PostBI.Location = new System.Drawing.Point(656, 16);
            this.rb_PostBI.Name = "rb_PostBI";
            this.rb_PostBI.Size = new System.Drawing.Size(84, 23);
            this.rb_PostBI.TabIndex = 40;
            this.rb_PostBI.Text = "老化后";
            this.rb_PostBI.UseVisualStyleBackColor = true;
            this.rb_PostBI.CheckedChanged += new System.EventHandler(this.rb_PostBI_CheckedChanged);
            // 
            // rb_PreBI
            // 
            this.rb_PreBI.AutoSize = true;
            this.rb_PreBI.Checked = true;
            this.rb_PreBI.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb_PreBI.Location = new System.Drawing.Point(555, 16);
            this.rb_PreBI.Name = "rb_PreBI";
            this.rb_PreBI.Size = new System.Drawing.Size(84, 23);
            this.rb_PreBI.TabIndex = 39;
            this.rb_PreBI.TabStop = true;
            this.rb_PreBI.Text = "老化前";
            this.rb_PreBI.UseVisualStyleBackColor = true;
            // 
            // rb_GS
            // 
            this.rb_GS.AutoSize = true;
            this.rb_GS.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb_GS.Location = new System.Drawing.Point(465, 16);
            this.rb_GS.Name = "rb_GS";
            this.rb_GS.Size = new System.Drawing.Size(65, 23);
            this.rb_GS.TabIndex = 38;
            this.rb_GS.Text = "金样";
            this.rb_GS.UseVisualStyleBackColor = true;
            // 
            // label90
            // 
            this.label90.AutoSize = true;
            this.label90.Location = new System.Drawing.Point(686, 110);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(53, 12);
            this.label90.TabIndex = 35;
            this.label90.Text = "截止时间";
            // 
            // label89
            // 
            this.label89.AutoSize = true;
            this.label89.Location = new System.Drawing.Point(465, 110);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(53, 12);
            this.label89.TabIndex = 34;
            this.label89.Text = "开始时间";
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(689, 131);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowCheckBox = true;
            this.dtpTo.Size = new System.Drawing.Size(172, 21);
            this.dtpTo.TabIndex = 33;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(465, 131);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowCheckBox = true;
            this.dtpFrom.Size = new System.Drawing.Size(170, 21);
            this.dtpFrom.TabIndex = 32;
            // 
            // btn_GetTestStations
            // 
            this.btn_GetTestStations.Location = new System.Drawing.Point(344, 211);
            this.btn_GetTestStations.Name = "btn_GetTestStations";
            this.btn_GetTestStations.Size = new System.Drawing.Size(75, 27);
            this.btn_GetTestStations.TabIndex = 27;
            this.btn_GetTestStations.Text = "读取";
            this.btn_GetTestStations.UseVisualStyleBackColor = true;
            this.btn_GetTestStations.Click += new System.EventHandler(this.btn_GetTestStations_Click);
            // 
            // lbl_teststation
            // 
            this.lbl_teststation.AutoSize = true;
            this.lbl_teststation.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_teststation.Location = new System.Drawing.Point(21, 215);
            this.lbl_teststation.Name = "lbl_teststation";
            this.lbl_teststation.Size = new System.Drawing.Size(66, 19);
            this.lbl_teststation.TabIndex = 26;
            this.lbl_teststation.Text = "工位号";
            // 
            // cbo_TestStation
            // 
            this.cbo_TestStation.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbo_TestStation.FormattingEnabled = true;
            this.cbo_TestStation.Location = new System.Drawing.Point(148, 211);
            this.cbo_TestStation.Name = "cbo_TestStation";
            this.cbo_TestStation.Size = new System.Drawing.Size(167, 27);
            this.cbo_TestStation.TabIndex = 25;
            // 
            // tb_DataFile
            // 
            this.tb_DataFile.Enabled = false;
            this.tb_DataFile.Location = new System.Drawing.Point(468, 211);
            this.tb_DataFile.Name = "tb_DataFile";
            this.tb_DataFile.Size = new System.Drawing.Size(627, 21);
            this.tb_DataFile.TabIndex = 23;
            // 
            // btn_UploadData
            // 
            this.btn_UploadData.Location = new System.Drawing.Point(578, 170);
            this.btn_UploadData.Name = "btn_UploadData";
            this.btn_UploadData.Size = new System.Drawing.Size(92, 23);
            this.btn_UploadData.TabIndex = 22;
            this.btn_UploadData.Text = "上传数据";
            this.btn_UploadData.UseVisualStyleBackColor = true;
            this.btn_UploadData.Visible = false;
            this.btn_UploadData.Click += new System.EventHandler(this.btn_UploadData_Click);
            // 
            // btn_SelectData
            // 
            this.btn_SelectData.Location = new System.Drawing.Point(465, 170);
            this.btn_SelectData.Name = "btn_SelectData";
            this.btn_SelectData.Size = new System.Drawing.Size(94, 23);
            this.btn_SelectData.TabIndex = 21;
            this.btn_SelectData.Text = "选择数据文件";
            this.btn_SelectData.UseVisualStyleBackColor = true;
            this.btn_SelectData.Click += new System.EventHandler(this.btn_SelectData_Click);
            // 
            // btn_ExportData
            // 
            this.btn_ExportData.Location = new System.Drawing.Point(787, 59);
            this.btn_ExportData.Name = "btn_ExportData";
            this.btn_ExportData.Size = new System.Drawing.Size(93, 30);
            this.btn_ExportData.TabIndex = 20;
            this.btn_ExportData.Text = "导出";
            this.btn_ExportData.UseVisualStyleBackColor = true;
            this.btn_ExportData.Click += new System.EventHandler(this.btn_ExportData_Click);
            // 
            // cb_Fail
            // 
            this.cb_Fail.AutoSize = true;
            this.cb_Fail.Checked = true;
            this.cb_Fail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Fail.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_Fail.Location = new System.Drawing.Point(567, 66);
            this.cb_Fail.Name = "cb_Fail";
            this.cb_Fail.Size = new System.Drawing.Size(68, 23);
            this.cb_Fail.TabIndex = 19;
            this.cb_Fail.Text = "FAIL";
            this.cb_Fail.UseVisualStyleBackColor = true;
            // 
            // cb_Pass
            // 
            this.cb_Pass.AutoSize = true;
            this.cb_Pass.Checked = true;
            this.cb_Pass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Pass.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_Pass.Location = new System.Drawing.Point(465, 66);
            this.cb_Pass.Name = "cb_Pass";
            this.cb_Pass.Size = new System.Drawing.Size(68, 23);
            this.cb_Pass.TabIndex = 18;
            this.cb_Pass.Text = "PASS";
            this.cb_Pass.UseVisualStyleBackColor = true;
            // 
            // btn_GetChipID
            // 
            this.btn_GetChipID.Location = new System.Drawing.Point(344, 173);
            this.btn_GetChipID.Name = "btn_GetChipID";
            this.btn_GetChipID.Size = new System.Drawing.Size(75, 27);
            this.btn_GetChipID.TabIndex = 17;
            this.btn_GetChipID.Text = "读取";
            this.btn_GetChipID.UseVisualStyleBackColor = true;
            this.btn_GetChipID.Click += new System.EventHandler(this.btn_GetChipID_Click);
            // 
            // btn_GetCarrierID
            // 
            this.btn_GetCarrierID.Location = new System.Drawing.Point(344, 134);
            this.btn_GetCarrierID.Name = "btn_GetCarrierID";
            this.btn_GetCarrierID.Size = new System.Drawing.Size(75, 27);
            this.btn_GetCarrierID.TabIndex = 16;
            this.btn_GetCarrierID.Text = "读取";
            this.btn_GetCarrierID.UseVisualStyleBackColor = true;
            this.btn_GetCarrierID.Click += new System.EventHandler(this.btn_GetCarrierID_Click);
            // 
            // btn_GetPN
            // 
            this.btn_GetPN.Location = new System.Drawing.Point(344, 95);
            this.btn_GetPN.Name = "btn_GetPN";
            this.btn_GetPN.Size = new System.Drawing.Size(75, 27);
            this.btn_GetPN.TabIndex = 15;
            this.btn_GetPN.Text = "读取";
            this.btn_GetPN.UseVisualStyleBackColor = true;
            this.btn_GetPN.Click += new System.EventHandler(this.btn_GetPN_Click);
            // 
            // btn_GetSubOrder
            // 
            this.btn_GetSubOrder.Location = new System.Drawing.Point(344, 56);
            this.btn_GetSubOrder.Name = "btn_GetSubOrder";
            this.btn_GetSubOrder.Size = new System.Drawing.Size(75, 27);
            this.btn_GetSubOrder.TabIndex = 14;
            this.btn_GetSubOrder.Text = "读取";
            this.btn_GetSubOrder.UseVisualStyleBackColor = true;
            this.btn_GetSubOrder.Click += new System.EventHandler(this.btn_GetSubOrder_Click);
            // 
            // btn_QueryData
            // 
            this.btn_QueryData.Location = new System.Drawing.Point(665, 59);
            this.btn_QueryData.Name = "btn_QueryData";
            this.btn_QueryData.Size = new System.Drawing.Size(93, 30);
            this.btn_QueryData.TabIndex = 13;
            this.btn_QueryData.Text = "查询";
            this.btn_QueryData.UseVisualStyleBackColor = true;
            this.btn_QueryData.Click += new System.EventHandler(this.btn_QueryData_Click);
            // 
            // btn_GetWO
            // 
            this.btn_GetWO.Location = new System.Drawing.Point(344, 17);
            this.btn_GetWO.Name = "btn_GetWO";
            this.btn_GetWO.Size = new System.Drawing.Size(75, 27);
            this.btn_GetWO.TabIndex = 12;
            this.btn_GetWO.Text = "读取";
            this.btn_GetWO.UseVisualStyleBackColor = true;
            this.btn_GetWO.Click += new System.EventHandler(this.btn_GetWO_Click);
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label36.Location = new System.Drawing.Point(21, 177);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(66, 19);
            this.label36.TabIndex = 9;
            this.label36.Text = "管芯号";
            // 
            // cbo_ChipID
            // 
            this.cbo_ChipID.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbo_ChipID.FormattingEnabled = true;
            this.cbo_ChipID.Location = new System.Drawing.Point(148, 173);
            this.cbo_ChipID.Name = "cbo_ChipID";
            this.cbo_ChipID.Size = new System.Drawing.Size(167, 27);
            this.cbo_ChipID.TabIndex = 8;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label35.Location = new System.Drawing.Point(21, 137);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(66, 19);
            this.label35.TabIndex = 7;
            this.label35.Text = "夹具号";
            // 
            // cbo_CarrierID
            // 
            this.cbo_CarrierID.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbo_CarrierID.FormattingEnabled = true;
            this.cbo_CarrierID.Location = new System.Drawing.Point(148, 134);
            this.cbo_CarrierID.Name = "cbo_CarrierID";
            this.cbo_CarrierID.Size = new System.Drawing.Size(167, 27);
            this.cbo_CarrierID.TabIndex = 6;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label34.Location = new System.Drawing.Point(21, 98);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(66, 19);
            this.label34.TabIndex = 5;
            this.label34.Text = "物料号";
            // 
            // cbo_PartNumber
            // 
            this.cbo_PartNumber.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbo_PartNumber.FormattingEnabled = true;
            this.cbo_PartNumber.Location = new System.Drawing.Point(148, 95);
            this.cbo_PartNumber.Name = "cbo_PartNumber";
            this.cbo_PartNumber.Size = new System.Drawing.Size(167, 27);
            this.cbo_PartNumber.TabIndex = 4;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label33.Location = new System.Drawing.Point(21, 59);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(66, 19);
            this.label33.TabIndex = 3;
            this.label33.Text = "子工单";
            // 
            // cbo_SubOrder
            // 
            this.cbo_SubOrder.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbo_SubOrder.FormattingEnabled = true;
            this.cbo_SubOrder.Location = new System.Drawing.Point(148, 56);
            this.cbo_SubOrder.Name = "cbo_SubOrder";
            this.cbo_SubOrder.Size = new System.Drawing.Size(167, 27);
            this.cbo_SubOrder.TabIndex = 2;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label32.Location = new System.Drawing.Point(21, 21);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(66, 19);
            this.label32.TabIndex = 1;
            this.label32.Text = "工单号";
            // 
            // cbo_WorkOrder
            // 
            this.cbo_WorkOrder.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbo_WorkOrder.FormattingEnabled = true;
            this.cbo_WorkOrder.Location = new System.Drawing.Point(148, 17);
            this.cbo_WorkOrder.Name = "cbo_WorkOrder";
            this.cbo_WorkOrder.Size = new System.Drawing.Size(167, 27);
            this.cbo_WorkOrder.TabIndex = 0;
            // 
            // panel10
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.panel10, 2);
            this.panel10.Controls.Add(this.dgv_TestData);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(3, 304);
            this.panel10.Name = "panel10";
            this.tableLayoutPanel5.SetRowSpan(this.panel10, 2);
            this.panel10.Size = new System.Drawing.Size(1319, 499);
            this.panel10.TabIndex = 1;
            // 
            // dgv_TestData
            // 
            this.dgv_TestData.AllowUserToAddRows = false;
            this.dgv_TestData.AllowUserToDeleteRows = false;
            this.dgv_TestData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_TestData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_TestData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_TestData.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgv_TestData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_TestData.Location = new System.Drawing.Point(0, 0);
            this.dgv_TestData.Name = "dgv_TestData";
            this.dgv_TestData.ReadOnly = true;
            this.dgv_TestData.RowHeadersVisible = false;
            this.dgv_TestData.RowHeadersWidth = 50;
            this.dgv_TestData.RowTemplate.Height = 23;
            this.dgv_TestData.Size = new System.Drawing.Size(1319, 499);
            this.dgv_TestData.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 336);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 12);
            this.label2.TabIndex = 4;
            // 
            // tabPage视频
            // 
            this.tabPage视频.Controls.Add(this.panel_CV);
            this.tabPage视频.Location = new System.Drawing.Point(4, 42);
            this.tabPage视频.Name = "tabPage视频";
            this.tabPage视频.Size = new System.Drawing.Size(1331, 812);
            this.tabPage视频.TabIndex = 9;
            this.tabPage视频.Text = "视频";
            this.tabPage视频.UseVisualStyleBackColor = true;
            // 
            // panel_CV
            // 
            this.panel_CV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_CV.Location = new System.Drawing.Point(0, 0);
            this.panel_CV.Name = "panel_CV";
            this.panel_CV.Size = new System.Drawing.Size(1331, 812);
            this.panel_CV.TabIndex = 1;
            // 
            // tabPage仪表
            // 
            this.tabPage仪表.Controls.Add(this.panel2);
            this.tabPage仪表.Location = new System.Drawing.Point(4, 42);
            this.tabPage仪表.Name = "tabPage仪表";
            this.tabPage仪表.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage仪表.Size = new System.Drawing.Size(1331, 812);
            this.tabPage仪表.TabIndex = 18;
            this.tabPage仪表.Text = "仪表";
            this.tabPage仪表.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox8);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.label47);
            this.panel2.Controls.Add(this.tb_liv_pdcompCurr);
            this.panel2.Controls.Add(this.label49);
            this.panel2.Controls.Add(this.tb_liv_pdbiasV);
            this.panel2.Controls.Add(this.label50);
            this.panel2.Controls.Add(this.tb_liv_compV);
            this.panel2.Controls.Add(this.label46);
            this.panel2.Controls.Add(this.tb_liv_end);
            this.panel2.Controls.Add(this.label45);
            this.panel2.Controls.Add(this.tb_liv_step);
            this.panel2.Controls.Add(this.label44);
            this.panel2.Controls.Add(this.tb_liv_start);
            this.panel2.Controls.Add(this.bt_LIV);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1325, 806);
            this.panel2.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.bt_TC_start);
            this.groupBox7.Controls.Add(this.bt_TC_stop);
            this.groupBox7.Location = new System.Drawing.Point(6, 133);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(248, 56);
            this.groupBox7.TabIndex = 98;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "平台";
            // 
            // bt_TC_start
            // 
            this.bt_TC_start.Location = new System.Drawing.Point(14, 20);
            this.bt_TC_start.Name = "bt_TC_start";
            this.bt_TC_start.Size = new System.Drawing.Size(75, 23);
            this.bt_TC_start.TabIndex = 90;
            this.bt_TC_start.Text = "开始控温";
            this.bt_TC_start.UseVisualStyleBackColor = true;
            this.bt_TC_start.Click += new System.EventHandler(this.bt_TC_start_Click);
            // 
            // bt_TC_stop
            // 
            this.bt_TC_stop.Location = new System.Drawing.Point(122, 20);
            this.bt_TC_stop.Name = "bt_TC_stop";
            this.bt_TC_stop.Size = new System.Drawing.Size(75, 23);
            this.bt_TC_stop.TabIndex = 92;
            this.bt_TC_stop.Text = "停止控温";
            this.bt_TC_stop.UseVisualStyleBackColor = true;
            this.bt_TC_stop.Click += new System.EventHandler(this.bt_TC_stop_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.bt_ted_start);
            this.groupBox6.Controls.Add(this.bt_ted_stop);
            this.groupBox6.Controls.Add(this.bt_gettemp);
            this.groupBox6.Controls.Add(this.lab_ted);
            this.groupBox6.Location = new System.Drawing.Point(6, 43);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(248, 84);
            this.groupBox6.TabIndex = 98;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "TED4015";
            // 
            // bt_ted_start
            // 
            this.bt_ted_start.Location = new System.Drawing.Point(14, 20);
            this.bt_ted_start.Name = "bt_ted_start";
            this.bt_ted_start.Size = new System.Drawing.Size(75, 23);
            this.bt_ted_start.TabIndex = 90;
            this.bt_ted_start.Text = "开始控温";
            this.bt_ted_start.UseVisualStyleBackColor = true;
            this.bt_ted_start.Click += new System.EventHandler(this.bt_ted_start_Click);
            // 
            // bt_ted_stop
            // 
            this.bt_ted_stop.Location = new System.Drawing.Point(122, 20);
            this.bt_ted_stop.Name = "bt_ted_stop";
            this.bt_ted_stop.Size = new System.Drawing.Size(75, 23);
            this.bt_ted_stop.TabIndex = 92;
            this.bt_ted_stop.Text = "停止控温";
            this.bt_ted_stop.UseVisualStyleBackColor = true;
            this.bt_ted_stop.Click += new System.EventHandler(this.bt_ted_stop_Click);
            // 
            // bt_gettemp
            // 
            this.bt_gettemp.Location = new System.Drawing.Point(122, 54);
            this.bt_gettemp.Name = "bt_gettemp";
            this.bt_gettemp.Size = new System.Drawing.Size(75, 23);
            this.bt_gettemp.TabIndex = 93;
            this.bt_gettemp.Text = "读取";
            this.bt_gettemp.UseVisualStyleBackColor = true;
            this.bt_gettemp.Click += new System.EventHandler(this.bt_gettemp_Click);
            // 
            // lab_ted
            // 
            this.lab_ted.AutoSize = true;
            this.lab_ted.Location = new System.Drawing.Point(51, 59);
            this.lab_ted.Name = "lab_ted";
            this.lab_ted.Size = new System.Drawing.Size(53, 12);
            this.lab_ted.TabIndex = 94;
            this.lab_ted.Text = "********";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label63);
            this.groupBox3.Controls.Add(this.label56);
            this.groupBox3.Controls.Add(this.txt_OpticalChannel);
            this.groupBox3.Controls.Add(this.btn_SwitchCh);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(966, 592);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(260, 145);
            this.groupBox3.TabIndex = 97;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "OSwitch";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label56.Location = new System.Drawing.Point(25, 106);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(37, 20);
            this.label56.TabIndex = 55;
            this.label56.Text = "通道";
            // 
            // txt_OpticalChannel
            // 
            this.txt_OpticalChannel.Location = new System.Drawing.Point(82, 108);
            this.txt_OpticalChannel.Name = "txt_OpticalChannel";
            this.txt_OpticalChannel.Size = new System.Drawing.Size(69, 21);
            this.txt_OpticalChannel.TabIndex = 50;
            this.txt_OpticalChannel.Text = "1";
            // 
            // btn_SwitchCh
            // 
            this.btn_SwitchCh.Location = new System.Drawing.Point(157, 103);
            this.btn_SwitchCh.Name = "btn_SwitchCh";
            this.btn_SwitchCh.Size = new System.Drawing.Size(84, 28);
            this.btn_SwitchCh.TabIndex = 49;
            this.btn_SwitchCh.Text = "切换";
            this.btn_SwitchCh.UseVisualStyleBackColor = true;
            this.btn_SwitchCh.Click += new System.EventHandler(this.btn_SwitchCh_Click);
            // 
            // rb_right
            // 
            this.rb_right.AutoSize = true;
            this.rb_right.Location = new System.Drawing.Point(77, 21);
            this.rb_right.Name = "rb_right";
            this.rb_right.Size = new System.Drawing.Size(59, 16);
            this.rb_right.TabIndex = 96;
            this.rb_right.Text = "右载台";
            this.rb_right.UseVisualStyleBackColor = true;
            // 
            // rb_left
            // 
            this.rb_left.AutoSize = true;
            this.rb_left.Checked = true;
            this.rb_left.Location = new System.Drawing.Point(6, 20);
            this.rb_left.Name = "rb_left";
            this.rb_left.Size = new System.Drawing.Size(59, 16);
            this.rb_left.TabIndex = 95;
            this.rb_left.TabStop = true;
            this.rb_left.Text = "左载台";
            this.rb_left.UseVisualStyleBackColor = true;
            // 
            // tb_temp
            // 
            this.tb_temp.Location = new System.Drawing.Point(190, 20);
            this.tb_temp.Name = "tb_temp";
            this.tb_temp.Size = new System.Drawing.Size(64, 21);
            this.tb_temp.TabIndex = 91;
            this.tb_temp.Text = "25";
            this.tb_temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "GAIN",
            "SOA1"});
            this.comboBox1.Location = new System.Drawing.Point(1000, 249);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 89;
            this.comboBox1.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1031, 290);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 88;
            this.button1.Text = "测试";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(155, 24);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(29, 12);
            this.label62.TabIndex = 87;
            this.label62.Text = "温度";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(998, 199);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(137, 12);
            this.label47.TabIndex = 87;
            this.label47.Text = "pdComplianceCurrent_mA";
            this.label47.Visible = false;
            // 
            // tb_liv_pdcompCurr
            // 
            this.tb_liv_pdcompCurr.Location = new System.Drawing.Point(1151, 196);
            this.tb_liv_pdcompCurr.Name = "tb_liv_pdcompCurr";
            this.tb_liv_pdcompCurr.Size = new System.Drawing.Size(64, 21);
            this.tb_liv_pdcompCurr.TabIndex = 86;
            this.tb_liv_pdcompCurr.Text = "0.001";
            this.tb_liv_pdcompCurr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_liv_pdcompCurr.Visible = false;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(1040, 163);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(95, 12);
            this.label49.TabIndex = 85;
            this.label49.Text = "pdBiasVoltage_V";
            this.label49.Visible = false;
            // 
            // tb_liv_pdbiasV
            // 
            this.tb_liv_pdbiasV.Location = new System.Drawing.Point(1151, 160);
            this.tb_liv_pdbiasV.Name = "tb_liv_pdbiasV";
            this.tb_liv_pdbiasV.Size = new System.Drawing.Size(64, 21);
            this.tb_liv_pdbiasV.TabIndex = 84;
            this.tb_liv_pdbiasV.Text = "0";
            this.tb_liv_pdbiasV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_liv_pdbiasV.Visible = false;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(1016, 129);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(119, 12);
            this.label50.TabIndex = 83;
            this.label50.Text = "ComplianceVoltage_V";
            this.label50.Visible = false;
            // 
            // tb_liv_compV
            // 
            this.tb_liv_compV.Location = new System.Drawing.Point(1151, 126);
            this.tb_liv_compV.Name = "tb_liv_compV";
            this.tb_liv_compV.Size = new System.Drawing.Size(64, 21);
            this.tb_liv_compV.TabIndex = 82;
            this.tb_liv_compV.Text = "2.0";
            this.tb_liv_compV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_liv_compV.Visible = false;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(1067, 88);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(41, 12);
            this.label46.TabIndex = 81;
            this.label46.Text = "End_mA";
            this.label46.Visible = false;
            // 
            // tb_liv_end
            // 
            this.tb_liv_end.Location = new System.Drawing.Point(1151, 85);
            this.tb_liv_end.Name = "tb_liv_end";
            this.tb_liv_end.Size = new System.Drawing.Size(64, 21);
            this.tb_liv_end.TabIndex = 80;
            this.tb_liv_end.Text = "10.0";
            this.tb_liv_end.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_liv_end.Visible = false;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(1061, 52);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(47, 12);
            this.label45.TabIndex = 79;
            this.label45.Text = "Step_mA";
            this.label45.Visible = false;
            // 
            // tb_liv_step
            // 
            this.tb_liv_step.Location = new System.Drawing.Point(1151, 49);
            this.tb_liv_step.Name = "tb_liv_step";
            this.tb_liv_step.Size = new System.Drawing.Size(64, 21);
            this.tb_liv_step.TabIndex = 78;
            this.tb_liv_step.Text = "0.1";
            this.tb_liv_step.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_liv_step.Visible = false;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(1055, 18);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(53, 12);
            this.label44.TabIndex = 77;
            this.label44.Text = "Start_mA";
            this.label44.Visible = false;
            // 
            // tb_liv_start
            // 
            this.tb_liv_start.Location = new System.Drawing.Point(1151, 15);
            this.tb_liv_start.Name = "tb_liv_start";
            this.tb_liv_start.Size = new System.Drawing.Size(64, 21);
            this.tb_liv_start.TabIndex = 76;
            this.tb_liv_start.Text = "0.0";
            this.tb_liv_start.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_liv_start.Visible = false;
            // 
            // bt_LIV
            // 
            this.bt_LIV.Location = new System.Drawing.Point(1151, 247);
            this.bt_LIV.Name = "bt_LIV";
            this.bt_LIV.Size = new System.Drawing.Size(75, 23);
            this.bt_LIV.TabIndex = 75;
            this.bt_LIV.Text = "开始";
            this.bt_LIV.UseVisualStyleBackColor = true;
            this.bt_LIV.Visible = false;
            this.bt_LIV.Click += new System.EventHandler(this.bt_LIV_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label55);
            this.groupBox2.Controls.Add(this.tb_wave);
            this.groupBox2.Controls.Add(this.label54);
            this.groupBox2.Controls.Add(this.tb_PD_I);
            this.groupBox2.Controls.Add(this.lab_GetPD_V);
            this.groupBox2.Controls.Add(this.bt_GetPD);
            this.groupBox2.Controls.Add(this.lab_GetPD_I);
            this.groupBox2.Controls.Add(this.bt_PD_OFF);
            this.groupBox2.Controls.Add(this.bt_PD_ON);
            this.groupBox2.Controls.Add(this.label53);
            this.groupBox2.Controls.Add(this.lab_GetMPD2_V);
            this.groupBox2.Controls.Add(this.lab_GetMPD1_V);
            this.groupBox2.Controls.Add(this.lab_GetBIAS2_V);
            this.groupBox2.Controls.Add(this.lab_GetBIAS1_V);
            this.groupBox2.Controls.Add(this.label57);
            this.groupBox2.Controls.Add(this.label52);
            this.groupBox2.Controls.Add(this.bt_GetMPD2);
            this.groupBox2.Controls.Add(this.bt_GetMPD1);
            this.groupBox2.Controls.Add(this.bt_GetBIAS2);
            this.groupBox2.Controls.Add(this.bt_GetBIAS1);
            this.groupBox2.Controls.Add(this.lab_GetMPD2_I);
            this.groupBox2.Controls.Add(this.lab_GetMPD1_I);
            this.groupBox2.Controls.Add(this.lab_GetBIAS2_I);
            this.groupBox2.Controls.Add(this.lab_GetBIAS1_I);
            this.groupBox2.Controls.Add(this.label58);
            this.groupBox2.Controls.Add(this.label48);
            this.groupBox2.Controls.Add(this.label42);
            this.groupBox2.Controls.Add(this.label41);
            this.groupBox2.Controls.Add(this.label40);
            this.groupBox2.Controls.Add(this.label39);
            this.groupBox2.Controls.Add(this.label38);
            this.groupBox2.Controls.Add(this.label37);
            this.groupBox2.Controls.Add(this.label31);
            this.groupBox2.Controls.Add(this.label30);
            this.groupBox2.Controls.Add(this.bt_MPD2_OFF);
            this.groupBox2.Controls.Add(this.bt_MPD2_ON);
            this.groupBox2.Controls.Add(this.tb_MPD2_I);
            this.groupBox2.Controls.Add(this.tb_MPD2_V);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.bt_MPD1_OFF);
            this.groupBox2.Controls.Add(this.bt_MPD1_ON);
            this.groupBox2.Controls.Add(this.tb_MPD1_I);
            this.groupBox2.Controls.Add(this.tb_MPD1_V);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.bt_Bias2_OFF);
            this.groupBox2.Controls.Add(this.bt_Bias2_ON);
            this.groupBox2.Controls.Add(this.tb_Bias2_I);
            this.groupBox2.Controls.Add(this.tb_Bias2_V);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.bt_Bias1_OFF);
            this.groupBox2.Controls.Add(this.bt_Bias1_ON);
            this.groupBox2.Controls.Add(this.tb_Bias1_I);
            this.groupBox2.Controls.Add(this.tb_Bias1_V);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Location = new System.Drawing.Point(4, 441);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(945, 296);
            this.groupBox2.TabIndex = 43;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "电压源";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(97, 217);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(17, 12);
            this.label55.TabIndex = 94;
            this.label55.Text = "WL";
            // 
            // tb_wave
            // 
            this.tb_wave.Location = new System.Drawing.Point(99, 232);
            this.tb_wave.Name = "tb_wave";
            this.tb_wave.Size = new System.Drawing.Size(100, 21);
            this.tb_wave.TabIndex = 93;
            this.tb_wave.Text = "1550";
            this.tb_wave.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(271, 214);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(29, 12);
            this.label54.TabIndex = 92;
            this.label54.Text = "Rang";
            // 
            // tb_PD_I
            // 
            this.tb_PD_I.Location = new System.Drawing.Point(273, 229);
            this.tb_PD_I.Name = "tb_PD_I";
            this.tb_PD_I.Size = new System.Drawing.Size(100, 21);
            this.tb_PD_I.TabIndex = 91;
            this.tb_PD_I.Text = "2";
            this.tb_PD_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lab_GetPD_V
            // 
            this.lab_GetPD_V.AutoSize = true;
            this.lab_GetPD_V.Location = new System.Drawing.Point(688, 232);
            this.lab_GetPD_V.Name = "lab_GetPD_V";
            this.lab_GetPD_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetPD_V.TabIndex = 90;
            this.lab_GetPD_V.Text = "********";
            // 
            // bt_GetPD
            // 
            this.bt_GetPD.Location = new System.Drawing.Point(843, 227);
            this.bt_GetPD.Name = "bt_GetPD";
            this.bt_GetPD.Size = new System.Drawing.Size(75, 23);
            this.bt_GetPD.TabIndex = 89;
            this.bt_GetPD.Text = "回读";
            this.bt_GetPD.UseVisualStyleBackColor = true;
            this.bt_GetPD.Click += new System.EventHandler(this.bt_GetPD_Click);
            // 
            // lab_GetPD_I
            // 
            this.lab_GetPD_I.AutoSize = true;
            this.lab_GetPD_I.Location = new System.Drawing.Point(773, 232);
            this.lab_GetPD_I.Name = "lab_GetPD_I";
            this.lab_GetPD_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetPD_I.TabIndex = 88;
            this.lab_GetPD_I.Text = "********";
            // 
            // bt_PD_OFF
            // 
            this.bt_PD_OFF.Location = new System.Drawing.Point(561, 227);
            this.bt_PD_OFF.Name = "bt_PD_OFF";
            this.bt_PD_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_PD_OFF.TabIndex = 87;
            this.bt_PD_OFF.Text = "OFF";
            this.bt_PD_OFF.UseVisualStyleBackColor = true;
            this.bt_PD_OFF.Click += new System.EventHandler(this.bt_PD_OFF_Click);
            // 
            // bt_PD_ON
            // 
            this.bt_PD_ON.Location = new System.Drawing.Point(443, 227);
            this.bt_PD_ON.Name = "bt_PD_ON";
            this.bt_PD_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_PD_ON.TabIndex = 86;
            this.bt_PD_ON.Text = "ON";
            this.bt_PD_ON.UseVisualStyleBackColor = true;
            this.bt_PD_ON.Click += new System.EventHandler(this.bt_PD_ON_Click);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(22, 232);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(17, 12);
            this.label53.TabIndex = 85;
            this.label53.Text = "PD";
            // 
            // lab_GetMPD2_V
            // 
            this.lab_GetMPD2_V.AutoSize = true;
            this.lab_GetMPD2_V.Location = new System.Drawing.Point(682, 176);
            this.lab_GetMPD2_V.Name = "lab_GetMPD2_V";
            this.lab_GetMPD2_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetMPD2_V.TabIndex = 84;
            this.lab_GetMPD2_V.Text = "********";
            // 
            // lab_GetMPD1_V
            // 
            this.lab_GetMPD1_V.AutoSize = true;
            this.lab_GetMPD1_V.Location = new System.Drawing.Point(682, 132);
            this.lab_GetMPD1_V.Name = "lab_GetMPD1_V";
            this.lab_GetMPD1_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetMPD1_V.TabIndex = 83;
            this.lab_GetMPD1_V.Text = "********";
            // 
            // lab_GetBIAS2_V
            // 
            this.lab_GetBIAS2_V.AutoSize = true;
            this.lab_GetBIAS2_V.Location = new System.Drawing.Point(682, 88);
            this.lab_GetBIAS2_V.Name = "lab_GetBIAS2_V";
            this.lab_GetBIAS2_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetBIAS2_V.TabIndex = 82;
            this.lab_GetBIAS2_V.Text = "********";
            // 
            // lab_GetBIAS1_V
            // 
            this.lab_GetBIAS1_V.AutoSize = true;
            this.lab_GetBIAS1_V.Location = new System.Drawing.Point(682, 51);
            this.lab_GetBIAS1_V.Name = "lab_GetBIAS1_V";
            this.lab_GetBIAS1_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetBIAS1_V.TabIndex = 81;
            this.lab_GetBIAS1_V.Text = "********";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(688, 211);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(53, 12);
            this.label57.TabIndex = 80;
            this.label57.Text = "功率(mW)";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(688, 17);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(47, 12);
            this.label52.TabIndex = 80;
            this.label52.Text = "电压(v)";
            // 
            // bt_GetMPD2
            // 
            this.bt_GetMPD2.Location = new System.Drawing.Point(843, 171);
            this.bt_GetMPD2.Name = "bt_GetMPD2";
            this.bt_GetMPD2.Size = new System.Drawing.Size(75, 23);
            this.bt_GetMPD2.TabIndex = 79;
            this.bt_GetMPD2.Text = "回读";
            this.bt_GetMPD2.UseVisualStyleBackColor = true;
            this.bt_GetMPD2.Click += new System.EventHandler(this.bt_GetMPD2_Click);
            // 
            // bt_GetMPD1
            // 
            this.bt_GetMPD1.Location = new System.Drawing.Point(843, 128);
            this.bt_GetMPD1.Name = "bt_GetMPD1";
            this.bt_GetMPD1.Size = new System.Drawing.Size(75, 23);
            this.bt_GetMPD1.TabIndex = 78;
            this.bt_GetMPD1.Text = "回读";
            this.bt_GetMPD1.UseVisualStyleBackColor = true;
            this.bt_GetMPD1.Click += new System.EventHandler(this.bt_GetMPD1_Click);
            // 
            // bt_GetBIAS2
            // 
            this.bt_GetBIAS2.Location = new System.Drawing.Point(843, 84);
            this.bt_GetBIAS2.Name = "bt_GetBIAS2";
            this.bt_GetBIAS2.Size = new System.Drawing.Size(75, 23);
            this.bt_GetBIAS2.TabIndex = 77;
            this.bt_GetBIAS2.Text = "回读";
            this.bt_GetBIAS2.UseVisualStyleBackColor = true;
            this.bt_GetBIAS2.Click += new System.EventHandler(this.bt_GetBIAS2_Click);
            // 
            // bt_GetBIAS1
            // 
            this.bt_GetBIAS1.Location = new System.Drawing.Point(843, 46);
            this.bt_GetBIAS1.Name = "bt_GetBIAS1";
            this.bt_GetBIAS1.Size = new System.Drawing.Size(75, 23);
            this.bt_GetBIAS1.TabIndex = 76;
            this.bt_GetBIAS1.Text = "回读";
            this.bt_GetBIAS1.UseVisualStyleBackColor = true;
            this.bt_GetBIAS1.Click += new System.EventHandler(this.bt_GetBIAS1_Click);
            // 
            // lab_GetMPD2_I
            // 
            this.lab_GetMPD2_I.AutoSize = true;
            this.lab_GetMPD2_I.Location = new System.Drawing.Point(773, 176);
            this.lab_GetMPD2_I.Name = "lab_GetMPD2_I";
            this.lab_GetMPD2_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetMPD2_I.TabIndex = 75;
            this.lab_GetMPD2_I.Text = "********";
            // 
            // lab_GetMPD1_I
            // 
            this.lab_GetMPD1_I.AutoSize = true;
            this.lab_GetMPD1_I.Location = new System.Drawing.Point(773, 132);
            this.lab_GetMPD1_I.Name = "lab_GetMPD1_I";
            this.lab_GetMPD1_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetMPD1_I.TabIndex = 74;
            this.lab_GetMPD1_I.Text = "********";
            // 
            // lab_GetBIAS2_I
            // 
            this.lab_GetBIAS2_I.AutoSize = true;
            this.lab_GetBIAS2_I.Location = new System.Drawing.Point(773, 88);
            this.lab_GetBIAS2_I.Name = "lab_GetBIAS2_I";
            this.lab_GetBIAS2_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetBIAS2_I.TabIndex = 73;
            this.lab_GetBIAS2_I.Text = "********";
            // 
            // lab_GetBIAS1_I
            // 
            this.lab_GetBIAS1_I.AutoSize = true;
            this.lab_GetBIAS1_I.Location = new System.Drawing.Point(773, 51);
            this.lab_GetBIAS1_I.Name = "lab_GetBIAS1_I";
            this.lab_GetBIAS1_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetBIAS1_I.TabIndex = 72;
            this.lab_GetBIAS1_I.Text = "********";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(773, 211);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(53, 12);
            this.label58.TabIndex = 71;
            this.label58.Text = "电流(mA)";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(773, 17);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(53, 12);
            this.label48.TabIndex = 71;
            this.label48.Text = "电流(mA)";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(379, 177);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(41, 12);
            this.label42.TabIndex = 59;
            this.label42.Text = "(-0.1)";
            this.label42.Visible = false;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(379, 132);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(35, 12);
            this.label41.TabIndex = 58;
            this.label41.Text = "(-10)";
            this.label41.Visible = false;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(379, 89);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(35, 12);
            this.label40.TabIndex = 57;
            this.label40.Text = "(-10)";
            this.label40.Visible = false;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(379, 52);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(35, 12);
            this.label39.TabIndex = 56;
            this.label39.Text = "(-10)";
            this.label39.Visible = false;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(205, 177);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(29, 12);
            this.label38.TabIndex = 55;
            this.label38.Text = "(-5)";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(205, 133);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(29, 12);
            this.label37.TabIndex = 54;
            this.label37.Text = "(-5)";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(205, 90);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(29, 12);
            this.label31.TabIndex = 53;
            this.label31.Text = "(-7)";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(205, 52);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(29, 12);
            this.label30.TabIndex = 52;
            this.label30.Text = "(-7)";
            // 
            // bt_MPD2_OFF
            // 
            this.bt_MPD2_OFF.Location = new System.Drawing.Point(561, 171);
            this.bt_MPD2_OFF.Name = "bt_MPD2_OFF";
            this.bt_MPD2_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_MPD2_OFF.TabIndex = 51;
            this.bt_MPD2_OFF.Text = "OFF";
            this.bt_MPD2_OFF.UseVisualStyleBackColor = true;
            this.bt_MPD2_OFF.Click += new System.EventHandler(this.bt_MPD2_OFF_Click);
            // 
            // bt_MPD2_ON
            // 
            this.bt_MPD2_ON.Location = new System.Drawing.Point(443, 171);
            this.bt_MPD2_ON.Name = "bt_MPD2_ON";
            this.bt_MPD2_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_MPD2_ON.TabIndex = 50;
            this.bt_MPD2_ON.Text = "ON";
            this.bt_MPD2_ON.UseVisualStyleBackColor = true;
            this.bt_MPD2_ON.Click += new System.EventHandler(this.bt_MPD2_ON_Click);
            // 
            // tb_MPD2_I
            // 
            this.tb_MPD2_I.Location = new System.Drawing.Point(273, 173);
            this.tb_MPD2_I.Name = "tb_MPD2_I";
            this.tb_MPD2_I.Size = new System.Drawing.Size(100, 21);
            this.tb_MPD2_I.TabIndex = 49;
            this.tb_MPD2_I.Text = "-0.1";
            this.tb_MPD2_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_MPD2_V
            // 
            this.tb_MPD2_V.Location = new System.Drawing.Point(99, 174);
            this.tb_MPD2_V.Name = "tb_MPD2_V";
            this.tb_MPD2_V.Size = new System.Drawing.Size(100, 21);
            this.tb_MPD2_V.TabIndex = 48;
            this.tb_MPD2_V.Text = "0";
            this.tb_MPD2_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 182);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 47;
            this.label12.Text = "MPD2";
            // 
            // bt_MPD1_OFF
            // 
            this.bt_MPD1_OFF.Location = new System.Drawing.Point(561, 127);
            this.bt_MPD1_OFF.Name = "bt_MPD1_OFF";
            this.bt_MPD1_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_MPD1_OFF.TabIndex = 46;
            this.bt_MPD1_OFF.Text = "OFF";
            this.bt_MPD1_OFF.UseVisualStyleBackColor = true;
            this.bt_MPD1_OFF.Click += new System.EventHandler(this.bt_MPD1_OFF_Click);
            // 
            // bt_MPD1_ON
            // 
            this.bt_MPD1_ON.Location = new System.Drawing.Point(443, 127);
            this.bt_MPD1_ON.Name = "bt_MPD1_ON";
            this.bt_MPD1_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_MPD1_ON.TabIndex = 45;
            this.bt_MPD1_ON.Text = "ON";
            this.bt_MPD1_ON.UseVisualStyleBackColor = true;
            this.bt_MPD1_ON.Click += new System.EventHandler(this.bt_MPD1_ON_Click);
            // 
            // tb_MPD1_I
            // 
            this.tb_MPD1_I.Location = new System.Drawing.Point(273, 129);
            this.tb_MPD1_I.Name = "tb_MPD1_I";
            this.tb_MPD1_I.Size = new System.Drawing.Size(100, 21);
            this.tb_MPD1_I.TabIndex = 44;
            this.tb_MPD1_I.Text = "-10";
            this.tb_MPD1_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_MPD1_V
            // 
            this.tb_MPD1_V.Location = new System.Drawing.Point(99, 130);
            this.tb_MPD1_V.Name = "tb_MPD1_V";
            this.tb_MPD1_V.Size = new System.Drawing.Size(100, 21);
            this.tb_MPD1_V.TabIndex = 43;
            this.tb_MPD1_V.Text = "0";
            this.tb_MPD1_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(22, 138);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 42;
            this.label13.Text = "MPD1";
            // 
            // bt_Bias2_OFF
            // 
            this.bt_Bias2_OFF.Location = new System.Drawing.Point(561, 84);
            this.bt_Bias2_OFF.Name = "bt_Bias2_OFF";
            this.bt_Bias2_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_Bias2_OFF.TabIndex = 41;
            this.bt_Bias2_OFF.Text = "OFF";
            this.bt_Bias2_OFF.UseVisualStyleBackColor = true;
            this.bt_Bias2_OFF.Click += new System.EventHandler(this.bt_Bias2_OFF_Click);
            // 
            // bt_Bias2_ON
            // 
            this.bt_Bias2_ON.Location = new System.Drawing.Point(443, 84);
            this.bt_Bias2_ON.Name = "bt_Bias2_ON";
            this.bt_Bias2_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_Bias2_ON.TabIndex = 40;
            this.bt_Bias2_ON.Text = "ON";
            this.bt_Bias2_ON.UseVisualStyleBackColor = true;
            this.bt_Bias2_ON.Click += new System.EventHandler(this.bt_Bias2_ON_Click);
            // 
            // tb_Bias2_I
            // 
            this.tb_Bias2_I.Location = new System.Drawing.Point(273, 86);
            this.tb_Bias2_I.Name = "tb_Bias2_I";
            this.tb_Bias2_I.Size = new System.Drawing.Size(100, 21);
            this.tb_Bias2_I.TabIndex = 39;
            this.tb_Bias2_I.Text = "-10";
            this.tb_Bias2_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_Bias2_V
            // 
            this.tb_Bias2_V.Location = new System.Drawing.Point(99, 87);
            this.tb_Bias2_V.Name = "tb_Bias2_V";
            this.tb_Bias2_V.Size = new System.Drawing.Size(100, 21);
            this.tb_Bias2_V.TabIndex = 38;
            this.tb_Bias2_V.Text = "0";
            this.tb_Bias2_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(22, 95);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 37;
            this.label10.Text = "Bias2/MZM2";
            // 
            // bt_Bias1_OFF
            // 
            this.bt_Bias1_OFF.Location = new System.Drawing.Point(561, 46);
            this.bt_Bias1_OFF.Name = "bt_Bias1_OFF";
            this.bt_Bias1_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_Bias1_OFF.TabIndex = 36;
            this.bt_Bias1_OFF.Text = "OFF";
            this.bt_Bias1_OFF.UseVisualStyleBackColor = true;
            this.bt_Bias1_OFF.Click += new System.EventHandler(this.bt_Bias1_OFF_Click);
            // 
            // bt_Bias1_ON
            // 
            this.bt_Bias1_ON.Location = new System.Drawing.Point(443, 46);
            this.bt_Bias1_ON.Name = "bt_Bias1_ON";
            this.bt_Bias1_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_Bias1_ON.TabIndex = 35;
            this.bt_Bias1_ON.Text = "ON";
            this.bt_Bias1_ON.UseVisualStyleBackColor = true;
            this.bt_Bias1_ON.Click += new System.EventHandler(this.bt_Bias1_ON_Click);
            // 
            // tb_Bias1_I
            // 
            this.tb_Bias1_I.Location = new System.Drawing.Point(273, 48);
            this.tb_Bias1_I.Name = "tb_Bias1_I";
            this.tb_Bias1_I.Size = new System.Drawing.Size(100, 21);
            this.tb_Bias1_I.TabIndex = 34;
            this.tb_Bias1_I.Text = "-10";
            this.tb_Bias1_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_Bias1_V
            // 
            this.tb_Bias1_V.Location = new System.Drawing.Point(99, 49);
            this.tb_Bias1_V.Name = "tb_Bias1_V";
            this.tb_Bias1_V.Size = new System.Drawing.Size(100, 21);
            this.tb_Bias1_V.TabIndex = 33;
            this.tb_Bias1_V.Text = "0";
            this.tb_Bias1_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(22, 57);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 32;
            this.label11.Text = "Bias1/MZM1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bt_OFF_All);
            this.groupBox1.Controls.Add(this.lab_GetMIRROR2_I);
            this.groupBox1.Controls.Add(this.lab_GetMIRROR2_V);
            this.groupBox1.Controls.Add(this.lab_GetMIRROR1_I);
            this.groupBox1.Controls.Add(this.lab_GetMIRROR1_V);
            this.groupBox1.Controls.Add(this.lab_GetPH2_I);
            this.groupBox1.Controls.Add(this.lab_GetPH2_V);
            this.groupBox1.Controls.Add(this.lab_GetPH1_I);
            this.groupBox1.Controls.Add(this.lab_GetPH1_V);
            this.groupBox1.Controls.Add(this.lab_GetLP_I);
            this.groupBox1.Controls.Add(this.lab_GetLP_V);
            this.groupBox1.Controls.Add(this.lab_GetSOA2_I);
            this.groupBox1.Controls.Add(this.lab_GetSOA2_V);
            this.groupBox1.Controls.Add(this.lab_GetSOA1_I);
            this.groupBox1.Controls.Add(this.lab_GetSOA1_V);
            this.groupBox1.Controls.Add(this.lab_GetGAIN_I);
            this.groupBox1.Controls.Add(this.lab_GetGAIN_V);
            this.groupBox1.Controls.Add(this.label43);
            this.groupBox1.Controls.Add(this.bt_GetMIRROR2);
            this.groupBox1.Controls.Add(this.bt_GetMIRROR1);
            this.groupBox1.Controls.Add(this.bt_GetGAIN);
            this.groupBox1.Controls.Add(this.bt_GetPH2);
            this.groupBox1.Controls.Add(this.bt_GetSOA1);
            this.groupBox1.Controls.Add(this.bt_GetPH1);
            this.groupBox1.Controls.Add(this.bt_GetSOA2);
            this.groupBox1.Controls.Add(this.bt_GetLP);
            this.groupBox1.Controls.Add(this.label29);
            this.groupBox1.Controls.Add(this.label28);
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.bt_MIRROR2_OFF);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.bt_MIRROR2_ON);
            this.groupBox1.Controls.Add(this.label51);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tb_MIRROR2_I);
            this.groupBox1.Controls.Add(this.tb_GAIN_V);
            this.groupBox1.Controls.Add(this.tb_MIRROR2_V);
            this.groupBox1.Controls.Add(this.tb_GAIN_I);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.bt_GAIN_ON);
            this.groupBox1.Controls.Add(this.bt_MIRROR1_OFF);
            this.groupBox1.Controls.Add(this.bt_GAIN_OFF);
            this.groupBox1.Controls.Add(this.bt_MIRROR1_ON);
            this.groupBox1.Controls.Add(this.SOA1);
            this.groupBox1.Controls.Add(this.tb_MIRROR1_I);
            this.groupBox1.Controls.Add(this.tb_SOA1_V);
            this.groupBox1.Controls.Add(this.tb_MIRROR1_V);
            this.groupBox1.Controls.Add(this.tb_SOA1_I);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.bt_SOA1_ON);
            this.groupBox1.Controls.Add(this.bt_PH2_OFF);
            this.groupBox1.Controls.Add(this.bt_SOA1_OFF);
            this.groupBox1.Controls.Add(this.bt_PH2_ON);
            this.groupBox1.Controls.Add(this.SOA2);
            this.groupBox1.Controls.Add(this.tb_PH2_I);
            this.groupBox1.Controls.Add(this.tb_SOA2_V);
            this.groupBox1.Controls.Add(this.tb_PH2_V);
            this.groupBox1.Controls.Add(this.tb_SOA2_I);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.bt_SOA2_ON);
            this.groupBox1.Controls.Add(this.bt_PH1_OFF);
            this.groupBox1.Controls.Add(this.bt_SOA2_OFF);
            this.groupBox1.Controls.Add(this.bt_PH1_ON);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tb_PH1_I);
            this.groupBox1.Controls.Add(this.tb_LP_V);
            this.groupBox1.Controls.Add(this.tb_PH1_V);
            this.groupBox1.Controls.Add(this.tb_LP_I);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.bt_LP_ON);
            this.groupBox1.Controls.Add(this.bt_LP_OFF);
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(944, 431);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "电流源";
            // 
            // bt_OFF_All
            // 
            this.bt_OFF_All.Location = new System.Drawing.Point(560, 15);
            this.bt_OFF_All.Name = "bt_OFF_All";
            this.bt_OFF_All.Size = new System.Drawing.Size(75, 23);
            this.bt_OFF_All.TabIndex = 75;
            this.bt_OFF_All.Text = "OFF_All";
            this.bt_OFF_All.UseVisualStyleBackColor = true;
            this.bt_OFF_All.Click += new System.EventHandler(this.bt_OFF_All_Click);
            // 
            // lab_GetMIRROR2_I
            // 
            this.lab_GetMIRROR2_I.AutoSize = true;
            this.lab_GetMIRROR2_I.Location = new System.Drawing.Point(687, 384);
            this.lab_GetMIRROR2_I.Name = "lab_GetMIRROR2_I";
            this.lab_GetMIRROR2_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetMIRROR2_I.TabIndex = 74;
            this.lab_GetMIRROR2_I.Text = "********";
            // 
            // lab_GetMIRROR2_V
            // 
            this.lab_GetMIRROR2_V.AutoSize = true;
            this.lab_GetMIRROR2_V.Location = new System.Drawing.Point(772, 383);
            this.lab_GetMIRROR2_V.Name = "lab_GetMIRROR2_V";
            this.lab_GetMIRROR2_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetMIRROR2_V.TabIndex = 74;
            this.lab_GetMIRROR2_V.Text = "********";
            // 
            // lab_GetMIRROR1_I
            // 
            this.lab_GetMIRROR1_I.AutoSize = true;
            this.lab_GetMIRROR1_I.Location = new System.Drawing.Point(687, 334);
            this.lab_GetMIRROR1_I.Name = "lab_GetMIRROR1_I";
            this.lab_GetMIRROR1_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetMIRROR1_I.TabIndex = 73;
            this.lab_GetMIRROR1_I.Text = "********";
            // 
            // lab_GetMIRROR1_V
            // 
            this.lab_GetMIRROR1_V.AutoSize = true;
            this.lab_GetMIRROR1_V.Location = new System.Drawing.Point(772, 334);
            this.lab_GetMIRROR1_V.Name = "lab_GetMIRROR1_V";
            this.lab_GetMIRROR1_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetMIRROR1_V.TabIndex = 73;
            this.lab_GetMIRROR1_V.Text = "********";
            // 
            // lab_GetPH2_I
            // 
            this.lab_GetPH2_I.AutoSize = true;
            this.lab_GetPH2_I.Location = new System.Drawing.Point(687, 287);
            this.lab_GetPH2_I.Name = "lab_GetPH2_I";
            this.lab_GetPH2_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetPH2_I.TabIndex = 72;
            this.lab_GetPH2_I.Text = "********";
            // 
            // lab_GetPH2_V
            // 
            this.lab_GetPH2_V.AutoSize = true;
            this.lab_GetPH2_V.Location = new System.Drawing.Point(772, 287);
            this.lab_GetPH2_V.Name = "lab_GetPH2_V";
            this.lab_GetPH2_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetPH2_V.TabIndex = 72;
            this.lab_GetPH2_V.Text = "********";
            // 
            // lab_GetPH1_I
            // 
            this.lab_GetPH1_I.AutoSize = true;
            this.lab_GetPH1_I.Location = new System.Drawing.Point(687, 244);
            this.lab_GetPH1_I.Name = "lab_GetPH1_I";
            this.lab_GetPH1_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetPH1_I.TabIndex = 71;
            this.lab_GetPH1_I.Text = "********";
            // 
            // lab_GetPH1_V
            // 
            this.lab_GetPH1_V.AutoSize = true;
            this.lab_GetPH1_V.Location = new System.Drawing.Point(772, 243);
            this.lab_GetPH1_V.Name = "lab_GetPH1_V";
            this.lab_GetPH1_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetPH1_V.TabIndex = 71;
            this.lab_GetPH1_V.Text = "********";
            // 
            // lab_GetLP_I
            // 
            this.lab_GetLP_I.AutoSize = true;
            this.lab_GetLP_I.Location = new System.Drawing.Point(687, 199);
            this.lab_GetLP_I.Name = "lab_GetLP_I";
            this.lab_GetLP_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetLP_I.TabIndex = 70;
            this.lab_GetLP_I.Text = "********";
            // 
            // lab_GetLP_V
            // 
            this.lab_GetLP_V.AutoSize = true;
            this.lab_GetLP_V.Location = new System.Drawing.Point(772, 198);
            this.lab_GetLP_V.Name = "lab_GetLP_V";
            this.lab_GetLP_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetLP_V.TabIndex = 70;
            this.lab_GetLP_V.Text = "********";
            // 
            // lab_GetSOA2_I
            // 
            this.lab_GetSOA2_I.AutoSize = true;
            this.lab_GetSOA2_I.Location = new System.Drawing.Point(687, 155);
            this.lab_GetSOA2_I.Name = "lab_GetSOA2_I";
            this.lab_GetSOA2_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetSOA2_I.TabIndex = 69;
            this.lab_GetSOA2_I.Text = "********";
            // 
            // lab_GetSOA2_V
            // 
            this.lab_GetSOA2_V.AutoSize = true;
            this.lab_GetSOA2_V.Location = new System.Drawing.Point(772, 154);
            this.lab_GetSOA2_V.Name = "lab_GetSOA2_V";
            this.lab_GetSOA2_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetSOA2_V.TabIndex = 69;
            this.lab_GetSOA2_V.Text = "********";
            // 
            // lab_GetSOA1_I
            // 
            this.lab_GetSOA1_I.AutoSize = true;
            this.lab_GetSOA1_I.Location = new System.Drawing.Point(687, 106);
            this.lab_GetSOA1_I.Name = "lab_GetSOA1_I";
            this.lab_GetSOA1_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetSOA1_I.TabIndex = 68;
            this.lab_GetSOA1_I.Text = "********";
            // 
            // lab_GetSOA1_V
            // 
            this.lab_GetSOA1_V.AutoSize = true;
            this.lab_GetSOA1_V.Location = new System.Drawing.Point(772, 105);
            this.lab_GetSOA1_V.Name = "lab_GetSOA1_V";
            this.lab_GetSOA1_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetSOA1_V.TabIndex = 68;
            this.lab_GetSOA1_V.Text = "********";
            // 
            // lab_GetGAIN_I
            // 
            this.lab_GetGAIN_I.AutoSize = true;
            this.lab_GetGAIN_I.Location = new System.Drawing.Point(687, 57);
            this.lab_GetGAIN_I.Name = "lab_GetGAIN_I";
            this.lab_GetGAIN_I.Size = new System.Drawing.Size(53, 12);
            this.lab_GetGAIN_I.TabIndex = 67;
            this.lab_GetGAIN_I.Text = "********";
            // 
            // lab_GetGAIN_V
            // 
            this.lab_GetGAIN_V.AutoSize = true;
            this.lab_GetGAIN_V.Location = new System.Drawing.Point(772, 57);
            this.lab_GetGAIN_V.Name = "lab_GetGAIN_V";
            this.lab_GetGAIN_V.Size = new System.Drawing.Size(53, 12);
            this.lab_GetGAIN_V.TabIndex = 67;
            this.lab_GetGAIN_V.Text = "********";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(772, 24);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(47, 12);
            this.label43.TabIndex = 66;
            this.label43.Text = "电压(v)";
            // 
            // bt_GetMIRROR2
            // 
            this.bt_GetMIRROR2.Location = new System.Drawing.Point(842, 379);
            this.bt_GetMIRROR2.Name = "bt_GetMIRROR2";
            this.bt_GetMIRROR2.Size = new System.Drawing.Size(75, 23);
            this.bt_GetMIRROR2.TabIndex = 65;
            this.bt_GetMIRROR2.Text = "回读";
            this.bt_GetMIRROR2.UseVisualStyleBackColor = true;
            this.bt_GetMIRROR2.Click += new System.EventHandler(this.bt_GetMIRROR2_Click);
            // 
            // bt_GetMIRROR1
            // 
            this.bt_GetMIRROR1.Location = new System.Drawing.Point(842, 329);
            this.bt_GetMIRROR1.Name = "bt_GetMIRROR1";
            this.bt_GetMIRROR1.Size = new System.Drawing.Size(75, 23);
            this.bt_GetMIRROR1.TabIndex = 64;
            this.bt_GetMIRROR1.Text = "回读";
            this.bt_GetMIRROR1.UseVisualStyleBackColor = true;
            this.bt_GetMIRROR1.Click += new System.EventHandler(this.bt_GetMIRROR1_Click);
            // 
            // bt_GetGAIN
            // 
            this.bt_GetGAIN.Location = new System.Drawing.Point(842, 52);
            this.bt_GetGAIN.Name = "bt_GetGAIN";
            this.bt_GetGAIN.Size = new System.Drawing.Size(75, 23);
            this.bt_GetGAIN.TabIndex = 58;
            this.bt_GetGAIN.Text = "回读";
            this.bt_GetGAIN.UseVisualStyleBackColor = true;
            this.bt_GetGAIN.Click += new System.EventHandler(this.bt_GetGAIN_Click);
            // 
            // bt_GetPH2
            // 
            this.bt_GetPH2.Location = new System.Drawing.Point(842, 282);
            this.bt_GetPH2.Name = "bt_GetPH2";
            this.bt_GetPH2.Size = new System.Drawing.Size(75, 23);
            this.bt_GetPH2.TabIndex = 63;
            this.bt_GetPH2.Text = "回读";
            this.bt_GetPH2.UseVisualStyleBackColor = true;
            this.bt_GetPH2.Click += new System.EventHandler(this.bt_GetPH2_Click);
            // 
            // bt_GetSOA1
            // 
            this.bt_GetSOA1.Location = new System.Drawing.Point(842, 100);
            this.bt_GetSOA1.Name = "bt_GetSOA1";
            this.bt_GetSOA1.Size = new System.Drawing.Size(75, 23);
            this.bt_GetSOA1.TabIndex = 59;
            this.bt_GetSOA1.Text = "回读";
            this.bt_GetSOA1.UseVisualStyleBackColor = true;
            this.bt_GetSOA1.Click += new System.EventHandler(this.bt_GetSOA1_Click);
            // 
            // bt_GetPH1
            // 
            this.bt_GetPH1.Location = new System.Drawing.Point(842, 238);
            this.bt_GetPH1.Name = "bt_GetPH1";
            this.bt_GetPH1.Size = new System.Drawing.Size(75, 23);
            this.bt_GetPH1.TabIndex = 62;
            this.bt_GetPH1.Text = "回读";
            this.bt_GetPH1.UseVisualStyleBackColor = true;
            this.bt_GetPH1.Click += new System.EventHandler(this.bt_GetPH1_Click);
            // 
            // bt_GetSOA2
            // 
            this.bt_GetSOA2.Location = new System.Drawing.Point(842, 149);
            this.bt_GetSOA2.Name = "bt_GetSOA2";
            this.bt_GetSOA2.Size = new System.Drawing.Size(75, 23);
            this.bt_GetSOA2.TabIndex = 60;
            this.bt_GetSOA2.Text = "回读";
            this.bt_GetSOA2.UseVisualStyleBackColor = true;
            this.bt_GetSOA2.Click += new System.EventHandler(this.bt_GetSOA2_Click);
            // 
            // bt_GetLP
            // 
            this.bt_GetLP.Location = new System.Drawing.Point(842, 193);
            this.bt_GetLP.Name = "bt_GetLP";
            this.bt_GetLP.Size = new System.Drawing.Size(75, 23);
            this.bt_GetLP.TabIndex = 61;
            this.bt_GetLP.Text = "回读";
            this.bt_GetLP.UseVisualStyleBackColor = true;
            this.bt_GetLP.Click += new System.EventHandler(this.bt_GetLP_Click);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(378, 385);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(29, 12);
            this.label29.TabIndex = 57;
            this.label29.Text = "(65)";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(378, 335);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(29, 12);
            this.label28.TabIndex = 56;
            this.label28.Text = "(65)";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(378, 288);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(29, 12);
            this.label27.TabIndex = 55;
            this.label27.Text = "(20)";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(378, 244);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(29, 12);
            this.label26.TabIndex = 54;
            this.label26.Text = "(20)";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(378, 198);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(29, 12);
            this.label25.TabIndex = 53;
            this.label25.Text = "(20)";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(378, 154);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(35, 12);
            this.label24.TabIndex = 52;
            this.label24.Text = "(120)";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(378, 105);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(35, 12);
            this.label23.TabIndex = 51;
            this.label23.Text = "(120)";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(378, 57);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(35, 12);
            this.label22.TabIndex = 50;
            this.label22.Text = "(180)";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(204, 385);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(35, 12);
            this.label21.TabIndex = 49;
            this.label21.Text = "(1.6)";
            this.label21.Visible = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(204, 106);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(35, 12);
            this.label20.TabIndex = 48;
            this.label20.Text = "(1.6)";
            this.label20.Visible = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(204, 155);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(35, 12);
            this.label19.TabIndex = 47;
            this.label19.Text = "(1.6)";
            this.label19.Visible = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(204, 199);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 12);
            this.label18.TabIndex = 46;
            this.label18.Text = "(1.5)";
            this.label18.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(204, 244);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(35, 12);
            this.label17.TabIndex = 45;
            this.label17.Text = "(1.5)";
            this.label17.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(204, 288);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 12);
            this.label16.TabIndex = 44;
            this.label16.Text = "(1.5)";
            this.label16.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(204, 335);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 12);
            this.label15.TabIndex = 43;
            this.label15.Text = "(1.6)";
            this.label15.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(204, 58);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(23, 12);
            this.label14.TabIndex = 42;
            this.label14.Text = "(2)";
            this.label14.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(131, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "电压(v)";
            // 
            // bt_MIRROR2_OFF
            // 
            this.bt_MIRROR2_OFF.Location = new System.Drawing.Point(560, 379);
            this.bt_MIRROR2_OFF.Name = "bt_MIRROR2_OFF";
            this.bt_MIRROR2_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_MIRROR2_OFF.TabIndex = 41;
            this.bt_MIRROR2_OFF.Text = "OFF";
            this.bt_MIRROR2_OFF.UseVisualStyleBackColor = true;
            this.bt_MIRROR2_OFF.Click += new System.EventHandler(this.bt_MIRROR2_OFF_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "GAIN";
            // 
            // bt_MIRROR2_ON
            // 
            this.bt_MIRROR2_ON.Location = new System.Drawing.Point(442, 379);
            this.bt_MIRROR2_ON.Name = "bt_MIRROR2_ON";
            this.bt_MIRROR2_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_MIRROR2_ON.TabIndex = 40;
            this.bt_MIRROR2_ON.Text = "ON";
            this.bt_MIRROR2_ON.UseVisualStyleBackColor = true;
            this.bt_MIRROR2_ON.Click += new System.EventHandler(this.bt_MIRROR2_ON_Click);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(687, 24);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(53, 12);
            this.label51.TabIndex = 2;
            this.label51.Text = "电流(mA)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(304, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "电流(mA)";
            // 
            // tb_MIRROR2_I
            // 
            this.tb_MIRROR2_I.Location = new System.Drawing.Point(272, 381);
            this.tb_MIRROR2_I.Name = "tb_MIRROR2_I";
            this.tb_MIRROR2_I.Size = new System.Drawing.Size(100, 21);
            this.tb_MIRROR2_I.TabIndex = 39;
            this.tb_MIRROR2_I.Text = "0";
            this.tb_MIRROR2_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_GAIN_V
            // 
            this.tb_GAIN_V.Location = new System.Drawing.Point(98, 55);
            this.tb_GAIN_V.Name = "tb_GAIN_V";
            this.tb_GAIN_V.Size = new System.Drawing.Size(100, 21);
            this.tb_GAIN_V.TabIndex = 3;
            this.tb_GAIN_V.Text = "2";
            this.tb_GAIN_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_MIRROR2_V
            // 
            this.tb_MIRROR2_V.Location = new System.Drawing.Point(98, 382);
            this.tb_MIRROR2_V.Name = "tb_MIRROR2_V";
            this.tb_MIRROR2_V.Size = new System.Drawing.Size(100, 21);
            this.tb_MIRROR2_V.TabIndex = 38;
            this.tb_MIRROR2_V.Text = "1.6";
            this.tb_MIRROR2_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_GAIN_I
            // 
            this.tb_GAIN_I.Location = new System.Drawing.Point(272, 54);
            this.tb_GAIN_I.Name = "tb_GAIN_I";
            this.tb_GAIN_I.Size = new System.Drawing.Size(100, 21);
            this.tb_GAIN_I.TabIndex = 4;
            this.tb_GAIN_I.Text = "0";
            this.tb_GAIN_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 390);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 37;
            this.label9.Text = "MIRROR2";
            // 
            // bt_GAIN_ON
            // 
            this.bt_GAIN_ON.Location = new System.Drawing.Point(442, 52);
            this.bt_GAIN_ON.Name = "bt_GAIN_ON";
            this.bt_GAIN_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_GAIN_ON.TabIndex = 5;
            this.bt_GAIN_ON.Text = "ON";
            this.bt_GAIN_ON.UseVisualStyleBackColor = true;
            this.bt_GAIN_ON.Click += new System.EventHandler(this.bt_GAIN_ON_Click);
            // 
            // bt_MIRROR1_OFF
            // 
            this.bt_MIRROR1_OFF.Location = new System.Drawing.Point(560, 329);
            this.bt_MIRROR1_OFF.Name = "bt_MIRROR1_OFF";
            this.bt_MIRROR1_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_MIRROR1_OFF.TabIndex = 36;
            this.bt_MIRROR1_OFF.Text = "OFF";
            this.bt_MIRROR1_OFF.UseVisualStyleBackColor = true;
            this.bt_MIRROR1_OFF.Click += new System.EventHandler(this.bt_MIRROR1_OFF_Click);
            // 
            // bt_GAIN_OFF
            // 
            this.bt_GAIN_OFF.Location = new System.Drawing.Point(560, 52);
            this.bt_GAIN_OFF.Name = "bt_GAIN_OFF";
            this.bt_GAIN_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_GAIN_OFF.TabIndex = 6;
            this.bt_GAIN_OFF.Text = "OFF";
            this.bt_GAIN_OFF.UseVisualStyleBackColor = true;
            this.bt_GAIN_OFF.Click += new System.EventHandler(this.bt_GAIN_OFF_Click);
            // 
            // bt_MIRROR1_ON
            // 
            this.bt_MIRROR1_ON.Location = new System.Drawing.Point(442, 329);
            this.bt_MIRROR1_ON.Name = "bt_MIRROR1_ON";
            this.bt_MIRROR1_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_MIRROR1_ON.TabIndex = 35;
            this.bt_MIRROR1_ON.Text = "ON";
            this.bt_MIRROR1_ON.UseVisualStyleBackColor = true;
            this.bt_MIRROR1_ON.Click += new System.EventHandler(this.bt_MIRROR1_ON_Click);
            // 
            // SOA1
            // 
            this.SOA1.AutoSize = true;
            this.SOA1.Location = new System.Drawing.Point(21, 111);
            this.SOA1.Name = "SOA1";
            this.SOA1.Size = new System.Drawing.Size(29, 12);
            this.SOA1.TabIndex = 7;
            this.SOA1.Text = "SOA1";
            // 
            // tb_MIRROR1_I
            // 
            this.tb_MIRROR1_I.Location = new System.Drawing.Point(272, 331);
            this.tb_MIRROR1_I.Name = "tb_MIRROR1_I";
            this.tb_MIRROR1_I.Size = new System.Drawing.Size(100, 21);
            this.tb_MIRROR1_I.TabIndex = 34;
            this.tb_MIRROR1_I.Text = "0";
            this.tb_MIRROR1_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_SOA1_V
            // 
            this.tb_SOA1_V.Location = new System.Drawing.Point(98, 103);
            this.tb_SOA1_V.Name = "tb_SOA1_V";
            this.tb_SOA1_V.Size = new System.Drawing.Size(100, 21);
            this.tb_SOA1_V.TabIndex = 8;
            this.tb_SOA1_V.Text = "1.6";
            this.tb_SOA1_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_MIRROR1_V
            // 
            this.tb_MIRROR1_V.Location = new System.Drawing.Point(98, 332);
            this.tb_MIRROR1_V.Name = "tb_MIRROR1_V";
            this.tb_MIRROR1_V.Size = new System.Drawing.Size(100, 21);
            this.tb_MIRROR1_V.TabIndex = 33;
            this.tb_MIRROR1_V.Text = "1.6";
            this.tb_MIRROR1_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_SOA1_I
            // 
            this.tb_SOA1_I.Location = new System.Drawing.Point(272, 102);
            this.tb_SOA1_I.Name = "tb_SOA1_I";
            this.tb_SOA1_I.Size = new System.Drawing.Size(100, 21);
            this.tb_SOA1_I.TabIndex = 9;
            this.tb_SOA1_I.Text = "0";
            this.tb_SOA1_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 340);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 32;
            this.label8.Text = "MIRROR1";
            // 
            // bt_SOA1_ON
            // 
            this.bt_SOA1_ON.Location = new System.Drawing.Point(442, 100);
            this.bt_SOA1_ON.Name = "bt_SOA1_ON";
            this.bt_SOA1_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_SOA1_ON.TabIndex = 10;
            this.bt_SOA1_ON.Text = "ON";
            this.bt_SOA1_ON.UseVisualStyleBackColor = true;
            this.bt_SOA1_ON.Click += new System.EventHandler(this.bt_SOA1_ON_Click);
            // 
            // bt_PH2_OFF
            // 
            this.bt_PH2_OFF.Location = new System.Drawing.Point(560, 282);
            this.bt_PH2_OFF.Name = "bt_PH2_OFF";
            this.bt_PH2_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_PH2_OFF.TabIndex = 31;
            this.bt_PH2_OFF.Text = "OFF";
            this.bt_PH2_OFF.UseVisualStyleBackColor = true;
            this.bt_PH2_OFF.Click += new System.EventHandler(this.bt_PH2_OFF_Click);
            // 
            // bt_SOA1_OFF
            // 
            this.bt_SOA1_OFF.Location = new System.Drawing.Point(560, 100);
            this.bt_SOA1_OFF.Name = "bt_SOA1_OFF";
            this.bt_SOA1_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_SOA1_OFF.TabIndex = 11;
            this.bt_SOA1_OFF.Text = "OFF";
            this.bt_SOA1_OFF.UseVisualStyleBackColor = true;
            this.bt_SOA1_OFF.Click += new System.EventHandler(this.bt_SOA1_OFF_Click);
            // 
            // bt_PH2_ON
            // 
            this.bt_PH2_ON.Location = new System.Drawing.Point(442, 282);
            this.bt_PH2_ON.Name = "bt_PH2_ON";
            this.bt_PH2_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_PH2_ON.TabIndex = 30;
            this.bt_PH2_ON.Text = "ON";
            this.bt_PH2_ON.UseVisualStyleBackColor = true;
            this.bt_PH2_ON.Click += new System.EventHandler(this.bt_PH2_ON_Click);
            // 
            // SOA2
            // 
            this.SOA2.AutoSize = true;
            this.SOA2.Location = new System.Drawing.Point(21, 160);
            this.SOA2.Name = "SOA2";
            this.SOA2.Size = new System.Drawing.Size(29, 12);
            this.SOA2.TabIndex = 12;
            this.SOA2.Text = "SOA2";
            // 
            // tb_PH2_I
            // 
            this.tb_PH2_I.Location = new System.Drawing.Point(272, 284);
            this.tb_PH2_I.Name = "tb_PH2_I";
            this.tb_PH2_I.Size = new System.Drawing.Size(100, 21);
            this.tb_PH2_I.TabIndex = 29;
            this.tb_PH2_I.Text = "0";
            this.tb_PH2_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_SOA2_V
            // 
            this.tb_SOA2_V.Location = new System.Drawing.Point(98, 152);
            this.tb_SOA2_V.Name = "tb_SOA2_V";
            this.tb_SOA2_V.Size = new System.Drawing.Size(100, 21);
            this.tb_SOA2_V.TabIndex = 13;
            this.tb_SOA2_V.Text = "1.6";
            this.tb_SOA2_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_PH2_V
            // 
            this.tb_PH2_V.Location = new System.Drawing.Point(98, 285);
            this.tb_PH2_V.Name = "tb_PH2_V";
            this.tb_PH2_V.Size = new System.Drawing.Size(100, 21);
            this.tb_PH2_V.TabIndex = 28;
            this.tb_PH2_V.Text = "1.5";
            this.tb_PH2_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_SOA2_I
            // 
            this.tb_SOA2_I.Location = new System.Drawing.Point(272, 151);
            this.tb_SOA2_I.Name = "tb_SOA2_I";
            this.tb_SOA2_I.Size = new System.Drawing.Size(100, 21);
            this.tb_SOA2_I.TabIndex = 14;
            this.tb_SOA2_I.Text = "0";
            this.tb_SOA2_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 293);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 12);
            this.label7.TabIndex = 27;
            this.label7.Text = "PH2";
            // 
            // bt_SOA2_ON
            // 
            this.bt_SOA2_ON.Location = new System.Drawing.Point(442, 149);
            this.bt_SOA2_ON.Name = "bt_SOA2_ON";
            this.bt_SOA2_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_SOA2_ON.TabIndex = 15;
            this.bt_SOA2_ON.Text = "ON";
            this.bt_SOA2_ON.UseVisualStyleBackColor = true;
            this.bt_SOA2_ON.Click += new System.EventHandler(this.bt_SOA2_ON_Click);
            // 
            // bt_PH1_OFF
            // 
            this.bt_PH1_OFF.Location = new System.Drawing.Point(560, 238);
            this.bt_PH1_OFF.Name = "bt_PH1_OFF";
            this.bt_PH1_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_PH1_OFF.TabIndex = 26;
            this.bt_PH1_OFF.Text = "OFF";
            this.bt_PH1_OFF.UseVisualStyleBackColor = true;
            this.bt_PH1_OFF.Click += new System.EventHandler(this.bt_PH1_OFF_Click);
            // 
            // bt_SOA2_OFF
            // 
            this.bt_SOA2_OFF.Location = new System.Drawing.Point(560, 149);
            this.bt_SOA2_OFF.Name = "bt_SOA2_OFF";
            this.bt_SOA2_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_SOA2_OFF.TabIndex = 16;
            this.bt_SOA2_OFF.Text = "OFF";
            this.bt_SOA2_OFF.UseVisualStyleBackColor = true;
            this.bt_SOA2_OFF.Click += new System.EventHandler(this.bt_SOA2_OFF_Click);
            // 
            // bt_PH1_ON
            // 
            this.bt_PH1_ON.Location = new System.Drawing.Point(442, 238);
            this.bt_PH1_ON.Name = "bt_PH1_ON";
            this.bt_PH1_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_PH1_ON.TabIndex = 25;
            this.bt_PH1_ON.Text = "ON";
            this.bt_PH1_ON.UseVisualStyleBackColor = true;
            this.bt_PH1_ON.Click += new System.EventHandler(this.bt_PH1_ON_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "L_Phase/LP";
            // 
            // tb_PH1_I
            // 
            this.tb_PH1_I.Location = new System.Drawing.Point(272, 240);
            this.tb_PH1_I.Name = "tb_PH1_I";
            this.tb_PH1_I.Size = new System.Drawing.Size(100, 21);
            this.tb_PH1_I.TabIndex = 24;
            this.tb_PH1_I.Text = "0";
            this.tb_PH1_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_LP_V
            // 
            this.tb_LP_V.Location = new System.Drawing.Point(98, 196);
            this.tb_LP_V.Name = "tb_LP_V";
            this.tb_LP_V.Size = new System.Drawing.Size(100, 21);
            this.tb_LP_V.TabIndex = 18;
            this.tb_LP_V.Text = "1.5";
            this.tb_LP_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_PH1_V
            // 
            this.tb_PH1_V.Location = new System.Drawing.Point(98, 241);
            this.tb_PH1_V.Name = "tb_PH1_V";
            this.tb_PH1_V.Size = new System.Drawing.Size(100, 21);
            this.tb_PH1_V.TabIndex = 23;
            this.tb_PH1_V.Text = "1.5";
            this.tb_PH1_V.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_LP_I
            // 
            this.tb_LP_I.Location = new System.Drawing.Point(272, 195);
            this.tb_LP_I.Name = "tb_LP_I";
            this.tb_LP_I.Size = new System.Drawing.Size(100, 21);
            this.tb_LP_I.TabIndex = 19;
            this.tb_LP_I.Text = "0";
            this.tb_LP_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 249);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 12);
            this.label6.TabIndex = 22;
            this.label6.Text = "PH1";
            // 
            // bt_LP_ON
            // 
            this.bt_LP_ON.Location = new System.Drawing.Point(442, 193);
            this.bt_LP_ON.Name = "bt_LP_ON";
            this.bt_LP_ON.Size = new System.Drawing.Size(75, 23);
            this.bt_LP_ON.TabIndex = 20;
            this.bt_LP_ON.Text = "ON";
            this.bt_LP_ON.UseVisualStyleBackColor = true;
            this.bt_LP_ON.Click += new System.EventHandler(this.bt_LP_ON_Click);
            // 
            // bt_LP_OFF
            // 
            this.bt_LP_OFF.Location = new System.Drawing.Point(560, 193);
            this.bt_LP_OFF.Name = "bt_LP_OFF";
            this.bt_LP_OFF.Size = new System.Drawing.Size(75, 23);
            this.bt_LP_OFF.TabIndex = 21;
            this.bt_LP_OFF.Text = "OFF";
            this.bt_LP_OFF.UseVisualStyleBackColor = true;
            this.bt_LP_OFF.Click += new System.EventHandler(this.bt_LP_OFF_Click);
            // 
            // CoarseTuning
            // 
            this.CoarseTuning.Controls.Add(this.panel_coarsetuning);
            this.CoarseTuning.Location = new System.Drawing.Point(4, 42);
            this.CoarseTuning.Name = "CoarseTuning";
            this.CoarseTuning.Size = new System.Drawing.Size(1331, 812);
            this.CoarseTuning.TabIndex = 19;
            this.CoarseTuning.Text = "CoarseTuning";
            this.CoarseTuning.UseVisualStyleBackColor = true;
            // 
            // panel_coarsetuning
            // 
            this.panel_coarsetuning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_coarsetuning.Location = new System.Drawing.Point(0, 0);
            this.panel_coarsetuning.Name = "panel_coarsetuning";
            this.panel_coarsetuning.Size = new System.Drawing.Size(1331, 812);
            this.panel_coarsetuning.TabIndex = 0;
            // 
            // tabPage_coarse
            // 
            this.tabPage_coarse.Controls.Add(this.tableLayoutPanel1);
            this.tabPage_coarse.Location = new System.Drawing.Point(4, 42);
            this.tabPage_coarse.Name = "tabPage_coarse";
            this.tabPage_coarse.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_coarse.Size = new System.Drawing.Size(1331, 812);
            this.tabPage_coarse.TabIndex = 20;
            this.tabPage_coarse.Text = "coarseTuning";
            this.tabPage_coarse.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel_coarse, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.bt_choose, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.970223F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 96.02978F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1325, 806);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel_coarse
            // 
            this.panel_coarse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_coarse.Location = new System.Drawing.Point(3, 34);
            this.panel_coarse.Name = "panel_coarse";
            this.panel_coarse.Size = new System.Drawing.Size(1319, 769);
            this.panel_coarse.TabIndex = 2;
            // 
            // bt_choose
            // 
            this.bt_choose.Location = new System.Drawing.Point(3, 3);
            this.bt_choose.Name = "bt_choose";
            this.bt_choose.Size = new System.Drawing.Size(145, 23);
            this.bt_choose.TabIndex = 0;
            this.bt_choose.Text = "选择文件";
            this.bt_choose.UseVisualStyleBackColor = true;
            this.bt_choose.Click += new System.EventHandler(this.bt_choose_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 42);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1331, 812);
            this.tabPage1.TabIndex = 21;
            this.tabPage1.Text = "客户定制辅助";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox5, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1325, 806);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.splitContainer2);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(444, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(435, 262);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "合并(AlternativeQWLT)文件抽选数据";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(3, 17);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label61);
            this.splitContainer2.Panel1.Controls.Add(this.txt_ResultFileCommon_AlternativeQWLT);
            this.splitContainer2.Panel1.Controls.Add(this.btn_AnalyzeFileData_AlternativeQWLT);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txt_SelectedFileList_AlternativeQWLT);
            this.splitContainer2.Size = new System.Drawing.Size(429, 242);
            this.splitContainer2.SplitterDistance = 69;
            this.splitContainer2.TabIndex = 9;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(53, 42);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(35, 12);
            this.label61.TabIndex = 8;
            this.label61.Text = "备注:";
            // 
            // txt_ResultFileCommon_AlternativeQWLT
            // 
            this.txt_ResultFileCommon_AlternativeQWLT.Location = new System.Drawing.Point(88, 39);
            this.txt_ResultFileCommon_AlternativeQWLT.Name = "txt_ResultFileCommon_AlternativeQWLT";
            this.txt_ResultFileCommon_AlternativeQWLT.Size = new System.Drawing.Size(220, 21);
            this.txt_ResultFileCommon_AlternativeQWLT.TabIndex = 7;
            this.txt_ResultFileCommon_AlternativeQWLT.Text = "合并AlternativeQWLT数据";
            // 
            // btn_AnalyzeFileData_AlternativeQWLT
            // 
            this.btn_AnalyzeFileData_AlternativeQWLT.Location = new System.Drawing.Point(149, 10);
            this.btn_AnalyzeFileData_AlternativeQWLT.Name = "btn_AnalyzeFileData_AlternativeQWLT";
            this.btn_AnalyzeFileData_AlternativeQWLT.Size = new System.Drawing.Size(159, 23);
            this.btn_AnalyzeFileData_AlternativeQWLT.TabIndex = 0;
            this.btn_AnalyzeFileData_AlternativeQWLT.Text = "选择文件并合并导出";
            this.btn_AnalyzeFileData_AlternativeQWLT.UseVisualStyleBackColor = true;
            this.btn_AnalyzeFileData_AlternativeQWLT.Click += new System.EventHandler(this.btn_AnalyzeFileData_AlternativeQWLT_Click);
            // 
            // txt_SelectedFileList_AlternativeQWLT
            // 
            this.txt_SelectedFileList_AlternativeQWLT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_SelectedFileList_AlternativeQWLT.Location = new System.Drawing.Point(0, 0);
            this.txt_SelectedFileList_AlternativeQWLT.Multiline = true;
            this.txt_SelectedFileList_AlternativeQWLT.Name = "txt_SelectedFileList_AlternativeQWLT";
            this.txt_SelectedFileList_AlternativeQWLT.ReadOnly = true;
            this.txt_SelectedFileList_AlternativeQWLT.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_SelectedFileList_AlternativeQWLT.Size = new System.Drawing.Size(429, 169);
            this.txt_SelectedFileList_AlternativeQWLT.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.splitContainer1);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(435, 262);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "合并(Deviations)文件抽选数据";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 17);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label60);
            this.splitContainer1.Panel1.Controls.Add(this.label59);
            this.splitContainer1.Panel1.Controls.Add(this.txt_ResultFileCommon_Deviations);
            this.splitContainer1.Panel1.Controls.Add(this.btn_AnalyzeFileData_Deviations);
            this.splitContainer1.Panel1.Controls.Add(this.txt_SelectedChannel_Deviations);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txt_SelectedFileList_Deviations);
            this.splitContainer1.Size = new System.Drawing.Size(429, 242);
            this.splitContainer1.SplitterDistance = 69;
            this.splitContainer1.TabIndex = 9;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(53, 42);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(35, 12);
            this.label60.TabIndex = 8;
            this.label60.Text = "备注:";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(17, 15);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(71, 12);
            this.label59.TabIndex = 2;
            this.label59.Text = "目标通道号:";
            // 
            // txt_ResultFileCommon_Deviations
            // 
            this.txt_ResultFileCommon_Deviations.Location = new System.Drawing.Point(88, 39);
            this.txt_ResultFileCommon_Deviations.Name = "txt_ResultFileCommon_Deviations";
            this.txt_ResultFileCommon_Deviations.Size = new System.Drawing.Size(220, 21);
            this.txt_ResultFileCommon_Deviations.TabIndex = 7;
            this.txt_ResultFileCommon_Deviations.Text = "合并Deviations数据";
            // 
            // btn_AnalyzeFileData_Deviations
            // 
            this.btn_AnalyzeFileData_Deviations.Location = new System.Drawing.Point(149, 10);
            this.btn_AnalyzeFileData_Deviations.Name = "btn_AnalyzeFileData_Deviations";
            this.btn_AnalyzeFileData_Deviations.Size = new System.Drawing.Size(159, 23);
            this.btn_AnalyzeFileData_Deviations.TabIndex = 0;
            this.btn_AnalyzeFileData_Deviations.Text = "选择文件并合并导出";
            this.btn_AnalyzeFileData_Deviations.UseVisualStyleBackColor = true;
            this.btn_AnalyzeFileData_Deviations.Click += new System.EventHandler(this.btn_AnalyzeFileData_Deviations_Click);
            // 
            // txt_SelectedChannel_Deviations
            // 
            this.txt_SelectedChannel_Deviations.Location = new System.Drawing.Point(88, 10);
            this.txt_SelectedChannel_Deviations.Name = "txt_SelectedChannel_Deviations";
            this.txt_SelectedChannel_Deviations.Size = new System.Drawing.Size(45, 21);
            this.txt_SelectedChannel_Deviations.TabIndex = 1;
            this.txt_SelectedChannel_Deviations.Text = "51";
            // 
            // txt_SelectedFileList_Deviations
            // 
            this.txt_SelectedFileList_Deviations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_SelectedFileList_Deviations.Location = new System.Drawing.Point(0, 0);
            this.txt_SelectedFileList_Deviations.Multiline = true;
            this.txt_SelectedFileList_Deviations.Name = "txt_SelectedFileList_Deviations";
            this.txt_SelectedFileList_Deviations.ReadOnly = true;
            this.txt_SelectedFileList_Deviations.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_SelectedFileList_Deviations.Size = new System.Drawing.Size(429, 169);
            this.txt_SelectedFileList_Deviations.TabIndex = 1;
            // 
            // timer_Engineer
            // 
            this.timer_Engineer.Tick += new System.EventHandler(this.timer_Engineer_Tick);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.rb_left);
            this.groupBox8.Controls.Add(this.groupBox7);
            this.groupBox8.Controls.Add(this.label62);
            this.groupBox8.Controls.Add(this.groupBox6);
            this.groupBox8.Controls.Add(this.tb_temp);
            this.groupBox8.Controls.Add(this.rb_right);
            this.groupBox8.Location = new System.Drawing.Point(966, 382);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(260, 199);
            this.groupBox8.TabIndex = 99;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "温控";
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label63.Location = new System.Drawing.Point(25, 23);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(118, 60);
            this.label63.TabIndex = 55;
            this.label63.Text = "通道\r\n1:耦合\r\n2:光谱仪和波长计";
            // 
            // Form_MainPage_CT3103
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1339, 858);
            this.Controls.Add(this.tb_MainPage);
            this.Name = "Form_MainPage_CT3103";
            this.Text = "Form_MainPage_CT3103";
            this.Load += new System.EventHandler(this.Form_MainPage_CT3103_Load);
            this.tb_MainPage.ResumeLayout(false);
            this.tabPage测试.ResumeLayout(false);
            this.tab_TestPage.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage12.ResumeLayout(false);
            this.tabPage调试.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tabPage数据库.ResumeLayout(false);
            this.tabPage数据库.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TestData)).EndInit();
            this.tabPage视频.ResumeLayout(false);
            this.tabPage仪表.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.CoarseTuning.ResumeLayout(false);
            this.tabPage_coarse.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tb_MainPage;
        private System.Windows.Forms.TabPage tabPage数据库;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage测试;
        private System.Windows.Forms.Panel pnl_TestEnterance;
        private System.Windows.Forms.Panel pnl_RuntimeOverviewPage;
        private System.Windows.Forms.TabPage tabPage视频;
        private Panel panel7;
        private TabPage tabPage调试;
        private TabControl tab_TestPage;
        private TabPage tabPage9;
        private TabPage tabPage12;
        private TableLayoutPanel tableLayoutPanel4;
        private Panel panel_debug;
        private TableLayoutPanel tableLayoutPanel5;
        private Panel panel1;
        private Label label90;
        private Label label89;
        private DateTimePicker dtpTo;
        private DateTimePicker dtpFrom;
        private Button btn_GetTestStations;
        private Label lbl_teststation;
        private ComboBox cbo_TestStation;
        private TextBox tb_DataFile;
        private Button btn_UploadData;
        private Button btn_SelectData;
        private Button btn_ExportData;
        private CheckBox cb_Fail;
        private CheckBox cb_Pass;
        private Button btn_GetChipID;
        private Button btn_GetCarrierID;
        private Button btn_GetPN;
        private Button btn_GetSubOrder;
        private Button btn_QueryData;
        private Button btn_GetWO;
        private Label label36;
        private ComboBox cbo_ChipID;
        private Label label35;
        private ComboBox cbo_CarrierID;
        private Label label34;
        private ComboBox cbo_PartNumber;
        private Label label33;
        private ComboBox cbo_SubOrder;
        private Label label32;
        private ComboBox cbo_WorkOrder;
        private Panel panel10;
        private DataGridView dgv_TestData;
        private Panel panel_CV;
        private ComboBox cb_PostBIColumn;
        private RadioButton rb_PostBI;
        private RadioButton rb_PreBI;
        private RadioButton rb_GS;
        private Timer timer_Engineer;
        private TabPage tabPage仪表;
        private Panel panel2;
        private GroupBox groupBox2;
        private Button bt_MPD2_OFF;
        private Button bt_MPD2_ON;
        private TextBox tb_MPD2_I;
        private TextBox tb_MPD2_V;
        private Label label12;
        private Button bt_MPD1_OFF;
        private Button bt_MPD1_ON;
        private TextBox tb_MPD1_I;
        private TextBox tb_MPD1_V;
        private Label label13;
        private Button bt_Bias2_OFF;
        private Button bt_Bias2_ON;
        private TextBox tb_Bias2_I;
        private TextBox tb_Bias2_V;
        private Label label10;
        private Button bt_Bias1_OFF;
        private Button bt_Bias1_ON;
        private TextBox tb_Bias1_I;
        private TextBox tb_Bias1_V;
        private Label label11;
        private GroupBox groupBox1;
        private Label label3;
        private Button bt_MIRROR2_OFF;
        private Label label1;
        private Button bt_MIRROR2_ON;
        private Label label4;
        private TextBox tb_MIRROR2_I;
        private TextBox tb_GAIN_V;
        private TextBox tb_MIRROR2_V;
        private TextBox tb_GAIN_I;
        private Label label9;
        private Button bt_GAIN_ON;
        private Button bt_MIRROR1_OFF;
        private Button bt_GAIN_OFF;
        private Button bt_MIRROR1_ON;
        private Label SOA1;
        private TextBox tb_MIRROR1_I;
        private TextBox tb_SOA1_V;
        private TextBox tb_MIRROR1_V;
        private TextBox tb_SOA1_I;
        private Label label8;
        private Button bt_SOA1_ON;
        private Button bt_PH2_OFF;
        private Button bt_SOA1_OFF;
        private Button bt_PH2_ON;
        private Label SOA2;
        private TextBox tb_PH2_I;
        private TextBox tb_SOA2_V;
        private TextBox tb_PH2_V;
        private TextBox tb_SOA2_I;
        private Label label7;
        private Button bt_SOA2_ON;
        private Button bt_PH1_OFF;
        private Button bt_SOA2_OFF;
        private Button bt_PH1_ON;
        private Label label5;
        private TextBox tb_PH1_I;
        private TextBox tb_LP_V;
        private TextBox tb_PH1_V;
        private TextBox tb_LP_I;
        private Label label6;
        private Button bt_LP_ON;
        private Button bt_LP_OFF;
        private Label label29;
        private Label label28;
        private Label label27;
        private Label label26;
        private Label label25;
        private Label label24;
        private Label label23;
        private Label label22;
        private Label label21;
        private Label label20;
        private Label label19;
        private Label label18;
        private Label label17;
        private Label label16;
        private Label label15;
        private Label label14;
        private Label label42;
        private Label label41;
        private Label label40;
        private Label label39;
        private Label label38;
        private Label label37;
        private Label label31;
        private Label label30;
        private Label lab_GetMPD2_I;
        private Label lab_GetMPD1_I;
        private Label lab_GetBIAS2_I;
        private Label lab_GetBIAS1_I;
        private Label label48;
        private Label lab_GetMIRROR2_V;
        private Label lab_GetMIRROR1_V;
        private Label lab_GetPH2_V;
        private Label lab_GetPH1_V;
        private Label lab_GetLP_V;
        private Label lab_GetSOA2_V;
        private Label lab_GetSOA1_V;
        private Label lab_GetGAIN_V;
        private Label label43;
        private Button bt_GetMIRROR2;
        private Button bt_GetMIRROR1;
        private Button bt_GetGAIN;
        private Button bt_GetPH2;
        private Button bt_GetSOA1;
        private Button bt_GetPH1;
        private Button bt_GetSOA2;
        private Button bt_GetLP;
        private Button bt_GetMPD2;
        private Button bt_GetMPD1;
        private Button bt_GetBIAS2;
        private Button bt_GetBIAS1;
        private Label label47;
        private TextBox tb_liv_pdcompCurr;
        private Label label49;
        private TextBox tb_liv_pdbiasV;
        private Label label50;
        private TextBox tb_liv_compV;
        private Label label46;
        private TextBox tb_liv_end;
        private Label label45;
        private TextBox tb_liv_step;
        private Label label44;
        private TextBox tb_liv_start;
        private Button bt_LIV;
        private Button button1;
        private Label lab_GetMIRROR2_I;
        private Label lab_GetMIRROR1_I;
        private Label lab_GetPH2_I;
        private Label lab_GetPH1_I;
        private Label lab_GetLP_I;
        private Label lab_GetSOA2_I;
        private Label lab_GetSOA1_I;
        private Label lab_GetGAIN_I;
        private Label label51;
        private Label lab_GetMPD2_V;
        private Label lab_GetMPD1_V;
        private Label lab_GetBIAS2_V;
        private Label lab_GetBIAS1_V;
        private Label label52;
        private ComboBox comboBox1;
        private Button bt_OFF_All;
        private Label lab_GetPD_V;
        private Button bt_GetPD;
        private Label lab_GetPD_I;
        private Button bt_PD_OFF;
        private Button bt_PD_ON;
        private Label label53;
        private Label label54;
        private TextBox tb_PD_I;
        private Label lab_ted;
        private Button bt_gettemp;
        private Button bt_ted_stop;
        private TextBox tb_temp;
        private Button bt_ted_start;
        private TabPage CoarseTuning;
        private Panel panel_coarsetuning;
        private RadioButton rb_right;
        private RadioButton rb_left;
        private Label label55;
        private TextBox tb_wave;
        private TabPage tabPage_coarse;
        private TableLayoutPanel tableLayoutPanel1;
        private Button bt_choose;
        private Panel panel_coarse;
        private GroupBox groupBox3;
        private Label label56;
        private TextBox txt_OpticalChannel;
        private Button btn_SwitchCh;
        private Label label57;
        private Label label58;
        private TableLayoutPanel tableLayoutPanel2;
        private TabPage tabPage1;
        private GroupBox groupBox4;
        private Label label59;
        private TextBox txt_SelectedFileList_Deviations;
        private TextBox txt_SelectedChannel_Deviations;
        private Button btn_AnalyzeFileData_Deviations;
        private Label label60;
        private TextBox txt_ResultFileCommon_Deviations;
        private SplitContainer splitContainer1;
        private GroupBox groupBox5;
        private SplitContainer splitContainer2;
        private Label label61;
        private TextBox txt_ResultFileCommon_AlternativeQWLT;
        private Button btn_AnalyzeFileData_AlternativeQWLT;
        private TextBox txt_SelectedFileList_AlternativeQWLT;
        private GroupBox groupBox6;
        private Label label62;
        private GroupBox groupBox7;
        private Button bt_TC_start;
        private Button bt_TC_stop;
        private GroupBox groupBox8;
        private Label label63;
    }
}