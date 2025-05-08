namespace SolveWare_TesterCore
{
    partial class Form_MessageBorad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_MessageBorad));
            this.tab_messageBoard = new System.Windows.Forms.TabControl();
            this.tp_normalLog = new System.Windows.Forms.TabPage();
            this.rtb_Log = new System.Windows.Forms.RichTextBox();
            this.tp_exceptionLog = new System.Windows.Forms.TabPage();
            this.rtb_exceptionLog = new System.Windows.Forms.RichTextBox();
            this.cms_MessageBoard = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.窗口浮动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.窗口停靠ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tab_messageBoard.SuspendLayout();
            this.tp_normalLog.SuspendLayout();
            this.tp_exceptionLog.SuspendLayout();
            this.cms_MessageBoard.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab_messageBoard
            // 
            this.tab_messageBoard.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tab_messageBoard.Controls.Add(this.tp_normalLog);
            this.tab_messageBoard.Controls.Add(this.tp_exceptionLog);
            this.tab_messageBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab_messageBoard.Location = new System.Drawing.Point(0, 0);
            this.tab_messageBoard.Multiline = true;
            this.tab_messageBoard.Name = "tab_messageBoard";
            this.tab_messageBoard.SelectedIndex = 0;
            this.tab_messageBoard.Size = new System.Drawing.Size(1321, 807);
            this.tab_messageBoard.TabIndex = 0;
            // 
            // tp_normalLog
            // 
            this.tp_normalLog.Controls.Add(this.rtb_Log);
            this.tp_normalLog.Location = new System.Drawing.Point(4, 4);
            this.tp_normalLog.Name = "tp_normalLog";
            this.tp_normalLog.Size = new System.Drawing.Size(1313, 781);
            this.tp_normalLog.TabIndex = 0;
            this.tp_normalLog.Text = "日志";
            this.tp_normalLog.UseVisualStyleBackColor = true;
            // 
            // rtb_Log
            // 
            this.rtb_Log.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Log.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtb_Log.Location = new System.Drawing.Point(0, 0);
            this.rtb_Log.Name = "rtb_Log";
            this.rtb_Log.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtb_Log.Size = new System.Drawing.Size(1313, 781);
            this.rtb_Log.TabIndex = 0;
            this.rtb_Log.Text = "";
            this.rtb_Log.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MessageBoardHandle_MouseDown);
            // 
            // tp_exceptionLog
            // 
            this.tp_exceptionLog.Controls.Add(this.rtb_exceptionLog);
            this.tp_exceptionLog.Location = new System.Drawing.Point(4, 4);
            this.tp_exceptionLog.Name = "tp_exceptionLog";
            this.tp_exceptionLog.Size = new System.Drawing.Size(1313, 781);
            this.tp_exceptionLog.TabIndex = 1;
            this.tp_exceptionLog.Text = "异常信息[-]条";
            // 
            // rtb_exceptionLog
            // 
            this.rtb_exceptionLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_exceptionLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_exceptionLog.Location = new System.Drawing.Point(0, 0);
            this.rtb_exceptionLog.Name = "rtb_exceptionLog";
            this.rtb_exceptionLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtb_exceptionLog.Size = new System.Drawing.Size(1313, 781);
            this.rtb_exceptionLog.TabIndex = 1;
            this.rtb_exceptionLog.Text = "";
            this.rtb_exceptionLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MessageBoardHandle_MouseDown);
            // 
            // cms_MessageBoard
            // 
            this.cms_MessageBoard.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.窗口浮动ToolStripMenuItem,
            this.窗口停靠ToolStripMenuItem});
            this.cms_MessageBoard.Name = "cms_MessageBoard";
            this.cms_MessageBoard.Size = new System.Drawing.Size(125, 48);
            // 
            // 窗口浮动ToolStripMenuItem
            // 
            this.窗口浮动ToolStripMenuItem.Name = "窗口浮动ToolStripMenuItem";
            this.窗口浮动ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.窗口浮动ToolStripMenuItem.Text = "窗口浮动";
            this.窗口浮动ToolStripMenuItem.Click += new System.EventHandler(this.窗口浮动ToolStripMenuItem_Click);
            // 
            // 窗口停靠ToolStripMenuItem
            // 
            this.窗口停靠ToolStripMenuItem.Name = "窗口停靠ToolStripMenuItem";
            this.窗口停靠ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.窗口停靠ToolStripMenuItem.Text = "窗口还原";
            this.窗口停靠ToolStripMenuItem.Click += new System.EventHandler(this.窗口停靠ToolStripMenuItem_Click);
            // 
            // Form_MessageBorad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 807);
            this.Controls.Add(this.tab_messageBoard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "Form_MessageBorad";
            this.Text = "运行信息";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_MessageBorad_FormClosing);
            this.tab_messageBoard.ResumeLayout(false);
            this.tp_normalLog.ResumeLayout(false);
            this.tp_exceptionLog.ResumeLayout(false);
            this.cms_MessageBoard.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab_messageBoard;
        private System.Windows.Forms.TabPage tp_normalLog;
        private System.Windows.Forms.TabPage tp_exceptionLog;
        private System.Windows.Forms.RichTextBox rtb_Log;
        private System.Windows.Forms.RichTextBox rtb_exceptionLog;
        private System.Windows.Forms.ContextMenuStrip cms_MessageBoard;
        private System.Windows.Forms.ToolStripMenuItem 窗口浮动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 窗口停靠ToolStripMenuItem;
    }
}