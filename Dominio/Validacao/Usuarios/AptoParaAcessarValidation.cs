using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Usuarios;


namespace Dominio.Validacao.Usuarios
{
    public class AptoParaAcessarValidation : Validator<Usuario>
    {
        public AptoParaAcessarValidation()
        {
            var deveEstarAtivo = new DeveEstarAtivo();
            var deveEstarDesbloqueado = new DeveEstarDesbloqueado();
            var naoDeveEstarExpirado = new NaoDeveEstarExpirado();

            base.Add(Traducao.Resource.NaoAtivo, new Rule<Usuario>(deveEstarAtivo, Traducao.Login.ResourceLogin.Login_msg_UsuarioDeveEstarAtivo));
            base.Add(Traducao.Resource.Expirado, new Rule<Usuario>(naoDeveEstarExpirado, Traducao.Login.ResourceLogin.Login_msg_UsuarioExpirado));
            base.Add(Traducao.Resource.Bloquaeado, new Rule<Usuario>(deveEstarDesbloqueado, Traducao.Login.ResourceLogin.Login_msg_Bloqueado));

        }
    }
}
