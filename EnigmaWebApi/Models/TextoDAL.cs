using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EnigmaWebApi.Models
{
    public class TextoDAL
    {
        public TextoDAL()
        {
        }
        /// <summary>
        /// Insere um Texto no Banco de Dados e retorna o id do item iserido
        /// </summary>
        /// <param name="T">parametro do tipo Texto | sem id </param>
        public int Inserir(Texto T)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "InserirTexto";
            comm.Parameters.Add("@Tamanho", SqlDbType.Decimal).Value =  Convert.ToDecimal(T.Tamanho);
            comm.Parameters.Add("@Cor", SqlDbType.Char).Value =T.Cor;
            comm.Parameters.Add("@Conteudo", SqlDbType.Text).Value = T.Conteudo;
            comm.Parameters.Add("@Negrito", SqlDbType.Bit).Value = T.Negrito;
            comm.Parameters.Add("@Italico", SqlDbType.Bit).Value = T.Italico;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = T.Usuario;
            comm.ExecuteNonQuery();
            comm.CommandType = CommandType.Text;
            comm.CommandText = "Select top 1 ID_Texto from Texto order by ID_Texto desc";
            int id = Convert.ToInt32(comm.ExecuteScalar());
            comm.Connection.Close();
            return id;
        }
        /// <summary>
        /// Insere um Texto no Banco de Dados
        /// </summary>
        /// <param name="T"> parametro do tipo Texto | com id </param>
        public void Alterar(Texto T)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "AlterarTexto";
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = T.ID;
            comm.Parameters.Add("@Tamanho", SqlDbType.Decimal).Value = T.Tamanho;
            comm.Parameters.Add("@Cor", SqlDbType.Char).Value = T.Cor;
            comm.Parameters.Add("@Conteudo", SqlDbType.Text).Value = T.Conteudo;
            comm.Parameters.Add("@Negrito", SqlDbType.Bit).Value = T.Negrito;
            comm.Parameters.Add("@Italico", SqlDbType.Bit).Value = T.Italico;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = T.Usuario;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Delete um registro de Texto do Banco de Dados
        /// </summary>
        /// <param name="id"> parametro do tipo inteiro representando o id do Texto</param>
        public void Deletar(int id)
        {
            SqlCommand comm = new SqlCommand("Delete Texto where ID_Texto = "+id, Banco.Abrir());
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Retorna um Objeto do tipo Texto Completo
        /// </summary>
        /// <param name="id"> parametro do tipo inteiro representando o ID do Texto</param>
        /// <returns></returns>
        public Texto Consultar(int id)
        {
            SqlCommand comm = new SqlCommand("Select * from Texto Where ID_Texto = "+id, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Texto t = new Texto();
            while (dr.Read())
            {
                t.ID = Convert.ToInt32(dr.GetValue(0));
                t.Tamanho = Convert.ToInt32(dr.GetValue(1));
                t.Cor = dr.GetValue(2).ToString();
                t.Conteudo = dr.GetValue(3).ToString();
                t.Negrito = Convert.ToBoolean(dr.GetValue(4));
                t.Italico = Convert.ToBoolean(dr.GetValue(5));
                t.Usuario = Convert.ToInt32(dr.GetValue(6));
            }
            comm.Connection.Close();
            return t;
        }
    }
}
