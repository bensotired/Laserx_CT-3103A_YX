namespace SolveWare_TesterCore 
{
    partial class Form_TestExecutorCombo_ParamsEditor_SimpleMode
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tab_TestParamEditorLayer = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Close = new System.Windows.Forms.Button();
            this.btn_saveExecutorComboParams = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tab_TestParamEditorLayer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tab_TestParamEditorLayer, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1291, 827);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tab_TestParamEditorLayer
            // 
            this.tab_TestParamEditorLayer.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tab_TestParamEditorLayer.Controls.Add(this.tabPage1);
            this.tab_TestParamEditorLayer.Controls.Add(this.tabPage2);
            this.tab_TestParamEditorLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_TestParamEditorLayer.Location = new System.Drawing.Point(3, 3);
            this.tab_TestParamEditorLayer.Name = "tab_TestParamEditorLayer";
            this.tab_TestParamEditorLayer.SelectedIndex = 0;
            this.tab_TestParamEditorLayer.Size = new System.Drawing.Size(1285, 696);
            this.tab_TestParamEditorLayer.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1277, 670);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1277, 670);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_Close);
            this.panel1.Controls.Add(this.btn_saveExecutorComboParams);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 705);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1285, 119);
            this.panel1.TabIndex = 1;
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(729, 38);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(343, 42);
            this.btn_Close.TabIndex = 0;
            this.btn_Close.Text = "关闭";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // btn_saveExecutorComboParams
            // 
            this.btn_saveExecutorComboParams.Location = new System.Drawing.Point(119, 38);
            this.btn_saveExecutorComboParams.Name = "btn_saveExecutorComboParams";
            this.btn_saveExecutorComboParams.Size = new System.Drawing.Size(343, 42);
            this.btn_saveExecutorComboParams.TabIndex = 0;
            this.btn_saveExecutorComboParams.Text = "保存参数";
            this.btn_saveExecutorComboParams.UseVisualStyleBackColor = true;
            this.btn_saveExecutorComboParams.Click += new System.EventHandler(this.btn_saveExecutorComboParams_Click);
            // 
            // Form_TestExecutorCombo_ParamsEditor_SimpleMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1291, 827);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_TestExecutorCombo_ParamsEditor_SimpleMode";
            this.Text = "Form_TestExecutorCombo_ParamsEditor_SimpleMode";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form_TestExecutorCombo_ParamsEditor_SimpleMode_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tab_TestParamEditorLayer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tab_TestParamEditorLayer;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_saveExecutorComboParams;
        private System.Windows.Forms.Button btn_Close;
    }
}