using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface ISiteServico 
    {
        IEnumerable<Site> ObterSitesPorCliente(int idCliente);
        bool AtivarInativar(int id);
        void Valida(Site site, ref List<string> erros);

        bool Excluir(int id);
    }
}
