
namespace SolveWare_DataBaseViewer
{
    partial class Form_ProductConfigSelector
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
            this.comboBox_ProductNames = new System.Windows.Forms.ComboBox();
            this.btn_startSql = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox_ProductNames
            // 
            this.comboBox_ProductNames.FormattingEnabled = true;
            this.comboBox_ProductNames.Location = new System.Drawing.Point(100, 42);
            this.comboBox_ProductNames.Name = "comboBox_ProductNames";
            this.comboBox_ProductNames.Size = new System.Drawing.Size(174, 20);
            this.comboBox_ProductNames.TabIndex = 0;
            // 
            // btn_startSql
            // 
            this.btn_startSql.Location = new System.Drawing.Point(100, 106);
            this.btn_startSql.Name = "btn_startSql";
            this.btn_startSql.Size = new System.Drawing.Size(174, 37);
            this.btn_startSql.TabIndex = 1;
            this.btn_startSql.Text = "启用数据库";
            this.btn_startSql.UseVisualStyleBackColor = true;
            this.btn_startSql.Click += new System.EventHandler(this.btn_startSql_Click);
            // 
            // Form_ProductConfigSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 199);
            this.Controls.Add(this.btn_startSql);
            this.Controls.Add(this.comboBox_ProductNames);
            this.Name = "Form_ProductConfigSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_ProductConfigSelector";
            this.Load += new System.EventHandler(this.Form_ProductConfigSelector_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_ProductNames;
        private System.Windows.Forms.Button btn_startSql;
    }
}