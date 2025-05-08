namespace SolveWare_TestComponents.UIComponents
{
    partial class Form_RawDataViewer_FF
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_RawDataViewer_FF));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_RawDataCollection = new System.Windows.Forms.DataGridView();
            this.dgv_RawDataParameters = new System.Windows.Forms.DataGridView();
            this.Col_Prop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnl_chart = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tp_DataCharts = new System.Windows.Forms.TabPage();
            this.tp_FFDataCharts = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_RawDataCollection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_RawDataParameters)).BeginInit();
            this.pnl_chart.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.dgv_RawDataCollection, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgv_RawDataParameters, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.pnl_chart, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(714, 592);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // dgv_RawDataCollection
            // 
            this.dgv_RawDataCollection.AllowUserToResizeColumns = false;
            this.dgv_RawDataCollection.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgv_RawDataCollection.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_RawDataCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_RawDataCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_RawDataCollection.Location = new System.Drawing.Point(3, 3);
            this.dgv_RawDataCollection.Name = "dgv_RawDataCollection";
            this.dgv_RawDataCollection.RowHeadersVisible = false;
            this.dgv_RawDataCollection.RowHeadersWidth = 51;
            this.tableLayoutPanel1.SetRowSpan(this.dgv_RawDataCollection, 2);
            this.dgv_RawDataCollection.RowTemplate.Height = 23;
            this.dgv_RawDataCollection.Size = new System.Drawing.Size(279, 586);
            this.dgv_RawDataCollection.TabIndex = 1;
            // 
            // dgv_RawDataParameters
            // 
            this.dgv_RawDataParameters.AllowUserToAddRows = false;
            this.dgv_RawDataParameters.AllowUserToDeleteRows = false;
            this.dgv_RawDataParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_RawDataParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_Prop,
            this.Col_Value});
            this.dgv_RawDataParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_RawDataParameters.Location = new System.Drawing.Point(288, 358);
            this.dgv_RawDataParameters.Name = "dgv_RawDataParameters";
            this.dgv_RawDataParameters.ReadOnly = true;
            this.dgv_RawDataParameters.RowHeadersVisible = false;
            this.dgv_RawDataParameters.RowHeadersWidth = 51;
            this.dgv_RawDataParameters.RowTemplate.Height = 23;
            this.dgv_RawDataParameters.Size = new System.Drawing.Size(423, 231);
            this.dgv_RawDataParameters.TabIndex = 3;
            // 
            // Col_Prop
            // 
            this.Col_Prop.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col_Prop.FillWeight = 50F;
            this.Col_Prop.HeaderText = "参数名称";
            this.Col_Prop.MinimumWidth = 6;
            this.Col_Prop.Name = "Col_Prop";
            this.Col_Prop.ReadOnly = true;
            this.Col_Prop.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Col_Value
            // 
            this.Col_Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col_Value.FillWeight = 50F;
            this.Col_Value.HeaderText = "值";
            this.Col_Value.MinimumWidth = 6;
            this.Col_Value.Name = "Col_Value";
            this.Col_Value.ReadOnly = true;
            this.Col_Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // pnl_chart
            // 
            this.pnl_chart.Controls.Add(this.tabControl1);
            this.pnl_chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_chart.Location = new System.Drawing.Point(288, 3);
            this.pnl_chart.Name = "pnl_chart";
            this.pnl_chart.Size = new System.Drawing.Size(423, 349);
            this.pnl_chart.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tp_DataCharts);
            this.tabControl1.Controls.Add(this.tp_FFDataCharts);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(423, 349);
            this.tabControl1.TabIndex = 0;
            // 
            // tp_DataCharts
            // 
            this.tp_DataCharts.Location = new System.Drawing.Point(4, 22);
            this.tp_DataCharts.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tp_DataCharts.Name = "tp_DataCharts";
            this.tp_DataCharts.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tp_DataCharts.Size = new System.Drawing.Size(415, 323);
            this.tp_DataCharts.TabIndex = 0;
            this.tp_DataCharts.Text = "DataCharts";
            this.tp_DataCharts.UseVisualStyleBackColor = true;
            // 
            // tp_FFDataCharts
            // 
            this.tp_FFDataCharts.Location = new System.Drawing.Point(4, 22);
            this.tp_FFDataCharts.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tp_FFDataCharts.Name = "tp_FFDataCharts";
            this.tp_FFDataCharts.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tp_FFDataCharts.Size = new System.Drawing.Size(415, 323);
            this.tp_FFDataCharts.TabIndex = 1;
            this.tp_FFDataCharts.Text = "FFDataCharts";
            this.tp_FFDataCharts.UseVisualStyleBackColor = true;
            // 
            // Form_RawDataViewer_FF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 592);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_RawDataViewer_FF";
            this.Text = "Form_RawDataViewer_FF";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_RawDataViewer_FormClosing);
            this.Load += new System.EventHandler(this.Form_RawDataViewer_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_RawDataCollection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_RawDataParameters)).EndInit();
            this.pnl_chart.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgv_RawDataCollection;
        private System.Windows.Forms.DataGridView dgv_RawDataParameters;
        private System.Windows.Forms.Panel pnl_chart;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Prop;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Value;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp_DataCharts;
        private System.Windows.Forms.TabPage tp_FFDataCharts;
    }
}