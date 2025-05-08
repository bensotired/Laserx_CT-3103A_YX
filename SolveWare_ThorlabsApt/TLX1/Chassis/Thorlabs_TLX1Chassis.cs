using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AxMG17NanoTrakLib;
using SolveWare_BurnInAppInterface;
using SolveWare_BurnInInstruments;

namespace SolveWare_BurnInInstruments
{

    public partial class Thorlabs_TLX1Chassis : InstrumentChassisBase, IInstrumentChassis
    {
        ITesterCoreInteration _core;
        public Thorlabs_TLX1Chassis(string name, string resource, bool isOnline) 
            : base(name, resource, isOnline)
        {
            try
            {
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Constructor error, Chassis resource = [{1}].[{2}]-[{3}]", this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg, ex);
            }
        }
        public override void SetupLogger(ILogHandle logHandler, IExceptionHandle exceptionHandler)
        {
            _core = logHandler as ITesterCoreInteration;
            base.SetupLogger(logHandler, exceptionHandler);
        }
        public override void Initialize()
        {
            try
            {
                

            }
            catch (Exception ex)
            {
                throw new Exception($"Initialize NanoTrakChassis exception:[{ex.Message}{ex.StackTrace}]", ex);
            }
           
        }
        public override void Initialize(int timeout)
        {
            try
            {
                if (this.IsOnline == true)
                {
                    this.Open_HID();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Initialize NanoTrakChassis exception:[{ex.Message}{ex.StackTrace}]", ex);
            }
   
        }
              
        public override void ClearConnection()
        {
            try
            {
                if (this.IsOnline == true)
                {
                    this.Close_HID();
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Initialize error. Chassis resource = [{1}].[{2}]-[{3}]", this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg);

            }
        }


        private static IntPtr Device = IntPtr.Zero;

        ushort vid = 0x1313;
        ushort pid = 0x5007;
        public  void Open_HID() 
        {
            uint count = 0;
            int ret = Thorlabs_TLX1Chassis.HidUart_GetNumDevices(ref count, vid, pid);

            if (ret != Thorlabs_TLX1Chassis.HID_UART_SUCCESS)
            {
                //AddLog_String($"HidUart_GetNumDevices 失败返回[{count}]");
                throw new Exception(string.Format("TLX1 StartControl fails. Error Code: {0}.", ret));
                //return;
            }
            // AddLog_String($"HidUart_GetNumDevices 成功返回[{count}]");


            //=====================
            ret = Thorlabs_TLX1Chassis.HidUart_Open(ref Device, count - 1, vid, pid);

            if (ret != Thorlabs_TLX1Chassis.HID_UART_SUCCESS)
            {
                // AddLog_String($"HidUart_Open 失败返回");
                throw new Exception(string.Format("TLX1 StartControl fails. Error Code: {0}.", ret));
                //return;
            }
            // AddLog_String($"HidUart_Open 成功返回");

            //=====================
            ret = Thorlabs_TLX1Chassis.HidUart_SetTimeouts(Device, 200, 200);

            if (ret != Thorlabs_TLX1Chassis.HID_UART_SUCCESS)
            {
                //  AddLog_String($"HidUart_SetTimeouts 失败返回");
                throw new Exception(string.Format("TLX1 StartControl fails. Error Code: {0}.", ret));
                //return;
            }
            // AddLog_String($"HidUart_SetTimeouts 成功返回");

            //=====================
            //读取垃圾数据
            uint retcount = 0;
            byte[] bstr = new byte[500];
            ret = Thorlabs_TLX1Chassis.HidUart_Read(Device, bstr, (uint)bstr.Length, ref retcount);
            if (retcount <= 0)//ret != SLABHIDTOUART.HID_UART_SUCCESS)
            {
                // AddLog_String($"HidUart_Read 没有无效数据");
                return;
            }
            string str = System.Text.Encoding.Default.GetString(bstr, 0, (int)retcount);
            // AddLog_String($"HidUart_Read 无效数据[{str}]");


        }
        public void Close_HID() 
        {
            if (Device != IntPtr.Zero)
            {
                int ret = Thorlabs_TLX1Chassis.HidUart_Close(Device);
                if (ret != Thorlabs_TLX1Chassis.HID_UART_SUCCESS)
                {
                    // AddLog_String($"HidUart_Close 失败返回");
                    throw new Exception(string.Format("TLX1 StartControl fails. Error Code: {0}.", ret));
                    //return;
                }
                //AddLog_String($"HidUart_Close 成功返回");

                Device = IntPtr.Zero;
                return;
            }
        }

        private  static string endchar = "\x0A";


        public override string Query(string cmd, int delay_ms)
        {
            return SendCommand(cmd);
        }
        //发送指令
        public static string SendCommand(string command)  
        {
            int ret;
            string str = string.Empty;
            if (Device == IntPtr.Zero)
            {
                // AddLog_String($"未打开"); 
                str = ($"TLX1未打开");
            }
            
            byte[] bstr = System.Text.Encoding.Default.GetBytes(command + endchar);

            uint retcount = 0;
            ret = Thorlabs_TLX1Chassis.HidUart_Write(Device, bstr, (uint)bstr.Length, ref retcount);
            if (ret != Thorlabs_TLX1Chassis.HID_UART_SUCCESS)
            {
                //AddLog_String($"HidUart_Write 失败返回");
                str = ($"TLX1_Write 失败返回");
                return str;
            }
            //AddLog_String2($"发送[{str}]");

            //=======================
            bstr = new byte[500];
            ret = Thorlabs_TLX1Chassis.HidUart_Read(Device, bstr, (uint)bstr.Length, ref retcount);
            if (retcount <= 0)//ret != SLABHIDTOUART.HID_UART_SUCCESS)
            {
                //AddLog_String($"HidUart_Read 失败返回");
                str = ($"TLX1_Write 失败返回");
                return str;
            }
            str = System.Text.Encoding.Default.GetString(bstr, 0, (int)retcount);
           str = str.Substring(0, str.Length - 1);

           // AddLog_String2($"接收[{str}]");

            return str;
        }
    }
}