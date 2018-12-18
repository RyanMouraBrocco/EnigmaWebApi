using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EnigmaWebApi.Models;

namespace EnigmaWebApi.Controllers
{
    [RoutePrefix("api/Avaliacao")]
    public class AvaliacaoController : ApiController
    {
        [HttpGet]
        [Route("pergunta/byid/{usuario}/{pergunta}")]
        public Avaliacao GetPerguntaID(int usuario,int pergunta)
        {
            AvaliacaoDAL dal = new AvaliacaoDAL();
            Avaliacao a = new Avaliacao();
            a = dal.Consultar(usuario,pergunta,0);
            return a;
        }

        [HttpGet]
        [Route("pergunta/allpositivos/{pergunta}")]
        public int GetPerguntaPositivo(int pergunta)
        {
            AvaliacaoDAL dal = new AvaliacaoDAL();
            return dal.AvaliacaoPossitiva(pergunta,0);
        }
        [HttpGet]
        [Route("pergunta/allnegativos/{pergunta}")]
        public int GetPerguntaNegativo(int pergunta)
        {
            AvaliacaoDAL dal = new AvaliacaoDAL();
            return dal.AvaliacaoNegativa(pergunta, 0);
        }

        [HttpGet]
        [Route("pergunta/allDenuncias/{pergunta}")]
        public int GetPerguntaDenuncia(int pergunta)
        {
            AvaliacaoDAL dal = new AvaliacaoDAL();
            return dal.Denuncias(pergunta,0);
        }

        [HttpPost]
        [Route("pergunta/inserir")]
        public void InserirPergunta(Avaliacao A)
        {
            AvaliacaoDAL dal = new AvaliacaoDAL();
            dal.Inserir(A);
        }
        [HttpPost]
        [Route("pergunta/alterar")]
        public void AlterarPergunta(Avaliacao A)
        {
            AvaliacaoDAL dal = new AvaliacaoDAL();
            dal.Alterar(A);
        }

        [HttpGet]
        [Route("resposta/byid/{usuario}/{resposta}")]
        public Avaliacao GetRespostaID(int usuario, int resposta)
        {
            AvaliacaoDAL dal = new AvaliacaoDAL();
            Avaliacao a = new Avaliacao();
            a = dal.Consultar(usuario, 0, resposta);
            return a;
        }
        [HttpGet]
        [Route("resposta/allpositivos/{resposta}")]
        public int GeRespostaositivo(int resposta)
        {
            AvaliacaoDAL dal = new AvaliacaoDAL();
            return dal.AvaliacaoPossitiva(0,resposta);
        }
        [HttpGet]
        [Route("resposta/allnegativos/{resposta}")]
        public int GetRespostaNegativo(int resposta)
        {
            AvaliacaoDAL dal = new AvaliacaoDAL();
            return dal.AvaliacaoNegativa(0, resposta);
        }

        [HttpGet]
        [Route("resposta/allDenuncias/{resposta}")]
        public int GetRespostaDenuncia(int resposta)
        {
            AvaliacaoDAL dal = new AvaliacaoDAL();
            return dal.Denuncias(0, resposta);
        }

        [HttpPost]
        [Route("resposta/inserir")]
        public void InserirResposta(Avaliacao A)
        {
            AvaliacaoDAL dal = new AvaliacaoDAL();
            dal.Inserir(A);
        }
        [HttpPost]
        [Route("resposta/alterar")]
        public void AlterarResposta(Avaliacao A)
        {
            AvaliacaoDAL dal = new AvaliacaoDAL();
            dal.Alterar(A);
        }
    }
}
