
using System;
using System.Text;
using System.Threading;

namespace MasterBot.Network
{
    [Serializable]
    public class Message
    {
        public class SerializedMessage
        {
            public readonly byte[] Array;
            public readonly int Start;
            public readonly int Count;
            public SerializedMessage(byte[] array)
            {
                this.Array = array;
                this.Start = 0;
                this.Count = array.Length;
            }
            public SerializedMessage(byte[] array, int start, int count)
            {
                this.Array = array;
                this.Start = start;
                this.Count = count;
            }
        }
        internal const byte EntryType_Integer = 0;
        internal const byte EntryType_UnsignedInteger = 1;
        internal const byte EntryType_Long = 2;
        internal const byte EntryType_UnsignedLong = 3;
        internal const byte EntryType_Double = 4;
        internal const byte EntryType_Float = 5;
        internal const byte EntryType_String = 6;
        internal const byte EntryType_ByteArray = 7;
        internal const byte EntryType_Boolean = 8;
        private string type;
        private object[] values = new object[20];
        internal byte[] types = new byte[20];
        private uint count = 0u;
        internal Message.SerializedMessage SerializedCache_Binary = null;
        internal Message.SerializedMessage SerializedCache_Websocket6455Binary = null;
        internal Message.SerializedMessage SerializedCache_WebsocketV76Base64 = null;
        public string Type
        {
            get
            {
                return this.type;
            }
        }
        public uint Count
        {
            get
            {
                return this.count;
            }
        }
        public object this[uint index]
        {
            get
            {
                return this.get<object>(index);
            }
        }
        public object[] ValueArray
        {
            get
            {
                return this.values;
            }
        }
        public byte[] TypesArray
        {
            get
            {
                return this.types;
            }
        }
        public Message(string type)
        {
            this.type = type;
        }
        public Message(string type, params object[] parameters)
        {
            this.type = type;
            this.Add(parameters);
        }
        private Message(uint count, string type, object[] values, byte[] types)
        {
            this.type = type;
            this.values = values;
            this.types = types;
            this.count = count;
        }
        public static Message Create(string type, object[] values, byte[] types, uint count)
        {
            return new Message(count, type, values, types);
        }
        public string GetString(uint index)
        {
            return this.get<string>(index);
        }
        public byte[] GetByteArray(uint index)
        {
            return this.get<byte[]>(index);
        }
        public bool GetBoolean(uint index)
        {
            return this.get<bool>(index);
        }
        public double GetDouble(uint index)
        {
            return this.getNumeric<double>(index, 4);
        }
        public float GetFloat(uint index)
        {
            return this.getNumeric<float>(index, 5);
        }
        public int GetInteger(uint index)
        {
            return this.getNumeric<int>(index, 0);
        }
        public int GetInt(uint index)
        {
            return this.getNumeric<int>(index, 0);
        }
        public uint GetUInt(uint index)
        {
            return this.getNumeric<uint>(index, 1);
        }
        public uint GetUnsignedInteger(uint index)
        {
            return this.getNumeric<uint>(index, 1);
        }
        public long GetLong(uint index)
        {
            return this.getNumeric<long>(index, 2);
        }
        public ulong GetULong(uint index)
        {
            return this.getNumeric<ulong>(index, 3);
        }
        public ulong GetUnsignedLong(uint index)
        {
            return this.getNumeric<ulong>(index, 3);
        }
        public void Add(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Cannot add null values to messages");
            }
            this.add(value, 6);
        }
        public void Add(int value)
        {
            this.add(value, 0);
        }
        public void Add(uint value)
        {
            this.add(value, 1);
        }
        public void Add(long value)
        {
            this.add(value, 2);
        }
        public void Add(ulong value)
        {
            this.add(value, 3);
        }
        public void Add(byte[] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Cannot add null values to messages");
            }
            this.add(value, 7);
        }
        public void Add(float value)
        {
            this.add(value, 5);
        }
        public void Add(double value)
        {
            this.add(value, 4);
        }
        public void Add(bool value)
        {
            this.add(value, 8);
        }
        public void Add(params object[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                object obj = parameters[i];
                if (obj is string)
                {
                    if (obj == null)
                    {
                        throw new ArgumentNullException("Cannot add null values to messages");
                    }
                    this.add(obj, 6);
                }
                else
                {
                    if (obj is int)
                    {
                        this.add(obj, 0);
                    }
                    else
                    {
                        if (obj is bool)
                        {
                            this.add(obj, 8);
                        }
                        else
                        {
                            if (obj is float)
                            {
                                this.add(obj, 5);
                            }
                            else
                            {
                                if (obj is double)
                                {
                                    this.add(obj, 4);
                                }
                                else
                                {
                                    if (obj is byte[])
                                    {
                                        if (obj == null)
                                        {
                                            throw new ArgumentNullException("Cannot add null values to messages");
                                        }
                                        this.add(obj, 7);
                                    }
                                    else
                                    {
                                        if (obj is uint)
                                        {
                                            this.add(obj, 1);
                                        }
                                        else
                                        {
                                            if (obj is long)
                                            {
                                                this.add(obj, 2);
                                            }
                                            else
                                            {
                                                if (!(obj is ulong))
                                                {
                                                    throw new InvalidOperationException("Message only supports objects of types: string, integer, boolean, byte[], float, double, uint, long & ulong. Type '" + obj.GetType().FullName + "' is not supported.");
                                                }
                                                this.add(obj, 3);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void add(object value, byte type)
        {
            if ((long)this.values.Length == (long)((ulong)this.count))
            {
                object[] destinationArray = new object[this.values.Length * 2];
                byte[] destinationArray2 = new byte[this.values.Length * 2];
                Array.Copy(this.values, destinationArray, this.values.Length);
                Array.Copy(this.types, destinationArray2, this.types.Length);
                this.values = destinationArray;
                this.types = destinationArray2;
            }
            this.values[(int)((UIntPtr)this.count)] = value;
            this.types[(int)((UIntPtr)this.count)] = type;
            this.count += 1u;
            this.invalidateBinarySerializedState();
        }
        private void invalidateBinarySerializedState()
        {
            this.SerializedCache_Binary = null;
            this.SerializedCache_Websocket6455Binary = null;
            this.SerializedCache_WebsocketV76Base64 = null;
        }
        private T get<T>(uint index)
        {
            if (this.count > index)
            {
                try
                {
                    return (T)((object)this.values[(int)((UIntPtr)index)]);
                }
                catch (Exception)
                {
                    throw new InvalidCastException(string.Concat(new object[]
					{
						"Value at index:",
						index,
						" is not ",
						typeof(T).FullName,
						". It's a ",
						this.values[(int)((UIntPtr)index)].GetType().FullName,
						". Value is: ",
						this.values[(int)((UIntPtr)index)]
					}));
                }
            }
            throw new IndexOutOfRangeException(string.Concat(new object[]
			{
				"this message (",
				this.Type,
				") only has ",
				this.count,
				" entries."
			}));
        }
        public T getNumeric<T>(uint index, byte expectedType)
        {
            if (this.count > index)
            {
                byte b = this.types[(int)((UIntPtr)index)];
                T result;
                if (b != expectedType)
                {
                    if (b < 6)
                    {
                        try
                        {
                            result = (T)((object)Convert.ChangeType(this.values[(int)((UIntPtr)index)], typeof(T), Thread.CurrentThread.CurrentCulture));
                            return result;
                        }
                        catch (Exception innerException)
                        {
                            throw new InvalidCastException(string.Concat(new object[]
							{
								"Value at index ",
								index,
								" can't be read as a ",
								typeof(T).FullName,
								", as it's a ",
								this.values[(int)((UIntPtr)index)].GetType().FullName
							}), innerException);
                        }
                    }
                    throw new InvalidCastException(string.Concat(new object[]
					{
						"Value at index:",
						index,
						" is not ",
						typeof(T).FullName,
						". It's a ",
						this.values[(int)((UIntPtr)index)].GetType().FullName
					}));
                }
                result = (T)((object)this.values[(int)((UIntPtr)index)]);
                return result;
            }
            throw new IndexOutOfRangeException(string.Concat(new object[]
			{
				"this message (",
				this.Type,
				") only has ",
				this.count,
				" entries."
			}));
        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.AppendLine(string.Concat(new object[]
			{
				"  msg.Type= ",
				this.Type,
				", ",
				this.count,
				" entries"
			}));
            int num = 0;
            while ((long)num != (long)((ulong)this.Count))
            {
                stringBuilder.AppendLine(string.Concat(new object[]
				{
					"  msg[",
					num,
					"] = ",
					this.values[num],
					"  (",
					this.types[num],
					")"
				}));
                num++;
            }
            return stringBuilder.ToString();
        }
    }
}