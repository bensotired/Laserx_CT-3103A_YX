using System.Windows.Forms;

namespace SolveWare_TestPlugin
{
    partial class Form_MainPage_TestPlugin<TTestPlugin>
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


            this.SuspendLayout();


            // 
            // Form_MainPage_TestPlugin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 348);


            this.Name = "Form_MainPage_TestPlugin";
            this.Text = "Form_MainPage_TestPlugin";


            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected void CreateMenuMain(Form pluninMainPage)
        { 
            var MenuMain = new System.Windows.Forms.MenuStrip();
            var 产品参数配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
              MenuMain.SuspendLayout();
            // 
            // MenuMain
            // 
            MenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            产品参数配置ToolStripMenuItem});
            MenuMain.Location = new System.Drawing.Point(0, 0);
            MenuMain.Name = "MenuMain";
            MenuMain.Size = new System.Drawing.Size(404, 25);
            MenuMain.TabIndex = 0;
            MenuMain.Text = "menuStrip1";

            // 
            // 产品参数配置ToolStripMenuItem
            // 
            产品参数配置ToolStripMenuItem.Name = "产品参数配置ToolStripMenuItem";
            产品参数配置ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            产品参数配置ToolStripMenuItem.Text = "产品参数配置";

            pluninMainPage.Controls.Add( MenuMain);
         
            MenuMain.ResumeLayout(false);
            MenuMain.PerformLayout();
        }

        protected virtual void CreateStatusStripMain(Form pluninMainPage)
        {
            var StatusStripMain = new System.Windows.Forms.StatusStrip();
            var tssl_CurrentProductInfo = new System.Windows.Forms.ToolStripStatusLabel();
            StatusStripMain.SuspendLayout();
            // 
            // StatusStripMain
            // 
            StatusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            tssl_CurrentProductInfo});
            StatusStripMain.Location = new System.Drawing.Point(0, 326);
            StatusStripMain.Name = "StatusStripMain";
            StatusStripMain.Size = new System.Drawing.Size(404, 22);
            StatusStripMain.TabIndex = 1;
            StatusStripMain.Text = "statusStrip1";
          
            // 
            // tssl_CurrentProductInfo
            // 
            tssl_CurrentProductInfo.Name = "tssl_CurrentProductInfo";
            tssl_CurrentProductInfo.Size = new System.Drawing.Size(133, 17);
            tssl_CurrentProductInfo.Text = "当前产品配置类型 : [--]";

            pluninMainPage.Controls.Add(StatusStripMain);
      
            StatusStripMain.ResumeLayout(false);
            StatusStripMain.PerformLayout();
        }


        //protected System.Windows.Forms.MenuStrip MenuMain;
        //protected System.Windows.Forms.StatusStrip StatusStripMain;
        //protected System.Windows.Forms.ToolStripMenuItem 产品参数配置ToolStripMenuItem;
        //protected System.Windows.Forms.ToolStripStatusLabel tssl_CurrentProductInfo;
    }
}