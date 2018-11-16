using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZFCTAPI.Core.Security
{
    public class RsaHelper
    {
        public static string tpublickkey = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCY6qVctIzU6GDUGpVodY3LVjFmqOjvd/HAMCPzHYeFmUR9+Fw/u5J5+Ib/9Yk2ZDk5lXqIGHUR8Un/cQ5X50ymGGe0s9Ha4tprCAWo8O0D6t+sPFCDIMR65HK2j/q/V085v9G3XUSZJmxcLRGTkj/nk/1AtRyzTOIW4rj4rR6JNwIDAQAB";
        public static string tprivateKey = @"MIICXAIBAAKBgQCY6qVctIzU6GDUGpVodY3LVjFmqOjvd/HAMCPzHYeFmUR9+Fw/u5J5+Ib/9Yk2ZDk5lXqIGHUR8Un/cQ5X50ymGGe0s9Ha4tprCAWo8O0D6t+sPFCDIMR65HK2j/q/V085v9G3XUSZJmxcLRGTkj/nk/1AtRyzTOIW4rj4rR6JNwIDAQABAoGADoqBnwcsPTK7wm+ktYI4MZXIrNJzFBJ70qLBSGUOdg3VDYWIgCb+DssrSzu2CI4PQwKL3FU3DLW9K6U8j+9Wwm790JPDm21t4ok5gImGZLhWzVfnrs9wderA4R8aJmJsDSsg2fqXEOIuOuT8hYT9YxvDeYq0dT1YfMs9L9dFxwECQQDIaM8wvSCvNurQBG7yy3td0vUVcQ9wL+1p13OWNvpfkRBIuWq9o0lRUN44kjLurlxgZuuyg8P8wXKAD/GFysknAkEAw1VXzyaBoqkWxCmTQTb3WaOmcev+E9r8wFMiHR87FeBbHxq38WpJ3bAmvKeyE4LAZEg6y3hnMAP2t7FyGkepcQJBALpHUW5IHT0/mnrH5CihooXPNCc988+ffNnIfZO+fnKd77jpP6INSHTg6e1DVzwrrYmdoK9Jcr5ZQQhvyVtG5jkCQCbZ06Q9CW276e5MQqCm+R2YIiPoh1mVpc3u+wsFyi+4twcBvnJC7LwuLBY72Hxu9is7/o23vfZb43d30LUPadECQCdQFeg4Qezi9SEQbjqoYnBzT+wF4slyjJvuyZoOzS5gjiAALTMEdzq1JaEJ96pCRmqqnUGH3r/5u08gzfQzAyc=";
        public static string EncryptStr = "tong1234";
        /// <summary>
        /// RSA 非对称加解密算法
        /// </summary>

        private static int MAXENCRYPTSIZE = 35;
        private static int MAXDECRYPTSIZE = 172;


        /// <summary>
        /// RSA公钥加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Encrypt(string content)
        {
            RSA rsa = CreateRsaFromPublicKey(tpublickkey);
            StringBuilder sBulider = new StringBuilder();
            string s;
            int len = content.Length;
            int nowLen = 0;
            string returnstr;

            try
            {
                while (nowLen < len)
                {
                    if (len - nowLen > MAXENCRYPTSIZE)
                    {
                        s = content.Substring(nowLen, MAXENCRYPTSIZE);
                        nowLen += MAXENCRYPTSIZE;
                        returnstr = Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(s), RSAEncryptionPadding.Pkcs1));
                    }
                    else
                    {
                        s = content.Substring(nowLen, len - nowLen);
                        nowLen = len;
                        returnstr = Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(s), RSAEncryptionPadding.Pkcs1));
                    }
                    sBulider.Append(returnstr);
                }
            }
            catch
            {

            }
            return sBulider.ToString();
        }


        /// <summary>  
        /// RSA私钥解密  
        /// </summary>  
        /// <param name="encryptData">经过Base64编码的密文</param>  
        /// <returns>RSA解密后的数据</returns>  
        public static string Decrypt(string encryptData)
        {
            RSA rsa = CreateRsaFromPrivateKey(tprivateKey);
            StringBuilder sBulider = new StringBuilder();
            string s;
            int len = encryptData.Length;
            int nowLen = 0;
            string returnstr;
            byte[] bEncrypt;
            try
            {
                while (nowLen < len)
                {
                    if (len - nowLen > MAXDECRYPTSIZE)
                    {
                        s = encryptData.Substring(nowLen, MAXDECRYPTSIZE);
                        nowLen += MAXDECRYPTSIZE;
                        bEncrypt = Convert.FromBase64String(s);
                        returnstr = Encoding.UTF8.GetString(rsa.Decrypt(bEncrypt, RSAEncryptionPadding.Pkcs1));
                    }
                    else
                    {
                        s = encryptData.Substring(nowLen, len - nowLen);
                        nowLen = len;
                        bEncrypt = Convert.FromBase64String(s);
                        returnstr = Encoding.UTF8.GetString(rsa.Decrypt(bEncrypt, RSAEncryptionPadding.Pkcs1));
                    }
                    sBulider.Append(returnstr);
                }
            }
            catch
            {

            }
            return sBulider.ToString();
        }



        #region 创建公钥

        private static RSA CreateRsaFromPublicKey(string publicKeyString)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] x509key;
            byte[] seq = new byte[15];
            int x509size;
            x509key = Convert.FromBase64String(publicKeyString);
            x509size = x509key.Length;

            using (var mem = new MemoryStream(x509key))
            {
                using (var binr = new BinaryReader(mem))
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return null;

                    seq = binr.ReadBytes(15);
                    if (!CompareBytearrays(seq, SeqOID))
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103)
                        binr.ReadByte();
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102)
                        lowbyte = binr.ReadByte();
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte();
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {
                        binr.ReadByte();
                        modsize -= 1;
                    }

                    byte[] modulus = binr.ReadBytes(modsize);

                    if (binr.ReadByte() != 0x02)
                        return null;
                    int expbytes = (int)binr.ReadByte();
                    byte[] exponent = binr.ReadBytes(expbytes);

                    var rsa = RSA.Create();
                    var rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);
                    return rsa;
                }

            }
        }


        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }


        private static RSA CreateRsaFromPrivateKey(string privateKey)
        {
            var privateKeyBits = System.Convert.FromBase64String(privateKey);
            var rsa = RSA.Create();
            var RSAparams = new RSAParameters();

            using (var binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            rsa.ImportParameters(RSAparams);
            return rsa;
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }
        #endregion
    }
}
