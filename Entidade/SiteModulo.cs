using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class SiteModulo : BaseEntidade
   {
      [Required(ErrorMessageResourceName = "SiteModulo_msg_erro_required_IdSiteModulo", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "SiteModulo_lbl_IdSiteModulo", ResourceType = typeof(Traducao.Resource))]
      public int IdSiteModulo { get; set; }

      [Required(ErrorMessageResourceName = "SiteModulo_msg_erro_required_IdSite", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "SiteModulo_lbl_IdSite", ResourceType = typeof(Traducao.Resource))]
      public int IdSite { get; set; }

      [Required(ErrorMessageResourceName = "SiteModulo_msg_erro_required_IdModulo", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "SiteModulo_lbl_IdModulo", ResourceType = typeof(Traducao.Resource))]
      public int IdModulo { get; set; }


      public Funcionalidade Funcionalidade { get; set; }
      public Site Site { get; set; }
   }
}