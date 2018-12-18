using System.Collections.Generic;

namespace EnigmaWebApi.Models
{
    public class Questao
    {

        public int ID { get; set; }
        public Exercicio Exercicio { get; set; }
        public int Ordem { get; set; }
        public bool AleatorioAlternativa { get; set; }
        public string Pergunta { get; set; }
        public string Tipo { get; set; }
        public List<Imagem> Imagem { get; set; }
        public List<Alternativa> Alternativa { get; set; }
        public int Usuario { get; set; }

        public Questao()
        {
        }

        public Questao(Exercicio exercicio, int ordem, bool aleatorioAlternativa, string pergunta, string tipo, List<Imagem> imagem, List<Alternativa> alternativa, int usuario)
        {
            Exercicio = exercicio;
            Ordem = ordem;
            AleatorioAlternativa = aleatorioAlternativa;
            Pergunta = pergunta;
            Tipo = tipo;
            Imagem = imagem;
            Alternativa = alternativa;
            Usuario = usuario;
        }

        public Questao(int iD, Exercicio exercicio, int ordem, bool aleatorioAlternativa, string pergunta, string tipo, List<Imagem> imagem, List<Alternativa> alternativa, int usuario)
        {
            ID = iD;
            Exercicio = exercicio;
            Ordem = ordem;
            AleatorioAlternativa = aleatorioAlternativa;
            Pergunta = pergunta;
            Tipo = tipo;
            Imagem = imagem;
            Alternativa = alternativa;
            Usuario = usuario;
        }
    }
}
