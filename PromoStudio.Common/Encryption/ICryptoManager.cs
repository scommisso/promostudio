namespace PromoStudio.Common.Encryption
{
    public interface ICryptoManager
    {
        string EncryptString(string value, string keys);
        string DecryptString(string encryptedValue, string keys);
    }
}