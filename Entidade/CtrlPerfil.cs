using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class CtrlPerfil : BaseEntidade
    {
        [Required(ErrorMessageResourceName = "CtrlPerfil_msg_erro_required_IdPerfil", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlPerfil_lbl_IdPerfil", ResourceType = typeof(Traducao.Resource))]
        public int IdPerfil { get; set; }

        [Required(ErrorMessageResourceName = "CtrlPerfil_msg_erro_required_NmNome", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(30, ErrorMessageResourceName = "CtrlPerfil_msg_erro_length_NmNome", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "CtrlPerfil_lbl_NmNome", ResourceType = typeof(Traducao.Resource))]
        public string NmNome { get; set; }


    }
}