using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IDocumentoAssuntoAppServico : IBaseServico<DocumentoAssunto>
    {
        void AlterarAssuntosDocumento(int idDocumento, List<DocumentoAssunto> lista);
    }
}
