using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IDocTemplateServico
    {
        void AlterarTemplatesDocumento(int idDocumento, List<DocTemplate> lista);
    }
}
