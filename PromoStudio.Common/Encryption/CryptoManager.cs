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

        #region Encryption

        public string EncryptString(string value, string keys)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");
            if (string.IsNullOrEmpty(keys))
                throw new ArgumentNullException("keys");

            byte[] decrypted = Encoding.UTF8.GetBytes(value);
            byte[] encrypted = EncryptBytes(decrypted, keys);
            return Convert.ToBase64String(encrypted);
        }

        public string EncryptObject(object value, string keys)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (string.IsNullOrEmpty(keys))
                throw new ArgumentNullException("keys");

            byte[] serialized = _serializationManager.Serialize(value);
            byte[] encrypted = EncryptBytes(serialized, keys);
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
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);
                return encrypted;
            }
        }

        public string DecryptString(string encryptedValue, string keys)
        {
            if (string.IsNullOrEmpty(encryptedValue))
                throw new ArgumentNullException("encryptedValue");
            if (string.IsNullOrEmpty(keys))
                throw new ArgumentNullException("keys");

            byte[] encrypted = Convert.FromBase64String(encryptedValue);
            byte[] decrypted = DecryptBytes(encrypted, keys);
            return Encoding.UTF8.GetString(decrypted);
        }

        public T DecryptObject<T>(string encryptedValue, string keys)
        {
            if (string.IsNullOrEmpty(encryptedValue))
                throw new ArgumentNullException("encryptedValue");
            if (string.IsNullOrEmpty(keys))
                throw new ArgumentNullException("keys");

            byte[] encrypted = Convert.FromBase64String(encryptedValue);
            byte[] decrypted = DecryptBytes(encrypted, keys);
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
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] decrypted = decryptor.TransformFinalBlock(data, 0, data.Length);
                return decrypted;
            }
        }

        private void SetupCipher(AesManaged aesAlg, string keys)
        {
            aesAlg.KeySize = 256;
            aesAlg.BlockSize = 128;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            string[] keyVals = Encoding.UTF8.GetString(Convert.FromBase64String(keys)).Split(',');
            aesAlg.Key = Convert.FromBase64String(keyVals[0]);
            aesAlg.IV = Convert.FromBase64String(keyVals[1]);
        }

        #endregion

        #region Hashing

        public byte[] HashBytes(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            using (var shaMAlg = new SHA256Managed())
            {
                return shaMAlg.ComputeHash(data);
            }
        }

        public string HashString(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            byte[] encoded = Encoding.UTF8.GetBytes(value);
            byte[] hashed = HashBytes(encoded);
            return Convert.ToBase64String(hashed);
        }

        public string HashObject(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            byte[] serialized = _serializationManager.Serialize(value);
            byte[] hashed = HashBytes(serialized);
            return Convert.ToBase64String(hashed);
        }

        #endregion
    }
}