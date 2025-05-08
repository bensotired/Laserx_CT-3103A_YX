using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    public enum IONameEnum_CT3103
    {
        SEN_FLOW,//流量感应器  00
        BTN_STR,//01
        BTN_STP,//02
        BTN_RST,//03
        SEN_FRN_DOOR,//04
        SEN_BK_L_DOOR,//05
        SEN_BK_R_DOOR,//06
        SEN_TRAY_IN_UP,//进料到位感应  07
        SEN_TRAY_OUT_UP,//出料到位感应  08
        SEN_STG_L_CARRIER_UP,//左夹具到位感应(上)
        SEN_STG_L_CARRIER_DN,//左夹具到位感应(下)
        SEN_STG_R_CARRIER_UP, //右夹具到位感应(上)
        SEN_STG_R_CARRIER_DN, //右夹具到位感应(下)
        SEN_STG_L_CARRIER_LOAD,//左夹具底部感应
        SEN_STG_R_CARRIER_LOAD,//右夹具底部感应
        //SEN_STG_L_HOLDER_UP,
        //SEN_STG_L_HOLDER_DN,
        //SEN_STG_R_HOLDER_UP,
        //SEN_STG_R_HOLDER_DN,
        //SEN_TEST_HEAD_L,
        //SEN_TEST_HEAD_R,
        SEN_PICKER_UP,//抓手上升感应信号
        SEN_PICKER_DN,//抓手下降感应信号
        SEN_PICKER_ROTATE_L,//抓手正面感应信号
        SEN_PICKER_ROTATE_R,//抓手反面感应信号
        SEN_PICKER_GRAB_ON,//抓手收紧感应信号
        SEN_Left_Back_Probe,//左后探针触点
        SEN_Left_Front_Probe,//左前探针触点
        SEN_Right_Front_Probe,//右前探针触点
        SEN_Right_Back_Probe,//右后探针触点
        SEN_Vacuum_L,//左真空
        SEN_Vacuum_R,//右真空
        LAMP_BTN_STR,//00
        LAMP_BTN_STP,//01
        LAMP_BTN_RST,//02
        TWR_RED,//03
        TWR_GRN,//04
        TWR_YEL,//05
        BEEP,//06
        //OPTICALSWITCH,//切PD的  07
        //CYL_STG_L_HOLDER_UP,//10
        //CYL_STG_L_HOLDER_DN,//11
        //CYL_STG_R_HOLDER_UP,//12
        //CYL_STG_R_HOLDER_DN,//13
        //CYL_TEST_HEAD_L,//14
        //CYL_TEST_HEAD_R,//15
        Pedestal_Vacuum_L,//左平台真空
        Pedestal_Vacuum_R,//右平台真空
        CYL_PICKER_UP,//抓手上升  16
        CYL_PICKER_DN,//抓手下降  17
        CYL_PICKER_ROTATE_L,//抓手正面  18
        CYL_PICKER_ROTATE_R,//抓手反面  19
        CYL_PICKER_GRAB_ON,//抓手收紧  110
        CYL_PICKER_GRAB_OFF,//抓手放松  111
        TEC_Left,
        TEC_Right,
        PD_3,
    }
}