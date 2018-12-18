using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace EnigmaWebApi.Models
{
    public class ResumoDAL
    {
        public ResumoDAL()
        {
        }
        /// <summary>
        /// Insere Um resumo na tabela resumo
        /// No conteudo somente necessitando do ID do Conteudo
        /// </summary>
        /// <param name="R"> parametro do tipo Resumo | sem id </param>
        public int Inserir(Resumo R)
        {
            SqlCommand comm = new SqlCommand("",Banco.Abrir());
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = "InserirResumo";
            comm.Parameters.Add("@Conteudo", SqlDbType.Int).Value =R.Conteudo.ID;
            comm.Parameters.Add("@NomeArquivo", SqlDbType.VarChar).Value = R.NomeArquivo;
            comm.Parameters.Add("@Arquivo", SqlDbType.VarBinary).Value = R.Arquivo;
            comm.Parameters.Add("@Extensao", SqlDbType.Char).Value = R.Extensao;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = R.Usuario;
            comm.ExecuteNonQuery();
            comm.CommandType = CommandType.Text;
            comm.CommandText = "Select top 1 ID_Resumo from Resumo where ID_Conteudo = "+R.Conteudo.ID+" order by ID_Resumo desc";
            int id = Convert.ToInt32(comm.ExecuteScalar());
            comm.Connection.Close();
            return id;
        }
        /// <summary>
        /// altera Um resumo na tabela resumo
        /// No conteudo somente necessitando do ID do Conteudo
        /// </summary>
        /// <param name="R">parametro do tipo Resumo | com id</param>
        public void Alterar(Resumo R)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = "AlterarResumo";
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = R.ID;
            comm.Parameters.Add("@Conteudo", SqlDbType.Int).Value = R.Conteudo.ID;
            comm.Parameters.Add("@NomeArquivo", SqlDbType.VarChar).Value = R.NomeArquivo;
            comm.Parameters.Add("@Arquivo", SqlDbType.VarBinary).Value = R.Arquivo;
            comm.Parameters.Add("@Extensao", SqlDbType.Char).Value = R.Extensao;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = R.Usuario;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Retorna um Objeto do tipo resumo 
        /// Com o conteudo somente retornando com o ID
        /// </summary>
        /// <param name="id"> parametro do tipo inteiro representando o ID do Resumo </param>
        /// <returns></returns>
        public Resumo Consultar(int id)
        {
            SqlCommand comm = new SqlCommand("Select * from Resumo Where ID_Resumo = "+id,Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Resumo r = new Resumo();
            while (dr.Read())
            {
                r = new Resumo
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Conteudo = new Conteudo { ID = Convert.ToInt32(dr.GetValue(1)) },
                    NomeArquivo=dr.GetValue(2).ToString(),
                    Arquivo = dr.GetValue(3) as byte[],
                    Extensao = dr.GetValue(4).ToString(),
                    Usuario = Convert.ToInt32(dr.GetValue(5))
                };
            }
            comm.Connection.Close();
            return r;
        }
    }
}
