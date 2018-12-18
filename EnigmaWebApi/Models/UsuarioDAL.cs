using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EnigmaWebApi.Models
{
    public class UsuarioDAL
    {
        public UsuarioDAL()
        {
        }
        /// <summary>
        /// Insere no Banco de Dados Um Usuario 
        /// Sendo a Senha Criptografada
        /// </summary>
        /// <param name="U"> parametro do tipo Usuario | sem id </param>
        public void Inserir(Usuario U)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = "InserirUsuario";
            comm.Parameters.Add("@Nome",SqlDbType.VarChar).Value = U.Nome;
            comm.Parameters.Add("@Email", SqlDbType.VarChar).Value = U.Email;
            comm.Parameters.Add("@Senha", SqlDbType.VarChar).Value = Criptografia.GerarMD5(U.Senha);
            comm.Parameters.Add("@TipoConta", SqlDbType.Char).Value = U.TipoConta;
            if (U.Foto != null)
            {
                comm.Parameters.Add("@Foto", SqlDbType.VarBinary).Value = U.Foto;
            }
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Insere no Banco de Dados Um Usuario 
        /// Sendo a Senha Criptografada
        /// </summary>
        /// <param name="U"> parametro do tipo Usuario | com id</param>
        public void Alterar(Usuario U)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = "AlterarUsuario";
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = U.ID;
            comm.Parameters.Add("@Nome", SqlDbType.VarChar).Value = U.Nome;
            comm.Parameters.Add("@Email", SqlDbType.VarChar).Value = U.Email;
            comm.Parameters.Add("@Senha", SqlDbType.VarChar).Value = Criptografia.GerarMD5(U.Senha);
            comm.Parameters.Add("@TipoConta", SqlDbType.Char).Value = U.TipoConta;
            if (U.Foto != null)
            {
                comm.Parameters.Add("@Foto", SqlDbType.VarBinary).Value = U.Foto;
            }
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        public void AlterarSemSenha(Usuario U)
        {
            SqlCommand comm = new SqlCommand("Select Senha_Usuario from Usuario Where ID_Usuario = "+U.ID, Banco.Abrir());
            string senha = comm.ExecuteScalar().ToString();
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = "AlterarUsuario";
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = U.ID;
            comm.Parameters.Add("@Nome", SqlDbType.VarChar).Value = U.Nome;
            comm.Parameters.Add("@Email", SqlDbType.VarChar).Value = U.Email;
            comm.Parameters.Add("@Senha", SqlDbType.VarChar).Value = senha;
            comm.Parameters.Add("@TipoConta", SqlDbType.Char).Value = U.TipoConta;
            if (U.Foto != null)
            {
                comm.Parameters.Add("@Foto", SqlDbType.VarBinary).Value = U.Foto;
            }
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Realiza a verificação do login de um Usuario
        /// Retornando true para logar e false para login incorreto ou inexistente
        /// </summary>
        /// <param name="login"> parametro do tipo caracter representando o Email do Usuario</param>
        /// <param name="senha">parametro do tipo caracter representando a Senha do Usuario</param>
        /// <returns></returns>
        public bool Logar(string login , string senha)
        {
            SqlCommand comm = new SqlCommand("Select Email_Usuario, Senha_Usuario from Usuario where Email_Usuario = '"+login+"'",Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            string _login = "";
            string _senha = "";
            while (dr.Read())
            {
                _login = dr.GetValue(0).ToString();
                _senha = dr.GetValue(1).ToString();
            }
            if (_login == "")
            {
                comm.Connection.Close();
                return false;
            }
            else
            {
                if (_login == login && _senha == Criptografia.GerarMD5(senha))
                {

                    comm.Connection.Close();
                    return true;
                }
                else
                {

                    comm.Connection.Close();
                    return false;
                }
            }
        }
        /// <summary>
        /// Retorna um Objeto do tipo inteiro de um Usuario (sem a Senha)
        /// </summary>
        /// <param name="id"> parametro do tipo inteiro representando o ID do Usuario</param>
        /// <returns></returns>
        public Usuario Consultar(int id)
        {
            SqlCommand comm = new SqlCommand("Select * from Usuario where ID_Usuario = " + id, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Usuario u = new Usuario();
            while (dr.Read())
            {
                u = new Usuario
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Nome = dr.GetValue(1).ToString(),
                    Email = dr.GetValue(2).ToString(),
                    Senha = null,
                    TipoConta = dr.GetValue(4).ToString(),
                    Foto = null
                 
                };
                if (dr.GetValue(5) != null)
                {
                    u.Foto = dr.GetValue(5) as byte[];
                }
            }
            comm.Connection.Close();
            return u;
        }
        /// <summary>
        ///  Retorna um Objeto do tipo inteiro de um Usuario (sem a Senha)
        /// </summary>
        /// <param name="login">parametro do tipo Caracter representando o Email do Usuario</param>
        /// <returns></returns>
        public Usuario Consultar(string login)
        {
            SqlCommand comm = new SqlCommand("Select * from Usuario where Email_Usuario = '" + login +"'", Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Usuario u = new Usuario();
            while (dr.Read())
            {
                u = new Usuario
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Nome = dr.GetValue(1).ToString(),
                    Email = dr.GetValue(2).ToString(),
                    Senha = null,
                    TipoConta = dr.GetValue(4).ToString(),
                    Foto = null

                };
                if (dr.GetValue(5) != null)
                {
                    u.Foto = dr.GetValue(5) as byte[];
                }
            }
            comm.Connection.Close();
            return u;
        }

        public List<Usuario> ConsultarTodos()
        {
            SqlCommand comm = new SqlCommand("Select * from Usuario", Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            List<Usuario> usuarios = new List<Usuario>();
            while (dr.Read())
            {
                Usuario u = new Usuario
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Nome = dr.GetValue(1).ToString(),
                    Email = dr.GetValue(2).ToString(),
                    Senha = null,
                    TipoConta = dr.GetValue(4).ToString(),
                    Foto = null

                };
                usuarios.Add(u);
            }
            comm.Connection.Close();
            return usuarios;
        }

    }
}
