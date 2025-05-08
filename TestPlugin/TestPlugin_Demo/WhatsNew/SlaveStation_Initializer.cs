using SolveWare_BurnInCommon;
using SolveWare_SlaveStation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPlugin_Demo
{
    public sealed partial class TestPluginWorker_CT3103
    {
        public SlaveStation_Initializer GetSlaveStation_Initializer(string stationName )
        {
            try
            {


                SlaveStation_Initializer ssi = new SlaveStation_Initializer();
                var config = this.providerResourse.SlaveStation_Provider[stationName];
 
                ssi.GetSlaveStationResources(
                                                stationName,
                                                config,
                                                this.LocalResource.Local_Axis_ResourceProvider,
                                                this.LocalResource.Local_IO_ResourceProvider,
                                                this.LocalResource.Local_AxesPosition_ResourceProvider
                                            //updatePluginAxesPositionAction
                                            );

                return ssi;
            }
            catch (Exception ex)
            {
                this.ReportException($"获取小工站[{stationName}]加载器失败!", ErrorCodes.TestPluginRuntimeExpectedError, ex);
                MessageBox.Show($"获取小工站[{stationName}]加载器失败!");
            }
            return null;
        }

        public SlaveStation_Initializer GetSlaveStation_Initializer(string stationName,Form frm)
        {
            try
            {


                SlaveStation_Initializer ssi = new SlaveStation_Initializer();
                var config = this.providerResourse.SlaveStation_Provider[stationName];

                //(frm as IFrm_Calibration).Setup(this.bizManager_CT3103, this.providerResourse);
                this._coreInteration.ModifyDockableUI(frm,true);
                ssi.GetSlaveStationResources(
                                                stationName,
                                                config,
                                                this.LocalResource.Local_Axis_ResourceProvider,
                                                this.LocalResource.Local_IO_ResourceProvider,
                                                this.LocalResource.Local_AxesPosition_ResourceProvider
                                       
                                            );

                return ssi;
            }
            catch (Exception ex)
            {
                this.ReportException($"获取小工站[{stationName}]加载器失败!", ErrorCodes.TestPluginRuntimeExpectedError, ex);
                MessageBox.Show($"获取小工站[{stationName}]加载器失败!");
            }
            return null;
        }
    }
}
