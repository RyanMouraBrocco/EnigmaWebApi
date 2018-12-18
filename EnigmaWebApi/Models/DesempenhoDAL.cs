using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EnigmaWebApi.Models
{
    public class DesempenhoDAL
    {
        public DesempenhoDAL()
        {
        }
        /// <summary>
        /// Insere um Desempenho na tabela Desempenho 
        /// Precisando Apenas do ID do Usuario e o ID do Materia 
        /// </summary>
        /// <param name="D"> parametro do tipo Desempenho | sem id </param>
        public void Inserir(Desempenho D)
        {
            SqlCommand comm = new SqlCommand(@"Select Count(*) 
                                                From Materia m inner join Conteudo c on m.ID_Materia = c.ID_Materia
                                                inner join Exercicio e on e.ID_Conteudo = c.ID_Conteudo Where m.ID_Materia = " + D.Materia.ID, Banco.Abrir());
            decimal quantidade = Convert.ToDecimal(comm.ExecuteScalar()) * 10;
            decimal notas = 0;
            comm.CommandText = @"Select e.ID_Exercicio, Max(Nota_Nota)
                                From Materia m inner join Conteudo c on m.ID_Materia = c.ID_Materia
                                inner join Exercicio e on e.ID_Conteudo = c.ID_Conteudo 
                                inner join Nota n on n.ID_Exercicio = e.ID_Exercicio Where m.ID_Materia = " + D.Materia.ID + " and n.ID_Usuario = " + D.Usuario.ID + " group by e.ID_Exercicio";
            SqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                notas += Convert.ToDecimal(dr.GetValue(1));
            }
            dr.Close();
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "InserirDesempenho";
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = D.Usuario.ID;
            comm.Parameters.Add("@Materia", SqlDbType.Int).Value = D.Materia.ID;
            if (notas > 0)
            {
                comm.Parameters.Add("@Porcentagem", SqlDbType.Decimal).Value = notas / quantidade;
            }
            else
            {
                comm.Parameters.Add("@Porcentagem", SqlDbType.Decimal).Value = 0;
            }
            comm.Parameters.Add("@HorasEstudadas", SqlDbType.Decimal).Value = D.HorasEstudadas;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Insere um Desempenho na tabela Desempenho 
        /// Precisando Apenas do ID do Usuario e o ID do Materia 
        /// </summary>
        /// <param name="D">parametro do tipo Desempenho | com id</param>
        public void Alterar(Desempenho D)
        {
            SqlCommand comm = new SqlCommand(@"Select Count(*) 
                                                From Materia m inner join Conteudo c on m.ID_Materia = c.ID_Materia
                                                inner join Exercicio e on e.ID_Conteudo = c.ID_Conteudo Where m.ID_Materia = " + D.Materia.ID, Banco.Abrir());
            decimal quantidade = Convert.ToDecimal(comm.ExecuteScalar()) * 10;
            decimal notas = 0;
            comm.CommandText = @"Select e.ID_Exercicio, Max(Nota_Nota)
                                From Materia m inner join Conteudo c on m.ID_Materia = c.ID_Materia
                                inner join Exercicio e on e.ID_Conteudo = c.ID_Conteudo 
                                inner join Nota n on n.ID_Exercicio = e.ID_Exercicio Where m.ID_Materia = " + D.Materia.ID + " and n.ID_Usuario = " + D.Usuario.ID + " group by e.ID_Exercicio";
            SqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                notas += Convert.ToDecimal(dr.GetValue(1));
            }
            dr.Close();
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "AlterarDesempenho";
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = D.ID;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = D.Usuario.ID;
            comm.Parameters.Add("@Materia", SqlDbType.Int).Value = D.Materia.ID;
            if (notas > 0)
            {
                comm.Parameters.Add("@Porcentagem", SqlDbType.Decimal).Value = notas / quantidade;
            }
            else
            {
                comm.Parameters.Add("@Porcentagem", SqlDbType.Decimal).Value = 0;
            }
            comm.Parameters.Add("@HorasEstudadas", SqlDbType.Decimal).Value = D.HorasEstudadas;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// retorna um objeto do tipo Desempenho contendo apenas o ID do Usuario e o ID Da Materia
        /// </summary>
        /// <param name="id"> parametro inteiro sendo o id do desempenho </param>
        /// <returns></returns>
        public Desempenho Consultar(int id)
        {
            SqlCommand comm = new SqlCommand("Select * from Desempenho where ID_Desempenho = " + id, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Desempenho d = new Desempenho();
            while (dr.Read())
            {
                Usuario u = new Usuario();
                u.ID = Convert.ToInt32(dr.GetValue(1));
                Materia m = new Materia();
                m.ID = Convert.ToInt32(dr.GetValue(2));
                d = new Desempenho
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Usuario = u,
                    Materia = m,
                    Porcentagem = Convert.ToDecimal(dr.GetValue(3)),
                    HorasEstudadas = Convert.ToDecimal(dr.GetValue(4))
                };
            }
            comm.Connection.Close();
            return d;
        }
        /// <summary>
        /// retorna um objeto do tipo Desempenho contendo apenas o ID do Usuario e o ID Da Materia
        /// </summary>
        /// <param name="materia"> parametro do tipo inteiro que representa o id da materia</param>
        /// <param name="usuario">parametro do tipo inteiro que representa o id do usuario</param>
        /// <returns></returns>
        public Desempenho Consultar(int materia,int usuario)
        {
            SqlCommand comm = new SqlCommand("Select * from Desempenho where ID_Materia = " + materia+" and ID_Usuario = "+ usuario, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Desempenho d = new Desempenho();
            while (dr.Read())
            {
                Usuario u = new Usuario();
                u.ID = Convert.ToInt32(dr.GetValue(1));
                Materia m = new Materia();
                m.ID = Convert.ToInt32(dr.GetValue(2));
                d = new Desempenho
                {
                    ID = Convert.ToInt32(dr.GetValue(0)),
                    Usuario = u,
                    Materia = m,
                    Porcentagem = Convert.ToDecimal(dr.GetValue(3)),
                    HorasEstudadas = Convert.ToDecimal(dr.GetValue(4))
                };
            }
            comm.Connection.Close();
            return d;
        }
    }
}
