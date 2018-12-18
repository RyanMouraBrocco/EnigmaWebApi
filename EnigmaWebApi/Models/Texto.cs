namespace EnigmaWebApi.Models
{
    public class Texto
    {
        public Texto()
        {
        }

        public int ID { get; set; }
        public decimal Tamanho { get; set; }
        public string Cor { get; set; }
        public string Conteudo { get; set; }
        public bool Negrito { get; set; }
        public bool Italico { get; set; }
        public int Usuario { get; set; }
        
    }
}
