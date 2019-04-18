using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class CargoProcesso : BaseEntidade
   {
      [Required(ErrorMessageResourceName = "CargoProcesso_msg_erro_required_IdCargoProcesso", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "CargoProcesso_lbl_IdCargoProcesso", ResourceType = typeof(Traducao.Resource))]
      public int IdCargoProcesso { get; set; }

      [Required(ErrorMessageResourceName = "CargoProcesso_msg_erro_required_IdProcesso", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "CargoProcesso_lbl_IdProcesso", ResourceType = typeof(Traducao.Resource))]
      public int IdProcesso { get; set; }

      [Required(ErrorMessageResourceName = "CargoProcesso_msg_erro_required_IdCargo", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "CargoProcesso_lbl_IdCargo", ResourceType = typeof(Traducao.Resource))]
      public int IdCargo { get; set; }

      [Required(ErrorMessageResourceName = "CargoProcesso_msg_erro_required_IdFuncao", ErrorMessageResourceType = typeof(Traducao.Resource))]
      [Display(Name = "CargoProcesso_lbl_IdFuncao", ResourceType = typeof(Traducao.Resource))]
      public int IdFuncao { get; set; }


      public Funcao Funcao { get; set; }
      public Cargo Cargo { get; set; }
      public Processo Processo { get; set; }
   }
}