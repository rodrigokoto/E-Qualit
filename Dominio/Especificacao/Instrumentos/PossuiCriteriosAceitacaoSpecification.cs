using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Servico;

namespace Dominio.Especificacao.Instrumentos
{
    public class PossuiCriteriosAceitacaoSpecification : ISpecification<Calibracao>
    {
        public bool IsSatisfiedBy(Calibracao entity)
        {
            if (entity.Instrumento.SistemaDefineStatus)
            {
                return UtilsServico.ListaEstaPreenchido(entity.CriterioAceitacao);
            }

            return false;
        }
    }
}
