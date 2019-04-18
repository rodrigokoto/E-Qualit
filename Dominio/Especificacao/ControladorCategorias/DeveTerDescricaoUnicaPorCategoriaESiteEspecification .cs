using DomainValidation.Interfaces.Specification;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace Dominio.Especificacao.ControladorCategorias
{
    public class DeveTerDescricaoUnicaPorCategoriaESiteEspecification : ISpecification<Entidade.ControladorCategoria>
    {
        private readonly IControladorCategoriasRepositorio _repositorio;

        public DeveTerDescricaoUnicaPorCategoriaESiteEspecification(IControladorCategoriasRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool IsSatisfiedBy(Entidade.ControladorCategoria controladorCategorias)
        {
            //categoria.TipoTabela != string.Empty && categoria.Descricao != string.Empty && categoria.Ativo != false
            return (
                _repositorio
                .GetAll()
                .Where( 
                    x => x.Descricao == controladorCategorias.Descricao
                    && x.TipoTabela == controladorCategorias.TipoTabela
                    && x.IdControladorCategorias == controladorCategorias.IdControladorCategorias
                )) != null;
        }
    }
}
