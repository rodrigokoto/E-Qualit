using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class Pai
    {
        public Pai()
        {
            Processos = new List<Processo>();
        }
        public int IdPai { get; set; }
        public int IdSite { get; set; }
        public int Ano { get; set; }

        public DateTime DataCadastro { get; set; }
        public int IdGestor { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Site Site { get; set; }
        public virtual List<Plai> Plais { get; set; }
        public virtual List<Processo> Processos { get; set; }
    }
}
