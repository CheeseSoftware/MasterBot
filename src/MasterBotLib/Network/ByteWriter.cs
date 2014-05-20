
namespace MasterBot.Network
{
    abstract internal class ByteWriter
    {
        public abstract long Position
        {
            get;
        }
        public abstract void Write(byte value);
        public abstract void Write(byte[] bytes, int offset, int count);
        public void Write(byte[] value)
        {
            this.Write(value, 0, value.Length);
        }
        public void WriteTagWithLength(int length, byte topPattern, byte bottomPattern)
        {
            if (length > 63 || length < 0)
            {
                byte[] bytes = LittleEndianToNetworkOrderBitConverter.GetBytes(length);
                this.WriteBottomPatternAndBytes(bottomPattern, bytes);
            }
            else
            {
                this.Write((byte)((int)topPattern | length));
            }
        }
        public void WriteBottomPatternAndBytes(byte pattern, byte[] bytes)
        {
            int num = 0;
            if (bytes[0] != 0)
            {
                num = 3;
            }
            else
            {
                if (bytes[1] != 0)
                {
                    num = 2;
                }
                else
                {
                    if (bytes[2] != 0)
                    {
                        num = 1;
                    }
                }
            }
            this.Write((byte)((int)pattern | num));
            this.Write(bytes, bytes.Length - num - 1, num + 1);
        }
        public void WriteLongPattern(byte shortPattern, byte longPattern, byte[] bytes)
        {
            int num = 0;
            for (int num2 = 0; num2 != 7; num2++)
            {
                if (bytes[num2] != 0)
                {
                    num = 7 - num2;
                    break;
                }
            }
            if (num > 3)
            {
                this.Write((byte)((int)longPattern | num - 4));
            }
            else
            {
                this.Write((byte)((int)shortPattern | num));
            }
            this.Write(bytes, bytes.Length - num - 1, num + 1);
        }
    }
}