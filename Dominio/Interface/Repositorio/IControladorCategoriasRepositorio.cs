using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface IControladorCategoriasRepositorio : IBaseRepositorio<ControladorCategoria>
    {
        List<ControladorCategoria> ListaGetByTipo(string tipo);
        List<ControladorCategoria> ListaGetByTipoAndSite(string tipo, int site);
        List<ControladorCategoria> ListaAtivos(string tipo, int site);
        ControladorCategoria GetByIdAsNoTracking(int id);
    }
}
