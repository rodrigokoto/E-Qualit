using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Especificacao.Usuarios
{
    class NaoDeveEstarExpirado : ISpecification<Usuario>
    {
        public bool IsSatisfiedBy(Usuario usuario)
        {
            if (usuario.DtExpiracao == null || usuario.DtExpiracao.Value >= DateTime.Now)
            {
                return true;
            }
            return false;
        }
    }
}
