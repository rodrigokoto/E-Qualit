using System;

namespace Dominio.Entidade
{
    public class Meta
    {
        public int Id { get; set; }
        public double Valor { get; set; }
        public byte UnidadeMedida { get; set; }
        public DateTime DataReferencia { get; set; }
        public string Mes
        {
            get
            {
                return DataReferencia.ToString("MMMM");
            }
        }
        public int IdPeriodicidadeAnalise { get; set; }
        public virtual PeriodicidaDeAnalise PeriodicidadeAnalise { get; set; }
    }
}
