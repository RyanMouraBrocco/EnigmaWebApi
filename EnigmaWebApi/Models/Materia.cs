using System.Collections.Generic;

namespace EnigmaWebApi.Models
{
    public class Materia
    {
        public Materia()
        {
        }

        public int ID { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public byte[] Imagem { get; set; }
        public List<Conteudo> Conteudo{ get; set; }
        public int Usuario { get; set; }
    }
}
