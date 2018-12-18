using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EnigmaWebApi.Models
{
    public class PerguntaDAL
    {
        public PerguntaDAL()
        {
        }
        /// <summary>
        /// Insere uma pergunta no Banco de Daodos
        /// Inserindo junto as Imagens em ordem necessitando do list de imagens 
        /// </summary>
        /// <param name="P"> parametro do tipo Pergunta | sem id </param>
        public int Inserir(Pergunta P)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "InserirPergunta";
            comm.Parameters.Add("@Titulo", SqlDbType.VarChar).Value = P.Titulo;
            comm.Parameters.Add("@Texto", SqlDbType.Text).Value = P.Texto;
            comm.Parameters.Add("@Visibilidade", SqlDbType.Bit).Value = P.Visibilidade;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = P.Usuario;
            comm.ExecuteNonQuery();
            comm.CommandType = CommandType.Text;
            comm.CommandText = "Select top 1 ID_Pergunta from Pergunta order by ID_Pergunta desc";
            P.ID = Convert.ToInt32(comm.ExecuteScalar());
            comm.CommandType = CommandType.StoredProcedure;
            ImagemDAL dalimg = new ImagemDAL();
            foreach (var item in P.Imagem)
            {
                item.ID = dalimg.Inserir(item);
            }
            comm.CommandText = "InserirImagemPergunta";
            comm.Parameters.Clear();
            int ordem = 1;
            foreach (var item in P.Imagem)
            {
                comm.Parameters.Add("@Imagem", SqlDbType.Int).Value = item.ID;
                comm.Parameters.Add("@Pergunta", SqlDbType.Int).Value = P.ID;
                comm.Parameters.Add("@Ordem", SqlDbType.Int).Value = ordem;
                comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = P.Usuario;
                ordem += 1;
                comm.ExecuteNonQuery();
                comm.Parameters.Clear();
            }
            comm.Connection.Close();
            return P.ID;
        }
        /// <summary>
        ///  Insere uma pergunta no Banco de Daodos
        ///  Inserindo junto as Imagens em ordem necessitando do list de imagens 
        /// </summary>
        /// <param name="P">parametro do tipo Pergunta | com id</param>
        public void Alterar(Pergunta P)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "AlterarPergunta";
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = P.ID;
            comm.Parameters.Add("@Titulo", SqlDbType.VarChar).Value = P.Titulo;
            comm.Parameters.Add("@Texto", SqlDbType.Text).Value = P.Texto;
            comm.Parameters.Add("@Visibilidade", SqlDbType.Bit).Value = P.Visibilidade;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = P.Usuario;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// retorna um objeto do tipo  pergunta
        /// Juntamento com o list de imagem em ordem e o de respostas
        /// </summary>
        /// <param name="id"> parametro do tipo inteiro representando o ID da Pergunta </param>
        /// <returns></returns>
        public Pergunta Consultar(int id)
        {
            SqlCommand comm = new SqlCommand("Select * from Pergunta where ID_Pergunta = "+id, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Pergunta p = new Pergunta();
            while (dr.Read())
            {
                p = new Pergunta
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Titulo = dr.GetValue(1).ToString(),
                    Texto = dr.GetValue(2).ToString(),
                    Visibilidade = Convert.ToBoolean(dr.GetValue(3)),
                    Usuario = Convert.ToInt32(dr.GetValue(4))
                };
            }
            dr.Close();
            comm.CommandText = @"Select i.ID_Imagem,ip.Ordem_ImagemPergunta 
                                from Imagem i inner join ImagemPergunta ip 
                                on i.ID_Imagem = ip.ID_Imagem 
                                Where ip.ID_Pergunta = "+id +" order by ip.Ordem_ImagemPergunta";
            dr = comm.ExecuteReader();
            List<Imagem> listaimg = new List<Imagem>();
            while (dr.Read())
            {
                ImagemDAL dal = new ImagemDAL();
                Imagem im = new Imagem();
                im = dal.Consultar(Convert.ToInt32(dr.GetValue(0)));
                listaimg.Add(im);
            }
            p.Imagem = listaimg;
            dr.Close();
            comm.CommandText = "Select ID_Resposta from Resposta Where ID_Pergunta = "+id;
            dr = comm.ExecuteReader();
            List<Resposta> listresposta = new List<Resposta>();
            while (dr.Read())
            {
                Resposta r = new Resposta();
                RespostaDAL dal = new RespostaDAL();
                r = dal.Consultar(Convert.ToInt32(dr.GetValue(0)));
                listresposta.Add(r);
            }
            p.Resposta = listresposta;
            comm.Connection.Close();
            return p;
        }
        public List<Pergunta> ConsultarTodos()
        {
            SqlCommand comm = new SqlCommand("Select * from Pergunta ", Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Pergunta p = new Pergunta();
            List<Pergunta> lista = new List<Pergunta>();
            while (dr.Read())
            {
                p = new Pergunta
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Titulo = dr.GetValue(1).ToString(),
                    Texto = dr.GetValue(2).ToString(),
                    Visibilidade = Convert.ToBoolean(dr.GetValue(3)),
                    Usuario = Convert.ToInt32(dr.GetValue(4)),
                    Imagem = null,
                    Resposta = null
                };
                lista.Add(p);
            }
            comm.Connection.Close();
            return lista;
          
        }

        public List<Pergunta> ConsultarPorUsuario(int usuario)
        {
            SqlCommand comm = new SqlCommand("Select * from Pergunta Where ID_Usuario = " + usuario, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            List<Pergunta> perguntas = new List<Pergunta>();
            while (dr.Read())
            {
                Pergunta p = new Pergunta
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Imagem = new List<Imagem>(),
                    Texto = dr.GetValue(2).ToString(),
                    Resposta = new List<Resposta>(),
                    Titulo = dr.GetValue(1).ToString(),
                    Visibilidade = Convert.ToBoolean(dr.GetValue(3)),
                    Usuario = Convert.ToInt32(dr.GetValue(4))
                };
                perguntas.Add(p);
            }
            comm.Connection.Close();
            return perguntas;
        }
        public void RemoverDenunciadas()
        {
            SqlCommand comm = new SqlCommand(@"Select p.ID_Pergunta ,Count(*)
                                            From Pergunta p inner join AvaliacaoPergunta ap
                                            on p.ID_Pergunta = ap.ID_Pergunta where  ap.Denuncia_AvaliacaoPergunta = 1
                                            Group by p.ID_Pergunta Having Count(*) >= 50 ", Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                SqlCommand comm2 = new SqlCommand("", Banco.Abrir());
                try
                {
                    comm2.CommandText = "Delete AvaliacaoPergunta Where ID_Pergunta = " + dr.GetValue(0).ToString();
                    comm2.ExecuteNonQuery();
                }
                catch { }
                comm2.CommandText = "Select ID_Imagem from ImagemPergunta Where ID_Pergunta = " + dr.GetValue(0).ToString();
                SqlDataReader dr2 = comm2.ExecuteReader();
                bool primeira = true;
                while (dr2.Read())
                {
                    SqlCommand comm3 = new SqlCommand("", Banco.Abrir());
                    if (primeira)
                    {
                        comm3.CommandText = "Delete ImagemPergunta Where ID_Pergunta = " + dr.GetValue(0).ToString();
                        comm3.ExecuteNonQuery();
                        primeira = false;
                    }
                    comm3.CommandText = "Delete Imagem Where ID_Imagem = " + dr2.GetValue(0).ToString();
                    comm3.ExecuteNonQuery();
                    comm3.Connection.Close();
                }
                dr2.Close();
                comm2.CommandText = "Select ID_Resposta from Resposta Where ID_Pergunta = " + dr.GetValue(0).ToString();
                dr2 = comm2.ExecuteReader();
                while (dr2.Read())
                {
                    primeira = true;
                    SqlCommand comm3 = new SqlCommand("Select ID_Imagem from ImagemResposta Where ID_Resposta = " + dr2.GetValue(0).ToString(), Banco.Abrir());
                    SqlDataReader dr3 = comm3.ExecuteReader();
                    while (dr3.Read())
                    {
                        SqlCommand comm4 = new SqlCommand("", Banco.Abrir());
                        if (primeira)
                        {
                            comm4.CommandText = "Delete ImagemResposta Where ID_Resposta = " + dr2.GetValue(0).ToString();
                            comm4.ExecuteNonQuery();
                            primeira = false;
                        }
                        comm4.CommandText = "Delete Imagem Where ID_Imagem = " + dr3.GetValue(0).ToString();
                        comm4.ExecuteNonQuery();

                        comm4.Connection.Close();
                    }
                    dr3.Close();
                    try
                    {
                        comm3.CommandText = "Delete AvaliacaoResposta Where ID_Resposta = " + dr2.GetValue(0).ToString();
                        comm3.ExecuteNonQuery();
                    }
                    catch { }
                    comm3.Connection.Close();
                }
                dr2.Close();
                try
                {
                    comm2.CommandText = "Delete Resposta Where ID_Pergunta = " + dr.GetValue(0).ToString();
                    comm2.ExecuteNonQuery();
                }
                catch { }
                comm2.CommandText = "Delete Pergunta Where ID_Pergunta = " + dr.GetValue(0).ToString();
                comm2.ExecuteNonQuery();
                comm2.Connection.Close();
            }
            comm.Connection.Close();
        }
        public void RemoverDenunciadas(Pergunta pergunta)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            try
            {
                comm.CommandText = "Delete AvaliacaoPergunta Where ID_Pergunta = " + pergunta.ID;
                comm.ExecuteNonQuery();
            }
            catch { }
            comm.CommandText = "Select ID_Imagem from ImagemPergunta Where ID_Pergunta = " + pergunta.ID;
            SqlDataReader dr = comm.ExecuteReader();
            bool primeira = true;
            while (dr.Read())
            {
                SqlCommand comm2 = new SqlCommand("", Banco.Abrir());
                if (primeira)
                {
                    comm2.CommandText = "Delete ImagemPergunta Where ID_Pergunta = " + pergunta.ID;
                    comm2.ExecuteNonQuery();
                    primeira = false;
                }
                comm2.CommandText = "Delete Imagem Where ID_Imagem = " + dr.GetValue(0).ToString();
                comm2.ExecuteNonQuery();
                comm2.Connection.Close();
            }
            dr.Close();
            comm.CommandText = "Select ID_Resposta from Resposta Where ID_Pergunta = " + pergunta.ID;
            dr = comm.ExecuteReader();
            while (dr.Read())
            {
                primeira = true;
                SqlCommand comm2 = new SqlCommand("Select ID_Imagem from ImagemResposta Where ID_Resposta = " + dr.GetValue(0).ToString(), Banco.Abrir());
                SqlDataReader dr2 = comm2.ExecuteReader();
                while (dr2.Read())
                {
                    SqlCommand comm3 = new SqlCommand("", Banco.Abrir());
                    if (primeira)
                    {
                        comm3.CommandText = "Delete ImagemResposta Where ID_Resposta = " + dr.GetValue(0).ToString();
                        comm3.ExecuteNonQuery();
                        primeira = false;
                    }
                    comm3.CommandText = "Delete Imagem Where ID_Imagem = " + dr2.GetValue(0).ToString();
                    comm3.ExecuteNonQuery();
                    comm3.Connection.Close();
                }
                dr2.Close();
                try
                {
                    comm2.CommandText = "Delete AvaliacaoResposta Where ID_Resposta = " + dr.GetValue(0).ToString();
                    comm2.ExecuteNonQuery();
                }
                catch { }
                comm2.Connection.Close();
            }
            dr.Close();
            try
            {
                comm.CommandText = "Delete Resposta Where ID_Pergunta = " + pergunta.ID;
                comm.ExecuteNonQuery();
            }
            catch { }
            comm.CommandText = "Delete Pergunta Where ID_Pergunta = " + pergunta.ID;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
    }
}
