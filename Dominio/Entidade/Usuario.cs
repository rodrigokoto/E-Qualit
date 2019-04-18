using DomainValidation.Interfaces.Validation;
using DomainValidation.Validation;
using Dominio.Enumerado;
using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class Usuario : ISelfValidator
    {
        public Usuario()
        {
            Notificacoes = new List<Notificacao>();
            AcoesImediatas = new List<RegistroAcaoImediata>();
            RegistrosResponsavelAcaoCorretiva = new List<RegistroConformidade>();
            Registros1 = new List<RegistroConformidade>();
            Registros2 = new List<RegistroConformidade>();
            RegistrosEmissor = new List<RegistroConformidade>();
            Registros4 = new List<RegistroConformidade>();
            RegistrosInicarAcaoImediata = new List<RegistroConformidade>();
            UsuarioClienteSites = new List<UsuarioClienteSite>();
            UsuarioCargoes = new List<UsuarioCargo>();
            DtAlteracaoSenha = DateTime.Now;
            FotoPerfil = new List<UsuarioAnexo>();
        }

        public int IdUsuario { get; set; }
        public Guid? Token { get; set; } 
        public int IdPerfil { get; set; }
        public string NmCompleto { get; set; }
        public string CdIdentificacao { get; set; }
        public string NuCPF { get; set; }
        public string CdSenha { get; set; }
        public DateTime? DtExpiracao { get; set; }
        public string NmFuncao { get; set; }
        public bool FlCompartilhado { get; set; }
        public bool FlRecebeEmail { get; set; }
        public bool FlBloqueado { get; set; }
        public bool FlAtivo { get; set; }
        public string FlSexo { get; set; }
        public DateTime? DtUltimoAcesso { get; set; }
        public int NuFalhaLNoLogin { get; set; }
        public DateTime? DtAlteracaoSenha { get; set; }
        public int? IdUsuarioIncluiu { get; set; }
        public DateTime? DtInclusao { get; set; }
        public DateTime? DtAlteracao { get; set; }

        public string SenhaAtual { get; set; }

        public string ConfirmaSenha { get; set; }

        public Anexo FotoPerfilAux { get; set; }

        public virtual ICollection<UsuarioAnexo> FotoPerfil { get; set; }

        public virtual ICollection<Notificacao> Notificacoes { get; set; }
        public virtual ICollection<RegistroAcaoImediata> AcoesImediatas { get; set; }
        public virtual ICollection<RegistroConformidade> RegistrosResponsavelAcaoCorretiva { get; set; }
        public virtual ICollection<RegistroConformidade> RegistrosEmissor { get; set; }
        public virtual ICollection<RegistroConformidade> Registros1 { get; set; }
        public virtual ICollection<RegistroConformidade> Registros2 { get; set; }
        public virtual ICollection<RegistroConformidade> Registros4 { get; set; }
        public virtual ICollection<RegistroConformidade> RegistrosInicarAcaoImediata { get; set; }
        public virtual ICollection<RegistroConformidade> RegistrosResponsavelEtapa { get; set; }

        public virtual ICollection<Fornecedor> UsuarioAvaliacaoFornecedor { get; set; }
        public virtual ICollection<AvaliaCriterioAvaliacao> UsuariAvaliaCriterioAvaliacaoFornecedor { get; set; }

        public virtual ICollection<UsuarioClienteSite> UsuarioClienteSites { get; set; }
        public virtual ICollection<UsuarioCargo> UsuarioCargoes { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }
        public virtual ICollection<AvaliaCriterioQualificacao> CriteriosQualificacaoPorControleVencimento { get; set; }
        public virtual ICollection<AvaliaCriterioQualificacao> CriteriosQualificacaoPorQualificar { get; set; }
        public virtual List<AnaliseCritica> AnalisesCriticas { get; set; }
        public virtual ICollection<UsuarioSenha> UsuarioSenha { get; set; }
        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }

        public bool EAdministrador(int idPerfil)
        {
            if (idPerfil != (int)PerfisAcesso.Administrador)
            {
                return false;
            }
            return true;
        }

        public bool ESuporte(int idPerfil)
        {
            if (idPerfil != (int)PerfisAcesso.Suporte)
            {
                return false;
            }
            return true;
        }

        public bool ECoordenador(int idPerfil)
        {
            if (idPerfil != (int)PerfisAcesso.Coordenador)
            {
                return false;
            }
            return true;
        }

        public bool EColaborador(int idPerfil)
        {
            if (idPerfil != (int)PerfisAcesso.Colaborador)
            {
                return false;
            }
            return true;
        }

    }
}
