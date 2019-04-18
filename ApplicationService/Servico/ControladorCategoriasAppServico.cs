using System.Collections.Generic;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using Dominio.Interface.Servico;

namespace ApplicationService.Servico
{
    public class ControladorCategoriasAppServico : BaseServico<ControladorCategoria>, IControladorCategoriasAppServico
    {
        private readonly IControladorCategoriasRepositorio _controladorCategoriasRepositorio;
        private readonly IControladorCategoriasServico _controladorCategoriasServico;

        public ControladorCategoriasAppServico(IControladorCategoriasRepositorio controladorCategoriasRepositorio,
            IControladorCategoriasServico controladorCategoriasServico
            ) : base(controladorCategoriasRepositorio)
        {
            _controladorCategoriasRepositorio = controladorCategoriasRepositorio;
            _controladorCategoriasServico = controladorCategoriasServico;
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

        public ControladorCategoria SalvarCadastro(ControladorCategoria controladorCategorias, ref List<string> erros)
        {
            ControladorCategoria categoria = null;

            if (_controladorCategoriasServico.ENovaCategoria(controladorCategorias, ref erros))
            {
                categoria = controladorCategorias;
                SalvaNovaCategoria(categoria);
            }
            else
            {
                categoria = _controladorCategoriasRepositorio.GetByIdAsNoTracking(controladorCategorias.IdControladorCategorias);
                if (categoria != null)
                {
                    categoria.Descricao = controladorCategorias.Descricao;
                    SalvaCategoriaEditada(categoria);
                }

            }

            return categoria;
        }

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
