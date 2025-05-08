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
using LX_BurnInSolution.Utilities;

namespace SolveWare_DataAnalayzer
{
    public partial class Form_DataAnalyzer : Form
    {
        List<string> _selectedFiles = new List<string>();
        public Form_DataAnalyzer()
        {
            InitializeComponent();
        }

        private void Form_DataAnalyzer_Load(object sender, EventArgs e)
        {
            this.tb_summaryFile.Text = Application.StartupPath;
            this.lv_StepComboFiles.Columns.Add("文件名", this.lv_StepComboFiles.Width, HorizontalAlignment.Left);
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                foreach (var item in files)
                {
                    if (this._selectedFiles.Contains(item))
                    {

                    }
                    else
                    {
                        this._selectedFiles.Add(item);
                    }
                }
                this.lv_StepComboFiles.Items.Clear();
                this.lv_StepComboFiles.Items.AddRange
                    (
                        this._selectedFiles.ConvertAll<ListViewItem>((item) => { return new ListViewItem(item); }).ToArray()
                    );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加文件错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }
        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.All;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加文件错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void btn_clearView_Click(object sender, EventArgs e)
        {
            this._selectedFiles.Clear();
            this.lv_StepComboFiles.Items.Clear();
        }

        private void btn_grabAllFilesData_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tb_summaryFile.Text))
                {
                    MessageBox.Show("存储路径为空!");
                    return;
                }
                Dictionary<string, List<string>> valDict = new Dictionary<string, List<string>>();
                StringBuilder strb = new StringBuilder();
                string header = string.Empty;
                string[] hArr = null;
                string[] lArr = null;
                int colCount = 0;
                foreach (var file in this._selectedFiles)
                {
                    if (File.Exists(file))
                    {
                        using (StreamReader sr = new StreamReader(file))
                        {
                            string currentLine = string.Empty;
                            if (string.IsNullOrEmpty(header))
                            {
                                header = sr.ReadLine();
                                strb.AppendLine(header);
                                currentLine = sr.ReadLine();
                                strb.AppendLine(currentLine);
                                hArr = header.Split(',');
                                lArr = currentLine.Split(',');
                                colCount = hArr.Length;
                                for (int i = 0; i < colCount; i++)
                                {
                                    valDict.Add(hArr[i], new List<string>());
                                    valDict[hArr[i]].Add(lArr[i]);
                                }
                            }
                            do
                            {
                                currentLine = sr.ReadLine();
                                if (currentLine == header)
                                {
                                    currentLine = sr.ReadLine();
                                    strb.AppendLine(currentLine);
                                    lArr = currentLine.Split(',');
                                    for (int i = 0; i < colCount; i++)
                                    {
                                        valDict[hArr[i]].Add(lArr[i]);
                                    }
                                }
                            } while (sr.EndOfStream == false);
                        }
                    }
                }
                var finalFileName = $@"{tb_summaryFile.Text}\{ DateTime.Now:yyyyMMdd_HHmmss}.csv";
                bool newMethod = true;
                switch (newMethod)
                {
                    case true:
                        {
                            using (StreamWriter sw = new StreamWriter(finalFileName))
                            {
                                //打印原始数据
                                sw.Write(strb.ToString());
                                sw.WriteLine();
 
                                StandardAnalyzer ana = new StandardAnalyzer();

                                if (this.chk_distributingCount.Checked == true)
                                {
                                    int seed = Convert.ToInt16(this.num_distributingCount.Value);

                                    var totalLines = valDict.Values.First().Count;

                                    var dCount = totalLines / seed;
                                    dCount += totalLines % seed > 0 ? 1 : 0;

                                    foreach (var kvp in valDict)
                                    {
                                        string line_ParamName = "SummaryParam,";
                                        string line_Max = "Max,";
                                        string line_Min = "Min,";
                                        string line_MaxDelta = "(Max-Min)/Min * 100%,";
                                        string line_count_delBelow3 = "<3% SampleCount,";
                                        string line_count_delBetween3_5 = "3~5% SampleCount,";
                                        string line_count_delBetween5_10 = "5~10% SampleCount,";
                                        string line_count_delAbove10 = ">10% SampleCount,";

                                        var allDataResult = ana.RunStardardResult(kvp.Value);

                                        line_ParamName += $"{kvp.Key},";
                                        line_Max += $"{allDataResult.Max},";
                                        line_Min += $"{allDataResult.Min},";
                                        line_MaxDelta += $"{allDataResult.MaxDelta},";
                                        line_count_delBelow3 += $"{allDataResult.count_delBelow3},";
                                        line_count_delBetween3_5 += $"{allDataResult.count_delBetween3_5},";
                                        line_count_delBetween5_10 += $"{allDataResult.count_delBetween5_10},";
                                        line_count_delAbove10 += $"{allDataResult.count_delAbove10},";

                                        for (int i = 0; i < dCount; i++)
                                        {
                                            int valLen = 0;
                                            if (i * seed + seed > totalLines)
                                            {
                                                valLen = totalLines - i * seed;
                                            }
                                            else
                                            {
                                                valLen = seed;
                                            }

                                            var rangeVals = kvp.Value.GetRange(i * seed, valLen);
                                            var result = ana.RunStardardResult(rangeVals);
 
                                            line_Max += $"{result.Max},";
                                            line_Min += $"{result.Min},";
                                            line_MaxDelta += $"{result.MaxDelta},";
                                            line_count_delBelow3 += $"{result.count_delBelow3},";
                                            line_count_delBetween3_5 += $"{result.count_delBetween3_5},";
                                            line_count_delBetween5_10 += $"{result.count_delBetween5_10},";
                                            line_count_delAbove10 += $"{result.count_delAbove10},";
                                        }
                                        //sw.WriteLine(kvp.Key);
                                        sw.WriteLine(line_ParamName);
                                        sw.WriteLine(line_Max);
                                        sw.WriteLine(line_Min);
                                        sw.WriteLine(line_MaxDelta);
                                        sw.WriteLine(line_count_delBelow3);
                                        sw.WriteLine(line_count_delBetween3_5);
                                        sw.WriteLine(line_count_delBetween5_10);
                                        sw.WriteLine(line_count_delAbove10);

                                        sw.WriteLine();
                                    }
                                }
                            }
                        }
                        break;
                    case false:
                        {
                            using (StreamWriter sw = new StreamWriter(finalFileName))
                            {
                                //打印原始数据
                                sw.Write(strb.ToString());
                                sw.WriteLine();
                                StandardAnalyzer ana = new StandardAnalyzer();

                                foreach (var kvp in valDict)
                                {
                                    var result = ana.RunStardardResult(kvp.Value);
                                    sw.WriteLine(kvp.Key);

                                    sw.WriteLine($"AllData Max,{result.Max}");

                                    sw.WriteLine($"AllData Min,{result.Min}");

                                    sw.WriteLine($"AllData (Max-Min)/Min * 100%,{result.MaxDelta}");

                                    sw.WriteLine($"AllData <3 % SampleCount,{result.count_delBelow3}");

                                    sw.WriteLine($"AllData 3~5% SampleCount,{result.count_delBetween3_5}");

                                    sw.WriteLine($"AllData 5~10% SampleCount,{result.count_delBetween5_10}");

                                    sw.WriteLine($"AllData >10% SampleCount,{result.count_delAbove10}");

                                    sw.WriteLine();
                                }


                                if (this.chk_distributingCount.Checked == true)
                                {
                                    int seed = Convert.ToInt16(this.num_distributingCount.Value);

                                    var totalLines = valDict.Values.First().Count;

                                    var dCount = totalLines / seed;
                                    dCount += totalLines % seed > 0 ? 1 : 0;

                                    foreach (var kvp in valDict)
                                    {
                                        string line_Max = "Max,";
                                        string line_Min = "Min,";
                                        string line_MaxDelta = "(Max-Min)/Min * 100%,";
                                        string line_count_delBelow3 = "<3% SampleCount,";
                                        string line_count_delBetween3_5 = "3~5% SampleCount,";
                                        string line_count_delBetween5_10 = "5~10% SampleCount,";
                                        string line_count_delAbove10 = ">10% SampleCount,";

                                        for (int i = 0; i < dCount; i++)
                                        {
                                            int valLen = 0;
                                            if (i * seed + seed > totalLines)
                                            {
                                                valLen = totalLines - i * seed;
                                            }
                                            else
                                            {
                                                valLen = seed;
                                            }
                                            var rangeVals = kvp.Value.GetRange(i * seed, valLen);
                                            var result = ana.RunStardardResult(rangeVals);
                                            line_Max += $"{result.Max},";
                                            line_Min += $"{result.Min},";
                                            line_MaxDelta += $"{result.MaxDelta},";
                                            line_count_delBelow3 += $"{result.count_delBelow3},";
                                            line_count_delBetween3_5 += $"{result.count_delBetween3_5},";
                                            line_count_delBetween5_10 += $"{result.count_delBetween5_10},";
                                            line_count_delAbove10 += $"{result.count_delAbove10},";
                                        }
                                        sw.WriteLine(kvp.Key);

                                        sw.WriteLine(line_Max);
                                        sw.WriteLine(line_Min);
                                        sw.WriteLine(line_MaxDelta);
                                        sw.WriteLine(line_count_delBelow3);
                                        sw.WriteLine(line_count_delBetween3_5);
                                        sw.WriteLine(line_count_delBetween5_10);
                                        sw.WriteLine(line_count_delAbove10);

                                        sw.WriteLine();
                                    }
                                }
                            }
                        }
                        break;
                }
  
                MessageBox.Show("数据抓取完成!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"抓取文件数据错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }

        private void btn_selectSummaryDir_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.SelectedPath = Path.GetFullPath(Application.StartupPath);
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    if (Directory.Exists(fbd.SelectedPath))
                    {
                        this.tb_summaryFile.Text = fbd.SelectedPath;
                    }
                    else
                    {
                        this.tb_summaryFile.Text = Path.GetFullPath(Application.StartupPath);
                        MessageBox.Show($"选择数据存储路径不存在!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"选择数据存储路径错误:[{ex.Message}-{ex.StackTrace}]!");
            }
        }
    }
}