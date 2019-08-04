using DomainValidation.Interfaces.Validation;
using DomainValidation.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Entidade
{
    public class Anexo : ISelfValidator
    {


        public int IdAnexo { get; set; }
        public string Nome { get; set; }
        public string Extensao { get; set; }
        public byte[] Arquivo { get; set; }
        public DateTime DtCriacao { get; set; }
        public DateTime DtAlteracao { get; set; }

        //informação somente da tela, solicitando para apagar
        public int ApagarAnexo { get; set; } = 0;


        //Aux
        public string ArquivoB64 { get; set; }


        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }

        public void Tratar()
        {
            Nome = Extensao;
            TratarComNomeCerto();
        }

        public void TratarComNomeCerto()
        {
            if (Nome == null)
                Nome = Extensao;
            Arquivo = TransformaString64EmBase64(ArquivoB64);
            Extensao = RetornaExtensao(Extensao);

            if (IdAnexo == 0)
            {
                DtCriacao = DateTime.Now;
                DtAlteracao = DateTime.Now;
            }
            else
            {
                DtAlteracao = DateTime.Now;
            }
        }

        public string TrataAnexoVindoBanco()
        {
            return String.Format("data:application/" + Extensao + ";base64," + Convert.ToBase64String(Arquivo));
        }

        protected byte[] TransformaString64EmBase64(string strB64) =>
            strB64 == null ? Encoding.ASCII.GetBytes("") : Convert.FromBase64String(strB64);

        protected string RetornaExtensao(string nomeArquivo) =>
            nomeArquivo != null ? nomeArquivo.Substring(nomeArquivo.LastIndexOf(".")) : "";

        #region Relacionamento
        public virtual ICollection<ArquivoCertificadoAnexo> ArquivoCertificadoAnexo { get; set; }
        public virtual ICollection<ArquivoInstrumentoAnexo> ArquivoInstrumentoAnexo { get; set; }
        public virtual ICollection<ArquivoDocDocumentoAnexo> ArquivoDocDocumentoAnexo { get; set; }
        public virtual ICollection<ArquivoNaoConformidadeAnexo> ArquivoNaoConformidadeAnexo { get; set; }
        public virtual ICollection<ArquivoPlaiAnexo> ArquivoPlaiAnexo { get; set; }
        public virtual ICollection<ArquivoDeEvidenciaAcaoImediata> ArquivoEvidenciaAcaoImediata { get; set; }
        public virtual ICollection<UsuarioAnexo> FotosUsuario { get; set; }
        public virtual ICollection<SiteAnexo> FotosSite { get; set; }

        public virtual ICollection<ClienteLogo> ClientesLogo { get; set; }
        public virtual ICollection<ClienteContrato> ClientesContratos { get; set; }
        public virtual ICollection<ArquivosDeEvidencia> ArquivosDeEvidencia { get; set; }
        public virtual ICollection<ArquivosEvidenciaCriterioQualificacao> ArquivosEvidenciaCriterioQualificacao { get; set; }


        public virtual ICollection<Usuario> Usuarios { get; set; }


        public virtual ICollection<Licenca> Licencas { get; set; }
        public virtual ICollection<DocExterno> DocsExterno { get; set; }

        #endregion
    }
}
