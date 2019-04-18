using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationService.Servico
{
    public class SiteModuloAppServico : BaseServico<SiteFuncionalidade>, ISiteModuloAppServico
    {
        private readonly ISiteModuloRepositorio _siteModuloRepostirio;

        public SiteModuloAppServico(ISiteModuloRepositorio siteModuloRepostirio) : base(siteModuloRepostirio)
        {
            _siteModuloRepostirio = siteModuloRepostirio;
        }

        public List<SiteFuncionalidade> ObterPorSite(int idSite) =>
            _siteModuloRepostirio.Get(x => x.IdSite == idSite, o => o.OrderBy(s=>s.IdFuncionalidade)).ToList();
        

        public List<SiteFuncionalidade> RetirarReferenciaCircularDaLista(IEnumerable<SiteFuncionalidade> sitesFuncionalidades)
        {
            var siteFuncionalidades = new List<SiteFuncionalidade>();

            foreach (var siteFuncionalidade in sitesFuncionalidades)
            {
                siteFuncionalidades.Add(new SiteFuncionalidade
                {
                    IdSite = siteFuncionalidade.IdSite,
                    IdFuncionalidade = siteFuncionalidade.IdFuncionalidade
                });
            }

            return siteFuncionalidades;
        }
    }
}
