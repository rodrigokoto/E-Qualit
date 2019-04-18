using System.Collections.Generic;

namespace Dominio.Entidade.RH
{
    public class Competencia : Base
    {
        public int NivelEscolaridade { get; set; }
        public int NivelFormacaoAcademica { get; set; }
        public int Tipo { get; set; }

        public virtual List<FormacaoAcademica> FormacoesAcademicas { get; set; }
        public virtual List<Conhecimento> Conhecimentos { get; set; }
        public virtual List<Habilidade> Habilidades { get; set; }
        public virtual List<Curso> Cursos { get; set; }
        public virtual List<Treinamento> Treinamentos { get; set; }
        public virtual List<Atribuicao> Atribuicoes { get; set; }
    }
}
