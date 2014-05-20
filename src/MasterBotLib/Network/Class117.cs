
using System.IO;
namespace MasterBot.Network
{
    public class Class117
    {
        private MemoryStream memoryStream_0;
        public Class117()
        {
            this.memoryStream_0 = new MemoryStream();
        }
        public byte[] method_0()
        {
            this.memoryStream_0.Flush();
            return this.memoryStream_0.ToArray();
        }
        public void method_1(byte byte_0)
        {
            this.memoryStream_0.WriteByte(byte_0);
        }
        public void method_2(byte[] byte_0)
        {
            this.memoryStream_0.Write(byte_0, 0, byte_0.Length);
        }
        public void method_3(int int_0, byte byte_0, byte byte_1)
        {
            if (int_0 <= 63 && int_0 >= 0)
            {
                this.memoryStream_0.WriteByte((byte)((int)byte_0 | int_0));
                return;
            }
            byte[] byte_2 = LittleEndianToNetworkOrderBitConverter.GetBytes(int_0);
            this.method_4(byte_1, byte_2);
        }
        public void method_4(byte byte_0, byte[] byte_1)
        {
            int num = 0;
            if (byte_1[0] != 0)
            {
                num = 3;
            }
            else
            {
                if (byte_1[1] != 0)
                {
                    num = 2;
                }
                else
                {
                    if (byte_1[2] != 0)
                    {
                        num = 1;
                    }
                }
            }
            this.method_1((byte)((int)byte_0 | num));
            this.memoryStream_0.Write(byte_1, byte_1.Length - num - 1, num + 1);
        }
        public void method_5(byte byte_0, byte byte_1, byte[] byte_2)
        {
            int num = 0;
            for (int num2 = 0; num2 != 7; num2++)
            {
                if (byte_2[num2] != 0)
                {
                    num = 7 - num2;

                    if (num > 3)
                    {
                        this.memoryStream_0.WriteByte((byte)((int)byte_1 | num - 4));
                    }
                    else
                    {
                        this.memoryStream_0.WriteByte((byte)((int)byte_0 | num));
                    }
                    this.memoryStream_0.Write(byte_2, byte_2.Length - num - 1, num + 1);
                    return;
                }
            }
        }
    }
}