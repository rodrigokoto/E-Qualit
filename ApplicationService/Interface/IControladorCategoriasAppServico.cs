using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IControladorCategoriasAppServico : IBaseServico<ControladorCategoria>
    {
        List<ControladorCategoria> ListaGetByTipo(string tipo);
        List<ControladorCategoria> ListaGetByTipoAndSite(string tipo, int site);
        List<ControladorCategoria> ListaAtivos(string tipo, int site);

        ControladorCategoria SalvarCadastro(ControladorCategoria controladorCategorias, ref List<string> erros);
        ControladorCategoria GetByIdAsNoTracking(int id);

    }
}
