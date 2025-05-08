using SolveWare_Data_AccessDatabase.Business;
using SolveWare_Data_AccessDatabase.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SolveWare_Data_AccessDatabase.TestDBUI
{
    public partial class FormDataBase : Form
    {
        //private string path = @"D:\test.mdb";//AccessHelper.DATABASE; //@"D:\product.mdb";
        //AccessDatabaseManager _accessDbM = new AccessDatabaseManager();
        private List<string> Filters = new List<string>();
        public FormDataBase()
        {
            InitializeComponent();
        }

        public void SetFilter(List<string> InputFilter)
        {
            Filters = InputFilter; 
        }
        private void FormDataBase_Load(object sender, EventArgs e)
        {
            //this.IsConnectedDatabase();
            this.UpdatecomboBox1_Table();
            //this.ViewTable();
        }
        /// <summary>
        /// 展示表中所有列(按表中字段顺序)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_Table_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dataGridView_ColumnName.Rows.Clear();
            var TableName = comboBox1_Table.Items[comboBox1_Table.SelectedIndex].ToString();
            
            var ListColumn = GetTableColumn_Ordered(TableName);
            for (int i = 0; i < ListColumn.Count; i++)
            {
                int rowIndex = dataGridView_ColumnName.Rows.Add();
                if (AccessHelper.DBLocalFilter != null || AccessHelper.DBLocalFilter.Count > 0)
                {
                    if (AccessHelper.DBLocalFilter.Contains(ListColumn[i].Trim()))
                    {
                        dataGridView_ColumnName.Rows[rowIndex].Cells[0].Style.ForeColor = Color.Black;
                    }
                    else
                    {
                        dataGridView_ColumnName.Rows[rowIndex].Cells[0].Style.ForeColor = Color.Red;
                    }
                }
                dataGridView_ColumnName.Rows[rowIndex].Cells[0].Value = ListColumn[i].ToString();
                
                dataGridView_ColumnName.Rows[rowIndex].Cells[1].Value = false;
            }
            this.GetPartNumber();
        }
        /// <summary>
        /// 按表中顺序获取表中所有字段
        /// </summary>
        /// <param name="tablenaem">表名</param>
        /// <returns></returns>
        private List<string> GetTableColumn_Ordered(string tablenaem)
        {
            string tblName = tablenaem.Trim();
            List<string> listColumn = new List<string>();
            try
            {
                AccessHelper.OpenConnect();
                string sql = "SELECT * FROM " + tblName;
                OleDbDataAdapter thisAdapter = new OleDbDataAdapter(sql, AccessHelper.m_conn);
                DataSet dataSet = new DataSet();
                thisAdapter.Fill(dataSet, tblName);
                foreach (DataTable item in dataSet.Tables)
                {
                    foreach (DataColumn column in dataSet.Tables[item.TableName].Columns)
                    {
                        if (column.ColumnName.ToString().Trim().ToLower()=="id")
                        {
                            continue;
                        }
                        if (column.ColumnName.ToString().Trim().ToLower() == "result")
                        {
                            continue;
                        }  
                        if (column.ColumnName.ToString().Trim().ToLower() == "partnumber")
                        {
                            continue;
                        }

                        listColumn.Add(column.ColumnName);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                AccessHelper.Dispose();
            }

            return listColumn;
        }
        /// <summary>
        /// 可选数据表
        /// </summary>
        private void UpdatecomboBox1_Table()
        {
            AccessHelper.OpenConnect();
            
            
            DataTable dt = AccessHelper.m_conn.GetSchema("Tables");
            List<string> list = new List<string>();
            foreach (DataRow item in dt.Rows)
            {
                if (item[3].ToString() == "TABLE")
                {
                    var str = item[2].ToString();
                    list.Add(str);
                }
            }
            this.comboBox1_Table.Items.Clear();
            this.comboBox1_Table.Items.AddRange(list.ToArray());
            AccessHelper.Dispose();
        }
        /// <summary>
        /// 检查数据库是否连接
        /// </summary>
        private void IsConnectedDatabase()
        {
            var isPass = AccessHelper.IsConnected;// _accessDbM.IsDataBaseConnected(path,out string errMsg);
            //AccessHelper.OpenConnect();
            //var isPass = AccessHelper.IsConnected;
            if (isPass)
            {

            }
            else
            {
                MessageBox.Show("数据库连接失败！");
                return;
            }
          
        }

        #region 可有可无的
        /// <summary>
        /// 展示库中所有表
        /// </summary>
        private void ViewTable()
        {
            //AccessHelper.OpenConnect();

            //const int colIndex = 0;
            //this.dataGridView_Table.Rows.Clear();
            //DataTable dt = AccessHelper.m_conn.GetSchema("Tables");
            //foreach (DataRow row in dt.Rows)
            //{
            //    if (row[3].ToString() == "TABLE")
            //    {
            //        var rIndex = this.dataGridView_Table.Rows.Add();
            //        this.dataGridView_Table.Rows[rIndex].Cells[colIndex].Value = row[2].ToString();
            //        this.dataGridView_Table.Rows[rIndex].Cells[colIndex].Tag = row[2].ToString();
            //    }
               
            //}
            //AccessHelper.Dispose();
          
        }

        private void dataGridView_Table_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            //{
            //    var tm = this.dataGridView_Table.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            //    AccessHelper.OpenConnect();
            //    string sql = "SELECT * FROM " + tm.ToString();
            //    var dt = AccessHelper.QueryDataTable(sql);
            //    dataGridView_TableData.DataSource = dt.DefaultView;
                
            //}
        }
        #endregion

        /// <summary>
        /// 查询,多条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_Click(object sender, EventArgs e)
        {
            try
            {
                this.ClearTECchart();
                if (comboBox1_Table.Text.Length == 0)
                {
                    MessageBox.Show("未选择数据表！");
                    return;
                }
                var tablename = comboBox1_Table.Items[comboBox1_Table.SelectedIndex].ToString();

                int redRowCount = 0;//红色的行有多少行
                string specialConditionName = string.Empty;

                string startTime = null;
                string endTime = null;
                string conditionName1 = null;
                string conditionName2 = null;
                string column = string.Empty;
           
                if (this.dateTimePicker_StartTime.Checked)
                {
                    startTime = this.dateTimePicker_StartTime.Value.ToString("yyyy-MM-dd HH:mm");
                }
                if (this.dateTimePicker_EndTime.Checked)
                {
                    endTime = this.dateTimePicker_EndTime.Value.ToString("yyyy-MM-dd HH:mm");
                }

                for (int i = 0; i < dataGridView_ColumnName.RowCount; i++)
                {
                    string select = ((DataGridViewCheckBoxCell)dataGridView_ColumnName.Rows[i].Cells[1]).Value.ToString();
                    if (select == "True")
                    {
                        Color color = dataGridView_ColumnName.Rows[i].Cells[0].Style.ForeColor;
                        if (color == Color.Red)
                        {
                            specialConditionName = dataGridView_ColumnName.Rows[i].Cells[0].Value.ToString().Trim();
                            redRowCount++;
                        }
                        string name = dataGridView_ColumnName.Rows[i].Cells[0].Value.ToString();
                        column += ",[" + name + "]";
                    }
                }
                if (column == string.Empty)
                {
                    column = "*";
                }
                else
                {
                    //column = "[id]" + column + ",[PartNumber],[result]";
                    column = "[id]" + column;
                }

              
                AccessHelper.OpenConnect();

                string sql = string.Empty;
                DataTable resultdataTable = null;
                if (!(this.dateTimePicker_StartTime.Checked) && !(this.dateTimePicker_EndTime.Checked))
                {
                    sql = "SELECT " + column + " FROM " + tablename;
                }
                else if (this.dateTimePicker_StartTime.Checked && this.dateTimePicker_EndTime.Checked)
                {
                    sql = "SELECT " + column + " FROM " + tablename + " WHERE 1=1 AND " + "CreateStartTime >=#" + startTime + "#" + " AND " + "CreateStartTime <=#" + endTime + "#";
                }
                else if (this.dateTimePicker_StartTime.Checked)
                {
                    sql = "SELECT " + column + " FROM " + tablename + " WHERE 1=1 AND " + "CreateStartTime >=#" + startTime + "#";
                }
                else if (this.dateTimePicker_EndTime.Checked)
                {
                    sql = "SELECT " + column + " FROM " + tablename + " WHERE 1=1 AND " + "CreateStartTime <=#" + endTime + "#";
                }

                if (sql != string.Empty)
                {
                    resultdataTable = AccessHelper.QueryDataTable(sql);
                    dataGridView_TableData.DataSource = null;
                    dataGridView_TableData.DataSource = resultdataTable.DefaultView;
                    //指定的ID和结果
                    if (resultdataTable == null)
                    {
                        MessageBox.Show("没有找到数据");
                        return;
                    }
                    if (redRowCount == 1)
                    {
                        List<int> idList = new List<int>();
                        List<double> selectConditionValues = new List<double>();

                        for (int i = 0; i < resultdataTable.Rows.Count; i++)
                        {
                            idList.Add(int.Parse(resultdataTable.Rows[i]["id"].ToString()));
                            double conditionValue = 0;
                            try
                            {
                                conditionValue = Math.Round(double.Parse(resultdataTable.Rows[i][specialConditionName].ToString()), 2);
                            }
                            catch
                            {
                                conditionValue = 0;
                            }
                            finally
                            {
                                selectConditionValues.Add(conditionValue);
                            }
                        }
                        UpdateTECchart(idList, selectConditionValues, specialConditionName); 
                    }

                    //int allCount = idList.Count;
                    //int failCount = allCount - passCount;
                    //textBox_All.Text = allCount.ToString();
                    //textBox_Pass.Text = passCount.ToString();
                    //textBox_Fail.Text = failCount.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetPartNumber()
        {
            var tablename = comboBox1_Table.Items[comboBox1_Table.SelectedIndex].ToString();
            AccessHelper.OpenConnect();
            string sql = $"SELECT [PartNumber]  FROM {tablename} ";
            DataTable resultdataTable = AccessHelper.QueryDataTable(sql);
            List<string> list = new List<string>();
            if (resultdataTable.Rows.Count < 1)
            {
                return;
            }
            foreach (DataRow item in resultdataTable.Rows)
            {
                var str = item[0].ToString();
                list.Add(str);
            }
            list = list.Distinct().ToList();
            //AccessHelper.Dispose();
            this.comboBox_Parttnumber.Items.Clear();
            this.comboBox_Parttnumber.Items.AddRange(list.ToArray());
        }
        private void comboBox_Parttnumber_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {

                var tablename = comboBox1_Table.Items[comboBox1_Table.SelectedIndex].ToString();

                string startTime = string.Empty;
                string endTime = string.Empty;
                string result = string.Empty;
                string column = string.Empty;
                string PartNumber = comboBox_Parttnumber.Items[comboBox_Parttnumber.SelectedIndex].ToString();
                for (int i = 0; i < dataGridView_ColumnName.RowCount; i++)
                {

                    string select = ((DataGridViewCheckBoxCell)dataGridView_ColumnName.Rows[i].Cells[1]).Value.ToString();
                    if (select == "True")
                    {
                        string name = dataGridView_ColumnName.Rows[i].Cells[0].Value.ToString();
                        column += ",[" + name + "]";
                    }
                }
                if (column == string.Empty)
                {
                    column = "*";
                }
                else
                {
                    column = "[id]" + column + ",[PartNumber],[result]";
                }
                if (this.radioButton_all.Checked)
                {
                    result = $"AND [PartNumber] = '{PartNumber}'";
                }
                if (this.radioButton_pass.Checked)
                {
                    result = $"AND [PartNumber] = '{PartNumber}' AND [result] = 'Pass'";

                }
                if (this.radioButton_fail.Checked)
                {
                    result = $"AND [PartNumber] = '{PartNumber}' AND [result] = 'Fail'";
                }
                if (this.dateTimePicker_StartTime.Checked)
                {
                    startTime = this.dateTimePicker_StartTime.Value.ToString("yyyy-MM-dd HH:mm");


                }
                if (this.dateTimePicker_EndTime.Checked)
                {
                    endTime = this.dateTimePicker_EndTime.Value.ToString("yyyy-MM-dd HH:mm");

                }
                AccessHelper.OpenConnect();

                string sql = string.Empty;
                DataTable resultdataTable = null;
                if (!(this.dateTimePicker_StartTime.Checked) && !(this.dateTimePicker_EndTime.Checked))
                {
                    sql = "SELECT " + column + " FROM " + tablename + " WHERE 1=1 " + result;
                }
                else if (this.dateTimePicker_StartTime.Checked && this.dateTimePicker_EndTime.Checked)
                {
                    sql = "SELECT " + column + " FROM " + tablename + " WHERE 1=1 " + result + " AND CreateStartTime >=#" + startTime + "# AND CreateStartTime <=#" + endTime + "#";
                }
                else if (this.dateTimePicker_StartTime.Checked)
                {
                    sql = "SELECT " + column + " FROM " + tablename + " WHERE 1=1 " + result + " AND CreateStartTime >=#" + startTime + "#";
                }
                else if (this.dateTimePicker_EndTime.Checked)
                {
                    sql = "SELECT " + column + " FROM " + tablename + " WHERE 1=1 " + result + " AND CreateStartTime <=#" + endTime + "#";
                }
                if (sql != string.Empty)
                {
                    resultdataTable = AccessHelper.QueryDataTable(sql);
                    dataGridView_TableData.DataSource = resultdataTable.DefaultView;
                    ////指定的ID和结果
                    //if (resultdataTable == null)
                    //{
                    //    MessageBox.Show("没有找到数据");
                    //    return;
                    //}

                    //List<int> idList = new List<int>();
                    //List<double> RSList = new List<double>();
                    ////查找指定的列名字
                    //string specialColumnName = string.Empty;
                    //foreach (var item in resultdataTable.Columns)
                    //{
                    //    if (item.ToString().Contains("Resistance"))
                    //    {
                    //        specialColumnName = item.ToString();
                    //        break;
                    //    }
                    //}
                    //if (specialColumnName == string.Empty || !specialColumnName.Contains("Resistance"))
                    //{
                    //    MessageBox.Show("列表中没有选择到数值");
                    //    return;
                    //}

                    //int passCount = 0;
                    //for (int i = 0; i < resultdataTable.Rows.Count; i++)
                    //{
                    //    idList.Add(int.Parse(resultdataTable.Rows[i]["id"].ToString()));
                    //    RSList.Add(Math.Round(double.Parse(resultdataTable.Rows[i][specialColumnName].ToString()), 2));
                    //    if (resultdataTable.Rows[i]["result"].ToString().ToLower() == "pass")
                    //    {
                    //        passCount++;
                    //    }
                    //}
                    //int allCount = idList.Count;
                    //int failCount = allCount - passCount;
                    //textBox_All.Text = allCount.ToString();
                    //textBox_Pass.Text = passCount.ToString();
                    //textBox_Fail.Text = failCount.ToString();
                    //UpdateTECchart(idList, RSList);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearTECchart()
        {
            this.chartTEC.ChartAreas.Clear();
            this.chartTEC.Series.Clear();
            this.chartTEC.Titles.Clear();
        }
        private void UpdateTECchart(List<int> idList, List<double> selectConditionValues, string conditionName)
        {
            try
            {
                ChartArea chartArea = new ChartArea();
                chartArea.AxisX = new Axis();
                chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.DashDotDot;
                chartArea.AxisX.MajorGrid.Interval = Math.Ceiling(double.Parse((idList.Count / 5).ToString()));
                chartArea.AxisX.MajorGrid.Enabled = true;
                chartArea.AxisX.MinorGrid.LineDashStyle = ChartDashStyle.DashDotDot;
                chartArea.AxisX.MinorGrid.Enabled = true;
                chartArea.AxisX.MinorGrid.Interval = Math.Ceiling(double.Parse((idList.Count / 10).ToString()));


                double Ylimit_Range = 1;
                if (selectConditionValues.Max() == selectConditionValues.Min())
                {

                }
                else
                {
                    Ylimit_Range = Math.Round(selectConditionValues.Max() - selectConditionValues.Min(), 2);
                }
                double Ylimit_Up = Math.Round(selectConditionValues.Max() + Math.Abs(Ylimit_Range) * 0.05, 2);
                double Ylimit_Low = Math.Round(selectConditionValues.Min() - Math.Abs(Ylimit_Range) * 0.05, 2);
                double Interval_Large = Math.Abs(Ylimit_Range) / 4.0;
                double Interval_Small = Math.Abs(Ylimit_Range) / 20.0;

                chartArea.AxisY = new Axis();
                chartArea.AxisY.Minimum = double.MinValue;
                chartArea.AxisY.Maximum = Ylimit_Up;
                chartArea.AxisY.Minimum = Ylimit_Low;
                chartArea.AxisY.LabelStyle.Format = "N2";
                chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.DashDotDot;
                chartArea.AxisY.MajorGrid.Interval = Interval_Large;
                chartArea.AxisY.MajorGrid.Enabled = true;
                chartArea.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.DashDotDot;
                chartArea.AxisY.MinorGrid.Enabled = true;
                chartArea.AxisY.MinorGrid.Interval = Interval_Small;

                chartArea.CursorX.AutoScroll = true;
                chartArea.CursorX.IsUserEnabled = true;
                chartArea.CursorX.IsUserSelectionEnabled = true;

                this.chartTEC.ChartAreas.Add(chartArea);

                this.chartTEC.Titles.Add(conditionName);
                this.chartTEC.Titles[0].Text = conditionName;
                this.chartTEC.Titles[0].BackColor = Color.Purple;

                Series se1 = new Series(conditionName);
                se1.IsValueShownAsLabel = true;

                se1.ChartType = SeriesChartType.Point;
                se1.XAxisType = AxisType.Primary;
                se1.YAxisType = AxisType.Primary;
                se1.Color = Color.Red;
                for (int i = 0; i < idList.Count; i++)
                {
                    se1.Points.AddXY(idList[i], selectConditionValues[i]);
                }
                this.chartTEC.Series.Add(se1);
            }
            catch
            {
                MessageBox.Show("画图数据有问题");
            }

        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
            private void Export_Click(object sender, EventArgs e)
        {
            if (this.dataGridView_TableData.Rows.Count <= 0)
            {
                MessageBox.Show("当前没有可导出的数据！");
                return;
            }

            DataGridViewToExcel(dataGridView_TableData);
            MessageBox.Show("导出成功！");
        }
        /// <summary>
        /// 导出为Excel
        /// </summary>
        /// <param name="dgv">dataGridView_TableData</param>
        public static void DataGridViewToExcel(DataGridView dgv)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Execl files (*.xlsx)|*.xls";
            dlg.CheckFileExists = false;
            dlg.CheckPathExists = false;
            dlg.FilterIndex = 0;
            dlg.RestoreDirectory = true;
            dlg.CreatePrompt = true;
            dlg.Title = "保存为Excel文件";
            dlg.FileName = DateTime.Now.Ticks.ToString().Trim();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Stream myStream;
                myStream = dlg.OpenFile();
                StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
                string columnTitle = "";
                try
                {
                    //写入列标题   
                    for (int i = 0; i < dgv.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            columnTitle += "\t";
                        }
                        columnTitle += dgv.Columns[i].HeaderText;
                    }
                    sw.WriteLine(columnTitle);

                    //写入列内容   
                    for (int j = 0; j < dgv.Rows.Count; j++)
                    {
                        string columnValue = "";
                        for (int k = 0; k < dgv.Columns.Count; k++)
                        {
                            if (k > 0)
                            {
                                columnValue += "\t";
                            }
                            if (dgv.Rows[j].Cells[k].Value == null)
                            {
                                columnValue += "";

                            }
                            else
                            {
                                columnValue += dgv.Rows[j].Cells[k].Value.ToString().Trim();

                            }
                                
                        }
                        sw.WriteLine(columnValue);
                    }
                    sw.Close();
                    myStream.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                finally
                {
                    sw.Close();
                    myStream.Close();
                }
            }
        }
    }
}
