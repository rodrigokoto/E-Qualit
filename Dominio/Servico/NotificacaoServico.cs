using Dominio.Entidade;
using Dominio.Interface.Servico;
using System.Collections.Generic;
using System.Linq;
using Dominio.Interface.Repositorio;
using Dominio.Enumerado;
using System;

namespace Dominio.Servico
{
    public class NotificacaoServico : INotificacaoServico
    {
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
  
        public NotificacaoServico(INotificacaoRepositorio notificacaoRepositorio,
                                  IUsuarioRepositorio usuario) 
        {
            _notificacaoRepositorio = notificacaoRepositorio;
            _usuarioRepositorio = usuario;
        }

        public NotificacaoServico()
        {
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

        public void RemovePorFuncionalidade(int idFuncionalidade, int idRegistroDaBase)
        {
            var notificacoes = _notificacaoRepositorio.Get(
                                        x => x.IdFuncionalidade == idFuncionalidade &&
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

        public void Valido(Notificacao notificacao, ref List<string> erros)
        {
            var validacao = new Validacao.Notificacoes.AptoParaCadastroNotificacaoValidation().Validate(notificacao);

            if (!validacao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacao.Errors));
            }
        }

    }
}
