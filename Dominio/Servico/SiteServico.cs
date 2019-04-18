using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System.Collections.Generic;
using System;
using Dominio.Validacao.Sites;
using Dominio.Validacao.Sites.View;

namespace Dominio.Servico
{
    public class SiteServico : ISiteServico
    {
        private readonly ISiteRepositorio _siteRepositorio;

        public SiteServico(ISiteRepositorio siteRepositorio)
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
            catch (Exception)
            {
                return false;
            }
        }

        public bool Excluir(int id)
        {
            return _siteRepositorio.Excluir(id);
        }

        public void Valida(Site site, ref List<string> erros)
        {
            var validaCampos = site.IdSite == 0?
                new CriarSiteViewValidation().Validate(site):
                new EditarViewValidation().Validate(site);

            if (!validaCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validaCampos.Errors));
            }
            if (validaCampos.IsValid && site.IdSite >0)
            {
                site.ValidationResult = new AptoParaEdicaoValidation(_siteRepositorio)
                                            .Validate(site);

                if (!site.ValidationResult.IsValid)
                {
                    erros.AddRange(UtilsServico.PopularErros(site.ValidationResult));
                }
            }

        }
    }
}
