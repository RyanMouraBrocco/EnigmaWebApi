namespace EnigmaWebApi.Models
{
    public class Imagem
    {
        public Imagem()
        {
        }

        public int ID { get; set; }
        public string Nome { get; set; }
        public byte[] _Imagem { get; set; }
        public string Extensao { get; set; }
        public int Usuario { get; set; }

    }
}
