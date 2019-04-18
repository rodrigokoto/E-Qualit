using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace Dominio.Especificacao.Instrumentos
{
    public class DevePossuirNumeroUnicoPorModeloSpecification : ISpecification<Instrumento>
    {
        private readonly IInstrumentoRepositorio _repositorio;

        public DevePossuirNumeroUnicoPorModeloSpecification(IInstrumentoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool IsSatisfiedBy(Instrumento instrumento)
        {
            if (instrumento.IdInstrumento == 0)
            {
                return _repositorio.Get(x => x.Numero == instrumento.Numero &&                                        
                                        x.IdSite == instrumento.IdSite).Count() == 0;
            }
            else
            {
                return _repositorio.Get(x => x.Numero == instrumento.Numero &&
                                        x.IdSite == instrumento.IdSite &&
                                        x.IdInstrumento == instrumento.IdInstrumento).Count() <= 1;
            }
        }
    }
}
