using LX_BurnInSolution.Utilities;
using SolveWare_Data_AccessDatabase.Utilities;
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

namespace SolveWare_DataBaseViewer
{
    public partial class Form_ProductConfigSelector : Form
    {
        public Form_ProductConfigSelector()
        {
            InitializeComponent();
        }
        public string GetProductConfigFileFullPath(string constConfigFileName)
        {
            string configFileFullPath = Path.Combine(Application.StartupPath, "ProductConfig", $"{constConfigFileName}", AccessHelper.ConfigFileName);
            return configFileFullPath;
        }
        private string[] GetProductConfigFolderNames()
        {
            string[] folders = new string[0];
            try
            {
                var productFolder = Path.Combine(Application.StartupPath, "ProductConfig");
                folders = Directory.GetDirectories(productFolder);

                for (int i = 0; i < folders.Length; i++)
                {
                    folders[i] = Path.GetFileName(folders[i]);
                }
            }
            catch
            {

            }
            return folders;
        }

        private void Form_ProductConfigSelector_Load(object sender, EventArgs e)
        {
            comboBox_ProductNames.Items.Clear();
            string[] productConfigFolderNames = GetProductConfigFolderNames();
            comboBox_ProductNames.Items.AddRange(productConfigFolderNames);
            comboBox_ProductNames.Text = comboBox_ProductNames.Items[0].ToString();
        }

        private void btn_startSql_Click(object sender, EventArgs e)
        {
            AccessHelper.DBLocalFilter = new List<string>();
            string productName = comboBox_ProductNames.Text.Trim();
            string producDbPath = GetProductConfigFileFullPath(productName);
            //反序列化
            AccessHelper.DBLocalFilter= XmlHelper.DeserializeFile<List<string>>(producDbPath);
            this.DialogResult = DialogResult.OK;
        }
    }
}
