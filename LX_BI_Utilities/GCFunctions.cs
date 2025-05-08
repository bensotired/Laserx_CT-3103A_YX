using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LX_BurnInSolution.Utilities
{
    public static class GCFunctions
    {

        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        private static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        public static void ForceReleaseResouces()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    GCFunctions.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    GCFunctions.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch
            {

            }
        }
    }
}
