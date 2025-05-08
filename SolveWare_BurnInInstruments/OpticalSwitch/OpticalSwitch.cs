using System;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace SolveWare_BurnInInstruments
{

    public class OpticalSwitch : InstrumentBase, IInstrumentBase
	{
		const int Delay_ms = 20;
		TcpClient DeviceClient;
		protected SerialPort DeviceSerialPort;
		public string DeviceIPAddress;
		public int DeviceNetPort;
		public string DeviceComPort;
		//public string InterfaceType;

		public bool DeviceReady = false;


		uint ErrorCode;
		const uint NO_ERROR = 0x0;
		const uint ERROR_INVALID_HEAD = 0x00000001;
		const uint ERROR_INVALID_LENGTH = 0x00000002;
		const uint ERROR_INVALID_SUM = 0x00000004;
		const uint ERROR_INVALID_CMD = 0x00000008;
		public OpticalSwitch(string name, string address, IInstrumentChassis chassis) : base(name, address, chassis)
		{
			//InterfaceType = "COM";

		}
	 
		public override void RefreshDataOnceCycle(CancellationToken token)
		{
			//throw new NotImplementedException();
		}

		public override void GenerateFakeDataOnceCycle(CancellationToken token)
		{
			//throw new NotImplementedException();
		}
		/// <summary>
		/// 读数据
		/// </summary>
		/// <param name="bytData">接收数据缓存区</param>
		/// <param name="iDataLen">读取的字节数</param>
		/// <param name="usOffset">数据缓冲区偏移量</param>
		private void ReadDataIn(byte[] bytData, int iDataLen, ushort usOffset = 0)
		{
			byte[] bytBuffer = new byte[iDataLen];
	 

			try
            {
 
				this._chassis.Read(bytBuffer, 0, iDataLen);
				Buffer.BlockCopy(bytBuffer, 0, bytData, usOffset, iDataLen);
			}
			catch (SocketException se)
			{
				throw new Exception("网络中断，" + se.Message);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// 写数据
		/// </summary>
		/// <param name="bytData">整个数据包缓存区</param>
		/// <param name="iDataLen">整个数据包长度</param>
		private void WriteDataOut(byte[] bytData, int iDataLen)
		{
			byte[] bytDataLen = new byte[2];
	 
			bytDataLen = BitConverter.GetBytes(iDataLen - 3);

			bytData[0] = 0xAA;//包头
			bytData[1] = bytDataLen[0];//包长度
			bytData[2] = bytDataLen[1];

			bytData[iDataLen - 1] = BytesSummary(bytData, iDataLen - 1);//校验和

			try
			{
  
					this._chassis.TryWrite(bytData );
			 
			}
			catch (SocketException se)
			{
				throw new Exception("网络中断，" + se.Message);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private byte BytesSummary(byte[] bytData, int iDataLen)
		{
			int iSum = bytData[0];
			byte[] bytSum;

			for (int i = 1; i < iDataLen; i++)
			{
				iSum = iSum + Convert.ToInt32(bytData[i]);
			}

			bytSum = BitConverter.GetBytes(iSum);

			return bytSum[0];
		}


        private byte[] BuildCommand(byte[] cmds)
        {
            int sum = 0;
            for (int i = 0; i < cmds.Length - 1; i++)
            {
                sum += cmds[i];
            }
            cmds[cmds.Length - 1] = (byte)sum;

            return cmds.ToArray();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytCmd">包命令字+附加命令字+设备信息</param>
        /// <param name="iCmdLen">命令字节长度(一般是8个字节+设备信息字节数)<</param>
        /// <returns></returns>
        private bool SetParameterWithCommand(byte[] bytCmd, int iCmdLen)
		{
			bool blnResult = false;

			byte[] bytOutBuffer = new byte[iCmdLen + 4];//整个发送数据包缓存区 4=包头1字节+包长度2字节+校验和1字节
			byte[] bytInBuffer;//整个接收数据包缓存区
			byte[] bytHead = new byte[3];//包头和包长度
			byte[] bytLen = new byte[2];
			int iLen;
			//bytOutBuffer = BuildCommand(bytOutBuffer);
			try
			{
				ErrorCode = NO_ERROR;
                //Thread.Sleep(10000);
				Buffer.BlockCopy(bytCmd, 0, bytOutBuffer, 3, iCmdLen);
                //Thread.Sleep(2000);
                WriteDataOut(bytOutBuffer, iCmdLen + 4);//4 封装包头AA+包长度+校验和
                Thread.Sleep(300);
                ReadDataIn(bytHead, 3);//接收数据包头(3个字节：包头+包长度)
				Buffer.BlockCopy(bytHead, 1, bytLen, 0, 2);//包长度

				if (bytHead[0] != 0xAA)
				{
					ErrorCode = ErrorCode | ERROR_INVALID_HEAD;
				}
				iLen = BitConverter.ToUInt16(bytLen, 0);
				if (iLen < 5)
				{
					ErrorCode = ErrorCode | ERROR_INVALID_LENGTH;
				}

				if (ErrorCode == NO_ERROR)
				{
					bytInBuffer = new byte[iLen + 3];
					Buffer.BlockCopy(bytHead, 0, bytInBuffer, 0, 3);//copy包头包长度字节到接收数据包缓冲区
					ReadDataIn(bytInBuffer, iLen, 3);

					blnResult = CheckResponseValid(bytInBuffer, iLen + 3, bytCmd, 4);
					if (blnResult == false)
					{
						throw new Exception("命令响应非法");
					}
				}
				return blnResult;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytCmd">包命令字+附加命令字</param>
		/// <param name="iCmdLen">命令字节长度(4个字节)</param>
		/// <param name="bytRspn">设备响应数据缓存区</param>
		/// <returns></returns>
		private bool GetBytesWithCommand(byte[] bytCmd, int iCmdLen, out byte[] bytRspn)
		{
			bool blnResult = false;

			byte[] bytOutBuffer = new byte[iCmdLen + 4];//整个发送数据包缓存区
			byte[] bytInBuffer;//整个接收数据包缓存区
			byte[] bytHead = new byte[3];//包头和包长度
			byte[] bytLen = new byte[2];
			int iLen;

			ErrorCode = NO_ERROR;

			try
			{
				Buffer.BlockCopy(bytCmd, 0, bytOutBuffer, 3, iCmdLen);
				WriteDataOut(bytOutBuffer, iCmdLen + 4);//发送命令数据包(封装包头,包长度和校验和)

				ReadDataIn(bytHead, 3);//接收数据包头(3个字节：包头+包长度)
				Buffer.BlockCopy(bytHead, 1, bytLen, 0, 2);//包长度

				if (bytHead[0] != 0xAA)
				{
					ErrorCode = ErrorCode | ERROR_INVALID_HEAD;
				}
				iLen = BitConverter.ToUInt16(bytLen, 0);
				if (iLen < 5)
				{
					ErrorCode = ErrorCode | ERROR_INVALID_LENGTH;
				}


				bytRspn = new byte[iLen - 1 - iCmdLen];//包长度 - 1字节(校验和) - 命令长度(命令字+附加命令字)

				if (ErrorCode == NO_ERROR)
				{
					bytInBuffer = new byte[iLen + 3];//iLen+3，iLen 包长度+3=数据包总字节长度
					Buffer.BlockCopy(bytHead, 0, bytInBuffer, 0, 3);//copy包头包长度字节到接收数据包缓冲区
					ReadDataIn(bytInBuffer, iLen, 3);

					blnResult = CheckResponseValid(bytInBuffer, iLen + 3, bytCmd, iCmdLen);
					if (blnResult == true)
					{
						//bytRspn = new byte[iLen - 1 - iCmdLen];
						Buffer.BlockCopy(bytInBuffer, 3 + iCmdLen, bytRspn, 0, iLen - 1 - iCmdLen);
					}
					else
					{
						throw new Exception("命令响应非法");
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return blnResult;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytData">整个数据包缓存区</param>
		/// <param name="iDataLen">整个数据包字节数长度</param>
		/// <param name="bytCmd">命令字缓存区</param>
		/// <param name="iCmdLen">命令字字节长度</param>
		/// <returns></returns>
		private bool CheckResponseValid(byte[] bytData, int iDataLen, byte[] bytCmd, int iCmdLen)
		{
			byte[] bytDataLen = new byte[2];
			int iLen;
			byte[] bytDummy = new byte[4];
			uint uiDummy;

			ErrorCode = NO_ERROR;

			Buffer.BlockCopy(bytData, 1, bytDataLen, 0, 2);//包长度
			iLen = BitConverter.ToInt16(bytDataLen, 0);

			Buffer.BlockCopy(bytData, 3, bytDummy, 0, 4);
			uiDummy = BitConverter.ToUInt32(bytDummy, 0);

			if (bytData[0] != 0xAA)
			{
				ErrorCode = ErrorCode | ERROR_INVALID_HEAD;
			}

			if (iDataLen - 3 != iLen)
			{
				ErrorCode = ErrorCode | ERROR_INVALID_LENGTH;
			}

			if (BytesSummary(bytData, iDataLen - 1) != bytData[iDataLen - 1])
			{
				ErrorCode = ErrorCode | ERROR_INVALID_SUM;
			}

			if (uiDummy == 0x97525245)//'ERRO'
			{
				Buffer.BlockCopy(bytData, 11, bytDummy, 0, 4);
				ErrorCode = ErrorCode | BitConverter.ToUInt32(bytDummy, 0);
			}
			else
			{
				for (int m = 0; m <= iCmdLen - 1; m++)
				{
					if (bytData[3 + m] != bytCmd[m])
					{
						ErrorCode = ErrorCode | ERROR_INVALID_CMD;
					}
				}
			}
			return (ErrorCode == NO_ERROR) ? true : false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytInfo">设备响应数据缓存区</param>
		/// <param name="iInfoLen">设备响应字节数长度</param>
		/// <returns></returns>
		public bool ReadDeviceIP(out byte[] bytInfo, int iInfoLen)
		{
			bool blnResult = false;

			byte[] bytCmd = new byte[4];
			byte[] bytRspnBuffer;

			bytInfo = new byte[iInfoLen];

			//if (DeviceReady == false)
			//{
			//	throw new Exception("设备尚未连接！");
			//}

			bytCmd[0] = 0x52;
			bytCmd[1] = 0x44;
			bytCmd[2] = 0x49;
			bytCmd[3] = 0x50;//RDIP
			try
			{
				blnResult = GetBytesWithCommand(bytCmd, 4, out bytRspnBuffer);

				if (blnResult == true)
				{
					Buffer.BlockCopy(bytRspnBuffer, 0, bytInfo, 0, iInfoLen);
				}
				else
				{
					throw new Exception("读设备信息失败");
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show("In ReadDeviceInfomation," + ex.Message);
				throw new Exception(ex.Message);
			}
			return blnResult;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytInfo"></param>
		/// <param name="iInfoLen"></param>
		/// <returns></returns>
		public bool WriteDeviceIP(byte[] bytInfo, int iInfoLen)
		{
			bool blnResult = false;
			byte[] bytCmd = new byte[4 + iInfoLen];


			bytCmd[0] = 0x57;
			bytCmd[1] = 0x52;
			bytCmd[2] = 0x49;
			bytCmd[3] = 0x50;//WRIP


			Buffer.BlockCopy(bytInfo, 0, bytCmd, 4, iInfoLen);
			try
			{
				blnResult = SetParameterWithCommand(bytCmd, 4 + iInfoLen);
				if (blnResult == false)
				{
					throw new Exception("发送写入IP地址命令失败");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("写入IP地址异常，" + ex.Message);
			}
			return blnResult;
		}


		public bool ReadCH(out byte bytCH)
		{
			bool blnResult = false;

			byte[] bytCmd = new byte[4];
			byte[] bytRspnBuffer;

			bytCmd[0] = 0x52;
			bytCmd[1] = 0x44;
			bytCmd[2] = 0x41;
			bytCmd[3] = 0x43;//RDAC
			try
			{
				blnResult = GetBytesWithCommand(bytCmd, 4, out bytRspnBuffer);

				if (blnResult == true)
				{
					//Buffer.BlockCopy(bytRspnBuffer, 0, bytInfo, 0, iInfoLen);
					bytCH = bytRspnBuffer[0];
				}
				else
				{
					throw new Exception("发送读通道命令失败");
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show("In ReadDeviceInfomation," + ex.Message);
				throw new Exception("读通道异常，" + ex.Message);
			}
			return blnResult;
		}

		public bool SetCH(byte bytCH)
		{
			if (!this._chassis.IsOnline) return false;
			bool blnResult = false;
			byte[] bytCmd = new byte[4+ 1];


            bytCmd[0] = 0x53;
            bytCmd[1] = 0x54;
            bytCmd[2] = 0x41;
            bytCmd[3] = 0x43;//STAC
			bytCmd[4] = bytCH;
			//         bytCmd[0] = 0xAA;
			//         bytCmd[1] = 0x06;
			//         bytCmd[2] = 0x00;
			//         bytCmd[3] = 0x53;//STAC
			//         bytCmd[4] = 0x54;
			//         bytCmd[5] = 0x41;
			//         bytCmd[6] = 0x43;//STAC


			//         //Buffer.BlockCopy(bytCH, 0, bytCmd, 4, 1);
			//         bytCmd[7] = bytCH;
			//bytCmd[8] = 0x00;
			//byte channel = 0x01;
			byte[] cmds = new byte[9] { 0xAA, 0x06, 0x00, 0x53, 0x54, 0x41, 0x43, bytCH, 0x00 };
			//bytCmd = BuildCommand(cmds);
			
			try
			{
				blnResult = SetParameterWithCommand(bytCmd, bytCmd.Length);
				if (blnResult == false)
				{
					throw new Exception("发送设置通道命令失败");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("设置通道异常，" + ex.Message);
			}

			Thread.Sleep(800); //等待一下光开关切换

			return blnResult;
		}


		public bool ReadDevicePort(out byte[] bytInfo, int iInfoLen)
		{
			//if (!this._chassis.IsOnline) return false;
			bool blnResult = false;

			byte[] bytCmd = new byte[4];
			byte[] bytRspnBuffer;

			bytInfo = new byte[iInfoLen];


			bytCmd[0] = 0x52;
			bytCmd[1] = 0x44;
			bytCmd[2] = 0x50;
			bytCmd[3] = 0x54;//RDPT
			try
			{
				blnResult = GetBytesWithCommand(bytCmd, 4, out bytRspnBuffer);

				if (blnResult == true)
				{
					Buffer.BlockCopy(bytRspnBuffer, 0, bytInfo, 0, iInfoLen);
				}
				else
				{
					throw new Exception("发送读端口号命令失败");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("读端口号异常，" + ex.Message);
			}
			return blnResult;
		}

		public bool WriteDevicePort(byte[] bytInfo, int iInfoLen)
		{
			if (!this._chassis.IsOnline) return false;
			bool blnResult = false;
			byte[] bytCmd = new byte[4 + iInfoLen];


			bytCmd[0] = 0x57;
			bytCmd[1] = 0x52;
			bytCmd[2] = 0x50;
			bytCmd[3] = 0x54;//WRPT


			Buffer.BlockCopy(bytInfo, 0, bytCmd, 4, iInfoLen);
			try
			{
				blnResult = SetParameterWithCommand(bytCmd, 4 + iInfoLen);
				if (blnResult == false)
				{
					throw new Exception("发送写入端口号命令失败");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("写入端口号异常，" + ex.Message);
			}
			return blnResult;
		}


		public bool ReadDeviceMAC(out byte[] bytInfo, int iInfoLen)
		{
			//if (!this._chassis.IsOnline) return false;
			bool blnResult = false;

			byte[] bytCmd = new byte[4];
			byte[] bytRspnBuffer;

			bytInfo = new byte[iInfoLen];


			bytCmd[0] = 0x52;
			bytCmd[1] = 0x44;
			bytCmd[2] = 0x4D;
			bytCmd[3] = 0x43;//RDMC
			try
			{
				blnResult = GetBytesWithCommand(bytCmd, 4, out bytRspnBuffer);

				if (blnResult == true)
				{
					Buffer.BlockCopy(bytRspnBuffer, 0, bytInfo, 0, iInfoLen);
				}
				else
				{
					throw new Exception("发送读MAC地址命令失败");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("读MAC地址异常，" + ex.Message);
			}
			return blnResult;
		}

		public bool WriteDeviceMAC(byte[] bytInfo, int iInfoLen)
		{
			if (!this._chassis.IsOnline) return false;
			bool blnResult = false;
			byte[] bytCmd = new byte[4 + iInfoLen];


			bytCmd[0] = 0x57;
			bytCmd[1] = 0x52;
			bytCmd[2] = 0x4D;
			bytCmd[3] = 0x43;//WRMC


			Buffer.BlockCopy(bytInfo, 0, bytCmd, 4, iInfoLen);
			try
			{
				blnResult = SetParameterWithCommand(bytCmd, 4 + iInfoLen);
				if (blnResult == false)
				{
					throw new Exception("发送写入MAC地址命令失败");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("写入MAC地址异常，" + ex.Message);
			}
			return blnResult;
		}
	}
}