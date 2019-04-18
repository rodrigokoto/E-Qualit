using Dominio.Entidade;

namespace Dominio.Interface.Repositorio
{
    public interface IRegistroConformidadesRepositorio: IBaseRepositorio<RegistroConformidade>
    {
        RegistroConformidade GerarNumeroSequencialPorSite(RegistroConformidade registroConformidade);
        RegistroConformidade GetByIdAsNoTracking(int id);
    }
}
