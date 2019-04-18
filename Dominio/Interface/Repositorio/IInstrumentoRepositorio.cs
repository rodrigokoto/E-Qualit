using Dominio.Entidade;

namespace Dominio.Interface.Repositorio
{
    public interface IInstrumentoRepositorio : IBaseRepositorio<Instrumento>
    {
        void RemoverComDelecaoDosRelacionamentos(int id);
    }
}
