using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.Fornecedores
{
    public class AptoParaCadastrarFornecedor : AbstractValidator<Fornecedor>
    {
        public AptoParaCadastrarFornecedor()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage(Traducao.Resource.Fornecedor_msg_erro_required_Nome);

            

        }
    }
}
