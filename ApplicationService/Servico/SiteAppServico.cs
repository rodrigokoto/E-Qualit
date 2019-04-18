using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Collections.Generic;

namespace ApplicationService.Servico
{
    public class SiteAppServico : BaseServico<Site>, ISiteAppServico
    {
        private readonly ISiteRepositorio _siteRepositorio;

        public SiteAppServico(ISiteRepositorio siteRepositorio) : base(siteRepositorio)
        {
            _siteRepositorio = siteRepositorio;
        }

        public IEnumerable<Site> ObterSitesPorCliente(int idCliente)
        {
            return _siteRepositorio.ListarSitesPorCliente(idCliente);
        }

        public bool AtivarInativar(int id)
        {
            try
            {
                var site = _siteRepositorio.GetById(id);

                site.FlAtivo = !site.FlAtivo;
                _siteRepositorio.Update(site);

                return true;    
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
