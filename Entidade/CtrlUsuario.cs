using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using DomainValidation.Interfaces.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidade
{
    public class CtrlUsuario : BaseEntidade, ISelfValidator
    {
        public CtrlUsuario()
        {
            FlRecebeEmail = false;
            LvPerfil = new List<CtrlPerfil>();
            UsuarioCargo = new List<UsuarioCargo>();
            UsuarioClienteSite = new List<UsuarioClienteSite>();
            LvCliente = new List<Cliente>();
            LvSite = new List<UsuarioSite>();
            LvProcesso = new List<Processo>();
        }

        public int IdUsuarioLogado { get; set; }

        [Required(ErrorMessageResourceName = "CtrlUsuario_msg_erro_required_IdUsuario", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_IdUsuario", ResourceType = typeof(Traducao.Resource))]
        public int IdUsuario { get; set; }

        [Required(ErrorMessageResourceName = "CtrlUsuario_msg_erro_required_IdPerfil", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_IdPerfil", ResourceType = typeof(Traducao.Resource))]
        public int IdPerfil { get; set; }

        [Required(ErrorMessageResourceName = "CtrlUsuario_msg_erro_required_NmCompleto", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(30, ErrorMessageResourceName = "CtrlUsuario_msg_erro_length_NmCompleto", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_NmCompleto", ResourceType = typeof(Traducao.Resource))]
        public string NmCompleto { get; set; }

        [Required(ErrorMessageResourceName = "CtrlUsuario_msg_erro_required_CdIdentificacao", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [EmailAddress(ErrorMessageResourceName = "mensagem_erro_email_invalido", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(120, ErrorMessageResourceName = "CtrlUsuario_msg_erro_length_CdIdentificacao", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_CdIdentificacao", ResourceType = typeof(Traducao.Resource))]
        public string CdIdentificacao { get; set; }

        [StringLength(14, ErrorMessageResourceName = "CtrlUsuario_msg_erro_length_NuCPF", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_NuCPF", ResourceType = typeof(Traducao.Resource))]
        public string NuCPF { get; set; }

        [Required(ErrorMessageResourceName = "CtrlUsuario_msg_erro_required_CdSenha", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(120, ErrorMessageResourceName = "CtrlUsuario_msg_erro_length_CdSenha", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_CdSenha", ResourceType = typeof(Traducao.Resource))]
        public string CdSenha { get; set; }

        [Display(Name = "CtrlUsuario_lbl_DtExpiracao", ResourceType = typeof(Traducao.Resource))]
        public string DtExpiracao { get; set; }

        public bool ArquivoTempo { get; set; }

        //[StringLength(80, ErrorMessageResourceName = "CtrlUsuario_msg_erro_length_NmArquivoFoto", ErrorMessageResourceType = typeof(Traducao.Resource))]
        //[Display(Name = "CtrlUsuario_lbl_NmArquivoFoto", ResourceType = typeof(Traducao.Resource))]
        //public string NmArquivoFoto { get; set; }
        
        //public string UrlArquivoFoto
        //{
        //    get
        //    {
        //        if (ArquivoTempo)
        //        {
        //            return string.Format("/content/usuario/{0}", NmArquivoFoto);
        //        }

        //        if (string.IsNullOrEmpty(NmArquivoFoto) || IdUsuario == 0)
        //        {
        //            return "/content/assets/imagens/avatar.jpg";
        //        }
        //        return string.Format("/content/usuario/{0}/{1}", IdUsuario, NmArquivoFoto);
        //    }
        //}

        [StringLength(100, ErrorMessageResourceName = "CtrlUsuario_msg_erro_length_NmFuncao", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_NmFuncao", ResourceType = typeof(Traducao.Resource))]
        public string NmFuncao { get; set; }

        [Required(ErrorMessageResourceName = "CtrlUsuario_msg_erro_required_FlCompartilhado", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_FlCompartilhado", ResourceType = typeof(Traducao.Resource))]
        public bool FlCompartilhado { get; set; }

        [Display(Name = "CtrlUsuario_lbl_FlRecebeEmail", ResourceType = typeof(Traducao.Resource))]
        public bool? FlRecebeEmail { get; set; }

        [Required(ErrorMessageResourceName = "CtrlUsuario_msg_erro_required_FlBloqueado", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_FlBloqueado", ResourceType = typeof(Traducao.Resource))]
        public bool FlBloqueado { get; set; }

        [Required(ErrorMessageResourceName = "CtrlUsuario_msg_erro_required_FlAtivo", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_FlAtivo", ResourceType = typeof(Traducao.Resource))]
        public bool FlAtivo { get; set; }

        [Required(ErrorMessageResourceName = "CtrlUsuario_msg_erro_required_FlSexo", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(1, ErrorMessageResourceName = "CtrlUsuario_msg_erro_length_FlSexo", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_FlSexo", ResourceType = typeof(Traducao.Resource))]
        public string FlSexo { get; set; }

        [Display(Name = "CtrlUsuario_lbl_DtUltimoAcesso", ResourceType = typeof(Traducao.Resource))]
        public DateTime? DtUltimoAcesso { get; set; }

        [Required(ErrorMessageResourceName = "CtrlUsuario_msg_erro_required_NuFalhaLNoLogin", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_NuFalhaLNoLogin", ResourceType = typeof(Traducao.Resource))]
        public int NuFalhaLNoLogin { get; set; }

        [Required(ErrorMessageResourceName = "CtrlUsuario_msg_erro_required_DtAlteracaoSenha", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlUsuario_lbl_DtAlteracaoSenha", ResourceType = typeof(Traducao.Resource))]
        public DateTime DtAlteracaoSenha { get; set; }

        [Display(Name = "Data de Inclusão")]
        public DateTime? DtInclusao { get; set; }

        public bool EPaginaCliente { get; set; }

        public List<CtrlPerfil> LvPerfil { get; set; }
        public List<UsuarioClienteSite> UsuarioClienteSite { get; set; }
        public List<UsuarioCargo> UsuarioCargo { get; set; }
        public List<UsuarioSite> LvSite { get; set; }
        public List<Cliente> LvCliente { get; set; }
        public List<Processo> LvProcesso { get; set; }

        [Display(Name = "Cliente")]
        public string Clientes { get; set; }

        [NotMapped]
        public DomainValidation.Validation.ValidationResult ValidationResult { get; set; }

    }
    
    public class UsuarioSite
    {
        public UsuarioSite()
        {
            Cargos = new List<UsuarioCargos>();
        }

        public int IdSite { get; set; }
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public bool FlSelecionado { get; set; }
        public List<UsuarioCargos> Cargos { get; set; }
    }

    public class UsuarioCargos
    {
        public int IdSite { get; set; }
        public int IdCargo { get; set; }
        public string Nome { get; set; }
        public bool FlSelecionado { get; set; }
    }
}