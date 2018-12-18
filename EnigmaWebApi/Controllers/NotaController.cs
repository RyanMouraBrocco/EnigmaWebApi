using EnigmaWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EnigmaWebApi.Controllers
{
    [RoutePrefix("api/Nota")]
    public class NotaController : ApiController
    {
        [HttpGet]
        [Route("byusuarioexercicio/{usuario}/{exercicio}")]
        public List<Nota> Consultar(int usuario, int exercicio)
        {
            NotaDAL dal = new NotaDAL();
            return dal.Consultar(usuario, exercicio);
        }
        [HttpGet]
        [Route("ultimo/byusuarioexercicio/{usuario}/{exercicio}")]
        public Nota ConsultarUltimo(int usuario, int exercicio)
        {
            NotaDAL dal = new NotaDAL();
            return dal.ConsultarUltimo(usuario, exercicio);
        }
    }
}
