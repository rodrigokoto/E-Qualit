using Dominio.Entidade;

namespace Dominio.Validacao.Sites.View
{
    public class EditarViewValidation: ValidaCampos<Site>
    {
        public EditarViewValidation()
        {
            LogoObrigatorio();
            NomeObrigatorio();
            RazaoSocialObrigatorio();
            CNPJObrigatorio();
            IdSiteObrigatorio();
        }
    }
}
