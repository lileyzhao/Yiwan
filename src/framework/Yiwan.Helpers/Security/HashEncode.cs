/// <summary>
/// ��˵����Assistant
/// �� �� �ˣ��շ�
/// ��ϵ��ʽ��361983679  
/// ������վ��http://www.sufeinet.com/thread-655-1-1.html
/// </summary>

using System;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;

namespace Yiwan.Helpers.Security
{
    /// <summary>
    /// �õ������ȫ�루��ϣ���ܣ���
    /// </summary>
    public static class HashEncode
    {
        /// <summary>
        /// �õ������ϣ�����ַ���
        /// </summary>
        /// <returns></returns>
        public static string GetSecurity()
        {
            string Security = HashEncoding(GetRandomValue());
            return Security;
        }
        /// <summary>
        /// �õ�һ�������ֵ
        /// </summary>
        /// <returns></returns>
        public static string GetRandomValue()
        {
            Random Seed = new Random();
            string RandomVaule = Seed.Next(1, int.MaxValue).ToString(CultureInfo.InvariantCulture);
            return RandomVaule;
        }
        /// <summary>
        /// ��ϣ����һ���ַ���
        /// </summary>
        /// <param name="Security"></param>
        /// <returns></returns>
        public static string HashEncoding(string Security)
        {
            byte[] Value;
            UnicodeEncoding Code = new UnicodeEncoding();
            byte[] Message = Code.GetBytes(Security);
            SHA512Managed Arithmetic = new SHA512Managed();
            Value = Arithmetic.ComputeHash(Message);
            Arithmetic.Dispose();
            Security = "";
            foreach (byte o in Value)
            {
                Security += (int)o + "O";
            }
            return Security;
        }
    }
}