using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IDocumentoComentarioServico
    {
        void AlterarCOmentariosDocumento(int idDocumento, List<DocumentoComentario> lista);
    }
}
