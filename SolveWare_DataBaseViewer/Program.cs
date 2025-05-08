using SolveWare_Data_AccessDatabase.TestDBUI;
using SolveWare_Data_AccessDatabase.Utilities;
using System;
using System.Windows.Forms;
using SolveWare_BurnInCommon;
using LX_BurnInSolution.Utilities;
using System.IO;

namespace SolveWare_DataBaseViewer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var dbConnStr = Load();
                if (string.IsNullOrEmpty(dbConnStr))
                {
                    MessageBox.Show($"获取数据库连接字段位空！");
                    return;
                }
                AccessHelper.DATABASE = dbConnStr;
                if (IsConnectedDatabase() == false)
                {
                    MessageBox.Show("数据库连接失败！");  
                    return;
                }

                Form_ProductConfigSelector form_ProductConfigSelector = new Form_ProductConfigSelector();
                DialogResult dialogResult= form_ProductConfigSelector.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    FormDataBase debugForm = new FormDataBase();
                    Application.Run(debugForm);
                    Environment.Exit(0);
                }
            }

            catch (Exception ex)
            {

            }
        }
        static bool IsConnectedDatabase()
        {
            return AccessHelper.IsConnected;
        }
        static string Load()
        {
            string DataBaseConnectionString = string.Empty;
            try
            {
                var DefaultStationConfigFile = XmlHelper.DeserializeFile<TestStationConfig>(@"SystemConfig\StationConfigs.xml");
                DataBaseConnectionString = DefaultStationConfigFile.GetSystemParamsValue(TesterKeyPathsAndFiles.DATA_BASE_CONNECTION_STRING);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取数据库连接字段错误:{ex.Message}-{ex.StackTrace}");
            }
            return DataBaseConnectionString;
        }
    
    }
}