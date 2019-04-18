using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IDocCargoAppServico : IBaseServico<DocumentoCargo>
    {
        void AlterarCargosDoDocumento(int idDocumento, List<DocumentoCargo> lista);
    }
}
