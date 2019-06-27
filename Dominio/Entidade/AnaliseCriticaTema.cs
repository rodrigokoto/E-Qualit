using DomainValidation.Interfaces.Validation;
using System;
using DomainValidation.Validation;

namespace Dominio.Entidade
{
    public class AnaliseCriticaTema : ISelfValidator
    {
        public int IdTema { get; set; }
        public int IdControladorCategoria { get; set; }
        public int? IdGestaoDeRisco { get; set; }
        public int IdAnaliseCritica { get; set; }

        public string Descricao { get; set; }
        public int CorRisco { get; set; }
		
		public bool PossuiInformarGestaoRisco { get; set; }
		public bool PossuiGestaoRisco { get; set; }
		public int IdProcesso { get; set; }
		public virtual Processo Processo { get; set; }

		public int Ativo { get; set; }
        public DateTime? DataCadastro { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }
        #region Relacionamentos
        public virtual AnaliseCritica AnaliseCritica { get; set; }
        public virtual ControladorCategoria ControladorCategoria { get; set; }
        public virtual RegistroConformidade GestaoDeRisco { get; set; }
        #endregion
    }
}
