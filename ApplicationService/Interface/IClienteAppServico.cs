
using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IClienteAppServico : IBaseServico<Cliente>
    {
        Cliente ObterPorUrl(string url);
        IEnumerable<Cliente> ObterClientesPorUsuario(int idUsuario);
        bool AtivarInativar(int id);
    }
}
