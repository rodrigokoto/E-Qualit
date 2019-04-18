
namespace Dominio.Entidade
{
    public class SiteAnexo
    {
        public int IdSiteAnexo { get; set; }
        public int IdSite { get; set; }
        public int IdAnexo { get; set; }

        public virtual Site Site { get; set; }
        public virtual Anexo Anexo { get; set; }
    }
}
