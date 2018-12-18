using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EnigmaWebApi.Models;

namespace EnigmaWebApi.Controllers
{
    [RoutePrefix("api/Materia")]
    public class MateriaController : ApiController
    {
        [HttpGet]
        [Route("byid/{id}")]
        public Materia GetMateria(int id)
        {
            MateriaDAL dal = new MateriaDAL();
            Materia m = dal.Consultar(id);
            m.Conteudo = null;
            if (m.ID == 0)
            {
                m = new Materia();
            }
            return m;
        }
        [HttpGet]
        [Route("bynome/{Nome}")]
        public Materia GetMateria(string nome)
        {
            MateriaDAL dal = new MateriaDAL();
            Materia m = dal.Consultar(nome);
            m.Conteudo = null;
            if (m.ID == 0)
            {
                m = new Materia();
            }
            return m;
        }
        [HttpGet]
        [Route("all")]
        public List<Materia> GetALLMateria()
        {
            MateriaDAL dal = new MateriaDAL();
            List<Materia> m = new List<Materia>();
            m = dal.ConsultarTodos();
            return m;
        }
    }
}
