using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EnigmaWebApi.Models
{
    public class MateriaDAL
    {
        public MateriaDAL()
        {
        }
        /// <summary>
        /// Insere no Banco de Dados Uma Materia
        /// podendo ter Imagem ou não
        /// </summary>
        /// <param name="M"> parametro do tipo Materia | sem id </param>
        public int Inserir(Materia M)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "InserirMateria";
            comm.Parameters.Add("@Nome", SqlDbType.VarChar).Value =M.Nome;
            comm.Parameters.Add("@Descricao", SqlDbType.VarChar).Value = M.Descricao;
            if (M.Imagem!=null)
            {

                comm.Parameters.Add("@Imagem", SqlDbType.VarBinary).Value = M.Imagem;
            }
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = M.Usuario;
            comm.ExecuteNonQuery();
            comm.CommandType = CommandType.Text;
            comm.CommandText = "Select top 1 ID_Materia from Materia order by ID_Materia desc";
            int id = Convert.ToInt32(comm.ExecuteScalar());
            comm.Connection.Close();
            return id;
        }
        /// <summary>
        /// altera no Banco de Dados Uma Materia
        /// podendo ter Imagem ou não
        /// </summary>
        /// <param name="M">parametro do tipo Materia | com id</param>
        public void Alterar(Materia M)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "AlterarMateria";
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = M.ID;
            comm.Parameters.Add("@Nome", SqlDbType.VarChar).Value = M.Nome;
            comm.Parameters.Add("@Descricao", SqlDbType.VarChar).Value = M.Descricao;
            if (M.Imagem != null)
            {

                comm.Parameters.Add("@Imagem", SqlDbType.VarBinary).Value = M.Imagem;
            }
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = M.Usuario;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }

        /// <summary>
        /// Retorna um objeto do tipo materia  
        /// Com lista de Conteudo Completo sem as listas de ConteudoTexto e de Exercicio e de Resumo
        /// </summary>
        /// <param name="id"> parametro do tipo inteiro que representa o ID da materia</param>
        /// <returns></returns>
        public Materia Consultar(int id)
        {
            SqlCommand comm = new SqlCommand("Select * from Materia where ID_Materia = " + id, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Materia m = new Materia();
            while (dr.Read())
            {
                m = new Materia
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Nome = dr.GetValue(1).ToString(),
                    Descricao = dr.GetValue(2).ToString(),
                    Usuario = Convert.ToInt32(dr.GetValue(4)),
                    Conteudo =null,
                    Imagem = null
                };
                if (dr.GetValue(3) != null)
                {
                    m.Imagem = dr.GetValue(3) as byte[];
                }
            }
            dr.Close();
            comm.CommandText = "Select * from Conteudo Where ID_Materia = "+id;
            List<Conteudo> lista = new List<Conteudo>();
            dr = comm.ExecuteReader();
            while (dr.Read())
            {
                Conteudo c = new Conteudo
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Materia = m,
                    Nome = dr.GetValue(2).ToString(),
                    Ordem = Convert.ToInt32(dr.GetValue(4)),
                    Usuario= Convert.ToInt32(dr.GetValue(5)),
                    ConteudoTexto = null,
                    Exercicio = null,
                    Imagem = null,
                    Resumo = null
                };
                if (dr.GetValue(3) != null)
                {
                    c.Imagem = dr.GetValue(3) as byte[];
                }
                lista.Add(c);
            }
            m.Conteudo = lista;
            comm.Connection.Close();
            return m;
        }
        /// <summary>
        /// Retorna um objeto do tipo materia  
        /// Com lista de Conteudo Completo sem as listas de ConteudoTexto e de Exercicio e de Resumo
        /// </summary>
        /// <param name="id"> parametro do tipo caracter que representa o Nome da materia</param>
        /// <returns></returns>
        public Materia Consultar(string nome)
        {
            SqlCommand comm = new SqlCommand("Select * from Materia where Nome_Materia = '" + nome+"'", Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Materia m = new Materia();
            while (dr.Read())
            {
                m = new Materia
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Nome = dr.GetValue(1).ToString(),
                    Descricao = dr.GetValue(2).ToString(),
                    Usuario = Convert.ToInt32(dr.GetValue(4)),
                    Imagem =null,
                    Conteudo =null
                };
                if (dr.GetValue(3) != null)
                {
                    m.Imagem = dr.GetValue(3) as byte[];
                }
            }
            dr.Close();
            comm.CommandText = @"Select ID_Conteudo,c.ID_Materia,Nome_Conteudo,Imagem_Conteudo,Ordem_Conteudo,c.ID_Usuario 
                                 from Conteudo c inner join Materia m 
                                 on m.ID_Materia = c.ID_Materia 
                                    Where Nome_Materia = '" + nome+"'";
            List<Conteudo> lista = new List<Conteudo>();
            dr = comm.ExecuteReader();
            while (dr.Read())
            {
                Conteudo c = new Conteudo
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Materia = m,
                    Nome = dr.GetValue(2).ToString(),
                    Ordem = Convert.ToInt32(dr.GetValue(4)),
                    Usuario = Convert.ToInt32(dr.GetValue(5)),
                    Resumo=null,
                    Imagem =null,
                    ConteudoTexto=null,
                    Exercicio=null
                };
                if (dr.GetValue(3) != null)
                {
                    c.Imagem = dr.GetValue(3) as byte[];
                }
                lista.Add(c);
            }
            m.Conteudo = lista;
            comm.Connection.Close();
            return m;
        }
        /// <summary>
        /// Retorna Todas As Materias cadastradas 
        /// Sem lista de Conteudo
        /// </summary>
        /// <returns></returns>
        public List<Materia> ConsultarTodos()
        {
            SqlCommand comm = new SqlCommand("Select * from Materia", Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Materia m = new Materia();
            List<Materia> lista = new List<Materia>();
            while (dr.Read())
            {
                m = new Materia
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Nome = dr.GetValue(1).ToString(),
                    Descricao = dr.GetValue(2).ToString(),
                    Usuario = Convert.ToInt32(dr.GetValue(4)),
                    Imagem = null,
                    Conteudo = null
                };
                if (dr.GetValue(3) != null)
                {
                    m.Imagem = dr.GetValue(3) as byte[];
                }
                lista.Add(m);
            }
            return lista;
        }

    }
}
