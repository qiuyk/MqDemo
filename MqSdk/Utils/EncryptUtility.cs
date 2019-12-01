using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace MqSdk.Utils
{
    public class EncryptUtility
    {
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="code">加密字符串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DesEncrypt(string code, string key = "cptbtxjp")
        {
            string iv = Reverse(key);
            return DesEncrypt(code, key, iv);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="code">加密字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <returns></returns>
        public static string DesEncrypt(string code, string key, string iv)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(code);
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(iv);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ms.Dispose();
            cs.Dispose();
            //ret.ToString();
            return ret.ToString();
        }


        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="code">解密字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DesDecrypt(string code, string key = "cptbtxjp")
        {
            string iv = Reverse(key);
            return DesDecrypt(code, key, iv);
        }


        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="code">解密字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <returns></returns>
        public static string DesDecrypt(string code, string key, string iv)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[code.Length / 2];
            for (int x = 0; x < code.Length / 2; x++)
            {
                int i = (Convert.ToInt32(code.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(iv);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            cs.Dispose();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        public static string Reverse(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return key;
            }
            char[] charArray = key.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
