using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IAnaliseCriticaAppServico : IBaseServico<AnaliseCritica>
    {
        List<AnaliseCritica> ListaTodosAtivos();
        List<Usuario> ObterUsuariosPorAnaliseCritica(int idAnaliseCritica, int idSite);
        List<AnaliseCritica> ObterPorIdSite(int idSite);
        void SalvarAnaliseCritica(AnaliseCritica analiseCritica);
        void AtualizaAnaliseCritica(AnaliseCritica analiseCritica);
    }
}
