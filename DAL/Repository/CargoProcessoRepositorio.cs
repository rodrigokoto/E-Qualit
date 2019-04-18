using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class CargoProcessoRepositorio : BaseRepositorio<CargoProcesso>, ICargoProcessoRepositorio
    {
        
            public List<CargoProcesso> ListarCargoProcessoPorCargo(int idCargo)
        {
            return Db.CargoProcesso.Where(x => x.IdCargo == idCargo).ToList();
        }
    }
}
