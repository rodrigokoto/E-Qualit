using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class ListaValor: BaseEntidade
	{
		[Required(ErrorMessageResourceName = "ListaValor_msg_erro_required_IdListaValor", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "ListaValor_lbl_IdListaValor", ResourceType = typeof(Traducao.Resource))]
		public int IdListaValor { get; set; }

		[Required(ErrorMessageResourceName = "ListaValor_msg_erro_required_CdTabela", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(10, ErrorMessageResourceName = "ListaValor_msg_erro_length_CdTabela", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "ListaValor_lbl_CdTabela", ResourceType = typeof(Traducao.Resource))]
		public string CdTabela { get; set; }

		[Required(ErrorMessageResourceName = "ListaValor_msg_erro_required_CdCodigo", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(40, ErrorMessageResourceName = "ListaValor_msg_erro_length_CdCodigo", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "ListaValor_lbl_CdCodigo", ResourceType = typeof(Traducao.Resource))]
		public string CdCodigo { get; set; }

		[Required(ErrorMessageResourceName = "ListaValor_msg_erro_required_DsDescricao", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(8000, ErrorMessageResourceName = "ListaValor_msg_erro_length_DsDescricao", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "ListaValor_lbl_DsDescricao", ResourceType = typeof(Traducao.Resource))]
		public string DsDescricao { get; set; }


	}
}