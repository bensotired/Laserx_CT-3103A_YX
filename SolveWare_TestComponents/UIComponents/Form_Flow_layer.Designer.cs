namespace SolveWare_TestComponents.Model
{
    partial class Form_Flow_layer
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
            this.flow_layer = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flow_layer
            // 
            this.flow_layer.AutoScroll = true;
            this.flow_layer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flow_layer.Location = new System.Drawing.Point(0, 0);
            this.flow_layer.Name = "flow_layer";
            this.flow_layer.Size = new System.Drawing.Size(800, 566);
            this.flow_layer.TabIndex = 0;
            this.flow_layer.Resize += new System.EventHandler(this.flow_layer_Resize);
            // 
            // Form_Flow_layer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 566);
            this.Controls.Add(this.flow_layer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Flow_layer";
            this.Text = "Form_TLP_layer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Flow_layer_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flow_layer;
    }
}