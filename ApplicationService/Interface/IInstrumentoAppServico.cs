using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IInstrumentoAppServico : IBaseServico<Instrumento>
    {
        bool DeletarInstrumentoEDependencias(int id);
        List<Instrumento> ObterPorIdSite(int idSite);

    }
}
