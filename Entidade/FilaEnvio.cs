using System.ComponentModel.DataAnnotations;
using System;

namespace Entidade
{
    public class FilaEnvio: BaseEntidade
	{
		[Required(ErrorMessageResourceName = "FilaEnvio_msg_erro_required_IdFilaEnvio", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "FilaEnvio_lbl_IdFilaEnvio", ResourceType = typeof(Traducao.Resource))]
		public int IdFilaEnvio { get; set; }

		[Display(Name = "FilaEnvio_lbl_IdCampanha", ResourceType = typeof(Traducao.Resource))]
		public int? IdCampanha { get; set; }

		[Required(ErrorMessageResourceName = "FilaEnvio_msg_erro_required_DtInclusao", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "FilaEnvio_lbl_DtInclusao", ResourceType = typeof(Traducao.Resource))]
		public DateTime DtInclusao { get; set; }

		[Display(Name = "FilaEnvio_lbl_DtDisparo", ResourceType = typeof(Traducao.Resource))]
		public DateTime? DtDisparo { get; set; }

		[Display(Name = "FilaEnvio_lbl_DtEnvio", ResourceType = typeof(Traducao.Resource))]
		public DateTime? DtEnvio { get; set; }

		[Required(ErrorMessageResourceName = "FilaEnvio_msg_erro_required_DsEmailTo", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(200, ErrorMessageResourceName = "FilaEnvio_msg_erro_length_DsEmailTo", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "FilaEnvio_lbl_DsEmailTo", ResourceType = typeof(Traducao.Resource))]
		public string DsEmailTo { get; set; }

		[StringLength(200, ErrorMessageResourceName = "FilaEnvio_msg_erro_length_DsEmailFrom", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "FilaEnvio_lbl_DsEmailFrom", ResourceType = typeof(Traducao.Resource))]
		public string DsEmailFrom { get; set; }

		[Required(ErrorMessageResourceName = "FilaEnvio_msg_erro_required_DsAssunto", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(200, ErrorMessageResourceName = "FilaEnvio_msg_erro_length_DsAssunto", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "FilaEnvio_lbl_DsAssunto", ResourceType = typeof(Traducao.Resource))]
		public string DsAssunto { get; set; }

		[Required(ErrorMessageResourceName = "FilaEnvio_msg_erro_required_DsBody", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[StringLength(8000, ErrorMessageResourceName = "FilaEnvio_msg_erro_length_DsBody", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "FilaEnvio_lbl_DsBody", ResourceType = typeof(Traducao.Resource))]
		public string DsBody { get; set; }

		[StringLength(8000, ErrorMessageResourceName = "FilaEnvio_msg_erro_length_DsErro", ErrorMessageResourceType = typeof(Traducao.Resource))]
		[Display(Name = "FilaEnvio_lbl_DsErro", ResourceType = typeof(Traducao.Resource))]
		public string DsErro { get; set; }


	}
}