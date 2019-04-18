using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Usuarios;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Usuarios
{
    public class ExisteEssaIdentificacaoValidation : Validator<Usuario>
    {
        public ExisteEssaIdentificacaoValidation(IUsuarioRepositorio usuarioRepositorio)
        {
            var devePossuirCdIdentificacaoIguais = new DevePossuirCdIdentificacaoIgualDoBanco(usuarioRepositorio);

            base.Add(Traducao.Resource.EmailNaoConfere, new Rule<Usuario>(devePossuirCdIdentificacaoIguais, Traducao.Login.ResourceLogin.RecuperarSenha_msg_Email_not_found));
        }
    }
}
