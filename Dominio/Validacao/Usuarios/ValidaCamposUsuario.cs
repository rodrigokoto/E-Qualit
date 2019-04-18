using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Validacao.Usuarios.View;
using FluentValidation;
using System.Linq;

namespace Dominio.Validacao.Usuarios
{
    public abstract class ValidaCamposUsuario<T> : AbstractValidator<T> where T : Usuario
    {
        protected void SenhaDeveSerIgualAConfirmaSenha()
        {
            RuleFor(x => x.CdSenha)
                .MinimumLength(6).WithMessage(Traducao.Login.ResourceLogin.AlterarSenha_msg_required_Senha_confirma_minlength)
                .Equal(x => x.ConfirmaSenha).WithMessage(Traducao.Login.ResourceLogin.AlterarSenha_msg_required_Senha_confirma_equals);
        }

        protected void IdUsuarioNull()
        {
            RuleFor(x => x.IdUsuario)
                .Must(x => x > 0).WithMessage(Traducao.Login.ResourceLogin.AlterarSenha_msg_Usuario_invalid);
        }

        protected void DeveSerEmailValido()
        {
            RuleFor(x => x.CdIdentificacao)
                .NotEmpty().WithMessage(Traducao.Login.ResourceLogin.Login_msg_required_Email)
                .EmailAddress().WithMessage(Traducao.Login.ResourceLogin.Login_msg_required_email_error_format);
        }

        protected void DeveTerNomeValido()
        {
            RuleFor(x => x.NmCompleto)
                .NotEmpty().WithMessage(Traducao.Usuario.ResourceUsuario.Usuario_msg_required_Responsavel)
                .MaximumLength(60).WithMessage(Traducao.Usuario.ResourceUsuario.Usuario_msg_required_Responsavel_maxlength);
        }

        protected void DeveTerCPFValido()
        {
            RuleFor(x => x.NuCPF)
                //.Length(11).WithMessage(Traducao.Usuario.ResourceUsuario.Usuario_msg_required_maxlength_Usuario)
                .SetValidator(new CPFValidator()).WithMessage(Traducao.Usuario.ResourceUsuario.Usuario_msg_required_CPF_isvalid)
                .When(x => x.NuCPF != null);

        }

        protected void DeveTerPerfilValido()
        {
            RuleFor(x => x.IdPerfil)
                .NotEmpty().WithMessage(Traducao.Usuario.ResourceUsuario.Usuario_msg_required_Perfil);
        }

        protected void DevePossuirRelacionamentoComUsuarioClienteSite()
        {
            RuleFor(x => x.UsuarioClienteSites)
               .Must(x => x.Any(y => y.IdCliente > 0 && y.IdSite > 0))
               .When(x => x.Equals((int)PerfisAcesso.Suporte));

            RuleFor(x => x.UsuarioClienteSites)
                .Must(x => x.Any(y => y.IdCliente > 0 && y.IdSite > 0))
                .When(x => x.Equals((int)PerfisAcesso.Coordenador));

            RuleFor(x => x.UsuarioClienteSites)
                .Must(x => x.Any(y => y.IdCliente > 0 && y.IdSite > 0))
                .When(x => x.Equals((int)PerfisAcesso.Colaborador));
        }

        protected void DevePossuirCargo()
        {
            RuleFor(x => x.UsuarioCargoes)
                .Must(x => x.Count > 0)
                .When(x => x.IdPerfil.Equals((int)PerfisAcesso.Colaborador));
        }

        protected void ValidaLogo()
        {
            //RuleFor(x => x.FotoPerfilAux.Arquivo)
            //    .NotEmpty().WithMessage(Traducao.Usuario.ResourceUsuario.Usuario_msg_required_Foto)
            //    .When(x => x.FotoPerfilAux.ArquivoB64 != string.Empty &&
            //    x.FotoPerfilAux.Extensao != string.Empty &&
            //     x.FotoPerfilAux.Nome != string.Empty);

        }
    }
}
