using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IDocTemplateAppServico : IBaseServico<DocTemplate>
    {
        void AlterarTemplatesDocumento(int idDocumento, List<DocTemplate> lista);
    }
}
