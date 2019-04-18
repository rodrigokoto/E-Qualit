using FluentValidation;
using Dominio.Entidade;
namespace Dominio.Validacao.Sites.View
{
    public abstract class ValidaCampos<T> : AbstractValidator<T> where T : Site
    {
        protected void LogoObrigatorio()
        {
            RuleFor(x => x.SiteLogoAux.Arquivo)
              .NotEmpty().WithMessage(Traducao.Site.ResourceSite.Site_msg_required_Logo);

            RuleFor(x => x.SiteLogoAux.Nome)
                .NotEmpty().WithMessage(Traducao.Site.ResourceSite.Site_msg_required_LogoNome);

            RuleFor(x => x.SiteLogoAux.Extensao)
                .NotEmpty().WithMessage(Traducao.Site.ResourceSite.Site_msg_required_ExtensaoLogo);
        }
             

        protected void NomeObrigatorio()
        {
            RuleFor(x => x.NmFantasia)
                .NotEmpty().WithMessage(Traducao.Site.ResourceSite.Site_msg_required_Nome)
                .MaximumLength(60).WithMessage(Traducao.Site.ResourceSite.Site_msg_required_maxlength_Nome);
        }

        protected void RazaoSocialObrigatorio()
        {
            RuleFor(x => x.NmRazaoSocial)
                .NotEmpty().WithMessage(Traducao.Site.ResourceSite.Site_msg_required_Razao_Social)
                .MaximumLength(60).WithMessage(Traducao.Site.ResourceSite.Site_msg_required_maxlength_Razao_Social);
        }

        protected void CNPJObrigatorio()
        {
            RuleFor(x => x.NuCNPJ)
                .NotEmpty().WithMessage(Traducao.Site.ResourceSite.Site_msg_required_Cnpj)
                .Length(14).WithMessage(Traducao.Site.ResourceSite.Site_msg_required_maxlength_Cnpj)
                .SetValidator(new CNPJValidator()).WithMessage(Traducao.Site.ResourceSite.Site_msg_required_Cnpj_isvalid);
        }

        protected void DeveEstarAtivoNaCriacao()
        {
            RuleFor(x => x.FlAtivo)
               .Equal(true).WithMessage(Traducao.Site.ResourceSite.Site_msg_required_Ativo);
        }

        protected void IdSiteObrigatorio()
        {
            RuleFor(x => x.IdSite)
               .NotEmpty().WithMessage(Traducao.Site.ResourceSite.Site_msg_not_found_IdSite);
        }

        protected void DeveTerModuloVinculado()
        {
            RuleFor(x => x.SiteFuncionalidades)
               .Must(b => b.Count > 0).WithMessage(Traducao.Processo.ResourceProcesso.Processo_msg_min_lenght);
        }

        protected void DeveTerProcessoVinculado()
        {
            RuleFor(x => x.Processos)
               .Must(b => b.Count > 0).WithMessage(Traducao.Processo.ResourceProcesso.Processo_msg_min_lenght);
        }

    }
}
