using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class Endereco: BaseEntidade
	{
		[Required(ErrorMessageResourceName = "Endereco_msg_erro_required_IdEndereco", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Endereco_lbl_IdEndereco", ResourceType = typeof(Traducao.Resource))]
		public int IdEndereco { get; set; }

		[Required(ErrorMessageResourceName = "Endereco_msg_erro_required_IdRelacionado", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Endereco_lbl_IdRelacionado", ResourceType = typeof(Traducao.Resource))]
		public int IdRelacionado { get; set; }

		[Required(ErrorMessageResourceName = "Endereco_msg_erro_required_FlTipoEndereco", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(3, ErrorMessageResourceName = "Endereco_msg_erro_length_FlTipoEndereco", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Endereco_lbl_FlTipoEndereco", ResourceType = typeof(Traducao.Resource))]
		public string FlTipoEndereco { get; set; }

		[Required(ErrorMessageResourceName = "Endereco_msg_erro_required_DsLogradouro", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(60, ErrorMessageResourceName = "Endereco_msg_erro_length_DsLogradouro", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Endereco_lbl_DsLogradouro", ResourceType = typeof(Traducao.Resource))]
		public string DsLogradouro { get; set; }

		[Required(ErrorMessageResourceName = "Endereco_msg_erro_required_NuNumero", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(15, ErrorMessageResourceName = "Endereco_msg_erro_length_NuNumero", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Endereco_lbl_NuNumero", ResourceType = typeof(Traducao.Resource))]
		public string NuNumero { get; set; }

		[StringLength(30, ErrorMessageResourceName = "Endereco_msg_erro_length_DsComplemento", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Endereco_lbl_DsComplemento", ResourceType = typeof(Traducao.Resource))]
		public string DsComplemento { get; set; }

		[Required(ErrorMessageResourceName = "Endereco_msg_erro_required_NmBairro", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(30, ErrorMessageResourceName = "Endereco_msg_erro_length_NmBairro", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Endereco_lbl_NmBairro", ResourceType = typeof(Traducao.Resource))]
		public string NmBairro { get; set; }

		[Required(ErrorMessageResourceName = "Endereco_msg_erro_required_NmCidade", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(30, ErrorMessageResourceName = "Endereco_msg_erro_length_NmCidade", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Endereco_lbl_NmCidade", ResourceType = typeof(Traducao.Resource))]
		public string NmCidade { get; set; }

		[Required(ErrorMessageResourceName = "Endereco_msg_erro_required_CdEstado", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(2, ErrorMessageResourceName = "Endereco_msg_erro_length_CdEstado", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Endereco_lbl_CdEstado", ResourceType = typeof(Traducao.Resource))]
		public string CdEstado { get; set; }

		[Required(ErrorMessageResourceName = "Endereco_msg_erro_required_NuCep", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(9, ErrorMessageResourceName = "Endereco_msg_erro_length_NuCep", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Endereco_lbl_NuCep", ResourceType = typeof(Traducao.Resource))]
		public string NuCep { get; set; }

		[StringLength(32, ErrorMessageResourceName = "Endereco_msg_erro_length_DsPais", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Endereco_lbl_DsPais", ResourceType = typeof(Traducao.Resource))]
		public string DsPais { get; set; }


	}
}