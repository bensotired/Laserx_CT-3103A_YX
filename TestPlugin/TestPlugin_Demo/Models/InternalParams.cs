using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_1120A
{
    internal class InternalParams
    {
        internal InternalParams()
        {

        }

        internal bool IsFree_Station1_P_ChipSlot { get; set; } = true;
        internal bool IsFree_Station1_N_ChipSlot { get; set; } = true;

        internal bool IsFree_Station2_P_ChipSlot { get; set; } = true;

        internal bool IsFree_Station2_N_ChipSlot { get; set; } = true;

        internal StationSlotUnderTest Station1_TestingSlot { get; set; } = StationSlotUnderTest.None;

        internal StationSlotUnderTest Station2_TestingSlot { get; set; } = StationSlotUnderTest.None;



        internal int DeviceCountSimu { get; set; } = 1;


        internal List<UnLoadInfo> LoadPosi_ing { get; set; } = new List<UnLoadInfo>();

        internal List<UnLoadInfo> LoadPosi_ed { get; set; } = new List<UnLoadInfo>();

        internal List<UnLoadInfo> LoadPosi_mis { get; set; } = new List<UnLoadInfo>();


        internal Dictionary<SorterType, List<UnLoadInfo>> OutPutSolutionDict { get; set; } = new Dictionary<SorterType, List<UnLoadInfo>>();


        /// <summary>
        /// 整机结束运行
        /// </summary>
        internal bool IsTester_Exit { get; set; } = false;


        internal string WaferID { get; set; } = string.Empty;

        internal bool IsWafer_SerachDevice { get; set; } = true;

        internal bool IsWafer_DeviceLost_Check { get; set; } = true;

        internal bool IsWafer_NoDevice_Handled { get; set; } = true;



        internal bool IsNozzle1_DeviceLost_Check { get; set; } = true;


        internal bool IsNozzle1_GainDevice_FromWafer { get; set; } = true;



        //internal bool IsNozzle1_NoDevice_Stop { get; set; } = false;

        internal bool IsNozzle2_DeviceLost_Check { get; set; } = false;



        internal bool IsNozzle3_NoDevice { get; set; } = true;



        internal bool IsBin_NoHouse_Handled { get; set; } = true;

        internal bool IsBin_ReNew_By_User { get; set; } = false;

        internal SorterType Sorter { get; set; } = default(SorterType);

    }
}
