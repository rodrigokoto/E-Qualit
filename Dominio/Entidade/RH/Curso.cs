using System;

namespace Dominio.Entidade.RH
{
    public class Curso : Base
    {
        public string Descricao { get; set; }
        public string Entidade { get; set; }
        public DateTime DataValidade { get; set; }
        public int CodigoCompetencia { get; set; }
        public virtual Competencia Competencia { get; set; }
    }
}
