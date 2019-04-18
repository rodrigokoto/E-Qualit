using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface ICargoProcessoRepositorio : IBaseRepositorio<CargoProcesso>
    {
        List<CargoProcesso> ListarCargoProcessoPorCargo(int idCargo);
    }
}
