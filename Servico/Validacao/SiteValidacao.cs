using Entidade;
using Servico.Validacao.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Servico.Validacao
{
    public class SiteValidacao : IBaseValidacao<Site>
    {

        private ProcessoValidacao _validaProcesso = new ProcessoValidacao();
        private List<ValidationResult> erros = new List<ValidationResult>();

        public List<ValidationResult> SiteValido(List<Site> lvSite)
        {
            var erroOrigem = new List<string>();

            foreach (var site in lvSite)
            {
                ObjetoValido(site);
            }

            if (PossuiSite(lvSite) && !lvSite[0].IsValid())
            {
                erros.AddRange(lvSite[0].ModelErros);
            }

            if (!PossuiSite(lvSite))
            {
                erroOrigem.Add("");
                erros.Add(new ValidationResult("O cadastro não pode ser realizado sem um site cadastrado", erroOrigem));
            }

            if (!PossuiSiteProcesso(lvSite))
            {
                erroOrigem.Add("");
                erros.Add(new ValidationResult("Um site deve obrigatóriamente possuir um processo do tipo qualidade", erroOrigem));
            }

            return erros;
        }

        private bool PossuiSiteProcesso(List<Site> site)
        {
            if (PossuiSite(site) && (_validaProcesso.PossuiProcesso(site[0].LvProcesso) && site[0].LvProcesso.Where(w => w.FlQualidade == true).Any()))
            {
                return true;
            }
            return false;

        }

        public bool ListaEstaVazia(int totalSite)
        {
            if (totalSite == 0)
            {
                return true;
            }
            return false;
        }

        private bool PossuiSite(List<Site> site)
        {
            if (site == null || site.Count == 0)
            {
                return false;
            }
            return true;
        }

        public void ObjetoValido(Site site)
        {
            if (!site.IsValid())
            {
                erros.AddRange(site.ModelErros);
            }
        }
    }
}
