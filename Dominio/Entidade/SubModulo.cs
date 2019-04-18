using System;

namespace Dominio.Entidade
{
    public class SubModulo
    {
        public int Codigo { get; set; }

        public string Descricao { get; set; }

        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public int CodigoSite { get; set; }
        public virtual Site Site { get; set; }

        public int CodigoFuncionalidade { get; set; }
        public virtual Funcionalidade Funcionalidade { get; set; }

    }
}
