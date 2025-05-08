using NationalInstruments.VisaNS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SolveWare_BurnInInstruments
{
    public class NiVisaChassis : InstrumentChassisBase, IInstrumentChassis
    {
        public const int _DefaultBufferSize = 1024;

        public MessageBasedSession mbSession { get; set; }

        public const int MAX_RETRIES = 3;

  

        public NiVisaChassis(string name, string resource, bool isOnline)
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

        public override int Timeout_ms
        {
            get
            {
                if (!this.IsOnline) { return 0; }
                try
                {
                    if (mbSession != null)
                    {
                        return mbSession.Timeout;
                    }
                }
                catch
                {
                }
                return 0;
            }
            set
            {
                if (!this.IsOnline) { return; }
                try
                {
                    if (mbSession != null)
                    {
                        mbSession.Timeout = value;
                    }
                }
                catch
                {
                }
            }
        }

        public new int DefaultBufferSize
        {
            get
            {
                if (!this.IsOnline) { return _DefaultBufferSize; }
                try
                {
                    if (mbSession != null)
                    {
                        return mbSession.DefaultBufferSize;
                    }
                }
                catch
                {
                }
                return 0;
            }
            set
            {
                if (!this.IsOnline) { return; }
                try
                {
                    if (mbSession != null)
                    {
                        mbSession.DefaultBufferSize = value;
                    }
                }
                catch
                {
                }
            }
        }

        public override void ClearConnection()
        {
            if (!this.IsOnline) { return; }
            try
            {
                this.mbSession?.Clear();
                this.mbSession?.Dispose();
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Clear connection fails. Chassis resource = [{1}].[{2}]-[{3}]",
                                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
            }
        }

        public override void BuildConnection(int timeout_ms)
        {
            if (!IsOnline) return;
            try
            {
                this.mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(this.Resource, AccessModes.NoLock, timeout_ms);
                this.mbSession.DefaultBufferSize = DefaultBufferSize;
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] BuildConnection error. Chassis resource = [{1}].[{2}]-[{3}]",
                                           this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
            }
        }

        public override void Initialize()
        {
            Initialize(5000);
        }

        public override void Initialize(int timeout)
        {
            if (!IsOnline) return;
            try
            {
                this.mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(this.Resource, AccessModes.NoLock, timeout);
            }
            catch (Exception ex)
            {
                string msg = string.Format("[{0}] Initialize error. Chassis resource = [{1}].[{2}]-[{3}]",
                                           this.Name, this.Resource, ex.Message, ex.StackTrace);
                this.ReportException(msg);
                throw new Exception(msg);
            }
        }

        public override void WaitRuning()
        {
        }

        /// <summary>
        /// write string function no retry
        /// </summary>
        /// <param name="data"></param>
        public void Write(string cmd)
        {
            if (!this.IsOnline) return;
            do
            {
                try
                {
                    Byte[] writeData = Encoding.ASCII.GetBytes(cmd);
                    this.Write(writeData);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            } while (false);
        }

        /// <summary>
        /// write bytes function no retry
        /// </summary>
        /// <param name="data"></param>
        public void Write(byte[] cmd)
        {
            if (!this.IsOnline) return;
            do
            {
                try
                {
                    this.mbSession.Write(cmd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            } while (false);
        }

        public override void TryWrite(byte[] cmd)
        {
            if (!this.IsOnline) return;

            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        this.Write(cmd); Thread.Sleep(5);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] TryWrite(byte[] cmd), int bytesToRead) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] TryWrite(byte[] cmd) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);
            }
        }

        public override void TryWrite(string cmd)
        {
            if (!this.IsOnline) return;

            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        this.Write(cmd);
                        Thread.Sleep(5);
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] TryWrite(string cmd), int bytesToRead) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] TryWrite(string cmd) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);
            }
        }

        public string ReadString()
        {
            if (!this.IsOnline) return string.Empty;

            string respon = string.Empty;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.mbSession.Timeout = 100;
            do
            {
                try
                {
                    respon = this.mbSession.ReadString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (sw.Elapsed.TotalMilliseconds > 1000)  //1秒
                {
                    break;
                }
                Thread.Sleep(10);
            } while (false);
            return respon;
        }

        public string ReadString(int responTime_ms)
        {
            if (!this.IsOnline) return string.Empty;

            string respon = string.Empty;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.mbSession.Timeout = 100;
            do
            {
                try
                {
                    respon = this.mbSession.ReadString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (sw.Elapsed.TotalMilliseconds > responTime_ms)  //1秒
                {
                    break;
                }
                Thread.Sleep(10);
            } while (false);
            return respon;
        }

        public byte[] ReadByteArray()
        {
            return ReadByteArray(DefaultBufferSize);
        }

        public byte[] ReadByteArrayWithLongResponTime(int responTime_ms)
        {
            return ReadByteArray(DefaultBufferSize, responTime_ms);
        }

        public byte[] ReadByteArray(int bytesToRead)
        {
            if (!this.IsOnline) return new byte[0];

            List<byte> respon = new List<byte>();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.mbSession.Timeout = 100;
            do
            {
                try
                {
                    var temp = this.mbSession.ReadByteArray(bytesToRead);
                    if (temp.Count() > 0 && temp.Count() <= bytesToRead)
                    {
                        respon.AddRange(temp);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (sw.Elapsed.TotalMilliseconds > 1000)  //1秒
                {
                    break;
                }
                Thread.Sleep(10);
            } while (false);
            return respon.ToArray();
        }

        public byte[] ReadByteArray(int bytesToRead, int responTime_ms)
        {
            if (!this.IsOnline) return new byte[0];

            List<byte> respon = new List<byte>();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            this.mbSession.Timeout = 100;
            do
            {
                try
                {
                    var temp = this.mbSession.ReadByteArray(bytesToRead);
                    if (temp.Count() > 0 && temp.Count() <= bytesToRead)
                    {
                        respon.AddRange(temp);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (sw.Elapsed.TotalMilliseconds > responTime_ms)  //1秒
                {
                    break;
                }
                Thread.Sleep(10);
            } while (true);
            return respon.ToArray();
        }

        public override string Query(string cmd, int delay_ms)
        {
            if (!this.IsOnline) return string.Empty;
            var resp = "";
            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        //this.Write(cmd);
                        //Thread.Sleep(delay_ms);
                        //resp = this.ReadString();

                        this.mbSession.Timeout = delay_ms;
                        resp = this.mbSession.Query(cmd);

                        if (string.IsNullOrEmpty(resp))
                        {
                            throw new InvalidDataException("空数据返回错误!");
                        }
                        break;
                    }
                    catch (InvalidDataException ide)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Query(string cmd, int delay_ms) <NULL DATA> error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ide.Message, ide.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Query(string cmd, int delay_ms) <NULL DATA> error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ide.Message,
                            ide.StackTrace);
                            this.ReportException(msg);
                            throw ide;
                        }
                        Thread.Sleep(1000);
                        retry++;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Query(string cmd, int delay_ms) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Query(string cmd, int delay_ms) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);

                return resp;
            }
        }

        public override byte[] Query(byte[] cmd, int delay_ms)
        {
            if (!this.IsOnline) return new byte[1];
            byte[] resp = new byte[1];
            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        //this.Write(cmd);
                        //Thread.Sleep(delay_ms);
                        //resp = this.ReadByteArray();

                        this.mbSession.Timeout = delay_ms;

                        resp = this.mbSession.Query(cmd);
                        if (resp == null)
                        {
                            throw new InvalidDataException("空数据返回错误!");
                        }
                        break;
                    }
                    catch (InvalidDataException ide)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd,int delay_ms) <NULL DATA> error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ide.Message, ide.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd,int delay_ms) <NULL DATA> error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ide.Message,
                            ide.StackTrace);
                            this.ReportException(msg);
                            throw ide;
                        }
                        Thread.Sleep(1000);
                        retry++;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int delay_ms) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int delay_ms) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);
            }
            return resp;
        }

        public override byte[] Query(byte[] cmd, int bytesToRead, int delay_ms)
        {
            if (!this.IsOnline) return new byte[1];
            byte[] resp = new byte[1];
            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        //this.Write(cmd);
                        //Thread.Sleep(delay_ms);
                        //resp = this.ReadByteArray(bytesToRead);

                        this.mbSession.Timeout = delay_ms;

                        resp = this.mbSession.Query(cmd, bytesToRead);

                        if (resp == null)
                        {
                            throw new InvalidDataException("空数据返回错误!");
                        }
                        break;
                    }
                    catch (InvalidDataException ide)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int bytesToRead, int delay_ms) <NULL DATA> error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ide.Message, ide.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int bytesToRead, int delay_ms) <NULL DATA> error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ide.Message,
                            ide.StackTrace);
                            this.ReportException(msg);
                            throw ide;
                        }
                        Thread.Sleep(1000);
                        retry++;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int bytesToRead, int delay_ms) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int bytesToRead, int delay_ms) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);
            }
            return resp;
        }

        public override byte[] QueryWithLongResponTime(byte[] cmd, int bytesToRead, int delay_ms, int respon_ms)
        {
            if (!this.IsOnline) return new byte[1];
            byte[] resp = new byte[1];
            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        //this.Write(cmd);
                        //Thread.Sleep(delay_ms);
                        //resp = this.ReadByteArray(bytesToRead, respon_ms);

                        this.mbSession.Timeout = delay_ms + respon_ms;

                        resp = this.mbSession.Query(cmd, bytesToRead);

                        if (resp == null)
                        {
                            throw new InvalidDataException("空数据返回错误!");
                        }
                        break;
                    }
                    catch (InvalidDataException ide)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] QueryWithLongResponTime(byte[] cmd, int bytesToRead, int delay_ms, int respon_ms) <NULL DATA> error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ide.Message, ide.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] QueryWithLongResponTime(byte[] cmd, int bytesToRead, int delay_ms, int respon_ms) <NULL DATA> error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ide.Message,
                            ide.StackTrace);
                            this.ReportException(msg);
                            throw ide;
                        }
                        Thread.Sleep(1000);
                        retry++;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int bytesToRead, int delay_ms) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int bytesToRead, int delay_ms) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);
            }
            return resp;
        }

        public override byte[] QueryWithLongResponTime(byte[] cmd, int delay_ms, int respon_ms)
        {
            if (!this.IsOnline) return new byte[1];
            byte[] resp = new byte[1];
            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        //this.Write(cmd);
                        //Thread.Sleep(delay_ms);
                        //resp = this.ReadByteArrayWithLongResponTime(respon_ms);

                        this.mbSession.Timeout = delay_ms + respon_ms;

                        resp = this.mbSession.Query(cmd);

                        if (resp == null)
                        {
                            throw new InvalidDataException("空数据返回错误!");
                        }
                        break;
                    }
                    catch (InvalidDataException ide)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] QueryWithLongResponTime(byte[] cmd, int delay_ms, int respon_ms) <NULL DATA> error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ide.Message, ide.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] QueryWithLongResponTime(byte[] cmd, int bytesToRead, int delay_ms, int respon_ms) <NULL DATA> error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ide.Message,
                            ide.StackTrace);
                            this.ReportException(msg);
                            throw ide;
                        }
                        Thread.Sleep(1000);
                        retry++;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int delay_ms) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Query(byte[] cmd, int delay_ms) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);
            }
            return resp;
        }

        public override string QueryWithLongResponTime(string cmd, int delay_ms, int respon_ms)
        {
            if (!this.IsOnline) return string.Empty;
            var resp = "";
            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        //this.Write(cmd);
                        //Thread.Sleep(delay_ms);
                        //resp = this.ReadString(respon_ms);

                        this.mbSession.Timeout = delay_ms + respon_ms;

                        resp = this.mbSession.Query(cmd);

                        if (string.IsNullOrEmpty(resp))
                        {
                            throw new InvalidDataException("空数据返回错误!");
                        }
                        break;
                    }
                    catch (InvalidDataException ide)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] QueryWithLongResponTime(string cmd, int delay_ms, int respon_ms) <NULL DATA> error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ide.Message, ide.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] QueryWithLongResponTime(string cmd, int delay_ms, int respon_ms) <NULL DATA> error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ide.Message,
                            ide.StackTrace);
                            this.ReportException(msg);
                            throw ide;
                        }
                        Thread.Sleep(1000);
                        retry++;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Query(string cmd, int delay_ms) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Query(string cmd, int delay_ms) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
                            this.Name,
                            retry,
                            this.Resource,
                            ex.Message,
                            ex.StackTrace);
                            this.ReportException(msg);
                            throw ex;
                        }
                        this.ClearConnection();
                        Thread.Sleep(500);
                        this.BuildConnection(5000);
                        retry++;
                    }
                } while (true);

                return resp;
            }
        }

        protected override void OnTurnOnline()
        {
            lock (this)
            {
                this.IsOnline = true;
                this.BuildConnection(5000);
                Thread.Sleep(5000);
            }
            base.OnTurnOnline();
        }

        public void debug(string cmd)
        {
            var en = this.mbSession.TerminationCharacterEnabled;
            var ret = this.mbSession.Query(cmd);
        }
    }
}