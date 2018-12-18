using System.Collections.Generic;

namespace EnigmaWebApi.Models
{
    public class Resposta
    {
        public Resposta()
        {
        }

        public int ID { get; set; }
        public Pergunta Pergunta { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public bool Visibilidade { get; set; }
        public int Usuario { get; set; }
        public List<Imagem> Imagem { get; set; }
    }
}
