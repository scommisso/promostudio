namespace PromoStudio.Common.Serialization
{
    public interface ISerializationManager
    {
        TData Deserialize<TData>(byte[] data);
        TData DeserializeFromString<TData>(string value);
        byte[] Serialize(object value);
        string SerializeToString(object value);
    }
}