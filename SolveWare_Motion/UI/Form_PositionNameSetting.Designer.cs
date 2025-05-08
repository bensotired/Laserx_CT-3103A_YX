
namespace SolveWare_Motion
{
    partial class Form_PositionNameSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_PositionNameSetting));
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_stophomingAllAxes = new System.Windows.Forms.Button();
            this.btn_homeAllAxes = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPositionName = new System.Windows.Forms.TextBox();
            this.gb_axesMotion = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(625, 75);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(177, 34);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "保存当前轴组坐标";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(855, 75);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(177, 34);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gb_axesMotion, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 142F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1337, 815);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_stophomingAllAxes);
            this.panel1.Controls.Add(this.btn_homeAllAxes);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Controls.Add(this.btnConfirm);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxPositionName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1329, 136);
            this.panel1.TabIndex = 0;
            // 
            // btn_stophomingAllAxes
            // 
            this.btn_stophomingAllAxes.Location = new System.Drawing.Point(1126, 75);
            this.btn_stophomingAllAxes.Name = "btn_stophomingAllAxes";
            this.btn_stophomingAllAxes.Size = new System.Drawing.Size(177, 34);
            this.btn_stophomingAllAxes.TabIndex = 2;
            this.btn_stophomingAllAxes.Text = "停止复位所有轴";
            this.btn_stophomingAllAxes.UseVisualStyleBackColor = true;
            this.btn_stophomingAllAxes.Click += new System.EventHandler(this.btn_stophomingAllAxes_Click);
            // 
            // btn_homeAllAxes
            // 
            this.btn_homeAllAxes.Location = new System.Drawing.Point(1126, 30);
            this.btn_homeAllAxes.Name = "btn_homeAllAxes";
            this.btn_homeAllAxes.Size = new System.Drawing.Size(177, 34);
            this.btn_homeAllAxes.TabIndex = 2;
            this.btn_homeAllAxes.Text = "按顺序复位所有轴";
            this.btn_homeAllAxes.UseVisualStyleBackColor = true;
            this.btn_homeAllAxes.Click += new System.EventHandler(this.btn_homeAllAxes_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(7, 6);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(510, 124);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "1\n2\n3\n4\n5\n6";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(566, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "坐标名称";
            // 
            // textBoxPositionName
            // 
            this.textBoxPositionName.Location = new System.Drawing.Point(625, 38);
            this.textBoxPositionName.Name = "textBoxPositionName";
            this.textBoxPositionName.Size = new System.Drawing.Size(407, 21);
            this.textBoxPositionName.TabIndex = 1;
            // 
            // gb_axesMotion
            // 
            this.gb_axesMotion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb_axesMotion.Location = new System.Drawing.Point(4, 147);
            this.gb_axesMotion.Name = "gb_axesMotion";
            this.gb_axesMotion.Size = new System.Drawing.Size(1329, 664);
            this.gb_axesMotion.TabIndex = 1;
            this.gb_axesMotion.TabStop = false;
            this.gb_axesMotion.Text = "轴运动";
            // 
            // Form_PositionNameSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1337, 815);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_PositionNameSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_PositionNameSetting";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form_PositionNameSetting_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxPositionName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gb_axesMotion;
        private System.Windows.Forms.Button btn_homeAllAxes;
        private System.Windows.Forms.Button btn_stophomingAllAxes;
    }
}