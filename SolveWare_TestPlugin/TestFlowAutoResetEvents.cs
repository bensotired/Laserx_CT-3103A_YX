using SolveWare_BurnInCommon;
using SolveWare_TestComponents.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SolveWare_TestPlugin
{
    public enum EventResult
    {
        SUCCEED,
        CANCEL,
        TIMEOUT
    }
    public class AutoResetEventItem : CURDItemLite
    {
        public AutoResetEventItem()
        {
            _AutoResetEvent = new AutoResetEvent(false);
        }
        AutoResetEvent _AutoResetEvent { get; }
        public virtual bool Set()
        {
            return _AutoResetEvent.Set();
        }
        public virtual bool Reset()
        {
            return _AutoResetEvent.Reset(); 
        }
        public virtual EventResult WaitOne(int millisecondsTimeout,  bool exitContext)
        {
            if (_AutoResetEvent.WaitOne(millisecondsTimeout, exitContext))
            {
                return EventResult.SUCCEED;
            }
            else
            {
                if (exitContext)
                {
                    return EventResult.CANCEL;
                }
                else
                {
                    return EventResult.TIMEOUT;
                }
            }
        }
        public virtual EventResult WaitOne(int millisecondsTimeout,  CancellationTokenSource exitContext)
        {
            if (_AutoResetEvent.WaitOne(millisecondsTimeout, exitContext.IsCancellationRequested))
            {
                return EventResult.SUCCEED;
            }
            else
            {
                if (exitContext.IsCancellationRequested)
                {
                    return EventResult.CANCEL;
                }
                else
                {
                    return EventResult.TIMEOUT;
                }
            }
        }
        public virtual EventResult WaitOne_BreakIfCancellationRequested(int millisecondsTimeout, CancellationTokenSource exitContext)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            do
            {
                //genggai10
                if (_AutoResetEvent.WaitOne(10, exitContext.IsCancellationRequested))
                {
                    return EventResult.SUCCEED;
                }
                else
                {
                    if (exitContext.IsCancellationRequested)
                    {
                        return EventResult.CANCEL;
                    }
                    else
                    {
                        if (sw.Elapsed.TotalMilliseconds >= millisecondsTimeout)
                        {
                            sw.Stop();

                            return EventResult.TIMEOUT;
                        }
                    }
                }

            } while (true);
        }

        public virtual EventResult WaitOne_BreakIfCancellationRequestedWithPauseFunc
                                    (
                                        int millisecondsTimeout, 
                                        CancellationTokenSource exitContext, 
                                        Func<bool> pauseFunc
                                    )
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            do
            {
                if (exitContext.IsCancellationRequested)
                {
                    return EventResult.CANCEL;
                }
                //如果是暂停状态  则停止倒计时
                if (pauseFunc.Invoke() == true)
                {
                    if (sw.IsRunning == true)
                    {
                        sw.Stop();
                    }
                    Thread.Sleep(5);
                }
                //如果未有暂停  则正常执行
                else if (pauseFunc.Invoke() == false)
                {
                    if (sw.IsRunning == false)
                    {
                        sw.Start();
                    }

                    if (_AutoResetEvent.WaitOne(10, exitContext.IsCancellationRequested))
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

        public virtual bool WaitOne(int millisecondsTimeout)
        {
            return _AutoResetEvent.WaitOne(millisecondsTimeout);
        }
        public virtual bool WaitOne()
        {
            return _AutoResetEvent.WaitOne();
        }
    }
    public abstract  class TestFlowAutoResetEvents : CURDBaseLite<AutoResetEventItem>
    {
        public TestFlowAutoResetEvents()
        {
            this.ItemCollection = new List<AutoResetEventItem>();
        }
        public abstract void Initialize();
        public virtual void Clear()
        {
            this.ItemCollection.Clear();
        }
        public  AutoResetEventItem this[string name]
        {
            get
            {
                return this.GetSingleItem(name);
            }
        }

    }
}