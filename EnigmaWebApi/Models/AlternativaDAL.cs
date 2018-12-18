using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EnigmaWebApi.Models
{
    public class AlternativaDAL
    {
        public AlternativaDAL()
        {
        }
        /// <summary>
        /// Insere uma alternativa no banco de dados, *NÃO* precisando passar a propriedade ID
        /// </summary>
        /// <param name="A"> parametro  do tipo alternativa</param>
        public int Criar(Alternativa A)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "InserirAlternativa";
            comm.Parameters.Add("@Questao", SqlDbType.Int).Value = A.Questao.ID;
            comm.Parameters.Add("@Tipo", SqlDbType.Char).Value = A.Tipo;
            comm.Parameters.Add("@Conteudo", SqlDbType.VarChar).Value = A.Conteudo;
            comm.Parameters.Add("@Ordem", SqlDbType.Int).Value = A.Ordem;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = A.Usuario;
            comm.ExecuteNonQuery();
            comm.CommandType = CommandType.Text;
            comm.CommandText = "Select top 1 ID_Alternativa from Alternativa Where ID_Alternativa = " + A.ID + " and ID_Questao = " + A.Questao.ID + " order by ID_Alternativa desc";
            A.ID = Convert.ToInt32(comm.ExecuteScalar());
            comm.Connection.Close();
            return A.ID;
        }
        /// <summary>
        /// Altera uma alternativa no banco de dados
        /// Passando todos as propriedades
        /// </summary>
        /// <param name="A"> parametro do tipo de alternativa </param>
        public void Alterar(Alternativa A)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "AlterarAlternativa";
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = A.ID;
            comm.Parameters.Add("@Questao", SqlDbType.Int).Value = A.Questao.ID;
            comm.Parameters.Add("@Tipo", SqlDbType.Char).Value = A.Tipo;
            comm.Parameters.Add("@Conteudo", SqlDbType.VarChar).Value = A.Conteudo;
            comm.Parameters.Add("@Ordem", SqlDbType.Int).Value = A.Ordem;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = A.Usuario;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Consulta no Banco de Dados na tabela alternativa e retorna Um objeto do tipo Alternativa com todas suas propriedades 
        /// Com Questão so conteindo o ID
        /// </summary>
        /// <param name="id"> parametro um inteiro, sendo o ID da Tupla do Banco de dados </param>
        /// <returns></returns>
        public Alternativa Consultar(int id)
        {
            SqlCommand comm = new SqlCommand("Select * from Alternativa Where ID_Alternativa = "+id,Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Alternativa a = new Alternativa();
            while (dr.Read())
            {
                a.ID = Convert.ToInt32(dr.GetValue(0));
                Questao q = new Questao();
                q.ID = Convert.ToInt32(dr.GetValue(1));
                a.Questao = q;
                a.Tipo = dr.GetValue(2).ToString();
                a.Conteudo = dr.GetValue(3).ToString();
                a.Ordem = Convert.ToInt32(dr.GetValue(4));
                a.Usuario = Convert.ToInt32(dr.GetValue(5));
            }
            comm.Connection.Close();
            return a;
        }
    }
}
