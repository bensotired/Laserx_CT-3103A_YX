using LX_BurnInSolution.Utilities;
using System;
using System.Collections.Generic;

namespace SolveWare_BurnInInstruments
{

    public enum ModbusStatus
    {
        WriteSuccessful,
        WriteError,
        CRCCheckFailed,
        PortNotOpened,
        Idle,
    }
    public class Modbus
    {
        public const byte MB_READ_COILS = 0x01;             //读线圈寄存器
        public const byte MB_READ_DISCRETE = 0x02;          //读离散输入寄存器
        public const byte MB_READ_HOLD_REG = 0x03;          //读保持寄存器
        public const byte MB_READ_INPUT_REG = 0x04;         //读输入寄存器

        public const byte MB_WRITE_SINGLE_COIL = 0x05;      //写单个线圈
        public const byte MB_WRITE_SINGLE_REG = 0x06;       //写单寄存器
        public const byte MB_WRITE_MULTIPLE_COILS = 0x0f;   //写多线圈
        public const byte MB_WRITE_MULTIPLE_REGS = 0x10;    //写多寄存器

        private int DELAY_MS = 500;

        IInstrumentChassis _chassis;
        ModbusStatus _status = ModbusStatus.Idle;

        public Modbus(IInstrumentChassis communicationChassis)
        {
            this._chassis = communicationChassis;
        }
        bool[] ByteToBit(byte byt)
        {

            bool[] bits = new bool[8];

            for (int j = 0; j < 8; j++)
            {
                bits[j] = Convert.ToBoolean((byt >> j) & 0x01);
            }

            return bits;
        }

        public void SetDelay_ms(int delms)
        {
            if (delms < 50)
                delms = 50;
            DELAY_MS = delms;
        }

        public ModbusStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }
        public string ChassisResource
        {
            get
            {
                return this._chassis.Resource;
            }
        }
        /// <summary>
        /// Write single coil value
        /// value in dec
        /// </summary>
        /// <param name="nodeAddress"></param>
        /// <param name="registerAddress"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Function_1(int nodeAddress, int coilAddress, int coilCount, ref bool[] vals)
        {
            byte[] header = BuildMessageHeader(nodeAddress, MB_READ_COILS, coilAddress, coilCount);
            byte crcHi, crcLo;
            CRCCalculation.CRC16(header, header.Length, out crcHi, out crcLo);
            byte[] message = new byte[header.Length + 2];
            header.CopyTo(message, 0);
            message[message.Length - 1] = crcHi;
            message[message.Length - 2] = crcLo;
            //byte[] response = new byte[8];
            byte[] response = new byte[5 + (int)Math.Ceiling(coilCount / 8.0)];
            try
            {
                response = _chassis.Query(message, response.Length, DELAY_MS);
            }
            catch
            {
                _status = ModbusStatus.WriteError;
                return false;
            }
            if (CheckResponse(response, nodeAddress, MB_READ_COILS))
            {
                int dataLength = (int)response[2];
                List<byte> data = new List<byte>();
                List<bool> temp = new List<bool>();
                for (int i = 0; i < dataLength; i++)
                {
                    data.Add(response[3 + i]);
                }
                for (int i = 0; i < data.Count; i++)
                {
                    temp.AddRange( ByteToBit(data[i]));
                }
                Array.Copy(temp.ToArray(), vals, vals.Length);

                _status = ModbusStatus.WriteSuccessful;

                return true;
            }
            else
            {
                _status = ModbusStatus.CRCCheckFailed;
                return false;
            }

        }

        public bool Function_2(int nodeAddress, int registerAddress, int bitCount, ref bool[] values)
        {
            return ReadBits(MB_READ_DISCRETE, nodeAddress, registerAddress, bitCount, ref values);
        }

        public bool Function_3(int nodeAddress, int startRegisterAddress, int registerCount, ref short[] values)
        {
            return ReadRegisters(MB_READ_HOLD_REG, nodeAddress, startRegisterAddress, registerCount, ref values);
        }
        public bool Function_5(int nodeAddress, int registerAddress, int value)
        {
            byte[] header = BuildMessageHeader(nodeAddress, MB_WRITE_SINGLE_COIL, registerAddress, value);

            //byte[] header = BuildMessageHeader(nodeAddress, MB_WRITE_SINGLE_COIL, registerAddress, value);
            byte crcHi, crcLo;
            CRCCalculation.CRC16(header, header.Length, out crcHi, out crcLo);
            byte[] message = new byte[header.Length + 2];
            header.CopyTo(message, 0);
            message[message.Length - 1] = crcHi;
            message[message.Length - 2] = crcLo;
            byte[] response = new byte[8];

            try
            {
                response = _chassis.Query(message, response.Length, DELAY_MS);
            }
            catch (Exception ex)
            {
                _status = ModbusStatus.WriteError;
                return false;
            }
            if (CheckResponse(response, nodeAddress, MB_WRITE_SINGLE_COIL))
            {
                _status = ModbusStatus.WriteSuccessful;
                return true;
            }
            else
            {
                _status = ModbusStatus.CRCCheckFailed;
                return false;
            }
        }

        /// <summary>
        /// Write single register value
        /// value in dec
        /// </summary>
        /// <param name="nodeAddress"></param>
        /// <param name="registerAddress"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Function_6(int nodeAddress, int registerAddress, short data)    //SET TYPE, SET TEMP, SET TORLERANCE
        {
            byte[] response = new byte[8];
            byte[] header = BuildMessageHeader(nodeAddress, MB_WRITE_SINGLE_REG, registerAddress, data);
            byte crcHi, crcLo;
            CRCCalculation.CRC16(header, header.Length, out crcHi, out crcLo);
            byte[] message = new byte[header.Length + 2];
            header.CopyTo(message, 0);
            message[message.Length - 1] = crcHi;
            message[message.Length - 2] = crcLo;

            try
            {

                response = _chassis.Query(message, response.Length, DELAY_MS);

            }
            catch
            {
                _status = ModbusStatus.WriteError;
                return false;
            }
            if (CheckResponse(response, nodeAddress, MB_WRITE_SINGLE_REG))
            {
                _status = ModbusStatus.WriteSuccessful;
                return true;
            }
            else
            {
                _status = ModbusStatus.CRCCheckFailed;
                return false;
            }
        }
        public bool ReadBits(byte functionType, int nodeAddress, int startRegisterAddress, int bitsCount, ref bool[] values)//int _addr, byte _stand, int _length)
        {
            byte[] response = new byte[5 + bitsCount / 8];
            byte[] header = BuildMessageHeader(nodeAddress, functionType, startRegisterAddress, bitsCount);
            byte[] headerWithDataLengthBit = CalcMessageDataLengthBit(functionType, header, ref bitsCount);
            byte[] message = new byte[headerWithDataLengthBit.Length + bitsCount + 2];
            headerWithDataLengthBit.CopyTo(message, 0);
            byte crcHi, crcLo;
            CRCCalculation.CRC16(message, message.Length - 2, out crcHi, out crcLo);
            message[message.Length - 1] = crcHi;
            message[message.Length - 2] = crcLo;

            try
            {

                response = _chassis.Query(message, response.Length, DELAY_MS);

            }
            catch (Exception ex)
            {
                _status = ModbusStatus.WriteError;
                return false;
            }
            //Evaluate message:
            if (CheckResponse(response, nodeAddress, functionType))
            {
                for (int j = 0; j < response[2]; j++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        values[j * 8 + i] = (response[3 + j] & Convert.ToInt16(Math.Pow(2, i))) == Convert.ToInt16(Math.Pow(2, i));
                    }
                }

                _status = ModbusStatus.WriteSuccessful;
                return true;
            }
            else
            {
                _status = ModbusStatus.CRCCheckFailed;
                return false;
            }
        }

        public bool ReadRegisters(byte functionType, int nodeAddress, int startRegisterAddress, int registerCount, ref short[] values)
        { //Ensure port is open:
            byte[] response = new byte[5 + 2 * registerCount];
            byte[] header = BuildMessageHeader(nodeAddress, functionType, startRegisterAddress, registerCount);
        
            byte[] headerWithDataLengthBit = CalcMessageDataLengthBit(functionType, header, ref registerCount);
         
            byte[] message = new byte[headerWithDataLengthBit.Length + registerCount + 2];
            headerWithDataLengthBit.CopyTo(message, 0);
            byte crcHi, crcLo;
            CRCCalculation.CRC16(message, message.Length - 2, out crcHi, out crcLo);
            message[message.Length - 1] = crcHi;
            message[message.Length - 2] = crcLo;

            try
            {
                response = _chassis.Query(message, response.Length, DELAY_MS);
            }
            catch (Exception ex)
            {

                _status = ModbusStatus.WriteError;
                throw ex;
                return false;
            }
            //Evaluate message:
            if (CheckResponse(response, nodeAddress, functionType))
            {
                //Return requested register values:
                for ( int i = 0; i < (response.Length - 5) / 2; i++)
                {
                    values[i] = response[2 * i + 3];
                    values[i] <<= 8;
                    values[i] += response[2 * i + 4];
                }
                _status = ModbusStatus.WriteSuccessful;
                return true;
            }
            else
            {
                _status = ModbusStatus.CRCCheckFailed;
                return false;
            }
        }
        
        /// <summary>
        /// Read multiple hold regsiters values
        /// </summary>
        /// <param name="nodeAddress"></param>
        /// <param name="startRegisterAddress"></param>
        /// <param name="registerCount"></param>
        /// <param name="values"></param>
        /// <returns></returns>
     
        /// <summary>
        /// Read multiple input regsiters values
        /// </summary>
        /// <param name="nodeAddress"></param>
        /// <param name="startRegisterAddress"></param>
        /// <param name="registerCount"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool Function_4(int nodeAddress, int startRegisterAddress, int registerCount, ref short[] values)
        {
            return ReadRegisters(MB_READ_INPUT_REG, nodeAddress, startRegisterAddress, registerCount, ref values);  
        }
        /// <summary>
        /// Write multiple coils
        /// Values formated in two bytes, example : every byte stand for 8bits - [CD] = 1100 1101
        /// </summary>
        /// <param name="nodeAddress"></param>
        /// <param name="startRegisterAddress"></param>
        /// <param name="registerCount"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool Function_15(int nodeAddress, int startRegisterAddress, int registerCount, byte[] values)
        {

            byte[] response = new byte[8];


            byte[] header = BuildMessageHeader(nodeAddress, MB_WRITE_MULTIPLE_COILS, startRegisterAddress, registerCount);
            byte[] headerWithDataLengthBit = CalcMessageDataLengthBit(MB_WRITE_MULTIPLE_COILS, header, ref registerCount);
            //2 CRC
            byte[] message = new byte[headerWithDataLengthBit.Length + registerCount + 2];
            headerWithDataLengthBit.CopyTo(message, 0);
            Array.Copy(values, 0, message, headerWithDataLengthBit.Length, registerCount);

            byte crcHi, crcLo;
            CRCCalculation.CRC16(message, message.Length - 2, out crcHi, out crcLo);
            message[message.Length - 1] = crcHi;
            message[message.Length - 2] = crcLo;

            try
            {
                response = _chassis.Query(message, response.Length, DELAY_MS);
            }
            catch
            {
                _status = ModbusStatus.WriteError;
                return false;
            }
            if (CheckResponse(response, nodeAddress, MB_WRITE_MULTIPLE_COILS))
            {
                _status = ModbusStatus.WriteSuccessful;
                return true;
            }
            else
            {
                _status = ModbusStatus.CRCCheckFailed;
                return false;
            }
        }

        //林斌新增
        public bool Function_16(int nodeAddress, int startRegisterAddress, int registerCount, ushort[] ushortvalues)
        {

            byte[] values = new byte[registerCount * 2];

            for (int i = 0; i < registerCount; i++)
            {
                byte[] sourceArray = BitConverter.GetBytes(ushortvalues[i]);
                values[i * 2] = sourceArray[1];
                values[i * 2 + 1] = sourceArray[0];
                //Array.Copy(sourceArray, 0, values, i * 2, sourceArray.Length);
            }

            bool rtn = Function_16(nodeAddress, startRegisterAddress, registerCount , values);
            return rtn;

        }

        /// <summary>
        /// Write multiple registers
        /// values formated in two bytes, example : [FF][01] | [EC][64] 
        /// </summary>
        /// <param name="nodeAddress"></param>
        /// <param name="startRegisterAddress"></param>
        /// <param name="registerCount"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool Function_16(int nodeAddress, int startRegisterAddress, int registerCount, byte[] values) //SET UNIT TIME, SET CONTROL MODE
        {

            //MB_WRITE_MULTIPLE_REGS
            byte[] response = new byte[8];
            byte[] header = BuildMessageHeader(nodeAddress, MB_WRITE_MULTIPLE_REGS, startRegisterAddress, registerCount);
            byte[] headerWithDataLengthBit = CalcMessageDataLengthBit(MB_WRITE_MULTIPLE_REGS, header, ref registerCount);
            byte[] message = new byte[headerWithDataLengthBit.Length + registerCount + 2];
            headerWithDataLengthBit.CopyTo(message, 0);

            Array.Copy(values, 0, message, headerWithDataLengthBit.Length, registerCount);

            byte crcHi, crcLo;
            CRCCalculation.CRC16(message, message.Length - 2, out crcHi, out crcLo);
            message[message.Length - 1] = crcHi;
            message[message.Length - 2] = crcLo;

            try
            {
                response = _chassis.Query(message, response.Length, DELAY_MS);
            }
            catch (Exception ex)
            {
                _status = ModbusStatus.WriteError;
                return false;
            }
            if (CheckResponse(response, nodeAddress, MB_WRITE_MULTIPLE_REGS))
            {
                _status = ModbusStatus.WriteSuccessful;
                return true;
            }
            else
            {
                _status = ModbusStatus.CRCCheckFailed;
                return false;
            }
        }
        private bool CheckResponse(byte[] response, int nodeAddress, byte functionType)
        {
            try
            {
                if (response[0] != (byte)nodeAddress)
                    return false;
                if (response[1] != functionType)
                    return false;
                //CRC check
                byte crcHi, crcLo;
                CRCCalculation.CRC16(response, response.Length - 2, out crcHi, out crcLo);
                if (crcHi == response[response.Length - 1] && crcLo == response[response.Length - 2])
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

     
        /// <summary>
        /// Build 6 bytes message header
        /// </summary>
        /// <param name="nodeAddress">Node address(instrument address)</param>
        /// <param name="functionType">Function type</param>
        /// <param name="startAddress">Start register/coil Address</param>
        /// <param name="length">Data length or single data</param>
        private byte[] BuildMessageHeader(int nodeAddress, byte functionType, int startAddress, int length)
        {
            byte[] header = new byte[6];

            header[0] = (byte)nodeAddress;
            header[1] = functionType;
            header[2] = (byte)(startAddress >> 8);
            header[3] = (byte)(startAddress & 0xFF);
            header[4] = (byte)(length >> 8);
            header[5] = (byte)(length & 0xFF);
            return header;
        }
 

        private byte[] CalcMessageDataLengthBit(byte functionType, byte[] messageHeader, ref int length)
        {
            byte[] newHeader = null;
            switch (functionType)
            {
                case MB_READ_COILS:
                case MB_READ_DISCRETE:
                case MB_READ_HOLD_REG:
                case MB_READ_INPUT_REG:
                case MB_WRITE_SINGLE_COIL:
                case MB_WRITE_SINGLE_REG:
                    newHeader = messageHeader;
                    length = 0;
                    break;
                case MB_WRITE_MULTIPLE_COILS://valuse like 0101001100001111
                    //calculate length
                    length = (length % 8 == 0) ? (length / 8) : (length / 8 + 1);
                    //extand source array as new array by 1 bit
                    newHeader = new byte[messageHeader.Length + 1];
                    //copy source to new one
                    messageHeader.CopyTo(newHeader, 0);
                    //write data length to new array last bit
                    newHeader[messageHeader.Length] = (byte)(length);
                    break;
                case MB_WRITE_MULTIPLE_REGS:
                    //calculate length
                    length *= 2;
                    //extand source array as new array by 1 bit
                    newHeader = new byte[messageHeader.Length + 1];
                    //copy source to new one
                    messageHeader.CopyTo(newHeader, 0);
                    //write data length to last bit
                    newHeader[messageHeader.Length] = (byte)length; 
                    break;
            }
            return newHeader;
        }
 
    }
}
