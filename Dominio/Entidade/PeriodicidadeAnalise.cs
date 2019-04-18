using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class PeriodicidaDeAnalise
    {
        public int Id { get; set; }
        public byte PeriodoAnalise { get; set; }
        public string Justificativa { get; set; }
        public string CorRisco { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public double RealAcumulado { get; set; }
        public double MetaEstimulada { get; set; }

        public virtual List<Meta> PlanoDeVoo { get; set; }

        public virtual List<PlanoVoo> MetasRealizadas { get; set; }
        
        public int IdIndicador { get; set; }
        public virtual Indicador Indicador { get; set; }

        public int? IdPlanDeAcao { get; set; }
        public virtual RegistroConformidade PlanoDeAcao { get; set; }
    }
}
