namespace SolveWare_BurnInInstruments
{
    partial class Thorlabs_NanoTrakUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Thorlabs_NanoTrakUI));
            this.axMG17NanoTrak1 = new AxMG17NanoTrakLib.AxMG17NanoTrak();
            ((System.ComponentModel.ISupportInitialize)(this.axMG17NanoTrak1)).BeginInit();
            this.SuspendLayout();
            // 
            // axMG17NanoTrak1
            // 
            this.axMG17NanoTrak1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMG17NanoTrak1.Enabled = true;
            this.axMG17NanoTrak1.Location = new System.Drawing.Point(0, 0);
            this.axMG17NanoTrak1.Margin = new System.Windows.Forms.Padding(2);
            this.axMG17NanoTrak1.Name = "axMG17NanoTrak1";
            this.axMG17NanoTrak1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMG17NanoTrak1.OcxState")));
            this.axMG17NanoTrak1.Size = new System.Drawing.Size(674, 320);
            this.axMG17NanoTrak1.TabIndex = 0;
            // 
            // Thorlabs_NanoTrakUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 320);
            this.Controls.Add(this.axMG17NanoTrak1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Thorlabs_NanoTrakUI";
            this.Text = "NanoTrakUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Thorlabs_NanoTrakUI_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.axMG17NanoTrak1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxMG17NanoTrakLib.AxMG17NanoTrak axMG17NanoTrak1;
    }
}