using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SolveWare_BurnInInstruments
{
    public class NiSync
    {
        [DllImport("niSync.dll", EntryPoint = "niSync_init", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        public static extern int init(string resourceName, bool IDQuery, bool resetDevice, out IntPtr vi);

        [DllImport("niSync.dll", EntryPoint = "niSync_ConnectTrigTerminals", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        public static extern int ConnectTrigTerminals(IntPtr vi, string srcTerminal, string destTerminal, string syncClock, int invert, int updateEdge);

        [DllImport("niSync.dll", EntryPoint = "niSync_DisconnectTrigTerminals", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Ansi)]
        public static extern int DisconnectTrigTerminals(IntPtr vi, string srcTerminal, string destTerminal);

        [DllImport("niSync.dll", EntryPoint = "niSync_close", CallingConvention = CallingConvention.Winapi)]
        public static extern int close(IntPtr vi);

        [DllImport("niSync.dll", EntryPoint = "niSync_error_message", CallingConvention = CallingConvention.Winapi)]
        public static extern int error_message(IntPtr vi, int errorCode, IntPtr errorMessage);

        static IntPtr session66xx = IntPtr.Zero;
        static string errorMessage = "";
        public static string ERROR_CHECK(int errorCode)
        {
            byte[] msg_buf = new byte[512];
            if (errorCode < 0)
            {
                close(session66xx);
            }
            GCHandle gch = GCHandle.Alloc(msg_buf, GCHandleType.Pinned);
            IntPtr ptr = gch.AddrOfPinnedObject();
            error_message(IntPtr.Zero, errorCode, ptr);
            gch.Free();
            int str_len = Array.IndexOf(msg_buf, (byte)0);
            errorMessage = Encoding.UTF8.GetString(msg_buf, 0, str_len);
            return errorMessage;
        }
    }
}
