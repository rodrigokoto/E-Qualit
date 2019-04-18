using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class Funcao : BaseEntidade
   {
      [Required(ErrorMessageResourceName = "Funcao_msg_erro_required_IdFuncao", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "Funcao_lbl_IdFuncao", ResourceType = typeof(Traducao.Resource))]
      public int IdFuncao { get; set; }

      [Required(ErrorMessageResourceName = "Funcao_msg_erro_required_IdFuncionalidade", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "Funcao_lbl_IdFuncionalidade", ResourceType = typeof(Traducao.Resource))]
      public int IdFuncionalidade { get; set; }

      [Required(ErrorMessageResourceName = "Funcao_msg_erro_required_NmNome", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [StringLength(40, ErrorMessageResourceName = "Funcao_msg_erro_length_NmNome", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "Funcao_lbl_NmNome", ResourceType = typeof(Traducao.Resource))]
      public string NmNome { get; set; }

      [Required(ErrorMessageResourceName = "Funcao_msg_erro_required_NuOrdem", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "Funcao_lbl_NuOrdem", ResourceType = typeof(Traducao.Resource))]
      public int NuOrdem { get; set; }

      public bool FlSelecionado { get; set; }


      public Funcionalidade Funcionalidade { get; set; }
   }
}