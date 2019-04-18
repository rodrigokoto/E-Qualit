using FluentValidation;

namespace Dominio.Validacao.ControladorCategorias
{
    public class CamposObrigatoriosValidation : AbstractValidator<Entidade.ControladorCategoria>
    {
        public CamposObrigatoriosValidation()
        {
            RuleFor(categoria => categoria.Descricao)
                .NotEmpty().WithMessage(Traducao.Resource.MsgTraducaoVObrigatoria)
                .NotNull().WithMessage(Traducao.Resource.MsgTraducaoNull)
                .Length(1, 70).WithMessage(Traducao.Resource.MsgTraducaoMinMax1);

            RuleFor(categoria => categoria.TipoTabela)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgTraducaoMinMax1)
                .Length(1, 10).WithMessage(Traducao.Resource.MsgTraducaoMinMax2);

            RuleFor(categoria => categoria.IdSite)
                .NotEmpty().WithMessage(Traducao.Resource.MsgTraducaoObrigatoria);

            RuleFor(categoria => categoria.Ativo)
                .NotEmpty().WithMessage(Traducao.Resource.MsgTraducaoVObrigatoria)
                .NotNull().WithMessage(Traducao.Resource.MsgTraducaoNull);
        }
    }
}
