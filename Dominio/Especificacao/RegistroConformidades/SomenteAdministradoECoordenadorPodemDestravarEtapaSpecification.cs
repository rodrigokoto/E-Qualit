using DomainValidation.Interfaces.Specification;
using Dominio.Enumerado;

namespace Dominio.Especificacao.RegistroConformidades
{
    public class SomenteAdministradoECoordenadorPodemDestravarEtapaSpecification : ISpecification<int>
    {
       
        public bool IsSatisfiedBy(int idPerfil)
        {
            if (idPerfil != (int)PerfisAcesso.Colaborador)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
