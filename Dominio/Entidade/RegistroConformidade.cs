using DomainValidation.Interfaces.Validation;
using System;
using System.Collections.Generic;
using DomainValidation.Validation;
using Dominio.Enumerado;

namespace Dominio.Entidade
{
    public class RegistroConformidade : ISelfValidator
    {
        public RegistroConformidade()
        {
            DtInclusao = DateTime.Now;
            DtAlteracao = DateTime.Now;
            DtEmissao = DateTime.Now;
            AcoesImediatas = new List<RegistroAcaoImediata>();
            ArquivosDeEvidenciaAux = new List<Anexo>();
            ArquivosDeEvidencia = new List<ArquivosDeEvidencia>();
        }

        public int IdSite { get; set; }
        public int? IdProcesso { get; set; }
        public int IdEmissor { get; set; }
        public int? IdNaoConformidade { get; set; }



        public int? IdResponsavelAcaoCorretiva { get; set; } //

        public int? IdResponsavelReverificador { get; set; }

        public int? IdResponsavelAnalisar { get; set; }

        public int? IdResponsavelImplementar { get; set; }

        public int? IdTipoNaoConformidade { get; set; }
        public int? IdNuRegistroFilho { get; set; }

        public int? IdResponsavelEtapa { get; set; }

        public int IdUsuarioIncluiu { get; set; }
        public int IdUsuarioAlterou { get; set; }


        public string TipoRegistro { get; set; } // ac, gr, nc
        public int NuRegistro { get; set; }
        public string DsAcao { get; set; }

        //public string EvidenciaImg { get; set; }
        //public DateTime? DtRegistroEvidencia { get; set; }

        public List<Anexo> ArquivosDeEvidenciaAux { get; set; }




        public DateTime? DtAnalise { get; set; }
        public DateTime? DtDescricaoAcao { get; set; }
        public DateTime? DtEfetivaImplementacao { get; set; }
        public DateTime DtEmissao { get; set; }
        public DateTime? DtEnceramento { get; set; }
        public DateTime? DtPrazoImplementacao { get; set; }
        public byte FlDesbloqueado { get; set; }
        public bool? FlEficaz { get; set; }
        public byte? FlStatusAntesAnulacao { get; set; }
        public byte? StatusEtapa { get; set; }

        public DateTime DtInclusao { get; set; }
        public DateTime? DtAlteracao { get; set; }
        public bool? EProcedente { get; set; }
        public bool? ECorrecao { get; set; }
        public string JustificativaAnulacao { get; set; }
        public string Parecer { get; set; }
        public string DescricaoAcao { get; set; }


        public bool? NecessitaAcaoCorretiva { get; set; }
        public bool? ENaoConformidadeAuditoria { get; set; }
        //risco
        public int? CriticidadeGestaoDeRisco { get; set; } //enum.CriticidadeGestaoDeRisco
        public int? IdResponsavelInicarAcaoImediata { get; set; } // Por Adicionar Acoes Imediatas
        public string DescricaoRegistro { get; set; } //o que falhou?
        public string Causa { get; set; }
        public int IdRegistroConformidade { get; set; }
        public string DsJustificativa { get; set; }


        public byte StatusRegistro { get; set; }
        public int? IdRegistroPai { get; set; }

        public string Tags { get; set; }


        public IEnumerable<ArquivoNaoConformidadeAnexo> ArquivosNaoConformidadeAnexos { get; set; } = new List<ArquivoNaoConformidadeAnexo>();

        #region Relacionamentos

        public virtual ICollection<ArquivosDeEvidencia> ArquivosDeEvidencia { get; set; }
        public virtual Usuario ResponsavelAcaoCorretiva { get; set; }
        public virtual Usuario ResponsavelAnalisar { get; set; }
        public virtual Usuario ResponsavelReverificador { get; set; }
        public virtual Usuario Emissor { get; set; }
        public virtual Usuario ResponsavelImplementar { get; set; }
        public virtual Usuario ResponsavelEtapa { get; set; }
        public virtual List<RegistroAcaoImediata> AcoesImediatas { get; set; }
        public virtual Site Site { get; set; }
        public virtual ControladorCategoria TipoNaoConformidade { get; set; }
        public virtual Usuario ResponsavelInicarAcaoImediata { get; set; }
        public virtual Processo Processo { get; set; }

        #endregion
        // Auxiliar
        public string DescricaoAnaliseCausa { get; set; }

        // Auxiliar
        public int IdResponsavelPorIniciarTratativaAcaoCorretiva { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }

        public bool OStatusEAcaoImediata()
        {
            return StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata;
        }

        public bool OStatusEEncerrada()
        {
            return StatusEtapa == (byte)EtapasRegistroConformidade.Encerrada;
        }

        public bool OStatusEImplementacao()
        {
            return StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao;
        }

        public bool OStatusEReverificacao()
        {
            return StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao;
        }
    }
}
