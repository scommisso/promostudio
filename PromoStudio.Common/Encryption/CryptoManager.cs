using System;
using System.Security.Cryptography;
using System.Text;

namespace PromoStudio.Common.Encryption
{
    public class CryptoManager : ICryptoManager
    {
        public CryptoManager()
        {
        }

        public string EncryptString(string value, string keys)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");
            if (string.IsNullOrEmpty(keys))
                throw new ArgumentNullException("keys");

            string result = null;
            using (var aesAlg = new AesManaged())
            {
                SetupCipher(aesAlg, keys);
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                var decrypted = Encoding.UTF8.GetBytes(value);
                var encrypted = encryptor.TransformFinalBlock(decrypted, 0, decrypted.Length);
                result = Convert.ToBase64String(encrypted);
            }

            return result;
        }

        public string DecryptString(string encryptedValue, string keys)
        {
            if (string.IsNullOrEmpty(encryptedValue))
                throw new ArgumentNullException("encryptedValue");
            if (string.IsNullOrEmpty(keys))
                throw new ArgumentNullException("keys");

            string result = null;
            using (var aesAlg = new AesManaged())
            {
                SetupCipher(aesAlg, keys);
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                var encrypted = Convert.FromBase64String(encryptedValue);
                var decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
                result = Encoding.UTF8.GetString(decrypted);
            }

            return result;
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
