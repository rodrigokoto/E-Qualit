using DomainValidation.Interfaces.Specification;
using Dominio.Entidade.RH;
using System.Linq;

namespace Dominio.Especificacao.RH.Funcionarios
{
    public class DevePossuirValeTransporteInformadoEspecification : ISpecification<Funcionario>
    {
        public bool IsSatisfiedBy(Funcionario funcionario)
        {
            if (ValeTransporteVazio(funcionario))
            {
                return false;
            }
            else if (!InformouValeTransporte(funcionario))
            {
                return false;
            }

            return true;
        }

        private bool ValeTransporteVazio(Funcionario funcionario)
        {
            if (funcionario.ValeTransportes.Count == 0)
            {
                return true;
            }

            return false;
        }

        private bool InformouValeTransporte(Funcionario funcionario)
        {
            if (funcionario.ValeTransportes.FirstOrDefault().Possui)
            {
                if (funcionario.ValeTransportes.FirstOrDefault().DataVigencia == null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
