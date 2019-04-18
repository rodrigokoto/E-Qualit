using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Usuarios;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Usuarios
{
    public class AptoParaLogarValidation : Validator<Usuario>
    {
        public AptoParaLogarValidation(IUsuarioRepositorio usuarioRepositorio)
        {
            //var devePossuirSenhasIguais = new DevePossuirSenhaIgualDoBanco(usuarioRepositorio);
            //var devePossuirCdIdentificacaoIguais = new DevePossuirCdIdentificacaoIgualDoBanco(usuarioRepositorio);
            var devePossuirCdIdentificacaoESenhaIguaisAoBanco = new DevePossuirCdIdentificacaoESenhaIguaisAoBanco(usuarioRepositorio);


            //base.Add("Senha não confere", new Rule<Usuario>(devePossuirSenhasIguais, "Verifique a senha."));
            //base.Add("E-mail não confere", new Rule<Usuario>(devePossuirCdIdentificacaoIguais, "Verifique o e-mail informado."));
            base.Add(Traducao.Resource.EmailNaoConfere, new Rule<Usuario>(devePossuirCdIdentificacaoESenhaIguaisAoBanco, Traducao.Login.ResourceLogin.Login_msg_Login_or_Senha_invalid));
        }
    }
}
