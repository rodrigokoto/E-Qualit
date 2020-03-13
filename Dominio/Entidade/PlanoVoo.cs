using System;

namespace Dominio.Entidade
{
    public class PlanoVoo
    {
        public int Id { get; set; }
        public double? Realizado { get; set; }
        public DateTime DataReferencia { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataInclusao { get; set; }
        public bool AtingiuAMeta { get; set; }
        public string Analise { get; set; }

        public int? IdGestaoRisco { get; set; }
        public int? CorRisco { get; set; }

        public int IdPeriodicidadeAnalise { get; set; }
        public int? IdProcesso { get; set; }
        public virtual Processo Processo { get; set; }
        public virtual PeriodicidaDeAnalise PeriodicidadeAnalise { get; set; }
        public virtual RegistroConformidade GestaoDeRisco { get; set; }

    }
}
