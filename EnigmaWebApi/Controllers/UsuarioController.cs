using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EnigmaWebApi.Models;
namespace EnigmaWebApi.Controllers
{
    [RoutePrefix("api/Usuario")]
    public class UsuarioController : ApiController
    {
        [HttpGet]
        [Route("bylogin")]
        public Usuario GetUsuario(string login)
        {
            Usuario u = new Usuario();
            UsuarioDAL dal = new UsuarioDAL();
            u = dal.Consultar(login);
            return u;
        }
        [HttpGet]
        [Route("byid/{id}")]
        public Usuario GetUsuario(int id)
        {
            Usuario u = new Usuario();
            UsuarioDAL dal = new UsuarioDAL();
            u = dal.Consultar(id);
            return u;
        }
        [HttpGet]
        [Route("logar/{login}/{senha}")]
        public bool Logar(string login,string senha)
        {
            UsuarioDAL dal = new UsuarioDAL();
            return dal.Logar(login, senha);
        }
        [HttpPost]
        [Route("inserir")]
        public void Inserir(Usuario u)
        {
            UsuarioDAL dal = new UsuarioDAL();
            dal.Inserir(u);
        }
        [HttpPost]
        [Route("alterar")]
        public void Alterar(Usuario u)
        {
            UsuarioDAL dal = new UsuarioDAL();
            dal.AlterarSemSenha(u);
        }
    }
}
