using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EnigmaWebApi.Models
{
    public class ExercicioDAL
    {
        public ExercicioDAL()
        {
        }
        /// <summary>
        /// Cria um Exercicio no Banco de daodos
        /// Precisando apenas do ID do Conteudo
        /// </summary>
        /// <param name="E"> Parametro do tipo exercicio | sem id </param>
        public int Criar(Exercicio E)
        {
            SqlCommand comm = new SqlCommand("", Banco.Abrir());
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "InserirExercicio";
            comm.Parameters.Add("@Conteudo", SqlDbType.Int).Value = E.Conteudo.ID;
            comm.Parameters.Add("@Descricao", SqlDbType.VarChar).Value = E.Descricao;
            comm.Parameters.Add("@Tipo", SqlDbType.Char).Value = E.Tipo;
            comm.Parameters.Add("@AleatorioQuestao", SqlDbType.Bit).Value = E.AleatorioQuestao;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = E.Usuario;
            comm.ExecuteNonQuery();
            comm.CommandType = CommandType.Text;
            comm.CommandText = "Select top 1 ID_Exercicio from Exercicio Where ID_Conteudo = " + E.Conteudo.ID + " order by ID_Exercicio desc";
            E.ID = Convert.ToInt32(comm.ExecuteScalar());
            comm.Connection.Close();
            return E.ID;
        }
        /// <summary>
        /// Altera um Exercicio no Banco de daodos
        /// Precisando apenas do ID do Conteudo
        /// </summary>
        /// <param name="E"> Parametro do tipo exercicio | com id </param>
        public void Alterar(Exercicio E)
        {
            SqlCommand comm = new SqlCommand("Select * From Questao Where ID_Exercicio = " + E.ID, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                SqlCommand comm2 = new SqlCommand("Delete Alternativa Where ID_Questao = " + dr.GetValue(0).ToString(), Banco.Abrir());
                comm2.ExecuteNonQuery();
                try
                {
                    comm2.CommandText = "Delete ImagemQuestao Where ID_Questao = " + dr.GetValue(0).ToString();
                    comm2.ExecuteNonQuery();
                }
                catch { }
                comm2.Connection.Close();
            }
            dr.Close();
            comm.CommandText = "Delete Questao Where ID_Exercicio = " + E.ID;
            comm.ExecuteNonQuery();
            foreach (var item in E.Questao)
            {
                QuestaoDAL dal = new QuestaoDAL();
                item.ID = dal.Criar(item);
                foreach (var i in item.Alternativa)
                {
                    i.Questao = item;
                    AlternativaDAL dalalt = new AlternativaDAL();
                    dalalt.Criar(i);
                }
            }
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.CommandText = "AlterarExercicio";
            comm.Parameters.Add("@ID", SqlDbType.Int).Value = E.ID;
            comm.Parameters.Add("@Conteudo", SqlDbType.Int).Value = E.Conteudo.ID;
            comm.Parameters.Add("@Descricao", SqlDbType.VarChar).Value = E.Descricao;
            comm.Parameters.Add("@Tipo", SqlDbType.Char).Value = E.Tipo;
            comm.Parameters.Add("@AleatorioQuestao", SqlDbType.Bit).Value = E.AleatorioQuestao;
            comm.Parameters.Add("@Usuario", SqlDbType.Int).Value = E.Usuario;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
        }
        /// <summary>
        /// Retorna um objeto do tipo exercicio completo com list de questoes (ordenadas ) ja com alternativas (ordenadas)
        /// </summary>
        /// <param name="id">parametro inteiro representando o ID do exercicio</param>
        /// <returns></returns>
        public Exercicio Consultar(int id)
        {
            SqlCommand comm = new SqlCommand("Select * from Exercicio Where ID_Exercicio = " + id, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            Exercicio e = new Exercicio();
            while (dr.Read())
            {
                e.ID = Convert.ToInt32(dr.GetValue(0));
                Conteudo cont = new Conteudo();
                cont.ID = Convert.ToInt32(dr.GetValue(1));
                e.Conteudo = cont;
                e.Descricao = dr.GetValue(2).ToString();
                e.Tipo = dr.GetValue(3).ToString();
                e.AleatorioQuestao = Convert.ToBoolean(dr.GetValue(4));
                e.Usuario = Convert.ToInt32(dr.GetValue(5));
            }
            dr.Close();
            comm.CommandText = "Select ID_Questao, Ordem_Questao from Questao Where ID_Exercicio = " + id + " order by Ordem_Questao";
            dr = comm.ExecuteReader();
            List<Questao> lista = new List<Questao>();
            while (dr.Read())
            {
                QuestaoDAL dalq = new QuestaoDAL();
                Questao q = new Questao();
                q = dalq.Consultar(Convert.ToInt32(dr.GetValue(0)));
                lista.Add(q);
            }
            e.Questao = lista;
            comm.Connection.Close();
            return e;
        }
        /// <summary>
        /// Retorna todo os individuos de um conteudo
        /// </summary>
        /// <param name="id">id do conteudo</param>
        /// <returns></returns>
        public List<Exercicio> ConsultarTodos(int id)
        {
            SqlCommand comm = new SqlCommand("Select * from Exercicio Where ID_Conteudo = " + id, Banco.Abrir());
            SqlDataReader dr = comm.ExecuteReader();
            List<Exercicio> lista = new List<Exercicio>();
            while (dr.Read())
            {
                Exercicio e = new Exercicio();
                e.ID = Convert.ToInt32(dr.GetValue(0));
                Conteudo cont = new Conteudo();
                cont.ID = Convert.ToInt32(dr.GetValue(1));
                e.Conteudo = cont;
                e.Descricao = dr.GetValue(2).ToString();
                e.Tipo = dr.GetValue(3).ToString();
                e.AleatorioQuestao = Convert.ToBoolean(dr.GetValue(4));
                e.Usuario = Convert.ToInt32(dr.GetValue(5));
                lista.Add(e);
            }
            comm.Connection.Close();
            return lista;
        }
        /// <summary>
        /// Corrige o exercicio e retorna um objeto do tipo nota com configuração total de Usuario
        /// </summary>
        /// <param name="Realizado"> parametro do tipe Exercicio sendo o Exercicio realizado </param>
        /// <param name="Gabarito">parametro do tipe Exercicio sendo o Exercicio que servirá como gabarito</param>
        /// <returns></returns>
        public Nota Corrigir(Exercicio Realizado, Exercicio Gabarito)
        {
            decimal TotalQuestoes = Gabarito.Questao.Count;
            decimal acerto = 0;
            foreach (var itemGabaritoQ in Gabarito.Questao)
            {
                foreach (var itemRealizadoQ in Realizado.Questao)
                {
                    if (itemGabaritoQ.Ordem == itemRealizadoQ.Ordem)
                    {
                        foreach (var itemGabaritoA in itemGabaritoQ.Alternativa)
                        {
                            foreach (var itemRealizadoA in itemRealizadoQ.Alternativa)
                            {
                                if (itemRealizadoA.Ordem == itemGabaritoA.Ordem)
                                {
                                    if (itemRealizadoA.Tipo == "R")
                                    {
                                        if (itemGabaritoA.Tipo == "C")
                                        {
                                            if (itemRealizadoA.Conteudo == itemGabaritoA.Conteudo)
                                            {
                                                if (itemGabaritoQ.Tipo == "A")
                                                {
                                                    acerto += 1;
                                                }
                                                else
                                                {
                                                    decimal valor = ((decimal)1 / (decimal)itemGabaritoQ.Alternativa.Count);
                                                    acerto += Convert.ToDecimal(valor);
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }

            decimal pontoquestao = 10 / TotalQuestoes;
            decimal nota = pontoquestao * acerto;
            Nota n = new Nota
            {
                _Nota = nota,
                Exercicio = Realizado
            };
            Usuario u = new Usuario();
            UsuarioDAL dal = new UsuarioDAL();
            u = dal.Consultar(Realizado.Usuario);
            n.Usuario = u;
            return n;
        }
    }
}
