using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IControladorCategoriasServico 
    {
        List<ControladorCategoria> ListaGetByTipo(string tipo);
        List<ControladorCategoria> ListaGetByTipoAndSite(string tipo, int site);
        List<ControladorCategoria> ListaAtivos(string tipo, int site);

        ControladorCategoria GetByIdAsNoTracking(int id);

        bool ENovaCategoria(ControladorCategoria categoria, ref List<string> erros);
    }
}
