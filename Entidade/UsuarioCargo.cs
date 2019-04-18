using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class UsuarioCargo : BaseEntidade
   {
      [Required(ErrorMessageResourceName = "UsuarioCargo_msg_erro_required_IdUsuarioProcesso", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "UsuarioCargo_lbl_IdUsuarioProcesso", ResourceType = typeof(Traducao.Resource))]
      public int IdUsuarioProcesso { get; set; }

      [Required(ErrorMessageResourceName = "UsuarioCargo_msg_erro_required_IdUsuario", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "UsuarioCargo_lbl_IdUsuario", ResourceType = typeof(Traducao.Resource))]
      public int IdUsuario { get; set; }

      [Required(ErrorMessageResourceName = "UsuarioCargo_msg_erro_required_IdCargo", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "UsuarioCargo_lbl_IdCargo", ResourceType = typeof(Traducao.Resource))]
      public int IdCargo { get; set; }


      public Cargo Cargo { get; set; }
      public CtrlUsuario CtrlUsuario { get; set; }
   }
}