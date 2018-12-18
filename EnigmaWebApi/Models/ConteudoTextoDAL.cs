using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EnigmaWebApi.Models
{
    public class ConteudoTextoDAL
    {
        public ConteudoTextoDAL()
        {
        }
        /// <summary>
        /// Insere um objeto do tipo ConteudoTexto na tabela conteudo texto do Banco de Dados
        /// Podendo ser nulo os valores Texto ou Video ou Imagem
        /// Para que monte a estrutura correa
        /// </summary>
        /// <param name="C"> parametro do tipo ConteudoTexto | sem id </param>
        public void Inserir(ConteudoTexto C)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "InserirConteudoTexto";
            comm.Parameters.Add("@Conteudo", SqlDbType.Int).Value = C.Conteudo.ID;
            if (C.Texto != null)
            {
                comm.Parameters.Add("@Texto", SqlDbType.VarChar).Value = C.Texto.ID;
            }
            if (C.Video != null)
            {
                comm.Parameters.Add("@Video", SqlDbType.VarChar).Value = C.Video.ID;
            }
            if (C.Imagem != null)
            {
                comm.Parameters.Add("@Imagem", SqlDbType.VarChar).Value = C.Imagem.ID;
            }
            comm.Parameters.Add("@Ordem", SqlDbType.Int).Value = C.Ordem;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = C.Usuario;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Altera um objeto do tipo ConteudoTexto na tabela conteudo texto do Banco de Dados
        /// Podendo ser nulo os valores Texto ou Video ou Imagem
        /// Para que monte a estrutura correa
        /// </summary>
        /// <param name="C">parametro do tipo ConteudoTexto | Com id</param>
        public void Alterar(ConteudoTexto C)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "AlterarConteudoTexto";
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = C.ID;
            comm.Parameters.Add("@Conteudo", SqlDbType.Int).Value = C.Conteudo.ID;
            if (C.Texto != null)
            {
                comm.Parameters.Add("@Texto", SqlDbType.VarChar).Value = C.Texto.ID;
            }
            if (C.Video != null)
            {
                comm.Parameters.Add("@Video", SqlDbType.VarChar).Value = C.Video.ID;
            }
            if (C.Imagem != null)
            {
                comm.Parameters.Add("@Imagem", SqlDbType.VarChar).Value = C.Imagem.ID;
            }
            comm.Parameters.Add("@Ordem", SqlDbType.Int).Value = C.Ordem;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = C.Usuario;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Deleta informações da tabela conteudotexto sendo especifico pelo id do conteudotexto
        /// </summary>
        /// <param name="id"> parametro do tipo inteiro sendo o id do ConteudoTexto </param>
        public void Deletar(int id)
        {
            SqlCommand comm = new SqlCommand("Delete ConteudoTexto where ID_ConteudoTexto = "+id,Banco.Abrir());
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Deleta informações da tabela conteudotexto sendo  pelo id do conteudo
        /// Limpando todo o ConteudoTexto de Um Conteudo
        /// </summary>
        /// <param name="id"> parametro do tipo inteiro sendo o id do Conteudo </param>
        public void DeletarTudo(int id)
        {
            SqlCommand comm = new SqlCommand("Delete ConteudoTexto where ID_Conteudo = " + id, Banco.Abrir());
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }

        /// <summary>
        /// retorna um objeto ConteudoTexto com informações completas do Texto ou imagem ou video completos
        /// </summary>
        /// <param name="id">parametro inteiro do id ConteudoTexto</param>
        /// <returns></returns>
        public ConteudoTexto Consultar(int id)
        {
            SqlCommand comm = new SqlCommand("Select * from ConteudoTexto where ID_ConteudoTexto = " + id, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            ConteudoTexto c = new ConteudoTexto();
            while (dr.Read())
            {
                Texto t = null;
                if (dr.GetValue(2).ToString() != "")
                {
                    t = new Texto();
                    TextoDAL daltex = new TextoDAL();
                    t = daltex.Consultar(Convert.ToInt32(dr.GetValue(2)));
                }
                Conteudo cc = new Conteudo { ID = Convert.ToInt32(dr.GetValue(1)) };
                Imagem i = null;
                if (dr.GetValue(4).ToString() != "")
                {
                    i = new Imagem();
                    ImagemDAL dalimg = new ImagemDAL();
                    i = dalimg.Consultar(Convert.ToInt32(dr.GetValue(4)));
                }
                Video v = null;
                if (dr.GetValue(3).ToString() != "")
                {
                    v = new Video();
                    VideoDAL dalvid = new VideoDAL();
                    v = dalvid.Consultar(Convert.ToInt32(dr.GetValue(3)));
                }
                c = new ConteudoTexto
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Conteudo = cc,
                    Texto = t,
                    Imagem = i,
                    Video = v,
                    Ordem = Convert.ToInt32(dr.GetValue(5)),
                    Usuario = Convert.ToInt32(dr.GetValue(6))
                };
            }
            comm.Connection.Close();
            return c;
        }
    }
}
