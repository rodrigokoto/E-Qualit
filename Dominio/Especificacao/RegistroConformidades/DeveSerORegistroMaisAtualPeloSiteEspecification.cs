using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace Dominio.Especificacao.RegistroConformidades
{
    public class DeveSerORegistroMaisAtualPeloSiteEspecification : ISpecification<RegistroConformidade>
    {
        private readonly IRegistroConformidadesRepositorio _registroConformidadesRepositorio;

        public DeveSerORegistroMaisAtualPeloSiteEspecification(IRegistroConformidadesRepositorio registroConformidadesRepositorio)
        {
            _registroConformidadesRepositorio = registroConformidadesRepositorio;
        }

        public bool IsSatisfiedBy(RegistroConformidade registroConformidade)
        {
            var maiorNuRegistro = _registroConformidadesRepositorio
                                .Get(x => x.IdSite == registroConformidade.IdSite &&
                                     x.TipoRegistro == registroConformidade.TipoRegistro)
                                .Max(x => x.NuRegistro);

            if (registroConformidade.NuRegistro >= maiorNuRegistro)
            {
                return true;
            }
            return false;
        }
    }
}