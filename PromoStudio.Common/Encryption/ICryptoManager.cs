namespace PromoStudio.Common.Encryption
{
    public interface ICryptoManager
    {
        string EncryptString(string value, string keys);
        string EncryptObject(object value, string keys);
        byte[] EncryptBytes(byte[] data, string keys);
        string DecryptString(string encryptedValue, string keys);
        T DecryptObject<T>(string encryptedValue, string keys);
        byte[] DecryptBytes(byte[] data, string keys);
        string HashString(string value);
        byte[] HashBytes(byte[] data);
        string HashObject(object value);
    }
}