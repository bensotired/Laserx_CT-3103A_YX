namespace SolveWare_TestComponents.Model
{
    partial class Form_TLP_layer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TLP_layer));
            this.tlp_layer = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // tlp_layer
            // 
            this.tlp_layer.ColumnCount = 2;
            this.tlp_layer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_layer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_layer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_layer.Location = new System.Drawing.Point(0, 0);
            this.tlp_layer.Name = "tlp_layer";
            this.tlp_layer.RowCount = 2;
            this.tlp_layer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_layer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_layer.Size = new System.Drawing.Size(800, 566);
            this.tlp_layer.TabIndex = 0;
            // 
            // Form_TLP_layer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 566);
            this.Controls.Add(this.tlp_layer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_TLP_layer";
            this.Text = "Form_TLP_layer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_TLP_layer_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlp_layer;
    }
}