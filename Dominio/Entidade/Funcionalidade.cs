using Dominio.Enumerado;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public partial class Funcionalidade
    {
        public Funcionalidade()
        {
            Funcoes = new List<Funcao>();
            SiteModulos = new List<SiteFuncionalidade>();
        }

        public int IdFuncionalidade { get; set; }
        public string Nome { get; set; }
        public string Tag { get; set; }
        public int NuOrdem { get; set; }
        public string CdFormulario { get; set; }
        public string Image
        {
            get
            {
                switch (IdFuncionalidade)
                {
                    case (int)Funcionalidades.Administrativo:
                        return "";
                    case (int)Funcionalidades.ControlDoc:
                        return "fa fa-suitcase fa-stack-1x fa-inverse";
                    case (int)Funcionalidades.NaoConformidade:
                        return "fa fa-ban fa-stack-1x fa-inverse";
                    case (int)Funcionalidades.AcaoCorretiva:
                        return "fa fa-eye fa-stack-1x fa-inverse";
                    case (int)Funcionalidades.Indicadores:
                        return "fa fa-thermometer-three-quarters fa-stack-1x fa-inverse";
                    case (int)Funcionalidades.Auditoria:
                        return "fa fa-users fa-stack-1x fa-inverse";
                    case (int)Funcionalidades.AnaliseCritica:
                        return "fa fa-eye fa-stack-1x fa-inverse";
                    case (int)Funcionalidades.Licencas:
                        return "fa fa-users fa-stack-1x fa-inverse";
                    case (int)Funcionalidades.Instrumentos:
                        return "fa fa-thermometer-three-quarters fa-stack-1x fa-inverse";
                    case (int)Funcionalidades.Fornecedores:
                        return "fa fa-users fa-stack-1x fa-inverse";
                    case (int)Funcionalidades.GestaoDeRiscos:
                        return "fa fa-users fa-stack-1x fa-inverse";
                    case (int)Funcionalidades.RecursosHumanos:
                        return "fa fa-users fa-stack-1x fa-inverse";
                    case (int)Funcionalidades.Docs:
                        return "fa fa-users fa-stack-1x fa-inverse";
                    case (int)Funcionalidades.Usuario:
                        return "fa fa-users fa-stack-1x fa-inverse";
                    default:
                        return "";
                }
            }
        }
        public string Url { get; set; }

        public bool Ativo { get; set; }

        public virtual ICollection<Funcao> Funcoes { get; set; }
        public virtual ICollection<SiteFuncionalidade> SiteModulos { get; set; }
        public virtual ICollection<Notificacao> Notificacoes { get; set; }
        public virtual ICollection<Relatorio> Relatorios { get; set; }

    }
}
