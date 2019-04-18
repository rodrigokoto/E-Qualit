using Dominio.Entidade;
using FluentValidation;
using System;

namespace Dominio.Validacao.Plais
{
    public class AptoParaCadastroValidacao : AbstractValidator<Plai>
    {
        public AptoParaCadastroValidacao()
        {
            RuleFor(x => x.IdPai)
                .NotEmpty().WithMessage(Traducao.Resource.MsgPaiObrigatorio)
                .NotNull().WithMessage(Traducao.Resource.MsgPaiObrigatorio);

            //RuleFor(x => x.IdSite)
            //    .NotEmpty().WithMessage("Site Obrigatória")
            //    .NotNull().WithMessage("Site Obrigatória");

            RuleFor(x => x.IdRepresentanteDaDirecao)
                .NotEmpty().WithMessage(Traducao.Resource.MsgRepresentanteObrigatorio)
                .NotNull().WithMessage(Traducao.Resource.MsgRepresentanteAplicavelObrigatorio);

            //RuleFor(x => x.Endereco)
            //    .NotEmpty().WithMessage("Endereço Obrigatório")
            //    .NotNull().WithMessage("Endereço Aplicaveis Obrigatório");

            //RuleFor(x => x.Escopo)
            //    .NotEmpty().WithMessage("Escopo Obrigatório")
            //    .NotNull().WithMessage("Escopo Aplicaveis Obrigatório");

            RuleFor(x => x.PlaiGerentes)
                .Must(x=> x.Count > 0).WithMessage(Traducao.Resource.MsgGestor);

            RuleFor(x => Convert.ToInt32(x.DataReuniaoAbertura.ToString("MMyyyy")))
                .Equal(x => Convert.ToInt32(x.Mes.ToString() + x.Pai.Ano))
                .WithMessage(Traducao.Resource.DataAberturaReuniaoDeveEstaDentroDoPeriodoPlai);

            RuleFor(x => Convert.ToInt32(x.DataReuniaoEncerramento.ToString("MMyyyy")))
                .Equal(x => Convert.ToInt32(x.Mes.ToString() + x.Pai.Ano))
                .WithMessage(Traducao.Resource.DataEnceramentoReuniaoDeveEstarDentroDoPeriodoPlai);

            RuleFor(x => x.DataReuniaoAbertura)
                .NotEmpty().WithMessage(Traducao.Resource.MsgDataReuniao)
                .NotNull().WithMessage(Traducao.Resource.MsgDataReuniao)
                .LessThan(x => x.DataReuniaoEncerramento).WithMessage(Traducao.Resource.MsgDataAberturaDeveSerMenorQueADataDeEncerramento);

            RuleFor(x => x.DataReuniaoEncerramento)
                .NotEmpty().WithMessage(Traducao.Resource.MsgDataEncReuniao)
                .NotNull().WithMessage(Traducao.Resource.MsgDataEncReuniao);
        }
    }

}
