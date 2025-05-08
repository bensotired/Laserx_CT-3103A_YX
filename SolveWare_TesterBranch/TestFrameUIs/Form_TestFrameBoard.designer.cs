namespace SolveWare_TesterCore
{
    partial class Form_TestFrameBoard
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TestFrameBoard));
            this.tb_TestFrames = new System.Windows.Forms.TabControl();
            this.tp_TexeBuilder = new System.Windows.Forms.TabPage();
            this.tp_TexeComboBuilder = new System.Windows.Forms.TabPage();
            this.tp_TexeComboProfiler = new System.Windows.Forms.TabPage();
            this.tp_TestRecipeBuilder = new System.Windows.Forms.TabPage();
            this.tp_TcalcRecipeBuilder = new System.Windows.Forms.TabPage();
            this.tp_TexeComboInstrEditor = new System.Windows.Forms.TabPage();
            this.cms_ExecutorConfigItemTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cms_ExecutorComboTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_ExecutorCombo = new System.Windows.Forms.MenuStrip();
            this.窗口设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.浮动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.还原ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tb_TestFrames.SuspendLayout();
            this.cms_ExecutorConfigItemTreeView.SuspendLayout();
            this.cms_ExecutorComboTreeView.SuspendLayout();
            this.menu_ExecutorCombo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_TestFrames
            // 
            this.tb_TestFrames.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tb_TestFrames.Controls.Add(this.tp_TexeBuilder);
            this.tb_TestFrames.Controls.Add(this.tp_TexeComboBuilder);
            this.tb_TestFrames.Controls.Add(this.tp_TexeComboProfiler);
            this.tb_TestFrames.Controls.Add(this.tp_TestRecipeBuilder);
            this.tb_TestFrames.Controls.Add(this.tp_TcalcRecipeBuilder);
            this.tb_TestFrames.Controls.Add(this.tp_TexeComboInstrEditor);
            this.tb_TestFrames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_TestFrames.Location = new System.Drawing.Point(0, 25);
            this.tb_TestFrames.Multiline = true;
            this.tb_TestFrames.Name = "tb_TestFrames";
            this.tb_TestFrames.SelectedIndex = 0;
            this.tb_TestFrames.Size = new System.Drawing.Size(1164, 688);
            this.tb_TestFrames.TabIndex = 1;
            this.tb_TestFrames.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tb_TestFrames_Selecting);
            // 
            // tp_TexeBuilder
            // 
            this.tp_TexeBuilder.Location = new System.Drawing.Point(4, 25);
            this.tp_TexeBuilder.Name = "tp_TexeBuilder";
            this.tp_TexeBuilder.Padding = new System.Windows.Forms.Padding(3);
            this.tp_TexeBuilder.Size = new System.Drawing.Size(1156, 659);
            this.tp_TexeBuilder.TabIndex = 2;
            this.tp_TexeBuilder.Text = "测试项目配置";
            this.tp_TexeBuilder.UseVisualStyleBackColor = true;
            // 
            // tp_TexeComboBuilder
            // 
            this.tp_TexeComboBuilder.Location = new System.Drawing.Point(4, 25);
            this.tp_TexeComboBuilder.Name = "tp_TexeComboBuilder";
            this.tp_TexeComboBuilder.Padding = new System.Windows.Forms.Padding(3);
            this.tp_TexeComboBuilder.Size = new System.Drawing.Size(1156, 659);
            this.tp_TexeComboBuilder.TabIndex = 1;
            this.tp_TexeComboBuilder.Text = "测试链表配置";
            this.tp_TexeComboBuilder.UseVisualStyleBackColor = true;
            // 
            // tp_TexeComboProfiler
            // 
            this.tp_TexeComboProfiler.Location = new System.Drawing.Point(4, 25);
            this.tp_TexeComboProfiler.Name = "tp_TexeComboProfiler";
            this.tp_TexeComboProfiler.Padding = new System.Windows.Forms.Padding(3);
            this.tp_TexeComboProfiler.Size = new System.Drawing.Size(1156, 659);
            this.tp_TexeComboProfiler.TabIndex = 3;
            this.tp_TexeComboProfiler.Text = "测试链参数导入";
            this.tp_TexeComboProfiler.UseVisualStyleBackColor = true;
            // 
            // tp_TestRecipeBuilder
            // 
            this.tp_TestRecipeBuilder.Location = new System.Drawing.Point(4, 25);
            this.tp_TestRecipeBuilder.Name = "tp_TestRecipeBuilder";
            this.tp_TestRecipeBuilder.Size = new System.Drawing.Size(1156, 659);
            this.tp_TestRecipeBuilder.TabIndex = 5;
            this.tp_TestRecipeBuilder.Text = "测试条件编辑";
            this.tp_TestRecipeBuilder.UseVisualStyleBackColor = true;
            // 
            // tp_TcalcRecipeBuilder
            // 
            this.tp_TcalcRecipeBuilder.Location = new System.Drawing.Point(4, 25);
            this.tp_TcalcRecipeBuilder.Name = "tp_TcalcRecipeBuilder";
            this.tp_TcalcRecipeBuilder.Size = new System.Drawing.Size(1156, 659);
            this.tp_TcalcRecipeBuilder.TabIndex = 4;
            this.tp_TcalcRecipeBuilder.Text = "算子条件编辑";
            this.tp_TcalcRecipeBuilder.UseVisualStyleBackColor = true;
            // 
            // tp_TexeComboInstrEditor
            // 
            this.tp_TexeComboInstrEditor.Location = new System.Drawing.Point(4, 25);
            this.tp_TexeComboInstrEditor.Name = "tp_TexeComboInstrEditor";
            this.tp_TexeComboInstrEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tp_TexeComboInstrEditor.Size = new System.Drawing.Size(1156, 659);
            this.tp_TexeComboInstrEditor.TabIndex = 6;
            this.tp_TexeComboInstrEditor.Text = "测试链仪器配置与调试";
            this.tp_TexeComboInstrEditor.UseVisualStyleBackColor = true;
            // 
            // cms_ExecutorConfigItemTreeView
            // 
            this.cms_ExecutorConfigItemTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除ToolStripMenuItem});
            this.cms_ExecutorConfigItemTreeView.Name = "contextMenuStrip1";
            this.cms_ExecutorConfigItemTreeView.Size = new System.Drawing.Size(101, 26);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            // 
            // cms_ExecutorComboTreeView
            // 
            this.cms_ExecutorComboTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.cms_ExecutorComboTreeView.Name = "contextMenuStrip1";
            this.cms_ExecutorComboTreeView.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem1.Text = "删除";
            // 
            // menu_ExecutorCombo
            // 
            this.menu_ExecutorCombo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.窗口设置ToolStripMenuItem});
            this.menu_ExecutorCombo.Location = new System.Drawing.Point(0, 0);
            this.menu_ExecutorCombo.Name = "menu_ExecutorCombo";
            this.menu_ExecutorCombo.Size = new System.Drawing.Size(1164, 25);
            this.menu_ExecutorCombo.TabIndex = 3;
            this.menu_ExecutorCombo.Text = "menuStrip1";
            // 
            // 窗口设置ToolStripMenuItem
            // 
            this.窗口设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.浮动ToolStripMenuItem,
            this.还原ToolStripMenuItem});
            this.窗口设置ToolStripMenuItem.Name = "窗口设置ToolStripMenuItem";
            this.窗口设置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.窗口设置ToolStripMenuItem.Text = "窗口设置";
            // 
            // 浮动ToolStripMenuItem
            // 
            this.浮动ToolStripMenuItem.Name = "浮动ToolStripMenuItem";
            this.浮动ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.浮动ToolStripMenuItem.Text = "浮动";
            this.浮动ToolStripMenuItem.Click += new System.EventHandler(this.浮动ToolStripMenuItem_Click);
            // 
            // 还原ToolStripMenuItem
            // 
            this.还原ToolStripMenuItem.Name = "还原ToolStripMenuItem";
            this.还原ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.还原ToolStripMenuItem.Text = "还原";
            this.还原ToolStripMenuItem.Click += new System.EventHandler(this.还原ToolStripMenuItem_Click);
            // 
            // Form_TestFrameBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 713);
            this.Controls.Add(this.tb_TestFrames);
            this.Controls.Add(this.menu_ExecutorCombo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_TestFrameBoard";
            this.Text = "老化定制";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_TestFrameBoard_FormClosing);
            this.Load += new System.EventHandler(this.Form_TestFrameBoard_Load);
            this.tb_TestFrames.ResumeLayout(false);
            this.cms_ExecutorConfigItemTreeView.ResumeLayout(false);
            this.cms_ExecutorComboTreeView.ResumeLayout(false);
            this.menu_ExecutorCombo.ResumeLayout(false);
            this.menu_ExecutorCombo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tb_TestFrames;
        private System.Windows.Forms.TabPage tp_TexeComboBuilder;
        private System.Windows.Forms.ContextMenuStrip cms_ExecutorConfigItemTreeView;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cms_ExecutorComboTreeView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.MenuStrip menu_ExecutorCombo;
        private System.Windows.Forms.ToolStripMenuItem 窗口设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 浮动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 还原ToolStripMenuItem;
        private System.Windows.Forms.TabPage tp_TexeBuilder;
        private System.Windows.Forms.TabPage tp_TexeComboProfiler;
        private System.Windows.Forms.TabPage tp_TestRecipeBuilder;
        private System.Windows.Forms.TabPage tp_TcalcRecipeBuilder;
        private System.Windows.Forms.TabPage tp_TexeComboInstrEditor;
    }
}