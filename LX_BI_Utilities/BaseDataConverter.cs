using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LX_BurnInSolution.Utilities
{
    /// <summary>
    /// 提供基础数据转换方法
    /// </summary>
    public static class BaseDataConverter
    {
        public static double ScientificNotationRound(this double value, int digits)
        {
            if (digits < 1)
            {
                throw new ArgumentOutOfRangeException("digits", digits, "digits cannot be smaller than 1.");
            }
            string temp = value.ToString("e" + (digits - 1).ToString());
            return double.Parse(temp);
        }
        public static string ConvertCollectionToString<TItem>(IEnumerable<TItem> collection, string splitor)
        {
            string result = string.Empty;
            foreach (var item in collection)
            {
                result += $"{item}{splitor}";
            }

            int _finded = result.LastIndexOf(splitor);
            if (_finded != -1)
            {
                return result.Substring(0, _finded);
            }
            return result;
        }

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
                catch
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
        public static string ConvertTimespanToString(TimeSpan span)
        {
            double totalHrs = span.Days * 24.0 + span.Hours;
            return string.Format("{0:00}:{1:00}:{2:00}", totalHrs, span.Minutes, span.Seconds);
        }
        public static DateTime ConvertCommentStringToDateTime(string commentString)
        {
            DateTime rDT = DateTime.Now;
            if (DateTime.TryParse(commentString, out rDT))
            {

            }
            return rDT;
        }
        public static string ConvertDateTimeToCommentString(DateTime time)
        {
            return time.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public static string ConvertDateTimeTo_FILE_string(DateTime time)
        {
            return time.ToString("yyyyMMdd_HHmmss");
        }
        public static string ConvertDateTimeStringTo_FILE_string(string timestring)
        {
            return timestring.Replace("/", "").Replace(" ", "_").Replace(":", "");
        }
        // CDAB 类型 float -> 2short
        public static ushort[] ConvertFloatToUshortCDAB(float value)
        {
            ushort[] buf = new ushort[2];
            byte[] byteData = BitConverter.GetBytes(value);
            ushort[] registerBuffer = new ushort[byteData.Length / 2];
            registerBuffer[1] = BitConverter.ToUInt16(byteData, 0);
            registerBuffer[0] = BitConverter.ToUInt16(byteData, 2);
            buf[1] = registerBuffer[0];
            buf[0] = registerBuffer[1];
            return buf;
        }
        // 2short -> float CDAB 类型
        //20200602
        public static float ConvertUshortToFloatCDAB(short[] registerBuffer)
        {
            float value = 0;
            byte[] byteData1 = BitConverter.GetBytes(registerBuffer[0]);
            byte[] byteData2 = BitConverter.GetBytes(registerBuffer[1]);
            byte[] byteData = new byte[4];
            byteData[3] = byteData2[1];
            byteData[2] = byteData2[0];
            byteData[1] = byteData1[1];
            byteData[0] = byteData1[0];
            value = BitConverter.ToSingle(byteData, 0);
            return value;
        }

        // 2short -> float ABCD 类型
        //20200602
        public static float ConvertUshortToFloatABCD(short[] registerBuffer)
        {
            float value = 0;
            byte[] byteData1 = BitConverter.GetBytes(registerBuffer[0]);
            byte[] byteData2 = BitConverter.GetBytes(registerBuffer[1]);
            byte[] byteData = new byte[4];
            byteData[3] = byteData1[1];
            byteData[2] = byteData1[0];
            byteData[1] = byteData2[1];
            byteData[0] = byteData2[0];
            value = BitConverter.ToSingle(byteData, 0);
            return value;
        }

        // ABCD 类型 float -> 2short
        public static ushort[] FloatToUshort_ABCD(float value)
        {
            ushort[] buf = new ushort[2];
            byte[] byteData = BitConverter.GetBytes(value);
            ushort[] registerBuffer = new ushort[byteData.Length / 2];
            registerBuffer[1] = BitConverter.ToUInt16(byteData, 0);
            registerBuffer[0] = BitConverter.ToUInt16(byteData, 2);
            buf[1] = registerBuffer[1];
            buf[0] = registerBuffer[0];
            return buf;
        }
        public static double[] ArrayConvert_ABDEShortToDouble(short[] array)
        {
            List<double> ArrayDouble = new List<double>();
            int count = array.Length;
            short[] shortvalue = new short[2];

            for (int i = 0; i < count; i += 2)
            {
                Array.Copy(array, i, shortvalue, 0, 2);
                
                ArrayDouble.Add(ConvertUshortToFloatABCD(shortvalue));
            }

            return ArrayDouble.ToArray();
        }
        public static ushort[] LongToUshort(int value)
        {
            ushort[] buf = new ushort[2];
            byte[] byteData = BitConverter.GetBytes(value);
            ushort[] registerBuffer = new ushort[byteData.Length / 2];
            registerBuffer[1] = BitConverter.ToUInt16(byteData, 0);
            registerBuffer[0] = BitConverter.ToUInt16(byteData, 2);
            buf[1] = registerBuffer[1];
            buf[0] = registerBuffer[0];
            return buf;
        }

        //2short -> long
        public static int UshortToLong(short[] registerBuffer)
        {
            int value = 0;
            byte[] byteData1 = BitConverter.GetBytes(registerBuffer[0]);
            byte[] byteData2 = BitConverter.GetBytes(registerBuffer[1]);
            byte[] byteData = new byte[4];
            byteData[3] = byteData1[1];
            byteData[2] = byteData1[0];
            byteData[1] = byteData2[1];
            byteData[0] = byteData2[0];
            value = BitConverter.ToInt32(byteData, 0);
            return value;
        }
        public static float[] ArrayConvertStringToSingle(string[] array)
        {
            return Array.ConvertAll(array, new Converter<string, float>(ConvertStringToSingle));
        }
        private static float ConvertStringToSingle(string str)
        {
            return Convert.ToSingle(str);
        }
        public static double[] ArrayConvertStringToDouble(string[] array)
        {
            return Array.ConvertAll(array, new Converter<string, double>(ConvertStringToDouble));
        }
        private static double ConvertStringToDouble(string str)
        {
            return Convert.ToDouble(str);
        }
        public static double[] ArrayConvertByteToDouble(byte[] array)
        {
            return Array.ConvertAll(array, new Converter<byte, double>(ConvertByteToDouble));
        }
        static double ConvertByteToDouble(byte byteVal)
        {
            return Convert.ToDouble(byteVal);
        }
        public static float[] ArrayConvertShortToFloat(short[] array)
        {
            return Array.ConvertAll(array, new Converter<short, float>(ConvertShortToFloat));
        }
        public static float[] ArrayConvertDoubleToFloat(double[] array)
        {
            return Array.ConvertAll(array, new Converter<double, float>(ConvertDoubleToFloat));
        }
        static float ConvertDoubleToFloat(double dblVal)
        {
            var temp = Convert.ToSingle(dblVal);
            return temp;
        }
        public static double[] ArrayConvertShortToFloatToDouble(short[] tempArr)
        {
            double[] tempf = new double[tempArr.Length / 2];
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
        static float ConvertShortToFloat(short shortVal)
        {
            var temp = Convert.ToSingle(shortVal);
            return temp;
        }
        public static double[] ArrayConvertShortToDouble(short[] array)
        {
            return Array.ConvertAll(array, new Converter<short, double>(ConvertShortToDouble));
        }
        public static double[] ArrayConvertFloatToDouble(float[] array)
        {
            return Array.ConvertAll(array, new Converter<float, double>(ConvertFloatToDouble));
        }
        public static double[] ArrayConvertABCDLongToDouble(short[] array)
        {
            return Array.ConvertAll(array, new Converter<short, double>(ConvertShortToDouble));
        }

        static double ConvertShortToDouble(short shortVal)
        {
            var temp = Convert.ToDouble(shortVal);
            return temp;
        }
        static double ConvertFloatToDouble(float fval)
        {
            var temp = Convert.ToDouble(fval);
            return temp;
        }
        public static int[] ArrayConvertShortToInteger(short[] array)
        {
            return Array.ConvertAll(array, new Converter<short, int>(ConvertShortToInteger));
        }
        static int ConvertShortToInteger(short shortVal)
        {
            return Convert.ToInt16(shortVal);
        }
        public static string ConvertByteArrayToHexString(byte[] Bytes)
        {
            const string HexAlphabet = "0123456789ABCDEF";
            StringBuilder Result = new StringBuilder(Bytes.Length * 2);

            foreach (byte B in Bytes)
            {
                Result.Append(HexAlphabet[(int)(B >> 4)]);
                Result.Append(HexAlphabet[(int)(B & 0xF)]);
            }

            return Result.ToString();
        }

        public static bool[] ConvertByteToBitBoolean(byte byt)
        {
            const int bitsForOneByte = 8;
            bool[] bits = new bool[bitsForOneByte];
            for (int j = 0; j < bitsForOneByte; j++)
            {
                bits[j] = Convert.ToBoolean((byt >> j) & 0x01);
            }
            return bits;
        }
        public static bool[] ConvertShortToBitBoolean(short val)
        {
            var byt = Convert.ToByte(val);
            return ConvertByteToBitBoolean(byt);
        }
        public static int[] ConvertByteToBitInterger(byte byt)
        {
            const int bitsForOneByte = 8;
            int[] bits = new int[bitsForOneByte];
            for (int j = 0; j < bitsForOneByte; j++)
            {
                bits[j] = ((byt >> j) & 0x01);
            }
            return bits;
        }
        public static string ConvertByteToBinString(byte b, bool isReverse)
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
        public static byte[] ConvertHexStringToByteArray(string Hex, bool isReverse)
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
        public static float[] ArrayConvertByteToFloat(short[] floatArray)
        {
            float[] tempf = new float[floatArray.Length / 2];
            byte[] lo = new byte[2];
            byte[] hi = new byte[2];
            byte[] temp = new byte[4];
            for (int i = 0; i < floatArray.Length; i += 2)
            {
                lo = BitConverter.GetBytes(floatArray[i + 1]);
                hi = BitConverter.GetBytes(floatArray[i]);

                Array.Copy(lo, 0, temp, 0, 2);
                Array.Copy(hi, 0, temp, 2, 2);
                //Array.Reverse(temp);
                tempf[i / 2] = BitConverter.ToSingle(temp, 0);
            }
            return tempf;
        }
        public static byte[] ConvertDoubleToBytes(double value, int decimalPlace = 2, bool isReverse = true)
        {
            byte[] bytes = new byte[2];
            string format = ".".PadRight(decimalPlace + 1, '0');
            int temp = Convert.ToInt16(value.ToString(format).Replace(".", ""));
            return isReverse ? BitConverter.GetBytes(temp).Reverse().ToArray() : BitConverter.GetBytes(temp);
        }
        public static byte[] ConvertIntToBytes(int value)
        {
            byte[] bytes = new byte[2];
            bytes[0] = (byte)(value >> 8);
            bytes[1] = (byte)(value & 0xFF);
            return bytes;
        }
        public static byte[] ConvertLongToBytes(long value)
        {
            byte[] bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = (byte)(value & 0xFF);
                value = value >> 8;
            }
            return bytes;
        }
        public static long ConvertBytesToInt(byte[] value, int startIndex, int length)
        {
            if(startIndex >= value.Length)
            {
                throw new IndexOutOfRangeException("ConvertBytesToInt(byte[] value, int startIndex, int length) startIndex out of range!");
            }
            byte[] tempBytes = new byte[length];
            Array.Copy(value, startIndex, tempBytes, 0, length);
            return ConvertBytesToInt(tempBytes);
        }
        public static long ConvertBytesToInt(byte[] value)
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
        public static byte[] ConvertShortToByte(short value)
        {
            byte high = (byte)(0x00FF & (value >> 8));
            byte low = (byte)(0x00FF & value);
            byte[] bytes = new byte[2];
            bytes[0] = high;
            bytes[1] = low;
            return bytes;
        }
        public static double ConvertShortArrayToDouble(short[] valArr)
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
    }
}