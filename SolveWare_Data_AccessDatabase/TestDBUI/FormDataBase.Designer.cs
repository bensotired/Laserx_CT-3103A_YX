
namespace SolveWare_Data_AccessDatabase.TestDBUI
{
    partial class FormDataBase
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridView_ColumnName = new System.Windows.Forms.DataGridView();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridView_TableData = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.Export = new System.Windows.Forms.Button();
            this.comboBox1_Table = new System.Windows.Forms.ComboBox();
            this.dateTimePicker_StartTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_EndTime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_fail = new System.Windows.Forms.RadioButton();
            this.radioButton_pass = new System.Windows.Forms.RadioButton();
            this.radioButton_all = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_Parttnumber = new System.Windows.Forms.ComboBox();
            this.textBox_Fail = new System.Windows.Forms.TextBox();
            this.textBox_Pass = new System.Windows.Forms.TextBox();
            this.textBox_All = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chartTEC = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ColumnName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TableData)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTEC)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_ColumnName
            // 
            this.dataGridView_ColumnName.AllowUserToAddRows = false;
            this.dataGridView_ColumnName.AllowUserToDeleteRows = false;
            this.dataGridView_ColumnName.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_ColumnName.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column3});
            this.dataGridView_ColumnName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_ColumnName.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_ColumnName.Name = "dataGridView_ColumnName";
            this.dataGridView_ColumnName.RowHeadersVisible = false;
            this.dataGridView_ColumnName.RowTemplate.Height = 23;
            this.dataGridView_ColumnName.Size = new System.Drawing.Size(408, 383);
            this.dataGridView_ColumnName.TabIndex = 1;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "可选条件";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "选择";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.Width = 80;
            // 
            // dataGridView_TableData
            // 
            this.dataGridView_TableData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_TableData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_TableData.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_TableData.Name = "dataGridView_TableData";
            this.dataGridView_TableData.RowHeadersVisible = false;
            this.dataGridView_TableData.RowTemplate.Height = 23;
            this.dataGridView_TableData.Size = new System.Drawing.Size(1022, 383);
            this.dataGridView_TableData.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(92, 183);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 39);
            this.button1.TabIndex = 2;
            this.button1.Text = "查询";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Select_Click);
            // 
            // Export
            // 
            this.Export.Location = new System.Drawing.Point(224, 183);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(88, 39);
            this.Export.TabIndex = 3;
            this.Export.Text = "导出";
            this.Export.UseVisualStyleBackColor = true;
            this.Export.Click += new System.EventHandler(this.Export_Click);
            // 
            // comboBox1_Table
            // 
            this.comboBox1_Table.FormattingEnabled = true;
            this.comboBox1_Table.Location = new System.Drawing.Point(129, 33);
            this.comboBox1_Table.Name = "comboBox1_Table";
            this.comboBox1_Table.Size = new System.Drawing.Size(194, 20);
            this.comboBox1_Table.TabIndex = 4;
            this.comboBox1_Table.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_Table_SelectionChangeCommitted);
            // 
            // dateTimePicker_StartTime
            // 
            this.dateTimePicker_StartTime.Checked = false;
            this.dateTimePicker_StartTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dateTimePicker_StartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_StartTime.Location = new System.Drawing.Point(129, 89);
            this.dateTimePicker_StartTime.Name = "dateTimePicker_StartTime";
            this.dateTimePicker_StartTime.ShowCheckBox = true;
            this.dateTimePicker_StartTime.Size = new System.Drawing.Size(194, 21);
            this.dateTimePicker_StartTime.TabIndex = 5;
            // 
            // dateTimePicker_EndTime
            // 
            this.dateTimePicker_EndTime.Checked = false;
            this.dateTimePicker_EndTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dateTimePicker_EndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_EndTime.Location = new System.Drawing.Point(129, 135);
            this.dateTimePicker_EndTime.Name = "dateTimePicker_EndTime";
            this.dateTimePicker_EndTime.ShowCheckBox = true;
            this.dateTimePicker_EndTime.Size = new System.Drawing.Size(194, 21);
            this.dateTimePicker_EndTime.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "开始时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "结束时间";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "可选数据表";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.radioButton_fail);
            this.groupBox1.Controls.Add(this.radioButton_pass);
            this.groupBox1.Controls.Add(this.radioButton_all);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.comboBox_Parttnumber);
            this.groupBox1.Controls.Add(this.textBox_Fail);
            this.groupBox1.Controls.Add(this.textBox_Pass);
            this.groupBox1.Controls.Add(this.textBox_All);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateTimePicker_EndTime);
            this.groupBox1.Controls.Add(this.dateTimePicker_StartTime);
            this.groupBox1.Controls.Add(this.comboBox1_Table);
            this.groupBox1.Controls.Add(this.Export);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(408, 384);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询";
            // 
            // radioButton_fail
            // 
            this.radioButton_fail.AutoSize = true;
            this.radioButton_fail.Location = new System.Drawing.Point(128, 302);
            this.radioButton_fail.Name = "radioButton_fail";
            this.radioButton_fail.Size = new System.Drawing.Size(47, 16);
            this.radioButton_fail.TabIndex = 20;
            this.radioButton_fail.TabStop = true;
            this.radioButton_fail.Text = "fail";
            this.radioButton_fail.UseVisualStyleBackColor = true;
            // 
            // radioButton_pass
            // 
            this.radioButton_pass.AutoSize = true;
            this.radioButton_pass.Location = new System.Drawing.Point(75, 302);
            this.radioButton_pass.Name = "radioButton_pass";
            this.radioButton_pass.Size = new System.Drawing.Size(47, 16);
            this.radioButton_pass.TabIndex = 19;
            this.radioButton_pass.TabStop = true;
            this.radioButton_pass.Text = "pass";
            this.radioButton_pass.UseVisualStyleBackColor = true;
            // 
            // radioButton_all
            // 
            this.radioButton_all.AutoSize = true;
            this.radioButton_all.Checked = true;
            this.radioButton_all.Location = new System.Drawing.Point(28, 302);
            this.radioButton_all.Name = "radioButton_all";
            this.radioButton_all.Size = new System.Drawing.Size(41, 16);
            this.radioButton_all.TabIndex = 18;
            this.radioButton_all.TabStop = true;
            this.radioButton_all.Text = "all";
            this.radioButton_all.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 261);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "Partnumber";
            // 
            // comboBox_Parttnumber
            // 
            this.comboBox_Parttnumber.FormattingEnabled = true;
            this.comboBox_Parttnumber.Location = new System.Drawing.Point(75, 258);
            this.comboBox_Parttnumber.Name = "comboBox_Parttnumber";
            this.comboBox_Parttnumber.Size = new System.Drawing.Size(121, 20);
            this.comboBox_Parttnumber.TabIndex = 16;
            this.comboBox_Parttnumber.SelectionChangeCommitted += new System.EventHandler(this.comboBox_Parttnumber_SelectionChangeCommitted);
            // 
            // textBox_Fail
            // 
            this.textBox_Fail.Location = new System.Drawing.Point(308, 331);
            this.textBox_Fail.Name = "textBox_Fail";
            this.textBox_Fail.Size = new System.Drawing.Size(70, 21);
            this.textBox_Fail.TabIndex = 15;
            this.textBox_Fail.Text = "0";
            this.textBox_Fail.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Pass
            // 
            this.textBox_Pass.Location = new System.Drawing.Point(308, 297);
            this.textBox_Pass.Name = "textBox_Pass";
            this.textBox_Pass.Size = new System.Drawing.Size(70, 21);
            this.textBox_Pass.TabIndex = 14;
            this.textBox_Pass.Text = "0";
            this.textBox_Pass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_All
            // 
            this.textBox_All.Location = new System.Drawing.Point(308, 259);
            this.textBox_All.Name = "textBox_All";
            this.textBox_All.Size = new System.Drawing.Size(70, 21);
            this.textBox_All.TabIndex = 13;
            this.textBox_All.Text = "0";
            this.textBox_All.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(226, 334);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "坏品数量：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(226, 300);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "良品数量：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(226, 266);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "测试数量：";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.07212F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.64078F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.19255F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.80745F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1442, 779);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // panel3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel3, 2);
            this.panel3.Controls.Add(this.dataGridView_TableData);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 393);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1022, 383);
            this.panel3.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1031, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 384);
            this.panel1.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView_ColumnName);
            this.panel2.Location = new System.Drawing.Point(1031, 393);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(408, 383);
            this.panel2.TabIndex = 14;
            // 
            // panel4
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel4, 2);
            this.panel4.Controls.Add(this.chartTEC);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1022, 384);
            this.panel4.TabIndex = 15;
            // 
            // chartTEC
            // 
            chartArea2.Name = "ChartArea1";
            this.chartTEC.ChartAreas.Add(chartArea2);
            this.chartTEC.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chartTEC.Legends.Add(legend2);
            this.chartTEC.Location = new System.Drawing.Point(0, 0);
            this.chartTEC.Name = "chartTEC";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "实际电阻";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.Name = "理想电阻";
            this.chartTEC.Series.Add(series3);
            this.chartTEC.Series.Add(series4);
            this.chartTEC.Size = new System.Drawing.Size(1022, 384);
            this.chartTEC.TabIndex = 0;
            this.chartTEC.Text = "chart1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(8, 369);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(335, 12);
            this.label8.TabIndex = 9;
            this.label8.Text = "↓↓↓↓↓可选条件(红色）选择一个时可显示图案↓↓↓↓↓";
            // 
            // FormDataBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1442, 779);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormDataBase";
            this.Text = "FormDataBase";
            this.Load += new System.EventHandler(this.FormDataBase_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ColumnName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_TableData)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTEC)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView_TableData;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button Export;
        private System.Windows.Forms.ComboBox comboBox1_Table;
        private System.Windows.Forms.DateTimePicker dateTimePicker_StartTime;
        private System.Windows.Forms.DataGridView dataGridView_ColumnName;
        private System.Windows.Forms.DateTimePicker dateTimePicker_EndTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTEC;
        private System.Windows.Forms.TextBox textBox_Fail;
        private System.Windows.Forms.TextBox textBox_Pass;
        private System.Windows.Forms.TextBox textBox_All;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton radioButton_fail;
        private System.Windows.Forms.RadioButton radioButton_pass;
        private System.Windows.Forms.RadioButton radioButton_all;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_Parttnumber;
        private System.Windows.Forms.Label label8;
    }
}