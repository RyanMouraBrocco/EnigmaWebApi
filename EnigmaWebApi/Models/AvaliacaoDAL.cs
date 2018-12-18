using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EnigmaWebApi.Models
{
    public class AvaliacaoDAL
    {
        public AvaliacaoDAL()
        {
        }
        /// <summary>
        /// Insere uma avaliação feita no forum no Banco de Dados, Sendo ela da pergunta ou da resposta
        /// Se Pergunta = null, Insere da Resposta
        /// se resposta = null, Insere a Da Pergunta
        /// </summary>
        /// <param name="A"> parametro do tipo Avaliacao / sem ID </param>

        public void Inserir(Avaliacao A)
        {
            SqlCommand comm = new SqlCommand("",Banco.Abrir());
            comm.CommandType = CommandType.StoredProcedure;
            if (A.Pergunta == null)
            {
                comm.CommandText = "InserirAvaliacaoResposta";
                comm.Parameters.Add("@Resposta", SqlDbType.Int).Value = A.Resposta.ID;

            }
            else
            {
                comm.CommandText = "InserirAvaliacaoPergunta";
                comm.Parameters.Add("@Pergunta", SqlDbType.Int).Value =A.Pergunta.ID;
            }
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = A.Usuario.ID;
            comm.Parameters.Add("@Avaliacao", SqlDbType.Bit).Value = A._Avaliacao;
            comm.Parameters.Add("@Denuncia", SqlDbType.Bit).Value = A.Denuncia;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Atlera uma avaliação feita no forum no Banco de Dados, Sendo ela da pergunta ou da resposta
        /// Se Pergunta = null, Altera da Resposta
        /// se resposta = null, Altera a Da Pergunta
        /// </summary>
        /// <param name="A">parametro do tipo Avaliacao / com ID</param>
        public void Alterar(Avaliacao A)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = CommandType.StoredProcedure;
            if (A.Pergunta == null)
            {
                comm.CommandText = "AlterarAvaliacaoResposta";
                comm.Parameters.Add("@Resposta", SqlDbType.Int).Value = A.Resposta.ID;

            }
            else
            {
                comm.CommandText = "AlterarAvaliacaoPergunta";
                comm.Parameters.Add("@Pergunta", SqlDbType.Int).Value = A.Pergunta.ID;
            }
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = A.ID;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = A.Usuario.ID;
            comm.Parameters.Add("@Avaliacao", SqlDbType.Bit).Value = A._Avaliacao;
            comm.Parameters.Add("@Denuncia", SqlDbType.Bit).Value = A.Denuncia;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Retorna um objeto do tipo Avaliacao com a Avaliação de 1 usuario em especifico sendo, de uma resposta ou pergunta
        /// Com Pergunta, Usuario e Resposta sendo nulo ou so o ID
        /// Se o ID da Pergunta for 0 então retorna a avaliação da resposta
        /// Se o ID_Resposta for 0 então retorna a avaliação da pergunta
        /// </summary>
        /// <param name="usuario">Recebe um inteiro do ID do Usuario</param>
        /// <param name="pergunta">Recebe um inteiro do ID do Pergunta</param>
        /// <param name="resposta">Recebe um inteiro do ID da Resposta</param>
        /// <returns></returns>
        public Avaliacao Consultar(int usuario, int pergunta, int resposta)
        {
            SqlCommand comm = new SqlCommand("",Banco.Abrir());
            if (pergunta > 0)
            {
                comm.CommandText = "Select * from AvaliacaoPergunta where ID_Usuario = "+usuario +" and ID_Pergunta = "+pergunta;
            }
            else
            {
                comm.CommandText = "Select * from AvaliacaoResposta where ID_Usuario = " + usuario + " and ID_Resposta = " + resposta;
            }
            SqlDataReader dr = comm.ExecuteReader();
            Avaliacao a = new Avaliacao();
            while (dr.Read())
            {
                a.ID = Convert.ToInt32(dr.GetValue(0));
                Usuario u = new Usuario();
                u.ID = Convert.ToInt32(dr.GetValue(1));
                a.Usuario = u;
                if (pergunta>0)
                {
                    Pergunta p = new Pergunta();
                    p.ID = Convert.ToInt32(dr.GetValue(2));
                    a.Pergunta = p;
                    a.Resposta = null;
                }
                else
                {
                    Resposta r = new Resposta();
                    r.ID = Convert.ToInt32(dr.GetValue(2));
                    a.Pergunta = null;
                    a.Resposta = r;
                }
                if (Convert.ToBoolean(dr.GetValue(4))==false)
                {
                    a._Avaliacao = Convert.ToBoolean(dr.GetValue(3));
                    a.Denuncia = false;
                }
                else
                {
                    a.Denuncia = true;
                }
            }
            comm.Connection.Close();
            return a;
        }
        /// <summary>
        /// Retorna um inteiro com todas as avaliações possitiva de uma pergunta ou resposta
        /// Se o id da pergunta for 0 pesquisa as avaliações da resposta
        /// se o id da resposta for 0 pesquisa as avaliações da pergunta
        /// </summary>
        /// <param name="pergunta"> Recebe o id da pergunta </param>
        /// <param name="resposta"> Recebe o id  da resposta </param>
        /// <returns></returns>
        public int AvaliacaoPossitiva(int pergunta, int resposta)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            if (pergunta > 0)
            {
                comm.CommandText = "Select Count(ID_AvaliacaoPergunta) from AvaliacaoPergunta where ID_Pergunta = " + pergunta + " and Avaliacao_AvaliacaoPergunta = 1";
            }
            else
            {
                comm.CommandText = "Select Count(ID_AvaliacaoResposta) from AvaliacaoResposta where  ID_Resposta = " + resposta + " and Avaliacao_AvaliacaoResposta = 1";
            }
            int qtd = 0;
            try
            {
                qtd = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch
            {
                qtd = 0;
            }
            comm.Connection.Close();
            return qtd;
        }
        /// <summary>
        /// Retorna um inteiro com todas as avaliações Negativas de uma pergunta ou resposta
        /// Se o id da pergunta for 0 pesquisa as avaliações da resposta
        /// se o id da resposta for 0 pesquisa as avaliações da pergunta
        /// </summary>
        /// <param name="pergunta">Recebe o id da pergunta</param>
        /// <param name="resposta">Recebe o id da Resposta</param>
        /// <returns></returns>
        public int AvaliacaoNegativa(int pergunta, int resposta)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            if (pergunta > 0)
            {
                comm.CommandText = "Select Count(ID_AvaliacaoPergunta) from AvaliacaoPergunta where ID_Pergunta = " + pergunta + " and Avaliacao_AvaliacaoPergunta = 0 and Denuncia_AvaliacaoPergunta = 0";
            }
            else
            {
                comm.CommandText = "Select Count(ID_AvaliacaoResposta)  from AvaliacaoResposta where  ID_Resposta = " + resposta + " and Avaliacao_AvaliacaoResposta = 0 and Denuncia_AvaliacaoResposta = 0";
            }
            int qtd = 0;
            try
            {
                qtd = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch
            {
                qtd = 0;
            }
            comm.Connection.Close();
            return qtd;
        }
        /// <summary>
        /// Retorna um inteiro com todas as denucias de uma pergunta ou resposta
        /// Se o id da pergunta for 0 pesquisa as avaliações da resposta
        /// se o id da resposta for 0 pesquisa as avaliações da pergunta
        /// </summary>
        /// <param name="pergunta">Recebe o id da pergunta</param>
        /// <param name="resposta">Recebe o id da resposta</param>
        /// <returns></returns>
        public int Denuncias(int pergunta, int resposta)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            if (pergunta > 0)
            {
                comm.CommandText = "Select Count(ID_AvaliacaoPergunta) from AvaliacaoPergunta where ID_Pergunta = " + pergunta + " and Denuncia_AvaliacaoPergunta = 1";
            }
            else
            {
                comm.CommandText = "Select Count(ID_AvaliacaoResposta) from AvaliacaoResposta where  ID_Resposta = " + resposta + " and Denuncia_AvaliacaoResposta = 1";
            }
            int qtd = 0;
            try
            {
                qtd = Convert.ToInt32(comm.ExecuteScalar());
            }
            catch
            {
                qtd = 0;
            }
            comm.Connection.Close();
            return qtd;
        }
    }
}
