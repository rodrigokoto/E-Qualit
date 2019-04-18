using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace Dominio.Especificacao.Cargos
{
    public class DeveTerNomeUnicoPorSiteSpecification : ISpecification<Cargo>
    {
        private readonly ICargoRepositorio _cargoRepository;

        public DeveTerNomeUnicoPorSiteSpecification(ICargoRepositorio cargoRepository)
        {
            _cargoRepository = cargoRepository;
        }

        public bool IsSatisfiedBy(Cargo cargo)
        {
            var teste =_cargoRepository.Get(x => x.NmNome == cargo.NmNome && x.IdSite == cargo.IdSite);

            return !teste.Any();
        }
    }
}
