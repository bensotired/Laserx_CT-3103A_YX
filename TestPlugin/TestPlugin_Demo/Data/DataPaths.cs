using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using System;
using System.IO;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    public class DataPaths : ITesterCoreLink
    {
        ITesterCoreInteration _core;
        public void ConnectToCore(ITesterCoreInteration core)
        {
            _core = core;
        }

        public void DisconnectFromCore(ITesterCoreInteration core)
        {
            _core = null;
        }
        static DataPaths _instance;
        static object _mutex = new object();
        const string ConfigFile = @"DataPathConfig\DataPaths.xml";
        public static DataPaths Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new DataPaths();
                        }
                    }
                }
                return _instance;
            }
        }
        public DataPaths()
        {
            //Path_Graph = Application.StartupPath;
            //Path_TestRawDataCsv = Application.StartupPath;
            //Path_TestRawDataCsv_Backup = Application.StartupPath;
            //Path_WaferFile = Application.StartupPath;
            //Path_BinFile = Application.StartupPath;
        }
        public void CreateDefaultDataPaths()
        {
            Path_Graph = Application.StartupPath;
            Path_TestRawDataCsv = Application.StartupPath;
            Path_TestRawDataCsv_Backup = Application.StartupPath;
            Path_WaferFile = Application.StartupPath;
            Path_BinFile = Application.StartupPath;
            Path_InternalData = Application.StartupPath;
        }
        public string Path_Graph { get; set; }
        public string Path_TestRawDataCsv { get; set; }
        public string Path_TestRawDataCsv_Backup { get; set; }
        public string Path_WaferFile { get; set; }
        public string Path_BinFile { get; set; }
        public string Path_InternalData { get; set; }
        public void Load()
        {
            try
            {
                _instance = XmlHelper.DeserializeFile<DataPaths>(ConfigFile);
            }
            catch
            {
                if (Directory.Exists(Path.GetDirectoryName(ConfigFile)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(ConfigFile));
                }
                //_instance.CreateDefaultDataPaths();
                XmlHelper.SerializeFile<DataPaths>(ConfigFile, _instance);
            }
        }
        public void Save()
        {
            try
            {
                XmlHelper.SerializeFile<DataPaths>(ConfigFile, _instance);
            }
            catch (Exception ex)
            {
                var msg = $"保存Data paths配置文件失败:{ex.Message}-{ex.StackTrace}!";
                this._core.Log_Global(msg);
                MessageBox.Show(msg);
            }
        }
        public bool CreatePaths()
        {
            try
            {
                //if (Directory.Exists(Path_Graph) == false)
                //{
                //    Directory.CreateDirectory(Path_Graph);
                //}
                if (Directory.Exists(Path_TestRawDataCsv) == false)
                {
                    Directory.CreateDirectory(Path_TestRawDataCsv);
                }
                if (Directory.Exists(Path_TestRawDataCsv_Backup) == false)
                {
                    Directory.CreateDirectory(Path_TestRawDataCsv_Backup);
                }
                if (Directory.Exists(Path_WaferFile) == false)
                {
                    Directory.CreateDirectory(Path_WaferFile);
                }
                if (Directory.Exists(Path_BinFile) == false)
                {
                    Directory.CreateDirectory(Path_BinFile);
                }
                if (Directory.Exists(Path_InternalData) == false)
                {
                    Directory.CreateDirectory(Path_InternalData);
                }
                return true;
            }
            catch(Exception ex )
            {
                var msg = $"创建Data paths失败:{ex.Message}-{ex.StackTrace}!";
                this._core.Log_Global(msg);
                MessageBox.Show(msg);
            }
            return false;
        }
    }
}