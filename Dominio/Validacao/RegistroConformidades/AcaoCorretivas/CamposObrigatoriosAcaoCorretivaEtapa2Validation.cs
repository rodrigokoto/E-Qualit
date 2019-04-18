using Dominio.Entidade;
using Dominio.Enumerado;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.AcaoCorretivas
{
    public class CamposObrigatoriosAcaoCorretivaEtapa2Validation : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosAcaoCorretivaEtapa2Validation()
        {

            RuleFor(x => x.AcoesImediatas)
              .Must(x => x.Count > 0).WithMessage(Traducao.Resource.MsgAcaoImediataNecessaria);

            RuleFor(x => x.StatusEtapa)
                .NotEmpty().WithMessage(Traducao.Resource.MsgNaoPodeVazio)
                .Equal((byte)EtapasRegistroConformidade.Implementacao).WithMessage(Traducao.Resource.MsgItemEtapaImplementacao);

            RuleFor(x => x.DtDescricaoAcao.Value.AddSeconds(1))
               .GreaterThanOrEqualTo(x => x.DtEmissao)
               .WithMessage(Traducao.Resource.DtDescricaoDeveSerMaiorQueADataDeRegistro);       



        }
    }
}
