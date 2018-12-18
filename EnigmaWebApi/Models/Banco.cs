using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace EnigmaWebApi.Models
{
    public  static class Banco
    {
        /// <summary>
        /// Retorna um objeto SqlConnection  com sua conexão ao banco de dados.
        /// </summary>
        /// <returns> Retorna um Sqlconnection Com a conexão já Aberta </returns>
        public static  SqlConnection Abrir()
        {
            SqlConnection cn = new SqlConnection(@"Data Source=dbsq0007.whservidor.com;Initial Catalog=otermaenig;Persist Security Info=True;User ID=otermaenig;Password=GJMPRV10");
            cn.Open();
            return cn;
        }
    }
}
