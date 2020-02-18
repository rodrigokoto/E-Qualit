using Dominio.Entidade;
using FluentValidation;
using System;

namespace Dominio.Validacao.Instrumentos.View
{
    public abstract class ValidaCampoLicenca<T> : AbstractValidator<T> where T : Licenca
    {

        protected void TituloObrigatorio()
        {
            RuleFor(x => x.Titulo)
                    .NotEmpty().WithMessage(Traducao.Resource.ObsObrigatoria);
        }

        protected void ValidarDataCriacao()
        {
            RuleFor(x => x.DataCriacao).Must(x => ValidarData(x.ToString())).WithMessage("Data de criação inválida");
        }

        protected void ValidarDataEmissao()
        {
            RuleFor(x => x.DataEmissao).Must(x => ValidarData(x.ToString())).WithMessage("Data de Emissão inválida");
        }
        protected void ValidarDataProximaNotificacao()
        {
            RuleFor(x => x.DataProximaNotificacao).Must(x => ValidarData(x.ToString())).WithMessage("Data de Notificação inválida");
        }

        protected void ValidarDataVencimento()
        {
            RuleFor(x => x.DataVencimento).Must(x => ValidarData(x.ToString())).WithMessage("Data de Vencimento inválida");
        }

        protected bool ValidarData(string data)
        {
            DateTime dataresult = new DateTime();
            
            
            return DateTime.TryParse(data, out dataresult);
        }

    }
}
