using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace Dominio.Especificacao.DocDocumentos
{
    public class DeveTerUsuarioUnicoAprovadorPorEtapa : ISpecification<DocDocumento>
    {
        private readonly IDocUsuarioVerificaAprovaRepositorio _docUsuarioVerificaAprovaRepositorio;
        
        public DeveTerUsuarioUnicoAprovadorPorEtapa(IDocUsuarioVerificaAprovaRepositorio docUsuarioVerificaAprovaRepositorio)
        {
            _docUsuarioVerificaAprovaRepositorio = docUsuarioVerificaAprovaRepositorio;
        }


        public bool IsSatisfiedBy(DocDocumento docDocumento)
        {
            var usuario = _docUsuarioVerificaAprovaRepositorio.VerificaDuplicidadeAprovador(docDocumento); 

            if (usuario.Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
