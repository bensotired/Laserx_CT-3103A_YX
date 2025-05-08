using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using SolveWare_Motion;
using SolveWare_TestPlugin;
using SolveWare_Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT3103
    {

        /// <summary>
        /// 盘获取龙门位置信息
        /// </summary>
        /// <returns></returns>
        public XYCoord GetXYPosi()
        {
            XYCoord xY = new XYCoord();
            //xY.X = this.LocalResource.Axes[AxisNameEnum_CT3103.龙门_X].Get_CurUnitPos();
            //xY.Y = this.LocalResource.Axes[AxisNameEnum_CT3103.龙门_Y].Get_CurUnitPos();

            return xY;
        }
        /// <summary>
        /// 根据两点坐标和行数列数计算矩阵点位
        /// </summary>
        /// <param name="startX">左上角X</param>
        /// <param name="startY">左上角Y</param>
        /// <param name="stopX">右下角X</param>
        /// <param name="stopY">右下角Y</param>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <returns></returns>
        public List<XYCoord> CreatePointList(double startX, double startY, double stopX, double stopY, double rows, double columns)
        {
            List<XYCoord> retList = new List<XYCoord>();
            double rangeX = stopX - startX;
            double rangeY = stopY - startY;
            double diffColumns = columns - 1 >= 1 ? columns - 1 : 1;
            double diffRows = rows - 1 >= 1 ? rows - 1 : 1;
            double stepX = rangeX / diffColumns;//行与行之间的坐标是Y
            double stepY = rangeY / diffRows;
            stepX = rangeX > 0 ? Math.Abs(stepX) : -Math.Abs(stepX);
            stepY = rangeY > 0 ? Math.Abs(stepY) : -Math.Abs(stepY);
            for (int Y = 0; Y < rows; Y++)
            {
                for (int X = 0; X < columns; X++)
                {
                    retList.Add(new XYCoord(startX + X * stepX, startY + Y * stepY));
                }
            }
            return retList;
        }


        /// <summary>
        /// 匹配后计算坐标
        /// </summary>
        /// <param name="pos_current_unit_X">X轴当前位置</param>
        /// <param name="pos_current_unit_Y">Y轴当前位置</param>
        /// <param name="pixRatio_X">像素比X</param>
        /// <param name="pixRatio_Y">像素比Y</param>
        /// <param name="CameraCenterX">图像中心像素X</param>
        /// <param name="CameraCenterY">图像中心像素Y</param>
        /// <param name="tec_pix_current_X">模板中心像素X</param>
        /// <param name="tec_pix_current_Y">模板中心像素Y</param>
        /// <returns></returns>
        public XYCoord CalTargetPosition(double pos_current_unit_X, double pos_current_unit_Y,
                                  double pixRatio_X, double pixRatio_Y,
                              double CameraCenterX, double CameraCenterY,
                              double tec_pix_current_X, double tec_pix_current_Y)
        {
            XYCoord pointsUsing = new XYCoord();
            double dif_pix_X = CameraCenterX - tec_pix_current_X;
            double dif_pix_Y = CameraCenterY - tec_pix_current_Y;
            double dif_Unit_X = Math.Round(dif_pix_X * pixRatio_X, 3);
            double dif_Unit_Y = Math.Round(dif_pix_Y * pixRatio_Y, 3);
            pointsUsing.X = pos_current_unit_X + dif_Unit_X;
            pointsUsing.Y = pos_current_unit_Y + dif_Unit_Y;
            return pointsUsing;
        }
        public XYCoord CalPosition(double pos_current_unit_X, double pos_current_unit_Y,
                          double pixRatio_X, double pixRatio_Y,
                      double CameraCenterX, double CameraCenterY,
                      double tec_pix_current_X, double tec_pix_current_Y)
        {
            XYCoord pointsUsing = new XYCoord();
            double dif_pix_X = CameraCenterX - tec_pix_current_X;
            double dif_pix_Y = CameraCenterY - tec_pix_current_Y;
            double dif_Unit_X = Math.Round(dif_pix_X * pixRatio_X, 3);
            double dif_Unit_Y = Math.Round(dif_pix_Y * pixRatio_Y, 3);
            pointsUsing.X = pos_current_unit_X + dif_Unit_X;
            pointsUsing.Y = pos_current_unit_Y - dif_Unit_Y;
            return pointsUsing;
        }


        #region 暂停一个线程
        public void WaitMessage(CancellationTokenSource cancellationTokenSource)
        {
            this.Bridges_WithPauseFunc[Action3103.Signal_step1_wait].WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.NanoTimeout_ms, cancellationTokenSource);
        }


        /// <summary>
        /// 暂停第一个线程
        /// </summary>
        /// <param name="cancellationTokenSource"></param>
        public void WaitMessageChooseRestart(CancellationTokenSource cancellationTokenSource)
        {
            AutoResetEventItem_WithPauseCheckFunc signal_step_wait = this.Bridges_WithPauseFunc[Action3103.Signal_step1_wait];
            AutoResetEventItem_WithPauseCheckFunc signal_step_Restart = this.Bridges_WithPauseFunc[Action3103.Signal_step1_Restart];

            switch (signal_step_wait.WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.MiddumTimeout_ms, cancellationTokenSource))
            {
                case EventResult.SUCCEED:
                    {
                        do
                        {
                            switch (signal_step_Restart.WaitOne_BreakIfCancellationRequestedWithPauseFunc(Constant_CT3103.MiddumTimeout_ms, cancellationTokenSource))
                            {
                                case EventResult.SUCCEED:
                                    {
                                        return;
                                    }
                                case EventResult.CANCEL:
                                    {
                                        return;
                                    }
                            }
                        } while (true);
                    }
            }


        }

        public void SetWaitStep1()
        {
            this.Bridges_WithPauseFunc[Action3103.Signal_step1_wait].Set();
            Paused_IO(1.5);
        }
        public void SetRestartStep1()
        {
            this.Bridges_WithPauseFunc[Action3103.Signal_step1_Restart].Set();
            Resume_IO();
        }

        #endregion

        #region RefreshProductParameter
        public override void ReinstallController()
        {
            this.providerResourse.ReinstallController();
        }
        public override bool SwitchProductConfig()
        {
            this.providerResourse.SwitchProductConfig();
            this.RunResourceProvider();
            return true;
        }
        public override bool CreateProductConfig()
        {
            this.providerResourse.CreateProductConfig();

            return true;
        }
        #endregion

        #region TEC
        public void TEC_Controller_1(double temp, double temperatureTolerance)
        {
            try
            {
                var status = this.LocalResource.TC_1.DeviceStatus;
                switch (status)
                {
                    case SolveWare_BurnInInstruments.DeviceStatus.Ready:
                        {
                            this.LocalResource.TC_1.TemperatureSetPoint_DegreeC = temp;
                            this.LocalResource.TC_1.TemperatureDeviationDegreeC = temperatureTolerance;
                            this.LocalResource.TC_1.IsOutputEnabled = true;
                            this.Log_Global($"设定TC_1 温度为：{temp};波动范围：{temperatureTolerance}");
                        }
                        break;
                    case SolveWare_BurnInInstruments.DeviceStatus.Run:
                        {
                            this.LocalResource.TC_1.TemperatureSetPoint_DegreeC = temp;
                            this.LocalResource.TC_1.TemperatureDeviationDegreeC = temperatureTolerance;
                            this.LocalResource.TC_1.IsOutputEnabled = true;
                            this.Log_Global($"更改TC_1 温度为：{temp};波动范围：{temperatureTolerance}");
                        }
                        break;
                    case SolveWare_BurnInInstruments.DeviceStatus.Error:
                        {
                            this.Log_Global($"TC_1 控制器状态异常");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TC_1 异常--{ ex.Message}");
            }
        }
        public void TEC_Controller_1_Close()
        {
            try
            {
                var status = this.LocalResource.TC_1.DeviceStatus;
                switch (status)
                {
                    case SolveWare_BurnInInstruments.DeviceStatus.Ready:
                        {
                            this.Log_Global($"TC_1 未开启控温");
                        }
                        break;
                    case SolveWare_BurnInInstruments.DeviceStatus.Run:
                        {
                            this.LocalResource.TC_1.IsOutputEnabled = false;
                            this.Log_Global($"TC_1 关闭控温");
                        }
                        break;
                    case SolveWare_BurnInInstruments.DeviceStatus.Error:
                        {
                            this.Log_Global($"TC_1 控制器状态异常");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TC_1 异常--{ ex.Message}");
            }
        }
        public bool TEC_isStable_1()
        {
            try
            {
                var isStable = this.LocalResource.TC_1.isStable;
                if (isStable == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TC_1 异常--{ ex.Message}");
                return false;
            }
        }
        public void TEC_Controller_2(double temp, double temperatureTolerance)
        {
            try
            {
                var status = this.LocalResource.TC_2.DeviceStatus;
                switch (status)
                {
                    case SolveWare_BurnInInstruments.DeviceStatus.Ready:
                        {
                            this.LocalResource.TC_2.TemperatureSetPoint_DegreeC = temp;
                            this.LocalResource.TC_2.TemperatureDeviationDegreeC = temperatureTolerance;
                            this.LocalResource.TC_2.IsOutputEnabled = true;
                            this.Log_Global($"设定TC_2 温度为：{temp};波动范围：{temperatureTolerance}");
                        }
                        break;
                    case SolveWare_BurnInInstruments.DeviceStatus.Run:
                        {
                            this.LocalResource.TC_2.TemperatureSetPoint_DegreeC = temp;
                            this.LocalResource.TC_2.TemperatureDeviationDegreeC = temperatureTolerance;
                            this.LocalResource.TC_2.IsOutputEnabled = true;
                            this.Log_Global($"更改TC_2 温度为：{temp};波动范围：{temperatureTolerance}");
                        }
                        break;
                    case SolveWare_BurnInInstruments.DeviceStatus.Error:
                        {
                            this.Log_Global($"TC_2 控制器状态异常");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TC_2 异常--{ ex.Message}");
            }

        }
        public void TEC_Controller_2_Close()
        {
            try
            {
                var status = this.LocalResource.TC_2.DeviceStatus;
                switch (status)
                {
                    case SolveWare_BurnInInstruments.DeviceStatus.Ready:
                        {
                            this.Log_Global($"TC_2 未开启控温");
                        }
                        break;
                    case SolveWare_BurnInInstruments.DeviceStatus.Run:
                        {
                            this.LocalResource.TC_2.IsOutputEnabled = false;
                            this.Log_Global($"TC_2 关闭控温");
                        }
                        break;
                    case SolveWare_BurnInInstruments.DeviceStatus.Error:
                        {
                            this.Log_Global($"TC_2 控制器状态异常");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TC_2 异常--{ ex.Message}");
            }
        }
        public bool TEC_isStable_2()
        {
            try
            {
                var isStable = this.LocalResource.TC_2.isStable;
                if (isStable == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TC_2 异常--{ ex.Message}");
                return false;
            }
        }
        #endregion

        public bool Update_MotorPositionCollection(AxesPositionEnum_CT3103 PosEnum)
        {
            bool isok = true;
            try
            {
                var pos = this.LocalResource.Positions[PosEnum];
                foreach (var axisPos in pos.ItemCollection)
                {
                    var axisInstance = this.LocalResource.Axes[(AxisNameEnum_CT3103)Enum.Parse(typeof(AxisNameEnum_CT3103), axisPos.Name)];
                    //(
                    //    axis =>
                    //    axis.Name == axisPos.Name &&
                    //    axis.MotorGeneralSetting.MotorTable.CardNo == Convert.ToInt16(axisPos.CardNo) &&
                    //    axis.MotorGeneralSetting.MotorTable.AxisNo == Convert.ToInt16(axisPos.AxisNo)
                    //);
                    if (axisInstance == null)
                    {
                        isok = false;
                        break;
                    }
                    else
                    {
                        axisPos.Position = axisInstance.Get_CurUnitPos();
                    }
                }

            }
            catch (Exception ex)
            {
                isok = false;
            }
            return isok;
        }

        public void Ser()
        {
            SqlConnStr SqlConnStr = new SqlConnStr();
            string configFileFullPath = System.IO.Path.Combine(Application.StartupPath, "ConfigFiles", "SqlConnStr.xml");
            XmlHelper.SerializeFile<SqlConnStr>(configFileFullPath, SqlConnStr);
        }

        public SqlConnStr Dser()
        {
            string configFileFullPath = System.IO.Path.Combine(Application.StartupPath, "ConfigFiles", "SqlConnStr.xml");
            var sqlstr = XmlHelper.DeserializeFile<SqlConnStr>(configFileFullPath);
            return sqlstr;
        }

        public bool GetCarrierNumber(out List<string> CarrierNumberlist, out int count)
        {
            try
            {
                count = 0;
                string configFileFullPath = System.IO.Path.Combine(Application.StartupPath, "SerialNumber", "CarrierNumber.csv");
                string[] allLine = File.ReadAllLines(configFileFullPath);
                CarrierNumberlist = new List<string>();
                foreach (var line in allLine)
                {
                    string[] columns = line.Split(',');
                    CarrierNumberlist.Add(columns[0]);
                    if (columns[0] != "NA" && string.IsNullOrEmpty(columns[0])==false)
                    {
                        count++;
                    }

                }
                return true;

            }
            catch (Exception ex)
            {
                count = 0;
                CarrierNumberlist = new List<string>();
                this.Log_Global($"[CarrierNumber]  Get Eorr");
                return false;
                throw;
            }
        }
        public List<string> GetCarrierChipNumber(string CarrierNumber)
        {
            try
            {
                string configFileFullPath = System.IO.Path.Combine(Application.StartupPath, "SerialNumber", $"{CarrierNumber}.csv");
                string[] allLine = File.ReadAllLines(configFileFullPath);
                var ChipNumberlist = new List<string>();
                foreach (var line in allLine)
                {
                    string[] columns = line.Split(',');
                    ChipNumberlist.Add(columns[0]);
                    //if (columns[0] != "NA" && string.IsNullOrEmpty(columns[0]) == false)
                    //{
                    //    count++;
                    //}
                }
                return ChipNumberlist;
            }
            catch (Exception ex)
            {
                throw new Exception($"[{ex.Message}]-[{ex.StackTrace}]");
            }
        }


        public void SerUserCustomized(UserCustomizedInformation userCustomized)
        {
            string configFileFullPath = System.IO.Path.Combine(Application.StartupPath, "ConfigFiles", "UserCustomizedInformation.xml");
            XmlHelper.SerializeFile<UserCustomizedInformation>(configFileFullPath, userCustomized);
        }
        public UserCustomizedInformation DserUserCustomized()
        {
            string configFileFullPath = System.IO.Path.Combine(Application.StartupPath, "ConfigFiles", "UserCustomizedInformation.xml");
            var userCustomized = XmlHelper.DeserializeFile<UserCustomizedInformation>(configFileFullPath);
            return userCustomized;
        }

    }
    public class ParameterInformation
    {
        public ParameterInformation()
        {
            this.CarrierNumber = 0;
            this.LowTemperature = 30;
            //this.MediumTemperature = 40;
            this.HightTemperature = 60;
            this.TemperatureListLeft = new List<double>();
            this.TemperatureListRight = new List<double>();
            this.TemperatureTolerance = 0.3;

            this.CarrierNumberlist = new List<string>();
        }
        public int CarrierNumber { get; set; }
        public int UnLoadCarrierNumber { get; set; }
        public double LowTemperature { get; set; }
        //public double MediumTemperature { get; set; }
        public double HightTemperature { get; set; }

        public List<double> TemperatureListLeft { get; set; }//左载台目标温度列表
        public List<double> TemperatureListRight { get; set; }//右载台目标温度列表
        public double TemperatureTolerance { get; set; }//温度容差范围

        public string TargetTableName { get; set; }//目标表名
        public string TargetColumnName { get; set; }//目标列名

        public bool ComparativeCalculation { get; set; }//是否比较数据
        public string ComparativeTableName { get; set; }//用来比较的表名
        public string ComparativeColumnName { get; set; }//用来比较的列名gg

        public List<string> CarrierNumberlist { get; set; }



    }

    public static class GetControls
    {
        #region 获取Control
        public static Dictionary<string, Control> GetFormControls(this Form form)
        {
            List<Control> ctrlFamily = new List<Control>();
            foreach (Control item in form.Controls)
            {
                GetControlFamily(item, ref ctrlFamily);
            }

            Dictionary<string, Control> ctrlDict = new Dictionary<string, Control>();

            foreach (var item in ctrlFamily)
            {
                if (item.Name != null && item.Name.Length > 0)
                {
                    ctrlDict.Add(item.Name, item);
                }
            }
            //ctrlFamily.ForEach(item => ctrlDict.Add(item.Name, item));

            return ctrlDict;
        }
        public static void GetControlFamily(Control ctrl, ref List<Control> ctrlFamily)
        {
            if (ctrl.Controls.Count > 0)
            {
                foreach (Control subCtrl in ctrl.Controls)
                {
                    GetControlFamily(subCtrl, ref ctrlFamily);
                }
            }
            else
            {
                ctrlFamily.Add(ctrl);
            }
        }
        public static TCtrl GetCtrl<TCtrl>(string ctrlName, Dictionary<string, Control> dict)
        {
            if (dict.ContainsKey(ctrlName))
            {
                return (TCtrl)((object)dict[ctrlName]);
            }
            return default(TCtrl);
        }
        #endregion
    }

    public class Status
    {
        public Status()
        {
            this.LoadAndUnLoad = Operation.Idle;
            this.LeftStation = true;
            this.RightStation = true;
            this.LoadLeftStation = true;
            this.UnLoadLeftStation = false;
            this.LoadRightStation = true;
            this.UnLoadRightStation = false;

            this.LeftStationTest = TestStatusOnBoard.未测试;
            this.RightStationTest = TestStatusOnBoard.未测试;
            this.TempControlStateLeft = Operation.Idle;
            this.TempControlStateRight = Operation.Idle;
            this.IsTesting = Operation.Idle;
        }

        public Operation LoadAndUnLoad { get; set; }//上下料状态
        public bool LeftStation { get; set; }//左载台启用
        public bool RightStation { get; set; }//右载台启用
        public bool LoadLeftStation { get; set; }//左载台上料
        public bool UnLoadLeftStation { get; set; }//左载台卸料
        public bool LoadRightStation { get; set; }//右载台上料
        public bool UnLoadRightStation { get; set; }//右载台卸料

        public TestStatusOnBoard LeftStationTest { get; set; }
        public TestStatusOnBoard RightStationTest { get; set; }
        public Operation TempControlStateLeft { get; set; }
        public Operation TempControlStateRight { get; set; }
        public Operation IsTesting { get; set; }//测试状态
    }

    [Serializable]
    public class SqlConnStr
    {
        public SqlConnStr()
        {
            //EnableSql = false;
            //DBConnStr = @"Server=localhost;DataBase=LX_DB;user=root;password=LaserX";
            //DBTableName_GS = "GoldenSample";
            //DBTableName_Pre = "PreBI";
            //DBTableName_Post = "PostBI";
            //Post_ColumnNameList = new List<string> { "PostBI1", "PostBI2", "PostBI3" };
        }

        public bool EnableSql { get; set; }
        public string DBConnStr { get; set; }
        public string DBTableName_GS { get; set; }
        public string DBTableName_Pre { get; set; }
        public string DBTableName_Post { get; set; }
        public List<string> Post_ColumnNameList { get; set; }
    }

    [Serializable]
    public class UserCustomizedInformation
    {
        public string Left_Temp { get; set; } = "55";
        public string Right_Temp { get; set; } = "55";
        public string Tolerance_Temp { get; set; } = "0.3";

        public string MaskName { get; set; } = "demo";
        public string WaferName { get; set; } = "demo";
        public string ChipName { get; set; } = "demo";
        public string WorkOrder { get; set; } = "demo";
    }
}
