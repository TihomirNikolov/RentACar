using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Client.Helpers
{
    public class CryptHelper
    {
        #region Declarations

        private static readonly object lockObj = new object();
        private static CryptHelper instance;

        #endregion

        #region Constructors

        private CryptHelper()
        {

        }

        #endregion

        #region Properties

        public static CryptHelper Instance
        {
            get
            {
                lock (lockObj)
                {
                    return instance ?? (instance = new CryptHelper());
                }
            }
        }

        private string Key => "b14ca5898a4e4133bbce2ea2315a1916";

        #endregion

        #region Methods

        public string Encrypt(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string Decrypt(string cipherText)
        {
            cipherText = cipherText ?? "";
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        #endregion
    }
}
