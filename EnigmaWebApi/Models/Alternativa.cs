namespace EnigmaWebApi.Models
{
    public class Alternativa
    {

        public int ID { get; set; }
        public Questao Questao { get; set; }
        public string Tipo { get; set; }
        public string Conteudo { get; set; }
        public int Ordem { get; set; }
        public int Usuario { get; set; }

        public Alternativa()
        {
        }

        public Alternativa(int iD, Questao questao, string tipo, string conteudo, int ordem, int usuario)
        {
            ID = iD;
            Questao = questao;
            Tipo = tipo;
            Conteudo = conteudo;
            Ordem = ordem;
            Usuario = usuario;
        }

        public Alternativa(Questao questao, string tipo, string conteudo, int ordem, int usuario)
        {
            Questao = questao;
            Tipo = tipo;
            Conteudo = conteudo;
            Ordem = ordem;
            Usuario = usuario;
        }
    }
}
