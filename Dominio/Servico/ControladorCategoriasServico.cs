using System.Collections.Generic;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.ControladorCategorias;

namespace Dominio.Servico
{
    public class ControladorCategoriasServico : IControladorCategoriasServico
    {
        public readonly IControladorCategoriasRepositorio _controladorCategoriasRepositorio;

        private CamposObrigatoriosValidation _validaCampoObrigatorios;

        public ControladorCategoriasServico(IControladorCategoriasRepositorio controladorCategoriasRepositorio) 
        {
            _controladorCategoriasRepositorio = controladorCategoriasRepositorio;
            _validaCampoObrigatorios = new CamposObrigatoriosValidation();
        }

        public List<ControladorCategoria> ListaGetByTipo(string tipo)
        {
            return _controladorCategoriasRepositorio.ListaGetByTipo(tipo);
        }

        public List<ControladorCategoria> ListaGetByTipoAndSite(string tipo, int site)
        {
            return _controladorCategoriasRepositorio.ListaGetByTipoAndSite(tipo, site);
        }

        public List<ControladorCategoria> ListaAtivos(string tipo, int site)
        {
            return _controladorCategoriasRepositorio.ListaAtivos(tipo, site);
        }


        public bool ENovaCategoria(ControladorCategoria categoria, ref List<string> erros)
        {

            if (categoria.IdControladorCategorias > 0)
            {
                return false;
            }

            categoria.ValidationResult = new ENovaCategoriaValidation(_controladorCategoriasRepositorio)
                                                           .Validate(categoria);
            if (!categoria.ValidationResult.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(categoria.ValidationResult));

                return false;
            }
            return true;
        }

        //private bool 
        private void SalvaNovaCategoria(ControladorCategoria cadastro)
        {
            _controladorCategoriasRepositorio.Add(cadastro);
        }

        private void SalvaCategoriaEditada(ControladorCategoria cadastro)
        {
            _controladorCategoriasRepositorio.Update(cadastro);
        }

        public ControladorCategoria GetByIdAsNoTracking(int id)
        {
            var result = _controladorCategoriasRepositorio.GetByIdAsNoTracking(id);
             result.Site = null;

            return result;
        }
              
    }
}
