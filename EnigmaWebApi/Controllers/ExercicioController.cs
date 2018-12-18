using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EnigmaWebApi.Models;

namespace EnigmaWebApi.Controllers
{
    [RoutePrefix("api/Exercicio")]
    public class ExercicioController : ApiController
    {
        [HttpGet]
        [Route("byid/{id}")]
        public Exercicio GetID(int id)
        {
            ExercicioDAL dal = new ExercicioDAL();
            Exercicio E = new Exercicio();
            E = dal.Consultar(id);
            return E;
        }
        [HttpGet]
        [Route("all/{conteudo}")]
        public List<Exercicio> GetAll(int conteudo)
        {
            ExercicioDAL dal = new ExercicioDAL();
            List<Exercicio> lista = dal.ConsultarTodos(conteudo);
            return lista;
        }
        [HttpPost]
        [Route("corrigir")]
        public void Corrigir(Exercicio realizado)
        {
            ExercicioDAL dal = new ExercicioDAL();
            Nota n = new Nota();
            Exercicio gabarito = new Exercicio();
            gabarito = dal.Consultar(realizado.ID);
            n = dal.Corrigir(realizado, gabarito);
            NotaDAL dalnota = new NotaDAL();
            dalnota.Inserir(n);
            DesempenhoDAL daldesempenho = new DesempenhoDAL();
            ConteudoDAL dalconteudo = new ConteudoDAL();
            gabarito.Conteudo = dalconteudo.Consultar(gabarito.Conteudo.ID);
            Desempenho desempenho = daldesempenho.Consultar(gabarito.Conteudo.Materia.ID, realizado.Usuario);
            if (desempenho.ID == 0)
            {
                desempenho = new Desempenho
                {
                    HorasEstudadas = 0,
                    Materia = gabarito.Conteudo.Materia,
                    Porcentagem = 0,
                    Usuario = new Usuario { ID = realizado.Usuario }
                };
                daldesempenho.Inserir(desempenho);
            }
            else
            {
                daldesempenho.Alterar(desempenho);
            }
        }
    }
}
