
using System;
using System.Text;

namespace MasterBot.Network
{
    internal class MessageSerializer : IDisposable
    {
        private const byte TopPattern = 192;
        private const byte BottomPattern = 60;
        private const byte StringTopPattern = 192;
        private const byte IntegerTopPattern = 128;
        private const byte ByteArrayTopPattern = 64;
        private const byte IntegerBottomPattern = 4;
        private const byte UnsignedIntegerBottomPattern = 8;
        private const byte StringBottomPattern = 12;
        private const byte ByteArrayBottomPattern = 16;
        private const byte ShortLongBottomPattern = 48;
        private const byte LongBottomPattern = 52;
        private const byte ShortUnsignedLongBottomPattern = 56;
        private const byte UnsignedLongBottomPattern = 60;
        private const byte DoublePattern = 3;
        private const byte FloatPattern = 2;
        private const byte BooleanTruePattern = 1;
        private const byte BooleanFalsePattern = 0;
        internal const byte EntryType_Integer = 0;
        internal const byte EntryType_UnsignedInteger = 1;
        internal const byte EntryType_Long = 2;
        internal const byte EntryType_UnsignedLong = 3;
        internal const byte EntryType_Double = 4;
        internal const byte EntryType_Float = 5;
        internal const byte EntryType_String = 6;
        internal const byte EntryType_ByteArray = 7;
        internal const byte EntryType_Boolean = 8;
        private byte[] savedBytes = null;
        private Message message = null;
        private int partsInMessage = 0;
        void IDisposable.Dispose() { }
        public Message Deserialize(byte[] bytes, int start, int count)
        {
            if (this.savedBytes != null)
            {
                byte[] array = new byte[this.savedBytes.Length + count];
                Array.Copy(this.savedBytes, array, this.savedBytes.Length);
                Array.Copy(bytes, start, array, this.savedBytes.Length, count);
                bytes = array;
                start = 0;
                count = bytes.Length;
                this.savedBytes = null;
            }
            byte[] array2 = new byte[8];
            int i = start;
            int num = start + count;
            while (i < num)
            {
                int num2 = i;
                int num3 = 0;
                int value = 0;
                byte b = bytes[i];
                i++;
                int num4 = (int)(b & 192);
                if (num4 == 0)
                {
                    num4 = (int)(b & 60);
                    if (num4 == 0)
                    {
                        num4 = (int)b;
                    }
                }
                int num5 = num4;
                if (num5 <= 52)
                {
                    if (num5 <= 16)
                    {
                        switch (num5)
                        {
                            case 0:
                                goto IL_246;
                            case 1:
                                goto IL_246;
                            case 2:
                                num3 = 4;
                                goto IL_246;
                            case 3:
                                num3 = 8;
                                goto IL_246;
                            case 4:
                            case 8:
                                goto IL_212;
                            case 5:
                            case 6:
                            case 7:
                            case 9:
                            case 10:
                            case 11:
                                goto IL_246;
                            case 12:
                                break;
                            default:
                                if (num5 != 16)
                                {
                                    goto IL_246;
                                }
                                break;
                        }
                        num3 = (int)((b & 3) + 1);
                        if (i + num3 > num)
                        {
                            this.savedBytes = new byte[num - num2];
                            Array.Copy(bytes, num2, this.savedBytes, 0, this.savedBytes.Length);
                            break;
                        }
                        array2[0] = 0;
                        array2[1] = 0;
                        array2[2] = 0;
                        array2[3] = 0;
                        Array.Copy(bytes, i, array2, 0, num3);
                        i += num3;
                        num3 = LittleEndianToNetworkOrderBitConverter.ToInt32(array2, 0, num3);
                    }
                    else
                    {
                        if (num5 == 48)
                        {
                            goto IL_212;
                        }
                        if (num5 == 52)
                        {
                            goto IL_21F;
                        }
                    }
                }
                else
                {
                    if (num5 <= 60)
                    {
                        if (num5 == 56)
                        {
                            goto IL_212;
                        }
                        if (num5 == 60)
                        {
                            goto IL_21F;
                        }
                    }
                    else
                    {
                        if (num5 != 64)
                        {
                            if (num5 != 128)
                            {
                                if (num5 == 192)
                                {
                                    num3 = (int)(b & 63);
                                }
                            }
                            else
                            {
                                value = (int)(b & 63);
                            }
                        }
                        else
                        {
                            num3 = (int)(b & 63);
                        }
                    }
                }
            IL_246:
                if (i + num3 > num)
                {
                    this.savedBytes = new byte[num - num2];
                    Array.Copy(bytes, num2, this.savedBytes, 0, this.savedBytes.Length);
                    break;
                }
                Message message = this.message;
                num5 = num4;
                if (num5 <= 52)
                {
                    if (num5 <= 16)
                    {
                        switch (num5)
                        {
                            case 0:
                                message.Add(false);
                                break;
                            case 1:
                                message.Add(true);
                                break;
                            case 2:
                                message.Add(LittleEndianToNetworkOrderBitConverter.ToSingle(bytes, i, 4));
                                break;
                            case 3:
                                message.Add(LittleEndianToNetworkOrderBitConverter.ToDouble(bytes, i, 8));
                                break;
                            case 4:
                                array2[0] = 0;
                                array2[1] = 0;
                                array2[2] = 0;
                                array2[3] = 0;
                                Array.Copy(bytes, i, array2, 0, num3);
                                value = LittleEndianToNetworkOrderBitConverter.ToInt32(array2, 0, num3);
                                goto IL_3D7;
                            case 5:
                            case 6:
                            case 7:
                            case 9:
                            case 10:
                            case 11:
                                break;
                            case 8:
                                array2[0] = 0;
                                array2[1] = 0;
                                array2[2] = 0;
                                array2[3] = 0;
                                Array.Copy(bytes, i, array2, 0, num3);
                                message.Add(LittleEndianToNetworkOrderBitConverter.ToUInt32(array2, 0, num3));
                                break;
                            case 12:
                                goto IL_34A;
                            default:
                                if (num5 == 16)
                                {
                                    goto IL_40A;
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (num5 == 48 || num5 == 52)
                        {
                            array2[0] = 0;
                            array2[1] = 0;
                            array2[2] = 0;
                            array2[3] = 0;
                            array2[4] = 0;
                            array2[5] = 0;
                            array2[6] = 0;
                            array2[7] = 0;
                            Array.Copy(bytes, i, array2, 0, num3);
                            message.Add(LittleEndianToNetworkOrderBitConverter.ToInt64(array2, 0, num3));
                        }
                    }
                }
                else
                {
                    if (num5 <= 60)
                    {
                        if (num5 == 56 || num5 == 60)
                        {
                            array2[0] = 0;
                            array2[1] = 0;
                            array2[2] = 0;
                            array2[3] = 0;
                            array2[4] = 0;
                            array2[5] = 0;
                            array2[6] = 0;
                            array2[7] = 0;
                            Array.Copy(bytes, i, array2, 0, num3);
                            message.Add(LittleEndianToNetworkOrderBitConverter.ToUInt64(array2, 0, num3));
                        }
                    }
                    else
                    {
                        if (num5 == 64)
                        {
                            goto IL_40A;
                        }
                        if (num5 == 128)
                        {
                            goto IL_3D7;
                        }
                        if (num5 == 192)
                        {
                            goto IL_34A;
                        }
                    }
                }
            IL_52B:
                i += num3;
                if (message != null && --this.partsInMessage == 0)
                {
                    return message;
                }
                continue;
            IL_34A:
                if (message == null)
                {
                    message = (this.message = new Message(Encoding.UTF8.GetString(bytes, i, num3)));
                    this.partsInMessage++;
                }
                else
                {
                    message.Add(Encoding.UTF8.GetString(bytes, i, num3));
                }
                goto IL_52B;
            IL_3D7:
                if (this.partsInMessage == 0)
                {
                    this.partsInMessage = value;
                }
                else
                {
                    message.Add(value);
                }
                goto IL_52B;
            IL_40A:
                byte[] array3 = new byte[num3];
                Array.Copy(bytes, i, array3, 0, num3);
                message.Add(array3);
                goto IL_52B;
            IL_212:
                num3 = (int)((b & 3) + 1);
                goto IL_246;
            IL_21F:
                num3 = (int)((b & 3) + 5);
                goto IL_246;
            }
            return null;
        }
        internal void Serialize(Message m, ByteWriter writer)
        {
            this.serializeToBytes(m, writer);
        }
        private void serializeToBytes(Message m, ByteWriter writer)
        {
            writer.WriteTagWithLength((int)m.Count, 128, 4);
            byte[] array = Encoding.UTF8.GetBytes(m.Type);
            writer.WriteTagWithLength(array.Length, 192, 12);
            writer.Write(array);
            for (uint num = 0u; num != m.Count; num += 1u)
            {
                object obj = m[num];
                switch (m.types[(int)((UIntPtr)num)])
                {
                    case 0:
                        writer.WriteTagWithLength((int)obj, 128, 4);
                        break;
                    case 1:
                        writer.WriteBottomPatternAndBytes(8, LittleEndianToNetworkOrderBitConverter.GetBytes((uint)obj));
                        break;
                    case 2:
                        writer.WriteLongPattern(48, 52, LittleEndianToNetworkOrderBitConverter.GetBytes((long)obj));
                        break;
                    case 3:
                        writer.WriteLongPattern(56, 60, LittleEndianToNetworkOrderBitConverter.GetBytes((ulong)obj));
                        break;
                    case 4:
                        writer.Write(3);
                        writer.Write(LittleEndianToNetworkOrderBitConverter.GetBytes((double)obj));
                        break;
                    case 5:
                        writer.Write(2);
                        writer.Write(LittleEndianToNetworkOrderBitConverter.GetBytes((float)obj));
                        break;
                    case 6:
                        array = Encoding.UTF8.GetBytes((string)obj);
                        writer.WriteTagWithLength(array.Length, 192, 12);
                        writer.Write(array);
                        break;
                    case 7:
                        array = (byte[])obj;
                        writer.WriteTagWithLength(array.Length, 64, 16);
                        writer.Write(array);
                        break;
                    case 8:
                        writer.Write((byte)((bool)obj ? 1 : 0));
                        break;
                }
            }
        }
    }
}