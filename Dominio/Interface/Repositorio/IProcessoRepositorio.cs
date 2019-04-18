using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface IProcessoRepositorio : IBaseRepositorio<Processo>
    {
        List<Processo> ListaProcessosPorSite(int site);
    }
}
