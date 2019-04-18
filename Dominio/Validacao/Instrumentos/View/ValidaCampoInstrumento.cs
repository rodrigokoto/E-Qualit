using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.Instrumentos.View
{
    public abstract class ValidaCampoInstrumento<T> : AbstractValidator<T> where T : Instrumento
    {
        protected void IdUsuarioObrigatorio()
        {
            RuleFor(x => x.IdUsuarioIncluiu)
                .NotEmpty().WithMessage(Traducao.Resource.MsgIdObrigatorio);
        }

        protected void DataCriacaoObrigatorio()
        {
            RuleFor(x => x.DataCriacao)
                .NotEmpty().WithMessage(Traducao.Resource.MsgCriacaoObrigatoria);
        }

        protected void DataAlteracaoObrigatorio()
        {
            RuleFor(x => x.IdUsuarioIncluiu)
                .NotEmpty().WithMessage(Traducao.Resource.MsgDataAlteracaoObrigatoria);
        }

        protected void EquipamentoObrigatorio()
        {
            RuleFor(x => x.Equipamento)
                .NotEmpty().WithMessage(Traducao.Resource.MsgInstrumentoObrigatorio);
        }

        protected void NumeroObrigatorio()
        {
            RuleFor(x => x.Numero)
                .NotEmpty().WithMessage(Traducao.Resource.MsgNumeroObrigatorio);
        }

        protected void ModeloObrigatorio()
        {
            RuleFor(x => x.Modelo)
                .NotEmpty().WithMessage(Traducao.Resource.MsgModeloObrigatorio);
        }

        protected void ResponsavelObrigatorio()
        {
            RuleFor(x => x.IdResponsavel)
                .NotEmpty().WithMessage(Traducao.Resource.MsgResposavelObrigatorio);
        }

        protected void LocalDeUsoObrigatorio()
        {
            RuleFor(x => x.LocalDeUso)
                .NotEmpty().WithMessage(Traducao.Resource.MsgCampoLocalDeUso);
        }

        protected void EscalaObrigatorio()
        {
            RuleFor(x => x.Escala)
                .NotEmpty().WithMessage(Traducao.Resource.MsgCampoUso);
        }

        protected void MenorDivisaoObrigatorio()
        {
            RuleFor(x => x.MenorDivisao)
                .NotEmpty().WithMessage(Traducao.Resource.MsgCampoMenorDivisao);
        }

        protected void CriterioAceitacaoObrigatorio()
        {
            RuleFor(x => x.valorAceitacao)
                .NotEmpty().WithMessage(Traducao.Resource.MsgCampoCriterioAceitacao);

            RuleFor(x => x.valorAceitacao)
                .Matches("^[0-9]+([,.][0-9]+)?$").WithMessage(Traducao.Resource.MsgApenasNumeros)
                .When(y=>y.SistemaDefineStatus == true);
        }

    }
}
