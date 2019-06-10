using DomainValidation.Validation;
using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class Calibracao
    {
        public Calibracao()
        {
            ArquivoCertificado = new List<ArquivoCertificadoAnexo>();
            ArquivoCertificadoAux = new List<Anexo>();
        }
        public int IdCalibracao { get; set; }
        public int IdInstrumento { get; set; }
        public string Certificado { get; set; }
        public string OrgaoCalibrador { get; set; }
        public Byte Aprovado { get; set; }
        public long? IdFilaEnvio { get; set; }
        public int Aprovador { get; set; }
        public virtual Usuario  UsuarioAprovador{ get; set; }

        public String NomeUsuarioAprovador { get; set; }

        public string Observacoes { get; set; }

        //public string ArquivoCertificado { get; set; }

        public List<Anexo> ArquivoCertificadoAux { get; set; }
        public virtual ICollection<ArquivoCertificadoAnexo> ArquivoCertificado { get; set; }

        public DateTime ? DataCalibracao { get; set; }
        public DateTime ? DataProximaCalibracao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public int IdUsuarioIncluiu { get; set; }
        public string StatusCalibracao { get { return (Aprovado == 1 ? "Sim" : "Não"); } }
        public DateTime ? DataRegistro { get; set; }
        public DateTime ? DataNotificacao { get; set; }
        public virtual List<CriterioAceitacao> CriterioAceitacao { get; set; }

        public virtual Instrumento Instrumento { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
