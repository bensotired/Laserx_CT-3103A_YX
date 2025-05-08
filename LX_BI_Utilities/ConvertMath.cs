using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LX_BurnInSolution.Utilities
{
    /// <summary>
    /// Provides the funtionality for converting a type to another type.
    /// </summary>
    public static class ConvertMath
    {
        /// <summary>
        /// Converts IEEE754 format string (32-bits number) to a number.
        /// <para></para>
        /// The byte order is reserved of the number string.
        /// </summary>
        /// <param name="value">The number string to be converted.
        /// <para>The value should be UTF8 format.</para></param>
        /// <returns></returns>
        public static double ConvertIEEE754ReservedStringToNumber(string value)
        {
            byte[] asc = Encoding.UTF8.GetBytes(value);
            double number = System.BitConverter.ToSingle(asc, 0);
            return number;
        }
        /// <summary>
        /// Converts IEEE754 format byte array (32-bits number) to a number.
        /// <para></para>
        /// The byte order is reserved.
        /// </summary>
        /// <param name="values">The byte array to be converted.
        /// </param>
        /// <param name="startIndex"></param>
        public static double ConvertIEEE754ReservedByteArrayToNumber(byte[] values, int startIndex)
        {
            double number = System.BitConverter.ToSingle(values, startIndex);
            return number;
        }

        /// <summary>
        /// Converts IEEE754 format string (32-bits number) to numbers.
        /// <para>The byte order is reserved of each number string.</para>
        /// </summary>
        /// <param name="value">The string of number array to be converted.
        /// <para>The value should be UTF8 format.</para></param>
        /// <returns></returns>
        public static double[] ConvertIEEE754ReservedStringToNumbers(string value)
        {
            int count = (value.Length - 3) / 4;
            double[] numbers = new double[count];
            for (int i = 0; i < count; i++)
            {
                numbers[i] = ConvertIEEE754ReservedStringToNumber(value.Substring(2 + i * 4, 4));
            }
            return numbers;
        }
        /// <summary>
        ///  Converts IEEE754 format byte array (32-bits number) to numbers.
        ///  <para>The byte order is reserved of each number.</para>
        /// </summary>
        /// <param name="values">The byte array to be converted.
        /// </param>
        /// <returns></returns>
        public static double[] ConvertIEEE754ReservedByteArrayToNumbers(byte[] values)
        {
            int count = (values.Length - 3) / 4;
            double[] numbers = new double[count];
            for (int i = 0; i < count; i++)
            {
                numbers[i] = ConvertIEEE754ReservedByteArrayToNumber(values, i * 4 + 2);
            }

            return numbers;
        }

        /// <summary>
        /// Converts a number to Scientific Notation format and rounds its Scientific Notation number before E notation.
        /// <para></para>
        /// E.g. value=0.0003456 and digits=2, the returned data will be 0.00035.
        /// </summary>
        /// <param name="value">A double-precision floating-point number to be rounded.</param>
        /// <param name="digits">The number of fractional digits in the Scientific Notation format of the value.</param>
        /// <returns></returns>
        //public static double ScientificNotationRound(this double value, int digits)
        //{
        //    if (digits < 1)
        //    {
        //        throw new ArgumentOutOfRangeException("digits", digits, "digits cannot be smaller than 1.");
        //    }
        //    string temp = value.ToString("e" + (digits - 1).ToString());
        //    return double.Parse(temp);
        //}

        public static string ConvertNumberToIEEE754ReservedString(double value)
        {
            byte[] asc = System.BitConverter.GetBytes(value);

            string st = "";
            foreach (byte b in asc)
            {
                st = st + b.ToString("X2");
            }
            return st;
        }
        public static string ConvertFloatToIEEE754ReservedString(float value)
        {
            byte[] asc = BitConverter.GetBytes(value);
            Array.Reverse(asc);
            string st = "";
            foreach (byte b in asc)
            {
                st = st + b.ToString("X2");
            }
            return st;
        }
        public static float ConvertIEEE754ReservedStringToFloat(string value)
        {
            byte[] asc2 = BitConverter.GetBytes(Convert.ToUInt32(value, 16));
            float number = System.BitConverter.ToSingle(asc2, 0);
            return number;
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
    }
}
