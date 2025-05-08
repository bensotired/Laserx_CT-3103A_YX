namespace TestPlugin_Demo
{
    partial class Form_PluginRuntimeOverview_CT3103
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Device_01");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Device_02");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Device_03");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Device_04");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Device_05");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Device_06");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Device_07");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Device_08");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Device_09");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Device_10");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Wafer_001", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_PluginRuntimeOverview_CT3103));
            this.label1 = new System.Windows.Forms.Label();
            this.tlp_ovMain = new System.Windows.Forms.TableLayoutPanel();
            this.tb_dataViewer = new System.Windows.Forms.TabControl();
            this.tp_DataCharts = new System.Windows.Forms.TabPage();
            this.tlp_fullDataViewlayer = new System.Windows.Forms.FlowLayoutPanel();
            this.tp_SummaryData = new System.Windows.Forms.TabPage();
            this.pdgv_summaryData = new SolveWare_TestComponents.UI.PropertyDataGirdView();
            this.tp_CoarseTuning = new System.Windows.Forms.TabPage();
            this.panel_coarsetuning = new System.Windows.Forms.Panel();
            this.tp_coarse = new System.Windows.Forms.TabPage();
            this.panel_coarse = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeView_MainStreamData = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chk_autoRefresh = new System.Windows.Forms.CheckBox();
            this.tlp_ovMain.SuspendLayout();
            this.tb_dataViewer.SuspendLayout();
            this.tp_DataCharts.SuspendLayout();
            this.tp_SummaryData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pdgv_summaryData)).BeginInit();
            this.tp_CoarseTuning.SuspendLayout();
            this.tp_coarse.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(323, 517);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 0;
            // 
            // tlp_ovMain
            // 
            this.tlp_ovMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tlp_ovMain.ColumnCount = 2;
            this.tlp_ovMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.90449F));
            this.tlp_ovMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.0955F));
            this.tlp_ovMain.Controls.Add(this.tb_dataViewer, 1, 0);
            this.tlp_ovMain.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tlp_ovMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_ovMain.Location = new System.Drawing.Point(0, 0);
            this.tlp_ovMain.Name = "tlp_ovMain";
            this.tlp_ovMain.RowCount = 1;
            this.tlp_ovMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_ovMain.Size = new System.Drawing.Size(1425, 761);
            this.tlp_ovMain.TabIndex = 3;
            // 
            // tb_dataViewer
            // 
            this.tb_dataViewer.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tb_dataViewer.Controls.Add(this.tp_DataCharts);
            this.tb_dataViewer.Controls.Add(this.tp_SummaryData);
            this.tb_dataViewer.Controls.Add(this.tp_CoarseTuning);
            this.tb_dataViewer.Controls.Add(this.tp_coarse);
            this.tb_dataViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_dataViewer.Location = new System.Drawing.Point(202, 4);
            this.tb_dataViewer.Name = "tb_dataViewer";
            this.tb_dataViewer.SelectedIndex = 0;
            this.tb_dataViewer.Size = new System.Drawing.Size(1219, 753);
            this.tb_dataViewer.TabIndex = 0;
            // 
            // tp_DataCharts
            // 
            this.tp_DataCharts.Controls.Add(this.tlp_fullDataViewlayer);
            this.tp_DataCharts.Location = new System.Drawing.Point(4, 25);
            this.tp_DataCharts.Name = "tp_DataCharts";
            this.tp_DataCharts.Padding = new System.Windows.Forms.Padding(3);
            this.tp_DataCharts.Size = new System.Drawing.Size(1211, 724);
            this.tp_DataCharts.TabIndex = 1;
            this.tp_DataCharts.Text = "DataCharts";
            this.tp_DataCharts.UseVisualStyleBackColor = true;
            // 
            // tlp_fullDataViewlayer
            // 
            this.tlp_fullDataViewlayer.AutoScroll = true;
            this.tlp_fullDataViewlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_fullDataViewlayer.Location = new System.Drawing.Point(3, 3);
            this.tlp_fullDataViewlayer.Name = "tlp_fullDataViewlayer";
            this.tlp_fullDataViewlayer.Size = new System.Drawing.Size(1205, 718);
            this.tlp_fullDataViewlayer.TabIndex = 0;
            this.tlp_fullDataViewlayer.Resize += new System.EventHandler(this.flow_layer_Resize);
            // 
            // tp_SummaryData
            // 
            this.tp_SummaryData.Controls.Add(this.pdgv_summaryData);
            this.tp_SummaryData.Location = new System.Drawing.Point(4, 25);
            this.tp_SummaryData.Name = "tp_SummaryData";
            this.tp_SummaryData.Padding = new System.Windows.Forms.Padding(3);
            this.tp_SummaryData.Size = new System.Drawing.Size(1211, 724);
            this.tp_SummaryData.TabIndex = 0;
            this.tp_SummaryData.Text = "SummaryData";
            this.tp_SummaryData.UseVisualStyleBackColor = true;
            // 
            // pdgv_summaryData
            // 
            this.pdgv_summaryData.AllowUserToAddRows = false;
            this.pdgv_summaryData.AllowUserToDeleteRows = false;
            this.pdgv_summaryData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Info;
            this.pdgv_summaryData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.pdgv_summaryData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pdgv_summaryData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdgv_summaryData.Location = new System.Drawing.Point(3, 3);
            this.pdgv_summaryData.Margin = new System.Windows.Forms.Padding(0);
            this.pdgv_summaryData.Name = "pdgv_summaryData";
            this.pdgv_summaryData.ReadOnly = true;
            this.pdgv_summaryData.RowHeadersVisible = false;
            this.pdgv_summaryData.RowHeadersWidth = 51;
            this.pdgv_summaryData.RowTemplate.Height = 23;
            this.pdgv_summaryData.Size = new System.Drawing.Size(1205, 718);
            this.pdgv_summaryData.TabIndex = 6;
            // 
            // tp_CoarseTuning
            // 
            this.tp_CoarseTuning.Controls.Add(this.panel_coarsetuning);
            this.tp_CoarseTuning.Location = new System.Drawing.Point(4, 25);
            this.tp_CoarseTuning.Name = "tp_CoarseTuning";
            this.tp_CoarseTuning.Size = new System.Drawing.Size(1211, 724);
            this.tp_CoarseTuning.TabIndex = 2;
            this.tp_CoarseTuning.Text = "CoarseTuning";
            this.tp_CoarseTuning.UseVisualStyleBackColor = true;
            // 
            // panel_coarsetuning
            // 
            this.panel_coarsetuning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_coarsetuning.Location = new System.Drawing.Point(0, 0);
            this.panel_coarsetuning.Name = "panel_coarsetuning";
            this.panel_coarsetuning.Size = new System.Drawing.Size(1211, 724);
            this.panel_coarsetuning.TabIndex = 0;
            // 
            // tp_coarse
            // 
            this.tp_coarse.Controls.Add(this.panel_coarse);
            this.tp_coarse.Location = new System.Drawing.Point(4, 25);
            this.tp_coarse.Name = "tp_coarse";
            this.tp_coarse.Padding = new System.Windows.Forms.Padding(3);
            this.tp_coarse.Size = new System.Drawing.Size(1211, 724);
            this.tp_coarse.TabIndex = 3;
            this.tp_coarse.Text = "coarsetuning";
            this.tp_coarse.UseVisualStyleBackColor = true;
            // 
            // panel_coarse
            // 
            this.panel_coarse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_coarse.Location = new System.Drawing.Point(3, 3);
            this.panel_coarse.Name = "panel_coarse";
            this.panel_coarse.Size = new System.Drawing.Size(1205, 718);
            this.panel_coarse.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.treeView_MainStreamData, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.388298F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.6117F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(191, 753);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // treeView_MainStreamData
            // 
            this.treeView_MainStreamData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_MainStreamData.Location = new System.Drawing.Point(4, 37);
            this.treeView_MainStreamData.Name = "treeView_MainStreamData";
            treeNode1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            treeNode1.Name = "节点1";
            treeNode1.Text = "Device_01";
            treeNode2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            treeNode2.Name = "节点4";
            treeNode2.Text = "Device_02";
            treeNode3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            treeNode3.Name = "节点5";
            treeNode3.Text = "Device_03";
            treeNode4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            treeNode4.Name = "节点6";
            treeNode4.Text = "Device_04";
            treeNode5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            treeNode5.Name = "节点7";
            treeNode5.Text = "Device_05";
            treeNode6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            treeNode6.Name = "节点8";
            treeNode6.Text = "Device_06";
            treeNode7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            treeNode7.Name = "节点9";
            treeNode7.Text = "Device_07";
            treeNode8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            treeNode8.Name = "节点10";
            treeNode8.Text = "Device_08";
            treeNode9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            treeNode9.Name = "节点11";
            treeNode9.Text = "Device_09";
            treeNode10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            treeNode10.Name = "节点12";
            treeNode10.Text = "Device_10";
            treeNode11.Name = "节点0";
            treeNode11.Text = "Wafer_001";
            this.treeView_MainStreamData.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode11});
            this.treeView_MainStreamData.PathSeparator = "$$";
            this.treeView_MainStreamData.Size = new System.Drawing.Size(183, 712);
            this.treeView_MainStreamData.TabIndex = 3;
            this.treeView_MainStreamData.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_MainStreamData_AfterSelect);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chk_autoRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(183, 26);
            this.panel1.TabIndex = 2;
            // 
            // chk_autoRefresh
            // 
            this.chk_autoRefresh.AutoSize = true;
            this.chk_autoRefresh.Checked = true;
            this.chk_autoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_autoRefresh.Location = new System.Drawing.Point(4, 4);
            this.chk_autoRefresh.Name = "chk_autoRefresh";
            this.chk_autoRefresh.Size = new System.Drawing.Size(72, 16);
            this.chk_autoRefresh.TabIndex = 0;
            this.chk_autoRefresh.Text = "自动刷新";
            this.chk_autoRefresh.UseVisualStyleBackColor = true;
            this.chk_autoRefresh.CheckedChanged += new System.EventHandler(this.chk_autoRefresh_CheckedChanged);
            // 
            // Form_PluginRuntimeOverview_CT3103
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1425, 761);
            this.Controls.Add(this.tlp_ovMain);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_PluginRuntimeOverview_CT3103";
            this.Text = "Form_PluginRuntimeOverview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_PluginRuntimeOverview_FormClosing);
            this.Load += new System.EventHandler(this.Form_PluginRuntimeOverview_Load);
            this.tlp_ovMain.ResumeLayout(false);
            this.tb_dataViewer.ResumeLayout(false);
            this.tp_DataCharts.ResumeLayout(false);
            this.tp_SummaryData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pdgv_summaryData)).EndInit();
            this.tp_CoarseTuning.ResumeLayout(false);
            this.tp_coarse.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tlp_ovMain;
        private System.Windows.Forms.TabControl tb_dataViewer;
        private System.Windows.Forms.TabPage tp_SummaryData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chk_autoRefresh;
        private SolveWare_TestComponents.UI.PropertyDataGirdView pdgv_summaryData;
        private System.Windows.Forms.TabPage tp_DataCharts;
        private System.Windows.Forms.TreeView treeView_MainStreamData;
        private System.Windows.Forms.FlowLayoutPanel tlp_fullDataViewlayer;
        private System.Windows.Forms.TabPage tp_CoarseTuning;
        private System.Windows.Forms.Panel panel_coarsetuning;
        private System.Windows.Forms.TabPage tp_coarse;
        private System.Windows.Forms.Panel panel_coarse;
    }
}