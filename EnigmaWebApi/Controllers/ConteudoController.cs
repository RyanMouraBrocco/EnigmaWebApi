using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EnigmaWebApi.Models;

namespace EnigmaWebApi.Controllers
{
    [RoutePrefix("api/Conteudo")]
    public class ConteudoController : ApiController
    {
        [HttpGet]
        [Route("bymateria/{id}")]
        public List<Conteudo> GetForMateria(int id)
        {
            ConteudoDAL dal = new ConteudoDAL();
            return dal.ConsultarPorMateria(id);
        }
        [HttpGet]
        [Route("byid/{id}")]
        public Conteudo Get(int id)
        {
            ConteudoDAL dal = new ConteudoDAL();
            return dal.Consultar(id);
        }
    }
}
