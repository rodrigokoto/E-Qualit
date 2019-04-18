using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Entidade
{
    public partial class Cliente : BaseEntidade
    {
        public Cliente()
        {
            LvArmazenarSenhas = new List<ListaValor>();
            LvBloquearAcesso = new List<ListaValor>();
            LvSite = new List<Site>();
            LvTrocarSenhas = new List<ListaValor>();
            Usuario = new CtrlUsuario();
            LvUsuarioClienteSite = new List<UsuarioClienteSite>();
        }

        [Required(ErrorMessageResourceName = "Cliente_msg_erro_required_IdCliente", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cliente_lbl_IdCliente", ResourceType = typeof(Traducao.Resource))]
        public int IdCliente { get; set; }

        [Required(ErrorMessageResourceName = "Cliente_msg_erro_required_NmFantasia", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(40, ErrorMessageResourceName = "Cliente_msg_erro_length_NmFantasia", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cliente_lbl_NmFantasia", ResourceType = typeof(Traducao.Resource))]
        public string NmFantasia { get; set; }
        public int Url { get; set; }

        public bool ArquivoTempo { get; set; }

        [StringLength(80, ErrorMessageResourceName = "Cliente_msg_erro_length_NmLogo", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cliente_lbl_NmLogo", ResourceType = typeof(Traducao.Resource))]
        public string NmLogo { get; set; }
        public string UrlArquivoLogo
        {
            get
            {
                if (ArquivoTempo)
                {
                    return string.Format("/content/cliente/{0}", NmLogo);
                }

                if (string.IsNullOrEmpty(NmLogo) || IdCliente == 0)
                {
                    return "/content/assets/imagens/avatar.jpg";
                }
                return string.Format("/content/cliente/{0}/{1}", IdCliente, NmLogo);
            }
        }

        [StringLength(1000, ErrorMessageResourceName = "Cliente_msg_erro_length_NmAquivoContrato", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cliente_lbl_NmAquivoContrato", ResourceType = typeof(Traducao.Resource))]
        public string NmAquivoContrato { get; set; }

        //[Url(ErrorMessageResourceName = "mensagem_nmSite_invalido", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Required(ErrorMessageResourceName = "Cliente_msg_erro_required_NmUrlAcesso", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(60, ErrorMessageResourceName = "Cliente_msg_erro_length_NmUrlAcesso", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cliente_lbl_NmUrlAcesso", ResourceType = typeof(Traducao.Resource))]
        public string NmUrlAcesso { get; set; }

        [Required(ErrorMessageResourceName = "Cliente_msg_erro_required_DtValidadeContrato", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cliente_lbl_DtValidadeContrato", ResourceType = typeof(Traducao.Resource))]
        public string DtValidadeContrato { get; set; }

        [Required(ErrorMessageResourceName = "Cliente_msg_erro_required_NuDiasTrocaSenha", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cliente_lbl_NuDiasTrocaSenha", ResourceType = typeof(Traducao.Resource))]
        public int NuDiasTrocaSenha { get; set; }

        [Display(Name = "Cliente_lbl_NuTentativaBloqueioLogin", ResourceType = typeof(Traducao.Resource))]
        public int? NuTentativaBloqueioLogin { get; set; }

        [Display(Name = "Cliente_lbl_NuArmazenaSenha", ResourceType = typeof(Traducao.Resource))]
        public int? NuArmazenaSenha { get; set; }

        [Display(Name = "Cliente_lbl_FlExigeSenhaForte", ResourceType = typeof(Traducao.Resource))]
        public bool FlExigeSenhaForte { get; set; }

        [Required(ErrorMessageResourceName = "Cliente_msg_erro_required_FlTemCaptcha", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cliente_lbl_FlTemCaptcha", ResourceType = typeof(Traducao.Resource))]
        public bool FlTemCaptcha { get; set; }

        [Display(Name = "Cliente_lbl_FlEstruturaAprovador", ResourceType = typeof(Traducao.Resource))]
        public bool? FlEstruturaAprovador { get; set; }

        [Required(ErrorMessageResourceName = "Cliente_msg_erro_required_FlAtivo", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cliente_lbl_FlAtivo", ResourceType = typeof(Traducao.Resource))]
        public bool FlAtivo { get; set; }

        public bool FlSelecionado { get; set; }

        public List<UsuarioClienteSite> LvUsuarioClienteSite { get; set; }
        public List<Site> LvSite { get; set; }

        public Site Site { get; set; }
        public CtrlUsuario Usuario { get; set; }
        public Processo Processo { get; set; }

        public List<ListaValor> LvArmazenarSenhas { get; set; }
        public List<ListaValor> LvBloquearAcesso { get; set; }
        public List<ListaValor> LvTrocarSenhas { get; set; }
        public int IdUsuarioLogado { get; set; }

    }
}