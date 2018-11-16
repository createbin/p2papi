using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZFCTAPI.Core.Security
{
    public class CryptHelper
    {
        private static string EncryptStr = "tong1234";

        public static string Encrypt(string data)
        {
            var byKey = Encoding.ASCII.GetBytes(EncryptStr);
            var byIV = Encoding.ASCII.GetBytes(EncryptStr);

            var cryptoProvider = new DESCryptoServiceProvider();
            var ms = new MemoryStream();
            var cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey,

                byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(data);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

        }

        public static string Decrypt(string data)
        {
            var byKey = Encoding.ASCII.GetBytes(EncryptStr);
            var byIV = Encoding.ASCII.GetBytes(EncryptStr);
            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }
            var cryptoProvider = new DESCryptoServiceProvider();
            var ms = new MemoryStream(byEnc);
            var cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey,byIV), CryptoStreamMode.Read);
            var sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
    }
}
