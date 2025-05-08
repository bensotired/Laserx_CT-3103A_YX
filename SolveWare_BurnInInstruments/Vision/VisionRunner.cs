using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace SolveWare_BurnInInstruments
{
    public class VisionJsonCmdRunner : InstrumentBase,  IInstrumentBase 
    {
        const int Delay_ms = 20;
        public VisionJsonCmdRunner(string name, string address, IInstrumentChassis chassis)
            : base(name, address, chassis)
        {
         

        }
        //方式一
        public VisionResult_OCR OCR_1(  string projectName)
        {
                VisionJsonCmdSender cw = new VisionJsonCmdSender("CMDTest", projectName);
                var val = this._chassis.Query(cw.GetCommand(), Delay_ms);
                var temp = VisionJsonDataConverter.ConvertJsonDataTo<VisionResult_OCR>(val);
                return temp;
        }
        //方式二
        public TResult GetVisionResult<TResult>(string cmd,string projectName) where TResult : VisionJsonCmdBase
        {
            VisionJsonCmdSender cw = new VisionJsonCmdSender(cmd,   projectName);
            var val = this._chassis.Query(cw.GetCommand(), Delay_ms);
            return VisionJsonDataConverter.ConvertJsonDataTo<TResult>(val);
        }

        public override void Initialize()
        {
            if (this._isOnline)
            {
                //if (this._isSimulation)
                //{

                //}
                //else
                //{
                    
                //}

            }
            base.Initialize();
        }
 

        public override void RefreshDataOnceCycle(CancellationToken token)
        {
           // throw new NotImplementedException();
        }

        public override void GenerateFakeDataOnceCycle(CancellationToken token)
        {
          //  throw new NotImplementedException();
        }
    }
 
 

   //public static class JsonBusiness
   // {
  
   //     public static string Put(string data, string uri)
   //     {
   //         try
   //         {
   //             //创建Web访问对象
   //             HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(uri);
   //             //把用户传过来的数据转成“UTF-8”的字节流
   //             byte[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(data);

   //             myRequest.Method = "PUT";
   //             myRequest.ContentLength = buf.Length;
   //             myRequest.ContentType = "application/json";
   //             myRequest.MaximumAutomaticRedirections = 1;
   //             myRequest.AllowAutoRedirect = true;
   //             //发送请求
   //             Stream stream = myRequest.GetRequestStream();
   //             stream.Write(buf, 0, buf.Length);
   //             stream.Close();

   //             //获取接口返回值
   //             //通过Web访问对象获取响应内容
   //             HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
   //             //通过响应内容流创建StreamReader对象，因为StreamReader更高级更快
   //             StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
   //             //string returnXml = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
   //             string returnXml = reader.ReadToEnd();//利用StreamReader就可以从响应内容从头读到尾
   //             reader.Close();
   //             myResponse.Close();
   //             return returnXml;
   //         }
   //         catch (System.Exception ex)
   //         {
   //             return ex.Message;
   //         }

   //     }
   // }
}
