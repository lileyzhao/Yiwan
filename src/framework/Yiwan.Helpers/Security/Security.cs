/// <summary>
/// ��˵����Encrypt�ӽ��ܲ�����
/// ��ϵ��ʽ��liley@foxmail.com ����:897250000
/// </summary>
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Yiwan.Helpers.Security
{
    /// <summary>
    /// Encrypt ��ժҪ˵����
    /// </summary>
    public static class Security
    {
        const string defaultKey = "Yiwanlee|liley@foxmail.com"; //ȱʡ��Կ�ַ���

        #region ʹ�� ������Կ�ַ��� ����/����string
        /// <summary>
        /// ʹ�ø�����Կ�ַ�������string
        /// </summary>
        /// <param name="original">ԭʼ����</param>
        /// <param name="key">��Կ��Ϊ����ʹ��ȱʡ��Կ��</param>
        /// <param name="encoding">�ַ����뷽��</param>
        /// <returns>����</returns>
        public static string Encrypt(string original, string key = "")
        {
            key = key.Length < 9 ? defaultKey : key;//���ٳ���Ϊ9/Ҳ������8
            byte[] buff = Encoding.UTF8.GetBytes(original);
            byte[] kb = Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider
            {
                Key = MD5Base64(kb),
                Mode = CipherMode.ECB
            };

            return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
        }

        /// <summary>
        /// ʹ�ø�����Կ�ַ�������string,����ָ�����뷽ʽ����
        /// </summary>
        /// <param name="encrypted">����</param>
        /// <param name="key">��Կ��Ϊ����ʹ��ȱʡ��Կ��</param>
        /// <param name="encoding">�ַ����뷽��</param>
        /// <returns>����</returns>
        public static string Decrypt(string encrypted, string key = "")
        {
            if (string.IsNullOrWhiteSpace(encrypted)) return "";
            key = key.Length < 9 ? defaultKey : key;//���ٳ���Ϊ9/Ҳ������8
            byte[] buff = Convert.FromBase64String(encrypted);
            byte[] kb = Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider
            {
                Key = MD5Base64(kb),
                Mode = CipherMode.ECB
            };

            return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));
        }
        #endregion ʹ�� ������Կ�ַ��� ����/����string

        #region ʹ�� ������Կ Url��ȫ��
        /// <summary>
        /// ʹ�ø�����Կ���ܣ�Url��ȫ�ģ�
        /// </summary>
        /// <param name="original">����</param>
        /// <param name="key">��Կ</param>
        /// <param name="ignoreEqualSign">�Ƿ��滻����λ��=��</param>
        /// <returns>����</returns>
        public static string EncryptUrlSafe(string original, string key = "", bool ignoreEqualSign = true)
        {
            key = key.Length < 9 ? defaultKey : key;//���ٳ���Ϊ9/Ҳ������8
            if (ignoreEqualSign)
                return Encrypt(original, key).Replace("+", "-").Replace("/", "_").Replace("=", "");
            else
                return Encrypt(original, key).Replace("+", "-").Replace("/", "_");
        }

        /// <summary>
        /// ʹ�ø�����Կ�������ݣ�Url��ȫ�ģ�
        /// </summary>
        /// <param name="encrypted">����</param>
        /// <param name="key">��Կ</param>
        /// <returns>����</returns>
        public static string DecryptUrlSafe(string encrypted, string key = "")
        {
            if (string.IsNullOrWhiteSpace(encrypted)) return "";
            key = key.Length < 9 ? defaultKey : key;//���ٳ���Ϊ9/Ҳ������8
            encrypted = encrypted.Replace("-", "+").Replace("_", "/");
            int mod4 = encrypted.Length % 4;
            if (mod4 > 0) encrypted += ("====".Substring(0, 4 - mod4));
            return Decrypt(encrypted, key);
        }
        #endregion ʹ�� ������Կ Url��ȫ��

        #region ����MD5ժҪ
        /// <summary>
        /// ����MD5ժҪ
        /// </summary>
        /// <param name="original">����Դ</param>
        /// <returns>ժҪ</returns>
        public static string MD5Base64(string original, Encoding encoding = null)
        {
            if (encoding == null)
                return Convert.ToBase64String(MD5Base64(Encoding.UTF8.GetBytes(original)));
            else
                return Convert.ToBase64String(MD5Base64(encoding.GetBytes(original)));
        }

        /// <summary>
        /// ����MD5ժҪ,˽��
        /// </summary>
        private static byte[] MD5Base64(byte[] original)
        {
            using (MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider())
            {
                return hashmd5.ComputeHash(original);
            }
        }

        public static string MD5(string text, Encoding encoding = null)
        {
            if (encoding == null)
                return BitConverter.ToString(MD5Base64(Encoding.Unicode.GetBytes(text))).Replace("-", "");
            else
                return BitConverter.ToString(MD5Base64(encoding.GetBytes(text))).Replace("-", "");
        }

        #endregion ����MD5ժҪ

        #region 3DES�ӽ���
        public static string Encrypt3DES(string text, string key = "")
        {
            key = key.Length < 9 ? defaultKey : key;//���ٳ���Ϊ9/Ҳ������8

            var keyHex = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            var ivHex = Encoding.UTF8.GetBytes(key.Substring(4, 4) + key.Substring(0, 4));
            var textHex = Encoding.UTF8.GetBytes(text);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(keyHex, ivHex), CryptoStreamMode.Write);

            cs.Write(textHex, 0, textHex.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        public static string Decrypt3DES(string text, string key = "")
        {
            key = key.Length < 9 ? defaultKey : key;//���ٳ���Ϊ9/Ҳ������8

            if (string.IsNullOrWhiteSpace(text)) return text;

            var keyHex = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            var ivHex = Encoding.UTF8.GetBytes(key.Substring(4, 4) + key.Substring(0, 4));
            var textHex = Convert.FromBase64String(text);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(keyHex, ivHex), CryptoStreamMode.Write);

            cs.Write(textHex, 0, textHex.Length);
            cs.FlushFinalBlock();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        #endregion 3DESC�ӽ���

        #region Base64 �ӽ���
        /// <summary>
        /// Base64���룬����2Ϊ����Ĭ��Encoding.UTF8
        /// </summary>
        public static string EncodeBase64(string value, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            byte[] bytes = encoding.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// Base64���루URL��ȫ�ģ�������2Ϊ����Ĭ��Encoding.UTF8
        /// </summary>
        public static string EncodeBase64UrlSafe(string value, Encoding encoding = null)
        {
            return EncodeBase64(value, encoding).Replace("+", "-").Replace("/", "_");
        }
        /// <summary>
        /// Base64���룬����2Ϊ����Ĭ��Encoding.UTF8
        /// </summary>
        public static string DecodeBase64(string value, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            byte[] bytes = Convert.FromBase64String(value);
            return encoding.GetString(bytes);
        }
        /// <summary>
        /// Base64���루URL��ȫ�ģ�������2Ϊ����Ĭ��Encoding.UTF8
        /// </summary>
        public static string DecodeBase64UrlSafe(string value, Encoding encoding = null)
        {
            return DecodeBase64(value.Replace("-", "+").Replace("_", "/"), encoding);
        }
        #endregion Base64 �ӽ���

        #region SHA1�㷨
        /// <summary>
        /// ���ַ�������SHA1����
        /// </summary>
        /// <param name="encrypted">δ���ܵȴ���֤ԭʼ�ַ���</param>
        /// <param name="encode">���뷽ʽ��������ΪUTF-8</param>
        public static string SHA1_Encrypt(string encrypted, Encoding encode = null)
        {
            encode = encode ?? Encoding.UTF8;
            byte[] byteEncrypted = encode.GetBytes(encrypted);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            byteEncrypted = iSHA.ComputeHash(byteEncrypted);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in byteEncrypted)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString().ToUpper();
        }

        /// <summary>
        /// SHA1�ַ�����֤
        /// </summary>
        /// <param name="encrypted">δ���ܵȴ���֤ԭʼ�ַ���</param>
        /// <param name="hashed">SHA1���ܺ��</param>
        /// <param name="encode">���뷽ʽ</param>
        public static bool SHA1_Verify(string encrypted, string hashed, Encoding encode = null)
        {
            encode = encode ?? Encoding.UTF8;
            string hashEncrypted = SHA1_Encrypt(encrypted, encode);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashEncrypted, hashed)) return true;
            else return false;
        }
        #endregion SHA1�㷨
    }
}
