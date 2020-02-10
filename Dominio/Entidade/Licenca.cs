using DomainValidation.Interfaces.Validation;
using DomainValidation.Validation;
using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class Licenca : ISelfValidator
    {
        public Licenca()
        {
            ArquivoLicenca = new List<ArquivoLicencaAnexo>();
            ArquivosLicencaAux = new List<Anexo>();
        }

        public int IdLicenca { get; set; }
        public int Idcliente { get; set; }
        public int IdResponsavel { get; set; }
        public int IdProcesso { get; set; }
        public string Titulo { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataProximaNotificacao { get; set; }
        public string Obervacao { get; set; }
        public IEnumerable<ArquivoLicencaAnexo> ArquivosLicencaAnexos { get; set; } = new List<ArquivoLicencaAnexo>();

        public List<Anexo> ArquivosLicencaAux { get; set; }
        public virtual ICollection<ArquivoLicencaAnexo> ArquivoLicenca { get; set; }

        #region Relacionamento

        public virtual Cliente Cliente { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Processo Processo { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }

        #endregion
    }
}