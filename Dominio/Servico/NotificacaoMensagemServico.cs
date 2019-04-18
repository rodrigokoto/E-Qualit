using Dominio.Entidade;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.Interface.Repositorio;
using Dominio.Enumerado;

namespace Dominio.Servico
{
    public class NotificacaoMensagemServico : INotificacaoMensagemServico
    {
        private readonly INotificacaoMensagemRepositorio _notificacaoMensagemRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IRegistroConformidadesRepositorio _registroConformidadeRepositorio;

        public NotificacaoMensagemServico(INotificacaoMensagemRepositorio notificacaoMensagemRepositorio,
                                          INotificacaoRepositorio notificacaoServico,
                                          IRegistroConformidadesRepositorio registroConformidadeRepositorio) 
        {
            _notificacaoMensagemRepositorio = notificacaoMensagemRepositorio;
            _notificacaoRepositorio = notificacaoServico;
            _registroConformidadeRepositorio = registroConformidadeRepositorio;
        }

        public IEnumerable<NotificacaoMensagem> ObterMensagensNaoEnviadas(int totalRegistros)
        {
            IEnumerable<NotificacaoMensagem> _notificoesUsuario = _notificacaoMensagemRepositorio.GetAll().Where(w => w.FlEnviada == false).OrderBy(ob => ob.DtEnvio).Take(totalRegistros).ToList();

            return _notificoesUsuario;
        }

        public void GeraFilaEmail()
        {
            string _tpNotificacao = ((char)TipoNotificacao.NotificacaoPorEmail).ToString();
            IEnumerable<Notificacao> _notificoesUsuario = _notificacaoRepositorio.Get(w => w.TpNotificacao == _tpNotificacao && w.DtEnvioFilaDisparo == null, null, "Usuario, Site, Site.Cliente")
                                                                                 .ToList();

            foreach (Notificacao _notificacao in _notificoesUsuario)
            {
                string _assunto = Assunto(_notificacao.Funcionalidade.Nome);
                string _mensagem = Mensagem(_notificacao.Funcionalidade.Nome);

                //Se não tem uma data de vencimento o envio é ignorado
                if (!_notificacao.DtVencimento.HasValue)
                {
                    continue;
                }
                //O usuário não está autorizado a receber email
                if (!_notificacao.Usuario.FlRecebeEmail)
                {
                    continue;
                }
                if (_notificacao.DtVencimento.Value.AddDays(_notificacao.NuDiasAntecedencia * -1) <= DateTime.Now)
                {
                
                    var _registro = new NotificacaoMensagem()
                    {
                        DsAssunto = _assunto,
                        DsMensagem = _mensagem,
                        DtCadastro = DateTime.Now,
                        DtEnvio = _notificacao.DtVencimento,
                        FlEnviada = false,
                        IdSite = _notificacao.IdSite,
                        NmEmailNome = _notificacao.Usuario.NmCompleto,
                        NmEmailPara = _notificacao.Usuario.CdIdentificacao,
                    };

                    //Inclui a mensagem na fila
                    _notificacaoMensagemRepositorio.Add(_registro);

                    //Atualiza a data que a notificação foi incluida na fila de disparo de email
                    _notificacao.DtEnvioFilaDisparo = DateTime.Now;
                    _notificacaoRepositorio.Update(_notificacao);
                }
            }
        }

        private string Assunto(string modulo)
        {
            return "Assunto -" + modulo;
        }
        private string Mensagem(string modulo)
        {
            return "Mensagem - " + modulo;
        }









        public void Dispose()
        {
            _notificacaoMensagemRepositorio.Dispose();
        }
        public void Update(NotificacaoMensagem notificacaoMensagem)
        {
            _notificacaoMensagemRepositorio.Update(notificacaoMensagem);
        }

    }
}
