using DomainValidation.Interfaces.Specification;
using Dominio.Entidade.RH;

namespace Dominio.Especificacao.RH.Funcionarios
{
    public class SeInformadoCNHDeveTerVencimentoCNH : ISpecification<Funcionario>
    {
        public bool IsSatisfiedBy(Funcionario funcionario)
        {
            if (!PossuiCNH(funcionario.CNH))
            {
                return true;
            }
            else
            {
                if (funcionario.VencimentoCNH == null)
                {
                    return false;
                }
            }
            return true;
        }


        private bool PossuiCNH(string cnh)
        {
            if (cnh == null)
            {
                return false;
            }
            return true;
        }
    }
}
