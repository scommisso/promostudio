using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PromoStudio.Common.Serialization
{
    public class SerializationManager : ISerializationManager
    {
        public TData Deserialize<TData>(byte[] data)
        {
            if (data == null)
            {
                return default(TData);
            }
            using (var stream = new MemoryStream(data))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (TData) formatter.Deserialize(stream);
            }
        }

        public TData DeserializeFromString<TData>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(TData);
            }
            return Deserialize<TData>(Convert.FromBase64String(value));
        }

        public byte[] Serialize(object value)
        {
            if (value == null)
            {
                return null;
            }
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, value);
                stream.Flush();
                stream.Position = 0;
                return stream.ToArray();
            }
        }

        public string SerializeToString(object value)
        {
            if (value == null)
            {
                return null;
            }
            return Convert.ToBase64String(Serialize(value));
        }
    }
}