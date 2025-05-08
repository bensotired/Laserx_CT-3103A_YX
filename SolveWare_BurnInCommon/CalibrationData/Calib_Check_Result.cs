using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SolveWare_BurnInCommon
{

    public static class Calib_Check_Result
    {
        static public string CalibrationData = @"Calibration\Data";

        //static string[,] objDataTable;

        static private DataTable objDataTable = new DataTable();

        static private DateTime CreateTime;

        public static Action<int, DataTable> SendMessageToGuiAction { get; set; }


        private static void UpdateStatus(int Step)
        {
            if (SendMessageToGuiAction != null)
            {
                SendMessageToGuiAction(Step, objDataTable);
            }
        }

        public static void InitDataTable()//, int RowCount)
        {
            objDataTable.Columns.Clear();

        }
        public static void InitDataTable(double[] CheckPoint, int RowCount)
        {
            //显示电源的校准数据
            objDataTable.Columns.Clear();
            objDataTable.Rows.Clear();

            //第一列
            //dgv_MeasureTable.Columns.Add("通道", "通道");

            //数据列
            foreach (double calPoint in CheckPoint)
            {
                objDataTable.Columns.Add("[I]" + calPoint.ToString() + "(A)");//电流值
                objDataTable.Columns.Add("[SMU_V]" + calPoint.ToString() + "(A)");//电压值
                objDataTable.Columns.Add("[Meter_V]" + calPoint.ToString() + "(A)");//电压值
            }

            for (int ch = 1; ch <= RowCount; ch++)
            {
                objDataTable.Rows.Add(objDataTable.NewRow());
            }

            UpdateStatus(0);

            CreateTime = DateTime.Now;
        }
        public static void InitDataTable_V2(double[] CheckPoint, int RowCount)
        {
            //显示电源的校准数据
            objDataTable.Columns.Clear();
            objDataTable.Rows.Clear();

            //第一列
            //dgv_MeasureTable.Columns.Add("通道", "通道");

            //数据列
            foreach (double calPoint in CheckPoint)
            {
                objDataTable.Columns.Add("[SMU_I]" + calPoint.ToString() + "(A)");//电流值
                objDataTable.Columns.Add("[Meter_I]" + calPoint.ToString() + "(A)");//电流值
                objDataTable.Columns.Add("[SMU_V]" + calPoint.ToString() + "(A)");//电压值
                objDataTable.Columns.Add("[Meter_V]" + calPoint.ToString() + "(A)");//电压值
            }

            for (int ch = 1; ch <= RowCount; ch++)
            {
                objDataTable.Rows.Add(objDataTable.NewRow());
            }

            UpdateStatus(0);

            CreateTime = DateTime.Now;
        }
        public static void InitDataTableByHeader(List<string> colHeaders, int RowCount)
        {
            //显示电源的校准数据
            objDataTable.Columns.Clear();
            objDataTable.Rows.Clear();

            //第一列
            //dgv_MeasureTable.Columns.Add("通道", "通道");

            //数据列
            //foreach (double calPoint in CheckPoint)
            //{
            //    objDataTable.Columns.Add("[SMU_I]" + calPoint.ToString() + "(A)");//电流值
            //    objDataTable.Columns.Add("[Meter_I]" + calPoint.ToString() + "(A)");//电流值
            //    objDataTable.Columns.Add("[SMU_V]" + calPoint.ToString() + "(A)");//电压值
            //    objDataTable.Columns.Add("[Meter_V]" + calPoint.ToString() + "(A)");//电压值
            //}
            foreach (string col in colHeaders)
            {
                objDataTable.Columns.Add(col); 
            }
            for (int ch = 1; ch <= RowCount; ch++)
            {
                objDataTable.Rows.Add(objDataTable.NewRow());
            }

            UpdateStatus(0);

            CreateTime = DateTime.Now;
        }


        public static void ChangeDataTable(int Row, int Col, string Value, bool ok)//, int RowCount)
        {
            //Calib_Check_Result.DataItem data = new Calib_Check_Result.DataItem();

            //data.Value = Value;
            //data.BackColor = color;

            objDataTable.Rows[Row][Col] = Value.ToString() + "_" + ok.ToString();

            UpdateStatus(1);
        }
        public static void ChangeDataTable(int Row, int Col, string Value)
        {
            ChangeDataTable(Row, Col, Value, true);
        }

        public static string GetCSVData()
        {
            // StreamWriter sw = new StreamWriter(fileName);

            StringBuilder sr = new StringBuilder();

            string strLine = "";
            try
            {

                DateTime dt = DateTime.Now;
                #region 数据表

                strLine = "<电流电压检查数据>";
                sr.AppendLine(strLine);
                //sw.WriteLine("\r\n");

                //校准日期
                strLine = "检查开始时间," + CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                sr.AppendLine(strLine);
                strLine = "文件存储时间," + dt.ToString("yyyy-MM-dd HH:mm:ss");
                sr.AppendLine(strLine);
                //sw.WriteLine("\r\n");

                //SN编号


                //表头
                strLine = "";
                for (int i = 0; i < objDataTable.Columns.Count; i++)
                {
                    if (i > 0)
                        strLine += ",";
                    strLine += objDataTable.Columns[i].ColumnName;
                }
                strLine.Remove(strLine.Length - 1);
                sr.AppendLine(strLine);
                strLine = "";
                //表的内容
                for (int j = 0; j < objDataTable.Rows.Count; j++)
                {
                    strLine = "";
                    int colCount = objDataTable.Columns.Count;
                    for (int k = 0; k < colCount; k++)
                    {
                        if (k > 0 && k < colCount)
                            strLine += ",";
                        if (objDataTable.Rows[j][k] == null)
                            strLine += "";
                        else
                        {
                            string stritem = (string)objDataTable.Rows[j][k];
                            string[] sp = stritem.Split('_');
                            string cell = sp[0];
                            bool ok = bool.Parse(sp[1]);
                            if (ok == false)
                            {
                                cell = "[X]" + cell;
                            }
                            //防止里面含有特殊符号
                            cell = cell.Replace("\"", "\"\"");
                            cell = "\"" + cell + "\"";
                            strLine += cell;
                        }
                    }
                    sr.AppendLine(strLine);
                }

                #endregion
            }
            catch (Exception ex)
            {
                sr.AppendLine(ex.Message);
            }

            return sr.ToString();
        }
    }
}
