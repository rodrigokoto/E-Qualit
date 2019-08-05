using DomainValidation.Interfaces.Validation;
using Dominio.Enumerado;
using System;
using System.Collections.Generic;
using DomainValidation.Validation;

namespace Dominio.Entidade
{
    public class Instrumento : ISelfValidator
    {
        public Instrumento()
        {
            Calibracao = new List<Calibracao>();

            if (IdInstrumento == 0)
            {
                DataCriacao = DateTime.Now;
                DataAlteracao = DateTime.Now;
            }
            else
            {
                DataAlteracao = DateTime.Now;
            }
        }
        public int IdInstrumento { get; set; }
        public int IdSite { get; set; }
        public int ? IdProcesso { get; set; }
        public int? IdResponsavel { get; set; }
        public string Equipamento { get; set; }
        public int Numero { get; set; }
        public string LocalDeUso { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public byte Periodicidade { get; set; }
        public string Escala { get; set; }
        public string valorAceitacao { get; set; }
        public string MenorDivisao { get; set; }
        public byte? Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public int IdUsuarioIncluiu { get; set; }
        public bool SistemaDefineStatus { get; set; }
        public string DescricaoCriterio { get; set; }
        public bool FlagTravado { get; set; }
        public int IdSigla { get; set; }
        public string CorStatus
        {
            get
            {
                if (Status == (byte)EquipamentoStatus.NaoCalibrado)
                {
                    return "Red";
                }

                if (Status == (byte)EquipamentoStatus.Calibrado)
                {
                    return "Green";
                }

                if (Status == (byte)EquipamentoStatus.ForaDeUso)
                {
                    return "Red";
                }

                if (Status == (byte)EquipamentoStatus.EmCalibracao)
                {
                    return "Blue";
                }

                return string.Empty;
            }
        }

        public string NomeStatus
        {
            get
            {
                if (Status == (byte)EquipamentoStatus.NaoCalibrado)
                {
                    return "Não Calibrado";
                }

                if (Status == (byte)EquipamentoStatus.Calibrado)
                {
                    return "Calibrado";
                }

                if (Status == (byte)EquipamentoStatus.ForaDeUso)
                {
                    return "Fora De Uso";
                }

                if (Status == (byte)EquipamentoStatus.EmCalibracao)
                {
                    return "Em Calibração";
                }
                return string.Empty;
            }
        }

        public virtual Processo Processo { get; set; }
        public virtual Site Site { get; set; }
        public virtual List<Calibracao> Calibracao { get; set; }

        #region Relacionamentos
        public virtual ControladorCategoria Sigla { get; set; }
        public virtual ICollection<ArquivoInstrumentoAnexo> ArquivoInstrumento { get; set; }
        #endregion

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
