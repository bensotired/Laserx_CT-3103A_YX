
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace SolveWare_TesterCore
{
    public static class TestPlatformStarterProgram
    {
        [STAThread]
        public static void Main(params string[] args)
      {

            //string path = @"D:\playground\temp5.csv";
            //using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            //{
            //    sw.WriteLine("你好1, 你好2,你好,你好4,0.53214324235");
            //    sw.WriteLine("你好1, 你好2,你好,你好4");
            //    sw.WriteLine("你好1, 你好2,你好,你好4");

            //    sw.WriteLine("你好1, 你好2,你好,你好4");

            //}
            //using (StreamReader sw = new StreamReader(path, Encoding.UTF8))
            //{
            //    string str=sw.ReadLine();


            //}

            //return;


            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                var mtx = new Mutex(false, "Global\\" + Application.ProductName);

                if (!mtx.WaitOne(0, false))
                {
                    mtx.Close();
                    System.Windows.Forms.MessageBox.Show("程序已经运行", "注意", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //20240728 禁止系统休眠
                SystemSleepManagement.PreventSleep();


                ThreadPool.SetMinThreads(1000, 1000);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Form_UniversalTesterMain frm_ubm = new Form_UniversalTesterMain();
                frm_ubm.Show();
                TesterCore.Instance.StartUp();
                frm_ubm.ConnectToCore(TesterCore.Instance);
                Application.Run(frm_ubm);

            }
           
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
            {
                if(e.ExceptionObject is System.Runtime.InteropServices.InvalidComObjectException)
                {
                    return;
                }
                object o = e.ExceptionObject;
                Trace.Assert(false, "UnhandledException: " + DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss") + " -> " + o.ToString());
            }
        }
    }


    //禁止休眠
    /* https://www.jb51.net/program/2851451zu.htm
     * 封装类

    用于阻止系统休眠的C#类。以下是代码注释的解释：

    DllImport("kernel32.dll")：定义了一个API函数，该函数在Windows内核中定义。
    enum ExecutionFlag : uint：定义了一个枚举类型，其中包含三个标志，分别用于阻止系统休眠、关闭显示器和继续执行。
    PreventSleep(bool includeDisplay = false)：这个方法用于阻止系统休眠，直到线程结束恢复休眠。
    如果includeDisplay参数为true，则还会阻止关闭显示器。
    ResotreSleep()：这个方法用于恢复系统休眠。

    使用了kernel32.dll中的SetThreadExecutionState函数来阻止系统休眠。我们还定义了一个枚举类型ExecutionFlag，
    用于指定阻止系统休眠的选项。我们可以使用SetThreadExecutionState函数来设置ExecutionFlag标志，以防止系统休眠
    因此，要实现下载时阻止程序休眠，则有两种实现方式：

    下载期间起计时器定期执行ResetSleepTimer函数
    下载开始时执行PreventSleep函数，下载结束后执行ResotreSleep函数。
    另外，和阻止系统休眠类似，有的程序还需要有阻止屏保功能。

    到此这篇关于C#实现系统休眠或静止休眠的文章就介绍到这了,更多相关c#系统休眠内容请搜索脚本之家以前的文章或继续浏览下面的相关文章希望大家以后多多支持脚本之家！
     */
    class SystemSleepManagement
    {
        //定义API函数
        [DllImport("kernel32.dll")]
        static extern uint SetThreadExecutionState(ExecutionFlag flags);
        [Flags]
        enum ExecutionFlag : uint
        {
            System = 0x00000001,
            Display = 0x00000002,
            Continus = 0x80000000,
        }
        /// <summary>
        ///阻止系统休眠，直到线程结束恢复休眠
        /// </summary>
        /// <param name="includeDisplay">是否阻止关闭显示器</param>
        public static void PreventSleep(bool includeDisplay = false)
        {
            if (includeDisplay)
                SetThreadExecutionState(ExecutionFlag.System | ExecutionFlag.Display | ExecutionFlag.Continus);
            else
                SetThreadExecutionState(ExecutionFlag.System | ExecutionFlag.Continus);
        }
        /// <summary>
        ///恢复系统休眠
        /// </summary>
        public static void ResotreSleep()
        {
            SetThreadExecutionState(ExecutionFlag.Continus);
        }
        /// <summary>
        ///重置系统休眠计时器
        /// </summary>
        /// <param name="includeDisplay">是否阻止关闭显示器</param>
        public static void ResetSleepTimer(bool includeDisplay = false)
        {
            if (includeDisplay)
                SetThreadExecutionState(ExecutionFlag.System | ExecutionFlag.Display);
            else
                SetThreadExecutionState(ExecutionFlag.System);
        }
    }
}
