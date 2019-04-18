using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface ICargoServico 
    {
        List<Cargo> ObtemCargosPorSiteEProcesso(int idSite, int idProcesso);
        List<Cargo> ObtemCargosPorSite(int idSite);
        void Valida(Cargo cargo, ref List<string> erros);
        bool Excluir(int id);
    }
}
