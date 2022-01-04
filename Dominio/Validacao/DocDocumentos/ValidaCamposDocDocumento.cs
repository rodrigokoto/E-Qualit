using Dominio.Entidade;
using Dominio.Enumerado;
using FluentValidation;
using System.Linq;

namespace Dominio.Validacao.DocDocumentos
{
    public abstract class ValidaCamposDocDocumento<T> : AbstractValidator<T> where T : DocDocumento
    {
        protected void SiteObrigatorio()
        {
            RuleFor(documento => documento.IdSite)
                .NotEmpty().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_IdSite);
        }

        protected void DocumentoIndentificadorObrigatorio()
        {
            RuleFor(documento => documento.IdDocIdentificador)
                .NotEmpty().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_IdDocIdentificador)
                .When(x => x.NuRevisao > 0);
        }

        protected void CategoriaObrigtoria()
        {
            RuleFor(documento => documento.IdCategoria)
                .NotEmpty().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_IdCategoria);
        }

        protected void ElaboradorObrigatorio()
        {
            RuleFor(documento => documento.IdElaborador)
                .NotEmpty().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_IdElaborador);
        }

        protected void TemWorkFlowObrigatorio()
        {
            RuleFor(documento => documento.FlWorkFlow)
               .NotNull().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_FlWorkFlow);
        }

        protected void TemRevisaoPeriodica()
        {
            RuleFor(documento => documento.FlRevisaoPeriodica)
               .NotNull().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_FlRevisaoPeriodica);
        }

        protected void StatusObrigatorio()
        {
            RuleFor(documento => documento.FlStatus)
               .NotNull().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_FlStatus)
               .WithName(Traducao.Resource.StatusEtapa);
        }

        protected void DataAlteracaoObrigatorio()
        {
            RuleFor(documento => documento.DtAlteracao)
               .NotEmpty().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_DtAlteracao)
               .NotNull().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_DtAlteracao)
               .When(x => x.FlRevisaoPeriodica == true);
        }

        protected void TempleteObrigatorio()
        {
            RuleFor(documento => documento.DocTemplate.Count)
            .GreaterThan(0).WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_DocTemplate);
        }

        protected void SiglaObrigatorio()
        {
            RuleFor(documento => documento.IdSigla)
                .NotEmpty().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_IdSigla);
        }

        protected void NumeroDocumentoObrigatorio()
        {
            RuleFor(documento => documento.NumeroDocumento)
                .NotEmpty().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_NumeroDocumento);
        }

        protected void TituloObrigatorio()
        {
            RuleFor(documento => documento.Titulo)
               .NotEmpty().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_Titulo);
        }

        protected void UsuarioInclusorObrigatorio()
        {
            RuleFor(documento => documento.IdUsuarioIncluiu)
                .NotEmpty().WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_IdUsuarioIncluiu)
                .When(x => x.IdDocumento == 0);
        }

        protected void VerificadoresObrigatorio()
        {
            RuleFor(documento => documento.Verificadores.Count)
               .GreaterThan(0)
               .When(x => x.FlWorkFlow)
               .WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_Verificadores);
        }

        protected void AprovadoresObrigatorio()
        {
            RuleFor(documento => documento.Aprovadores.Count)
                .GreaterThan(0)
                .When(x => x.FlWorkFlow)
               .WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_Aprovadores);
        }

        protected void DataNotificacaoObrigatoria()
        {
            RuleFor(documento => documento.DtNotificacao)
                .NotNull()
                .When(x => x.FlRevisaoPeriodica)
                .WithMessage(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_erro_required_DtNotificacao);
        }

        protected void ValidaDocTemplate()
        {
            //RuleFor(x => x.DocTemplate)
            //    .Must(y => y.Count == 1)
            //    .When(z => z.DocTemplate.Any(i => i.TpTemplate == Traducao.Resource.MsgL || i.TpTemplate == Traducao.Resource.MsgDE))
            //    .WithMessage(Traducao.Resource.MsgEscolhaLicencaDocExt);
        }

        protected void ValidaRevisao()
        {
            RuleFor(x => x.Assuntos)
                .Must(y => y.Count > 0)
                .When(z => z.FlStatus == (int)StatusDocumento.Aprovado)
                .WithMessage(Traducao.Resource.MsgRevisaoCriarAssunto);
        }

        protected void ValidaQuantidadeCaracteresCampos()
        {
            RuleFor(documento => documento.RecursoDoc)
                .Length(0, 1000).WithMessage(Traducao.Resource.MsgMaxRecursos500Caracteres);

            //RuleFor(documento => documento.TextoDoc)
            //    .MaximumLength(100000).WithMessage(Traducao.Resource.MsgMaxTextoDoc100000Caracteres);
        }

        public void ValidarDocumentoCargoObrigatorio()
        {
            RuleFor(x => x.DocCargo)
                .Must(y => y.Count > 0)
                .When(z => z.FlWorkFlow == false)
                .WithMessage(Traducao.Resource.DocDocumento_msg_erro_required_DocCargo);
                
            
             //.GreaterThan(0).WithMessage(Traducao.Resource.DocDocumento_msg_erro_required_DocCargo);
        }
    }
}
