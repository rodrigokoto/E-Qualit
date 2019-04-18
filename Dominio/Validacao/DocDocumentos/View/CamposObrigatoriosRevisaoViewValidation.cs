using Dominio.Entidade;

namespace Dominio.Validacao.DocDocumentos.View
{
    public class CamposObrigatoriosRevisaoViewValidation: ValidaCamposDocDocumento<DocDocumento>
    {
        public CamposObrigatoriosRevisaoViewValidation()
        {
            ValidaRevisao();
        }
    }
}
