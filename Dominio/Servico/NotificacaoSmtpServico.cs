using Dominio.Entidade;
using Dominio.Interface.Servico;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace Dominio.Servico
{
    public class NotificacaoSmtpServico : INotificacaoSmtpServico
    {
        private readonly INotificacaoSmtpRepositorio _notificacaoSmtpRepositorio;
        public NotificacaoSmtpServico(INotificacaoSmtpRepositorio notificacaoSmtpRepositorio) 
        {
            _notificacaoSmtpRepositorio = notificacaoSmtpRepositorio;
        }

        public NotificacaoSmtp ObterSmtpAtivo()
        {
            return _notificacaoSmtpRepositorio.Get(x => x.FlAtivo == true).FirstOrDefault();
        }
    }
}
