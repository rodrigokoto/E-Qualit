using Dominio.Entidade;

namespace ApplicationService.Interface
{
    public interface ICalibracaoAppServico : IBaseServico<Calibracao>
    {
        void AlterarEstadoParaDeletado(Calibracao obj);
        bool RemoverComRelacionamentos(int id);
        void SalvarComCriteriosAceitacao(Calibracao calibracao);
        void SalvarCalibracao(Calibracao calibracao);
        void AtualizarCalibracao(Calibracao calibracao);
    }
}
