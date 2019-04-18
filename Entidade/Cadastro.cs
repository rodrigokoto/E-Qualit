using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class Cadastro : BaseEntidade
    {
        [Required(ErrorMessageResourceName = "Cadastro_msg_erro_required_IdCadastro", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cadastro_lbl_IdCadastro", ResourceType = typeof(Traducao.Resource))]
        public int IdCadastro { get; set; }

        [Required(ErrorMessageResourceName = "Cadastro_msg_erro_required_IdSite", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cadastro_lbl_IdSite", ResourceType = typeof(Traducao.Resource))]
        public int IdSite { get; set; }

        [Required(ErrorMessageResourceName = "Cadastro_msg_erro_required_DsDescricao", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(70, ErrorMessageResourceName = "Cadastro_msg_erro_length_DsDescricao", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cadastro_lbl_DsDescricao", ResourceType = typeof(Traducao.Resource))]
        public string DsDescricao { get; set; }

        [Required(ErrorMessageResourceName = "Cadastro_msg_erro_required_CdTabela", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(10, ErrorMessageResourceName = "Cadastro_msg_erro_length_CdTabela", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cadastro_lbl_CdTabela", ResourceType = typeof(Traducao.Resource))]
        public string CdTabela { get; set; }

        [Required(ErrorMessageResourceName = "Cadastro_msg_erro_required_FlAtivo", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Cadastro_lbl_FlAtivo", ResourceType = typeof(Traducao.Resource))]
        public bool FlAtivo { get; set; }


        public Site Site { get; set; }
    }
}
