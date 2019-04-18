using System;

namespace Dominio.Entidade
{
    public class AnaliseCriticaFuncionario
    {
        public int IdAnaliseCriticaFuncionario { get; set; }

        public int IdUsuario { get; set; }
        public virtual Usuario Funcionario { get; set; }

        public int IdAnaliseCritica { get; set; }
        public virtual AnaliseCritica AnaliseCritica { get; set; }

        public string Funcao { get; set; }

        public DateTime? DataCadastro { get; set; }

        public bool Ativo { get; set; }
    }
}
