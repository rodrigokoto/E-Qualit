using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Usuarios;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Usuarios
{
    public class AptoParaEditarValidation : Validator<Usuario>
    {
        public AptoParaEditarValidation(IUsuarioRepositorio usuarioRepositorio)
        {
            var deveExistirNoBanco = new DeveExistirNoBanco(usuarioRepositorio);

            base.Add(Traducao.Resource.UsuarioNaoExiste, new Rule<Usuario>(deveExistirNoBanco, Traducao.Usuario.ResourceUsuario.Usuario_msg_not_found_IdUsuario));

        }
    }
}
