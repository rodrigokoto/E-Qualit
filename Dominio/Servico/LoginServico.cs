using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.Usuarios;
using System.Collections.Generic;
using Dominio.Validacao.Usuarios.LoginView;

namespace Dominio.Servico
{
    public class LoginServico : ILoginServico
    {
        private readonly IUsuarioRepositorio _loginRepositorio;

        public LoginServico(IUsuarioRepositorio loginRepositorio)
        {
            _loginRepositorio = loginRepositorio;
        }

        public void ValidoParaLogar(Usuario usuario, ref List<string> erros)
        {
            usuario.CdSenha = UtilsServico.Sha1Hash(usuario.CdSenha);

            usuario.ValidationResult = new AptoParaLogarValidation(_loginRepositorio).Validate(usuario);

            if (!usuario.ValidationResult.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(usuario.ValidationResult));
            }
        }

        public void ValidoParaAcessar(Usuario usuario, ref List<string> erros)
        {
            usuario.ValidationResult = new AptoParaAcessarValidation().Validate(usuario);

            if (!usuario.ValidationResult.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(usuario.ValidationResult));
            }
        }

        public void ValidoParaEsqueciSenha(Usuario usuario, ref List<string> erros)
        {
            var validaEmail = new EsqueciaSenhaViewValidation().Validate(usuario);

            if (!validaEmail.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaEmail.Errors));

            }
            else
            {
                usuario.ValidationResult = new ExisteEssaIdentificacaoValidation(_loginRepositorio).Validate(usuario);

                if (!usuario.ValidationResult.IsValid)
                {
                    erros.AddRange(UtilsServico.PopularErros(usuario.ValidationResult));
                }
                else
                {
                    usuario.ValidationResult = new UsuarioEComplartilhadoValidation(_loginRepositorio).Validate(usuario);

                    if (!usuario.ValidationResult.IsValid)
                    {
                        erros.AddRange(UtilsServico.PopularErros(usuario.ValidationResult));
                    }
                }
            }

        }

        public void ValidoParaAlterarSenhaViaEmail(Usuario usuario, ref List<string> erros)
        {
            var validaSenhaNovaEConfirma = new AlterarSenhaViewValidation().Validate(usuario);

            if (!validaSenhaNovaEConfirma.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaSenhaNovaEConfirma.Errors));
            }
            else
            {
                usuario.SenhaAtual = UtilsServico.Sha1Hash(usuario.SenhaAtual);
                usuario.ValidationResult = new AptoParaAlterarSenhaViaEmailValidation(_loginRepositorio).Validate(usuario);

                if (!usuario.ValidationResult.IsValid)
                {
                    erros.AddRange(UtilsServico.PopularErros(usuario.ValidationResult));
                }
            }

        }

    }
}
