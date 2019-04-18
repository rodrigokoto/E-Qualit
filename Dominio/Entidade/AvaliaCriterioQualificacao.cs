using DomainValidation.Interfaces.Validation;
using DomainValidation.Validation;
using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class AvaliaCriterioQualificacao : ISelfValidator
    {
        public AvaliaCriterioQualificacao()
        {
            ArquivosDeEvidenciaAux = new List<Anexo>();
            ArquivosEvidenciaCriterioQualificacao = new List<ArquivosEvidenciaCriterioQualificacao>();
        }
                
        public int IdAvaliaCriterioQualificacao { get; set; }
        public int IdCriterioQualificacao { get; set; }
        public int IdFornecedor { get; set; }
        public DateTime? DtVencimento { get; set; }
        public bool? Aprovado { get; set; }
        public string ArquivoEvidencia { get; set; }
        public DateTime? DtEmissao { get; set; }
        public DateTime? DtQualificacaoVencimento { get; set; }
        public int ? IdResponsavelPorControlarVencimento { get; set; }
        public int IdResponsavelPorQualificar { get; set; }
        public string NumeroDocumento { get; set; }
        public string Observacoes { get; set; }
        public string ObservacoesDocumento { get; set; }
        public string OrgaoExpedidor { get; set; }
        public DateTime? DtAlteracaoEmissao { get; set; }
        public ValidationResult ValidationResult { get; set; }

        public List<Anexo> ArquivosDeEvidenciaAux { get; set; }
        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }

        #region Relacionamento

        public virtual Fornecedor Fornecedor { get; set; }
        public virtual CriterioQualificacao CriterioQualificacao { get; set; }
        public virtual Usuario ResponsavelPorControlarVencimento { get; set; }
        public virtual Usuario ResponsavelPorQualificar { get; set; }
        public virtual List<ArquivosEvidenciaCriterioQualificacao> ArquivosEvidenciaCriterioQualificacao { get; set; }

        #endregion
    }
}
