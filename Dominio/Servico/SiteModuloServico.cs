using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System.Collections.Generic;

namespace Dominio.Servico
{
    public class SiteModuloServico : ISiteModuloServico
    {
        private readonly ISiteModuloRepositorio _siteModuloRepostirio;

        public SiteModuloServico(ISiteModuloRepositorio siteModuloRepostirio) 
        {
            _siteModuloRepostirio = siteModuloRepostirio;
        }


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
