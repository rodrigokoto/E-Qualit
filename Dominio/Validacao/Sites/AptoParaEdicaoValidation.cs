using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Sites;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Sites
{
    public class AptoParaEdicaoValidation : Validator<Site>
    {
        public AptoParaEdicaoValidation(ISiteRepositorio _repositorio)
        {
            var siteDeveEstarCadastrado = new SiteDeveEstarCadastrado(_repositorio);

            base.Add(Traducao.Resource.MsgIdSite, new Rule<Site>(siteDeveEstarCadastrado, Traducao.Site.ResourceSite.Site_msg_not_found_IdSite));
        }

    }
}
