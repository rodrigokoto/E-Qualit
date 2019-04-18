using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace Dominio.Especificacao.Processos
{
    public class DevePossuirNomeUnicoPorSite : ISpecification<Processo>
    {
        private readonly IProcessoRepositorio _repositorio;

        public DevePossuirNomeUnicoPorSite(IProcessoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool IsSatisfiedBy(Processo processo)
        {
            return  _repositorio.Get(x => x.Nome == processo.Nome && x.IdSite == processo.IdSite).FirstOrDefault() == null;
        }
    }
}
