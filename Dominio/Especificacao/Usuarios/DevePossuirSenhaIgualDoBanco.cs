using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace Dominio.Especificacao.Usuarios
{
    public class DevePossuirSenhaIgualDoBanco : ISpecification<Usuario>
    {
        private readonly IUsuarioRepositorio _repositorio;

        public DevePossuirSenhaIgualDoBanco(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool IsSatisfiedBy(Usuario usuario)
        {
            return _repositorio.Get(x => x.CdSenha == usuario.SenhaAtual).FirstOrDefault() != null;
        }
    }
}
