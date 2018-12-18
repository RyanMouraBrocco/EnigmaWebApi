namespace EnigmaWebApi.Models
{
    public class Avaliacao
    {
        public Avaliacao()
        {
        }

        public int ID { get; set; }
        public Pergunta Pergunta { get; set; }
        public Resposta Resposta { get; set; }
        public Usuario Usuario { get; set; }
        public bool _Avaliacao { get; set; }
        public bool Denuncia { get; set; }
    }
}
