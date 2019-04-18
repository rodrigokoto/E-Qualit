using Dominio.Entidade;
using Dominio.Enumerado;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface INotificacaoAppServico : IBaseServico<Notificacao>
    {
        IEnumerable<Notificacao> ObterNotificacoesUsuario(int idUsuario, int idPerfil, int idSite);

        void RemovePorFuncionalidade(Funcionalidades funcionalidade, int idRegistroDaBase);
        void Remove(int idFuncionalidade, int idRegistroDaBase, int IdUsuarioNotificado);
        //void Add(object notificacao);
    }
}
