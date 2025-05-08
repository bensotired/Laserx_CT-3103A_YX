using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SolveWare_Motion
{
    public partial class Form_AxesRealtimeInfo : Form , IAccessPermissionLevel
    {
        System.Timers.Timer refreshTimer = new System.Timers.Timer(200);
        IMotionController _motionController;
        public Form_AxesRealtimeInfo()
        {
            InitializeComponent();
           
            this.dgv_axesInfo.RowCount = 15;
            refreshTimer.Elapsed += RefreshTimer_Elapsed;
        }
     
        private void RefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            refreshTimer.Stop();
            try
            {
                if (_motionController == null)
                {
                    return;
                }
                if (_motionController.AxesPositionMonitor.Count <= 0)
                {
                    return;
                }
                else
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((EventHandler)delegate { RefreshAxesInfo(); });
                    }
                    else
                    {
                        RefreshAxesInfo();
                    }
                }
            }
            catch
            {

            }
            refreshTimer.Start();
        }
        void RefreshAxesInfo()
        {
            const int seed = 2;


            int destRowCount = _motionController.AxesPositionMonitor.Count / seed +
                               _motionController.AxesPositionMonitor.Count % seed;

            var listCount = _motionController.AxesPositionMonitor.Count;
            //var keys = _motionController.AxesPositionMonitor.Keys.ToList();
            //var values = _motionController.AxesPositionMonitor.Values.ToList();
            if (this.dgv_axesInfo.RowCount < destRowCount)
            {
                this.dgv_axesInfo.RowCount = destRowCount;
            }

            if (listCount % seed == 0)
            {
                for (int rowIndex = 0; rowIndex < destRowCount; rowIndex++)
                {
                    this.dgv_axesInfo.Rows[rowIndex].SetValues(
                        _motionController.AxesPositionMonitor[rowIndex * 2].AxisName,
                        _motionController.AxesPositionMonitor[rowIndex * 2].CurrentPosition,
                        _motionController.AxesPositionMonitor[rowIndex * 2].IsServoOn,
                        _motionController.AxesPositionMonitor[rowIndex * 2].IsAlarm,

                        _motionController.AxesPositionMonitor[rowIndex * 2 + 1].AxisName,
                        _motionController.AxesPositionMonitor[rowIndex * 2 + 1].CurrentPosition,
                        _motionController.AxesPositionMonitor[rowIndex * 2 + 1].IsServoOn,
                        _motionController.AxesPositionMonitor[rowIndex * 2 + 1].IsAlarm);
                    //keys[rowIndex * 2 + 1], 
                    //    values[rowIndex * 2 + 1]);
                }
            }
            else if (listCount % seed == 1)
            {
                for (int rowIndex = 0; rowIndex < destRowCount; rowIndex++)
                {
                    if (rowIndex == destRowCount - 1)
                    {
                        //this.dgv_axesInfo.Rows[rowIndex].SetValues(keys[rowIndex * 2], values[rowIndex * 2]);
                            this.dgv_axesInfo.Rows[rowIndex].SetValues(
                        _motionController.AxesPositionMonitor[rowIndex * 2].AxisName,
                        _motionController.AxesPositionMonitor[rowIndex * 2].CurrentPosition,
                        _motionController.AxesPositionMonitor[rowIndex * 2].IsServoOn,
                        _motionController.AxesPositionMonitor[rowIndex * 2].IsAlarm);
                    }
                    else
                    {
                        this.dgv_axesInfo.Rows[rowIndex].SetValues(
                        _motionController.AxesPositionMonitor[rowIndex * 2].AxisName,
                        _motionController.AxesPositionMonitor[rowIndex * 2].CurrentPosition,
                        _motionController.AxesPositionMonitor[rowIndex * 2].IsServoOn,
                        _motionController.AxesPositionMonitor[rowIndex * 2].IsAlarm,

                        _motionController.AxesPositionMonitor[rowIndex * 2 + 1].AxisName,
                        _motionController.AxesPositionMonitor[rowIndex * 2 + 1].CurrentPosition,
                        _motionController.AxesPositionMonitor[rowIndex * 2 + 1].IsServoOn,
                        _motionController.AxesPositionMonitor[rowIndex * 2 + 1].IsAlarm);
                        //this.dgv_axesInfo.Rows[rowIndex].SetValues(keys[rowIndex * 2], values[rowIndex * 2], keys[rowIndex * 2 + 1], values[rowIndex * 2 + 1]);
                    }
                }
            }
        }

        public AccessPermissionLevel APL
        {
            get
            {
                return AccessPermissionLevel.None;
            }
        }

        public void AssignMotionController(IMotionController motionController)
        {
            this._motionController = motionController;
        }

        private void Form_AxesRealtimeInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.refreshTimer.Stop();
            this._motionController = null;
        }

        private void Form_AxesRealtimeInfo_Load(object sender, System.EventArgs e)
        {
            this.refreshTimer.Start();
        }

        private void chk_topMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = (sender as CheckBox).Checked;
        }
    }
}
