using Dominio.Entidade;
using FluentValidation;
using System;

namespace Dominio.Validacao.Clientes.View
{
    public abstract class ValidaCampos<T> : AbstractValidator<T> where T : Cliente
    {
        private const bool ATIVO_NA_CRIACAO = true;

        protected void LogoObrigatorio()
        {
            RuleFor(x => x.ClienteLogoAux.Arquivo)
              .NotEmpty().WithMessage(Traducao.Cliente.ResourceCliente.Cliente_msg_required_Logo);

            RuleFor(x => x.ClienteLogoAux.Nome)
                .NotEmpty().WithMessage(Traducao.Cliente.ResourceCliente.Cliente_msg_required_LogoNome);

            RuleFor(x => x.ClienteLogoAux.Extensao)
                .NotEmpty().WithMessage(Traducao.Cliente.ResourceCliente.Cliente_msg_required_ExtensaoLogo);
        }

        protected void NomeEmpresa()
        {
            RuleFor(x => x.NmFantasia)
                .NotEmpty().WithMessage(Traducao.Cliente.ResourceCliente.Cliente_msg_required_Nome)
                .MaximumLength(60).WithMessage(Traducao.Cliente.ResourceCliente.Cliente_msg_required_maxlength_Nome);
        }

        protected void UrlAcesso()
        {
            RuleFor(x => x.NmUrlAcesso)
                .NotEmpty().WithMessage(Traducao.Cliente.ResourceCliente.Cliente_msg_required_Url)
                .MaximumLength(60).WithMessage(Traducao.Cliente.ResourceCliente.Cliente_msg_required_maxlength_Url);
        }

        protected void IdClienteObrigatorio()
        {
            RuleFor(x => x.IdCliente)
                .NotEmpty().WithMessage(Traducao.Cliente.ResourceCliente.Cliente_msg_not_found_IdCliente);
        }

        protected void NumeroDiasParaTrocasSenha()
        {
            RuleFor(x => x.NuDiasTrocaSenha)
                .NotEmpty().WithMessage(Traducao.Cliente.ResourceCliente.Cliente_msg_required_TrocaSenha);
        }

        protected void DataValidadeContrato()
        {
            RuleFor(x => x.DtValidadeContrato)
                .NotEmpty().WithMessage(Traducao.Cliente.ResourceCliente.Cliente_msg_required_TrocaSenha)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage(Traducao.Cliente.ResourceCliente.Cliente_msg_invalid_DataContrato_less_today);
        }

        protected void ClienteDeveEstaAtivo()
        {
            RuleFor(x => x.FlAtivo)
                .Equal(ATIVO_NA_CRIACAO).WithMessage(Traducao.Cliente.ResourceCliente.Cliente_msg_required_Ativo);
        }

        protected void UsuarioObrigatorioNaCriacao()
        {
            RuleFor(x => x.Usuario.NmCompleto)
                .NotEmpty().WithMessage(Traducao.Usuario.ResourceUsuario.Usuario_msg_required_Responsavel);

            RuleFor(x => x.Usuario.CdIdentificacao)
                .NotEmpty().WithMessage(Traducao.Usuario.ResourceUsuario.Usuario_msg_required_Email);
        }

        protected void SiteModuloObrigatorio()
        {
            RuleFor(x => x.Site.SiteFuncionalidades)
                .Must(x=>x.Count > 0).WithMessage(Traducao.Usuario.ResourceUsuario.Cliente_msg_required_SiteFuncionalidade);

           
        }
    }
}
