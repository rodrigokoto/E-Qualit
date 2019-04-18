using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class UsuarioClienteSite : BaseEntidade
   {
      [Required(ErrorMessageResourceName = "UsuarioClienteSite_msg_erro_required_IdUsuarioClienteSite", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "UsuarioClienteSite_lbl_IdUsuarioClienteSite", ResourceType = typeof(Traducao.Resource))]
      public int IdUsuarioClienteSite { get; set; }

      [Required(ErrorMessageResourceName = "UsuarioClienteSite_msg_erro_required_IdCliente", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "UsuarioClienteSite_lbl_IdCliente", ResourceType = typeof(Traducao.Resource))]
      public int IdCliente { get; set; }

      [Required(ErrorMessageResourceName = "UsuarioClienteSite_msg_erro_required_IdUsuario", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "UsuarioClienteSite_lbl_IdUsuario", ResourceType = typeof(Traducao.Resource))]
      public int IdUsuario { get; set; }

      [Display(Name = "UsuarioClienteSite_lbl_IdSite", ResourceType = typeof(Traducao.Resource))]
      public int? IdSite { get; set; }


      public Site Site { get; set; }
      public CtrlUsuario CtrlUsuario { get; set; }
      public Cliente Cliente { get; set; }
   }
}