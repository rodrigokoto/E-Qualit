using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace Dominio.Especificacao.DocDocumentos
{
    public class DeveTerUsuarioUnicoVerificadorPorEtapa: ISpecification<DocDocumento>
    {
        private readonly IDocUsuarioVerificaAprovaRepositorio _docUsuarioVerificaAprovaRepositorio;


        public DeveTerUsuarioUnicoVerificadorPorEtapa(IDocUsuarioVerificaAprovaRepositorio docUsuarioVerificaAprovaRepositorio)
        {
            _docUsuarioVerificaAprovaRepositorio = docUsuarioVerificaAprovaRepositorio;
        }


        public bool IsSatisfiedBy(DocDocumento docDocumento)
        {
            var usuario = _docUsuarioVerificaAprovaRepositorio.VerificaDuplicidadeVerificador(docDocumento);

            if (usuario == null)
            {
                return true;
            }
            return false;
        }
    }
}
