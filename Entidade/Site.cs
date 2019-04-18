using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace Entidade
{
    public class Site : BaseEntidade
    {

        [Required(ErrorMessageResourceName = "Site_msg_erro_required_IdSite", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Site_lbl_IdSite", ResourceType = typeof(Traducao.Resource))]
        public int IdSite { get; set; }

        [Required(ErrorMessageResourceName = "Site_msg_erro_required_IdCliente", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Site_lbl_IdCliente", ResourceType = typeof(Traducao.Resource))]
        public int IdCliente { get; set; }

        [Display(Name = "Site_lbl_NmLogo", ResourceType = typeof(Traducao.Resource))]
        public string NmLogo { get; set; }

        public string UrlNmLogo
        {
            get
            {
                if (string.IsNullOrEmpty(NmLogo) || IdSite == 0)
                {
                    return "/content/assets/imagens/avatar.jpg";
                }

                if (string.IsNullOrEmpty(NmLogo) || IdSite == 0)
                    return "/content/cliente/{0}/{2}" + NmLogo;
                else
                    return string.Format("/content/cliente/{0}/Site/{1}/{2}?{3}", IdCliente, IdSite, NmLogo, DateTime.Now.Ticks);
            }
        }
        [Required(ErrorMessageResourceName = "Site_msg_erro_required_NmFantasia", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(40, ErrorMessageResourceName = "Site_msg_erro_length_NmFantasia", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Site_lbl_NmFantasia", ResourceType = typeof(Traducao.Resource))]
        public string NmFantasia { get; set; }

        [Required(ErrorMessageResourceName = "Site_msg_erro_required_NmRazaoSocial", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(60, ErrorMessageResourceName = "Site_msg_erro_length_NmRazaoSocial", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Site_lbl_NmRazaoSocial", ResourceType = typeof(Traducao.Resource))]
        public string NmRazaoSocial { get; set; }

        [StringLength(18, ErrorMessageResourceName = "Site_msg_erro_length_NuCNPJ", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Site_lbl_NuCNPJ", ResourceType = typeof(Traducao.Resource))]
        public string NuCNPJ { get; set; }

        [StringLength(2000, ErrorMessageResourceName = "Site_msg_erro_length_DsObservacoes", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Site_lbl_DsObservacoes", ResourceType = typeof(Traducao.Resource))]
        public string DsObservacoes { get; set; }

        [StringLength(150, ErrorMessageResourceName = "Site_msg_erro_length_DsFrase", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Site_lbl_DsFrase", ResourceType = typeof(Traducao.Resource))]
        public string DsFrase { get; set; }

        [Required(ErrorMessageResourceName = "Site_msg_erro_required_FlAtivo", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Site_lbl_FlAtivo", ResourceType = typeof(Traducao.Resource))]
        public bool FlAtivo { get; set; }


        public Cliente Cliente { get; set; }
        public List<Funcionalidade> LvModulo { get; set; }
        public List<Cargo> LvCargo { get; set; }
        public List<Processo> LvProcesso { get; set; }
        public List<UsuarioClienteSite> LvUsuarioClienteSite { get; set; }

        public Site()
        {
            LvModulo = new List<Funcionalidade>();
            LvProcesso = new List<Processo>();
            LvCargo = new List<Cargo>();
            LvUsuarioClienteSite = new List<UsuarioClienteSite>();
            Cliente = new Cliente();
        }
    }
}