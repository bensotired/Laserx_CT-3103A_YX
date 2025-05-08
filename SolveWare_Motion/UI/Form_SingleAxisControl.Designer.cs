namespace SolveWare_Motion 
{
    partial class Form_SingleAxisControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_SingleAxisControl));
            this.btn_servoOn = new System.Windows.Forms.Button();
            this.btn_servoOff = new System.Windows.Forms.Button();
            this.btn_PosLimtSignal = new System.Windows.Forms.Button();
            this.btn_NegLimtSignal = new System.Windows.Forms.Button();
            this.btn_OrgSignal = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gb_info = new System.Windows.Forms.GroupBox();
            this.lbl_servoStatus = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_currentPulse = new System.Windows.Forms.TextBox();
            this.tb_currentAbsPos = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_NegRelMove = new System.Windows.Forms.Button();
            this.btn_PosRelMove = new System.Windows.Forms.Button();
            this.tb_planRelPos = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_NegJogMove = new System.Windows.Forms.Button();
            this.btn_PosJogMove = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_AbsMove = new System.Windows.Forms.Button();
            this.tb_planAbsPos = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_stp_1 = new System.Windows.Forms.RadioButton();
            this.btn_moveRelMicroStep_negative = new System.Windows.Forms.Button();
            this.btn_moveRelMicroStep_positive = new System.Windows.Forms.Button();
            this.rb_stp_0_1 = new System.Windows.Forms.RadioButton();
            this.rb_stp_0_01 = new System.Windows.Forms.RadioButton();
            this.rb_stp_0_001 = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_AxisStop = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chk_AxisHomed = new System.Windows.Forms.CheckBox();
            this.btn_HomeMove = new System.Windows.Forms.Button();
            this.rb_stp_0_0001 = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.gb_info.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_servoOn
            // 
            this.btn_servoOn.Location = new System.Drawing.Point(22, 25);
            this.btn_servoOn.Name = "btn_servoOn";
            this.btn_servoOn.Size = new System.Drawing.Size(75, 23);
            this.btn_servoOn.TabIndex = 1;
            this.btn_servoOn.Text = "ON";
            this.btn_servoOn.UseVisualStyleBackColor = true;
            this.btn_servoOn.Click += new System.EventHandler(this.btn_servoOn_Click);
            // 
            // btn_servoOff
            // 
            this.btn_servoOff.Location = new System.Drawing.Point(103, 25);
            this.btn_servoOff.Name = "btn_servoOff";
            this.btn_servoOff.Size = new System.Drawing.Size(75, 23);
            this.btn_servoOff.TabIndex = 1;
            this.btn_servoOff.Text = "OFF";
            this.btn_servoOff.UseVisualStyleBackColor = true;
            this.btn_servoOff.Click += new System.EventHandler(this.btn_servoOff_Click);
            // 
            // btn_PosLimtSignal
            // 
            this.btn_PosLimtSignal.Enabled = false;
            this.btn_PosLimtSignal.Location = new System.Drawing.Point(22, 27);
            this.btn_PosLimtSignal.Name = "btn_PosLimtSignal";
            this.btn_PosLimtSignal.Size = new System.Drawing.Size(75, 23);
            this.btn_PosLimtSignal.TabIndex = 1;
            this.btn_PosLimtSignal.Text = "正极限";
            this.btn_PosLimtSignal.UseVisualStyleBackColor = true;
            // 
            // btn_NegLimtSignal
            // 
            this.btn_NegLimtSignal.Enabled = false;
            this.btn_NegLimtSignal.Location = new System.Drawing.Point(103, 27);
            this.btn_NegLimtSignal.Name = "btn_NegLimtSignal";
            this.btn_NegLimtSignal.Size = new System.Drawing.Size(75, 23);
            this.btn_NegLimtSignal.TabIndex = 1;
            this.btn_NegLimtSignal.Text = "负极限";
            this.btn_NegLimtSignal.UseVisualStyleBackColor = true;
            // 
            // btn_OrgSignal
            // 
            this.btn_OrgSignal.Enabled = false;
            this.btn_OrgSignal.Location = new System.Drawing.Point(184, 27);
            this.btn_OrgSignal.Name = "btn_OrgSignal";
            this.btn_OrgSignal.Size = new System.Drawing.Size(75, 23);
            this.btn_OrgSignal.TabIndex = 1;
            this.btn_OrgSignal.Text = "原点";
            this.btn_OrgSignal.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.gb_info, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(565, 376);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // gb_info
            // 
            this.gb_info.Controls.Add(this.lbl_servoStatus);
            this.gb_info.Controls.Add(this.btn_servoOn);
            this.gb_info.Controls.Add(this.btn_servoOff);
            this.gb_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_info.Location = new System.Drawing.Point(3, 3);
            this.gb_info.Name = "gb_info";
            this.gb_info.Size = new System.Drawing.Size(559, 69);
            this.gb_info.TabIndex = 0;
            this.gb_info.TabStop = false;
            this.gb_info.Text = "使能";
            // 
            // lbl_servoStatus
            // 
            this.lbl_servoStatus.AutoSize = true;
            this.lbl_servoStatus.BackColor = System.Drawing.SystemColors.Info;
            this.lbl_servoStatus.Font = new System.Drawing.Font("宋体", 15F);
            this.lbl_servoStatus.ForeColor = System.Drawing.Color.Black;
            this.lbl_servoStatus.Location = new System.Drawing.Point(286, 28);
            this.lbl_servoStatus.Name = "lbl_servoStatus";
            this.lbl_servoStatus.Size = new System.Drawing.Size(139, 20);
            this.lbl_servoStatus.TabIndex = 3;
            this.lbl_servoStatus.Text = "使能状态:模拟";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tb_currentPulse);
            this.groupBox2.Controls.Add(this.tb_currentAbsPos);
            this.groupBox2.Controls.Add(this.btn_PosLimtSignal);
            this.groupBox2.Controls.Add(this.btn_NegLimtSignal);
            this.groupBox2.Controls.Add(this.btn_OrgSignal);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 78);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(559, 69);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "状态反馈";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(446, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "脉冲位置(pulse)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(288, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "绝对位置(mm或角度)";
            // 
            // tb_currentPulse
            // 
            this.tb_currentPulse.Location = new System.Drawing.Point(448, 32);
            this.tb_currentPulse.Name = "tb_currentPulse";
            this.tb_currentPulse.ReadOnly = true;
            this.tb_currentPulse.Size = new System.Drawing.Size(95, 21);
            this.tb_currentPulse.TabIndex = 2;
            // 
            // tb_currentAbsPos
            // 
            this.tb_currentAbsPos.Location = new System.Drawing.Point(290, 32);
            this.tb_currentAbsPos.Name = "tb_currentAbsPos";
            this.tb_currentAbsPos.ReadOnly = true;
            this.tb_currentAbsPos.Size = new System.Drawing.Size(95, 21);
            this.tb_currentAbsPos.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 153);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(559, 220);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(413, 214);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_NegRelMove);
            this.groupBox3.Controls.Add(this.btn_PosRelMove);
            this.groupBox3.Controls.Add(this.tb_planRelPos);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(407, 64);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "相对运动";
            // 
            // btn_NegRelMove
            // 
            this.btn_NegRelMove.Location = new System.Drawing.Point(203, 21);
            this.btn_NegRelMove.Name = "btn_NegRelMove";
            this.btn_NegRelMove.Size = new System.Drawing.Size(75, 23);
            this.btn_NegRelMove.TabIndex = 0;
            this.btn_NegRelMove.Text = "负向移动";
            this.btn_NegRelMove.UseVisualStyleBackColor = true;
            this.btn_NegRelMove.Click += new System.EventHandler(this.btn_NegRelMove_Click);
            // 
            // btn_PosRelMove
            // 
            this.btn_PosRelMove.Location = new System.Drawing.Point(112, 21);
            this.btn_PosRelMove.Name = "btn_PosRelMove";
            this.btn_PosRelMove.Size = new System.Drawing.Size(75, 23);
            this.btn_PosRelMove.TabIndex = 0;
            this.btn_PosRelMove.Text = "正向移动";
            this.btn_PosRelMove.UseVisualStyleBackColor = true;
            this.btn_PosRelMove.Click += new System.EventHandler(this.btn_PosRelMove_Click);
            // 
            // tb_planRelPos
            // 
            this.tb_planRelPos.Location = new System.Drawing.Point(16, 23);
            this.tb_planRelPos.Name = "tb_planRelPos";
            this.tb_planRelPos.Size = new System.Drawing.Size(75, 21);
            this.tb_planRelPos.TabIndex = 1;
            this.tb_planRelPos.Text = "0.0";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 70);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel3.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(413, 144);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.groupBox5, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(413, 144);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btn_NegJogMove);
            this.groupBox5.Controls.Add(this.btn_PosJogMove);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 75);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 66);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Jog";
            // 
            // btn_NegJogMove
            // 
            this.btn_NegJogMove.Location = new System.Drawing.Point(107, 25);
            this.btn_NegJogMove.Name = "btn_NegJogMove";
            this.btn_NegJogMove.Size = new System.Drawing.Size(75, 23);
            this.btn_NegJogMove.TabIndex = 0;
            this.btn_NegJogMove.Text = "负向Jog";
            this.btn_NegJogMove.UseVisualStyleBackColor = true;
            this.btn_NegJogMove.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_NegJogMove_MouseDown);
            this.btn_NegJogMove.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_NegJogMove_MouseUp);
            // 
            // btn_PosJogMove
            // 
            this.btn_PosJogMove.Location = new System.Drawing.Point(16, 25);
            this.btn_PosJogMove.Name = "btn_PosJogMove";
            this.btn_PosJogMove.Size = new System.Drawing.Size(75, 23);
            this.btn_PosJogMove.TabIndex = 0;
            this.btn_PosJogMove.Text = "正向Jog";
            this.btn_PosJogMove.UseVisualStyleBackColor = true;
            this.btn_PosJogMove.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_PosJogMove_MouseDown);
            this.btn_PosJogMove.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_PosJogMove_MouseUp);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_AbsMove);
            this.groupBox4.Controls.Add(this.tb_planAbsPos);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 66);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "绝对运动";
            // 
            // btn_AbsMove
            // 
            this.btn_AbsMove.Location = new System.Drawing.Point(107, 24);
            this.btn_AbsMove.Name = "btn_AbsMove";
            this.btn_AbsMove.Size = new System.Drawing.Size(75, 23);
            this.btn_AbsMove.TabIndex = 0;
            this.btn_AbsMove.Text = "运动";
            this.btn_AbsMove.UseVisualStyleBackColor = true;
            this.btn_AbsMove.Click += new System.EventHandler(this.btn_AbsMove_Click);
            // 
            // tb_planAbsPos
            // 
            this.tb_planAbsPos.Location = new System.Drawing.Point(16, 24);
            this.tb_planAbsPos.Name = "tb_planAbsPos";
            this.tb_planAbsPos.Size = new System.Drawing.Size(75, 21);
            this.tb_planAbsPos.TabIndex = 1;
            this.tb_planAbsPos.Text = "0.0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_stp_1);
            this.groupBox1.Controls.Add(this.btn_moveRelMicroStep_negative);
            this.groupBox1.Controls.Add(this.btn_moveRelMicroStep_positive);
            this.groupBox1.Controls.Add(this.rb_stp_0_1);
            this.groupBox1.Controls.Add(this.rb_stp_0_01);
            this.groupBox1.Controls.Add(this.rb_stp_0_0001);
            this.groupBox1.Controls.Add(this.rb_stp_0_001);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(209, 3);
            this.groupBox1.Name = "groupBox1";
            this.tableLayoutPanel5.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.Size = new System.Drawing.Size(201, 138);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "微步进运动(mm或角度)";
            // 
            // rb_stp_1
            // 
            this.rb_stp_1.AutoSize = true;
            this.rb_stp_1.Location = new System.Drawing.Point(18, 114);
            this.rb_stp_1.Name = "rb_stp_1";
            this.rb_stp_1.Size = new System.Drawing.Size(29, 16);
            this.rb_stp_1.TabIndex = 0;
            this.rb_stp_1.TabStop = true;
            this.rb_stp_1.Text = "1";
            this.rb_stp_1.UseVisualStyleBackColor = true;
            // 
            // btn_moveRelMicroStep_negative
            // 
            this.btn_moveRelMicroStep_negative.Location = new System.Drawing.Point(107, 96);
            this.btn_moveRelMicroStep_negative.Name = "btn_moveRelMicroStep_negative";
            this.btn_moveRelMicroStep_negative.Size = new System.Drawing.Size(75, 23);
            this.btn_moveRelMicroStep_negative.TabIndex = 0;
            this.btn_moveRelMicroStep_negative.Text = "负向移动";
            this.btn_moveRelMicroStep_negative.UseVisualStyleBackColor = true;
            this.btn_moveRelMicroStep_negative.Click += new System.EventHandler(this.btn_moveRelMicroStep_negative_Click);
            // 
            // btn_moveRelMicroStep_positive
            // 
            this.btn_moveRelMicroStep_positive.Location = new System.Drawing.Point(107, 31);
            this.btn_moveRelMicroStep_positive.Name = "btn_moveRelMicroStep_positive";
            this.btn_moveRelMicroStep_positive.Size = new System.Drawing.Size(75, 23);
            this.btn_moveRelMicroStep_positive.TabIndex = 0;
            this.btn_moveRelMicroStep_positive.Text = "正向移动";
            this.btn_moveRelMicroStep_positive.UseVisualStyleBackColor = true;
            this.btn_moveRelMicroStep_positive.Click += new System.EventHandler(this.btn_moveRelMicroStep_positive_Click);
            // 
            // rb_stp_0_1
            // 
            this.rb_stp_0_1.AutoSize = true;
            this.rb_stp_0_1.Location = new System.Drawing.Point(18, 91);
            this.rb_stp_0_1.Name = "rb_stp_0_1";
            this.rb_stp_0_1.Size = new System.Drawing.Size(41, 16);
            this.rb_stp_0_1.TabIndex = 0;
            this.rb_stp_0_1.TabStop = true;
            this.rb_stp_0_1.Text = "0.1";
            this.rb_stp_0_1.UseVisualStyleBackColor = true;
            // 
            // rb_stp_0_01
            // 
            this.rb_stp_0_01.AutoSize = true;
            this.rb_stp_0_01.Checked = true;
            this.rb_stp_0_01.Location = new System.Drawing.Point(18, 68);
            this.rb_stp_0_01.Name = "rb_stp_0_01";
            this.rb_stp_0_01.Size = new System.Drawing.Size(47, 16);
            this.rb_stp_0_01.TabIndex = 0;
            this.rb_stp_0_01.TabStop = true;
            this.rb_stp_0_01.Text = "0.01";
            this.rb_stp_0_01.UseVisualStyleBackColor = true;
            // 
            // rb_stp_0_001
            // 
            this.rb_stp_0_001.AutoSize = true;
            this.rb_stp_0_001.Location = new System.Drawing.Point(18, 45);
            this.rb_stp_0_001.Name = "rb_stp_0_001";
            this.rb_stp_0_001.Size = new System.Drawing.Size(53, 16);
            this.rb_stp_0_001.TabIndex = 0;
            this.rb_stp_0_001.TabStop = true;
            this.rb_stp_0_001.Text = "0.001";
            this.rb_stp_0_001.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.btn_AxisStop, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.groupBox6, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(422, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(134, 214);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // btn_AxisStop
            // 
            this.btn_AxisStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AxisStop.Location = new System.Drawing.Point(3, 73);
            this.btn_AxisStop.Name = "btn_AxisStop";
            this.btn_AxisStop.Size = new System.Drawing.Size(128, 138);
            this.btn_AxisStop.TabIndex = 0;
            this.btn_AxisStop.Text = "STOP";
            this.btn_AxisStop.UseVisualStyleBackColor = true;
            this.btn_AxisStop.Click += new System.EventHandler(this.btn_AxisStop_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chk_AxisHomed);
            this.groupBox6.Controls.Add(this.btn_HomeMove);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(128, 64);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "回零运动";
            // 
            // chk_AxisHomed
            // 
            this.chk_AxisHomed.AutoSize = true;
            this.chk_AxisHomed.Enabled = false;
            this.chk_AxisHomed.Location = new System.Drawing.Point(15, 45);
            this.chk_AxisHomed.Name = "chk_AxisHomed";
            this.chk_AxisHomed.Size = new System.Drawing.Size(102, 16);
            this.chk_AxisHomed.TabIndex = 2;
            this.chk_AxisHomed.Text = "Axis is Homed";
            this.chk_AxisHomed.UseVisualStyleBackColor = true;
            // 
            // btn_HomeMove
            // 
            this.btn_HomeMove.Location = new System.Drawing.Point(26, 20);
            this.btn_HomeMove.Name = "btn_HomeMove";
            this.btn_HomeMove.Size = new System.Drawing.Size(75, 23);
            this.btn_HomeMove.TabIndex = 0;
            this.btn_HomeMove.Text = "回零";
            this.btn_HomeMove.UseVisualStyleBackColor = true;
            this.btn_HomeMove.Click += new System.EventHandler(this.btn_HomeMove_Click);
            // 
            // rb_stp_0_0001
            // 
            this.rb_stp_0_0001.AutoSize = true;
            this.rb_stp_0_0001.Location = new System.Drawing.Point(18, 22);
            this.rb_stp_0_0001.Name = "rb_stp_0_0001";
            this.rb_stp_0_0001.Size = new System.Drawing.Size(59, 16);
            this.rb_stp_0_0001.TabIndex = 0;
            this.rb_stp_0_0001.TabStop = true;
            this.rb_stp_0_0001.Text = "0.0001";
            this.rb_stp_0_0001.UseVisualStyleBackColor = true;
            // 
            // Form_SingleAxisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 376);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_SingleAxisControl";
            this.Text = "Form_SingleAxisControl";
            this.Load += new System.EventHandler(this.Form_SingleAxisControl_Load);
            this.Resize += new System.EventHandler(this.Form_SingleAxisControl_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gb_info.ResumeLayout(false);
            this.gb_info.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_servoOn;
        private System.Windows.Forms.Button btn_servoOff;
        private System.Windows.Forms.Button btn_PosLimtSignal;
        private System.Windows.Forms.Button btn_NegLimtSignal;
        private System.Windows.Forms.Button btn_OrgSignal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.GroupBox gb_info;
        private System.Windows.Forms.Label lbl_servoStatus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_currentAbsPos;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btn_AxisStop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_NegRelMove;
        private System.Windows.Forms.Button btn_PosRelMove;
        private System.Windows.Forms.TextBox tb_planRelPos;
        private System.Windows.Forms.Button btn_NegJogMove;
        private System.Windows.Forms.Button btn_PosJogMove;
        private System.Windows.Forms.Button btn_AbsMove;
        private System.Windows.Forms.TextBox tb_planAbsPos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_currentPulse;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btn_HomeMove;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb_stp_1;
        private System.Windows.Forms.RadioButton rb_stp_0_1;
        private System.Windows.Forms.RadioButton rb_stp_0_01;
        private System.Windows.Forms.RadioButton rb_stp_0_001;
        private System.Windows.Forms.Button btn_moveRelMicroStep_negative;
        private System.Windows.Forms.Button btn_moveRelMicroStep_positive;
        private System.Windows.Forms.CheckBox chk_AxisHomed;
        private System.Windows.Forms.RadioButton rb_stp_0_0001;
    }
}