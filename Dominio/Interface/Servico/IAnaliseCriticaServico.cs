using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IAnaliseCriticaServico 
    {
        void Valido(AnaliseCritica analiseCritica, ref List<string> erros);
        List<AnaliseCritica> ListaTodosAtivos();
        List<Usuario> ObterUsuariosPorAnaliseCritica(int idAnaliseCritica, int idSite);
        void Remove(List<AnaliseCritica> analiseCriticas);
        List<AnaliseCritica> ObterPorIdSite(int idSite);
    }
}
