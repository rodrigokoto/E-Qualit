using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace Dominio.Especificacao.Usuarios
{
    public class DevePossuirCdIdentificacaoESenhaIguaisAoBanco : ISpecification<Usuario>
    {
        private readonly IUsuarioRepositorio _repositorio;

        public DevePossuirCdIdentificacaoESenhaIguaisAoBanco(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool IsSatisfiedBy(Usuario usuario)
        {
            return _repositorio.Get(x => x.CdIdentificacao == usuario.CdIdentificacao && x.CdSenha == usuario.CdSenha).FirstOrDefault() != null;
        }
    }
}
