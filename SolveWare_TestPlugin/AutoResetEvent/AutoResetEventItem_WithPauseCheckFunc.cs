using SolveWare_BurnInCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SolveWare_TestPlugin
{
 
    public class AutoResetEventItem_WithPauseCheckFunc : CURDItemLite
    {
        public AutoResetEventItem_WithPauseCheckFunc(Func<bool> pauseFunc)
        {
            _AutoResetEvent = new AutoResetEvent(false);
            _pauseFunc = pauseFunc;
        } 
        AutoResetEvent _AutoResetEvent { get; }
        Func<bool> _pauseFunc { get; set;  }
        //public virtual bool Set()
        //{
        //    return _AutoResetEvent.Set();
        //}
        public virtual bool Set()
        {
            do
            {
                Thread.Sleep(20);
            }
            while (_pauseFunc?.Invoke() == true);
            Debug.WriteLine("are_ is set after pause check");
            return _AutoResetEvent.Set();
        }
        public virtual bool Reset()
        {
            return _AutoResetEvent.Reset(); 
        }
        //public virtual EventResult WaitOne(int millisecondsTimeout,  bool exitContext)
        //{
        //    if (_AutoResetEvent.WaitOne(millisecondsTimeout, exitContext))
        //    {
        //        return EventResult.SUCCEED;
        //    }
        //    else
        //    {
        //        if (exitContext)
        //        {
        //            return EventResult.CANCEL;
        //        }
        //        else
        //        {
        //            return EventResult.TIMEOUT;
        //        }
        //    }
        //}
        //public virtual EventResult WaitOne(int millisecondsTimeout,  CancellationTokenSource exitContext)
        //{
        //    if (_AutoResetEvent.WaitOne(millisecondsTimeout, exitContext.IsCancellationRequested))
        //    {
        //        return EventResult.SUCCEED;
        //    }
        //    else
        //    {
        //        if (exitContext.IsCancellationRequested)
        //        {
        //            return EventResult.CANCEL;
        //        }
        //        else
        //        {
        //            return EventResult.TIMEOUT;
        //        }
        //    }
        //}
        //public virtual EventResult WaitOne_BreakIfCancellationRequested(int millisecondsTimeout, CancellationTokenSource exitContext)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    do
        //    {
        //        //genggai10
        //        if (_AutoResetEvent.WaitOne(10, exitContext.IsCancellationRequested))
        //        {
        //            return EventResult.SUCCEED;
        //        }
        //        else
        //        {
        //            if (exitContext.IsCancellationRequested)
        //            {
        //                return EventResult.CANCEL;
        //            }
        //            else
        //            {
        //                if (sw.Elapsed.TotalMilliseconds >= millisecondsTimeout)
        //                {
        //                    sw.Stop();

        //                    return EventResult.TIMEOUT;
        //                }
        //            }
        //        }

        //    } while (true);
        //}
        public virtual EventResult WaitOne_BreakIfCancellationRequestedWithPauseFunc(int millisecondsTimeout, CancellationTokenSource exitContext )
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            do
            {
                if (exitContext.IsCancellationRequested)
                {
                    return EventResult.CANCEL;
                }
              
                if (_pauseFunc?.Invoke() == true)
                {
        
                    if (sw.IsRunning == true)
                    {
                        Debug.WriteLine("are_ is paused");
                        sw.Stop();
                    }
                    Thread.Sleep(2);
                }
                else if (_pauseFunc?.Invoke() == false)
                {
                    if (sw.IsRunning == false)
                    {
                        Debug.WriteLine("are_ is resumed");
                        sw.Start();
                    } 
                 
                    if (_AutoResetEvent.WaitOne(5, exitContext.IsCancellationRequested))
                    {
                        return EventResult.SUCCEED;
                    }

                    if (sw.Elapsed.TotalMilliseconds >= millisecondsTimeout)
                    {
                        sw.Stop();
                        return EventResult.TIMEOUT;
                    }
                }
            } while (true);
        }
        //public virtual bool WaitOne(int millisecondsTimeout)
        //{
        //    return _AutoResetEvent.WaitOne(millisecondsTimeout);
        //}
        //public virtual bool WaitOne()
        //{
        //    return _AutoResetEvent.WaitOne();
        //}
    }
    
}