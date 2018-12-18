using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EnigmaWebApi.Models;

namespace EnigmaWebApi.Controllers
{
    [RoutePrefix("api/Desempenho")]
    public class DesempenhoController : ApiController
    {
        [HttpGet]
        [Route("bymateriaandusuario/{materia}/{usuario}")]
        public Desempenho GetMateriaUsuario(int materia,int usuario)
        {
            DesempenhoDAL dal = new DesempenhoDAL();
            return dal.Consultar(materia,usuario);
        }

        [HttpPost]
        [Route("atualizar")]
        public void Atualizar(Desempenho D)
        {
            DesempenhoDAL dal = new DesempenhoDAL();
            int materia = D.Materia.ID, usuario = D.Usuario.ID;
            decimal hestudadas = D.HorasEstudadas;
            D = dal.Consultar(D.Materia.ID, D.Usuario.ID);
            if (D.ID == 0)
            {
                D.Materia = new Materia { ID = materia };
                D.Usuario = new Usuario { ID = usuario };
                dal.Inserir(D);
            }
            else
            {
                dal.Alterar(D);
            }
        }
        [HttpPost]
        [Route("atualizarhoras")]
        public void AtualizarHoras(Desempenho D)
        {
            DesempenhoDAL dal = new DesempenhoDAL();
            int materia = D.Materia.ID, usuario = D.Usuario.ID;
            decimal hestudadas = D.HorasEstudadas;
            D = dal.Consultar(D.Materia.ID, D.Usuario.ID);
            if (D.ID == 0)
            {
                D.Materia = new Materia { ID = materia };
                D.Usuario = new Usuario { ID = usuario };
                D.HorasEstudadas = hestudadas;
                dal.Inserir(D);
            }
            else
            {
                D.HorasEstudadas += hestudadas;
                dal.Alterar(D);
            }
        }
    }
}
