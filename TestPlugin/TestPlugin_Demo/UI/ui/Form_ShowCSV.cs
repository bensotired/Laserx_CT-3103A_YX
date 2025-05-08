using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    public partial class Form_ShowCSV : Form
    {
        public Form_ShowCSV()
        {
            InitializeComponent();
        }

        private void Form_ShowCSV_Load(object sender, EventArgs e)
        {
            GetCarrierNumber();
        }
        void GetCarrierNumber()
        {
            try
            {
                string configFileFullPath = System.IO.Path.Combine(Application.StartupPath, "SerialNumber", "CarrierNumber.csv");
                string[] allLine = File.ReadAllLines(configFileFullPath);
                List<string> CarrierNumberlist = new List<string>();
                foreach (var line in allLine)
                {
                    string[] columns = line.Split(',');
                    CarrierNumberlist.Add(columns[0]);
                }
                listView_CarrierNumber.Clear();
                listView_CarrierNumber.Columns.Add("Index", 40, HorizontalAlignment.Center);
                listView_CarrierNumber.Columns.Add("CarrierNumber", listView_CarrierNumber.Width - 40, HorizontalAlignment.Center);
                //listView_CarrierNumber.View = View.List;
                //foreach (string item in CarrierNumberlist)
                //{
                //    listView_CarrierNumber.Items.Add(item);
                //}

                for (int i = 0; i < CarrierNumberlist.Count; i++)
                {
                    listView_CarrierNumber.Items.Add($"{i + 1}");
                    listView_CarrierNumber.Items[i].SubItems.Add(CarrierNumberlist[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载错误:{ex.Message}-{ex.StackTrace}!");
            }

        }

        private void listView_CarrierNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.listView_CarrierNumber.SelectedItems.Count >= 1)
                {
                    var file = listView_CarrierNumber.SelectedItems[0].SubItems[1].Text.ToString();
                    if (file != "NA")
                    {
                        GetCarrierChipNumber(file);
                    }
                    else
                    {
                        listView_chipnumber.Clear();
                        label_chip.Text = "***-ChipNumber";
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载错误:{ex.Message}-{ex.StackTrace}!");
            }
        }
        void GetCarrierChipNumber(string CarrierNumber)
        {
            try
            {
                string configFileFullPath = System.IO.Path.Combine(Application.StartupPath, "SerialNumber", $"{CarrierNumber}.csv");
                if (!File.Exists(configFileFullPath))
                {
                    MessageBox.Show($"路径错误，该文件不存在:{configFileFullPath}!");
                    return;
                }
                string[] allLine = File.ReadAllLines(configFileFullPath);
                var ChipNumberlist = new List<string>();
                foreach (var line in allLine)
                {
                    string[] columns = line.Split(',');
                    ChipNumberlist.Add(columns[0]);
                }
                listView_chipnumber.Clear();
                listView_chipnumber.Columns.Add("Index", 40, HorizontalAlignment.Center);
                listView_chipnumber.Columns.Add("ChipNumber", listView_chipnumber.Width - 40, HorizontalAlignment.Center);
                //listView_chipnumber.View = View.List;
                //foreach (string item in ChipNumberlist)
                //{
                //    listView_chipnumber.Items.Add(item);
                //}

                for (int i = 0; i < ChipNumberlist.Count; i++)
                {
                    listView_chipnumber.Items.Add($"{i + 1}");
                    listView_chipnumber.Items[i].SubItems.Add(ChipNumberlist[i]);
                }

                label_chip.Text = CarrierNumber + "-ChipNumber";
            }
            catch (Exception ex)
            {
            }
        }

        private void bt_getCarrier_Click(object sender, EventArgs e)
        {
            GetCarrierNumber();
        }

        private void bt_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bt_exit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
