using Dominio.Entidade;

namespace Dominio.Validacao.Sites.View
{
    public class CriarSiteViewValidation: ValidaCampos<Site>
    {
        public CriarSiteViewValidation()
        {
            LogoObrigatorio();
            NomeObrigatorio();
            RazaoSocialObrigatorio();
            CNPJObrigatorio();
            DeveEstarAtivoNaCriacao();
            DeveTerModuloVinculado();
            DeveTerProcessoVinculado();
        }
    }
}
