using DomainValidation.Interfaces.Validation;
using System;
using System.Collections.Generic;
using DomainValidation.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidade
{
    public class DocDocumento : ISelfValidator
    {
        public DocDocumento()
        {
            DocCargo = new List<DocumentoCargo>();
            DocTemplate = new List<DocTemplate>();
            DocUsuarioVerificaAprova = new List<DocUsuarioVerificaAprova>();
            Verificadores = new List<DocUsuarioVerificaAprova>();
            Aprovadores = new List<DocUsuarioVerificaAprova>();
            DtAlteracao = DateTime.Now;
            Assuntos = new List<DocumentoAssunto>();
            Comentarios = new List<DocumentoComentario>();
            Rotinas = new List<DocRotina>();
            Registros = new List<DocRegistro>();
            StatusRegistro = 0;
            Indicadores = new List<DocIndicadores>();
            DocRisco = new List<DocRisco>();
            //GestaoDeRisco = new List<RegistroConformidade>();
        }

        public int IdDocumento { get; set; }

        public int IdCategoria { get; set; }
        public int IdSigla { get; set; }
        public int IdElaborador { get; set; }
        public int IdSite { get; set; }
        public int IdDocIdentificador { get; set; }

        public int? IdProcesso { get; set; }
        public int? IdDocumentoPai { get; set; }

        public string Titulo { get; set; }
        public decimal NumeroDocumento { get; set; }
        public bool FlWorkFlow { get; set; }
        public bool FlRevisaoPeriodica { get; set; }
        public byte FlStatus { get; set; }
        public string CorRisco { get; set; }
        public DateTime DtAlteracao { get; set; }

        public bool? Ativo { get; set; }
        public byte? NuRevisao { get; set; }
        public int? IdUsuarioIncluiu { get; set; }
        public bool? PossuiGestaoRisco { get; set; }
        public DateTime? DtPedidoVerificacao { get; set; }
        public DateTime? DtVerificacao { get; set; }
        public DateTime? DtVencimento { get; set; }
        public DateTime? DtEmissao { get; set; }
        public DateTime? DtPedidoAprovacao { get; set; }
        public DateTime? DtAprovacao { get; set; }
        public DateTime? DtNotificacao { get; set; }
        public DateTime? DtInclusao { get; set; }

        public string XmlMetadata { get; set; }
        public DocDocumentoXML ConteudoDocumento { get; set; }

        public int? IdLicenca { get; set; }
        public int? IdDocExterno { get; set; }
        public int? IdGestaoDeRisco { get; set; }

        public string EntradaTextoDoc { get; set; }
        public string SaidaTextoDoc { get; set; }
        public string TextoDoc { get; set; }
        public string FluxoDoc { get; set; }
        [NotMapped]
        public string FluxoBase64 { get; set; }
        public string RecursoDoc { get; set; }
        public byte StatusRegistro { get; set; }
        public int? IdUsuarioAlteracao { get; set; }

        #region Relacionamentos
        public virtual Processo Processo { get; set; }
        public virtual ControladorCategoria Categoria { get; set; }
        public virtual ControladorCategoria Sigla { get; set; }
        public virtual Usuario Elaborador { get; set; }
        public virtual RegistroConformidade GestaoDeRisco { get; set; }

        public virtual ICollection<ArquivoDocDocumentoAnexo> ArquivoDocDocumentoAnexo { get; set; }
        
        public virtual List<DocumentoCargo> DocCargo { get; set; }
        public virtual List<DocTemplate> DocTemplate { get; set; }
        public virtual List<DocUsuarioVerificaAprova> DocUsuarioVerificaAprova { get; set; }
        public virtual List<DocUsuarioVerificaAprova> Verificadores { get; set; }
        public virtual List<DocUsuarioVerificaAprova> Aprovadores { get; set; }
        public virtual List<DocumentoComentario> Comentarios { get; set; }
        public virtual List<DocumentoAssunto> Assuntos { get; set; }

        public virtual Licenca Licenca { get; set; }
        public virtual DocExterno DocExterno { get; set; }

        //novo
        public virtual List<DocIndicadores> Indicadores { get; set; }
        public virtual List<DocRotina> Rotinas { get; set; }
        public virtual List<DocRegistro> Registros { get; set; }
        public virtual List<DocRisco> DocRisco { get; set; }



        #endregion

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
