using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;

namespace MasterBot.Network
{
    public class PIOCon
    {
        private Socket sock;
        private byte[] bytes = new byte[UInt16.MaxValue];
        private List<Message> messageBuffer = new List<Message>();
        public List<Socket> sockli = new List<Socket>();
        public event MessageHandler onMessage;
        public void Connect(string GameId, string UsernameOrEmail, string Password, string Room, string Version)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://api.playerio.com/api/400");
            req.Proxy = null;
            string request = (char)0x0A + "&" + GameId + (char)UsernameOrEmail.Length + UsernameOrEmail + (char)26 + (char)Password.Length + Password;
            req.Method = "POST"; req.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = Encoding.UTF8.GetBytes(request);
            req.ContentLength = bytes.Length;
            Stream requestStream = req.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            WebResponse response = req.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            var result = reader.ReadToEnd();
            string Token = System.Text.RegularExpressions.Regex.Match(result, @"(?<=\" + (char)0x0A + @")(.*?)(?=\" + (char)0x12 + ")").Value;
            //stream.Dispose();
            //reader.Dispose();
            var value = WebRequest.Create("http://api.playerio.com/api/27");
            value.Timeout = 15000;
            value.Method = "POST";
            value.Headers["playertoken"] = Token.Remove(0, 1);
            string request2 = "\n" + (char)Room.Length + Room + (char)18 + (char)Version.Length + Version + (char)24 + (char)1;
            value.Proxy = null;
            byte[] bytes2 = Encoding.UTF8.GetBytes(request2);
            value.ContentLength = bytes2.Length;
            Stream resps = value.GetRequestStream();
            resps.Write(bytes2, 0, bytes2.Length);
            var resp = value.GetResponse();
            Stream resps2 = resp.GetResponseStream();
            StreamReader reader2 = new StreamReader(resps2);
            string result2 = reader2.ReadToEnd();
            string Token2 = Regex.Match(result2, @"\(.*?)\").Value.Remove(0, 3);
            string IP = "127.0.0.1";
            try
            {
                string x = result2;
                string[] x1 = x.Split('\n');
                x = x1[x1.Length - 1];
                x = x.Remove(x.Length - 4).Remove(0, 1);
                IP = x;
            }
            catch
            {
                Console.Write("Failed to grab ip please type it here:");
                IP = Console.ReadLine();
            }
            Console.WriteLine("IP: [" + IP + "]");
            Token2 = Token2.Remove(Token2.Length - 1, 1);
            Token2 = Regex.Replace(Token2, @"[^\u0000-\u007F]", string.Empty);
            Token2 = Token2.Replace("\n", "").Replace("\r", "");
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(IP, 8184);
            Message _join = new Message("join", new object[] { Token2 });
            Thread.Sleep(500);
            sock.Send(new byte[] { (byte)0 });
            Thread.Sleep(500);
            sock.Send(method_3(_join));
            Thread.Sleep(500);
            Thread th = new Thread(Reciever);
            th.Start();
        }

        public void Send(string type, params object[] args)
        {
            Send(new Message(type, args));
        }

        public void Send(Message m)
        {
            if (m.Type == "b" || m.Type == ".hG")
            {
                messageBuffer.Add(m);
                if (messageBuffer.Count >= 2)
                {
                    Send(messageBuffer.ToArray());
                    messageBuffer.Clear();
                }
            }
            else
                sock.Send(method_3(m));
            Thread.Sleep(40);
        }

        public void Send(Message[] m)
        {
            List<Byte[]> arrays = new List<byte[]>();
            foreach (Message m2 in m) arrays.Add(method_3(m2));
            sock.Send(JoinArrays(arrays));
            //Console.WriteLine("Sent Arrays");
        }
        /*
        public void Send(string type, params object[] parameters)
        {
            sock.Send(method_3(type, parameters)));
        }*/
        private byte[] JoinArrays(IEnumerable<byte[]> arrays)
        {
            int offset = 0;
            byte[] fullArray = new byte[arrays.Sum(a => a.Length)];
            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, fullArray, offset, array.Length);
                offset += array.Length;
            }
            return fullArray;
        }
        public void Reciever()
        {
            try
            {
                while (sock.Connected)
                {
                    byte[] b = new byte[UInt16.MaxValue];
                    int k = sock.Receive(b);
                    //byte[] b2 = new byte[k];
                    //Array.Copy(b, b2, k);
                    Message m = new MessageSerializer().Deserialize(b, 0, k);
                    onMessage.Invoke(null, m);
                    //Console.WriteLine(m);
                }
            }
            catch
            {
                Console.WriteLine("[ERROR] Listener died");
                Reciever();
            }
        }
        public void Disconnect(bool doTrue = false)
        {
            sock.Disconnect(doTrue);
        }

        public static byte[] method_3(Message myMessage)
        {
            Class117 @class = new Class117();
            @class.method_3((int)myMessage.Count, 128, 4);
            byte[] array = Encoding.UTF8.GetBytes(myMessage.Type);
            @class.method_3(array.Length, 192, 12);
            @class.method_2(array);
            for (uint num = 0u; num != myMessage.Count; num += 1u)
            {
                object obj = myMessage[num];
                switch (myMessage.types[(int)((UIntPtr)num)])
                {
                    case 0:
                        @class.method_3((int)obj, 128, 4);
                        break;
                    case 1:
                        @class.method_4(8, LittleEndianToNetworkOrderBitConverter.GetBytes((uint)obj));
                        break;
                    case 2:
                        @class.method_5(48, 52, LittleEndianToNetworkOrderBitConverter.GetBytes((long)obj));
                        break;
                    case 3:
                        @class.method_5(56, 60, LittleEndianToNetworkOrderBitConverter.GetBytes((ulong)obj));
                        break;
                    case 4:
                        @class.method_1(3);
                        @class.method_2(LittleEndianToNetworkOrderBitConverter.GetBytes((double)obj));
                        break;
                    case 5:
                        @class.method_1(2);
                        @class.method_2(LittleEndianToNetworkOrderBitConverter.GetBytes((float)obj));
                        break;
                    case 6:
                        array = Encoding.UTF8.GetBytes((string)obj);
                        @class.method_3(array.Length, 192, 12);
                        @class.method_2(array);
                        break;
                    case 7:
                        array = (byte[])obj;
                        @class.method_3(array.Length, 64, 16);
                        @class.method_2(array);
                        break;
                    case 8:
                        @class.method_1((byte)((bool)obj ? 1 : 0));
                        break;
                }
            }
            return @class.method_0();
        }
		
        public delegate void MessageHandler(object sender, Message m);
    }
}
