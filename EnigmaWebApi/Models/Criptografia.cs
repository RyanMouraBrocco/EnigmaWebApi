using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace EnigmaWebApi.Models
{
    public  static  class Criptografia
    {
        /// <summary>
        /// gera e implementa a criptografia MD5, recebendo um texto e retornando ele criptografado
        /// </summary>
        /// <param name="texto"> string que será criptografada </param>
        /// <returns></returns>
        public  static string GerarMD5(string texto)
        {
            MD5 md = MD5.Create();
            byte[] text = md.ComputeHash(Encoding.Default.GetBytes(texto));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                sb.Append(text[i].ToString("X2"));
            }
            return sb.ToString();
        }
        
    }
}
