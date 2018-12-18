using System.Collections.Generic;

namespace EnigmaWebApi.Models
{
    public class Conteudo
    {
        public Conteudo()
        {
        }

        public int ID { get; set; }
        public Materia Materia{ get; set; }
        public string Nome { get; set; }
        public byte[] Imagem { get; set; }
        public int Ordem { get; set; }
        public List<ConteudoTexto> ConteudoTexto { get; set; }
        public List<Exercicio> Exercicio { get; set; }
        public List<Resumo> Resumo { get; set; }
        public int Usuario { get; set; }
    }
}
