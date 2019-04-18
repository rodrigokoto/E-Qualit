using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Entidade
{
    public class Processo : BaseEntidade
    {
        [Display(Name = "Processo_lbl_IdProcesso", ResourceType = typeof(Traducao.Resource))]
        [Required(ErrorMessageResourceName = "Processo_msg_erro_required_IdProcesso", ErrorMessageResourceType = typeof(Traducao.Resource))]
        public int IdProcesso { get; set; }

        [Display(Name = "Processo_lbl_IdSite", ResourceType = typeof(Traducao.Resource))]
        [Required(ErrorMessageResourceName = "Processo_msg_erro_required_IdSite", ErrorMessageResourceType = typeof(Traducao.Resource))]
        public int IdSite { get; set; }

        [Display(Name = "Processo_lbl_NmProcesso", ResourceType = typeof(Traducao.Resource))]
        [Required(ErrorMessageResourceName = "Processo_msg_erro_required_NmProcesso", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(60, ErrorMessageResourceName = "Processo_msg_erro_length_NmProcesso", ErrorMessageResourceType = typeof(Traducao.Resource))]
        public string NmProcesso { get; set; }

        [Display(Name = "Processo_lbl_FlAtivo", ResourceType = typeof(Traducao.Resource))]
        [Required(ErrorMessageResourceName = "Processo_msg_erro_required_FlAtivo", ErrorMessageResourceType = typeof(Traducao.Resource))]
        public bool FlAtivo { get; set; }

        public bool FlQualidade { get; set; }

        public Site Site { get; set; }
        public List<CargoProcesso> LvCargoProcesso { get; set; }
    }
}