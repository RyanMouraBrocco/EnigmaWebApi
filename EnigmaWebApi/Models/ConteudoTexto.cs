namespace EnigmaWebApi.Models
{
    public class ConteudoTexto
    {
        public ConteudoTexto()
        {
        }

        public int ID { get; set; }
        public Conteudo Conteudo { get; set; }
        public Texto Texto { get; set; }
        public Video Video { get; set; }
        public Imagem Imagem { get; set; }
        public int Ordem { get; set; }
        public int Usuario { get; set; }
    }
}
