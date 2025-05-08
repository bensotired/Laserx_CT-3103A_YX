
namespace TestPlugin_Demo
{
     partial class Form_TestEnterance_CT3103
    {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TestEnterance_CT3103));
            this.btn_EditTestProfile = new System.Windows.Forms.Button();
            this.cmb_TestProfile_Selector = new System.Windows.Forms.ComboBox();
            this.btn_Ex_StopTest = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txt_OperatorID = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage_load = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tb_ChipName = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.tb_CarrierID = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.tb_WorkOrder = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tb_OeskID = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tb_WaferName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tb_MaskName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.bt_Test = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_PostBIColumn_Compare = new System.Windows.Forms.ComboBox();
            this.rb_PostBI_Compare = new System.Windows.Forms.RadioButton();
            this.rb_PreBI_Compare = new System.Windows.Forms.RadioButton();
            this.rb_GS_Compare = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cb_PostBIColumn = new System.Windows.Forms.ComboBox();
            this.rb_PostBI = new System.Windows.Forms.RadioButton();
            this.rb_PreBI = new System.Windows.Forms.RadioButton();
            this.rb_GS = new System.Windows.Forms.RadioButton();
            this.bt_Right_enable = new System.Windows.Forms.Button();
            this.bt_Left_enable = new System.Windows.Forms.Button();
            this.textBox_TempTolerance = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_Right_TempList = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_Left_TempList = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_HightTemp = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_LowTemp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btn_startTest = new System.Windows.Forms.Button();
            this.btn_All_RequesPauseTest = new System.Windows.Forms.Button();
            this.btn_All_RequesResumeTest = new System.Windows.Forms.Button();
            this.btn_Normal_StopTest = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage_load.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_EditTestProfile
            // 
            this.btn_EditTestProfile.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_EditTestProfile.Location = new System.Drawing.Point(607, 21);
            this.btn_EditTestProfile.Name = "btn_EditTestProfile";
            this.btn_EditTestProfile.Size = new System.Drawing.Size(152, 38);
            this.btn_EditTestProfile.TabIndex = 0;
            this.btn_EditTestProfile.Text = "编辑";
            this.btn_EditTestProfile.UseVisualStyleBackColor = true;
            this.btn_EditTestProfile.Click += new System.EventHandler(this.btn_editTestProfile_Click);
            // 
            // cmb_TestProfile_Selector
            // 
            this.cmb_TestProfile_Selector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_TestProfile_Selector.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmb_TestProfile_Selector.FormattingEnabled = true;
            this.cmb_TestProfile_Selector.Location = new System.Drawing.Point(157, 26);
            this.cmb_TestProfile_Selector.Name = "cmb_TestProfile_Selector";
            this.cmb_TestProfile_Selector.Size = new System.Drawing.Size(434, 29);
            this.cmb_TestProfile_Selector.TabIndex = 1;
            this.cmb_TestProfile_Selector.SelectionChangeCommitted += new System.EventHandler(this.cb_testProfileSelector_SelectionChangeCommitted);
            // 
            // btn_Ex_StopTest
            // 
            this.btn_Ex_StopTest.BackColor = System.Drawing.Color.Red;
            this.btn_Ex_StopTest.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Ex_StopTest.Location = new System.Drawing.Point(1066, 31);
            this.btn_Ex_StopTest.Name = "btn_Ex_StopTest";
            this.btn_Ex_StopTest.Size = new System.Drawing.Size(152, 52);
            this.btn_Ex_StopTest.TabIndex = 2;
            this.btn_Ex_StopTest.Text = "退出";
            this.btn_Ex_StopTest.UseVisualStyleBackColor = false;
            this.btn_Ex_StopTest.Click += new System.EventHandler(this.btn_Ex_StopTest_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(39, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 19);
            this.label3.TabIndex = 11;
            this.label3.Text = "测试方案";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.78375F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.21625F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 127F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1339, 826);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.cmb_TestProfile_Selector);
            this.panel2.Controls.Add(this.txt_OperatorID);
            this.panel2.Controls.Add(this.btn_EditTestProfile);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1333, 83);
            this.panel2.TabIndex = 0;
            // 
            // txt_OperatorID
            // 
            this.txt_OperatorID.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_OperatorID.Location = new System.Drawing.Point(958, 26);
            this.txt_OperatorID.Name = "txt_OperatorID";
            this.txt_OperatorID.Size = new System.Drawing.Size(345, 29);
            this.txt_OperatorID.TabIndex = 120;
            this.txt_OperatorID.Text = "operator";
            this.txt_OperatorID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(814, 31);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(129, 19);
            this.label14.TabIndex = 119;
            this.label14.Text = "操作人员编号";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage_load);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl2.ItemSize = new System.Drawing.Size(108, 38);
            this.tabControl2.Location = new System.Drawing.Point(3, 92);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1333, 603);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage_load
            // 
            this.tabPage_load.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage_load.Controls.Add(this.tableLayoutPanel2);
            this.tabPage_load.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage_load.Location = new System.Drawing.Point(4, 42);
            this.tabPage_load.Name = "tabPage_load";
            this.tabPage_load.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_load.Size = new System.Drawing.Size(1325, 557);
            this.tabPage_load.TabIndex = 0;
            this.tabPage_load.Text = "测试参数编辑";
            this.tabPage_load.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.35353F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.64646F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 57.09024F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.90976F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1317, 549);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tb_ChipName);
            this.panel1.Controls.Add(this.label19);
            this.panel1.Controls.Add(this.tb_CarrierID);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.tb_WorkOrder);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.tb_OeskID);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.tb_WaferName);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.tb_MaskName);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.bt_Test);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.cb_PostBIColumn);
            this.panel1.Controls.Add(this.rb_PostBI);
            this.panel1.Controls.Add(this.rb_PreBI);
            this.panel1.Controls.Add(this.rb_GS);
            this.panel1.Controls.Add(this.bt_Right_enable);
            this.panel1.Controls.Add(this.bt_Left_enable);
            this.panel1.Controls.Add(this.textBox_TempTolerance);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.textBox_Right_TempList);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.textBox_Left_TempList);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.textBox_HightTemp);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBox_LowTemp);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1311, 543);
            this.panel1.TabIndex = 0;
            // 
            // tb_ChipName
            // 
            this.tb_ChipName.Location = new System.Drawing.Point(229, 452);
            this.tb_ChipName.Name = "tb_ChipName";
            this.tb_ChipName.Size = new System.Drawing.Size(100, 21);
            this.tb_ChipName.TabIndex = 62;
            this.tb_ChipName.Text = "Demo";
            this.tb_ChipName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(146, 456);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(59, 12);
            this.label19.TabIndex = 61;
            this.label19.Text = "ChipName:";
            // 
            // tb_CarrierID
            // 
            this.tb_CarrierID.Enabled = false;
            this.tb_CarrierID.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_CarrierID.Location = new System.Drawing.Point(756, 242);
            this.tb_CarrierID.Name = "tb_CarrierID";
            this.tb_CarrierID.Size = new System.Drawing.Size(254, 29);
            this.tb_CarrierID.TabIndex = 60;
            this.tb_CarrierID.Text = "*********";
            this.tb_CarrierID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.Location = new System.Drawing.Point(582, 248);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(171, 19);
            this.label18.TabIndex = 59;
            this.label18.Text = "当前测试夹具编号:";
            // 
            // tb_WorkOrder
            // 
            this.tb_WorkOrder.Location = new System.Drawing.Point(229, 409);
            this.tb_WorkOrder.Name = "tb_WorkOrder";
            this.tb_WorkOrder.Size = new System.Drawing.Size(100, 21);
            this.tb_WorkOrder.TabIndex = 58;
            this.tb_WorkOrder.Text = "Demo";
            this.tb_WorkOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(146, 413);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 57;
            this.label17.Text = "WorkOrder:";
            // 
            // tb_OeskID
            // 
            this.tb_OeskID.Enabled = false;
            this.tb_OeskID.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_OeskID.Location = new System.Drawing.Point(756, 290);
            this.tb_OeskID.Name = "tb_OeskID";
            this.tb_OeskID.Size = new System.Drawing.Size(254, 29);
            this.tb_OeskID.TabIndex = 56;
            this.tb_OeskID.Text = "*********";
            this.tb_OeskID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(583, 296);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(171, 19);
            this.label16.TabIndex = 55;
            this.label16.Text = "当前测试产品编号:";
            // 
            // tb_WaferName
            // 
            this.tb_WaferName.Location = new System.Drawing.Point(229, 366);
            this.tb_WaferName.Name = "tb_WaferName";
            this.tb_WaferName.Size = new System.Drawing.Size(100, 21);
            this.tb_WaferName.TabIndex = 54;
            this.tb_WaferName.Text = "Demo";
            this.tb_WaferName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(146, 370);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 53;
            this.label15.Text = "WaferName:";
            // 
            // tb_MaskName
            // 
            this.tb_MaskName.Location = new System.Drawing.Point(229, 324);
            this.tb_MaskName.Name = "tb_MaskName";
            this.tb_MaskName.Size = new System.Drawing.Size(100, 21);
            this.tb_MaskName.TabIndex = 52;
            this.tb_MaskName.Text = "Demo";
            this.tb_MaskName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(146, 328);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 12);
            this.label13.TabIndex = 51;
            this.label13.Text = "MaskName:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1218, 489);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 50;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bt_Test
            // 
            this.bt_Test.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Test.Location = new System.Drawing.Point(1099, 476);
            this.bt_Test.Name = "bt_Test";
            this.bt_Test.Size = new System.Drawing.Size(194, 52);
            this.bt_Test.TabIndex = 49;
            this.bt_Test.Text = "测试";
            this.bt_Test.UseVisualStyleBackColor = true;
            this.bt_Test.Visible = false;
            this.bt_Test.Click += new System.EventHandler(this.bt_Test_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_PostBIColumn_Compare);
            this.groupBox1.Controls.Add(this.rb_PostBI_Compare);
            this.groupBox1.Controls.Add(this.rb_PreBI_Compare);
            this.groupBox1.Controls.Add(this.rb_GS_Compare);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(1163, 170);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(479, 87);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "比较源";
            this.groupBox1.Visible = false;
            // 
            // cb_PostBIColumn_Compare
            // 
            this.cb_PostBIColumn_Compare.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_PostBIColumn_Compare.Enabled = false;
            this.cb_PostBIColumn_Compare.FormattingEnabled = true;
            this.cb_PostBIColumn_Compare.Location = new System.Drawing.Point(330, 33);
            this.cb_PostBIColumn_Compare.Name = "cb_PostBIColumn_Compare";
            this.cb_PostBIColumn_Compare.Size = new System.Drawing.Size(121, 20);
            this.cb_PostBIColumn_Compare.TabIndex = 49;
            // 
            // rb_PostBI_Compare
            // 
            this.rb_PostBI_Compare.AutoSize = true;
            this.rb_PostBI_Compare.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb_PostBI_Compare.Location = new System.Drawing.Point(214, 33);
            this.rb_PostBI_Compare.Name = "rb_PostBI_Compare";
            this.rb_PostBI_Compare.Size = new System.Drawing.Size(84, 23);
            this.rb_PostBI_Compare.TabIndex = 48;
            this.rb_PostBI_Compare.TabStop = true;
            this.rb_PostBI_Compare.Text = "老化后";
            this.rb_PostBI_Compare.UseVisualStyleBackColor = true;
            this.rb_PostBI_Compare.CheckedChanged += new System.EventHandler(this.rb_PostBI_Compare_CheckedChanged);
            // 
            // rb_PreBI_Compare
            // 
            this.rb_PreBI_Compare.AutoSize = true;
            this.rb_PreBI_Compare.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb_PreBI_Compare.Location = new System.Drawing.Point(113, 33);
            this.rb_PreBI_Compare.Name = "rb_PreBI_Compare";
            this.rb_PreBI_Compare.Size = new System.Drawing.Size(84, 23);
            this.rb_PreBI_Compare.TabIndex = 47;
            this.rb_PreBI_Compare.TabStop = true;
            this.rb_PreBI_Compare.Text = "老化前";
            this.rb_PreBI_Compare.UseVisualStyleBackColor = true;
            // 
            // rb_GS_Compare
            // 
            this.rb_GS_Compare.AutoSize = true;
            this.rb_GS_Compare.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb_GS_Compare.Location = new System.Drawing.Point(23, 33);
            this.rb_GS_Compare.Name = "rb_GS_Compare";
            this.rb_GS_Compare.Size = new System.Drawing.Size(65, 23);
            this.rb_GS_Compare.TabIndex = 46;
            this.rb_GS_Compare.TabStop = true;
            this.rb_GS_Compare.Text = "金样";
            this.rb_GS_Compare.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox1.Location = new System.Drawing.Point(1099, 144);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(91, 20);
            this.checkBox1.TabIndex = 47;
            this.checkBox1.Text = "数值比较";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(573, 123);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 19);
            this.label12.TabIndex = 46;
            this.label12.Text = "测试状态：";
            // 
            // cb_PostBIColumn
            // 
            this.cb_PostBIColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_PostBIColumn.Enabled = false;
            this.cb_PostBIColumn.FormattingEnabled = true;
            this.cb_PostBIColumn.Location = new System.Drawing.Point(984, 122);
            this.cb_PostBIColumn.Name = "cb_PostBIColumn";
            this.cb_PostBIColumn.Size = new System.Drawing.Size(121, 20);
            this.cb_PostBIColumn.TabIndex = 45;
            this.cb_PostBIColumn.Visible = false;
            // 
            // rb_PostBI
            // 
            this.rb_PostBI.AutoSize = true;
            this.rb_PostBI.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb_PostBI.Location = new System.Drawing.Point(873, 121);
            this.rb_PostBI.Name = "rb_PostBI";
            this.rb_PostBI.Size = new System.Drawing.Size(84, 23);
            this.rb_PostBI.TabIndex = 44;
            this.rb_PostBI.Text = "老化后";
            this.rb_PostBI.UseVisualStyleBackColor = true;
            this.rb_PostBI.CheckedChanged += new System.EventHandler(this.rb_PostBI_CheckedChanged);
            // 
            // rb_PreBI
            // 
            this.rb_PreBI.AutoSize = true;
            this.rb_PreBI.Checked = true;
            this.rb_PreBI.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb_PreBI.Location = new System.Drawing.Point(772, 121);
            this.rb_PreBI.Name = "rb_PreBI";
            this.rb_PreBI.Size = new System.Drawing.Size(84, 23);
            this.rb_PreBI.TabIndex = 43;
            this.rb_PreBI.TabStop = true;
            this.rb_PreBI.Text = "老化前";
            this.rb_PreBI.UseVisualStyleBackColor = true;
            // 
            // rb_GS
            // 
            this.rb_GS.AutoSize = true;
            this.rb_GS.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rb_GS.Location = new System.Drawing.Point(682, 121);
            this.rb_GS.Name = "rb_GS";
            this.rb_GS.Size = new System.Drawing.Size(65, 23);
            this.rb_GS.TabIndex = 42;
            this.rb_GS.Text = "金样";
            this.rb_GS.UseVisualStyleBackColor = true;
            this.rb_GS.Visible = false;
            // 
            // bt_Right_enable
            // 
            this.bt_Right_enable.BackColor = System.Drawing.Color.LightGreen;
            this.bt_Right_enable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_Right_enable.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Right_enable.Location = new System.Drawing.Point(334, 225);
            this.bt_Right_enable.Name = "bt_Right_enable";
            this.bt_Right_enable.Size = new System.Drawing.Size(101, 53);
            this.bt_Right_enable.TabIndex = 16;
            this.bt_Right_enable.Text = "右载台启用";
            this.bt_Right_enable.UseVisualStyleBackColor = false;
            this.bt_Right_enable.Click += new System.EventHandler(this.bt_Right_enable_Click);
            // 
            // bt_Left_enable
            // 
            this.bt_Left_enable.BackColor = System.Drawing.Color.LightGreen;
            this.bt_Left_enable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_Left_enable.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Left_enable.Location = new System.Drawing.Point(118, 225);
            this.bt_Left_enable.Name = "bt_Left_enable";
            this.bt_Left_enable.Size = new System.Drawing.Size(101, 53);
            this.bt_Left_enable.TabIndex = 15;
            this.bt_Left_enable.Text = "左载台启用";
            this.bt_Left_enable.UseVisualStyleBackColor = false;
            this.bt_Left_enable.Click += new System.EventHandler(this.bt_Left_enable_Click);
            // 
            // textBox_TempTolerance
            // 
            this.textBox_TempTolerance.Location = new System.Drawing.Point(226, 168);
            this.textBox_TempTolerance.Name = "textBox_TempTolerance";
            this.textBox_TempTolerance.Size = new System.Drawing.Size(209, 21);
            this.textBox_TempTolerance.TabIndex = 14;
            this.textBox_TempTolerance.Text = "0.3";
            this.textBox_TempTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(118, 173);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 13;
            this.label11.Text = "温度容差：";
            // 
            // textBox_Right_TempList
            // 
            this.textBox_Right_TempList.Location = new System.Drawing.Point(226, 125);
            this.textBox_Right_TempList.Name = "textBox_Right_TempList";
            this.textBox_Right_TempList.Size = new System.Drawing.Size(209, 21);
            this.textBox_Right_TempList.TabIndex = 12;
            this.textBox_Right_TempList.Text = "55";
            this.textBox_Right_TempList.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(118, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 12);
            this.label10.TabIndex = 11;
            this.label10.Text = "右载台目标温度：";
            // 
            // textBox_Left_TempList
            // 
            this.textBox_Left_TempList.Location = new System.Drawing.Point(226, 88);
            this.textBox_Left_TempList.Name = "textBox_Left_TempList";
            this.textBox_Left_TempList.Size = new System.Drawing.Size(209, 21);
            this.textBox_Left_TempList.TabIndex = 10;
            this.textBox_Left_TempList.Text = "55";
            this.textBox_Left_TempList.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(118, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 12);
            this.label9.TabIndex = 9;
            this.label9.Text = "左载台目标温度：";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Red;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(456, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 32);
            this.label8.TabIndex = 8;
            this.label8.Text = "高温";
            this.label8.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(429, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 21);
            this.label7.TabIndex = 7;
            this.label7.Text = "<";
            this.label7.Visible = false;
            // 
            // textBox_HightTemp
            // 
            this.textBox_HightTemp.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_HightTemp.Location = new System.Drawing.Point(352, 18);
            this.textBox_HightTemp.Name = "textBox_HightTemp";
            this.textBox_HightTemp.Size = new System.Drawing.Size(72, 31);
            this.textBox_HightTemp.TabIndex = 6;
            this.textBox_HightTemp.Text = "60";
            this.textBox_HightTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_HightTemp.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(314, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 21);
            this.label6.TabIndex = 5;
            this.label6.Text = "<=";
            this.label6.Visible = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.LightCoral;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(251, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 32);
            this.label5.TabIndex = 4;
            this.label5.Text = "中温";
            this.label5.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(225, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "<";
            this.label4.Visible = false;
            // 
            // textBox_LowTemp
            // 
            this.textBox_LowTemp.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_LowTemp.Location = new System.Drawing.Point(148, 18);
            this.textBox_LowTemp.Name = "textBox_LowTemp";
            this.textBox_LowTemp.Size = new System.Drawing.Size(72, 31);
            this.textBox_LowTemp.TabIndex = 2;
            this.textBox_LowTemp.Text = "30";
            this.textBox_LowTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_LowTemp.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(110, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "<=";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SpringGreen;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(47, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "低温";
            this.label1.Visible = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btn_startTest);
            this.panel3.Controls.Add(this.btn_All_RequesPauseTest);
            this.panel3.Controls.Add(this.btn_All_RequesResumeTest);
            this.panel3.Controls.Add(this.btn_Normal_StopTest);
            this.panel3.Controls.Add(this.btn_Ex_StopTest);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 701);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1333, 122);
            this.panel3.TabIndex = 2;
            // 
            // btn_startTest
            // 
            this.btn_startTest.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_startTest.Location = new System.Drawing.Point(34, 31);
            this.btn_startTest.Name = "btn_startTest";
            this.btn_startTest.Size = new System.Drawing.Size(194, 52);
            this.btn_startTest.TabIndex = 2;
            this.btn_startTest.Text = "开始测试";
            this.btn_startTest.UseVisualStyleBackColor = true;
            this.btn_startTest.Click += new System.EventHandler(this.btn_startTest_Click);
            // 
            // btn_All_RequesPauseTest
            // 
            this.btn_All_RequesPauseTest.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_All_RequesPauseTest.Location = new System.Drawing.Point(316, 31);
            this.btn_All_RequesPauseTest.Name = "btn_All_RequesPauseTest";
            this.btn_All_RequesPauseTest.Size = new System.Drawing.Size(152, 52);
            this.btn_All_RequesPauseTest.TabIndex = 17;
            this.btn_All_RequesPauseTest.Text = "暂停";
            this.btn_All_RequesPauseTest.UseVisualStyleBackColor = true;
            this.btn_All_RequesPauseTest.Click += new System.EventHandler(this.btn_All_RequesPauseTest_Click);
            // 
            // btn_All_RequesResumeTest
            // 
            this.btn_All_RequesResumeTest.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_All_RequesResumeTest.Location = new System.Drawing.Point(550, 31);
            this.btn_All_RequesResumeTest.Name = "btn_All_RequesResumeTest";
            this.btn_All_RequesResumeTest.Size = new System.Drawing.Size(152, 52);
            this.btn_All_RequesResumeTest.TabIndex = 18;
            this.btn_All_RequesResumeTest.Text = "继续";
            this.btn_All_RequesResumeTest.UseVisualStyleBackColor = true;
            this.btn_All_RequesResumeTest.Click += new System.EventHandler(this.btn_All_RequesResumeTest_Click);
            // 
            // btn_Normal_StopTest
            // 
            this.btn_Normal_StopTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btn_Normal_StopTest.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Normal_StopTest.Location = new System.Drawing.Point(818, 31);
            this.btn_Normal_StopTest.Name = "btn_Normal_StopTest";
            this.btn_Normal_StopTest.Size = new System.Drawing.Size(152, 52);
            this.btn_Normal_StopTest.TabIndex = 16;
            this.btn_Normal_StopTest.Text = "运行正常结束";
            this.btn_Normal_StopTest.UseVisualStyleBackColor = false;
            this.btn_Normal_StopTest.Visible = false;
            this.btn_Normal_StopTest.Click += new System.EventHandler(this.btn_Normal_StopTest_Click);
            // 
            // Form_TestEnterance_CT3103
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1339, 826);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_TestEnterance_CT3103";
            this.Text = "测试入口";
            this.Load += new System.EventHandler(this.Form_TestEnterance_CT3103_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage_load.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_EditTestProfile;
        private System.Windows.Forms.ComboBox cmb_TestProfile_Selector;
        private System.Windows.Forms.Button btn_Ex_StopTest;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage_load;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_All_RequesPauseTest;
        private System.Windows.Forms.Button btn_Normal_StopTest;
        private System.Windows.Forms.Button btn_All_RequesResumeTest;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txt_OperatorID;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btn_startTest;
        private System.Windows.Forms.Button bt_Right_enable;
        private System.Windows.Forms.Button bt_Left_enable;
        private System.Windows.Forms.TextBox textBox_TempTolerance;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_Right_TempList;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_Left_TempList;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_HightTemp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_LowTemp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cb_PostBIColumn_Compare;
        private System.Windows.Forms.RadioButton rb_PostBI_Compare;
        private System.Windows.Forms.RadioButton rb_PreBI_Compare;
        private System.Windows.Forms.RadioButton rb_GS_Compare;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cb_PostBIColumn;
        private System.Windows.Forms.RadioButton rb_PostBI;
        private System.Windows.Forms.RadioButton rb_PreBI;
        private System.Windows.Forms.RadioButton rb_GS;
        private System.Windows.Forms.Button bt_Test;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_MaskName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tb_OeskID;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tb_WaferName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tb_WorkOrder;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tb_CarrierID;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tb_ChipName;
        private System.Windows.Forms.Label label19;
    }
}