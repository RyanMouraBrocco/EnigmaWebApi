using System.Collections.Generic;

namespace EnigmaWebApi.Models
{
    public class Pergunta
    {
        public Pergunta()
        {
        }

        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public bool Visibilidade { get; set; }
        public List<Imagem> Imagem { get; set; }
        public List<Resposta> Resposta { get; set; }
        public int Usuario { get; set; }

    }
}
