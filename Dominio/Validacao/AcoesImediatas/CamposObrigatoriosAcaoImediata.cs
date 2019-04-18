using Dominio.Entidade;
using Dominio.Enumerado;
using FluentValidation;
using System;

namespace Dominio.Validacao.AcoesImediatas
{
    public class CamposObrigatoriosAcaoImediata : AbstractValidator<RegistroAcaoImediata>
    {
        public CamposObrigatoriosAcaoImediata()
        {
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage(Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_required_AI_Descricao)
                .Length(4, 2000).WithMessage(Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_required_minlength_max_descricao);

            RuleFor(x => x.DtPrazoImplementacao)
                .NotNull().WithMessage(Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_DtrPrazoImplementacao_required)
                .NotEmpty().WithMessage(Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_DtrPrazoImplementacao_required);

            RuleFor(x => x.IdResponsavelImplementar)
                //.NotNull().WithMessage(Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_required_AI_Responsavel_implementar)
                .Must(x => x > 0).WithMessage(Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_required_AI_Responsavel_implementar);

            RuleFor(x => x.DtPrazoImplementacao.Value.Date)
                .GreaterThanOrEqualTo(x => x.Registro.DtEmissao.Date)
                .WithMessage(Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_save_valid_DtPrazoImplementacao);

            RuleFor(x => x.DtEfetivaImplementacao.Value.Date)
                //.GreaterThanOrEqualTo(x => x.Registro.DtEmissao.Date)
                //.WithMessage(Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_save_valid_DtEvetivaImplementacao)
                .GreaterThanOrEqualTo(x => x.Registro.DtDescricaoAcao)
                .WithMessage(Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_save_valid_DtPrazoImpleNC_msg_save_valid_DtEvetivaImplementacaoDtDescricao)
                .LessThanOrEqualTo   (x => DateTime.Now)
                .WithMessage(Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_save_valid_DtPrazoImpleNC_msg_save_valid_DtEvetivaImplementacaoAtual)
                .When(x=> x.Registro.OStatusEImplementacao() && x.Estado == EstadoObjetoEF.Modified);




        }
    }
}
