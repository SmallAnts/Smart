using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Smart.Core.Utilites
{
    /// <summary>
    /// 加解密工具类
    /// </summary>
    public static class CryptoUtility
    {
        #region 散列加密算法

        /// <summary>
        /// MD5 加密静态方法
        /// </summary>
        /// <param name="encryptString">待加密的明文</param>
        /// <returns>加密后的密文</returns>
        public static string MD5Encrypt(string encryptString)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(encryptString));
                return BitConverter.ToString(bytes).Replace("-", "");
            }
        }

        /// <summary>
        /// SHA256 散列算法加密
        /// </summary>
        /// <param name="data">要加密的内容</param>
        /// <returns></returns>
        public static string SHA256Encrypt(string data)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                byte[] sha256Data = sha256.ComputeHash(buffer);
                sha256.Clear();
                return BitConverter.ToString(sha256Data).Replace("-", "").ToLower();
            }
        }

        #endregion

        #region 对称加密算法

        /// <summary>
        /// DES 加密(数据加密标准，速度较快，适用于加密大量数据的场合）
        /// </summary>
        /// <param name="encryptString">待加密的明文</param>
        /// <param name="encryptKey">加密的密钥,必须为8位</param>
        /// <returns>加密后的密文</returns>
        public static string DESEncrypt(string encryptString, string encryptKey)
        {
            if (string.IsNullOrEmpty(encryptString))
            {
                throw (new Exception("明文不得为空！"));
            }
            if (string.IsNullOrEmpty(encryptKey))
            {
                throw (new Exception("密钥不得为空！"));
            }
            if (encryptKey.Length != 8)
            {
                throw new Exception("密钥必须为8位");
            }
            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            string m_strEncrypt = "";
            var m_DESProvider = new DESCryptoServiceProvider();

            try
            {
                byte[] m_btencryptString = Encoding.Default.GetBytes(encryptString);
                using (var m_stream = new MemoryStream())
                {
                    using (var m_cstream = new CryptoStream(m_stream, m_DESProvider.CreateEncryptor(Encoding.Default.GetBytes(encryptKey), m_btIV), CryptoStreamMode.Write))
                    {
                        m_cstream.Write(m_btencryptString, 0, m_btencryptString.Length);
                        m_cstream.FlushFinalBlock();
                        m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_DESProvider.Clear();
            }

            return m_strEncrypt;
        }
        /// <summary>
        /// DES 解密(数据加密标准，速度较快，适用于加密大量数据的场合）
        /// </summary>
        /// <param name="DecryptString">待解密的密文</param>
        /// <param name="DecryptKey">解密的密钥,必须为8位</param>
        /// <returns>解密后的明文</returns>
        public static string DESDecrypt(string DecryptString, string DecryptKey)
        {
            if (string.IsNullOrEmpty(DecryptString))
            {
                throw (new Exception("密文不得为空！"));
            }
            if (string.IsNullOrEmpty(DecryptKey))
            {
                throw (new Exception("密钥不得为空！"));
            }
            if (DecryptKey.Length != 8)
            {
                throw new Exception("密钥必须为8位");
            }
            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            string m_strDecrypt = "";

            var m_DESProvider = new DESCryptoServiceProvider();

            try
            {
                byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);
                using (var m_stream = new MemoryStream())
                {
                    using (var m_cstream = new CryptoStream(m_stream, m_DESProvider.CreateDecryptor(Encoding.Default.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write))
                    {
                        m_cstream.Write(m_btDecryptString, 0, m_btDecryptString.Length);
                        m_cstream.FlushFinalBlock();
                        m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_DESProvider.Clear();
            }

            return m_strDecrypt;
        }

        /// <summary>
        /// RC2 加密(用变长密钥对大量数据进行加密)
        /// </summary>
        /// <param name="encryptString">待加密密文</param>
        /// <param name="encryptKey">加密密钥</param>
        /// <returns>returns</returns>
        public static string RC2Encrypt(string encryptString, string encryptKey)
        {
            if (string.IsNullOrEmpty(encryptString)) { throw (new Exception("密文不得为空")); }
            if (string.IsNullOrEmpty(encryptKey)) { throw (new Exception("密钥不得为空")); }
            if (encryptKey.Length < 5 || encryptKey.Length > 16) { throw (new Exception("密钥必须为5-16位")); }

            string m_strEncrypt = "";
            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            RC2CryptoServiceProvider m_RC2Provider = new RC2CryptoServiceProvider();

            try
            {
                byte[] m_btencryptString = Encoding.Default.GetBytes(encryptString);

                using (var m_stream = new MemoryStream())
                {
                    using (var m_cstream = new CryptoStream(m_stream, m_RC2Provider.CreateEncryptor(Encoding.Default.GetBytes(encryptKey), m_btIV), CryptoStreamMode.Write))
                    {
                        m_cstream.Write(m_btencryptString, 0, m_btencryptString.Length);
                        m_cstream.FlushFinalBlock();
                        m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_RC2Provider.Clear();
            }

            return m_strEncrypt;
        }
        /// <summary>
        /// RC2 解密(用变长密钥对大量数据进行加密)
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="DecryptKey">解密密钥</param>
        /// <returns>returns</returns>
        public static string RC2Decrypt(string DecryptString, string DecryptKey)
        {
            if (string.IsNullOrEmpty(DecryptString)) { throw (new Exception("密文不得为空")); }
            if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }
            if (DecryptKey.Length < 5 || DecryptKey.Length > 16) { throw (new Exception("密钥必须为5-16位")); }

            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            string m_strDecrypt = "";
            RC2CryptoServiceProvider m_RC2Provider = new RC2CryptoServiceProvider();

            try
            {
                byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);

                using (var m_stream = new MemoryStream())
                {
                    using (var m_cstream = new CryptoStream(m_stream, m_RC2Provider.CreateDecryptor(Encoding.Default.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write))
                    {
                        m_cstream.Write(m_btDecryptString, 0, m_btDecryptString.Length);
                        m_cstream.FlushFinalBlock();
                        m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_RC2Provider.Clear();
            }

            return m_strDecrypt;
        }

        /// <summary>
        /// 3DES 加密(基于DES，对一块数据用三个不同的密钥进行三次加密，强度更高)
        /// </summary>
        /// <param name="encryptString">待加密密文</param>
        /// <param name="encryptKey1">密钥一</param>
        /// <param name="encryptKey2">密钥二</param>
        /// <param name="encryptKey3">密钥三</param>
        /// <returns>returns</returns>
        public static string DES3Encrypt(string encryptString, string encryptKey1, string encryptKey2, string encryptKey3)
        {
            string m_strEncrypt = "";
            try
            {
                m_strEncrypt = DESEncrypt(encryptString, encryptKey3);
                m_strEncrypt = DESEncrypt(m_strEncrypt, encryptKey2);
                m_strEncrypt = DESEncrypt(m_strEncrypt, encryptKey1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return m_strEncrypt;
        }
        /// <summary>
        /// 3DES 解密(基于DES，对一块数据用三个不同的密钥进行三次加密，强度更高)
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="DecryptKey1">密钥一</param>
        /// <param name="DecryptKey2">密钥二</param>
        /// <param name="DecryptKey3">密钥三</param>
        /// <returns>returns</returns>
        public static string DES3Decrypt(string DecryptString, string DecryptKey1, string DecryptKey2, string DecryptKey3)
        {
            string m_strDecrypt = "";
            try
            {
                m_strDecrypt = DESDecrypt(DecryptString, DecryptKey1);
                m_strDecrypt = DESDecrypt(m_strDecrypt, DecryptKey2);
                m_strDecrypt = DESDecrypt(m_strDecrypt, DecryptKey3);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return m_strDecrypt;
        }

        /// <summary>
        /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="encryptString">待加密密文</param>
        /// <param name="encryptKey">加密密钥</param>
        /// <returns></returns>
        public static string AESEncrypt(string encryptString, string encryptKey)
        {
            if (string.IsNullOrEmpty(encryptString)) { throw (new Exception("密文不得为空")); }
            if (string.IsNullOrEmpty(encryptKey)) { throw (new Exception("密钥不得为空")); }
            else if (encryptKey.Length < 8) encryptKey = encryptKey.PadRight(8, ' ');
            string m_strEncrypt = "";
            byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
            Rijndael m_AESProvider = Rijndael.Create();

            try
            {
                byte[] m_btencryptString = Encoding.Default.GetBytes(encryptString);

                using (var m_stream = new MemoryStream())
                {
                    using (var m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateEncryptor(Encoding.Default.GetBytes(encryptKey), m_btIV), CryptoStreamMode.Write))
                    {
                        m_csstream.Write(m_btencryptString, 0, m_btencryptString.Length);
                        m_csstream.FlushFinalBlock();
                        m_strEncrypt = Convert.ToBase64String(m_stream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_AESProvider.Clear();
            }

            return m_strEncrypt;
        }
        /// <summary>
        /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="DecryptKey">解密密钥</param>
        /// <returns></returns>
        public static string AESDecrypt(string DecryptString, string DecryptKey)
        {
            if (string.IsNullOrEmpty(DecryptString)) { throw (new Exception("密文不得为空")); }
            if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }
            else if (DecryptKey.Length < 8) DecryptKey = DecryptKey.PadRight(8, ' ');

            string m_strDecrypt = "";
            byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
            Rijndael m_AESProvider = Rijndael.Create();

            try
            {
                byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);

                using (var m_stream = new MemoryStream())
                {
                    using (var m_csstream = new CryptoStream(m_stream, m_AESProvider.CreateDecryptor(Encoding.Default.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write))
                    {
                        m_csstream.Write(m_btDecryptString, 0, m_btDecryptString.Length);
                        m_csstream.FlushFinalBlock();
                        m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                m_AESProvider.Clear();
            }

            return m_strDecrypt;
        }

        #endregion

        #region 非对称加密算法

        /// <summary>
        /// RSA 非对称加密
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string RSAEncrypt(string data, string key)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentException("加密数据不能为空！");
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("加密时密钥不能为空！");
            using (var rsa = new RSACryptoServiceProvider())
            {
                var maxBlockSize = rsa.KeySize / 8 - 11; //加密块最大长度限制
                var encryptBuffer = Encoding.UTF8.GetBytes(data);
                rsa.FromXmlString(key);

                if (encryptBuffer.Length <= maxBlockSize)
                    return Convert.ToBase64String(rsa.Encrypt(encryptBuffer, false), Base64FormattingOptions.None);

                using (var sourceStream = new MemoryStream(encryptBuffer))
                using (var targetStream = new MemoryStream())
                {
                    var buffer = new Byte[maxBlockSize];
                    int blockSize = sourceStream.Read(buffer, 0, maxBlockSize);
                    while (blockSize > 0)
                    {
                        var toEncrypt = new Byte[blockSize];
                        Array.Copy(buffer, 0, toEncrypt, 0, blockSize);
                        var cryptData = rsa.Encrypt(toEncrypt, false);
                        targetStream.Write(cryptData, 0, cryptData.Length);
                        blockSize = sourceStream.Read(buffer, 0, maxBlockSize);
                    }
                    return Convert.ToBase64String(targetStream.ToArray(), Base64FormattingOptions.None);
                }
            }
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string RSADecrypt(string data, string key)
        {
            if (string.IsNullOrEmpty(data)) throw new ArgumentException("解密数据不能为空！");
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("解密时密钥不能为空！");

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(key);
                int maxBlockSize = rsa.KeySize / 8; //解密块最大长度限制
                var decryptBuffer = Convert.FromBase64String(data);
                if (decryptBuffer.Length <= maxBlockSize)
                    return Encoding.UTF8.GetString(rsa.Decrypt(decryptBuffer, false));

                using (MemoryStream sourceStream = new MemoryStream(decryptBuffer))
                using (MemoryStream targetStream = new MemoryStream())
                {
                    var buffer = new Byte[maxBlockSize];
                    int blockSize = sourceStream.Read(buffer, 0, maxBlockSize);
                    while (blockSize > 0)
                    {
                        var toDecrypt = new Byte[blockSize];
                        Array.Copy(buffer, 0, toDecrypt, 0, blockSize);
                        var cryptData = rsa.Decrypt(toDecrypt, false);
                        targetStream.Write(cryptData, 0, cryptData.Length);
                        blockSize = sourceStream.Read(buffer, 0, maxBlockSize);
                    }
                    return Encoding.UTF8.GetString(targetStream.ToArray());
                }
            }
        }

        /// <summary>
        /// 获取RSA私钥和公钥，0:私钥, 1:公钥
        /// </summary>
        /// <returns>0:私钥, 1:公钥</returns>
        public static string[] GetRSAKeys()
        {
            var keys = new string[2];
            using (var rsa = new RSACryptoServiceProvider())
            {
                keys[0] = rsa.ToXmlString(true); //私钥
                keys[1] = rsa.ToXmlString(false); //公钥
            }
            return keys;
        }

        #endregion
    }
}
