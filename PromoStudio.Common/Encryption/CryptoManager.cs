using System;
using System.Security.Cryptography;
using System.Text;
using PromoStudio.Common.Serialization;

namespace PromoStudio.Common.Encryption
{
    public class CryptoManager : ICryptoManager
    {
        private readonly ISerializationManager _serializationManager;

        public CryptoManager(ISerializationManager serializationManager)
        {
            _serializationManager = serializationManager;
        }

        public string EncryptString(string value, string keys)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");
            if (string.IsNullOrEmpty(keys))
                throw new ArgumentNullException("keys");

            var decrypted = Encoding.UTF8.GetBytes(value);
            var encrypted = EncryptBytes(decrypted, keys);
            return Convert.ToBase64String(encrypted);
        }

        public string EncryptObject(object value, string keys)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (string.IsNullOrEmpty(keys))
                throw new ArgumentNullException("keys");

            var serialized = _serializationManager.Serialize(value);
            var encrypted = EncryptBytes(serialized, keys);
            return Convert.ToBase64String(encrypted);
        }

        public byte[] EncryptBytes(byte[] data, string keys)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (string.IsNullOrEmpty(keys))
                throw new ArgumentNullException("keys");

            using (var aesAlg = new AesManaged())
            {
                SetupCipher(aesAlg, keys);
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                var encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);
                return encrypted;
            }
        }

        public string DecryptString(string encryptedValue, string keys)
        {
            if (string.IsNullOrEmpty(encryptedValue))
                throw new ArgumentNullException("encryptedValue");
            if (string.IsNullOrEmpty(keys))
                throw new ArgumentNullException("keys");

            var encrypted = Convert.FromBase64String(encryptedValue);
            var decrypted = DecryptBytes(encrypted, keys);
            return Encoding.UTF8.GetString(decrypted);
        }

        public T DecryptObject<T>(string encryptedValue, string keys)
        {
            if (string.IsNullOrEmpty(encryptedValue))
                throw new ArgumentNullException("encryptedValue");
            if (string.IsNullOrEmpty(keys))
                throw new ArgumentNullException("keys");

            var encrypted = Convert.FromBase64String(encryptedValue);
            var decrypted = DecryptBytes(encrypted, keys);
            return _serializationManager.Deserialize<T>(decrypted);
        }

        public byte[] DecryptBytes(byte[] data, string keys)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (string.IsNullOrEmpty(keys))
                throw new ArgumentNullException("keys");

            using (var aesAlg = new AesManaged())
            {
                SetupCipher(aesAlg, keys);
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                var decrypted = decryptor.TransformFinalBlock(data, 0, data.Length);
                return decrypted;
            }
        }

        private void SetupCipher(AesManaged aesAlg, string keys)
        {
            aesAlg.KeySize = 256;
            aesAlg.BlockSize = 128;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            var keyVals = Encoding.UTF8.GetString(Convert.FromBase64String(keys)).Split(',');
            aesAlg.Key = Convert.FromBase64String(keyVals[0]);
            aesAlg.IV = Convert.FromBase64String(keyVals[1]);
        }
    }
}
