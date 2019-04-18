using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.Produtos
{
    public abstract class ValidaCampoProduto<T> : AbstractValidator<T> where T : Produto
    {
        protected void NomeObrigatorio()
        {
            RuleFor(x => x.Nome)
                  .Length(0, 300).WithMessage(Traducao.Resource.MsgMax300Nome)
                  .NotEmpty().WithMessage(Traducao.Resource.Produto_msg_erro_required_Nome);
        }

        protected void ResponsavelObrigatorio()
        {
            RuleFor(x => x.IdResponsavel)
                .NotEmpty().WithMessage(Traducao.Resource.Produto_msg_erro_required_IdResponsavel);
        }

        protected void SiteObrigatorio()
        {
            RuleFor(x => x.IdSite)
                .NotEmpty().WithMessage(Traducao.Fornecedores.ResourceFornecedores.Produto_msg_erro_required_IdSite)
                .Must(x => x > 0).WithMessage(Traducao.Fornecedores.ResourceFornecedores.Produto_msg_erro_required_IdSite);
        }

        protected void TamanhoMaximoEspecificacao()
        {
            RuleFor(x => x.Especificacao)
                .Length(0, 300).WithMessage(Traducao.Resource.MsgMax300Especificacao);
        }

        protected void TamanhoMaximoCodigo()
        {
            RuleFor(x => x.Tags)
                .Length(0, 300).WithMessage(Traducao.Resource.MsgMax300Codigo);
        }

        protected void StatusNaoCritico()
        {
            RuleFor(x => x.Status)
                .NotEqual(0)
                .WithMessage(Traducao.Resource.MsgNaoCritico)
                .When(z => z.AvaliacoesCriticidade.Count > 0);
        }

    }
}
