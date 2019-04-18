using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IDocumentoComentarioAppServico : IBaseServico<DocumentoComentario>
    {
        void AlterarCOmentariosDocumento(int idDocumento, List<DocumentoComentario> lista);
    }
}
