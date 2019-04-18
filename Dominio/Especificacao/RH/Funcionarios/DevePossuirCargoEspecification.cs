using DomainValidation.Interfaces.Specification;
using Dominio.Entidade.RH;

namespace Dominio.Especificacao.RH.Funcionarios
{
    public class DevePossuirCargoEspecification : ISpecification<Funcionario>
    {
        public bool IsSatisfiedBy(Funcionario funcionario)
        {
            if (PossuiCargo(funcionario.Cargo))
            {
                return true;
            }
            return false;
        }

        private bool PossuiCargo(CargoRH cargos)
        {
            if (cargos == null)
            {
                return false;
            }

            return true;
        }
    }
}
