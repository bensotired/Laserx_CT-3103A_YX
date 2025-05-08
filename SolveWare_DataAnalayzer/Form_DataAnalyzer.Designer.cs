namespace SolveWare_DataAnalayzer
{
    partial class Form_DataAnalyzer
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
            this.btn_clearView = new System.Windows.Forms.Button();
            this.lv_StepComboFiles = new System.Windows.Forms.ListView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_selectSummaryDir = new System.Windows.Forms.Button();
            this.tb_summaryFile = new System.Windows.Forms.TextBox();
            this.btn_grabAllFilesData = new System.Windows.Forms.Button();
            this.num_distributingCount = new System.Windows.Forms.NumericUpDown();
            this.chk_distributingCount = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_distributingCount)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_clearView
            // 
            this.btn_clearView.Location = new System.Drawing.Point(879, 93);
            this.btn_clearView.Name = "btn_clearView";
            this.btn_clearView.Size = new System.Drawing.Size(151, 41);
            this.btn_clearView.TabIndex = 0;
            this.btn_clearView.Text = "清空原始数据文件列表";
            this.btn_clearView.UseVisualStyleBackColor = true;
            this.btn_clearView.Click += new System.EventHandler(this.btn_clearView_Click);
            // 
            // lv_StepComboFiles
            // 
            this.lv_StepComboFiles.AllowDrop = true;
            this.lv_StepComboFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_StepComboFiles.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lv_StepComboFiles.GridLines = true;
            this.lv_StepComboFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lv_StepComboFiles.HideSelection = false;
            this.lv_StepComboFiles.Location = new System.Drawing.Point(3, 3);
            this.lv_StepComboFiles.MultiSelect = false;
            this.lv_StepComboFiles.Name = "lv_StepComboFiles";
            this.lv_StepComboFiles.Size = new System.Drawing.Size(1057, 293);
            this.lv_StepComboFiles.TabIndex = 9;
            this.lv_StepComboFiles.UseCompatibleStateImageBehavior = false;
            this.lv_StepComboFiles.View = System.Windows.Forms.View.Details;
            this.lv_StepComboFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.lv_StepComboFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lv_StepComboFiles, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1063, 598);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chk_distributingCount);
            this.panel1.Controls.Add(this.num_distributingCount);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btn_selectSummaryDir);
            this.panel1.Controls.Add(this.tb_summaryFile);
            this.panel1.Controls.Add(this.btn_grabAllFilesData);
            this.panel1.Controls.Add(this.btn_clearView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 302);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1057, 293);
            this.panel1.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "数据汇总文件路径";
            // 
            // btn_selectSummaryDir
            // 
            this.btn_selectSummaryDir.Location = new System.Drawing.Point(955, 42);
            this.btn_selectSummaryDir.Name = "btn_selectSummaryDir";
            this.btn_selectSummaryDir.Size = new System.Drawing.Size(75, 23);
            this.btn_selectSummaryDir.TabIndex = 3;
            this.btn_selectSummaryDir.Text = "...";
            this.btn_selectSummaryDir.UseVisualStyleBackColor = true;
            this.btn_selectSummaryDir.Click += new System.EventHandler(this.btn_selectSummaryDir_Click);
            // 
            // tb_summaryFile
            // 
            this.tb_summaryFile.Location = new System.Drawing.Point(65, 42);
            this.tb_summaryFile.Name = "tb_summaryFile";
            this.tb_summaryFile.Size = new System.Drawing.Size(873, 21);
            this.tb_summaryFile.TabIndex = 2;
            // 
            // btn_grabAllFilesData
            // 
            this.btn_grabAllFilesData.Location = new System.Drawing.Point(65, 95);
            this.btn_grabAllFilesData.Name = "btn_grabAllFilesData";
            this.btn_grabAllFilesData.Size = new System.Drawing.Size(151, 41);
            this.btn_grabAllFilesData.TabIndex = 1;
            this.btn_grabAllFilesData.Text = "生成汇总文件";
            this.btn_grabAllFilesData.UseVisualStyleBackColor = true;
            this.btn_grabAllFilesData.Click += new System.EventHandler(this.btn_grabAllFilesData_Click);
            // 
            // num_distributingCount
            // 
            this.num_distributingCount.Location = new System.Drawing.Point(589, 115);
            this.num_distributingCount.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.num_distributingCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_distributingCount.Name = "num_distributingCount";
            this.num_distributingCount.Size = new System.Drawing.Size(120, 21);
            this.num_distributingCount.TabIndex = 5;
            this.num_distributingCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // chk_distributingCount
            // 
            this.chk_distributingCount.AutoSize = true;
            this.chk_distributingCount.Checked = true;
            this.chk_distributingCount.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_distributingCount.Location = new System.Drawing.Point(589, 93);
            this.chk_distributingCount.Name = "chk_distributingCount";
            this.chk_distributingCount.Size = new System.Drawing.Size(156, 16);
            this.chk_distributingCount.TabIndex = 7;
            this.chk_distributingCount.Text = "按测试次数划分统计数据";
            this.chk_distributingCount.UseVisualStyleBackColor = true;
            // 
            // Form_DataAnalyzer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 598);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form_DataAnalyzer";
            this.Text = "Form_DataAnalyzer";
            this.Load += new System.EventHandler(this.Form_DataAnalyzer_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_distributingCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_clearView;
        private System.Windows.Forms.ListView lv_StepComboFiles;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_grabAllFilesData;
        private System.Windows.Forms.Button btn_selectSummaryDir;
        private System.Windows.Forms.TextBox tb_summaryFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chk_distributingCount;
        private System.Windows.Forms.NumericUpDown num_distributingCount;
    }
}