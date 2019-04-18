using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IDocumentoAssuntoServico
    {
        void AlterarAssuntosDocumento(int idDocumento, List<DocumentoAssunto> lista);
    }
}
