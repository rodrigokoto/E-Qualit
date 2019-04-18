using Dominio.Entidade;
using ApplicationService.Interface;
using System.Collections.Generic;
using System.Linq;
using Dominio.Interface.Repositorio;
using Dominio.Enumerado;
using System;

namespace ApplicationService.Servico
{
    public class NotificacaoAppServico : BaseServico<Notificacao>, INotificacaoAppServico
    {
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
  
        public NotificacaoAppServico(INotificacaoRepositorio notificacaoRepositorio,
                                  IUsuarioRepositorio usuario) : base(notificacaoRepositorio)
        {
            _notificacaoRepositorio = notificacaoRepositorio;
            _usuarioRepositorio = usuario;
        }

        public IEnumerable<Notificacao> ObterNotificacoesUsuario(int idUsuario, int idPerfil, int idSite)
        {

            IEnumerable<Notificacao> _notificoesUsuario = null;

            if (idPerfil == (int)PerfisAcesso.Colaborador)
            {
                //O colaborador vê somente as suas próprias notificações
                _notificoesUsuario = _notificacaoRepositorio.Get(
                                                                 w => w.IdUsuario == idUsuario && w.IdSite == idSite,
                                                                 ob => ob.OrderBy(o => o.IdSite).ThenBy(tb => tb.IdSite).ThenBy(tb1 => tb1.Processo.Nome),
                                                                 "Funcionalidade,Processo"
                                                                 );
            }
            else
            {
                //Os demais perfis vê as notificações de todos os usuários
                _notificoesUsuario = _notificacaoRepositorio.Get(
                                                                 w => w.IdSite == idSite,
                                                                 ob => ob.OrderBy(o => o.IdSite).ThenBy(tb => tb.Usuario.NmCompleto).ThenBy(tb => tb.IdSite).ThenBy(tb1 => tb1.Processo.Nome),
                                                                 "Funcionalidade,Processo"
                                                                 );
            }
            return _notificoesUsuario;

        }

        public void RemovePorFuncionalidade(Funcionalidades funcionalidade, int idRegistroDaBase)
        {
            var notificacoes = _notificacaoRepositorio.Get(
                                        x => x.IdFuncionalidade == (int)funcionalidade &&
                                        x.IdRelacionado == idRegistroDaBase).ToList();

            notificacoes.ForEach(notificacao =>
            {
                _notificacaoRepositorio.Remove(notificacao);
            });

        }

        public void Remove(int idFuncionalidade, int idRegistroDaBase, int idUsuarioNotificado)
        {
            throw new NotImplementedException();
        }
    }
}
