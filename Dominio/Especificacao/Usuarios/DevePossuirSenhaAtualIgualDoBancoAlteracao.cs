using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace Dominio.Especificacao.Usuarios
{
    public class DevePossuirSenhaAtualIgualDoBancoAlteracao : ISpecification<Usuario>
    {
        private readonly IUsuarioRepositorio _repositorio;

        public DevePossuirSenhaAtualIgualDoBancoAlteracao(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool IsSatisfiedBy(Usuario entity)
        {
            return _repositorio.Get(x => x.CdSenha == entity.CdSenha && x.IdUsuario == entity.IdUsuario).FirstOrDefault() != null;
        }
    }
}
