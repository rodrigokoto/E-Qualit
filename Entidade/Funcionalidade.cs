using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Entidade
{
    public class Funcionalidade : BaseEntidade
    {
        public Funcionalidade()
        {
            LvCargoProcesso = new List<CargoProcesso>();
            LvFeature = new List<Funcionalidade>();
            LvFuncao = new List<Funcao>();
            LvSite = new List<Site>();
        }

        [Required(ErrorMessageResourceName = "Funcionalidade_msg_erro_required_IdFuncionalidade", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Funcionalidade_lbl_IdFuncionalidade", ResourceType = typeof(Traducao.Resource))]
        public int IdFuncionalidade { get; set; }

        //[Required(ErrorMessageResourceName = "Funcionalidade_msg_erro_required_NmNome", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [StringLength(30, ErrorMessageResourceName = "Funcionalidade_msg_erro_length_NmNome", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Funcionalidade_lbl_NmNome", ResourceType = typeof(Traducao.Resource))]
        public string NmNome { get; set; }

        [Display(Name = "Funcionalidade_lbl_IdFuncionalidadePai", ResourceType = typeof(Traducao.Resource))]
        public int? IdFuncionalidadePai { get; set; }

        [Required(ErrorMessageResourceName = "Funcionalidade_msg_erro_required_NuOrdem", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Funcionalidade_lbl_NuOrdem", ResourceType = typeof(Traducao.Resource))]
        public int NuOrdem { get; set; }

        [StringLength(20, ErrorMessageResourceName = "Funcionalidade_msg_erro_length_CdFormulario", ErrorMessageResourceType = typeof(Traducao.Resource))]
        [Display(Name = "Funcionalidade_lbl_CdFormulario", ResourceType = typeof(Traducao.Resource))]
        public string CdFormulario { get; set; }

        public List<Funcionalidade> LvFeature { get; set; }

        public bool FlSelecionado { get; set; }

        public List<CargoProcesso> LvCargoProcesso { get; set; }
        public List<Site> LvSite { get; set; }
        public List<Funcao> LvFuncao { get; set; }
    }
}