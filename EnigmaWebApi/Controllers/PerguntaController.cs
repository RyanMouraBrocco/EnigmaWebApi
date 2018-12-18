using EnigmaWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EnigmaWebApi.Controllers
{
    [RoutePrefix("api/Pergunta")]
    public class PerguntaController : ApiController
    {
        [HttpGet]
        [Route("byid/{id}")]
        public Pergunta GetID(int id)
        {
            Pergunta P = new Pergunta();
            PerguntaDAL dal = new PerguntaDAL();
             P = dal.Consultar(id);
            return P;
        }
        [HttpGet]
        [Route("all")]
        public List<Pergunta> GetAll()
        {
            PerguntaDAL dal = new PerguntaDAL();
            return dal.ConsultarTodos();
        }
        [HttpGet]
        [Route("byusuario/{id}")]
        public List<Pergunta> GetAllbyUser(int id)
        {
            PerguntaDAL dal = new PerguntaDAL();
            List<Pergunta> perguntas = dal.ConsultarPorUsuario(id);
            return perguntas;
        }
        [HttpPost]
        [Route("inserir")]
        public void Inserir(Pergunta P)
        {
            PerguntaDAL dal = new PerguntaDAL();
            dal.Inserir(P);
        }
        [HttpPost]
        [Route("alterar")]
        public void Alterar(Pergunta P)
        {
            PerguntaDAL dal = new PerguntaDAL();
            dal.Alterar(P);
        }
    }
}
