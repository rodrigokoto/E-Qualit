using Dominio.Entidade;
using ApplicationService.Interface;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace ApplicationService.Servico
{
    public class NotificacaoSmtpAppServico : BaseServico<NotificacaoSmtp>, INotificacaoSmtpAppServico
    {
        private readonly INotificacaoSmtpRepositorio _notificacaoSmtpRepositorio;
        public NotificacaoSmtpAppServico(INotificacaoSmtpRepositorio notificacaoSmtpRepositorio) : base(notificacaoSmtpRepositorio)
        {
            _notificacaoSmtpRepositorio = notificacaoSmtpRepositorio;
        }

        public NotificacaoSmtp ObterSmtpAtivo()
        {
            return _notificacaoSmtpRepositorio.Get(x => x.FlAtivo == true).FirstOrDefault();
        }
    }
}
