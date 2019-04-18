using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface INotificacaoServico
    {
        IEnumerable<Notificacao> ObterNotificacoesUsuario(int idUsuario, int idPerfil, int idSite);

        void RemovePorFuncionalidade(int idFuncionalidade, int idRegistroDaBase);
        void Remove(int idFuncionalidade, int idRegistroDaBase, int IdUsuarioNotificado);

        void Valido(Notificacao notificacao, ref List<string> erros);
    }
}
