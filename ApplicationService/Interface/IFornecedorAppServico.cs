using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IFornecedorAppServico : IBaseServico<Fornecedor>
    {
        List<Fornecedor> ObterPorSite(int idSite);
    }
}
