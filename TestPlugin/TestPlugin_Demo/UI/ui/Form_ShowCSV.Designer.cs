
namespace TestPlugin_Demo
{
    partial class Form_ShowCSV
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
            this.listView_CarrierNumber = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label_chip = new System.Windows.Forms.Label();
            this.listView_chipnumber = new System.Windows.Forms.ListView();
            this.bt_getCarrier = new System.Windows.Forms.Button();
            this.bt_OK = new System.Windows.Forms.Button();
            this.bt_exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView_CarrierNumber
            // 
            this.listView_CarrierNumber.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView_CarrierNumber.FullRowSelect = true;
            this.listView_CarrierNumber.GridLines = true;
            this.listView_CarrierNumber.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView_CarrierNumber.HideSelection = false;
            this.listView_CarrierNumber.Location = new System.Drawing.Point(42, 36);
            this.listView_CarrierNumber.Name = "listView_CarrierNumber";
            this.listView_CarrierNumber.Size = new System.Drawing.Size(255, 316);
            this.listView_CarrierNumber.TabIndex = 0;
            this.listView_CarrierNumber.UseCompatibleStateImageBehavior = false;
            this.listView_CarrierNumber.View = System.Windows.Forms.View.Details;
            this.listView_CarrierNumber.SelectedIndexChanged += new System.EventHandler(this.listView_CarrierNumber_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(38, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "CarrierNumber";
            // 
            // label_chip
            // 
            this.label_chip.AutoSize = true;
            this.label_chip.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_chip.Location = new System.Drawing.Point(475, 14);
            this.label_chip.Name = "label_chip";
            this.label_chip.Size = new System.Drawing.Size(119, 19);
            this.label_chip.TabIndex = 3;
            this.label_chip.Text = "ChipNumber";
            // 
            // listView_chipnumber
            // 
            this.listView_chipnumber.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView_chipnumber.GridLines = true;
            this.listView_chipnumber.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView_chipnumber.HideSelection = false;
            this.listView_chipnumber.Location = new System.Drawing.Point(479, 36);
            this.listView_chipnumber.Name = "listView_chipnumber";
            this.listView_chipnumber.Size = new System.Drawing.Size(636, 316);
            this.listView_chipnumber.TabIndex = 2;
            this.listView_chipnumber.UseCompatibleStateImageBehavior = false;
            this.listView_chipnumber.View = System.Windows.Forms.View.Details;
            // 
            // bt_getCarrier
            // 
            this.bt_getCarrier.Location = new System.Drawing.Point(347, 36);
            this.bt_getCarrier.Name = "bt_getCarrier";
            this.bt_getCarrier.Size = new System.Drawing.Size(91, 46);
            this.bt_getCarrier.TabIndex = 4;
            this.bt_getCarrier.Text = "重新读取CarrierNumber";
            this.bt_getCarrier.UseVisualStyleBackColor = true;
            this.bt_getCarrier.Click += new System.EventHandler(this.bt_getCarrier_Click);
            // 
            // bt_OK
            // 
            this.bt_OK.Location = new System.Drawing.Point(347, 127);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(91, 46);
            this.bt_OK.TabIndex = 5;
            this.bt_OK.Text = "确认";
            this.bt_OK.UseVisualStyleBackColor = true;
            this.bt_OK.Click += new System.EventHandler(this.bt_OK_Click);
            // 
            // bt_exit
            // 
            this.bt_exit.Location = new System.Drawing.Point(347, 214);
            this.bt_exit.Name = "bt_exit";
            this.bt_exit.Size = new System.Drawing.Size(91, 46);
            this.bt_exit.TabIndex = 6;
            this.bt_exit.Text = "退出";
            this.bt_exit.UseVisualStyleBackColor = true;
            this.bt_exit.Click += new System.EventHandler(this.bt_exit_Click);
            // 
            // Form_ShowCSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1127, 379);
            this.Controls.Add(this.bt_exit);
            this.Controls.Add(this.bt_OK);
            this.Controls.Add(this.bt_getCarrier);
            this.Controls.Add(this.label_chip);
            this.Controls.Add(this.listView_chipnumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView_CarrierNumber);
            this.Name = "Form_ShowCSV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_ShowCSV";
            this.Load += new System.EventHandler(this.Form_ShowCSV_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_CarrierNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_chip;
        private System.Windows.Forms.ListView listView_chipnumber;
        private System.Windows.Forms.Button bt_getCarrier;
        private System.Windows.Forms.Button bt_OK;
        private System.Windows.Forms.Button bt_exit;
    }
}