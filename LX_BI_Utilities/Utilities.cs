using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LX_BurnInSolution.Utilities
{
    public static class Reflections
    {
        public static T CreateInstance<T>(string fullTypeName, object[] parameters)
        {
            //try
            //{
            Assembly assembly = Assembly.GetExecutingAssembly();
            T instance = (T)assembly.CreateInstance(fullTypeName, true, BindingFlags.CreateInstance, null, parameters, null, null);
            return instance;
            //}
            //catch (Exception ex)
            //{
            //    string msg = string.Format("Fail to create instance, type name:[{0}],exception:[{1}].", fullTypeName, ex.Message);
            //}
            //return default(T);
        }
    }

    //public static class Converter
    //  {
    //      /// <summary>
    //      /// Extand function for reversing string, because the one which .net provided sucks all the time. 
    //      /// </summary>
    //      /// <param name="sourceString"></param>
    //      /// <returns></returns>
    //      public static string Reverse(this string sourceString)
    //      {
    //          List<char> tempArr = sourceString.ToList<char>();
    //          tempArr.Reverse();
    //          sourceString = string.Concat<char>(tempArr);
    //          return sourceString;
    //      }
    //      public static string ByteArrayToHexString(byte[] Bytes)
    //      {
    //          StringBuilder Result = new StringBuilder(Bytes.Length * 2);
    //          string HexAlphabet = "0123456789ABCDEF";

    //          foreach (byte B in Bytes)
    //          {
    //              Result.Append(HexAlphabet[(int)(B >> 4)]);
    //              Result.Append(HexAlphabet[(int)(B & 0xF)]);
    //          }

    //          return Result.ToString();
    //      }
    //      public static string ByteArrayToBinaryString(byte[] byteArray, bool isReverse = false)
    //      {
    //          int capacity = byteArray.Length * 8;
    //          StringBuilder sb = new StringBuilder(capacity);
    //          for (int i = 0; i < byteArray.Length; i++)
    //          {
    //              string temp = Convert.ToString(byteArray[i], 2).PadLeft(8, '0');
    //              sb.Append(temp);
    //          }
    //          if (isReverse)
    //          {
    //              string temp = sb.ToString();
    //              List<char> tempArr = temp.ToList<char>();
    //              tempArr.Reverse();
    //              return string.Concat<char>(tempArr);
    //          }
    //          return sb.ToString();
    //      }
    //      public static string Byte2BinString(byte b, bool isReverse = false)
    //      {
    //          string temp = Convert.ToString(b, 2).PadLeft(8, '0');
    //          if (isReverse)
    //          {
    //              List<char> tempArr = temp.ToList<char>();
    //              tempArr.Reverse();
    //              return string.Concat<char>(tempArr);
    //          }
    //          return temp;
    //      }
    //      public static byte[] HexStringToByteArray(string Hex, bool isReverse = false)
    //      {
    //          byte[] Bytes = new byte[Hex.Length / 2];
    //          try
    //          {

    //              int[] HexValue = new int[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 
    //                                          0x06, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
    //                                          0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

    //              for (int x = 0, i = 0; i < Hex.Length; i += 2, x += 1)
    //              {
    //                  Bytes[x] = (byte)(HexValue[Char.ToUpper(Hex[i + 0]) - '0'] << 4 |
    //                                    HexValue[Char.ToUpper(Hex[i + 1]) - '0']);
    //              }
    //              if (isReverse)
    //              {
    //                  return Bytes.Reverse().ToArray();
    //              }

    //          }
    //          catch
    //          {

    //          } 
    //          return Bytes;
    //      }
    //      public static byte[] DoubleToBytes(double value, int decimalPlace = 2, bool isReverse = true)
    //      {
    //          byte[] bytes = new byte[2];
    //          string format = ".".PadRight(decimalPlace + 1, '0');
    //          int temp = Convert.ToInt32(value.ToString(format).Replace(".", ""));
    //          return isReverse ? BitConverter.GetBytes(temp).Reverse().ToArray() : BitConverter.GetBytes(temp);
    //      }
    //      public static byte[] IntToBytes(int value)
    //      {
    //          byte[] bytes = new byte[2];
    //          bytes[0] = (byte)(value >> 8);
    //          bytes[1] = (byte)(value & 0xFF);
    //          return bytes;
    //      }
    //      public static byte[] LongToBytes(long value)
    //      {
    //          byte[] bytes = new byte[2];
    //          bytes[0] = (byte)(value >> 8);
    //          bytes[1] = (byte)(value & 0xFF);
    //          return bytes;
    //      }
    //      public static long BytesToInt(byte[] value, int startIndex, int length)
    //      {
    //          byte[] tempBytes = new byte[length];
    //          Array.Copy(value, startIndex, tempBytes, 0, length);
    //          return BytesToInt(tempBytes);
    //      }
    //      public static long BytesToInt(byte[] value)
    //      {
    //          int mask = 0xff;
    //          long temp = 0;
    //          long result = 0;
    //          for (int i = 0; i < value.Length; i++)
    //          {
    //              result <<= 8;
    //              temp = value[i] & mask;
    //              result |= temp;
    //          }
    //          return result;
    //      }
    //      public static T[] ArrayGenerator<T>(T value, int count)
    //      {
    //          return Enumerable.Repeat<T>(value, count).ToArray();
    //      }
    //      public static string TimespanToString(TimeSpan span)
    //      {
    //          double totalHrs = span.Days * 24.0 + span.Hours;
    //          return string.Format("{0:00}:{1:00}:{2:00}", totalHrs, span.Minutes, span.Seconds);
    //      }
    //  }
    //public static class JuniorMath
    //{
    //    public static bool IsValueInLimitRange(double val, double lowerLimitVal, double upperLimitVal)
    //    {
    //        if (val < lowerLimitVal || val > upperLimitVal)
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }
    //    public static bool IsValueInRateRange(double actVal, double tarVal, double rate )
    //    {
    //        if (Math.Abs(actVal - tarVal) > (tarVal * rate))
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }
    //    public static bool IsValueInRateRange(double actVal, double tarVal, double rate,double noiseOffset)
    //    {
    //        if (Math.Abs(actVal - tarVal) > ((tarVal * rate) + noiseOffset))
    //        {
    //            return false;
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }

    //}

}