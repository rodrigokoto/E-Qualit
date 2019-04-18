using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;

namespace Dominio.Especificacao.DocDocumentos
{
    public class DeveTerUmaUnicaCopiaComStatusDiferenteDeRevisar : ISpecification<DocDocumento>
    {
        private readonly IDocDocumentoRepositorio _repositorio;

        public DeveTerUmaUnicaCopiaComStatusDiferenteDeRevisar(IDocDocumentoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool IsSatisfiedBy(DocDocumento docDocumento)
        {
            var documentos = _repositorio.Get(x => x.IdDocumentoPai == docDocumento.IdDocumento);

            foreach (var documento in documentos)
            {
                if (documento.FlStatus != (int)StatusDocumento.Aprovado)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
