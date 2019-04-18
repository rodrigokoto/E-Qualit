using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.NaoConformidades
{
    public class CamposObrigatoriosSegundaEtapaAtaulizacaoAcaoImediata : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosSegundaEtapaAtaulizacaoAcaoImediata()
        {
            RuleFor(x => x.DtDescricaoAcao)
                .GreaterThanOrEqualTo(x => x.DtEmissao.Date)
                .WithMessage(Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_save_valid_DtDescricao);

        }
    }
}
