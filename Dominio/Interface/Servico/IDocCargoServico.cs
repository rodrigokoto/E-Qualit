using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IDocCargoServico 
    {
        void AlterarCargosDoDocumento(int idDocumento, List<DocumentoCargo> lista);
    }
}
