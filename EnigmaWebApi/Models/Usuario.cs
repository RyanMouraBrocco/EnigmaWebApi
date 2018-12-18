namespace EnigmaWebApi.Models
{
    public class Usuario
    {
        public Usuario()
        {
        }

        public int ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string TipoConta { get; set; }
        public byte[] Foto { get; set; }
    }
}
