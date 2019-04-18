using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface ICalibracaoServico 
    {
        void AlterarEstadoParaDeletado(Calibracao obj);
        void Valido(Calibracao calibracao, ref List<string> erros);
        bool RemoverComRelacionamentos(int id);
        void SalvarComCriteriosAceitacao(Calibracao calibracao);
        void SalvarCalibracao(Calibracao calibracao);
        void AtualizarCalibracao(Calibracao calibracao);
    }
}
