using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace SolveWare_BurnInInstruments
{
    public class EthernetChassis : InstrumentChassisBase, IInstrumentChassis
    {


        NetworkStream myStream { get; set; }
        TcpClient myClient { get; set; }
        IPAddress IPAddress { get; set; }
        int Port { get; set; }
        const int MAX_RETRIES = 3;
        public EthernetChassis(string name, string resource, bool isOnline )
            : base(name, resource, isOnline )
        {
            try
            {
                string[] resourceInfo = resource.Split(':');
                this.IPAddress = IPAddress.Parse(resourceInfo[0]);
                this.Port = int.Parse(resourceInfo[1]);
               
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
                    if (myStream != null)
                    {
                        return myStream.ReadTimeout;
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
                    if (myStream != null)
                    {
                        myStream.ReadTimeout = value;
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
                this.myStream?.Close(5000);
                this.myClient?.Close();
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
                myClient = new TcpClient();
                myClient.Connect(IPAddress, Port);
                myStream = myClient.GetStream();
                myStream.ReadTimeout = timeout_ms;
                myStream.WriteTimeout = timeout_ms;
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
                myClient = new TcpClient();
                myClient.Connect(IPAddress, Port);
                myStream = myClient.GetStream();
                myStream.ReadTimeout = timeout;
                myStream.WriteTimeout = timeout;
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
        private void Write(string cmd)
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
        private void Write(byte[] cmd)
        {
            if (!this.IsOnline) return;
            do
            {
                try
                {
                    while (myStream.DataAvailable)
                    {
                        myStream.ReadByte();
                    }
                    myStream.Write(cmd, 0, cmd.Length);
                    myStream.Flush();
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
                        this.Write(cmd);
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

        private string ReadString()
        {
            if (!this.IsOnline) return string.Empty;
            byte[] data;
            string respon = string.Empty;
            Byte[] buffer = new Byte[DefaultBufferSize];

            myStream.ReadTimeout = 20;

            Stopwatch sw = new Stopwatch();
 
            using (MemoryStream memStream = new MemoryStream(DefaultBufferSize))
            {
                sw.Start();
                int numBytesRead = -1;
                do
                {
                    try
                    {
                        if (myStream.DataAvailable)
                        {
                            numBytesRead = myStream.Read(buffer, 0, buffer.Length);
                            memStream.Write(buffer, 0, numBytesRead);
                            if (numBytesRead >= DefaultBufferSize)
                            {
                                memStream.Capacity += DefaultBufferSize;
                            }
                            //break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    if (numBytesRead > 0 && myStream.DataAvailable == false)
                    {
                        break;
                    }

                    if (sw.Elapsed.TotalMilliseconds > 1000)  //1秒
                    {
                        break;
                    }
                    Thread.Sleep(10);
                } while (true);
                data = (memStream.Length > 0) ? memStream.ToArray() : null;
            }
            respon = (data != null) ? Encoding.GetEncoding(65001).GetString(data).TrimEnd('\0') : string.Empty;

            return respon;

        }
        private string ReadString(int responTime_ms)
        {
            if (!this.IsOnline) return string.Empty;
            byte[] data;
            string respon = string.Empty;
            Byte[] buffer = new Byte[DefaultBufferSize];

            myStream.ReadTimeout = 100;

            Stopwatch sw = new Stopwatch();

            using (MemoryStream memStream = new MemoryStream(DefaultBufferSize))
            {
                sw.Start();
                int numBytesRead = -1;
                do
                {
                    try
                    {
                        if (myStream.DataAvailable)
                        {
                            numBytesRead = myStream.Read(buffer, 0, buffer.Length);
                            memStream.Write(buffer, 0, numBytesRead);
                            if (numBytesRead >= DefaultBufferSize)
                            {
                                memStream.Capacity += DefaultBufferSize;
                            }
                            //break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    if (numBytesRead > 0 && 
                        myStream.DataAvailable == false)
                    {
                        break;
                    }

                    if (sw.Elapsed.TotalMilliseconds > responTime_ms)  //1秒
                    {
                        break;
                    }
                    Thread.Sleep(10);
                } while (true);
                data = (memStream.Length > 0) ? memStream.ToArray() : null;
            }
            respon = (data != null) ? Encoding.GetEncoding(65001).GetString(data).TrimEnd('\0') : string.Empty;

            return respon;

        }

        private byte[] ReadByteArray()
        {
            return ReadByteArray(DefaultBufferSize);
        }
        private byte[] ReadByteArrayWithLongResponTime(int responTime_ms)
        {
            return ReadByteArray(DefaultBufferSize, responTime_ms);
        }
        private byte[] ReadByteArray(int bytesToRead)
        {
            if (!this.IsOnline) return new byte[1];
            byte[] data;
            string respon = string.Empty;
            byte[] buffer = new byte[bytesToRead];

            myStream.ReadTimeout = 100;

            Stopwatch sw = new Stopwatch();

            using (MemoryStream memStream = new MemoryStream(DefaultBufferSize))
            {
                sw.Start();
                int numBytesRead = -1;
                do
                {
                    try
                    {
                        if (myStream.DataAvailable)
                        {
                            numBytesRead = myStream.Read(buffer, 0, buffer.Length);
                            memStream.Write(buffer, 0, numBytesRead);
                            if (numBytesRead >= DefaultBufferSize)
                            {
                                memStream.Capacity += DefaultBufferSize;
                            }
                            //break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    if (numBytesRead > 0 && myStream.DataAvailable == false)
                    {
                        break;
                    }

                    if (sw.Elapsed.TotalMilliseconds > 1000)  //1秒
                    {
                        break;
                    }
                    Thread.Sleep(10);
                } while (true);
                data = (memStream.Length > 0) ? memStream.ToArray() : null;
            }
            return data;
        }
        private byte[] ReadByteArray(int bytesToRead, int responTime_ms)
        {
            int InvTime_ms = 10;

            if (!this.IsOnline) return new byte[1];
            byte[] data;
            string respon = string.Empty;
            byte[] buffer = new byte[bytesToRead];

            myStream.ReadTimeout = 100;

            Stopwatch sw = new Stopwatch();

            using (MemoryStream memStream = new MemoryStream(DefaultBufferSize))
            {
                sw.Start();
                int numBytesRead = -1;
                do
                {
                    try
                    {
                        if (myStream.DataAvailable)
                        {
                            numBytesRead = myStream.Read(buffer, 0, buffer.Length);
                            memStream.Write(buffer, 0, numBytesRead);
                            if (numBytesRead >= DefaultBufferSize)
                            {
                                memStream.Capacity += DefaultBufferSize;
                            }
                            //break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    if (numBytesRead > 0 && myStream.DataAvailable == false)
                    {
                        break;
                    }

                    if (sw.Elapsed.TotalMilliseconds > responTime_ms)  //1秒
                    {
                        break;
                    }
                    Thread.Sleep(InvTime_ms);
                } while (true);
                data = (memStream.Length > 0) ? memStream.ToArray() : null;
            }
            return data;
        }

        private byte[] ReadByteArray(int bytesToRead, int InvTime_ms, int responTime_ms)
        {
            if (!this.IsOnline) return new byte[1];

            byte[] data;
            List<byte> readBuffer = new List<byte>();
            //MemoryStream memStream = new MemoryStream(DefaultBufferSize);

            myStream.ReadTimeout = 100;

            bool DataReceiving = false;
            Stopwatch sw = new Stopwatch();

            Byte[] Buffer = new Byte[DefaultBufferSize];
            using (MemoryStream memStream = new MemoryStream(DefaultBufferSize))
            {
                try
                {
                    sw.Start();
                    while (true)
                    {
                        int numBytesRead = -1;
                        try
                        {
                            while ((numBytesRead = myStream.Read(Buffer, 0, Buffer.Length)) > 0)
                            {
                                byte[] _buffer = new byte[numBytesRead];
                                Array.Copy(Buffer, _buffer, numBytesRead);
                                //memStream.Write(_buffer, 0, numBytesRead);

                                readBuffer.AddRange(_buffer);
                                DataReceiving = true;
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                        if (numBytesRead <= 0 && DataReceiving == true)
                        {
                            break;
                        }

                        if (sw.Elapsed.TotalMilliseconds > responTime_ms)  //1秒
                        {
                            //break;
                        }

                        Thread.Sleep(InvTime_ms);
                    }
                    //myStream.Read(Buffer, 0, Buffer.Length);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            data = (readBuffer.Count > 0) ? readBuffer.ToArray() : null;
            return data;







































            //byte[] data;
            //string respon = string.Empty;
            //byte[] buffer = new byte[bytesToRead];
            //List<Byte> lstbuffer = new List<byte>();


            //myStream.ReadTimeout = 100;

            //Stopwatch sw = new Stopwatch();

            //using (MemoryStream memStream = new MemoryStream(DefaultBufferSize))
            //{
            //    sw.Start();
            //    int numBytesRead = -1;
            //    do
            //    {
            //        try
            //        {
            //            if (myStream.DataAvailable)
            //            {
            //                numBytesRead = myStream.Read(buffer, 0, buffer.Length);
                            
            //                memStream.Write(buffer, 0, numBytesRead);
            //                lstbuffer.Add(buffer)
            //                if (numBytesRead >= DefaultBufferSize)
            //                {
            //                    memStream.Capacity += DefaultBufferSize;
            //                }
            //                //break;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            throw ex;
            //        }

            //        if (numBytesRead > 0 && myStream.DataAvailable == false)
            //        {
            //            break;
            //        }

            //        if (sw.Elapsed.TotalMilliseconds > responTime_ms)  //1秒
            //        {
            //            break;
            //        }
            //        Thread.Sleep(InvTime_ms);
            //    } while (true);
            //    data = (memStream.Length > 0) ? memStream.ToArray() : null;
            //}
            //return data;
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
                        this.Write(cmd);
                        Thread.Sleep(delay_ms);
                        resp = this.ReadString();
                        if (string.IsNullOrEmpty(resp) )
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
                        this.Write(cmd);
                        Thread.Sleep(delay_ms);
                        resp = this.ReadByteArray();
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
                        this.Write(cmd);
                        Thread.Sleep(delay_ms);
                        resp = this.ReadByteArray(bytesToRead);
                        if(resp == null)
                        {
                            throw new InvalidDataException("空数据返回错误!");
                        }
                        break;
                    }
                    catch ( InvalidDataException ide)
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
                        this.Write(cmd);
                        Thread.Sleep(delay_ms);
                        resp = this.ReadByteArray(bytesToRead, respon_ms);
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
                        this.Write(cmd);
                        Thread.Sleep(delay_ms);
                        resp = this.ReadByteArrayWithLongResponTime(respon_ms);
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
                        this.Write(cmd);
                        Thread.Sleep(delay_ms);
                        resp = this.ReadString(respon_ms);
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
        public override void Read(byte[] buffer, int offset, int bytesToRead)
        {
            if (!this.IsOnline) return;

            lock (this)
            {
                int retry = 0;
                do
                {
                    try
                    {
                        this.myStream.Read(buffer, 0, buffer.Length);

                        break;
                    }
                    catch (Exception ex)
                    {
                        if (retry == 0)
                        {
                            string msg = string.Format("[{0}] Read(byte[] buffer,  int bytesToRead ) error. Chassis resource = [{1}].[{2}]-[{3}]",
                            this.Name, this.Resource, ex.Message, ex.StackTrace);
                            this.ReportException(msg);
                        }
                        else if (retry >= MAX_RETRIES)
                        {
                            string msg = string.Format("[{0}] Read(byte[] buffer,  int bytesToRead ) error after [{1}] retries. Chassis resource = [{2}].[{3}]-[{4}]",
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


        public virtual string ReadStringLoop(int count, int sleepBetweenLoops_ms = 5*60*1000)
        {

            int InvTime_ms = 2000;

            if (!this.IsOnline) return "";

            byte[] data;
            List<byte> readBuffer = new List<byte>();
            //MemoryStream memStream = new MemoryStream(DefaultBufferSize);

            myStream.ReadTimeout = 500;

            bool DataReceiving = false;
            Stopwatch sw = new Stopwatch();

            Byte[] Buffer = new Byte[DefaultBufferSize];
            using (MemoryStream memStream = new MemoryStream(DefaultBufferSize))
            {
                try
                {
                    sw.Start();
                    while (true)
                    {
                        int numBytesRead = -1;
                        try
                        {
                            while ((numBytesRead = myStream.Read(Buffer, 0, Buffer.Length)) > 0)
                            {
                                byte[] _buffer = new byte[numBytesRead];
                                Array.Copy(Buffer, _buffer, numBytesRead);
                                //memStream.Write(_buffer, 0, numBytesRead);

                                readBuffer.AddRange(_buffer);
                                DataReceiving = true;
                                Thread.Sleep(5);
                            }
                        }
                        catch (Exception ex)
                        {
                        }


                        var c = readBuffer.Count(t => t == ';');
                        if (c >= count)
                        {
                            break;
                        }
                        if (numBytesRead <= 0 && DataReceiving == true)
                        {
                            break;
                        }

                        if (sw.Elapsed.TotalMilliseconds > sleepBetweenLoops_ms)  //超时
                        {
                            break;
                        }

                        Thread.Sleep(InvTime_ms);
                    }
                    //myStream.Read(Buffer, 0, Buffer.Length);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            if(readBuffer.Count > 0)
            {
                data = readBuffer.ToArray();

                var str = Encoding.ASCII.GetString(data, 0, data.Length);

                return str;
            }
            else
            {
                return "";// "1000,-99";
            }

            //ReadByteArray();



            ////string response = "";
            //byte[] responseData = new byte[28000];
            ////bool stopRequested = false;
            ////int count = 0;
            ////List<string> temp = new List<string>();
            //string tempRespon = string.Empty;
            //this.myStream.ReadTimeout = 10;
            //do
            //{
            //    try
            //    {
            //        int bytesRead = this.myStream.Read(responseData,0, responseData.Length);
            //        tempRespon += Encoding.ASCII.GetString(responseData, 0, bytesRead);
            //        //Console.WriteLine("Response from server: " + response);
            //        //response = response.Trim(';');
            //        //tempRespon += response.Trim(';');

            //        //var resut = response.Split(';');
            //        //var resutlist = resut.ToList();
            //        //resutlist.RemoveAt(resutlist.Count - 1);
            //        //count += resutlist.Count;
            //        //temp.AddRange(resutlist);
            //        //Console.WriteLine("count: " + count);
            //        Thread.Sleep(sleepBetweenLoops_ms);
            //    }
            //    catch (Exception ex)
            //    {
            //        break;
            //    }



            //} while (true);
            ////return response;
            //return tempRespon;



            ////string response = "";
            //byte[] responseData = new byte[28000];
            ////bool stopRequested = false;
            ////int count = 0;
            ////List<string> temp = new List<string>();
            //string tempRespon = string.Empty;
            //this.myStream.ReadTimeout = 10;
            //do
            //{
            //    try
            //    {

            //        int bytesRead = this.myStream.Read(responseData, 0, responseData.Length);
            //        tempRespon += Encoding.ASCII.GetString(responseData, 0, bytesRead);
            //        //Console.WriteLine("Response from server: " + response);
            //        //response = response.Trim(';');
            //        //tempRespon += response.Trim(';');

            //        //var resut = response.Split(';');
            //        //var resutlist = resut.ToList();
            //        //resutlist.RemoveAt(resutlist.Count - 1);
            //        //count += resutlist.Count;
            //        //temp.AddRange(resutlist);
            //        //Console.WriteLine("count: " + count);
            //        Thread.Sleep(sleepBetweenLoops_ms);
            //    }
            //    catch (Exception ex)
            //    {
            //        break;
            //    }
            //} while (true);
            ////return response;
            //return tempRespon;

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
    }
}