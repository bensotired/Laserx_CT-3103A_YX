using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using SolveWare_TestComponents.Model;
using SolveWare_TestComponents.ResourceProvider;
using SolveWare_TestComponents.UIComponents;
using SolveWare_TestPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_TestExecutorDebugger_SimpleMode : Form, ITesterCoreLink
    {
        ITesterCoreInteration _core;
        public Form_TestExecutorDebugger_SimpleMode()
        {
            InitializeComponent();
        }
        Form_RawDataViewer _dataUI;
        TestExecutorBase _tempExecutor;
        TestExecutorBase _tempReloadExecutor;
        TestPluginResourceProvider _rp;

        ExecutorConfigItem _tempExecutorConfigItem;
        ExecutorConfigItem_TestParamsConfig _tempTestParamConfigItem;

        CancellationTokenSource _tempTokenSource = new CancellationTokenSource();
        CancellationTokenSource _tempReloadActionTokenSource = new CancellationTokenSource();
        const string ConfigFile = @"DataPathConfig\DataPaths.xml";
        static DataPaths _instance;
        private void Form_TestExecutorDebugger_Load(object sender, EventArgs e)
        {
            _dataUI = new Form_RawDataViewer();
            this._core.ModifyDockableUI(_dataUI, true);
            pnl_data.Controls.Add(_dataUI);
            _dataUI.Show();

            cb_pluginResourceProvider.Items.Clear();
            var pluginNames = this._core.GetTestPlugInKeys();
            foreach (var name in pluginNames)
            {
                cb_pluginResourceProvider.Items.Add(name);
            }
            if (pluginNames.Length > 0)
            {
                cb_pluginResourceProvider.SelectedIndex = 0;
            }
           
            this.tb_debugDataPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //this.tb_saveReloadDataPath.Text = Path.GetFullPath(Application.StartupPath);
            try
            {
                _instance = XmlHelper.DeserializeFile<DataPaths>(ConfigFile);
                this.tb_debugDataPath.Text = _instance.Path_SimpleMode;
            }
            catch (Exception)
            {
                // throw;
            }
        }

        public void ConnectToCore(ITesterCoreInteration core)
        {
            this._core = core;
        }

        public void DisconnectFromCore(ITesterCoreInteration core)
        {
            this._core = null;

        }
        public bool Setup(object eciObj , object tstParamObj)
        {
            ExecutorConfigItem eci = eciObj as ExecutorConfigItem;
            ExecutorConfigItem_TestParamsConfig tstParamCfg = tstParamObj as ExecutorConfigItem_TestParamsConfig;
            if (eci == null || 
                tstParamCfg == null)
            {
                MessageBox.Show($"无法解析导入的测试项配置!");
                return false;
            }
            this.Text = $"单项调试[{eci.TestExecutorName}]";
 

            _tempExecutorConfigItem = eci;
            _tempTestParamConfigItem = tstParamCfg;
            return true;
        }

        private void Form_TestExecutorDebugger_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.DisconnectFromCore(this._core);
        }

        private void btn_debugOnce_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (this._tempTokenSource.IsCancellationRequested)
                    {
                        this._tempTokenSource = new CancellationTokenSource();
                    }
                    _dataUI.ClearContext();
                    string resourceProvider = string.Empty;

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_debugOnce.Enabled = false;
                        this.btn_debugTimes.Enabled = false;
                        resourceProvider = cb_pluginResourceProvider.SelectedItem?.ToString();
                    });

                    if (string.IsNullOrEmpty(resourceProvider))
                    {
                        MessageBox.Show("无效的测试组件资源池!本次调试不会执行!");
                        return;
                    }

                    _tempExecutor = new TestExecutorBase($"测试调试执行器[{this._tempExecutorConfigItem.TestExecutorName}]");
                    ITestPluginWorkerBase plugin = (ITestPluginWorkerBase)this._core.GetTestPlugin(resourceProvider);
                    _rp = plugin.GetPluginLocalizedResourceProvider();

                    //_tempExecutor.Setup_DebugMode(this._core, _rp, _tempExecutorConfigItem);
                    _tempExecutor.Setup_DebugMode_DynamicParamRecipes(this._core, _rp, _tempExecutorConfigItem, _tempTestParamConfigItem);

                    var data = _tempExecutor.Execute_DebugMode(this._tempTokenSource.Token);
                                       
                    //_dataUI.ClearContext();
                    //_dataUI.ImportRawData(data.RawData);


                    this.Invoke((EventHandler)delegate
                    {
                        string finalFileName = string.Empty;
                        if (this.chk_enableSaveDataDialog.Checked|| this.chk_enableSaveImage.Checked)
                        {

                            string dir = Path.GetFullPath(Application.StartupPath);
                            if (string.IsNullOrEmpty(this.tb_debugDataPath.Text) ||
                                Directory.Exists(this.tb_debugDataPath.Text) == false)
                            {

                            }
                            else
                            {
                                dir = this.tb_debugDataPath.Text;
                            }
                            var defaultFileName = string.Concat
                                    (
                                        this._tempExecutorConfigItem.TestExecutorName,
                                        BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now),
                                        FileExtension.CSV
                                    );

                            //var finalFileName = $@"{dir}\{this.tb_debugDataPathComment.Text}{defaultFileName}";
                            finalFileName = $@"{dir}\{this.tb_debugDataPathComment.Text}{defaultFileName}";
                            if (File.Exists(finalFileName) == false)
                            {
                                if (Directory.Exists(Path.GetDirectoryName(finalFileName)) == false)
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(finalFileName));
                                }
                            }
                           // data.PrintToCSV(finalFileName);
                        }
                        if (this.chk_enableSaveDataDialog.Checked)
                        {
                            data.PrintToCSV(finalFileName);
                        }
                        //if (this.chk_enableSaveImage.Checked)
                        //{
                        //    string _finalFileName = finalFileName.Remove(finalFileName.LastIndexOf('\\')) + $@"\Image\";
                        //    //if (Directory.Exists(_finalFileName) == false)
                        //    //{
                        //    //    Directory.CreateDirectory(_finalFileName);
                        //    //}
                        //    //_dataUI.SaveImagePath = _finalFileName;
                        //    //_dataUI.FFChartSaveImage_SW = true;
                        //}
                    });

                    _dataUI.ClearContext();
                    _dataUI.ImportRawData(data.RawData);
                }
                catch (Exception ex)
                {
                    var msg = $"测试调试执行器[{this._tempExecutorConfigItem.TestExecutorName}]运行单次错误:[{ex.Message}-{ex.StackTrace}]!";
                    MessageBox.Show(msg);
                    this._core.Log_Global(msg);
                }
                finally
                {

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_debugOnce.Enabled = true;
                        this.btn_debugTimes.Enabled = true;
                    });
                    _tempExecutor.Dispose();
                    _tempExecutor = null;
                    _rp.Dispose();
                    _rp = null;
                }
            });
        }
        private void btn_debugStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._tempTokenSource.IsCancellationRequested == false)
                {
                    this._tempTokenSource.Cancel();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"停止调试错误:[{ex.Message}-{ex.StackTrace}]!");
            }
            finally
            {
                this.btn_debugTimes.Enabled = true;
                this.btn_debugOnce.Enabled = true;
            }
        }

        private void btn_debugSelectDataPath_Click(object sender, EventArgs e)
        {
            try
            {
                this.Invoke((EventHandler)delegate
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.SelectedPath = Path.GetFullPath(Application.StartupPath);
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        if (Directory.Exists(fbd.SelectedPath))
                        {
                            this.tb_debugDataPath.Text = fbd.SelectedPath;
                        }
                        else
                        {
                            this.tb_debugDataPath.Text = Path.GetFullPath(Application.StartupPath);
                            MessageBox.Show($"选择数据存储路径不存在!");
                        }
                    }
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show($"选择数据存储路径错误:[{ex.Message}-{ex.StackTrace}]!");
            }

        }

        private void btn_debugTimes_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    int debugTimes = 1;
                    int debugInterval_ms = 1000;
                    if (int.TryParse(tb_debugTimes.Text, out debugTimes) == false)
                    {
                        MessageBox.Show("无效的运行次数!");
                        return;
                    }
                    if (JuniorMath.IsValueInLimitRange(debugTimes, 0, 200) == false)
                    {
                        MessageBox.Show("运行次数超限[1,200]!");
                        return;
                    }
                    if (int.TryParse(tb_debugInterval_ms.Text, out debugInterval_ms) == false ||
                        debugInterval_ms < 0)
                    {
                        MessageBox.Show("无效的运行间隔!");
                        return;
                    }

                    this._core.Log_Global($"开始连续运行测试...");

                    if (this._tempTokenSource.IsCancellationRequested)
                    {
                        this._tempTokenSource = new CancellationTokenSource();
                    }
                    _dataUI.ClearContext();
                    string resourceProvider = string.Empty;

                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_debugOnce.Enabled = false;
                        this.btn_debugTimes.Enabled = false;
                        resourceProvider = cb_pluginResourceProvider.SelectedItem?.ToString();
                    });

                    if (string.IsNullOrEmpty(resourceProvider))
                    {
                        MessageBox.Show("无效的测试组件资源池!本次调试不会执行!");
                        return;
                    }

                    _tempExecutor = new TestExecutorBase($"测试调试执行器[{this._tempExecutorConfigItem.TestExecutorName}]");
                    ITestPluginWorkerBase plugin = (ITestPluginWorkerBase)this._core.GetTestPlugin(resourceProvider);
                      _rp = plugin.GetPluginLocalizedResourceProvider();
                    _tempExecutor.Setup_DebugMode_DynamicParamRecipes(this._core, _rp, _tempExecutorConfigItem, _tempTestParamConfigItem);
                    //_tempExecutor.Setup_DebugMode(this._core, _rp, _tempExecutorConfigItem);

                    string finalFileName = string.Empty;
                    this.Invoke((EventHandler)delegate
                    {
                        if (this.chk_enableSaveDataDialog.Checked || this.chk_enableSaveImage.Checked)
                        {

                            string dir = Path.GetFullPath(Application.StartupPath);
                            if (string.IsNullOrEmpty(this.tb_debugDataPath.Text) ||
                                Directory.Exists(this.tb_debugDataPath.Text) == false)
                            {

                            }
                            else
                            {
                                dir = this.tb_debugDataPath.Text;
                            }
                            var defaultFileName = string.Concat
                                    (
                                        this._tempExecutorConfigItem.TestExecutorName,
                                        BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now),
                                        FileExtension.CSV
                                    );

                            //var finalFileName = $@"{dir}\{this.tb_debugDataPathComment.Text}{defaultFileName}";
                            finalFileName = $@"{dir}\{this.tb_debugDataPathComment.Text}{defaultFileName}";
                            if (File.Exists(finalFileName) == false)
                            {
                                if (Directory.Exists(Path.GetDirectoryName(finalFileName)) == false)
                                {
                                    Directory.CreateDirectory(Path.GetDirectoryName(finalFileName));
                                }
                            }                           
                        }
                        //if (this.chk_enableSaveImage.Checked)
                        //{
                        //    _dataUI.FFChartSaveImage_SW = true;
                        //    _dataUI.SaveImagePath = finalFileName;
                        //}
                    });

                    List<DeviceStreamDataLite> dataCollection = new List<DeviceStreamDataLite>();
                    for (int time = 1; time <= debugTimes; time++)
                    {
                        this._core.Log_Global($"正在连续运行第{time}次测试...");
                        var data = _tempExecutor.Execute_DebugMode(this._tempTokenSource.Token);

                        dataCollection.Add(data);
                        _dataUI.ClearContext();
                        //_dataUI.FFChartSaveImage_SW = false;
                        _dataUI.ImportRawData(data.RawData);
                        Thread.Sleep(debugInterval_ms);
                        if (this._tempTokenSource.IsCancellationRequested)
                        {
                            this._core.Log_Global($"取消连续运行.");
                            break;
                        }
                    }

                    this.Invoke((EventHandler)delegate
                    {
                        if (this.chk_enableSaveDataDialog.Checked)
                        {
                            //string dir = Path.GetFullPath(Application.StartupPath);
                            //if (string.IsNullOrEmpty(this.tb_debugDataPath.Text) ||
                            //    Directory.Exists(this.tb_debugDataPath.Text) == false)
                            //{

                            //}
                            //else
                            //{
                            //    dir = this.tb_debugDataPath.Text;
                            //}

                            ////var defaultFileName = string.Concat
                            ////        (
                            ////            this._tempExecutorConfigItem.TestExecutorName,
                            ////            BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now),
                            ////            $"[{debugTimes}]",
                            ////            FileExtension.CSV
                            ////        );

                            //var finalFileName = $@"{dir}\{this.tb_debugDataPathComment.Text}";// {defaultFileName}";
                            //if (File.Exists(finalFileName) == false)
                            //{
                            //    if (Directory.Exists(Path.GetDirectoryName(finalFileName)) == false)
                            //    {
                            //        Directory.CreateDirectory(Path.GetDirectoryName(finalFileName));
                            //    }
                            //}
                            int count = 0;
                            foreach (var data in dataCollection)
                            {
                                count++;
                                string  defaultFileName = string.Concat
                                        (
                                            finalFileName,
                                            this._tempExecutorConfigItem.TestExecutorName,
                                            BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now),
                                            $"[{count}]",
                                            FileExtension.CSV
                                        );
                                data.PrintToCSV(defaultFileName);                                
                            }                          
                             
                            //// var finalFileName = $@"{dir}\{defaultFileName}";
                            //// var finalFileName = $@"{dir}\{this.tb_debugDataPathComment.Text}{defaultFileName}";
                            //using (StreamWriter sw = new StreamWriter(finalFileName, false))
                            //{
                            //    foreach (var data in dataCollection)
                            //    {
                            //        sw.WriteLine(data.SummaryDataCollection.ToString());
                            //        sw.WriteLine(data.RawData.ToString());
                            //        sw.WriteLine();
                            //    }
                            //}
                        }
                        //int FFcount = 0; 
                        //string _finalFileName = finalFileName.Remove(finalFileName.LastIndexOf('\\')) + $@"\Image\";//{count}\
                        //if (Directory.Exists(_finalFileName) == false)
                        //{
                        //    Directory.CreateDirectory(_finalFileName);
                        //}
                        //foreach (var data in dataCollection)
                        //{
                        //FFcount++;
                        //if (this.chk_enableSaveImage.Checked)
                        //{
                        //    _dataUI.SaveImagePath = _finalFileName;
                        //    _dataUI.FFChartSaveImage_SW = true;
                        //}
                        //_dataUI.SN = FFcount.ToString();
                        //_dataUI.ClearContext();
                        //_dataUI.ImportRawData(data.RawData);
                        //}
                    });
                }
                catch (Exception ex)
                {
                    var msg = $"测试调试执行器[{this._tempExecutorConfigItem.TestExecutorName}]运行单次错误:[{ex.Message}-{ex.StackTrace}]!";
                    MessageBox.Show(msg);
                    this._core.Log_Global(msg);
                }
                finally
                 {
                    this.Invoke((EventHandler)delegate
                    {
                        this.btn_debugOnce.Enabled = true;
                        this.btn_debugTimes.Enabled = true;
                    }); 
                    _tempExecutor.Dispose();
                    _tempExecutor = null;
                    _rp.Dispose();
                    _rp = null;
                }
            });
        }
        public class DataPaths
        {
            public string Path_Graph { get; set; }
            public string Path_TestRawDataCsv { get; set; }
            public string Path_SimpleMode { get; set; }
            public string Path_WaferFile { get; set; }
            public string Path_BinFile { get; set; }
            public string Path_InternalData { get; set; }
        }
        #region old rollback
        //private void btn_runDataReloader_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string resourceProvider = string.Empty;

        //        this.btn_runDataReloader.Enabled = false;

        //        resourceProvider = cb_pluginResourceProvider.SelectedItem?.ToString();


        //        if (string.IsNullOrEmpty(resourceProvider))
        //        {
        //            MessageBox.Show("无效的测试组件资源池!本次回滚不会执行!");
        //            return;
        //        }
        //        if (string.IsNullOrEmpty(rtb_reloadData.Text))
        //        {
        //            MessageBox.Show("空白的回滚源数据!本次回滚不会执行!");
        //            return;
        //        }

        //        if (_tempReloadActionTokenSource.IsCancellationRequested == true)
        //        {
        //            this._tempReloadActionTokenSource = new CancellationTokenSource();
        //        }

        //        if (this._tempReloadExecutor == null ||
        //            this._tempReloadExecutor.IsDebugModeReady == false ||
        //            this.chk_importResourceBeforeCalculatingReloadData.Checked == true)
        //        {
        //            _tempReloadExecutor = new TestExecutorBase($"测试调试执行器[{this._tempExecutorConfigItem.TestExecutorName}]");
        //            ITestPluginWorkerBase plugin = (ITestPluginWorkerBase)this._core.GetTestPlugin(resourceProvider);
        //            var rp = plugin.GetPluginLocalizedResourceProvider();

        //            _tempReloadExecutor.Setup_DebugMode(this._core, rp, _tempExecutorConfigItem);
        //        }



        //        var data = _tempReloadExecutor.CalculateReloadData_DebugMode(rtb_reloadData.Text, _tempReloadActionTokenSource.Token);

        //        this.Invoke((EventHandler)delegate
        //        {
        //            if (this.chk_enableSaveDataDialog.Checked)
        //            {

        //                string dir = Path.GetFullPath(Application.StartupPath);
        //                if (string.IsNullOrEmpty(this.tb_saveReloadDataPath.Text) ||
        //                    Directory.Exists(this.tb_saveReloadDataPath.Text) == false)
        //                {

        //                }
        //                else
        //                {
        //                    dir = this.tb_saveReloadDataPath.Text;
        //                }
        //                var defaultFileName = string.Concat
        //                        (
        //                            this._tempExecutorConfigItem.TestExecutorName,
        //                            BaseDataConverter.ConvertDateTimeTo_FILE_string(DateTime.Now),
        //                            FileExtension.CSV
        //                        );
        //                var finalFileName = $@"{dir}\{defaultFileName}";
        //                data.PrintToCSV(finalFileName);
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        this.btn_runDataReloader.Enabled = true;

        //    }
        //}
        //private void btn_saveReloadDataPath_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.Invoke((EventHandler)delegate
        //        {
        //            FolderBrowserDialog fbd = new FolderBrowserDialog();
        //            fbd.SelectedPath = Path.GetFullPath(Application.StartupPath);
        //            if (fbd.ShowDialog() == DialogResult.OK)
        //            {
        //                if (Directory.Exists(fbd.SelectedPath))
        //                {
        //                    this.tb_saveReloadDataPath.Text = fbd.SelectedPath;
        //                }
        //                else
        //                {
        //                    this.tb_saveReloadDataPath.Text = Path.GetFullPath(Application.StartupPath);
        //                    MessageBox.Show($"选择回滚数据存储路径不存在!");
        //                }
        //            }
        //        });

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"选择回滚数据存储路径错误:[{ex.Message}-{ex.StackTrace}]!");
        //    }
        //}
        //private void btn_clearReloadData_Click(object sender, EventArgs e)
        //{
        //    this.rtb_reloadData.Clear();
        //}

        #endregion old rollback

        private void chk_enableSaveImage_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}