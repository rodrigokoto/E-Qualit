using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.Calibracoes
{
    public class CamposObrigatoriosCalibracao : AbstractValidator<Calibracao>
    {
        public CamposObrigatoriosCalibracao()
        {
            RuleFor(x => x.DataRegistro)
                .NotNull().WithMessage(Traducao.Instrumentos.ResourceInstrumentos.IN_msg_required_data_registro);

            RuleFor(x => x.DataProximaCalibracao)
                .NotNull().WithMessage(Traducao.Instrumentos.ResourceInstrumentos.IN_msg_required_proxima_calibracao)
                .GreaterThan(x=> x.DataRegistro).WithMessage(Traducao.Instrumentos.ResourceInstrumentos.ADataProximaCalibracaoDeveSerMaiorQueADataDoRegistro);

            //RuleFor(x => x.Certificado)
            //    .NotNull().WithMessage(Traducao.Instrumentos.ResourceInstrumentos.IN_msg_required_certificado)
            //    .NotEmpty().WithMessage(Traducao.Instrumentos.ResourceInstrumentos.IN_msg_required_certificado);

            //RuleFor(x => x.OrgaoCalibrador)
            //    .NotNull().WithMessage(Traducao.Instrumentos.ResourceInstrumentos.IN_msg_required_orgao_calibrador);

            RuleFor(x => x.DataNotificacao)
                .NotNull().WithMessage(Traducao.Instrumentos.ResourceInstrumentos.IN_msg_required_data_notificacao)
                .GreaterThan(x=> x.DataRegistro).WithMessage(Traducao.Instrumentos.ResourceInstrumentos.ADataNotificacaoCalibracaoDeveSerMaiorQueADataDoRegistro)
                .LessThan(x => x.DataProximaCalibracao).WithMessage(Traducao.Instrumentos.ResourceInstrumentos.ADataNotificacaoCalibracaoDeveSerMenorQueADataDaProximaCalibracao);
            
            RuleFor(x => x.NomeUsuarioAprovador)
                .NotNull().WithMessage(Traducao.Instrumentos.ResourceInstrumentos.IN_msg_required_data_aprovador);
        }
    }
}
