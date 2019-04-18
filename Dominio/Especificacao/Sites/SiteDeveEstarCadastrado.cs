using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace Dominio.Especificacao.Sites
{
    public class SiteDeveEstarCadastrado : ISpecification<Site>
    {
        private readonly ISiteRepositorio _repositorio;

        public SiteDeveEstarCadastrado(ISiteRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool IsSatisfiedBy(Site site)
        {
            return _repositorio.GetById(site.IdSite) !=  null;
        }
    }
}
