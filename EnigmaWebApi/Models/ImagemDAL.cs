using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EnigmaWebApi.Models
{
    public class ImagemDAL
    {
        /// <summary>
        /// Insere uma Imagem na tabela Imagem do banco de dado e retorna o id do item iserido
        /// </summary>
        /// <param name="I"> parametro do tipo Imagem | sem id </param>
        public int Inserir(Imagem I)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "InserirImagem";
            comm.Parameters.Add("@Nome", SqlDbType.VarChar).Value = I.Nome;
            comm.Parameters.Add("@Imagem", SqlDbType.VarBinary).Value = I._Imagem;
            comm.Parameters.Add("@Extensao", SqlDbType.Char).Value = I.Extensao;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = I.Usuario;
            comm.ExecuteNonQuery();
            comm.CommandType = CommandType.Text;
            comm.CommandText = "Select top 1 ID_Imagem from Imagem order by ID_Imagem desc";
            int id = Convert.ToInt32(comm.ExecuteScalar());
            comm.Connection.Close();
            return id;
        }
        /// <summary>
        /// altera uma Imagem na tabela Imagem do banco de dado
        /// </summary>
        /// <param name="I">parametro do tipo Imagem | com id</param>
        public void Alterar(Imagem I)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "AlterarImagem";
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = I.ID;
            comm.Parameters.Add("@Nome", SqlDbType.VarChar).Value = I.Nome;
            comm.Parameters.Add("@Imagem", SqlDbType.VarBinary).Value = I._Imagem;
            comm.Parameters.Add("@Extensao", SqlDbType.Char).Value = I.Extensao;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = I.Usuario;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Deleta uma Imagem na tabela Imagem do banco de dado
        /// </summary>
        /// <param name="id">parametro do tipo inteiro represnetando o ID da Imagem</param>
        public void Deletar(int id)
        {
            SqlCommand comm = new SqlCommand("Delete Imagem where ID_Imagem = " + id, Banco.Abrir());
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Retorna um objeto do tipo imagem (Completo)
        /// </summary>
        /// <param name="id"> parametro do tipo inteiro representanto o ID da Imagem </param>
        /// <returns></returns>
        public Imagem Consultar(int id)
        {
            SqlCommand comm = new SqlCommand("Select * from Imagem Where ID_Imagem = " + id, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Imagem I = new Imagem();
            while (dr.Read())
            {
                I.ID = Convert.ToInt32(dr.GetValue(0));
                I.Nome = dr.GetValue(1).ToString();
                I._Imagem= dr.GetValue(2) as byte[];
                I.Extensao = dr.GetValue(3).ToString();
                I.Usuario = Convert.ToInt32(dr.GetValue(4));
            }
            comm.Connection.Close();
            return I;
        }
    }
}
