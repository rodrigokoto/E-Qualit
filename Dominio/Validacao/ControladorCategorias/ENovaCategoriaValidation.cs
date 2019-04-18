using DomainValidation.Validation;
using Dominio.Especificacao.ControladorCategorias;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.ControladorCategorias
{
    public class ENovaCategoriaValidation: Validator<Entidade.ControladorCategoria>
    {
        private readonly IControladorCategoriasRepositorio _controladorCategoriasRepositorio;
        public ENovaCategoriaValidation(IControladorCategoriasRepositorio controladorCategoriasRepositorio)
        {
            _controladorCategoriasRepositorio = controladorCategoriasRepositorio;
        }
              
        public ENovaCategoriaValidation()
        {
            var deveTerDescricaoUnica = new DeveTerDescricaoUnicaPorCategoriaESiteEspecification(_controladorCategoriasRepositorio);

            base.Add(Traducao.Resource.MsgErroCadastro, new Rule<Entidade.ControladorCategoria>(deveTerDescricaoUnica, Traducao.Resource.MsgDoErro));
        }
    }
}
