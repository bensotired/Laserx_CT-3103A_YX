using LX_BurnInSolution.Utilities;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInCommon;

using SolveWare_BurnInMessage;

using System;
using System.Reflection;
using System.Windows.Forms;

namespace SolveWare_TesterCore
{
    public partial class Form_StationBoard : Form, ITesterCoreLink, IAccessPermissionLevel
    {
       const int CHAS_NAME_COL_INDEX = 0;
       const int CHAS_RESOURCE_COL_INDEX = 1;
       const int CHAS_ONLINE_COL_INDEX = 2;
       const int CHAS_INITTIME_COL_INDEX = 3;
       const int CHAS_CHASSISTYPE_COL_INDEX = 4;

        const int INST_NAME_COL_INDEX = 0;
        const int INST_ADDR_COL_INDEX = 1;
        const int INST_ONLINE_COL_INDEX = 2;
        const int INST_CHASSISNAME_COL_INDEX = 3;
        const int INST_TYPE_COL_INDEX = 4;
        //const int INST_SIMU_COL_INDEX = 3;
        //const int INST_CHASSISNAME_COL_INDEX = 4;
        //const int INST_TYPE_COL_INDEX = 5;

        public Form_StationBoard()
        {
            InitializeComponent();
            //this.TopLevel = false;
        }
        ITesterCoreInteration _core;

        public AccessPermissionLevel APL
        {
            get
            {
                return AccessPermissionLevel.None;
            }
        }
        public void ConnectToCore(ITesterCoreInteration core)
        {
            _core = core as ITesterCoreInteration;
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
            _core.SendOutFormCoreToGUIEvent += ReceiveMessageFromCore;
        }
        public void DisconnectFromCore(ITesterCoreInteration core)
        {
            _core.SendOutFormCoreToGUIEvent -= ReceiveMessageFromCore;
        }

 
        private void Dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                var dgv = sender as DataGridView;
                if (e.Exception is System.ArgumentException)
                {
                    if (dgv[e.ColumnIndex, e.RowIndex] is DataGridViewComboBoxCell)
                    {
                        var value = dgv[e.ColumnIndex, e.RowIndex].Value;
                        var valueType = (dgv[e.ColumnIndex, e.RowIndex].Tag as PropertyInfo).PropertyType;

                        if (valueType.IsEnum)
                        {
                            dgv[e.ColumnIndex, e.RowIndex].Value = Converter.ConvertObjectTo(value, valueType).ToString();
                        }
                        else
                        {
                            dgv[e.ColumnIndex, e.RowIndex].Value = Converter.ConvertObjectTo(value, valueType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void Form_MessageBorad_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                e.Cancel = true;
                this._core.DockingStationBoard();
            }
            catch (Exception ex)
            {
            }
        }
        private void 浮动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Parent.Controls.Count > 0)
                {
                    this.Parent.Controls.Clear();
                }
                this._core.PopUI(this);
            }
            catch (Exception ex)
            {
            }
        }

        private void 还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this._core.DockingStationBoard();
            }
            catch (Exception ex)
            {

            }
        }
        private void ReceiveMessageFromCore(IMessage message)
        {
            switch (message.Type)
            {
                case EnumMessageType.Internal:
                    HandleInternalMessage(message);
                    break;
                default:
                    break;
            }
        }

        private void HandleInternalMessage(IMessage message)
        {
            try
            {
                var im = message as InternalMessage;
                switch (im.OperationType)
                {
                    case InternalOperationType.TestStation_Initialized:
                        {
                            UpdateEditorContext();
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                this._core.ReportException(ex.Message,ErrorCodes.PlatformUIActionFailed, ex);
            }
        }


        private void UpdateEditorContext( )
        {

            try
            {
                this.Invoke((EventHandler)delegate
                {
                    UIGeneric.FillListDGV_InstrumentChassisConfigItem
                      (
                          this.dgv_InstChas,
                          this._core.StationConfig.InstrumentChassisConfigs,
                          CHAS_NAME_COL_INDEX,
                          CHAS_RESOURCE_COL_INDEX,
                          CHAS_ONLINE_COL_INDEX,
                          CHAS_INITTIME_COL_INDEX,
                          CHAS_CHASSISTYPE_COL_INDEX
                      );
                    UIGeneric.FillListDGV_InstrumentConfigItem
                      (
                          this.dgv_Inst,
                          this._core.StationConfig.InstrumentConfigs,
                          INST_NAME_COL_INDEX,
                          INST_ADDR_COL_INDEX,
                          INST_ONLINE_COL_INDEX,
                          //INST_SIMU_COL_INDEX,
                          INST_CHASSISNAME_COL_INDEX,
                          INST_TYPE_COL_INDEX
                      );
                });

            }
            catch (Exception ex)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var debug2 = (this._core.GetStationHardwareObject("SourceMeter_2") as SolveWare_BurnInInstruments.ISourceMeter_Golight).InstrumentSerialNumber;
            var debug1 = (this._core.GetStationHardwareObject("SourceMeter_1") as SolveWare_BurnInInstruments.ISourceMeter_Golight).InstrumentSerialNumber;
        }
    }
}