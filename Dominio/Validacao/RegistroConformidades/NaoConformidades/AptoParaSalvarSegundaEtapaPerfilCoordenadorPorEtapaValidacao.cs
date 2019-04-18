using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.NaoConformidades
{
    public class AptoParaSalvarSegundaEtapaPerfilCoordenadorPorEtapaValidacao : AbstractValidator<Usuario>
    {
        public AptoParaSalvarSegundaEtapaPerfilCoordenadorPorEtapaValidacao()
        {
            RuleFor(x => x.IdPerfil)
                .NotEqual(3).WithMessage(Traducao.Resource.MsgAptoParaSalvarSegundaEtapaPerfilCoordenadorPorEtapaValidacao);//IdPerfil 3 Coordenador
        }
    }
}
