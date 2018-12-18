namespace EnigmaWebApi.Models
{
    public class Video
    {
        public Video()
        {
        }

        public int ID { get; set; }
        public string Nome { get; set; }
        public string Link { get; set; }
        public decimal Duracao { get; set; }
        public decimal Inicio { get; set; }
        public decimal Fim { get; set; }
        public int Usuario { get; set; }
    }
}
