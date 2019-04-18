using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Entidade
{
    public class Cargo : BaseEntidade
    {
        [Required(ErrorMessageResourceName = "Cargo_msg_erro_required_IdCargo", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cargo_lbl_IdCargo", ResourceType = typeof(Traducao.Resource))]
        public int IdCargo { get; set; }

        [Required(ErrorMessageResourceName = "Cargo_msg_erro_required_IdSite", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cargo_lbl_IdSite", ResourceType = typeof(Traducao.Resource))]
        public int IdSite { get; set; }

        [Required(ErrorMessageResourceName = "Cargo_msg_erro_required_NmNome", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(40, ErrorMessageResourceName = "Cargo_msg_erro_length_NmNome", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cargo_lbl_NmNome", ResourceType = typeof(Traducao.Resource))]
        public string NmNome { get; set; }


        public Site Site { get; set; }
        public List<PermissaoProcesso> LvPermissao { get; set; }
        public List<Processo> LvProcesso { get; set; }
        public List<UsuarioCargo> LvUsuarioCargo { get; set; }
        public List<CargoProcesso> LvCargoProcesso { get; set; }

        public Cargo()
        {
            LvPermissao = new List<PermissaoProcesso>();
            LvCargoProcesso = new List<CargoProcesso>();
            LvUsuarioCargo = new List<UsuarioCargo>();
        }
    }
}