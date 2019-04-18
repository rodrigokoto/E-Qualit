namespace Dominio.Entidade
{
    public class SiteFuncionalidade
    {
        public int IdSiteFuncionalidade { get; set; }

        public int IdFuncionalidade { get; set; }
        public virtual Funcionalidade Funcionalidade { get; set; }

        public int IdSite { get; set; }
        public virtual Site Site { get; set; }
    }
}
