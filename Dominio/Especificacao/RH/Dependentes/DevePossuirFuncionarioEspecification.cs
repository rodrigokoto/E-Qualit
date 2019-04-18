using DomainValidation.Interfaces.Specification;
using Dominio.Entidade.RH;

namespace Dominio.Especificacao.RH.Dependentes
{
    public class DevePossuirFuncionarioEspecification : ISpecification<Dependente>
    {
        public bool IsSatisfiedBy(Dependente dependente)
        {
            if (dependente.CodigoFuncionario == 0)
            {
                return false;
            }
            return true;
        }
    }
}
