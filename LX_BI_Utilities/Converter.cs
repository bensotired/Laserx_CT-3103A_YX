using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LX_BurnInSolution.Utilities
{
    public static class Converter
    {
        public static object ConvertObjectTo(object obj, Type type)
        {
            if (type == null) return obj;
            if (obj == null) return type.IsValueType ? Activator.CreateInstance(type) : null;

            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (type.IsAssignableFrom(obj.GetType())) // 如果待转换对象的类型与目标类型兼容，则无需转换
            {
                return obj;
            }
            else if ((underlyingType ?? type).IsEnum) // 如果待转换的对象的基类型为枚举
            {
                if (underlyingType != null && string.IsNullOrEmpty(obj.ToString())) // 如果目标类型为可空枚举，并且待转换对象为null 则直接返回null值
                {
                    return null;
                }
                else
                {
                    return Enum.Parse(underlyingType ?? type, obj.ToString());
                }
            }
            else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type)) // 如果目标类型的基类型实现了IConvertible，则直接转换
            {
                try
                {
                    return Convert.ChangeType(obj, underlyingType ?? type, null);
                }
                catch(FormatException fex)
                {
                    throw fex;
                }
                catch(Exception ex)
                {
                    return underlyingType == null ? Activator.CreateInstance(type) : null;
                }
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(obj.GetType()))
                {
                    return converter.ConvertFrom(obj);
                }
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    object o = constructor.Invoke(null);
                    PropertyInfo[] propertys = type.GetProperties();
                    Type oldType = obj.GetType();
                    foreach (PropertyInfo property in propertys)
                    {
                        PropertyInfo p = oldType.GetProperty(property.Name);
                        if (property.CanWrite && p != null && p.CanRead)
                        {
                            property.SetValue(o, ConvertObjectTo(p.GetValue(obj, null), property.PropertyType), null);
                        }
                    }
                    return o;
                }
            }
            return obj;
        }
        /// <summary>
        /// Extand function for reversing string, because the one which .net provided sucks all the time. 
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string Reverse(this string sourceString)
        {
            List<char> tempArr = sourceString.ToList<char>();
            tempArr.Reverse();
            sourceString = string.Concat<char>(tempArr);
            return sourceString;
        }
        public static bool[] ByteToBit(byte byt)
        {

            bool[] bits = new bool[8];

            for (int j = 0; j < 8; j++)
            {
                bits[j] = Convert.ToBoolean((byt >> j) & 0x01);
            }

            return bits;
        }
        public static int[] ByteToBitInterger(byte byt)
        {

            int[] bits = new int[8];

            for (int j = 0; j < 8; j++)
            {
                bits[j] = ((byt >> j) & 0x01);
            }

            return bits;
        }
        public static bool[] ShortToBit(short val)
        {
            var byt = Convert.ToByte(val);
            return ByteToBit(byt);
        }
        public static string ByteArrayToHexString(byte[] Bytes)
        {
            StringBuilder Result = new StringBuilder(Bytes.Length * 2);
            string HexAlphabet = "0123456789ABCDEF";

            foreach (byte B in Bytes)
            {
                Result.Append(HexAlphabet[(int)(B >> 4)]);
                Result.Append(HexAlphabet[(int)(B & 0xF)]);
            }

            return Result.ToString();
        }
        public static string ByteArrayToBinaryString(byte[] byteArray, bool isReverse = false)
        {
            int capacity = byteArray.Length * 8;
            StringBuilder sb = new StringBuilder(capacity);
            for (int i = 0; i < byteArray.Length; i++)
            {
                string temp = Convert.ToString(byteArray[i], 2).PadLeft(8, '0');
                sb.Append(temp);
            }
            if (isReverse)
            {
                string temp = sb.ToString();
                List<char> tempArr = temp.ToList<char>();
                tempArr.Reverse();
                return string.Concat<char>(tempArr);
            }
            return sb.ToString();
        }
        public static string Byte2BinString(byte b, bool isReverse = false)
        {
            string temp = Convert.ToString(b, 2).PadLeft(8, '0');
            if (isReverse)
            {
                List<char> tempArr = temp.ToList<char>();
                tempArr.Reverse();
                return string.Concat<char>(tempArr);
            }
            return temp;
        }
        public static byte[] HexStringToByteArray(string Hex, bool isReverse = false)
        {
            byte[] Bytes = new byte[Hex.Length / 2];
            try
            {

                int[] HexValue = new int[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 
                                            0x06, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                                            0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

                for (int x = 0, i = 0; i < Hex.Length; i += 2, x += 1)
                {
                    Bytes[x] = (byte)(HexValue[Char.ToUpper(Hex[i + 0]) - '0'] << 4 |
                                      HexValue[Char.ToUpper(Hex[i + 1]) - '0']);
                }
                if (isReverse)
                {
                    return Bytes.Reverse().ToArray();
                }

            }
            catch
            {

            }
            return Bytes;
        }
        public static float[] BytesToFloat(short[] tempArr)
        {
            float[] tempf = new float[tempArr.Length / 2];
            byte[] lo = new byte[2];
            byte[] hi = new byte[2];
            byte[] temp = new byte[4];
            for (int i = 0; i < tempArr.Length; i += 2)
            {
                lo = BitConverter.GetBytes(tempArr[i + 1]);
                hi = BitConverter.GetBytes(tempArr[i]);

                Array.Copy(lo, 0, temp, 0, 2);
                Array.Copy(hi, 0, temp, 2, 2);
                //Array.Reverse(temp);
                tempf[i / 2] = BitConverter.ToSingle(temp, 0);
            }
            return tempf;
        }
        public static byte[] FloatToBytes(float value, bool isReverse = true)
        {
            byte[] ret;
            if (isReverse)
            {
                ret = BitConverter.GetBytes(value).Reverse().ToArray();
            }
            else
            {
                ret = BitConverter.GetBytes(value);
            }
            return ret;
        }
        public static byte[] DoubleToBytes(double value, int decimalPlace = 2, bool isReverse = true)
        {
            byte[] bytes = new byte[2];
            string format = ".".PadRight(decimalPlace + 1, '0');
            int temp = Convert.ToInt16(value.ToString(format).Replace(".", ""));
            return isReverse ? BitConverter.GetBytes(temp).Reverse().ToArray() : BitConverter.GetBytes(temp);
        }
        public static byte[] IntToBytes(int value)
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(value >> 8);
            bytes[1] = (byte)(value & 0xFF);
            return bytes;
        }
        public static byte[] LongToBytes(long value)
        {
            byte[] bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = (byte)(value & 0xFF);
                value = value >> 8;
            }
            return bytes;
        }
        public static long BytesToInt(byte[] value, int startIndex, int length)
        {
            byte[] tempBytes = new byte[length];
            Array.Copy(value, startIndex, tempBytes, 0, length);
            return BytesToInt(tempBytes);
        }
        public static long BytesToInt(byte[] value)
        {
            int mask = 0xff;
            long temp = 0;
            long result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                result <<= 8;
                temp = value[i] & mask;
                result |= temp;
            }
            return result;
        }   

        public static byte[] ShortToByte(short value)
        {
            byte high = (byte)(0x00FF & (value >> 8));    
            byte low = (byte)(0x00FF & value); 
            byte[] bytes = new byte[2];
            bytes[0] = high;
            bytes[1] = low;
            return bytes;
        }
        public static double ShortArrayToDouble(short[] valArr)
        {
            double tempf;
            byte[] lo = new byte[2];
            byte[] hi = new byte[2];
            byte[] temp = new byte[4];
            lo = BitConverter.GetBytes(valArr[1]);
            hi = BitConverter.GetBytes(valArr[0]);

            Array.Copy(lo, 0, temp, 0, 2);
            Array.Copy(hi, 0, temp, 2, 2);
            //Array.Reverse(temp);
            tempf = BitConverter.ToSingle(temp, 0);
            return tempf;
        }
        public static T[] ArrayGenerator<T>(T value, int count)
        {
            return Enumerable.Repeat<T>(value, count).ToArray();
        }
        public static string TimespanToString(TimeSpan span)
        {
            double totalHrs = span.Days * 24.0 + span.Hours;
            return string.Format("{0:00}:{1:00}:{2:00}", totalHrs, span.Minutes, span.Seconds);
        }
    }
}
