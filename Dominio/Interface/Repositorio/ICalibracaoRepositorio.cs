using Dominio.Entidade;

namespace Dominio.Interface.Repositorio
{
    public interface ICalibracaoRepositorio : IBaseRepositorio<Calibracao>
    {
        void RemoverComDelecaoDosRelacionamentos(int id);
        void CriarCalibracao(Calibracao calibracao);
        void Atualizar(Calibracao calibracao);
    }
}
