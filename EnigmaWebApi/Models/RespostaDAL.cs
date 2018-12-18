using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EnigmaWebApi.Models
{
    public class RespostaDAL
    {
        public RespostaDAL()
        {
        }
        /// <summary>
        /// Insere uma resposta do forum no Banco de Dados
        /// Contendo List de Imagens na Ordem
        /// </summary>
        /// <param name="R"> parametro do tipo Resposta | sem id </param>
        public int Inserir(Resposta R)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "InserirResposta";
            comm.Parameters.Add("@Pergunta", SqlDbType.Int).Value = R.Pergunta.ID;
            comm.Parameters.Add("@Titulo", SqlDbType.VarChar).Value = R.Titulo;
            comm.Parameters.Add("@Texto", SqlDbType.Text).Value = R.Texto;
            comm.Parameters.Add("@Visibilidade", SqlDbType.Bit).Value = R.Visibilidade;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = R.Usuario;
            comm.ExecuteNonQuery();
            comm.CommandType = CommandType.Text;
            comm.CommandText = "Select top 1 ID_Resposta from Resposta Where ID_Pergunta = " + R.Pergunta.ID + " order by ID_Resposta desc";
            R.ID = Convert.ToInt32(comm.ExecuteScalar());
            ImagemDAL dalimg = new ImagemDAL();
            foreach (var item in R.Imagem)
            {
                item.ID = dalimg.Inserir(item);
            }
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = "InserirImagemResposta";
            comm.Parameters.Clear();
            int ordem = 1;
            foreach (var item in R.Imagem)
            {
                comm.Parameters.Add("@Imagem", SqlDbType.Int).Value = item.ID;
                comm.Parameters.Add("@Resposta", SqlDbType.Int).Value = R.ID;
                comm.Parameters.Add("@Ordem", SqlDbType.Int).Value = ordem;
                comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = R.Usuario;
                comm.ExecuteNonQuery();
                ordem += 1;
                comm.Parameters.Clear();
            }
            comm.Connection.Close();
            return R.ID;
        }
        /// <summary>
        /// Insere uma resposta do forum no Banco de Dados
        /// Contendo List de Imagens na Ordem
        /// </summary>
        /// <param name="R">parametro do tipo Resposta | com id</param>
        public void Alterar(Resposta R)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "AlterarResposta";
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = R.ID;
            comm.Parameters.Add("@Pergunta", SqlDbType.Int).Value = R.Pergunta.ID;
            comm.Parameters.Add("@Titulo", SqlDbType.VarChar).Value = R.Titulo;
            comm.Parameters.Add("@Texto", SqlDbType.Text).Value = R.Texto;
            comm.Parameters.Add("@Visibilidade", SqlDbType.Bit).Value = R.Visibilidade;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = R.Usuario;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Retorna um objeto do tipo Resposta Com informação 
        /// Contendo lista de imagens na ordem
        /// Contendo apenas o ID da Pergunta
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Resposta Consultar(int id)
        {
            SqlCommand comm = new SqlCommand("Select * from Resposta Where ID_Resposta = "+id,Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Resposta r = new Resposta();
            while (dr.Read())
            {
                r.ID = Convert.ToInt32(dr.GetValue(0));
                Pergunta p = new Pergunta
                {
                    ID = Convert.ToInt32(dr.GetValue(1))
                };
                r.Pergunta = p;
                r.Titulo = dr.GetValue(2).ToString();
                r.Texto = dr.GetValue(3).ToString();
                r.Visibilidade = Convert.ToBoolean(dr.GetValue(4));
                r.Usuario = Convert.ToInt32(dr.GetValue(5));
            }
            dr.Close();
            comm.CommandText = @"Select i.ID_Imagem,ip.Ordem_ImagemResposta
                                 from Imagem i inner join ImagemResposta ip 
                                 on i.ID_Imagem = ip.ID_Imagem 
                                 Where ip.ID_Resposta = "+id +" order by ip.Ordem_ImagemResposta";
            dr = comm.ExecuteReader();
            List<Imagem> list = new List<Imagem>();
            while (dr.Read())
            {
                Imagem i = new Imagem();
                ImagemDAL dal = new ImagemDAL();
                i = dal.Consultar(Convert.ToInt32(dr.GetValue(0)));
                list.Add(i);
            }
            r.Imagem = list;
            comm.Connection.Close();
            return r;
        }
        public List<Resposta> ConsultarPorUsuario(int usuario)
        {
            SqlCommand comm = new SqlCommand("Select * from Resposta Where ID_Usuario = " + usuario, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            List<Resposta> respostas = new List<Resposta>();
            while (dr.Read())
            {
                Resposta r = new Resposta
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Imagem = new List<Imagem>(),
                    Texto = dr.GetValue(3).ToString(),
                    Pergunta = new Pergunta { ID = Convert.ToInt32(dr.GetValue(1)) },
                    Titulo = dr.GetValue(2).ToString(),
                    Visibilidade = Convert.ToBoolean(dr.GetValue(4)),
                    Usuario = Convert.ToInt32(dr.GetValue(5))
                };
                respostas.Add(r);
            }
            comm.Connection.Close();
            return respostas;
        }
        public void RemoverDenunciadas()
        {
            SqlCommand comm = new SqlCommand(@"Select p.ID_Resposta ,Count(*)
                                            From Resposta p inner join AvaliacaoResposta ap
                                            on p.ID_Resposta = ap.ID_Resposta where  ap.Denuncia_AvaliacaoResposta = 1
                                            Group by p.ID_Resposta Having Count(*) >= 50 ", Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                SqlCommand comm2 = new SqlCommand("", Banco.Abrir());
                comm2.CommandText = "Select ID_Imagem from ImagemResposta Where ID_Resposta = " + dr.GetValue(0).ToString();
                SqlDataReader dr2 = comm2.ExecuteReader();
                bool primeira = true;
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
                comm2.CommandText = "Delete Resposta Where ID_Resposta = " + dr.GetValue(0).ToString();
                comm2.ExecuteNonQuery();
                comm2.Connection.Close();
            }
            comm.Connection.Close();
        }

        public void RemoverDenunciadas(Resposta resposta)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandText = "Select ID_Imagem from ImagemResposta Where ID_Resposta = " + resposta.ID;
            SqlDataReader dr = comm.ExecuteReader();
            bool primeira = true;
            while (dr.Read())
            {
                SqlCommand comm2 = new SqlCommand("", Banco.Abrir());
                if (primeira)
                {
                    comm2.CommandText = "Delete ImagemResposta Where ID_Resposta = " + resposta.ID;
                    comm2.ExecuteNonQuery();
                    primeira = false;
                }
                comm2.CommandText = "Delete Imagem Where ID_Imagem = " + dr.GetValue(0).ToString();
                comm2.ExecuteNonQuery();
                comm2.Connection.Close();
            }
            dr.Close();
            try
            {
                comm.CommandText = "Delete AvaliacaoResposta Where ID_Resposta = " + resposta.ID;
                comm.ExecuteNonQuery();
            }
            catch { }
            comm.CommandText = "Delete Resposta Where ID_Resposta = " + resposta.ID;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
            comm.Connection.Close();
        }
    }
}
