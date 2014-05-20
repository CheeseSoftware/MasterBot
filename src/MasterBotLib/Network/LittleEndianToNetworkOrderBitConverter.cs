
using System;

namespace MasterBot.Network
{
    internal class LittleEndianToNetworkOrderBitConverter
    {
        public static byte[] GetBytes(int value)
        {
            return LittleEndianToNetworkOrderBitConverter.swap(BitConverter.GetBytes(value), 4, 0);
        }
        public static byte[] GetBytes(uint value)
        {
            return LittleEndianToNetworkOrderBitConverter.swap(BitConverter.GetBytes(value), 4, 0);
        }
        public static byte[] GetBytes(long value)
        {
            return LittleEndianToNetworkOrderBitConverter.swap(BitConverter.GetBytes(value), 8, 0);
        }
        public static byte[] GetBytes(ushort value)
        {
            return LittleEndianToNetworkOrderBitConverter.swap(BitConverter.GetBytes(value), 2, 0);
        }
        public static byte[] GetBytes(ulong value)
        {
            return LittleEndianToNetworkOrderBitConverter.swap(BitConverter.GetBytes(value), 8, 0);
        }
        public static byte[] GetBytes(float value)
        {
            return LittleEndianToNetworkOrderBitConverter.swap(BitConverter.GetBytes(value), 4, 0);
        }
        public static byte[] GetBytes(double value)
        {
            return LittleEndianToNetworkOrderBitConverter.swap(BitConverter.GetBytes(value), 8, 0);
        }
        public static int ToInt32(byte[] value, int startIndex, int length)
        {
            return BitConverter.ToInt32(LittleEndianToNetworkOrderBitConverter.swap(value, length, startIndex), startIndex);
        }
        public static uint ToUInt32(byte[] value, int startIndex, int length)
        {
            return BitConverter.ToUInt32(LittleEndianToNetworkOrderBitConverter.swap(value, length, startIndex), startIndex);
        }
        public static long ToInt64(byte[] value, int startIndex, int length)
        {
            return BitConverter.ToInt64(LittleEndianToNetworkOrderBitConverter.swap(value, length, startIndex), startIndex);
        }
        public static ulong ToUInt64(byte[] value, int startIndex, int length)
        {
            return BitConverter.ToUInt64(LittleEndianToNetworkOrderBitConverter.swap(value, length, startIndex), startIndex);
        }
        public static float ToSingle(byte[] value, int startIndex, int length)
        {
            return BitConverter.ToSingle(LittleEndianToNetworkOrderBitConverter.swap(value, length, startIndex), startIndex);
        }
        public static double ToDouble(byte[] value, int startIndex, int length)
        {
            return BitConverter.ToDouble(LittleEndianToNetworkOrderBitConverter.swap(value, length, startIndex), startIndex);
        }
        private static byte[] swap(byte[] input, int length, int startIndex)
        {
            Array.Reverse(input, startIndex, length);
            return input;
        }
    }
}