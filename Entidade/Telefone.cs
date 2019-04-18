using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class Telefone: BaseEntidade
	{
		[Required(ErrorMessageResourceName = "Telefone_msg_erro_required_IdTelefone", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Telefone_lbl_IdTelefone", ResourceType = typeof(Traducao.Resource))]
		public int IdTelefone { get; set; }

		[Required(ErrorMessageResourceName = "Telefone_msg_erro_required_IdRelacionado", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Telefone_lbl_IdRelacionado", ResourceType = typeof(Traducao.Resource))]
		public int IdRelacionado { get; set; }

		[Required(ErrorMessageResourceName = "Telefone_msg_erro_required_FlTipoTelefone", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(3, ErrorMessageResourceName = "Telefone_msg_erro_length_FlTipoTelefone", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Telefone_lbl_FlTipoTelefone", ResourceType = typeof(Traducao.Resource))]
		public string FlTipoTelefone { get; set; }

		[Required(ErrorMessageResourceName = "Telefone_msg_erro_required_NuTelefone", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(15, ErrorMessageResourceName = "Telefone_msg_erro_length_NuTelefone", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Telefone_lbl_NuTelefone", ResourceType = typeof(Traducao.Resource))]
		public string NuTelefone { get; set; }

		[StringLength(15, ErrorMessageResourceName = "Telefone_msg_erro_length_NuRamal", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Telefone_lbl_NuRamal", ResourceType = typeof(Traducao.Resource))]
		public string NuRamal { get; set; }

		[StringLength(300, ErrorMessageResourceName = "Telefone_msg_erro_length_DsObservacao", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "Telefone_lbl_DsObservacao", ResourceType = typeof(Traducao.Resource))]
		public string DsObservacao { get; set; }


	}
}