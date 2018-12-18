using EnigmaWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EnigmaWebApi.Controllers
{
    [RoutePrefix("api/Resposta")]
    public class RespostaController : ApiController
    {
        [HttpPost]
        [Route("inserir")]
        public void Inserir(Resposta r)
        {
            RespostaDAL dal = new RespostaDAL();
            dal.Inserir(r);
        }
        [HttpPost]
        [Route("alterar")]
        public void Alterar(Resposta r)
        {
            RespostaDAL dal = new RespostaDAL();
            dal.Alterar(r);
        }
        [HttpGet]
        [Route("byusuario/{id}")]
        public List<Resposta> GetAllbyUser(int id)
        {
            RespostaDAL dal = new RespostaDAL();
            List<Resposta> respostas = dal.ConsultarPorUsuario(id);
            return respostas;
        }
    }
}
