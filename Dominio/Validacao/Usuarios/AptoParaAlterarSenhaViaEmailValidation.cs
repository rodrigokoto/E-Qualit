using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Usuarios;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Usuarios
{
    class AptoParaAlterarSenhaViaEmailValidation : Validator<Usuario>
    {

        public AptoParaAlterarSenhaViaEmailValidation(IUsuarioRepositorio usuarioRepositorio)
        {
            var devePossuirCdSenhaIguais = new DevePossuirSenhaIgualDoBanco(usuarioRepositorio);

            base.Add(Traducao.Resource.SenhaNaoConfere, new Rule<Usuario>(devePossuirCdSenhaIguais, Traducao.Login.ResourceLogin.AlterarSenha_msg_Senha_error_equals));
        }

    }
}
